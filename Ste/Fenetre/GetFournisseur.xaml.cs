using Domain.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Logique d'interaction pour GetFournisseur.xaml
    /// </summary>
    public partial class GetFournisseur : Window
    {
        
        FournisseurService ser_four = new FournisseurService();
        public Fournisseur fournisseurToSend;
         public GetFournisseur()
        {
            InitializeComponent();
            fournisseurDataGrid.ItemsSource = ser_four.getAllFournisseur();
            fournisseurToSend = null;
        }

    

        private void fournisseurDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Fournisseur drv = (Fournisseur)fournisseurDataGrid.SelectedItem;
            try
            {
                fournisseurToSend = drv;
                this.Close();
            }
            catch (Exception)
            {
                // MessageBox.Show("-__-");
            }
        }

        private void nom_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = sender as TextBox;
            fournisseurDataGrid.ItemsSource = ser_four.findFournisseurByNom(t.Text);
        }

        private void code_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = sender as TextBox;
           // fournisseurDataGrid.ItemsSource = ser_four.findFournisseurByID(long.Parse( t.Text));
        }

        


       
    }
}
