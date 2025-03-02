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

namespace Ste.Fenetre.ProduitFolder
{
    /// <summary>
    /// Interaction logic for Win_ChercherPrixAchatPar_Fournisseur.xaml
    /// </summary>
    public partial class Win_ChercherPrixAchatPar_Fournisseur : Window
    {
        LigneBonDeRecepctionService ser_br = new LigneBonDeRecepctionService();
        public Win_ChercherPrixAchatPar_Fournisseur(string refProd)
        {
            InitializeComponent();
            if(refProd != null)
            {
                ligneBonReceptionDataGrid.ItemsSource = ser_br.findLigneBonReceptionByRefProd(refProd);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
                 
        }
    }
}
