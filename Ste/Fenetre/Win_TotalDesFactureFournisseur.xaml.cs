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
    /// Interaction logic for Win_TotalDesFactureFournisseur.xaml
    /// </summary>
    public partial class Win_TotalDesFactureFournisseur : Window
    {
        FactureFournisseurService ser_facFour = new FactureFournisseurService();
        SystemeService ser_systeme = new SystemeService();
        LigneBonDeRecepctionService ser_LigneBR = new LigneBonDeRecepctionService();
        BonDeReceptionService ser_bonDeRec = new BonDeReceptionService();
        PrintEngine printEng = new PrintEngine();
        SommeEnLettres somme = new SommeEnLettres();
        List<FactureFournisseur> listeFactureFour = new List<FactureFournisseur>();
        public Win_TotalDesFactureFournisseur()
        {
            InitializeComponent();
            listeFactureFour = ser_facFour.getAllFactureFournisseur();
        }

        private void FermerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void actualiseBtn_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
        }

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChercherBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!dateDebutPicker.SelectedDate.Equals(null) && !dateFinPicker.SelectedDate.Equals(null) && dateFinPicker.SelectedDate >= dateDebutPicker.SelectedDate)
            {
                listeFactureFour.RemoveAll(t => t.date < dateDebutPicker.SelectedDate || t.date > dateFinPicker.SelectedDate);
            }
            if (!FournisseurTextBlock.Text.Equals("Fournisseur non selectionné"))
            {
                listeFactureFour.RemoveAll(t => t.id_fournisseur.ToString() != CodeFourTextBlock.Text);
            }
            factureFournisseurDataGrid.ItemsSource = null;
            factureFournisseurDataGrid.ItemsSource = listeFactureFour;
            TotalTextBlock.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", listeFactureFour.Sum(t => t.tot_ttc));
            TotalHorsTVATextBlock.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", listeFactureFour.Sum(t => t.tot_H_tva));
            TotalTVATextBlock.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", listeFactureFour.Sum(t => t.tot_tva));
        }

        private void Fournisseurbtn_Click(object sender, RoutedEventArgs e)
        {
            GetFournisseur win = new GetFournisseur();
            win.ShowDialog();
            CodeFourTextBlock.Text = win.fournisseurToSend.id.ToString();
            FournisseurTextBlock.Text = win.fournisseurToSend.nom;
        }
        private void clearFields()
        {
            dateFinPicker.SelectedDate = null;
            dateDebutPicker.SelectedDate = null;
            FournisseurTextBlock.Text = "Fournisseur non selectionné";
            CodeFourTextBlock.Text = "";
            listeFactureFour = ser_facFour.getAllFactureFournisseur();
            factureFournisseurDataGrid.ItemsSource = null;
            //  bonDeLivraisonDataGrid.ItemsSource = bon
            TotalTextBlock.Text = null;
            TotalHorsTVATextBlock.Text = null;
            TotalTVATextBlock.Text = null;

        }
    }
}
