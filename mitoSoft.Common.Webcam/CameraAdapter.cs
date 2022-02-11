using mitoSoft.Common.Webcam.Contracts;
using System.Drawing;
using System.Net;
using System.Runtime.Versioning;
using System.Text;

namespace mitoSoft.Common.Webcam
{
    public abstract class CameraAdapter : ICameraAdapter
    {
        private readonly NetworkCredential _credentials = default!;
        private readonly string _imageUrl;
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);

        public CameraAdapter(string imageUrl)
        {
            _imageUrl = imageUrl;
        }

        public CameraAdapter(string imageUrl, NetworkCredential credentials) : this(imageUrl)
        {
            _credentials = credentials;
        }

        public CameraAdapter(string imageUrl, NetworkCredential credentials, TimeSpan timeout) : this(imageUrl, credentials)
        {
            _timeout = timeout;
        }

        [SupportedOSPlatform("windows")]
        public async Task<Image?> TryGetImage()
        {
            try
            {
                return await this.GetImage();
            }
            catch
            {
                return default;
            }
        }

        [SupportedOSPlatform("windows")]
        public async Task<Image?> GetImage()
        {
            try
            {
                using var client = new HttpClient()
                {
                    Timeout = _timeout,
                };

                if (_credentials != null)
                {
                    var byteArray = Encoding.ASCII.GetBytes($"{_credentials.UserName}:{_credentials.Password}");
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                }

                var response = await client.GetAsync(this._imageUrl);
                using var stream = new StreamReader(await response.Content.ReadAsStreamAsync());
                var image = Image.FromStream(stream.BaseStream);

                return image;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}