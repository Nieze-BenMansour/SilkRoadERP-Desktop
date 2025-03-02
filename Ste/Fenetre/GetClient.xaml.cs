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
using System.Data;
using Service;
using Domain.Models;

namespace Ste
{
    /// <summary>
    /// Logique d'interaction pour GetClient.xaml
    /// </summary>
    public partial class GetClient : Window
    {
        public Client clientToSend = null;

        ClientService ser_client = new ClientService();
        public GetClient()
        {
            InitializeComponent();
            clientDataGrid.ItemsSource = ser_client.getAllClient();

        }

        private void Get(object sender, MouseButtonEventArgs e)
        {
            Client drv = (Client)clientDataGrid.SelectedItem;
            try {
                clientToSend = drv;
                this.Close();
            }
            catch (Exception)
            {
                // MessageBox.Show("-__-");
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = sender as TextBox;
            try {
                int n = 0;
                if (int.TryParse(t.Text, out n) == true)
                    clientDataGrid.ItemsSource = ser_client.findClientByIdContains(int.Parse(t.Text));
                else
                    clientDataGrid.ItemsSource = ser_client.getAllClient();
            }
            catch(Exception)
            { 
                clientDataGrid.ItemsSource = ser_client.getAllClient();
            }
        }

        private void SearchNom_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = sender as TextBox;
            clientDataGrid.ItemsSource = ser_client.findClientByNom(t.Text);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (clientToSend == null) e.Cancel = true;
            else e.Cancel = false;
        }
    }
}
