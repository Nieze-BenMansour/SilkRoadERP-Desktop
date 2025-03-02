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
    /// Interaction logic for Win_Articles_manquants.xaml
    /// </summary>
    public partial class Win_Articles_manquants : Window
    {
        ProduitService ser_prod = new ProduitService();
        List<Produit> produits = new List<Produit>();
        public Win_Articles_manquants()
        {
            InitializeComponent();
            
        }

        private void Win_Loaded(object sender, RoutedEventArgs e)
        {
            checkBox.IsChecked = true;
            produits = ser_prod.getAllProduit();
            produits.RemoveAll(t => t.visibilite == !checkBox.IsChecked.Value );
            produits.RemoveAll(t => t.qte > t.qteLimite);

          
            produitDataGrid.ItemsSource = produits;
        }

        private void textBox_search_ref_TextChanged(object sender, TextChangedEventArgs e)
        {
            produits = ser_prod.findProduitByRef(textBox_search_ref.Text);
            produits.RemoveAll(t => t.visibilite == !checkBox.IsChecked.Value);
            produits.RemoveAll(t => t.qte > t.qteLimite);
            produitDataGrid.ItemsSource = produits;
        }

        private void textBox_search_desi_TextChanged(object sender, TextChangedEventArgs e)
        {
            produits = ser_prod.findProduitByDesignation(textBox_search_desi.Text);
            produits.RemoveAll(t => t.visibilite == !checkBox.IsChecked.Value);
            produits.RemoveAll(t => t.qte > t.qteLimite);
            produitDataGrid.ItemsSource = produits;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonRefresh_Click(object sender, RoutedEventArgs e)
        {
            textBox_search_desi.Text = null;
            textBox_search_ref.Text = null;
            produits = ser_prod.getAllProduit();

            produits.RemoveAll(t => t.visibilite == !checkBox.IsChecked.Value);
            produits.RemoveAll(t => t.qte > t.qteLimite);

            produitDataGrid.ItemsSource = produits;
        }
    }
}
