using System.Net;

namespace mitoSoft.Common.Webcam.Adapters
{
    /*
    Um Bilder der Kamera direkt vom integrierten Web-Server abzuholen, 
    stehen Ihnen die folgenden HTTP-Befehle zur Verfügung 
    (http://auge.physik.uni-mainz.de/help/help?cgi-image#faststream.jpg):
    
  - http://134.93.182.99/record/current.jpg
    Holt ein Bild der Kamera mit den aktuellen Bildeinstellungen. 
    Es können keine zusätzlichen Parameter übergeben werden. 
    Diese HTTP-API unterstützt die Verwendung einer sogenannten HTTP Keep-Alive-Verbindung, 
    um das Abholen mehrerer Bilder pro Sekunde zu optimieren. 
    Die besten Bildraten werden mit der faststream.jpg-API erreicht (siehe Parameter für faststream.jpg).
    
  - http://134.93.182.99/cgi-bin/image.jpg
    Holt ein Bild der Kamera, wobei Sie zusätzliche Parameter angeben können. 
    Beispielsweise liefert der folgende Aufruf ein Bild des rechten Kameraobjektivs 
    in der Größe 320x240 mit der Bildqualität 60%:
    http://134.93.182.99/cgi-bin/image.jpg?camera=right&size=320x240&quality=60
    Die gespeicherte Konfiguration wird nicht verändert.
    Fügen Sie hinter dem Befehl ?help hinzu, um die Hilfeseite dieses Befehls zu öffnen:    
    
  - http://134.93.182.99/cgi-bin/image.jpg?help
    Weitere Informationen finden Sie unter Parameter für image.jpg.
    
  - http://134.93.182.99/cgi-bin/faststream.jpg (Gastzugang)
    http://134.93.182.99/control/faststream.jpg (Benutzerzugang)
    Holt den Live-Stream der Kamera, wobei Sie zusätzliche Parameter angeben können. 
    Beispielsweise liefert der folgende Aufruf den Live-Stream der Kamera als M-JPEG in 
    einer dynamisch erzeugten HTML-Seite:
    http://134.93.182.99/cgi-bin/faststream.jpg?stream=full&html
    Weitere Informationen zu den Parametern sowie den Gast- und Benutzerzugang finden Sie 
    unter Parameter für faststream.jpg.
    Fügen Sie hinter dem Befehl ?help hinzu, um die Hilfeseite dieses Befehls zu öffnen:
    http://134.93.182.99/cgi-bin/faststream.jpg?help
    
  - http://134.93.182.99/control/event.jpg
    Ermöglicht den Zugriff auf gespeicherte Ereignisse der Kamera, 
    wobei Sie zusätzliche Parameter angeben können. Beispielsweise liefert der 
    folgende Aufruf das letzte Ereignisbild der Kamera:
    http://134.93.182.99/control/event.jpg?sequence=head
    Fügen Sie hinter dem Befehl ?help hinzu, um die Hilfeseite dieses Befehls zu öffnen: 
    http://134.93.182.99/control/event.jpg?help    
    */

    public class MobotixAdapter : CameraAdapter
    {
        public MobotixAdapter(string ip, NetworkCredential credentials) : base($"http://{ip}/record/current.jpg", credentials)
        {
        }
    }
}