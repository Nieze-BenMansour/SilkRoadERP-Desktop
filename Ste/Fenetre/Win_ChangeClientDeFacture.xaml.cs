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
    /// Interaction logic for Win_ChangeClientDeFacture.xaml
    /// </summary>
    public partial class Win_ChangeClientDeFacture : Window
    {
        BonDeLivraisonService ser_bl = new BonDeLivraisonService();
        FactureService ser_facture = new FactureService();
        ClientService ser_client = new ClientService();
        Facture currentFacture;
        Client currentClient;
        public Win_ChangeClientDeFacture(Facture facReceved)
        {
            InitializeComponent();
            currentFacture = ser_facture.findFactureByNum(facReceved.Num);
            currentClient = ser_client.findClientByID(currentFacture.id_client);

            datepiFac.SelectedDate = currentFacture.date;
            labelNomClient.Content = currentClient.nom;
        }

      

        private void AnnulerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetClient win = new GetClient();
                win.ShowDialog();
                labelNomClient.Content = win.clientToSend.nom;
                currentClient = ser_client.findClientByID(win.clientToSend.Id);

                currentFacture.id_client = currentClient.Id;
                currentFacture.date = datepiFac.SelectedDate.Value;
                ser_facture.editFacture(currentFacture);

                List<BonDeLivraison> listeBL = ser_facture.findBonDeLivraisonBynumFacture(currentFacture);
                foreach (BonDeLivraison item in listeBL)
                {
                    item.clientId = currentClient.Id;
                    ser_bl.editBonDeLivraison(item);
                }
               
                
            }
            catch (Exception)
            {

            }
        }
    }
}
