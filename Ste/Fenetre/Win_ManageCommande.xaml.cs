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
    /// Interaction logic for Win_ManageCommande.xaml
    /// </summary>
    public partial class Win_ManageCommande : Window
    {
        CommandeService ser_Commande = new CommandeService();
        List<Commande> Commandes = new List<Commande>();
        public Win_ManageCommande()
        {
            InitializeComponent();
            clearFields();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


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
            Commandes = ser_Commande.getAllCommande();

            if (!dateDebutPicker.SelectedDate.Equals(null) && !dateFinPicker.SelectedDate.Equals(null) && dateFinPicker.SelectedDate >= dateDebutPicker.SelectedDate)
            {
                Commandes.RemoveAll(t => t.date < dateDebutPicker.SelectedDate || t.date > dateFinPicker.SelectedDate);
            }
            if (!FournisseurTextBlock.Text.Equals("Fournisseur non selectionné"))
            {
                Commandes.RemoveAll(t => t.fournisseurId.ToString() != CodeFourTextBlock.Text);
            }

          
            commandesDataGrid.ItemsSource = null;
            commandesDataGrid.ItemsSource = Commandes;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
        }

        private void FermerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        void clearFields()
        {
            dateFinPicker.SelectedDate = null;
            dateDebutPicker.SelectedDate = null;
            FournisseurTextBlock.Text = "Fournisseur non selectionné";
            CodeFourTextBlock.Text = "";
            Commandes = ser_Commande.getAllCommande();
            commandesDataGrid.ItemsSource = null;
            commandesDataGrid.ItemsSource = Commandes;
        }

        private void GetSelectedCommande(object sender, MouseButtonEventArgs e)
        {
               try
               {
                   Commande bon = (Commande)commandesDataGrid.SelectedItem;
                   Win_Commande win = new Win_Commande(bon);
                   win.ShowDialog();
                   ChercherBtn_Click(new object(), new RoutedEventArgs());
               }
               catch (Exception)
               {

                   MessageBox.Show("Probleme !");
               }
        }
    }
}
