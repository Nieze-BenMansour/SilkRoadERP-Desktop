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
using System.Data;
using Service;
using Domain.Models;

namespace Ste
{
    public partial class WindowChercherProduitBL : Window
    {
        ProduitService ser_produit = new ProduitService();
        List<Produit> lista = new List<Produit>();
        public string REF=null;
        public Produit produitToSend = new Produit();
        public WindowChercherProduitBL()
        {
            InitializeComponent();
            refSearch.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            if (REF != null)
            {
                Produit produitTtmp = new Produit();
                produitTtmp = ser_produit.findProduit(REF);
                if(produitTtmp != null)
                { 
                lista.Add(ser_produit.findProduit(REF));
                produitDataGrid.ItemsSource = lista;
                }
            }
            else produitDataGrid.ItemsSource = ser_produit.getAllProduit();

         
        }

        private void desisearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            produitDataGrid.ItemsSource = ser_produit.findProduitByDesignation(desisearch.Text);
        }

        private void refSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            produitDataGrid.ItemsSource = ser_produit.findProduitByRef(refSearch.Text);
        }

        private void Get(object sender, MouseButtonEventArgs e)
        {
            try {
                produitToSend = (Produit)produitDataGrid.SelectedItem;
                this.Close();
            }
            catch (Exception)
            {
                // MessageBox.Show("_____");
            }
            
        }

        private void fermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
