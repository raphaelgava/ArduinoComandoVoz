using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ArduinoComandoVoz.classes
{
    class Sintetizador
    {
        public async static void sintetizar(string texto)
        {
            MediaElement media = new MediaElement();
            var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
            Windows.Media.SpeechSynthesis.SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(texto);
            media.SetSource(stream, stream.ContentType);
            media.Play();
        }
    }
}
