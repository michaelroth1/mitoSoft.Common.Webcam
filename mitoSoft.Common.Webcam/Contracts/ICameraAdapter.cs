using System.Drawing;

namespace mitoSoft.Common.Webcam.Contracts
{
    public interface ICameraAdapter
    {
        Task<Image?> TryGetImage();

        Task<Image?> GetImage();
    }
}