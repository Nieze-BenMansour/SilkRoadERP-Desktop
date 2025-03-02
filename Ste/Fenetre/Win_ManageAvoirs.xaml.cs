using Domain.Entites;
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
    /// Interaction logic for Win_ManageAvoirs.xaml
    /// </summary>
    public partial class Win_ManageAvoirs : Window
    {
        AvoirService ser = new AvoirService();
        List<Avoir> avoirs = new List<Avoir>();
        public Win_ManageAvoirs()
        {
            InitializeComponent();
            clearFields();
        }
        private void clearFields()
        {
            dateFinPicker.SelectedDate = null;
            dateDebutPicker.SelectedDate = null;
            ClientTextBlock.Text = "Client non sélectionné";
            CodeClientTextBlock.Text = "";
            avoirs = ser.getAllAvoirs();
            avoirsDataGrid.ItemsSource = null;
            avoirsDataGrid.ItemsSource = avoirs;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
 
        }

        private void ChercherBtn_Click(object sender, RoutedEventArgs e)
        {
            avoirs = ser.getAllAvoirs();

            if (!dateDebutPicker.SelectedDate.Equals(null) && !dateFinPicker.SelectedDate.Equals(null) && dateFinPicker.SelectedDate >= dateDebutPicker.SelectedDate)
            {
                avoirs.RemoveAll(t => t.date < dateDebutPicker.SelectedDate || t.date > dateFinPicker.SelectedDate);
            }
            if (!ClientTextBlock.Text.Equals("Client non sélectionné"))
            {
                avoirs.RemoveAll(t => t.clientId.ToString() != CodeClientTextBlock.Text);
            }


            avoirsDataGrid.ItemsSource = null;
            avoirsDataGrid.ItemsSource = avoirs;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
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

        private void avoirsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Avoir dev = (Avoir)avoirsDataGrid.SelectedItem;
                Win_Avoir win = new Win_Avoir(dev);
                win.ShowDialog();
                ChercherBtn_Click(new object(), new RoutedEventArgs());
            }
            catch (Exception)
            {

                MessageBox.Show("Probleme !");
            }
        }
    }
}
