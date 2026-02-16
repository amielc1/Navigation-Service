using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation_Service
{
    public interface INavigationDevice
    {
        // every device must implement this event !
        event EventHandler<PositionArrivedEventArgs> onPositionArrived;
    }
}
