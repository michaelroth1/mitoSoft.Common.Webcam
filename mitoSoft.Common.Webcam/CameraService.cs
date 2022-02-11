using mitoSoft.Common.Webcam.Contracts;
using mitoSoft.Common.Webcam.Extensions;
using System.Runtime.Versioning;

namespace mitoSoft.Common.Webcam
{
    public class CameraService
    {
        public event EventHandler<CameraEventArgs>? OnImageStringRead;

        private bool _cancelTask = false;
        private readonly ICameraAdapter _cameraHelper = default!;
        private int _count = 0;

        public CameraService(ICameraAdapter cameraHelper)
        {
            _cameraHelper = cameraHelper;
            _cancelTask = false;
        }

        public string? ImageString { get; set; }

        [SupportedOSPlatform("windows")]
        public void StartStream()
        {
            _count++;
            _cancelTask = false;
            Task.Run(() => this.BackgroundWorker());
        }

        public void StopStream()
        {
            _count--;
            if (_count <= 0)
            {
                _cancelTask = true;
            }
        }

        [SupportedOSPlatform("windows")]
        private async void BackgroundWorker()
        {
            while (true)
            {
                ImageString = (await _cameraHelper.TryGetImage())?.ToBase64String();

                this.OnImageStringRead?.Invoke(this, new CameraEventArgs(ImageString));

                if (_cancelTask)
                {
                    return;
                }

                await Task.Delay(50);
            }
        }
    }
}