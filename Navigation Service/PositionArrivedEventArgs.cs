namespace Navigation_Service
{
    public class PositionArrivedEventArgs  : EventArgs
    {
        public  IMeasurement _position { get; private set; }

        public PositionArrivedEventArgs(IMeasurement position)
        {
            _position = position;
        }
    }
}
