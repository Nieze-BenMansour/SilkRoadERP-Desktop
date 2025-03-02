using Domain.Entites;
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

namespace Ste.Fenetre.AvoirFinancierFour
{
    /// <summary>
    /// Interaction logic for Win_ManageAvoirFinancierFournisseur.xaml
    /// </summary>
    public partial class Win_ManageAvoirFinancierFournisseur : Window
    {
        FactureFournisseur currentFactureFournisseur;
        AvoirFinancierFournisseur currentAvoirFiFr;
        AvoirFinancierFournisseurService ser_avoirfinanFour = new AvoirFinancierFournisseurService();

        public Win_ManageAvoirFinancierFournisseur(FactureFournisseur FacFourRecived, AvoirFinancierFournisseur avoirReceved )
        {
            InitializeComponent();
            currentFactureFournisseur = FacFourRecived;
            currentAvoirFiFr = avoirReceved;
            numTextBox.Text = currentFactureFournisseur.Num.ToString();
            if(currentAvoirFiFr != null)
            {
                numSurPageTextBox.Text = currentAvoirFiFr.NumSurPage.ToString();
                dateDatePicker.SelectedDate = currentAvoirFiFr.date;
                descriptionTextBox.Text = currentAvoirFiFr.Description;
                tot_ttcTextBox.Text = currentAvoirFiFr.tot_ttc.ToString();
                
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
 
        }

        private void ValiderBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentAvoirFiFr == null)
                {
                    currentAvoirFiFr = new AvoirFinancierFournisseur();
                    currentAvoirFiFr.Num = int.Parse(numTextBox.Text);
                    currentAvoirFiFr.NumSurPage = int.Parse(numSurPageTextBox.Text);
                    currentAvoirFiFr.date = dateDatePicker.SelectedDate.Value;
                    currentAvoirFiFr.Description = descriptionTextBox.Text;
                    currentAvoirFiFr.tot_ttc = decimal.Parse(tot_ttcTextBox.Text);
                    ser_avoirfinanFour.AddAvoirFinancierFournisseur(currentAvoirFiFr);
                    MessageBox.Show("Avoir Financier ajouté !");
                }
                else
                {
                    currentAvoirFiFr.NumSurPage = int.Parse(numSurPageTextBox.Text);
                    currentAvoirFiFr.date = dateDatePicker.SelectedDate.Value;
                    currentAvoirFiFr.Description = descriptionTextBox.Text;
                    currentAvoirFiFr.tot_ttc = decimal.Parse(tot_ttcTextBox.Text);
                    ser_avoirfinanFour.editAvoirFinancierFournisseur(currentAvoirFiFr);
                    MessageBox.Show("Avoir Financier Modifié !");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Probleme");
            }
        }

        private void AnnullerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AnnulerSupprimerBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentAvoirFiFr != null)
                {
                    ser_avoirfinanFour.deleteAvoirFinancierFournisseur(currentAvoirFiFr);
                    currentAvoirFiFr = null;
                    numSurPageTextBox.Text = null;
                    dateDatePicker.SelectedDate = null;
                    descriptionTextBox.Text = null;
                    tot_ttcTextBox.Text = null;
                    MessageBox.Show("Avoir Financier Supprimé !");
                }
             
            }
            catch (Exception)
            {
                MessageBox.Show("Probleme");
            }
        }
        private void PreviewTextInput_nie(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;

            }
        }
    }
}
