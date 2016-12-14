using ArduinoComandoVoz.classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class Lista : Page
    {
        private bool flagSemaforo;
        public Lista()
        {
            this.InitializeComponent();
            flagSemaforo = false;
            lstUsuario.DataContext = UsuarioDB.ListaUsuarios();
        }

        private async void lstUsuario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (flagSemaforo == false)
            {
                flagSemaforo = true;
                if (lstUsuario.SelectedIndex > -1)
                {
                    try
                    {
                        Usuario user = UsuarioDB.GetUsuario(lstUsuario.SelectedIndex);

                        var dialog = new Windows.UI.Popups.MessageDialog("Nome do usuário é: " + user.Nome);
                        await dialog.ShowAsync();
                    }catch (Exception exception)
                    {
                        Debug.WriteLine("ERRO: " + exception.ToString());
                    }
                }
                flagSemaforo = false;
            }
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Opcoes));
        }
        
        private async void lstUsuario_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (flagSemaforo == false)
            {
                flagSemaforo = true;
                if (lstUsuario.SelectedIndex > -1)
                {
                    Usuario user = UsuarioDB.GetUsuario(lstUsuario.SelectedIndex);
                    var dialog = new Windows.UI.Popups.MessageDialog("Deseja deletar o usuário " + user.Nome + "?");
                    var yesCommand = new UICommand("Yes", cmd => { });
                    var noCommand = new UICommand("No", cmd => { });

                    dialog.Commands.Add(yesCommand);
                    dialog.Commands.Add(noCommand);

                    var command = await dialog.ShowAsync();

                    if (command == yesCommand)
                    {
                        // handle yes command                    
                        //lstUsuario.Items.Remove(user);
                        //int pos = lstUsuario.SelectedIndex;
                        //lstUsuario.Items.RemoveAt(pos);  
                        try
                        {
                            string nome = user.Nome;
                            UsuarioDB.DeletarUsuario(user);
                            lstUsuario.DataContext = UsuarioDB.ListaUsuarios();
                            Sintetizador.sintetizar(nome + " deleted!");
                        }
                        catch (Exception exception)
                        {
                            Debug.WriteLine("ERRO: " + exception.ToString());
                            var error = new Windows.UI.Popups.MessageDialog("Erro ao deletar!");
                            await error.ShowAsync();
                        }
                    }
                    flagSemaforo = false;
                }
            }
        }
    }
}
