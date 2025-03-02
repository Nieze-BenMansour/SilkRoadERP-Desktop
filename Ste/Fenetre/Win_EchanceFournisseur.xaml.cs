//susing Domain.Entites;
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

namespace Ste.Fenetre
{
    /// <summary>
    /// Interaction logic for Win_EchanceFournisseur.xaml
    /// </summary>
    public partial class Win_EchanceFournisseur : Window
    {
        EcheanceFournisseurService ser_ech = new EcheanceFournisseurService();
        EcheanceDesFournisseur current_echenace;
        Fournisseur selected_fournisseur,currentFournisseur;
        List<EcheanceDesFournisseur> EcheanceDesFournisseurS = new List<EcheanceDesFournisseur>();

        public Win_EchanceFournisseur()
        {
            InitializeComponent();
            clearFields();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void clearFields()
        {
            EcheanceDesFournisseurS = null;
            EcheanceDesFournisseurS = ser_ech.getAllEcheanceFournisseur();
            echeanceFournisseursDataGrid.ItemsSource = EcheanceDesFournisseurS;
            dateDebutPicker.SelectedDate = null;
            dateFinPicker.SelectedDate = null;
            date_echeanceDatePicker.SelectedDate = null;
            montantTextBox.Text = null;
            num_chequeTextBox.Text = null;
            selectedFourCodeTextBlock.Text = null;
            selectedFourNomTextBlock.Text = "Fournisseur non selectionné";
            current_echenace = null;
            selected_fournisseur = null;
            currentFournisseur = null;
            CodeFourTextBlock.Text = null;
            FournisseurTextBlock.Text = "Fournisseur non selectionné";

            TotalTextBlock.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", EcheanceDesFournisseurS.Sum(t => t.montant));

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (current_echenace == null && selected_fournisseur != null)//ajout
                {
                    EcheanceDesFournisseur ech = new EcheanceDesFournisseur();

                    ech.dateEcheance = date_echeanceDatePicker.SelectedDate.Value;
                    ech.montant = decimal.Parse(montantTextBox.Text);
                    ech.numCheque = long.Parse(num_chequeTextBox.Text);
                    ech.fournisseur_id = selected_fournisseur.id;
                    ser_ech.AddEcheanceFournisseur(ech);
                    clearFields();

                    int index = 0;
                    int i = 0;
                    foreach (EcheanceDesFournisseur p in EcheanceDesFournisseurS)
                    {
                        if (p.id == ech.id) { index = i; }
                        i++;
                    }
                    echeanceFournisseursDataGrid.SelectedItem = echeanceFournisseursDataGrid.Items[index];
                    echeanceFournisseursDataGrid.ScrollIntoView(echeanceFournisseursDataGrid.Items[index]);
                    DataGridRow dgrow = (DataGridRow)echeanceFournisseursDataGrid.ItemContainerGenerator.ContainerFromItem(echeanceFournisseursDataGrid.Items[index]);
                    dgrow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

                }
                else if (current_echenace != null && selected_fournisseur != null)//modifier
                {
                    current_echenace.dateEcheance = date_echeanceDatePicker.SelectedDate.Value;
                    current_echenace.montant = decimal.Parse(montantTextBox.Text);
                    current_echenace.numCheque = long.Parse(num_chequeTextBox.Text);
                    current_echenace.fournisseur_id = selected_fournisseur.id;
                    ser_ech.editEcheanceFournisseur(current_echenace);
                    clearFields();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("prob");
            }
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
        }
        private void PreviewTextInput_nie(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;

            }
        }

        private void GetSelectedEcheance(object sender, MouseButtonEventArgs e)
        {
            try
            {
                current_echenace = (EcheanceDesFournisseur)echeanceFournisseursDataGrid.SelectedItem;
                num_chequeTextBox.Text = current_echenace.numCheque.ToString();
                montantTextBox.Text = current_echenace.montant.ToString();
                date_echeanceDatePicker.SelectedDate = current_echenace.dateEcheance;
                selected_fournisseur = current_echenace.fournisseur;
                selectedFourCodeTextBlock.Text = selected_fournisseur.id.ToString();
                selectedFourNomTextBlock.Text = selected_fournisseur.nom;
            }
            catch (Exception)
            {
                MessageBox.Show("prob");
            }
        }

        private void SuppBtn_Click(object sender, RoutedEventArgs e)
        {
            if(current_echenace != null && selected_fournisseur != null)
            { 
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Êtes-vous sûr de vouloir supprimer l'échéance ?", "Confirmation de suppression", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                try
                {
                    ser_ech.deleteCEcheanceFournisseur(current_echenace.id);
                    clearFields();

                }
                catch (Exception)
                {
                    MessageBox.Show("Probleme");
                }
            }
            }
        }

        private void RefreshBtn2_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
        }

        private void ChercherBtn_Click(object sender, RoutedEventArgs e)
        {
            EcheanceDesFournisseurS = ser_ech.getAllEcheanceFournisseur();
            if (!dateDebutPicker.SelectedDate.Equals(null) && !dateFinPicker.SelectedDate.Equals(null) && dateFinPicker.SelectedDate >= dateDebutPicker.SelectedDate)
            {
                EcheanceDesFournisseurS.RemoveAll(t => t.dateEcheance < dateDebutPicker.SelectedDate || t.dateEcheance > dateFinPicker.SelectedDate);
            }
            if (!FournisseurTextBlock.Text.Equals("Fournisseur non selectionné"))
            {
                EcheanceDesFournisseurS.RemoveAll(t => t.fournisseur_id.ToString() != CodeFourTextBlock.Text);
            }
            echeanceFournisseursDataGrid.ItemsSource = EcheanceDesFournisseurS;

            TotalTextBlock.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", EcheanceDesFournisseurS.Sum(t => t.montant));
        }

        private void Fournisseurbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetFournisseur win = new GetFournisseur();
                win.ShowDialog();
                CodeFourTextBlock.Text = win.fournisseurToSend.id.ToString();
                FournisseurTextBlock.Text = win.fournisseurToSend.nom;
                currentFournisseur = win.fournisseurToSend;

            }
            catch (Exception)
            {
                MessageBox.Show("prob");
            }
        }

        private void getSelectedFourBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetFournisseur win = new GetFournisseur();
                win.ShowDialog();
                selectedFourCodeTextBlock.Text = win.fournisseurToSend.id.ToString();
                selectedFourNomTextBlock.Text = win.fournisseurToSend.nom;
                selected_fournisseur = win.fournisseurToSend;
            }
            catch (Exception)
            {
                MessageBox.Show("prob");
            }
        }
    }
}
