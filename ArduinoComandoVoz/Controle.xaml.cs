using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
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
    public sealed partial class Controle : Page
    {
        private ConexaoTCP tcp;
        public Controle()
        {
            this.InitializeComponent();

            Conexao db = ConexaoDB.GetConexao();
            if (db != null)
            {
                try
                {
                    tcp = new ConexaoTCP(this, db.ip, db.porta);
                    tcp.Connect();

                    textBlock.Text = "CONTROLE: " + db.ip;
                }catch(Exception e)
                {
                    Debug.WriteLine("ERRO: " + e.ToString());
                    textBlock.Text = "CONTROLE: SEM CONEXÃO!";
                }
            }
            else
            {
                textBlock.Text = "CONTROLE: SEM CONEXÃO!";
            }
        }

        public void receiveCallback(String message)
        {
            Debug.WriteLine("receiveCallback " + message);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (tcp != null)
                tcp.Close();
            Frame.Navigate(typeof(Opcoes));
        }

        private void enviarComando(int led, int valor)
        {
            if (tcp != null) {

                JsonObject jsonObject = new JsonObject();
                string envio = "";
                string texto = "";
                switch (led) {
                    case 1: texto = "led1"; break;
                    case 2: texto = "led2"; break;
                    case 3: texto = "led3"; break;
                    default:
                        texto = "led0";
                    break;
                }
                jsonObject.SetNamedValue(texto, JsonValue.CreateNumberValue(valor));
                envio = jsonObject.ToString();
                tcp.Send(envio);
            }
            else
            {
                Debug.WriteLine("SEM CONEXÃO TCP!!!");
            }
        }

        private async void button6_Click(object sender, RoutedEventArgs e)
        {
            //Cria instância de SpeechRecognizer
            var speechRecognizer = new Windows.Media.SpeechRecognition.SpeechRecognizer();
            //speechRecognizer.UIOptions.ShowConfirmation = false;


            //É possível criar dinâmicamente um array para reconhecimento
            //Palavras que serão aceitas
            string[] responses = { "Finish", "Led one on", "Led one off", "Led two on", "Led two off", "Led three on", "Led three off" };

            //Adiciona uma lista de restrição ao reconhecimento
            var listConstraint = new Windows.Media.SpeechRecognition.SpeechRecognitionListConstraint(responses);

            //@ = permite inserir caracteres como barra e aspas
            speechRecognizer.UIOptions.ExampleText = @"Fale 'Finish' para encerrar o reconhecimento";
            speechRecognizer.Constraints.Add(listConstraint);

            //verificar como limitar tempo de espera do recognizer!!!

            //Compila as restrições
            await speechRecognizer.CompileConstraintsAsync();

            //Inicia o reconhecimento
            Windows.Media.SpeechRecognition.SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeWithUIAsync();

            if (speechRecognitionResult.Text != "Finish")
            {
                JsonObject jsonObject = new JsonObject();

                switch (speechRecognitionResult.Text)
                {
                    case "Led one on": enviarComando(1, 255); break;
                    case "Led one off": enviarComando(1, 0); break;
                    case "Led two on": enviarComando(2, 255); break;
                    case "Led two off": enviarComando(2, 0); break;
                    case "Led three on": enviarComando(3, 255); break;
                    case "Led three off": enviarComando(3, 0); break;
                }
                
                //textBlock1.Text = speechRecognitionResult.Text;
                //var dialog = new Windows.UI.Popups.MessageDialog("Você falou: " + speechRecognitionResult.Text);
                //await dialog.ShowAsync();
            }
        }
        
        private async void sld_Changed(object sender, RangeBaseValueChangedEventArgs e)
        {
            Slider slider = (sender as Slider);
            if (slider != null)
            {
                if (slider.Equals(sldLed1))
                {
                    Debug.WriteLine("Slider 1: " + slider.Value);
                    enviarComando(1, (int)slider.Value);
                }
                else
                {
                    if (slider.Equals(sldLed2))
                    {
                        Debug.WriteLine("Slider 2: " + slider.Value);
                        enviarComando(2, (int)slider.Value);
                    }
                    else
                    {
                        if (slider.Equals(sldLed3))
                        {
                            Debug.WriteLine("Slider 3: " + slider.Value);
                            enviarComando(3, (int)slider.Value);
                        }
                        else
                        {
                            var dialog = new Windows.UI.Popups.MessageDialog("Slider inválido");
                            await dialog.ShowAsync();
                        }
                    }
                }
            }
            else
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Slider não encontrado");
                await dialog.ShowAsync();
            }
        }


        private async void btn_On(object sender, RoutedEventArgs e)
        {
            Button button = (sender as Button);
            if (button != null)
            {
                if (button.Equals(btn1On))
                {
                    Debug.WriteLine("Botão 1 On");
                    enviarComando(1, 255);
                }
                else
                {
                    if (button.Equals(btn2On))
                    {
                        Debug.WriteLine("Botão 2 On");
                        enviarComando(2, 255);
                    }
                    else
                    {
                        if (button.Equals(btn3On))
                        {
                            Debug.WriteLine("Botão 3 On");
                            enviarComando(3, 255);
                        }
                        else
                        {
                            var dialog = new Windows.UI.Popups.MessageDialog("Botão inválido");
                            await dialog.ShowAsync();
                        }
                    }
                }
            }
            else
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Botão não encontrado");
                await dialog.ShowAsync();
            }
        }


        private async void btn_Off(object sender, RoutedEventArgs e)
        {
            Button button = (sender as Button);
            if (button != null)
            {
                if (button.Equals(btn1Off))
                {
                    Debug.WriteLine("Botão 1 Off");
                    enviarComando(1, 0);
                }
                else
                {
                    if (button.Equals(btn2Off))
                    {
                        Debug.WriteLine("Botão 2 Off");
                        enviarComando(2, 0);
                    }
                    else
                    {
                        if (button.Equals(btn3Off))
                        {
                            Debug.WriteLine("Botão 3 Off");
                            enviarComando(3, 0);
                        }
                        else
                        {
                            var dialog = new Windows.UI.Popups.MessageDialog("Botão inválido");
                            await dialog.ShowAsync();
                        }
                    }
                }
            }
            else
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Botão não encontrado");
                await dialog.ShowAsync();
            }
        }

    }
}
