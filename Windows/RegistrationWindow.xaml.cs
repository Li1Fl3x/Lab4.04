using Lab4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lab4.Windows
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private HttpClient client;
        private PriceList? PriceList;
        public RegistrationWindow(String token)
        {
            InitializeComponent();
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Task.Run(() => LoadPriceLists());
        }
        public RegistrationWindow(String token, Registration Registration)
        {
            InitializeComponent();
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Task.Run(() => LoadPriceLists());
            DateBroadcast.SelectedDate = Registration.DateBroadcast;
            CodeBroadcast.Text = Registration.CodeBroadcast;
            NameBroadcast.Text = Registration.NameBroadcast;
            Regularity.Text = Registration.Regularity;
            TimeOnBroadcast.Text = Registration.TimeOnBroadcast;
            CostBroadcast.Text = Registration.CostBroadcast;
            cbPriceList.SelectedItem = Registration.PriceList!.Id;
        }
        private async void LoadPriceLists()
        {
            List<PriceList>? list = await client.GetFromJsonAsync<List<PriceList>>("http://localhost:5079/api/PriceLists");
            Dispatcher.Invoke(() =>
            {
                cbPriceList.ItemsSource = list?.Select(p => p.Id);
            });
        }
        public DateTime? DateBroadcastProperty
        {
            get { return DateTime.Parse(DateBroadcast.Text); }
        }
        public string? CodeBroadcastProperty
        {
            get { return CodeBroadcast.Text; }
        }
        public string? NameBroadcastProperty
        {
            get { return NameBroadcast.Text; }
        }
        public string? RegularityProperty
        {
            get { return Regularity.Text; }
        }
        public string? TimeOnBroadcastProperty
        {
            get { return TimeOnBroadcast.Text; }
        }
        public string? CostBroadcastProperty
        {
            get { return CostBroadcast.Text; }
        }

        public async Task<int> getIdPriceList()
        {
            PriceList? PriceList = await client.GetFromJsonAsync<PriceList>("http://localhost:5079/api/PriceList/" + cbPriceList.Text);
            return PriceList!.Id;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
