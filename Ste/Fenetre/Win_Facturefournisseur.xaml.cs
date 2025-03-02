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
    /// Interaction logic for Win_Facturefournisseur.xaml
    /// </summary>
    public partial class Win_Facturefournisseur : Window
    {
        FactureFournisseur currentFactureFour ;
        Fournisseur ourFour;
        FactureFournisseurService ser_fac = new FactureFournisseurService();
        public Win_Facturefournisseur(FactureFournisseur facReceved,Fournisseur fourReceved)
        {
            InitializeComponent();
            if(fourReceved != null)
            {
            currentFactureFour = facReceved;
            ourFour = fourReceved;
            id_fournisseurTextBox.Text = ourFour.nom;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void validerBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentFactureFour != null)
                {
                    int x = currentFactureFour.Num;
                    currentFactureFour = ser_fac.findFactureFournisseurByNum(x);
                    currentFactureFour.date = dateDatePicker.SelectedDate.Value;
                    currentFactureFour.dateFacturationFournisseur = dateFacturationFournisseurDatePicker.SelectedDate.Value;
                    currentFactureFour.NumFactureFournisseur = long.Parse(numFactureFournisseurTextBox.Text);
                    ser_fac.editFactureFournisseur(currentFactureFour);
                    this.Close();
                }
                else
                {
                    FactureFournisseur fac = new FactureFournisseur();
                    fac.date = dateDatePicker.SelectedDate.Value;
                    fac.dateFacturationFournisseur = dateFacturationFournisseurDatePicker.SelectedDate.Value;
                    fac.NumFactureFournisseur = long.Parse(numFactureFournisseurTextBox.Text);
                    fac.id_fournisseur = ourFour.id;
                    fac.paye = false;
                    ser_fac.AddFactureFournisseur(fac);
                    this.Close();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Probleme !");
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
