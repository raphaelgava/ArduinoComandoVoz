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

namespace ArduinoComandoVoz
{
    public sealed partial class Cadastro : Page
    {
        public Cadastro()
        {
            this.InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Usuario usuario = new Usuario();

            usuario.Login = txtLogin.Text;
            usuario.Senha = pwdSenha.Password;
            usuario.Nome = txtNome.Text;

            UsuarioDB.SalvarUsuario(usuario);

            Frame.Navigate(typeof(MainPage));
        }
    }
}
