using Domain.Models;
using Service;
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

namespace Ste.Fenetre
{
    /// <summary>
    /// Interaction logic for Win_GetFactureForBl.xaml
    /// </summary>
    public partial class Win_GetFactureForBl : Window
    {
        public Client currentClient;
        public Facture currentFacture;
        FactureService fac_ser = new FactureService();
        public Win_GetFactureForBl(Client recevedClient)
        {
            InitializeComponent();
            currentClient = recevedClient;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ClientLabel.Content = currentClient.nom;
            factureDataGrid.ItemsSource = fac_ser.findLast40FactureByClient(currentClient);
            if (factureDataGrid.Items.Count > 0)
            {
                var border = VisualTreeHelper.GetChild(factureDataGrid, 0) as Decorator;
                if (border != null)
                {
                    var scroll = border.Child as ScrollViewer;
                    if (scroll != null) scroll.ScrollToEnd();
                }
            }
        }

        private void AjouterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentClient != null)
            {
                Window_Facture_A_M win = new Window_Facture_A_M(null, currentClient);
                win.ShowDialog();
                factureDataGrid.ItemsSource = null;
                factureDataGrid.ItemsSource = fac_ser.findLast40FactureByClient(currentClient);
                if (factureDataGrid.Items.Count > 0)
                {
                    var border = VisualTreeHelper.GetChild(factureDataGrid, 0) as Decorator;
                    if (border != null)
                    {
                        var scroll = border.Child as ScrollViewer;
                        if (scroll != null) scroll.ScrollToEnd();
                    }
                }
            }
        }

        private void ModifierBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentClient != null && currentFacture != null)
            {
                Window_Facture_A_M win = new Window_Facture_A_M(currentFacture, currentClient);
                win.ShowDialog();
                factureDataGrid.ItemsSource = null;
                factureDataGrid.ItemsSource = fac_ser.findLast40FactureByClient(currentClient);
                if (factureDataGrid.Items.Count > 0)
                {
                    var border = VisualTreeHelper.GetChild(factureDataGrid, 0) as Decorator;
                    if (border != null)
                    {
                        var scroll = border.Child as ScrollViewer;
                        if (scroll != null) scroll.ScrollToEnd();
                    }
                }
            }
        }

        private void GetSelectedFacture(object sender, MouseButtonEventArgs e)
        {
            if (currentClient != null)
            {
                try
                {
                    currentFacture = (Facture)factureDataGrid.SelectedItem;
                    this.Close();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
