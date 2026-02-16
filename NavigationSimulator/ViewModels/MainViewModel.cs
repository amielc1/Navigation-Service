using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;
using NavigationSimulator.Helpers;
using NavigationSimulator.Models;

namespace NavigationSimulator.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly DispatcherTimer _timer;
        private readonly PlaneModel _plane = new PlaneModel();
        private readonly UdpClient _udpClient = new UdpClient();
        private readonly IPEndPoint _gpsEndPoint = new IPEndPoint(IPAddress.Loopback, 11000);
        private readonly IPEndPoint _imuEndPoint = new IPEndPoint(IPAddress.Loopback, 11001);

        private bool _isRunning;
        private double _noisyLat;
        private double _noisyLon;

        public MainViewModel()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(100); // 10Hz
            _timer.Tick += OnTimerTick;

            StartCommand = new RelayCommand(_ => StartSimulation(), _ => !_isRunning);
            StopCommand = new RelayCommand(_ => StopSimulation(), _ => _isRunning);
        }

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        public double TruthLat => _plane.CurrentLat;
        public double TruthLon => _plane.CurrentLon;
        public double NoisyLat
        {
            get => _noisyLat;
            private set { _noisyLat = value; OnPropertyChanged(); }
        }
        public double NoisyLon
        {
            get => _noisyLon;
            private set { _noisyLon = value; OnPropertyChanged(); }
        }

        public double Altitude => _plane.Altitude;
        public double SpeedKph => _plane.SpeedKph;
        public double HeadingDeg => _plane.CurrentHeading * 180.0 / Math.PI;

        // For visualization scaling on Canvas
        public double CenterLat => _plane.CenterLat;
        public double CenterLon => _plane.CenterLon;

        private void StartSimulation()
        {
            _isRunning = true;
            _timer.Start();
            CommandManager.InvalidateRequerySuggested();
        }

        private void StopSimulation()
        {
            _isRunning = false;
            _timer.Stop();
            CommandManager.InvalidateRequerySuggested();
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            double dt = _timer.Interval.TotalSeconds;
            _plane.Update(dt);

            // 1. Generate Noisy GPS
            // +/- 5 meters noise. 5 meters in Lat/Lon is approx 0.000045 degrees
            double noiseStdDev = 5.0 / 111139.0; 
            NoisyLat = TruthLat + NoiseGenerator.NextGaussian(0, noiseStdDev);
            NoisyLon = TruthLon + NoiseGenerator.NextGaussian(0, noiseStdDev / Math.Cos(TruthLat * Math.PI / 180.0));

            // 2. Transmit GPS (NMEA)
            string gga = NmeaHelpers.CreateGga(DateTime.Now, NoisyLat, NoisyLon, Altitude);
            string vtg = NmeaHelpers.CreateVtg(_plane.CurrentHeading, _plane.SpeedKph / 3.6);
            
            SendData(gga, _gpsEndPoint);
            SendData(vtg, _gpsEndPoint);

            // 3. Generate Noisy IMU
            // Format: timestamp,accelX,accelY,accelZ,gyroX,gyroY,gyroZ
            double ts = (DateTime.Now - DateTime.UnixEpoch).TotalSeconds;
            double ax = _plane.AccelX + NoiseGenerator.NextGaussian(0, 0.1);
            double ay = _plane.AccelY + NoiseGenerator.NextGaussian(0, 0.1);
            double az = _plane.AccelZ + NoiseGenerator.NextGaussian(0, 0.1);
            double gx = _plane.GyroX + NoiseGenerator.NextGaussian(0, 0.01);
            double gy = _plane.GyroY + NoiseGenerator.NextGaussian(0, 0.01);
            double gz = _plane.GyroZ + NoiseGenerator.NextGaussian(0, 0.01);

            string imuCsv = $"{ts:F3},{ax:F4},{ay:F4},{az:F4},{gx:F4},{gy:F4},{gz:F4}";
            SendData(imuCsv, _imuEndPoint);

            // Update UI
            OnPropertyChanged(nameof(TruthLat));
            OnPropertyChanged(nameof(TruthLon));
            OnPropertyChanged(nameof(Altitude));
            OnPropertyChanged(nameof(SpeedKph));
            OnPropertyChanged(nameof(HeadingDeg));
        }

        private void SendData(string data, IPEndPoint endPoint)
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(data);
                _udpClient.Send(bytes, bytes.Length, endPoint);
            }
            catch { /* Ignore network errors in sim */ }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
