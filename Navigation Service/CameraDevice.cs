using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation_Service
{
    public class CameraDevice : INavigationDevice
    {
        public event EventHandler<PositionArrivedEventArgs> onPositionArrived;
        public CameraDevice() { }
    }
}
