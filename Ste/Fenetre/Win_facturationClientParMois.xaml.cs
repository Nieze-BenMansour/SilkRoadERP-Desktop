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
    /// Interaction logic for Win_facturationClientParMois.xaml
    /// </summary>
    public partial class Win_facturationClientParMois : Window
    {
        public Client clientOur { get; set; }
        FactureService ser_facture = new FactureService();
        BonDeLivraisonService ser_bonDeLiv = new BonDeLivraisonService();
        LigneBLService ser_LigneBL = new LigneBLService();
        SystemeService ser_systeme = new SystemeService();

        public Win_facturationClientParMois()
        {
            InitializeComponent();
        }

        private void ValiderBtn_Click(object sender, RoutedEventArgs e)
        {
            decimal cumulFact = 0;
            decimal totperfact = decimal.Parse(textBoxMaxPerFacture.Text);
            int nbreFact = int.Parse(textBoxNbreFacture.Text);
            for (int i= 0;i< nbreFact;i++ )
            {
                cumulFact = 0;
                List<BonDeLivraison> listBL= ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(clientOur);
                Facture facture = new Facture();
                facture.id_client = clientOur.Id;
                facture.date = dateFin.SelectedDate.Value;
                ser_facture.AddFacture(facture);
                foreach(var item in listBL)
                {
                    if(cumulFact < totperfact && item.net_payer <= totperfact && datedebu.SelectedDate.Value < item.date && item.date < dateFin.SelectedDate.Value)
                    {
                        item.Num_Facture = facture.Num;
                        ser_bonDeLiv.editBonDeLivraison(item);
                        cumulFact += item.net_payer;
                    }
                }
            }
        }
    }
}
