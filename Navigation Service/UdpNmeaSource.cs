using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NmeaParser.Messages;

namespace Navigation_Service
{
    public class UdpNmeaSource : INmeaSource
    {
        private UdpClient _udpClient;
        private int _port;

        public event EventHandler<NmeaMessage> MessageReceived;

        public UdpNmeaSource(int port)
        {
            _port = port;
            _udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, _port));
        }

        public async Task Start()
        {
            Console.WriteLine($"[UDP Source] Listening on port {_port}...");

            // loop listening for incoming datagram
            while (true)
            {
                try
                {
                    var result = await _udpClient.ReceiveAsync();
                    byte[] rawBytes = result.Buffer;
                    string sentence = Encoding.ASCII.GetString(rawBytes).Trim();

                    try
                    {
                        // parse NMEA sentence to NmeaMessage object.
                        var msg = NmeaMessage.Parse(sentence);
                        MessageReceived?.Invoke(this, msg);
                    }
                    catch (Exception parseEx)
                    {
                        Console.WriteLine($"[UDP Source] Parse Error: {parseEx.Message}");
                    }
                }
                catch (ObjectDisposedException)
                {
                    break; 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[UDP Source] Receive Error: {ex.Message}");
                }
            }
        }

        public Task Stop()
        {
            _udpClient?.Close();
            return Task.CompletedTask;
        }
    }
}