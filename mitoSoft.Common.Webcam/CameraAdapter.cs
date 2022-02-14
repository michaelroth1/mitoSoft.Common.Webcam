using mitoSoft.Common.Webcam.Contracts;
using System.Drawing;
using System.Net;
using System.Runtime.Versioning;
using System.Text;

namespace mitoSoft.Common.Webcam
{
    public abstract class CameraAdapter : ICameraAdapter
    {
        private readonly string _imageUrl;

        public CameraAdapter(string imageUrl)
        {
            _imageUrl = imageUrl;
        }

        public CameraAdapter(string imageUrl, NetworkCredential credentials) : this(imageUrl)
        {
            Credentials = credentials;
        }

        public CameraAdapter(string imageUrl, NetworkCredential credentials, TimeSpan timeout) : this(imageUrl, credentials)
        {
            Timeout = timeout;
        }

        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(10);

        public NetworkCredential Credentials { get; } = default!;

        [SupportedOSPlatform("windows")]
        public async Task<Image?> TryGetImage()
        {
            try
            {
                return await GetImage();
            }
            catch
            {
                return default;
            }
        }

        [SupportedOSPlatform("windows")]
        public async Task<Image?> GetImage()
        {
            using var client = new HttpClient()
            {
                Timeout = Timeout,
            };

            if (Credentials != null)
            {
                var byteArray = Encoding.ASCII.GetBytes($"{Credentials.UserName}:{Credentials.Password}");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            }

            var response = await client.GetAsync(_imageUrl);
            using var stream = new StreamReader(await response.Content.ReadAsStreamAsync());
            var image = Image.FromStream(stream.BaseStream);

            return image;
        }
    }
}