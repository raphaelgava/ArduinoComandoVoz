using ArduinoComandoVoz.classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ArduinoComandoVoz
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Opcoes : Page
    {
        public Opcoes()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Usuario parameter = e.Parameter as Usuario;

            if (parameter != null)
            {
                if (parameter.Nome != "")
                {
                    textNome.Text = parameter.Nome;
                    Sintetizador.sintetizar("Welcome " + parameter.Nome);
                    /*
                    MediaElement media = new MediaElement();
                    var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
                    Windows.Media.SpeechSynthesis.SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync("Welcome " + parameter.Nome);
                    media.SetSource(stream, stream.ContentType);
                    media.Play();
                    */
                }
            }
        }

        private void imgAbout_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }

        private void imgLeds_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Controle));
        }

        private void imgLista_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Lista));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void btnConexao_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CadastroTCP));
        }
    }
}
