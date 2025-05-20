using System.Windows;
using System.Windows.Input;

namespace TCPChat_Assync
{
    public partial class NickDialog : Window
    {
        public string Nickname { get; private set; }

        public NickDialog(string defaultNick = "Convidado")
        {
            InitializeComponent();
            txtNickname.Text = defaultNick;
            txtNickname.Focus();
            txtNickname.SelectAll();

            this.Closing += NickDialog_Closing;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNickname.Text))
            {
                MessageBox.Show("Apelido inválido. Digite um apelido válido.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNickname.Focus();
                return;
            }

            Nickname = txtNickname.Text.Trim();
            DialogResult = true;
        }

        private void NickDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.DialogResult != true)
            {
                Application.Current.Shutdown();
            }
        }

        // Adicionado para permitir arrastar a janela pela barra de título personalizada
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        // Adicionado para o botão de fechar personalizado
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}