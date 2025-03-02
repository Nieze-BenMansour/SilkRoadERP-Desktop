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
    /// Interaction logic for FournisseurCRUD.xaml
    /// </summary>
    public partial class FournisseurCRUD : Window
    {
        FournisseurService ser_fournisseur = new FournisseurService();
        Fournisseur currentfournisseur;

        public FournisseurCRUD()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            fournisseurDataGrid.ItemsSource = ser_fournisseur.getAllFournisseur();
            currentfournisseur = null;
            ModifierBtn.Visibility = Visibility.Hidden;
            SupBtn.Visibility = Visibility.Hidden;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AjouterBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (nomTextBox.Text != null)
                {
                    Fournisseur four = new Fournisseur();
                    four.nom = nomTextBox.Text;
                    four.tel = telTextBox.Text;
                    four.fax = faxTextBox.Text;
                    four.code = codeTextBox.Text;
                    four.code_cat = code_catTextBox.Text;
                    four.matricule = matriculeTextBox.Text;
                    four.etb_sec = etb_secTextBox.Text;
                    four.mail = mailTextBox.Text;
                    four.mail_deux = mail_deuxTextBox.Text;
                    four.adresse = textBoxAdresse.Text;
                    four.constructeur = checkBoxConstructeur.IsChecked.Value;
                    ser_fournisseur.AddFournisseur(four);
                    clearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        void clearFields()
        {
            idTextBox.Text = null;
            nomTextBox.Text = null;
            telTextBox.Text = null;
            faxTextBox.Text = null;
            codeTextBox.Text = null;
            code_catTextBox.Text = null;
            etb_secTextBox.Text = null;
            matriculeTextBox.Text = null;
            mailTextBox.Text = null;
            mail_deuxTextBox.Text = null;
            textBox_searchNom.Text = null;
            textBoxAdresse.Text = null;
            checkBoxConstructeur.IsChecked = false;

            ModifierBtn.Visibility = Visibility.Hidden;
            SupBtn.Visibility = Visibility.Hidden;
            AjouterBtn.Visibility = Visibility.Visible;

            currentfournisseur = null;

            fournisseurDataGrid.ItemsSource = ser_fournisseur.getAllFournisseur();
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
        }

        private void GetSelectedFournisseur(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (nomTextBox.Text != null)
                {
                    Fournisseur four = (Fournisseur)fournisseurDataGrid.SelectedItem;
                    currentfournisseur = ser_fournisseur.findFournisseurByID(four.id);
                    idTextBox.Text = currentfournisseur.id.ToString();
                    nomTextBox.Text = currentfournisseur.nom;
                    telTextBox.Text = currentfournisseur.tel;
                    faxTextBox.Text = currentfournisseur.fax;
                    codeTextBox.Text = currentfournisseur.code;
                    code_catTextBox.Text = currentfournisseur.code_cat;
                    etb_secTextBox.Text = currentfournisseur.etb_sec;
                    matriculeTextBox.Text = currentfournisseur.matricule;
                    mailTextBox.Text = currentfournisseur.mail;
                    mail_deuxTextBox.Text = currentfournisseur.mail_deux;
                    textBoxAdresse.Text = currentfournisseur.adresse;
                    checkBoxConstructeur.IsChecked = currentfournisseur.constructeur;
                    AjouterBtn.Visibility = Visibility.Hidden;
                    ModifierBtn.Visibility = Visibility.Visible;
                    SupBtn.Visibility = Visibility.Visible; 
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void ModifierBtn_Click(object sender, RoutedEventArgs e)
        {
            currentfournisseur.nom = nomTextBox.Text;
            currentfournisseur.tel = telTextBox.Text;
            currentfournisseur.fax = faxTextBox.Text;
            currentfournisseur.code = codeTextBox.Text;
            currentfournisseur.code_cat = code_catTextBox.Text;
            currentfournisseur.matricule = matriculeTextBox.Text;
            currentfournisseur.etb_sec = etb_secTextBox.Text;
            currentfournisseur.mail = mailTextBox.Text;
            currentfournisseur.mail_deux = mail_deuxTextBox.Text;
            currentfournisseur.constructeur = checkBoxConstructeur.IsChecked.Value;
            currentfournisseur.adresse = textBoxAdresse.Text;
            ser_fournisseur.editFournisseur(currentfournisseur);
            clearFields();
        }
        private void PreviewTextInput_nie(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;

            }
        }

        private void textBox_searchNom_TextChanged(object sender, TextChangedEventArgs e)
        {
            fournisseurDataGrid.ItemsSource = ser_fournisseur.findFournisseurByNom(textBox_searchNom.Text);
        }

        private void SupBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Êtes-vous sûr de vouloir supprimer le fournisseur ?", "Confirmation de suppression", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                try
                {
                    ser_fournisseur.deleteFournisseur(currentfournisseur.id);
                    clearFields();
                    AjouterBtn.Visibility = Visibility.Visible;
                    ModifierBtn.Visibility = Visibility.Hidden;
                    SupBtn.Visibility = Visibility.Hidden;
                }
                catch (Exception)
                {
                    MessageBox.Show("Probleme");
                }
            }
        }
    }
}
