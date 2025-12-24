using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using NmeaParser;
using NmeaParser.Messages;

namespace Navigation_Service
{
    public class TcpNmeaSource : INmeaSource
    {
        private StreamDevice _device;
        private TcpClient _tcpClient;
        private string _ip;
        private int _port;

        public event EventHandler<NmeaMessage> MessageReceived;

        public TcpNmeaSource(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        public async Task Start()
        {
            try
            {
                _tcpClient = new TcpClient();
                await _tcpClient.ConnectAsync(_ip, _port);
                Console.WriteLine($"[TCP Source] Connected to {_ip}:{_port}");

                // create the StreamDevice over the TCP stream
                _device = new StreamDevice(_tcpClient.GetStream());

                // subscribe to message received event
                _device.MessageReceived += (s, e) =>
                {
                    // sent the parsed message to subscribers
                    MessageReceived?.Invoke(this, e.Message);
                };
                await _device.OpenAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TCP Source] Error: {ex.Message}");
            }
        }

        public async Task Stop()
        {
            if (_device != null) await _device.CloseAsync();
            _tcpClient?.Close();
        }
    }
}