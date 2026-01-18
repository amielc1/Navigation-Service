using NmeaParser.Messages;
using System;
using System.Threading.Tasks;

namespace Navigation_Service
{
    /// <summary>
    /// I can be a TCP \ UDP \ Serial Port and more.

    /// </summary>
    public interface INmeaSource
    {
        // the event passes a decoded and ready-to-use object.
        event EventHandler<NmeaMessage> MessageReceived;

    }
}