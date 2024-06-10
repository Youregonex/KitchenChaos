using System;

public interface IHasProgress
{
    public event EventHandler<OnProgressChengedEEventArgs> OnProgressChanged;
    public class OnProgressChengedEEventArgs : EventArgs
    {
        public float progressNormalized;
    }
}
