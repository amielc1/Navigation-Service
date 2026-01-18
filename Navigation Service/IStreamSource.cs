using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation_Service
{
    public interface IStreamSource
    {
        event EventHandler<RawDataEventArgs> RawDataReceived;
        Task Start();
        Task Stop();

    }
}
