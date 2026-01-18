namespace Navigation_Service
{
    internal class PositionNMEArrivedEventArgs  : EventArgs
    {
        public  GNSSPosition PositionData { get; private set; }

        public PositionNMEArrivedEventArgs(GNSSPosition position)
        {
            PositionData = position;
        }
    }
}
