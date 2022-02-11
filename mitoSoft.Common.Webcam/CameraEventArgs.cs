namespace mitoSoft.Common.Webcam
{
    public class CameraEventArgs : EventArgs
    {
        public string? ImageString { get; }

        public CameraEventArgs(string? imageString)
        {
            this.ImageString = imageString;
        }
    }
}