using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationSimulator_WPF
{
    internal class InsSensor
    {
        internal class GpsSensor : NavigationSensorBase
        {
            public override string SensorName => "GPS Sensor";

            protected override LocationData GenerateNewReading()
            {
                // לוגיקה פשוטה לדוגמה ליצירת קריאת מיקום אקראית
               double x = 2.0;
                double y = 2.0;
                double z = 2.0;
                double roll = 2.0;
                double pitch = 2.0;
                double yaw = 2.0;
                  
                return new LocationData
                {
                    SensorName = SensorName,
                    Timestamp = DateTime.Now,
                    X = x,
                    Y = y,
                    Z = z,
                    Roll = roll,
                    Pitch = pitch,
                    Yaw = yaw
                };
            }
        }
    }
}
