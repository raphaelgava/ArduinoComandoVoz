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


using Windows.ApplicationModel.Core;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Data.Json;
using ArduinoComandoVoz.classes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ArduinoComandoVoz
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Sintetizador.sintetizar("Hello, insert your login and password");
        }

        private async void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Usuario usuario = UsuarioDB.GetUsuario(txtLogin.Text, pwdSenha.Password);

            if (usuario != null)
            {
                Frame.Navigate(typeof(Opcoes), usuario);
            }
            else
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Usuário/Senha Inválidos!!!");
                await dialog.ShowAsync();
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Cadastro));
        }
        /*
        private ConexaoTCP _socket;
        private async void button_Click(object sender, RoutedEventArgs e)
        {
            if (_socket != null)
            {
                _socket.Close();
                _socket.OnDataRecived -= socket_OnDataRecived;
                _socket = null;
            }
            _socket = new ConexaoTCP(this,"192.168.1.177", Convert.ToInt32("23"));
            _socket.OnDataRecived += socket_OnDataRecived;
            _socket.Connect();

            Task.Run(() => _socket.Read());
        }

        private void socket_OnDataRecived(string data)
        {
            Debug.WriteLine("rec");
            Debug.WriteLine(data);
        }
        

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("send");
            JsonObject jsonObject = new JsonObject();
            jsonObject.SetNamedValue("led1", JsonValue.CreateNumberValue(255));

            //JsonObject jsonObject = new JsonObject
            //{
            //   {"led1", JsonValue.CreateStringValue("255")}
            //};
            string teste = jsonObject.ToString();
            _socket.Send(teste);
        }

        public static void receiveCallback(String message)
        {
            Debug.WriteLine("receiveCallback " + message);
        }
        */

    }
}
