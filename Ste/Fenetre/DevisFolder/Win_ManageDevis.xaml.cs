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
    /// Interaction logic for Win_ManageDevis.xaml
    /// </summary>
    public partial class Win_ManageDevis : Window
    {
        DeviService ser = new DeviService();
        List<Devi> devis = new List<Devi>();
        public Win_ManageDevis()
        {
            InitializeComponent();
            clearFields();
          
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          
            try
            {
                int nu = devisDataGrid.Items.Count - 1;
                devisDataGrid.ScrollIntoView(devisDataGrid.Items[nu]);
                DataGridRow dgrow = (DataGridRow)devisDataGrid.ItemContainerGenerator.ContainerFromItem(devisDataGrid.Items[nu]);
                dgrow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
            catch (Exception)
            {
                
            }
        }
        private void clearFields()
        {
            dateFinPicker.SelectedDate = null;
            dateDebutPicker.SelectedDate = null;
            ClientTextBlock.Text = "Client non sélectionné";
            CodeClientTextBlock.Text = "";
            devis = ser.getAllDevis();
            devisDataGrid.ItemsSource = null;
            devisDataGrid.ItemsSource = devis;
        }

        private void GetSelectedDevi(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Devi dev = (Devi)devisDataGrid.SelectedItem;
                WindowDevis win = new WindowDevis(dev);
                win.ShowDialog();
                ChercherBtn_Click(new object(), new RoutedEventArgs());
            }
            catch (Exception)
            {

                MessageBox.Show("Probleme !");
            }
        }

        private void FermerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Clientbtn_Click(object sender, RoutedEventArgs e)
        {
            GetClient win = new GetClient();
            win.ShowDialog();
            CodeClientTextBlock.Text = win.clientToSend.Id.ToString();
            ClientTextBlock.Text = win.clientToSend.nom;
        }

        private void ChercherBtn_Click(object sender, RoutedEventArgs e)
        {
            devis = ser.getAllDevis();

            if (!dateDebutPicker.SelectedDate.Equals(null) && !dateFinPicker.SelectedDate.Equals(null) && dateFinPicker.SelectedDate >= dateDebutPicker.SelectedDate)
            {
                devis.RemoveAll(t => t.date < dateDebutPicker.SelectedDate || t.date > dateFinPicker.SelectedDate);
            }
            if (!ClientTextBlock.Text.Equals("Client non sélectionné"))
            {
                devis.RemoveAll(t => t.id_client.ToString() != CodeClientTextBlock.Text);
            }


            devisDataGrid.ItemsSource = null;
            devisDataGrid.ItemsSource = devis;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
        }

        private void ToDayBtn_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
            devis.RemoveAll(t => t.date != DateTime.Today);
            devisDataGrid.ItemsSource = null;
            devisDataGrid.ItemsSource = devis;
        }
    }
}
