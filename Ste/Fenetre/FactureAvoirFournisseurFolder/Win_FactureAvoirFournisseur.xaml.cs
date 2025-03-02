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

namespace Ste.Fenetre.FactureAvoirFournisseurFolder
{
    /// <summary>
    /// Interaction logic for Win_FactureAvoirFournisseur.xaml
    /// </summary>
    public partial class Win_FactureAvoirFournisseur : Window
    {
        FactureAvoirFournisseur currentFactureAvoirFour;
        Fournisseur ourFour;
        factureAvoirFournisseurService ser_fac = new factureAvoirFournisseurService();
        public Win_FactureAvoirFournisseur(FactureAvoirFournisseur facReceved, Fournisseur fourReceved)
        {
            InitializeComponent();

            currentFactureAvoirFour = facReceved;
            ourFour = fourReceved;
            id_fournisseurTextBox.Text = ourFour.nom;
            if(currentFactureAvoirFour != null && ourFour != null)
            {
                numTextBox.Text = currentFactureAvoirFour.Num.ToString();
                id_fournisseurTextBox.Text = ourFour.nom;
                numFactureAvoirFournisseurTextBox.Text = currentFactureAvoirFour.Num_FactureAvoirFourSurPAge.ToString();
                dateFacturationFournisseurDatePicker.SelectedDate = currentFactureAvoirFour.date;
            }
        }

        private void validerBtn_Click(object sender, RoutedEventArgs e)
        {
            
                if (currentFactureAvoirFour != null)
                {
                    int x = currentFactureAvoirFour.Num;
                    currentFactureAvoirFour = ser_fac.findFactureAvoirFournisseurByNum(x);
                    currentFactureAvoirFour.date = dateFacturationFournisseurDatePicker.SelectedDate.Value;
                    currentFactureAvoirFour.Num_FactureAvoirFourSurPAge = int.Parse(numFactureAvoirFournisseurTextBox.Text);
                    ser_fac.editFactureAvoirFournisseur(currentFactureAvoirFour);
                    this.Close();
                }
                else
                {
                    FactureAvoirFournisseur fac = new FactureAvoirFournisseur();
                    fac.date = dateFacturationFournisseurDatePicker.SelectedDate.Value;
                    fac.Num_FactureAvoirFourSurPAge = int.Parse(numFactureAvoirFournisseurTextBox.Text);
                    fac.id_fournisseur = ourFour.id;
                    fac.Num_FactureFournisseur = null;         
                    ser_fac.AddFactureAvoirFournisseur(fac);
                    this.Close();
                }
   
        }

        private void annulerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
