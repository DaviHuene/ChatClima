using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace TCPChat_Assync
{
    public partial class ServerScreen : Window
    {
        private TcpListener listener;
        private List<TcpClient> clientesConectados = new List<TcpClient>();

        public ServerScreen()
        {
            InitializeComponent();
        }
        private void FecharJanela_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void btnConectar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listener == null)
                {
                    int porta = int.Parse(txtBox_PortServer.Text);
                    listener = new TcpListener(IPAddress.Any, porta);

                    listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    listener.Start();

                    txtBox_StatusMensagem.AppendText("Servidor aguardando conexão com cliente.\n");

                    _ = Task.Run(() => AguardarClientesAssync());
                }
                else
                {
                    CustomMessageBox.Show("O servidor já está em execução.\n");
                }
            }
            catch (SocketException)
            {
                CustomMessageBox.Show("A porta selecionada já está em uso.\n");
            }
            catch (Exception)
            {
                CustomMessageBox.Show("Não foi possível conectar-se.\nNenhum servidor encontrado com o endereço especificado");
            }
        }

        private async void btnDesconectar_Click(object sender, RoutedEventArgs e)
        {
            await DesconectarServidorAsync();
        }

        private async Task DesconectarServidorAsync()
        {
            try
            {
                if (listener != null)
                {
                    listener.Stop();
                    listener = null;

                    lock (clientesConectados)
                    {
                        foreach (var cliente in clientesConectados)
                        {
                            try
                            {
                                cliente.GetStream().Close();
                                cliente.Close();
                            }
                            catch { }
                        }
                        clientesConectados.Clear();
                    }

                    txtBox_StatusMensagem.AppendText("Servidor desconectado.\n");

                    await Task.Delay(500); // garante liberação da porta
                }
                else
                {
                    CustomMessageBox.Show("Servidor não está ativo.\n");
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Erro ao parar o servidor: " + ex.Message);
            }
        }

        private async Task ReceberMensagensAsync(TcpClient cliente)
        {
            try
            {
                using var reader = new StreamReader(cliente.GetStream());
                while (true)
                {
                    string mensagem = await reader.ReadLineAsync();
                    if (mensagem == null)
                        break;

                    Dispatcher.Invoke(() =>
                    {
                        txtBox_StatusMensagem.AppendText(mensagem + "\n");
                    });

                    EnviarParaTodos(mensagem);
                }
            }
            catch { }
            finally
            {
                lock (clientesConectados)
                {
                    clientesConectados.Remove(cliente);
                }
                cliente.Close();

                Dispatcher.Invoke(() =>
                {
                    txtBox_StatusMensagem.AppendText("Cliente desconectado.\n");
                });
            }
        }

        private async Task AguardarClientesAssync()
        {
            while (listener != null)
            {
                TcpClient novoCliente = null;
                try
                {
                    novoCliente = await listener.AcceptTcpClientAsync();
                }
                catch
                {
                    break; // Listener foi parado
                }

                if (novoCliente == null)
                    break;

                lock (clientesConectados)
                {
                    clientesConectados.Add(novoCliente);
                }

                Dispatcher.Invoke(() =>
                {
                    txtBox_StatusMensagem.AppendText("Cliente conectado.\n");
                });

                _ = Task.Run(() => ReceberMensagensAsync(novoCliente));
            }
        }

        private void EnviarParaTodos(string mensagem)
        {
            lock (clientesConectados)
            {
                foreach (var cliente in clientesConectados)
                {
                    try
                    {
                        var writer = new StreamWriter(cliente.GetStream()) { AutoFlush = true };
                        writer.WriteLine(mensagem);
                    }
                    catch { }
                }
            }
        }
 private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        private async void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            await DesconectarServidorAsync();

            var loginWindow = new LoginScreen();
            loginWindow.Show();

            this.Close();
        }
        
    }
}
