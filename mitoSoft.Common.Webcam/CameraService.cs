using mitoSoft.Common.Webcam.Contracts;
using mitoSoft.Common.Webcam.Extensions;
using System.Runtime.Versioning;

namespace mitoSoft.Common.Webcam
{
    public class CameraService
    {
        public event EventHandler<CameraEventArgs>? OnImageStringRead;

        private readonly ICameraAdapter _cameraHelper = default!;
        private bool _taskCompleted = true;
        private DateTime _lastStreamRequest;
        private bool _cancelTask = false;
        private int _consumerCount = 0;

        public CameraService(ICameraAdapter cameraHelper)
        {
            _cameraHelper = cameraHelper;
            _cancelTask = false;
        }

        public string? ImageString { get; set; }

        public TimeSpan RequestWaitingTime { get; set; } = TimeSpan.FromMilliseconds(200);

        public TimeSpan ConsumerResponsiveTime { get; set; } = TimeSpan.FromMinutes(10);

        [SupportedOSPlatform("windows")]
        public void StartStream()
        {
            this._consumerCount++;
            this._cancelTask = false;
            this._lastStreamRequest = DateTime.Now;

            if (_taskCompleted)
            {
                Task.Run(() => BackgroundWorker());
            }
        }

        public void StopStream()
        {
            this._consumerCount--;
            if (_consumerCount <= 0)
            {
                _cancelTask = true;
            }
        }

        [SupportedOSPlatform("windows")]
        private async void BackgroundWorker()
        {
            _taskCompleted = false;

            while (true)
            {
                ImageString = (await _cameraHelper.TryGetImage())?.ToBase64String();

                OnImageStringRead?.Invoke(this, new CameraEventArgs(ImageString));

                if (_cancelTask
                    || DateTime.Now - _lastStreamRequest > ConsumerResponsiveTime)
                {
                    break;
                }

                await Task.Delay((int)RequestWaitingTime.TotalMilliseconds);
            }

            _consumerCount = 0;
            _taskCompleted = true;
        }
    }
}