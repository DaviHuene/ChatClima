using System.Windows;
using System.Windows.Controls;

namespace TCPChat_Assync.Cadastro
{
    public partial class CadastroScreen : Window
    {
        public CadastroScreen()
        {
            InitializeComponent();
        }

        private void FecharJanela_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            LoginScreen telaLogin = new LoginScreen();
            telaLogin.Show();
            this.Close();
        }

        private void btnConcluir_Click(object sender, RoutedEventArgs e)
        {
            var mongo = new TCPChat_Assync.Repository.MongoDB();

            string nomeCompleto = txtBox_NomeCompleto.Text.Trim();
            string nomeUsuario = txtBox_NomeUsuario.Text.Trim();
            string senha = txtBox_Senha.Password;

            // Verifica se há campos vazios
            if (string.IsNullOrWhiteSpace(nomeCompleto) ||
                string.IsNullOrWhiteSpace(nomeUsuario) ||
                string.IsNullOrWhiteSpace(senha))
            {
                CustomMessageBox.Show("Todos os campos devem ser preenchidos.", "Erro de Cadastro");
                return;
            }

            // Verifica se o nome de usuário já existe no banco
            if (mongo.UserExists(nomeUsuario))
            {
                CustomMessageBox.Show("Nome de usuário já está em uso. Escolha outro.", "Erro de Cadastro");
                return;
            }

            // Realiza o cadastro
            mongo.InsertUser(nomeCompleto, nomeUsuario, senha);
            CustomMessageBox.Show("Cadastro concluído com sucesso!", "Sucesso");

            // Redireciona para login
            LoginScreen telaLogin = new LoginScreen();
            telaLogin.Show();
            this.Close();
        }

        private void txtBox_NomeCompleto_TextChanged(object sender, TextChangedEventArgs e)
        {
            placeholder_NomeCompleto.Visibility = string.IsNullOrWhiteSpace(txtBox_NomeCompleto.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void txtBox_NomeUsuario_TextChanged(object sender, TextChangedEventArgs e)
        {
            placeholder_NomeUsuario.Visibility = string.IsNullOrWhiteSpace(txtBox_NomeUsuario.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void txtBox_Senha_PasswordChanged(object sender, RoutedEventArgs e)
        {
            placeholder_Senha.Visibility = string.IsNullOrWhiteSpace(txtBox_Senha.Password)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        // Método para mover a janela ao clicar e arrastar
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
