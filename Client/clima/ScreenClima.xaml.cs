using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MongoDB.Bson;
using MongoDB.Driver;
using TCPChat_Assync.Repository;

namespace TCPChat_Assync.Clima
{
    public partial class ScreenClima : Window
    {
        private IMongoCollection<Climas> climas_collection;
        private List<Climas> _listaClimas;

        // Evento para enviar dados para o chat
        public event Action<string> OnDadosParaCompartilhar;

        public ScreenClima()
        {
            InitializeComponent();
            ConectarMongo();
            LoadCapitaisBrasil();

            ComboCidades.SelectionChanged += ComboCidades_SelectionChanged;
            ComboFiltros.SelectionChanged += ComboFiltros_SelectionChanged;
        }

        private void ConectarMongo()
        {
            try
            {
                var uri = "mongodb+srv://admin:123@cluster0.vgm081o.mongodb.net/AppChatClima?authSource=admin&retryWrites=true&w=majority&appName=Cluster0";
                var client = new MongoClient(uri);
                var database = client.GetDatabase("AppChatClima");
                climas_collection = database.GetCollection<Climas>("Climas");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro conexão MongoDB: " + ex.Message);
            }
        }

        private void LoadCapitaisBrasil()
        {
            try
            {
                var filtroBrasil = Builders<Climas>.Filter.Eq(c => c.Pais, "Brasil");
                _listaClimas = climas_collection.Find(filtroBrasil).ToList();

                var cidades = _listaClimas
                    .Select(c => new { Nome = c.Cidade, Id = c.Id.ToString() })
                    .Distinct()
                    .OrderBy(c => c.Nome)
                    .ToList();

                // Adiciona um item inicial para título no ComboBox
                var cidadesComTitulo = new List<object>
                {
                    new { Nome = "Selecione uma cidade", Id = "" }
                };
                cidadesComTitulo.AddRange(cidades);

                ComboCidades.ItemsSource = cidadesComTitulo;
                ComboCidades.DisplayMemberPath = "Nome";
                ComboCidades.SelectedValuePath = "Id";

                // Seleciona o item título inicialmente
                ComboCidades.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar capitais do Brasil: " + ex.Message);
            }
        }

        private void ComboCidades_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AtualizarResultado();
        }

        private void ComboFiltros_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AtualizarResultado();
        }

        private void AtualizarResultado()
        {
            if (ComboCidades.SelectedValue == null || ComboFiltros.SelectedItem == null)
                return;

            var idSelecionado = ComboCidades.SelectedValue.ToString();

            // Se for o item de título, não faz nada
            if (string.IsNullOrEmpty(idSelecionado))
            {
                TxtResultado.Text = "";
                return;
            }

            var filtroSelecionado = ((ComboBoxItem)ComboFiltros.SelectedItem)?.Content.ToString();

            Climas clima;
            try
            {
                var filtro = Builders<Climas>.Filter.Eq("_id", ObjectId.Parse(idSelecionado));
                clima = climas_collection.Find(filtro).FirstOrDefault();
            }
            catch
            {
                TxtResultado.Text = "Erro ao buscar cidade pelo ID.";
                return;
            }

            if (clima == null)
            {
                TxtResultado.Text = "Cidade não encontrada.";
                return;
            }

            string resultado = filtroSelecionado switch
            {
                "Tudo" => $"📍 {clima.Cidade}\n🌡️ Temperatura: {clima.Temperatura} °C\n" +
                          $"🌧️ Chuva: {clima.ChuvaMM} mm\n⚠️ Risco de Enchente: {clima.RiscoEnchente}",
                "Temperatura" => $"🌡️ Temperatura: {clima.Temperatura} °C",
                "Chuva (mm)" => $"🌧️ Chuva: {clima.ChuvaMM} mm",
                "Risco de Enchente" => $"⚠️ Risco de Enchente: {clima.RiscoEnchente}",
                _ => "Selecione um filtro válido."
            };

            TxtResultado.Text = resultado;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCompartilhar_Click(object sender, RoutedEventArgs e)
        {
            string dadosParaCompartilhar = TxtResultado.Text;

            // Dispara o evento para enviar dados para o chat
            OnDadosParaCompartilhar?.Invoke(dadosParaCompartilhar);

            // Fecha a janela após compartilhar
            this.Close();
        }

        // Permite mover a janela ao clicar e arrastar o Grid (já configurado no XAML)
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
