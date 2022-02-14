using System.Net;
using System.Text;

namespace mitoSoft.Common.Webcam.Adapters
{
    /*
    Eine Anleitung zum Abstellen der Benutzereingaben findet sich auf folgender Website: https://support.microsoft.com/de-de/kb/834489
    Um das Standardverhalten (Benuter und Passwort werden über seperates Feld eingegeben) in Windows Explorer und Internet Explorer zu deaktivieren,
    erstellen Sie die DWORD-Werte iexplore.exe und explorer.exe in folgendem Registrierungsschlüssel, und setzen Sie die Werte auf 0
    
    HKEY_LOCAL_MACHINE\ Software \ Microsoft \ Internet Explorer\Main\FeatureControl\FEATURE_HTTP_USERNAME_PASSWORD_DISABLE

    Stream der Webcam kann mittes folgender URL aufgerufen werden: ("http://192.168.2.10:81/monitor.htm#")
    Holt nur ein einzelnes Bild: http://192.168.2.10:81/snapshot.cgi
    Befehl zum Absenken der Kamera: http://192.168.2.10:81/decoder_control.cgi?onestep=1&command=0
    Befehl zum Hochfahren der Kamera: http://192.168.2.10:81/decoder_control.cgi?onestep=1&command=2
    Befehl zum Rechtsschwenk der Kamera: http://192.168.2.10:81/decoder_control.cgi?onestep=1&command=4
    Befehl zum Linksschwenk der Kamera: http://192.168.2.10:81/decoder_control.cgi?onestep=1&command=6
    Befehl zum speichern der ersten Speicherposition: http://192.168.2.10:81/decoder_control.cgi?command=30
    Befehl zum ansteuern der ersten Speicherposition: http://192.168.2.10:81/decoder_control.cgi?command=31
    Befehl zum speichern der 2. Speicherposition: http://192.168.2.10:81/decoder_control.cgi?command=32
    Befehl zum ansteuern der 2. Speicherposition: http://192.168.2.10:81/decoder_control.cgi?command=33
    ...
    insgesamt gibt es mindestens 5 Speicherpositionen

    Verwendung in Synology Surveillance Station (https://camera-sdk.com/p_6665-how-to-connect-to-a-easyn-camera.html):
    http://[user]:[password]@192.168.2.10:81/videostream.cgi

    Verwendung als direkter Link:
    http://192.168.2.10:81/snapshot.cgi?user=[username]&pwd=[password]

    */
    public class EasyNAdapter : CameraAdapter
    {
        public EasyNAdapter(string ip, NetworkCredential credentials)
            : base($"http://{ip}:81/snapshot.cgi", credentials)
        {
            IP = ip;
        }

        public string IP { get; }

        public async void ToPosition(int positionId)
        {
            if (positionId < 1 || positionId > 5)
            {
                throw new ArgumentException("Invalid position identifier (not between and 5).", nameof(positionId));
            }
            await SendCommand(31 + (positionId - 1) * 2);
        }

        public async Task<bool> SavePos(int positionId)
        {
            if (positionId < 1 || positionId > 5)
            {
                throw new ArgumentException("Invalid position identifier.", nameof(positionId));
            }
            return await SendCommand(30 + (positionId - 1) * 2);
        }

        public async void MoveDown() => await SendCommand(0);

        public async void MoveUp() => await SendCommand(2);

        public async void MoveRight() => await SendCommand(4);

        public async void MoveLeft() => await SendCommand(6);

        public async Task<bool> SendCommand(int command)
        {
            using var client = new HttpClient()
            {
                Timeout = Timeout,
            };

            var url = $"http://{IP}:81/decoder_control.cgi?onestep=1&command={command}";
            var auth = Encoding.ASCII.GetBytes($"{Credentials.UserName}:{Credentials.Password}");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(auth));

            var response = await client.GetAsync(url);

            return response.IsSuccessStatusCode;
        }
    }
}