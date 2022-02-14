using System.Net;
using System.Text;

namespace mitoSoft.Common.Webcam.Adapters
{
    /*
        Snapshot:
        (HTTPGet)
        Url: http://192.168.2.17/cgi-bin/api.cgi?cmd=Snap&channel=0&width=640&height=480&user=[user]&password=[password]";
        
        Control the cam:
        (HTTPPost)        
        Url: http://192.168.2.17/cgi-bin/api.cgi?cmd=PtzCtrl&user=[user]&password=[password]";
        Body:        
            [{"cmd":"PtzCtrl","action":0,"param":{"channel":0,"op":"[command]","speed":32,"id":1}}]
        
            possible commands:
            -Left
            -Right
            -Up
            -Down
            -ToPos (use id to identify the position)
            -Stop
    */
    public class ReolinkAdapter : CameraAdapter
    {
        public ReolinkAdapter(string ip, NetworkCredential credentials)
            : this(ip, credentials, 800, 600)
        {
        }

        public ReolinkAdapter(string ip, NetworkCredential credentials, int width, int height)
            : base($"http://{ip}/cgi-bin/api.cgi?cmd=Snap&channel=0&user={credentials.UserName}&password={credentials.Password}&width={width}&height={height}", credentials)
        {
            IP = ip;
        }

        public string IP { get; }

        public async void ToPosition(int positionId) => await SendCommand("ToPos", positionId);

        public void MoveLeft() => Move("Left");

        public void MoveRight() => Move("Right");

        public void MoveUp() => Move("Up");

        public void MoveDown() => Move("Down");

        private async void Move(string command)
        {
            await Task.Run(async () =>
            {
                await SendCommand(command);

                await Task.Delay(500);

                await SendCommand("Stop");
            });
        }

        private async Task<bool> SendCommand(string command, int positionId = 1)
        {
            using var client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(5),
            };

            var url = $"http://{IP}/cgi-bin/api.cgi?cmd=PtzCtrl&user={Credentials.UserName}&password={Credentials.Password}";
            var body = new StringContent("[{\"cmd\":\"PtzCtrl\",\"action\":0,\"param\":{\"channel\":0,\"op\":\"" + command + "\",\"speed\":32,\"id\":" + positionId + "}}]", Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, body);

            return response.IsSuccessStatusCode;
        }
    }
}