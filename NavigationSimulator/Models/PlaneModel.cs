using System;

namespace NavigationSimulator.Models
{
    public class PlaneModel
    {
        // Settings
        public double CenterLat { get; set; } = 45.0;
        public double CenterLon { get; set; } = 9.0;
        public double Radius { get; set; } = 1000.0; // meters
        public double SpeedKph { get; set; } = 250.0;
        public double Altitude { get; set; } = 500.0;

        // Current State (Truth)
        public double CurrentLat { get; private set; }
        public double CurrentLon { get; private set; }
        public double CurrentHeading { get; private set; } // Radians
        public double AccelX { get; private set; }
        public double AccelY { get; private set; }
        public double AccelZ { get; private set; }
        public double GyroX { get; private set; }
        public double GyroY { get; private set; }
        public double GyroZ { get; private set; }

        private double _totalTime = 0;

        public void Update(double deltaTimeSeconds)
        {
            _totalTime += deltaTimeSeconds;

            double speedMs = SpeedKph / 3.6;
            double omega = speedMs / Radius; // angular velocity

            double angle = omega * _totalTime;

            // X, Y in meters from center
            double x = Radius * Math.Cos(angle);
            double y = Radius * Math.Sin(angle);

            // Convert meters to Lat/Lon (approximate)
            double latRad = CenterLat * Math.PI / 180.0;
            CurrentLat = CenterLat + (y / 111139.0);
            CurrentLon = CenterLon + (x / (111139.0 * Math.Cos(latRad)));

            // Heading (tangent to circle)
            // if angle=0, (R, 0), velocity is (0, speedMs) -> Heading = 0 (North)
            // if angle=PI/2, (0, R), velocity is (-speedMs, 0) -> Heading = 270 (West)
            CurrentHeading = angle + Math.PI / 2.0;
            if (CurrentHeading > 2 * Math.PI) CurrentHeading -= 2 * Math.PI;

            // IMU data (Truth)
            // Centripetal acceleration
            double centripetalAccel = speedMs * speedMs / Radius;

            // Body frame: X forward, Y right, Z down
            // For clockwise circle (angle increasing), centripetal is towards center.
            // Vector to center is (-x, -y).
            // Velocity vector is (-y*omega, x*omega)
            // Body X is along velocity. Body Y is 90 deg right of Body X.
            // Body Y points away from center.
            
            AccelX = 0; // Constant speed
            AccelY = centripetalAccel; // lateral acceleration
            AccelZ = 9.81; // gravity (Z down)

            GyroX = 0;
            GyroY = 0;
            GyroZ = omega; // Rate of turn
        }
    }
}
