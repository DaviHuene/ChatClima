using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualBasic; // Necessário para InputBox
using System.Windows.Controls;
using TCPChat_Assync.Clima;

namespace TCPChat_Assync
{
    
    public partial class ClientScreen : Window
    {
        private TcpClient client;
        public StreamReader STR;
        public StreamWriter STW;
        private string lastSentMessage = ""; // Armazena a última mensagem enviada
        private string nickname = "";

        public ClientScreen()
        {
            InitializeComponent();

            // Solicita o nick assim que a janela abrir
            nickname = PromptNickname();
            txtBox_StatusMensagem.Text = $"BEM-VINDO, {nickname}!\n";
            txtBox_StatusMensagem.Text += "ENTRE NO SERVIDOR\n";
        }

        private string PromptNickname()
        {
            var dialog = new NickDialog();
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                return dialog.Nickname;
            }
            else
            {
                // Se o usuário cancelar, retorna "Convidado" ou pode tratar de outra forma
                return "Convidado";
            }
        }


        private async void btnConectar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (client != null && client.Connected)
                {
                    AppendStatusMessage("Você já está conectado.");
                    return;
                }

                if (!IPAddress.TryParse(txtBox_IPServer.Text, out _))
                {
                    MessageBox.Show("Formato de IP inválido.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(txtBox_PortServer.Text, out int port) || port < 1 || port > 65535)
                {
                    AppendStatusMessage("A porta deve estar entre 1 e 65535.");
                    return;
                }

                IPEndPoint IpEnd = new IPEndPoint(IPAddress.Parse(txtBox_IPServer.Text), port);
                AppendStatusMessage("Conectando ao servidor...");

                client = new TcpClient();
                await client.ConnectAsync(IpEnd.Address, IpEnd.Port);

                STR = new StreamReader(client.GetStream());
                STW = new StreamWriter(client.GetStream()) { AutoFlush = true };

                AppendStatusMessage("Conectado ao servidor!");
                AppendStatusMessage("Digite suas mensagens abaixo e clique em Enviar.");

                _ = Task.Run(() => ReceberMensagensAsync());
            }
            catch (Exception ex)
            {
                AppendStatusMessage($"Falha na conexão: {ex.Message}");
            }
        }

        private void btnDesconectar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (client == null || !client.Connected)
                {
                    MessageBox.Show("Você não está conectado.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                STR?.Close();
                STW?.Close();
                client?.Close();

                MessageBox.Show("Desconectado do servidor.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                AppendStatusMessage($"Erro ao desconectar: {ex.Message}");
            }
        }
private void btnClose_Click(object sender, RoutedEventArgs e)
{
    // Close the connection if it's open
    if (client != null && client.Connected)
    {
        btnDesconectar_Click(sender, e);
    }
    this.Close();
}

private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
{
    if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
    {
        this.DragMove();
    }
}
        private async Task ReceberMensagensAsync()
        {
            try
            {
                while (true)
                {
                    string mensagem = await STR.ReadLineAsync();
                    if (mensagem == null) break;

                    // Evita mostrar mensagem ecoada
                    if (mensagem != lastSentMessage)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            AppendStatusMessage(mensagem);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    AppendStatusMessage($"Erro na conexão: {ex.Message}");
                });
            }
            finally
            {
                Dispatcher.Invoke(() =>
                {
                    AppendStatusMessage("Conexão com o servidor foi encerrada.");
                });

                STR?.Close();
                STW?.Close();
                client?.Close();
                client = null;
            }
        }

        private void btnEnviar_Click(object sender, RoutedEventArgs e)
        {
            // Remove espaços e quebras de linha extras
            string rawMessage = txtBox_Mensagem.Text.Trim();

            // Substitui quebras de linha por espaço
            rawMessage = rawMessage.Replace("\r", "").Replace("\n", " ").Trim();

            if (string.IsNullOrWhiteSpace(rawMessage))
            {
                MessageBox.Show("Digite uma mensagem válida.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (client == null || !client.Connected)
            {
                MessageBox.Show("Você não está conectado a um servidor.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Envia no formato "nickname: mensagem"
                string TextToSend = $"{nickname}: {rawMessage}";
                lastSentMessage = TextToSend;
                STW.WriteLine(TextToSend);
                AppendStatusMessage(TextToSend);
                txtBox_Mensagem.Clear();
            }
            catch (Exception ex)
            {
                AppendStatusMessage($"Erro ao enviar mensagem: {ex.Message}");
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginScreen = new LoginScreen();
            loginScreen.Show();
            this.Close();
        }
private void btnAbrirClima_Click(object sender, RoutedEventArgs e)
{
    var telaClima = new ScreenClima();

    // Inscreve no evento para receber dados do clima e enviar ao chat
    telaClima.OnDadosParaCompartilhar += (dados) =>
    {
        // Envia a mensagem no formato: nickname: dados do clima
        if (client != null && client.Connected)
        {
            string mensagem = $"{nickname}: {dados}";
            lastSentMessage = mensagem;
            STW.WriteLine(mensagem);

            // Atualiza a interface do chat na thread da UI
            Dispatcher.Invoke(() =>
            {
                AppendStatusMessage(mensagem);
            });
        }
        else
        {
            Dispatcher.Invoke(() =>
            {
                AppendStatusMessage("Você não está conectado. Não foi possível enviar a mensagem do clima.");
            });
        }
    };

    telaClima.Show();
}


        private void AppendStatusMessage(string message)
        {
            txtBox_StatusMensagem.AppendText($"{message}\n");
            txtBox_StatusMensagem.ScrollToEnd();
        }
    }
}
