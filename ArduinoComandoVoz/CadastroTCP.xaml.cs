using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
    public sealed partial class CadastroTCP : Page
    {
        public CadastroTCP()
        {
            this.InitializeComponent();

            Conexao db = ConexaoDB.GetConexao();
            if (db != null)
            {
                txtHost.Text = db.ip;
                txtPort.Text = db.porta.ToString() ;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Opcoes));
        }
        
        private void imgLeds_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Controle));
        }

        private async void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if ((txtHost.Text != "") && (txtPort.Text != ""))
            {
                IPAddress ip;
                if (IPAddress.TryParse(txtHost.Text, out ip))
                {
                    try
                    {
                        Conexao con = new Conexao();
                        con.key = 1;
                        con.ip = txtHost.Text;
                        con.porta = int.Parse(txtPort.Text);
                        ConexaoDB.SalvarConexao(con);
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine("ERRO: " + exception.ToString());
                    }
                }
                else
                {
                    var dialog = new Windows.UI.Popups.MessageDialog("Preencha o Host corretamente!");
                    await dialog.ShowAsync();
                }
            }
            else
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Preencha o Host e a Porta");
                await dialog.ShowAsync();
            }
        }
    }
}
