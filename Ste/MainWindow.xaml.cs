using Ste.Fenetre;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Service;
using Domain.Models;
using Ste.Fenetre.FactureAvoirFournisseurFolder;

namespace Ste
{   
    
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
     
        public MainWindow()
        {
            InitializeComponent();       
        }


        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

       

        private void ConsulterFournisseur_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Source = new Uri("Pages/PageFournisseur.xaml", UriKind.RelativeOrAbsolute);

        }

        private void AjouterFournisseur_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void Gestion_Client_Click(object sender, RoutedEventArgs e)
        {
            ClientCRUD win = new ClientCRUD();
            win.ShowDialog();
        }

        private void Menu_BL_Click(object sender, RoutedEventArgs e)
        {
            WindowAjouterBonDeLivraison win = new WindowAjouterBonDeLivraison(null,null);
             win.ShowDialog();
        }

  

        private void Espace_Client_Click(object sender, RoutedEventArgs e)
        {
            Win_FacturationClient win = new Win_FacturationClient();
            win.ShowDialog();
        }
     
       
        //Devis
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            //-1 pour dire qu'il s'agit d'ajouter
            WindowDevis win = new WindowDevis(null);
            win.ShowDialog();
        }

        private void FournisseurMenu1_Click(object sender, RoutedEventArgs e)
        {
            FournisseurCRUD win = new FournisseurCRUD();
            win.ShowDialog();
        }


        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window_BonDeReception win = new Window_BonDeReception(null);
            win.ShowDialog();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            mainFrame.Source = new Uri("Pages/PageBonDeReception_vis.xaml", UriKind.RelativeOrAbsolute);  

        }

        private void PrdoduitMenu_Click(object sender, RoutedEventArgs e)
        {
            ProduitCRUD win = new ProduitCRUD();
            win.ShowDialog();
        }

        private void GestionBL_Click(object sender, RoutedEventArgs e)
        {
            Win_ManageBonDeLiv win = new Win_ManageBonDeLiv();
            win.ShowDialog();
        }

        private void GestionDevisBtn_Click(object sender, RoutedEventArgs e)
        {
            Win_ManageDevis win = new Win_ManageDevis();
            win.ShowDialog();
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            Win_ManageBonDeReception win = new Win_ManageBonDeReception();
            win.ShowDialog();
        }

        private void Facturation_fournisseur_Click(object sender, RoutedEventArgs e)
        {
            Win_FacturationFournisseur win = new Win_FacturationFournisseur();
            win.ShowDialog();
        }

        private void PrdoduitManquantMenu_Click(object sender, RoutedEventArgs e)
        {
            Win_Articles_manquants win = new Win_Articles_manquants();
            win.ShowDialog();
        }

        private void echeance_fournisseur_Click(object sender, RoutedEventArgs e)
        {
            Win_EchanceFournisseur win = new Win_EchanceFournisseur();
            win.ShowDialog();
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            Win_Avoir win = new Win_Avoir(null);
            win.ShowDialog();
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            Win_ManageAvoirs win = new Win_ManageAvoirs();
            win.ShowDialog();
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            Win_Commande win = new Win_Commande(null);
            win.ShowDialog();
        }

        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            Win_ManageCommande win = new Win_ManageCommande();
            win.ShowDialog();
        }

        private void visualitionFacBtn_Click(object sender, RoutedEventArgs e)
        {
            Win_TotalDesFacturesClient win = new Win_TotalDesFacturesClient();
            win.ShowDialog();
        }

        private void GestionFacFourBtn_Click(object sender, RoutedEventArgs e)
        {
            Win_FacturationFournisseur win = new Win_FacturationFournisseur();
            win.ShowDialog();
        }

        private void visualitionFacFourBtn_Click(object sender, RoutedEventArgs e)
        {
            Win_TotalDesFactureFournisseur win = new Win_TotalDesFactureFournisseur();
            win.Show();
        }

        private void sysMenu_Click(object sender, RoutedEventArgs e)
        {
            Win_ManageSysteme win = new Win_ManageSysteme();
            win.ShowDialog();
        }

      

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            Reporting2 a = new Reporting2();
            a.Show();
        }

        private void AjouterAvoir_fournisseur_Click(object sender, RoutedEventArgs e)
        {
            Win_AvoirFournisseur win = new Win_AvoirFournisseur(null);
            win.ShowDialog();
        }

        private void VisualiserAvoir_fournisseur_Click(object sender, RoutedEventArgs e)
        {
            Win_ManageAvoirFournisseur win = new Win_ManageAvoirFournisseur();
            win.ShowDialog();
        }

        private void GestionFactureAvoir_fournisseur_Click(object sender, RoutedEventArgs e)
        {
            Win_FacturationAvoirFournisseur win = new Win_FacturationAvoirFournisseur();
            win.ShowDialog();
        }
    }
}
