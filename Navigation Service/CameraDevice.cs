using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation_Service
{
    internal class CameraDevice : INavigationDevice
    {
        // constructor
        public CameraDevice() {
            System.Timers.Timer myTimer = new System.Timers.Timer(1000);
            myTimer.Elapsed += OnTimedEvent;
            myTimer.Enabled = true;
        }

        private void OnTimedEvent(object? sender, System.Timers.ElapsedEventArgs e)
        {
            SendPosition();
        }

        public event EventHandler<PositionArrivedEventArgs> onPositionArrived;

        // send Event of position arrived
        public void SendPosition()
        {
            Position pos = new Position
            {
                x = new Random().NextDouble() * 100,
                y = new Random().NextDouble() * 100,
                z = new Random().NextDouble() * 100
            };

            PositionArrivedEventArgs args = new PositionArrivedEventArgs(pos);

            onPositionArrived?.Invoke(this, args);
        }




    }
}
