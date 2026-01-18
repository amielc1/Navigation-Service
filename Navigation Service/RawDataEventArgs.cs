using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation_Service
{
     public class RawDataEventArgs
    {
        public byte[] Data { get; }
        public DateTime Timestamp { get; }
        public RawDataEventArgs(byte[] data)
        {
            Data = data;
            Timestamp = DateTime.UtcNow;
        }
    }
}
