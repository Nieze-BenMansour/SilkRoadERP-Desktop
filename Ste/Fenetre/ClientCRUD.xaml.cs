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
using Domain.Models;
using Service;
using System.Data;

namespace Ste.Fenetre
{
    /// <summary>
    /// Interaction logic for ClientCRUD.xaml
    /// </summary>
    public partial class ClientCRUD : Window
    {
        ClientService service = new ClientService();
        Client client;
        int old_id = 0;
        public ClientCRUD()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            clientDataGrid.ItemsSource = service.getAllClient();
            ModifierBtn.Visibility = Visibility.Hidden;
            SupprimerBtn.Visibility = Visibility.Hidden;
        }

        private void AjouterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (nomTextBox.Text != null)
            {
                try
                {
                    client = new Client();
                    client.nom = nomTextBox.Text;
                    client.tel = telTextBox.Text;
                    client.adresse = adresseTextBox.Text;
                    client.matricule = matriculeTextBox.Text;
                    client.code = codeTextBox.Text;
                    client.code_cat = code_catTextBox.Text;
                    client.etb_sec = etb_secTextBox.Text;
                    client.mail = mailTextBox.Text;
                    service.AddClient(client);
                    clearFields();

                    int index = clientDataGrid.Items.Count - 1;
                    clientDataGrid.SelectedItem = clientDataGrid.Items[index];
                    clientDataGrid.ScrollIntoView(clientDataGrid.Items[index]);
                    DataGridRow dgrow = (DataGridRow)clientDataGrid.ItemContainerGenerator.ContainerFromItem(clientDataGrid.Items[index]);
                    dgrow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }

                catch (Exception)
                {
                    MessageBox.Show("Probleme");
                }
            }
        }

        private void Double_click_grid(object sender, MouseButtonEventArgs e)
        {
            client = new Client();
            client = (Client)clientDataGrid.SelectedItem;
            old_id = client.Id;  // old id client
            nomTextBox.Text = client.nom;
            telTextBox.Text = client.tel;
            adresseTextBox.Text = client.adresse;
            matriculeTextBox.Text = client.matricule;
            codeTextBox.Text = client.code;
            code_catTextBox.Text = client.code_cat;
            etb_secTextBox.Text = client.etb_sec;
            mailTextBox.Text = client.mail;
            AjouterBtn.Visibility = Visibility.Hidden;
            ModifierBtn.Visibility = Visibility.Visible;
            SupprimerBtn.Visibility = Visibility.Visible;
        }

        private void ModifierBtn_Click(object sender, RoutedEventArgs e)
        {
            if (nomTextBox.Text != null)
            {
                try
                {
                    int index = clientDataGrid.SelectedIndex;
                    client = new Client();
                    client = service.findClientByID(old_id);
                    client.Id = old_id;
                    client.nom = nomTextBox.Text;
                    client.tel = telTextBox.Text;
                    client.adresse = adresseTextBox.Text;
                    client.matricule = matriculeTextBox.Text;
                    client.code = codeTextBox.Text;
                    client.code_cat = code_catTextBox.Text;
                    client.etb_sec = etb_secTextBox.Text;
                    client.mail = mailTextBox.Text;
                    service.editClient(client);
                    clearFields();
                    old_id = 0;

                    clientDataGrid.SelectedItem = clientDataGrid.Items[index];
                    clientDataGrid.ScrollIntoView(clientDataGrid.Items[index]);
                    DataGridRow dgrow = (DataGridRow)clientDataGrid.ItemContainerGenerator.ContainerFromItem(clientDataGrid.Items[index]);
                    dgrow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                catch (Exception)
                {

                    throw;
                }
            }
           
        }

        private void SupprimerBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Êtes-vous sûr de vouloir supprimer le client ?", "Confirmation de suppression", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                try
                {
                    client = new Client();
                    client = (Client)clientDataGrid.SelectedItem;
                    service.deleteClient(client.Id);
                    clearFields();
                    
                }
                catch (Exception)
                {
                    MessageBox.Show("Probleme");
                }
            }
        }

        private void fermerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        void clearFields()
        {
            AjouterBtn.Visibility = Visibility.Visible;
            ModifierBtn.Visibility = Visibility.Hidden;
            SupprimerBtn.Visibility = Visibility.Hidden;
            nomTextBox.Text = null;
            telTextBox.Text = null;
            adresseTextBox.Text = null;
            matriculeTextBox.Text = null;
            codeTextBox.Text = null;
            code_catTextBox.Text = null;
            etb_secTextBox.Text = null;
            mailTextBox.Text = null;
            searchNomtextBox.Text = null;
            searchCodetextBox.Text = null;
            clientDataGrid.ItemsSource = service.getAllClient();

        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
            client = new Client();
        }

        private void searchCodetextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int n =0;
                if (int.TryParse(searchCodetextBox.Text, out n) == true)
                    clientDataGrid.ItemsSource = service.findClientByIdContains(int.Parse(searchCodetextBox.Text));
                else
                    clientDataGrid.ItemsSource = service.getAllClient();
            }
            catch (Exception)
            {

                
            }
        }

        private void searchNomtextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            clientDataGrid.ItemsSource = service.findClientByNom(searchNomtextBox.Text);

        }
    }
}
