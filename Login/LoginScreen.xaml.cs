using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TCPChat_Assync.Cadastro;
using TCPChat_Assync.Repository;

namespace TCPChat_Assync
{
    /// <summary>
    /// Lógica interna para Window1.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private void btnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            CadastroScreen telaCadastro = new CadastroScreen();
            telaCadastro.Show();
            this.Close();
        }

        private void btnEntrar_Click(object sender, RoutedEventArgs e)
        {
            var mongo = new TCPChat_Assync.Repository.MongoDB();
            string nomeUsuario = txtBox_Usuario.Text;
            string senha = txtBox_Senha.Password; // ✅ Correto para PasswordBox

            var login = mongo.LoginUser(nomeUsuario, senha);

            if (login == "admin")
            {
                ServerScreen telaServer = new ServerScreen();
                telaServer.Show();
                this.Close();
            }
            else if (login == "user")
            {
                ClientScreen telaClient = new ClientScreen();
                telaClient.Show();
                this.Close();
            }
            else
            {

            }
        }

private void txtBox_Usuario_TextChanged(object sender, TextChangedEventArgs e)
{
    if (placeholderUsuario != null)
    {
        placeholderUsuario.Visibility = string.IsNullOrWhiteSpace(txtBox_Usuario.Text)
            ? Visibility.Visible
            : Visibility.Hidden;
    }
}

private void txtBox_Senha_PasswordChanged(object sender, RoutedEventArgs e)
{
    if (placeholderSenha != null)
    {
        placeholderSenha.Visibility = string.IsNullOrWhiteSpace(txtBox_Senha.Password)
            ? Visibility.Visible
            : Visibility.Hidden;
    }
}
private void FecharJanela_Click(object sender, RoutedEventArgs e)
{
    this.Close();
}

private void Window_MouseDown(object sender, MouseButtonEventArgs e)
{
    if (e.ChangedButton == MouseButton.Left)
        this.DragMove();
}

    }
}
