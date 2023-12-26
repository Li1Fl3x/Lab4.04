using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
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
using Lab4.Models;

namespace Lab4.Windows
{
    /// <summary>
    /// Логика взаимодействия для PriceListWindow.xaml
    /// </summary>
    public partial class PriceListWindow : Window
    {
        private HttpClient client;
        private PriceList? PriceList;
        public PriceListWindow(string token)
        {
            InitializeComponent();
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Task.Run(() => Load());
        }
        private async Task Load()
        {
            List<PriceList>? list = await client.GetFromJsonAsync<List<PriceList>>("http://localhost:5079/api/PriceLists");
            Dispatcher.Invoke(() =>
            {
                ListPriceLists.ItemsSource = null;
                ListPriceLists.Items.Clear();
                ListPriceLists.ItemsSource = list;
            });
        }
        private async Task Save()
        {
            PriceList PriceList = new PriceList
            {
                CodeBroadcast = CodeBroadcast.Text,
                NameBroadcast = NameBroadcast.Text,
                PricePerMinute = PricePerMinute.Text
            };
            JsonContent content = JsonContent.Create(PriceList);
            using var response = await client.PostAsync("http://localhost:5079/api/PriceList", content);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Save();
        }

        private void ListPriceLists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PriceList = ListPriceLists.SelectedItem as PriceList;
            CodeBroadcast.Text = PriceList?.CodeBroadcast;
            NameBroadcast.Text = PriceList?.NameBroadcast;
            PricePerMinute.Text = PriceList?.PricePerMinute;
        }

        private async Task Edit()
        {
            PriceList!.CodeBroadcast = CodeBroadcast.Text;
            PriceList!.NameBroadcast = NameBroadcast.Text;
            PriceList!.PricePerMinute = PricePerMinute.Text;
            JsonContent content = JsonContent.Create(PriceList);
            using var response = await client.PutAsync("http://localhost:5079/api/PriceList", content);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }
        private async Task Delete()
        {
            using var response = await client.DeleteAsync("http://localhost:5079/api/PriceList/" + PriceList?.Id);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await Edit();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            await Delete();
        }
    }
}
