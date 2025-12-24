using NmeaParser.Messages;
using System;
using System.Threading.Tasks;

namespace Navigation_Service
{
    public interface INmeaSource
    {
        // the event passes a decoded and ready-to-use object
        event EventHandler<NmeaMessage> MessageReceived;
       

        Task Start();
        Task Stop();
    }
}