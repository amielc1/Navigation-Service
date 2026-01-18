using System;
using NmeaParser.Messages;
using Serilog;

namespace Navigation_Service
{
    internal class GNSSDevice : INavigationDevice
    {
        public event EventHandler<PositionNMEArrivedEventArgs> onPositionArrived;

        private GNSSPosition _currentPosition = new GNSSPosition();
        private readonly Dictionary<Type, INmeaMapper> _mappers;
        private readonly ILogger _logger;
        public GNSSDevice(ILogger logger)
        {

            _logger = logger;
            _mappers = new Dictionary<Type, INmeaMapper>
            {
                { typeof(Gga), new GgaMapper() },
                { typeof(Vtg), new VtgMapper() },
                // { typeof(Rmc), new RmcMapper() },

            };
        }

        // function to connect from source
        public void ConnectSource(NmeaParser source)
        {
            source.sentceNMEArecived += OnNmeaMessageReceived;
        }

        private void OnNmeaMessageReceived(object sender, NmeaMessage message)
        {
            Type msgType = message.GetType();

            // There is no mapping for this message type..
            if (_mappers.TryGetValue(msgType, out var processor))
            {
                // map this messege.
                processor.Map(message, _currentPosition);

                // Raise an event or log the updated position.
                onPositionArrived?.Invoke(this, new PositionNMEArrivedEventArgs(_currentPosition));
                
                // for test OR logger.
                Console.WriteLine($"[GNSS] Pos Updated: Lat={_currentPosition.Latitude:F6}, Lon={_currentPosition.Longitude:F6} (Src: {msgType.Name})");
            }
            else
            {
                Console.WriteLine("There is no mapping for this message type.");
            }
        }
    }
}