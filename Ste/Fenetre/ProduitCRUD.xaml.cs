using Domain.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for ProduitCRUD.xaml
    /// </summary>
    public partial class ProduitCRUD : Window
    {
        Produit pro;
        ProduitService serviceProduit = new ProduitService();
        string old_ref = null;
        public ProduitCRUD()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            produitDataGrid.ItemsSource = serviceProduit.getAllProduit();
            modifierBtn.Visibility = Visibility.Hidden;
            supprimerBtn.Visibility = Visibility.Hidden;
        }

        private void DoubleClickDataDrid(object sender, MouseButtonEventArgs e)
        {
            try { 
            pro = new Produit();
            pro = (Produit)produitDataGrid.SelectedItem;
            old_ref = pro.refe;// old refe
            refeTextBox.Text = pro.refe;
            nomTextBox.Text = pro.nom;
            qteTextBox.Text = pro.qte.ToString();
            qteLimiteTextBox.Text = pro.qteLimite.ToString();
            remiseTextBox.Text = string.Format("{0:00.00}", pro.remise);
            remiseAchatTextBox.Text = string.Format("{0:00.00}", pro.remiseAchat);
            tVATextBox.Text = string.Format("{0:00.00}", pro.TVA);
            prixTextBox.Text = pro.prix.ToString();
            prixAchatTextBox.Text = pro.prixAchat.ToString();
            checkBoxVisible.IsChecked = pro.visibilite;

            ajouterBtn.Visibility = Visibility.Hidden;
            modifierBtn.Visibility = Visibility.Visible;
            supprimerBtn.Visibility = Visibility.Visible;
            }
            catch (NullReferenceException) { }

        }
        private void ajouterBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                pro = new Produit();
                pro.refe = refeTextBox.Text;
                pro.nom = nomTextBox.Text;
                pro.qte = int.Parse(qteTextBox.Text);
                pro.qteLimite = int.Parse(qteLimiteTextBox.Text);
                pro.remise = double.Parse (remiseTextBox.Text);
                pro.remiseAchat = double.Parse(remiseAchatTextBox.Text);
                pro.TVA = double.Parse(tVATextBox.Text);
                pro.prix = decimal.Parse(prixTextBox.Text);
                pro.prixAchat = decimal.Parse(prixAchatTextBox.Text);
                pro.visibilite = checkBoxVisible.IsChecked.Value;
                serviceProduit.AddProduit(pro);
                clearFields();


                int index = 0;
                int i = 0;
                    foreach(Produit p in serviceProduit.getAllProduit())
                {
                    if(p.refe == pro.refe) { index = i; }
                    i++;
                }
                        produitDataGrid.SelectedItem = produitDataGrid.Items[index];
                        produitDataGrid.ScrollIntoView(produitDataGrid.Items[index]);
                        DataGridRow dgrow = (DataGridRow)produitDataGrid.ItemContainerGenerator.ContainerFromItem(produitDataGrid.Items[index]);
                        dgrow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    
                

               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void clearFields()
        {
            refeTextBox.Text = null;
            nomTextBox.Text = null;
            qteTextBox.Text = null;
            qteLimiteTextBox.Text = null;
            remiseTextBox.Text = null;
            remiseAchatTextBox.Text = null;
            tVATextBox.Text = null;
            prixTextBox.Text = null;
            prixAchatTextBox.Text = null;
            textBox_search_desi.Text = null;
            textBox_search_ref.Text = null;
            checkBoxVisible.IsChecked = false;
            produitDataGrid.ItemsSource = serviceProduit.getAllProduit();
            ajouterBtn.Visibility = Visibility.Visible;
            modifierBtn.Visibility = Visibility.Hidden;
            supprimerBtn.Visibility = Visibility.Hidden;
        }

        private void fermerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void modifierBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = produitDataGrid.SelectedIndex;
                pro = new Produit();
                pro = serviceProduit.findProduit(old_ref);
                pro.refe = refeTextBox.Text;
                pro.nom = nomTextBox.Text;
                pro.qte = int.Parse(qteTextBox.Text);
                pro.qteLimite = int.Parse(qteLimiteTextBox.Text);
                pro.remise = double.Parse(remiseTextBox.Text);
                pro.remiseAchat = double.Parse(remiseAchatTextBox.Text);
                pro.TVA = float.Parse(tVATextBox.Text);
                pro.prix = decimal.Parse(prixTextBox.Text);
                pro.prixAchat = decimal.Parse(prixAchatTextBox.Text);
                pro.visibilite = checkBoxVisible.IsChecked.Value;
                serviceProduit.editProduit(pro);
                clearFields();
                old_ref = null;
                
               
               
                produitDataGrid.SelectedItem = produitDataGrid.Items[index];
                produitDataGrid.ScrollIntoView(produitDataGrid.Items[index]);
                DataGridRow dgrow = (DataGridRow)produitDataGrid.ItemContainerGenerator.ContainerFromItem(produitDataGrid.Items[index]);
                dgrow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void supprimerBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Êtes-vous sûr de vouloir supprimer l'article ?", "Confirmation de suppression", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                try
                {
                    pro = new Produit();
                    pro = (Produit)produitDataGrid.SelectedItem;
                    serviceProduit.deleteProduit(pro.refe);
                    clearFields();
                    ajouterBtn.Visibility = Visibility.Visible;
                    modifierBtn.Visibility = Visibility.Hidden;
                    supprimerBtn.Visibility = Visibility.Hidden;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void PreviewTextInput_nie(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;

            }
        }

        private void refreshBtn_Click_1(object sender, RoutedEventArgs e)
        {
            clearFields();
            pro = new Produit();
        }

        private void textBox_search_ref_TextChanged(object sender, TextChangedEventArgs e)
        {
            produitDataGrid.ItemsSource = serviceProduit.findProduitByRef(textBox_search_ref.Text);
        }

        private void textBox_search_desi_TextChanged(object sender, TextChangedEventArgs e)
        {
            produitDataGrid.ItemsSource = serviceProduit.findProduitByDesignation(textBox_search_desi.Text);
        }

       
    }
}
