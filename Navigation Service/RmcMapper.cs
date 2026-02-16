using System;
using NmeaParser.Messages;

namespace Navigation_Service
{
    public class RmcMapper : INmeaMapper
    {
        public void Map(NmeaMessage message, GNSSPosition target)
        {
            // safe cast to Rmc from NmeaMessage.
            if (message is Rmc rmc)
            {
                target.Latitude = rmc.Latitude;
                target.Longitude = rmc.Longitude;
                
                if (!double.IsNaN(rmc.Speed))
                {
                    target.SpeedMs = rmc.Speed * 0.514444; // Knots to m/s
                }
                
                if (!double.IsNaN(rmc.Course))
                {
                    target.CourseRad = rmc.Course * (Math.PI / 180.0);
                }

                target.IsValid = rmc.Active;
                target.Timestamp = rmc.FixTime.TimeOfDay.TotalSeconds;
            }
        }
    }
}
