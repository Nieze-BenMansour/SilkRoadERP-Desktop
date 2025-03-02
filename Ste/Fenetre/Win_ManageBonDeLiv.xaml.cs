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
using Domain.Models;

namespace Ste.Fenetre
{
    /// <summary>
    /// Interaction logic for Win_ManageBonDeLiv.xaml
    /// </summary>
    public partial class Win_ManageBonDeLiv : Window
    {
        BonDeLivraisonService ser ;
        List<BonDeLivraison> BonDeLivraisons ;
        bool isSelectedFiltre;
        public Win_ManageBonDeLiv()
        {
            InitializeComponent();
            isSelectedFiltre = false;


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {      
            ser = new BonDeLivraisonService();
            BonDeLivraisons = new List<BonDeLivraison>();
            clearFields();
        }

        private void FermerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void GetSelectedBL(object sender, MouseButtonEventArgs e)
        {
            try
            {
                BonDeLivraison bon = (BonDeLivraison)bonDeLivraisonDataGrid.SelectedItem;
                WindowAjouterBonDeLivraison win = new WindowAjouterBonDeLivraison(null,bon);
                win.ShowDialog();
                if(isSelectedFiltre)
                ChercherBtn_Click(new object() ,new RoutedEventArgs());
                else
                    ToDayBtn_Click(new object(), new RoutedEventArgs());
            }
            catch (Exception)
            {
                
                MessageBox.Show("Probleme !");
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
        }

        private void ChercherBtn_Click(object sender, RoutedEventArgs e)
        {
            BonDeLivraisons = ser.getAllBonDeLivraison();

            if (!dateDebutPicker.SelectedDate.Equals(null) && !dateFinPicker.SelectedDate.Equals(null) && dateFinPicker.SelectedDate >= dateDebutPicker.SelectedDate)
            {
                BonDeLivraisons.RemoveAll(t => t.date < dateDebutPicker.SelectedDate || t.date > dateFinPicker.SelectedDate);
                isSelectedFiltre = true;
            }
            if(!ClientTextBlock.Text.Equals("Client non sélectionné"))
            {
                BonDeLivraisons.RemoveAll(t => t.clientId.ToString() != CodeClientTextBlock.Text);
                isSelectedFiltre = true;
            }

                if(radioFacturer.IsChecked == true)
                {
                BonDeLivraisons.RemoveAll(t => t.Num_Facture == null);
                isSelectedFiltre = true;
            }
                else if (radioPasEncore.IsChecked == true)
                {
                BonDeLivraisons.RemoveAll(t => t.Num_Facture != null);
                isSelectedFiltre = true;
            }           
            bonDeLivraisonDataGrid.ItemsSource = null;
            bonDeLivraisonDataGrid.ItemsSource = BonDeLivraisons;
            TotalTextBlock.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", BonDeLivraisons.Sum(t => t.net_payer));
            TotalHorsTVATextBlock.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", BonDeLivraisons.Sum(t => t.tot_H_tva));
            TotalTVATextBlock.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", BonDeLivraisons.Sum(t => t.tot_tva));
        }

        private void Clientbtn_Click(object sender, RoutedEventArgs e)
        {
            GetClient win = new GetClient();
            win.ShowDialog();
            CodeClientTextBlock.Text = win.clientToSend.Id.ToString();
            ClientTextBlock.Text = win.clientToSend.nom;
        }
        private void clearFields()
        {
            dateFinPicker.SelectedDate = null;
            dateDebutPicker.SelectedDate = null;
            ClientTextBlock.Text = "Client non sélectionné";
            CodeClientTextBlock.Text = "";
            radioTous.IsChecked = true;
            BonDeLivraisons = ser.getAllBonDeLivraison();
            bonDeLivraisonDataGrid.ItemsSource = null;
            TotalTextBlock.Text = null;
            TotalHorsTVATextBlock.Text = null;
            TotalTVATextBlock.Text = null;
            isSelectedFiltre = false;
        }

        private void ToDayBtn_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
            BonDeLivraisons.RemoveAll(t => t.date != DateTime.Today);
            bonDeLivraisonDataGrid.ItemsSource = null;
            bonDeLivraisonDataGrid.ItemsSource = BonDeLivraisons;
            TotalTextBlock.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", BonDeLivraisons.Sum(t => t.net_payer));
            TotalHorsTVATextBlock.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", BonDeLivraisons.Sum(t => t.tot_H_tva));
            TotalTVATextBlock.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", BonDeLivraisons.Sum(t => t.tot_tva));
        }
    }
}
