using NmeaParser.Messages;
using System;
using System.Threading.Tasks;

namespace Navigation_Service
{
    /// <summary>
    /// I can be a TCP \ UDP \ Serial Port and more.
    /// 1. I am required to raise an event of receiving a message of type NmeaMessage, which is the parent class for all sentences of the NMEA protocol.
    /// 2. I am required to enable / disable message reception.
    /// </summary>
    public interface INmeaSource
    {
        // the event passes a decoded and ready-to-use object.
        event EventHandler<NmeaMessage> MessageReceived;

        Task Start();
        Task Stop();
    }
}