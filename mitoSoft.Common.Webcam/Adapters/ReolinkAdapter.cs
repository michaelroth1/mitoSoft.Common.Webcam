using System.Net;

namespace mitoSoft.Common.Webcam.Adapters
{
    public class ReolinkAdapter : CameraAdapter
    {
        public ReolinkAdapter(string ip, NetworkCredential credentials)
            : this(ip, credentials, 800, 600)
        {
        }

        public ReolinkAdapter(string ip, NetworkCredential credentials, int width, int height)
            : base($"http://{ip}/cgi-bin/api.cgi?cmd=Snap&channel=0&user={credentials.UserName}&password={credentials.Password}&width={width}&height={height}")
        {
        }
    }
}