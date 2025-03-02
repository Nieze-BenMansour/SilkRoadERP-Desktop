using Domain.Models;
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
using Service;
using Domain.Entites;

namespace Ste.Fenetre.FactureAvoirFournisseurFolder
{
    /// <summary>
    /// Interaction logic for Win_GetFactureAvoirFournisseur.xaml
    /// </summary>
    public partial class Win_GetFactureAvoirFournisseur : Window
    {
        factureAvoirFournisseurService ser_facAvoirFour;
        FactureAvoirFournisseur currentFacAvoirFour;
        FactureFournisseur currentFacFour;
        Fournisseur currentFournisseur;
        public Win_GetFactureAvoirFournisseur(Fournisseur fournisseurREceived , FactureAvoirFournisseur facRecived,FactureFournisseur facFourRecived)
        {
            InitializeComponent();
            ser_facAvoirFour = new factureAvoirFournisseurService();
            currentFournisseur = fournisseurREceived;
            currentFacAvoirFour = facRecived;
            currentFacFour = facFourRecived;
            factureAvoirFournisseurDataGrid.ItemsSource = ser_facAvoirFour.findFactureAvoirFournisseurByFournisseur(currentFournisseur);
            if(currentFacAvoirFour != null)
            {
                NumtextBlock.Text = currentFacAvoirFour.Num_FactureAvoirFourSurPAge.ToString();
                DatetextBlock_Copy.Text = currentFacAvoirFour.date.ToShortDateString();
                TotaltextBlock_Copy1.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFacAvoirFour.tot_ttc);  
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void AffecterFactAv(object sender, MouseButtonEventArgs e)
        {
            FactureAvoirFournisseur FacToAffect = (FactureAvoirFournisseur)factureAvoirFournisseurDataGrid.SelectedItem;
            if (ser_facAvoirFour.findFactureAvoirFournisseurByNumFactureFour(currentFacFour) == null && currentFacAvoirFour == null && FacToAffect != null)
            { 
       //     FacToAffect.Num_FactureFournisseur = currentFacFour.Num;
            ser_facAvoirFour.editFactureAvoirFournisseur(FacToAffect);
                if (ser_facAvoirFour.findFactureAvoirFournisseurByNumFactureFour(currentFacFour) != null)
                {
                    currentFacAvoirFour = FacToAffect;
                    NumtextBlock.Text = currentFacAvoirFour.Num_FactureAvoirFourSurPAge.ToString();
                    DatetextBlock_Copy.Text = currentFacAvoirFour.date.ToShortDateString();
                    TotaltextBlock_Copy1.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFacAvoirFour.tot_ttc);
                }
            }
        }

        private void AnnulerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AnnulerFactureAvoirbutton_Click(object sender, RoutedEventArgs e)
        {
            if(currentFacAvoirFour != null && ser_facAvoirFour.findFactureAvoirFournisseurByNumFactureFour(currentFacFour) != null)
            {
                currentFacAvoirFour.Num_FactureFournisseur = null;  
                ser_facAvoirFour.editFactureAvoirFournisseur(currentFacAvoirFour);
                if (ser_facAvoirFour.findFactureAvoirFournisseurByNumFactureFour(currentFacFour) == null)
                {
                    NumtextBlock.Text = null;
                    DatetextBlock_Copy.Text = null;
                    TotaltextBlock_Copy1.Text = null;
                    currentFacAvoirFour = null;
                }
            }
        }
    }
}
