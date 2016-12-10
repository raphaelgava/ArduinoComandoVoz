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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using System.Diagnostics;
using System.Threading.Tasks;

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
        }
        private ConexaoTCP _socket;
        private void button_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (_socket == null)
            {
                //_socket = new ConexaoTCP("192.168.1.177", Convert.ToInt32("23"));
                //_socket.Connect();
                _socket = new ConexaoTCP();
                _socket.connect("192.168.1.177", "23", "Ola!!!");
            }
            //_socket.send("obcd\r\n");
            Debug.WriteLine("ESCREVEU!!!");
            Task<String> msg = _socket.read();
            Debug.WriteLine("result:: " + msg.Result);
            //_socket.send(msg.Result);
            //_socket.send("\r\naaaaaa\r\n");
            //MessageDialog msgbox = new MessageDialog(msg);
            //    msgbox.ShowAsync();

            //_socket.Send("OIiii!!!\n");
            */

            if (_socket != null)
            {
                _socket.Close();
                _socket.OnDataRecived -= socket_OnDataRecived;
                _socket = null;
            }
            _socket = new ConexaoTCP("192.168.1.177", Convert.ToInt32("23"));            
            _socket.OnDataRecived += socket_OnDataRecived;
            _socket.Connect();

            //_socket.Send("AQUI!");
        }

        private void socket_OnDataRecived(string data)
        {
            Debug.WriteLine("rec");
            Debug.WriteLine(data);
        }
        

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("send");
            _socket.Send("TESTE\r\n");
        }
    }
}
