using System.Windows;

namespace TCPChat_Assync
{
   public partial class CustomMessageBox : Window
{
    public string WindowTitle { get; set; }

    public CustomMessageBox(string mensagem, string titulo = "Aviso")
    {
        InitializeComponent();
        txtMensagem.Text = mensagem;
        WindowTitle = titulo;
        DataContext = this;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    public static void Show(string mensagem, string titulo = "Aviso")
    {
        CustomMessageBox box = new CustomMessageBox(mensagem, titulo);
        box.ShowDialog();
    }
}

}
