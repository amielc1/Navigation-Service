using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Serilog;

namespace Navigation_Service
{
    public class UdpSource : IStreamSource
    {
        private readonly UdpClient _udpClient;
        private readonly int _port;
        private readonly ILogger _logger;
        private bool _isRunning;

        public event EventHandler<RawDataEventArgs> RawDataReceived;

        public UdpSource(int port, ILogger logger)
        {
            _port = port;
            _logger = logger.ForContext<UdpSource>();
            _udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, _port));
            Start();

        }

        public async Task Start()
        {
            if (_isRunning) return;

            _isRunning = true;
            _logger.Information("[UDP Source] Starting listener on port {Port}", _port);

            while (_isRunning)
            {
                try
                {
                    var result = await _udpClient.ReceiveAsync();
                    byte[] rawBytes = result.Buffer;

                    _logger.Debug("[UDP Source] Received {ByteCount} bytes from {RemoteEndPoint}",
                        rawBytes.Length, result.RemoteEndPoint);

                    // rais.
                    RawDataReceived?.Invoke(this, new RawDataEventArgs(rawBytes));
                }
                catch (ObjectDisposedException)
                {
                    _logger.Information("[UDP Source] Socket closed, stopping.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "[UDP Source] Error receiving data: {Message}", ex.Message);
                }
            }
        }

        public Task Stop()
        {
            _isRunning = false;
            _udpClient?.Close();
            return Task.CompletedTask;
        }
    }
}