using Domain.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Logique d'interaction pour Window_Facture_A_M.xaml
    /// </summary>
    public partial class Window_Facture_A_M : Window
    {
        Facture facture ;
        Client client ;
        FactureService ser_facture = new FactureService();
        
        public Window_Facture_A_M(Facture factureReceve,Client clientRec)
        {
            InitializeComponent();
            facture = factureReceve;
            client = clientRec;
            label.Content = client.Id;

            if (facture != null && client != null)
            {
                date_facture.SelectedDate = facture.date;
            }

        }

        private void valider_button_Click(object sender, RoutedEventArgs e)
        {
            if(facture == null && client != null)
            {
                facture = new Facture();
                facture.id_client = client.Id;
                facture.date = date_facture.SelectedDate.Value;
                ser_facture.AddFacture(facture);
                this.Close();
            }
            else if(facture != null && client != null)
            {
                Facture factureMod = ser_facture.findFactureByNum(facture.Num);
                factureMod.date = date_facture.SelectedDate.Value;
                ser_facture.editFacture(factureMod);
                this.Close();
            }
        }


        private void Annuler_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
           
    }
}
