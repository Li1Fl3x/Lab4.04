using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Principal;
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
using Lab4.Windows;

namespace Lab4
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private HttpClient httpClient;
        private MainWindow mainWindow;
        private string? token;
        public Main(Response response,MainWindow window)
        {
            InitializeComponent();
            this.mainWindow = window;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + response.access_token);
            token = response.access_token;
            Task.Run(() => Load());
        }
        private async Task Load()
        {
            List<Registration>? list = await httpClient.GetFromJsonAsync<List<Registration>>("http://localhost:5079/api/Registrations");
            foreach (Registration i in list!)
            {
                i.PriceList = await httpClient.GetFromJsonAsync<Models.PriceList>("http://localhost:5079/api/PriceList/" + i.PriceListId);
            }
            Dispatcher.Invoke(() =>
            {
                ListRegistrations.ItemsSource = null;
                ListRegistrations.Items.Clear();
                ListRegistrations.ItemsSource = list;
            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.mainWindow.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PriceListWindow PriceListWindow = new PriceListWindow(token!);
            PriceListWindow.ShowDialog();
        }
        //добавление
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RegistrationWindow RegistrationWindow = new RegistrationWindow(token!);
            if (RegistrationWindow.ShowDialog() == true)
            {
                Registration student = new Registration
                {
                    DateBroadcast = RegistrationWindow.DateBroadcastProperty,
                    CodeBroadcast = RegistrationWindow.CodeBroadcastProperty,
                    NameBroadcast = RegistrationWindow.NameBroadcastProperty,
                    Regularity = RegistrationWindow.RegularityProperty,
                    TimeOnBroadcast = RegistrationWindow.TimeOnBroadcastProperty,
                    CostBroadcast = RegistrationWindow.CostBroadcastProperty,
                    PriceListId = await RegistrationWindow.getIdPriceList()
                };
                JsonContent content = JsonContent.Create(student);
                using var response = await httpClient.PostAsync("http://localhost:5079/api/Registration", content);
                string responseText = await response.Content.ReadAsStringAsync();
                await Load();
            }
        }
        //изменение
        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Registration? st=ListRegistrations.SelectedItem as Registration;
            RegistrationWindow registrationWindow = new RegistrationWindow(token!,st!);
            if (registrationWindow.ShowDialog() == true)
            {
                st!.DateBroadcast = registrationWindow.DateBroadcastProperty;
                st!.CodeBroadcast = registrationWindow.CodeBroadcastProperty;
                st!.NameBroadcast = registrationWindow.NameBroadcastProperty;
                st!.Regularity = registrationWindow.RegularityProperty;
                st!.TimeOnBroadcast = registrationWindow.TimeOnBroadcastProperty;
                st!.CostBroadcast = registrationWindow.CostBroadcastProperty;
                st!.PriceListId = await registrationWindow.getIdPriceList();
                JsonContent content = JsonContent.Create(st);
                using var response = await httpClient.PutAsync("http://localhost:5079/api/Registration", content);
                string responseText = await response.Content.ReadAsStringAsync();
                await Load();
            }
        }
        //удаление
        private async void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Registration? st = ListRegistrations.SelectedItem as Registration;
            JsonContent content = JsonContent.Create(st);
            using var response = await httpClient.DeleteAsync("http://localhost:5079/api/Registration/" + st!.Id);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }
    }
}
