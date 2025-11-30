using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Navigation_Service
{
    public  class UdpReceiver
    {
        private UdpClient _udpClient;
        private int _port;
        private IPAddress _IPAddress = IPAddress.Loopback;

        public UdpReceiver(int port = 11000)
        {
            _port = port;
            // socket bound to any IP address on the specified port
            _udpClient = new UdpClient(new IPEndPoint(_IPAddress, _port));
            Console.WriteLine($"[UDP Receiver] Bound successfully to {_IPAddress} on port {_port}.");
        }

        public async Task StartListening()
        {
            Console.WriteLine("[UDP Receiver] Listening loop started. Waiting for datagrams..."); 
            while (true)
            {
                // await : recuuve a one datagram !!
                // while loop : keep receiving datagrams
                UdpReceiveResult result = await _udpClient.ReceiveAsync();  // UdpReceiveResult is Struct
                byte[] receivedBytes = result.Buffer; 
                string receivedText = Encoding.UTF8.GetString(receivedBytes);
                Console.WriteLine($"Received: {receivedText} from {result.RemoteEndPoint}");
            }
        }

        public void StopListening()
        {
            _udpClient.Close();
        }

       


    }
}
