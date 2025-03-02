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
    /// Interaction logic for Win_ManageAvoirFournisseur.xaml
    /// </summary>
    public partial class Win_ManageAvoirFournisseur : Window
    {
        AvoirFournisseurService ser_AvoirFour = new AvoirFournisseurService();
        List<AvoirFournisseur> AvoirFournisseurs = new List<AvoirFournisseur>();
        LigneAvoirFournisseurService ser_ligneAvFr = new LigneAvoirFournisseurService();
        public Win_ManageAvoirFournisseur()
        {
            InitializeComponent();
        }

        private void Fournisseurbtn_Click(object sender, RoutedEventArgs e)
        {
            GetFournisseur win = new GetFournisseur();
            win.ShowDialog();
            CodeFourTextBlock.Text = win.fournisseurToSend.id.ToString();
            FournisseurTextBlock.Text = win.fournisseurToSend.nom;
        }
        private void ChercherBtn_Click(object sender, RoutedEventArgs e)
        {
            AvoirFournisseurs = ser_AvoirFour.getAllAvoirFournisseur();
            AvoirFournnisseurataGrid.ItemsSource = null;

            if (!dateDebutPicker.SelectedDate.Equals(null) && !dateFinPicker.SelectedDate.Equals(null) && dateFinPicker.SelectedDate >= dateDebutPicker.SelectedDate)
            {
                AvoirFournisseurs.RemoveAll(t => t.date < dateDebutPicker.SelectedDate || t.date > dateFinPicker.SelectedDate);
            }
            if (!FournisseurTextBlock.Text.Equals("Fournisseur non selectionné"))
            {
                AvoirFournisseurs.RemoveAll(t => t.fournisseurId.ToString() != CodeFourTextBlock.Text);
            }

            if (radioFacturer.IsChecked == true)
            {
                AvoirFournisseurs.RemoveAll(t => t.Num_FactureAvoirFournisseur == null);
            }
            else if (radioPasEncore.IsChecked == true)
            {
                AvoirFournisseurs.RemoveAll(t => t.Num_FactureAvoirFournisseur != null);
            }

            foreach (var item in AvoirFournisseurs)
            {
                List<LigneAvoirFourniseur> listeLignes = new List<LigneAvoirFourniseur>();
                listeLignes = ser_AvoirFour.findLigneAvoirFourniseurByNumAvFr(item);
                item.tot_H_tva = listeLignes.Sum(t => t.tot_HT);
                item.net_payer = listeLignes.Sum(t => t.tot_TTC);
                item.tot_tva = listeLignes.Sum(t => t.tot_TTC) - listeLignes.Sum(t => t.tot_HT);
            }
            AvoirFournnisseurataGrid.ItemsSource = AvoirFournisseurs;
            TotalTextBlock.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", AvoirFournisseurs.Sum(t => t.net_payer));
            TotalHorsTVATextBlock.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", AvoirFournisseurs.Sum(t => t.tot_H_tva));
            TotalTVATextBlock.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", AvoirFournisseurs.Sum(t => t.tot_tva));
        }

        private void GetSelectedAvFr(object sender, MouseButtonEventArgs e)
        {
            try
            {
                AvoirFournisseur avoir = (AvoirFournisseur)AvoirFournnisseurataGrid.SelectedItem;
                if (avoir != null)
                {
                    Win_AvoirFournisseur win = new Win_AvoirFournisseur(avoir);
                    win.ShowDialog();
                    ChercherBtn_Click(new object(), new RoutedEventArgs());
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Probleme !");
            }
        }
        private void clearFields()
        {
            dateFinPicker.SelectedDate = null;
            dateDebutPicker.SelectedDate = null;
            FournisseurTextBlock.Text = "Fournisseur non selectionné";
            CodeFourTextBlock.Text = "";
            radioTous.IsChecked = true;
            AvoirFournnisseurataGrid.ItemsSource = null;

        }

        private void FermerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
        }
    }
}
