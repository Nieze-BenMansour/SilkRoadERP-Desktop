using Domain.Models;
using Service;
using Ste.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
using System.Windows.Xps.Packaging;

namespace Ste.Fenetre
{
    /// <summary>
    /// Logique d'interaction pour WindowDevis.xaml
    /// </summary>
    public partial class WindowDevis : Window
    {

        DeviService ser_Devi = new DeviService();
        ClientService ser_client = new ClientService();
        LigneDeviService ser_ligneDevi = new LigneDeviService();
        ProduitService ser_produit = new ProduitService();
        SystemeService ser_systeme = new SystemeService();
        

        LigneDynamique ligneDy = new LigneDynamique();
        Devis devis = new Devis();//classe de controle
        bool AltKeyPressed = false;
        bool Devis_Ajouter = false;
      
        List<LignrBLclass> Liste_Lignes = new List<LignrBLclass>();
        PrintEngine printEng = new PrintEngine();
        SommeEnLettres somme = new SommeEnLettres();
        Devi devi;
        WindowControle controle = new WindowControle();

        public WindowDevis(Devi deviToSend)
        {
            InitializeComponent();
            devi = deviToSend;
            //il ne s'agit pas d'ajouter
            if (devi != null)
            {
                Devis_Ajouter = true;
                ligneDy.GridBL = GridBL;//affecter le grid
                ligneDy.Liste_Lignes = Liste_Lignes;// affecter les lignes
                foreach (var item in ser_ligneDevi.GetLigneByNumDevis(devi.Num))
                {
                    LignrBLclass ligne = new LignrBLclass();
                    ligne.refUI.Text = item.Ref_produit;
                    ligne.desiUI.Text = item.Designation_li;
                    ligne.qteUI.Value = item.qte_li;
                    ligne.prixHT_UI.Value = item.prix_HT;
                    ligne.remise_UI.Text = string.Format("{0:00.00}", item.remise);
                    ligne.tot_ht_UI.Value = item.tot_HT;
                    ligne.tva_UI.Text = string.Format("{0:00.00}", item.tva);
                    ligne.tot_ttc_UI.Value = item.tot_TTC;
                    ligneDy.CreateRow_BL_Load(ligne);
                }
                ligneDy.button_plus_devis_load();
                NumDevis_Label.Content = devi.Num;
                CodeLabel.Content = devi.Client.Id;
                ClientLabel.Content = devi.Client.nom;
                DateBL.SelectedDate = devi.date;
            }
                // num_Devi = -1 ajout
            else
            {
                NumDevis_Label.Content = ser_Devi.MaxIdDevis() + 1;
                ligneDy.Liste_Lignes = Liste_Lignes;// affecter les lignes 
                ligneDy.GridBL = GridBL;//affecter le grid
                ligneDy.CreateRowDevis();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(devi != null)
            ligneDy.set_event_handler_Devis_Load();

        } 
        //key down on window
        private void Total_setRemise_setTVA(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F4)
            {
                Enregistrer_Devis_Click(new Object(),new RoutedEventArgs());
            }

            if (e.Key == Key.F3)
            {
                WindowTotalBL win = new WindowTotalBL();
                win.totHT.Text = ligneDy.Total_HT(Liste_Lignes).ToString();
                win.totTVA.Text = ligneDy.Total_tva(Liste_Lignes).ToString();
                win.netApayer.Text = ligneDy.Total_TTC(Liste_Lignes).ToString();
                win.ShowDialog();
            }
            if (e.Key == Key.F6)
            {
                int N_li_page = Liste_Lignes.Count / 34;
                int Res = Liste_Lignes.Count % 34;

                charge_Window_to_Print(N_li_page, Res);
            }

            if (AltKeyPressed)
            {
                AltKeyPressed = false;
                if (e.SystemKey == Key.R)
                {
                    WindowSetRemise win = new WindowSetRemise();
                    win.ShowDialog();
                    if (win.ALL_remise != null)
                    {
                        for (int i = 0; i < Liste_Lignes.Count; i++)
                        {
                            Liste_Lignes[i].remise_UI.Text = win.ALL_remise;
                        }
                    }
                }
                if (e.SystemKey == Key.T)
                {
                    WindowSetTVA win = new WindowSetTVA();
                    win.ShowDialog();
                    if (win.all_TVA != null)
                    {
                        for (int i = 0; i < Liste_Lignes.Count; i++)
                        {
                            Liste_Lignes[i].tva_UI.Text = win.all_TVA;
                        }
                    }
                }

            }
            if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt)
            {
                AltKeyPressed = true;
            }
            if (e.Key == Key.Add)
            {
                int laster_button = GridBL.Children.Count - 1;
                GridBL.Children.RemoveAt(laster_button);
                ligneDy.CreateRowDevis();

            }

        }
        private void getClient_Click(object sender, RoutedEventArgs e)
        {
            GetClient win = new GetClient();
            win.ShowDialog();
            CodeLabel.Content = win.clientToSend.Id;
            ClientLabel.Content = win.clientToSend.nom;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (Devis_Ajouter) 
            {
                this.Close(); 
            }
            else
            {
                MessageBox.Show("Devis non enregistré", "Alerte", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Enregistrer_Devis_Click(object sender, RoutedEventArgs e)
        {
            if (controle.BL_Valide(DateBL,CodeLabel) && controle.Lignes_Valide(Liste_Lignes))
            {
                try
                {

                    int code_Client = int.Parse(CodeLabel.Content.ToString());
                    if (Devis_Ajouter == true)
                    {
                        try
                        {
                            devi.id_client = code_Client;
                            devi.date = DateBL.SelectedDate.Value;
                            devi.tot_H_tva = ligneDy.Total_HT(Liste_Lignes);
                            devi.tot_tva = ligneDy.Total_tva(Liste_Lignes);
                            devi.tot_ttc = ligneDy.Total_TTC(Liste_Lignes);
                            ser_Devi.editDevis(devi);
                            ser_ligneDevi.DeleteLigneDevisByNumDevis(devi);
                            Ajouter_Lignes_Data_Base();
                            MessageBox.Show("Devis mis a jour !");
                        }
                        catch (Exception) { MessageBox.Show("Probleme Devis!"); }
                    }
                    if (Devis_Ajouter == false)
                    {
                        try
                        {
                            devi = new Devi();
                            devi.id_client = code_Client;
                            devi.date = DateBL.SelectedDate.Value;
                            devi.tot_H_tva = ligneDy.Total_HT(Liste_Lignes);
                            devi.tot_tva = ligneDy.Total_tva(Liste_Lignes);
                            devi.tot_ttc = ligneDy.Total_TTC(Liste_Lignes);
                            ser_Devi.addDevis(devi);
                            Ajouter_Lignes_Data_Base();
                            Devis_Ajouter = true;
                            MessageBox.Show("Devis enregistré !  ");
                        }
                        catch (Exception) { MessageBox.Show("Probleme Devis!"); }
                    }

                }
                catch (Exception r)
                {
                    MessageBox.Show(r.Message);
                } 
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            int N_li_page = Liste_Lignes.Count / 34;
            int Res = Liste_Lignes.Count % 34;

            charge_Window_to_Print(N_li_page, Res);
        }
        void Ajouter_Lignes_Data_Base()
        {

            try
            {
                List<LigneDevi> listeLigneDevi = new List<LigneDevi>();
                for (int i = 0; i < Liste_Lignes.Count; i++)
                {
                    string ref_t = Liste_Lignes[i].refUI.Text;
                    string desi_t = Liste_Lignes[i].desiUI.Text;
                    int qte_t = int.Parse(Liste_Lignes[i].qteUI.Value.ToString());
                    decimal prix_ht_t = decimal.Parse(Liste_Lignes[i].prixHT_UI.Value.ToString());
                    float remise_t = float.Parse(Liste_Lignes[i].remise_UI.Text);
                    decimal tot_ht_t = decimal.Parse(Liste_Lignes[i].tot_ht_UI.Value.ToString());
                    float tva_t = float.Parse(Liste_Lignes[i].tva_UI.Text);
                    decimal tot_ttc_t = decimal.Parse(Liste_Lignes[i].tot_ttc_UI.Value.ToString());

                    LigneDevi ligne = new LigneDevi();
                    ligne.Num_devis = devi.Num;
                    ligne.qte_li = qte_t;
                    ligne.Ref_produit = ref_t;
                    ligne.Designation_li = desi_t;
                    ligne.prix_HT = prix_ht_t;
                    ligne.remise = remise_t;
                    ligne.tot_HT = tot_ht_t;
                    ligne.tva = tva_t;
                    ligne.tot_TTC = tot_ttc_t;
                    listeLigneDevi.Add(ligne);
                }
                ser_ligneDevi.AddManyLigneDevi(listeLigneDevi);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
          
        }
        public void charge_Window_to_Print(int N_li_page, int Res)
        {
            try
            {
                string sEnlettre = somme.converti((float)ligneDy.Total_TTC(Liste_Lignes), "Devis");
                List<Canvas> vc = new List<Canvas>();

                if (N_li_page < 1)
                {
                    WindowToPrint winToPrint = new WindowToPrint();
                    for (int i = 1; i <= Res; i++)
                    {
                        printEng.Search(0, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i - 1].refUI.Text;
                        printEng.Search(1, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i - 1].desiUI.Text;
                        printEng.Search(2, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i - 1].qteUI.Text;
                        printEng.Search(3, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i - 1].prixHT_UI.Text;
                        printEng.Search(4, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i - 1].remise_UI.Text;
                        printEng.Search(5, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i - 1].tot_ht_UI.Text;
                        printEng.Search(6, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i - 1].tva_UI.Text;
                        printEng.Search(7, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i - 1].tot_ttc_UI.Text;
                    }
                    SetWindowToPrintBL(winToPrint, false, 1, 1);
                    winToPrint.Close();
                    vc.Add(winToPrint.CanvasToPrint);

                }
                if (N_li_page >= 1)
                {
                    int pas = 0;

                    for (int j = 0; j < N_li_page; j++)
                    {
                        WindowToPrint winToPrint = new WindowToPrint();
                        for (int i = 1; i <= 34; i++)
                        {

                            printEng.Search(0, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i + pas - 1].refUI.Text;
                            printEng.Search(1, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i + pas - 1].desiUI.Text;
                            printEng.Search(2, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i + pas - 1].qteUI.Text;
                            printEng.Search(3, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i + pas - 1].prixHT_UI.Text;
                            printEng.Search(4, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i + pas - 1].remise_UI.Text;
                            printEng.Search(5, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i + pas - 1].tot_ht_UI.Text;
                            printEng.Search(6, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i + pas - 1].tva_UI.Text;
                            printEng.Search(7, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i + pas - 1].tot_ttc_UI.Text;

                        }
                        pas += 34;
                        if (Res == 0 && j + 1 < N_li_page)
                        {
                            SetWindowToPrintBL(winToPrint, true, j + 1, N_li_page);
                        }
                        else if (Res != 0 || j + 1 < N_li_page)
                        {
                            SetWindowToPrintBL(winToPrint, true, j + 1, N_li_page + 1);
                        }
                        else
                        {
                            SetWindowToPrintBL(winToPrint, false, j + 1, N_li_page);
                        }
                        vc.Add(winToPrint.CanvasToPrint);
                        winToPrint.Close();
                    }
                    if (Res != 0)
                    {
                        WindowToPrint LastwinToPrint = new WindowToPrint();
                        for (int i = 1; i <= Res; i++)
                        {
                            printEng.Search(0, i, LastwinToPrint.ArticleGrid).Text = Liste_Lignes[i + pas - 1].refUI.Text;
                            printEng.Search(1, i, LastwinToPrint.ArticleGrid).Text = Liste_Lignes[i + pas - 1].desiUI.Text;
                            printEng.Search(2, i, LastwinToPrint.ArticleGrid).Text = Liste_Lignes[i + pas - 1].qteUI.Text;
                            printEng.Search(3, i, LastwinToPrint.ArticleGrid).Text = Liste_Lignes[i + pas - 1].prixHT_UI.Text;
                            printEng.Search(4, i, LastwinToPrint.ArticleGrid).Text = Liste_Lignes[i + pas - 1].remise_UI.Text;
                            printEng.Search(5, i, LastwinToPrint.ArticleGrid).Text = Liste_Lignes[i + pas - 1].tot_ht_UI.Text;
                            printEng.Search(6, i, LastwinToPrint.ArticleGrid).Text = Liste_Lignes[i + pas - 1].tva_UI.Text;
                            printEng.Search(7, i, LastwinToPrint.ArticleGrid).Text = Liste_Lignes[i + pas - 1].tot_ttc_UI.Text;
                        }
                        //remlplir les blocs
                        SetWindowToPrintBL(LastwinToPrint, false, N_li_page + 1, N_li_page + 1);
                        vc.Add(LastwinToPrint.CanvasToPrint);
                        LastwinToPrint.Close();
                    }

                }
                printEng.Export(vc);
            //    printEng.Export(new Uri(Directory.GetCurrentDirectory().ToString() + "\\Devis\\" + "Devis-" + NumDevis_Label.Content.ToString() + "___" + String.Format("{0:d-M-yyyy}", DateBL.DisplayDate) + ".xps"), vc);
            //fenetre contient le Document View
            /*      WindowBeforePrint wi = new WindowBeforePrint();
                  XpsDocument xpsDocument = new XpsDocument(Directory.GetCurrentDirectory().ToString() + "\\Devis\\" + "Devis-" + NumDevis_Label.Content.ToString() + "___" + String.Format("{0:d-M-yyyy}", DateBL.DisplayDate) + ".xps", FileAccess.Read);
                  wi.document_a_print.Document = xpsDocument.GetFixedDocumentSequence();
                  wi.Show();
                  xpsDocument.Close();*/

            }
            catch (Exception e )
            {
                MessageBox.Show(e.Message);  
            }
        }
        public void SetWindowToPrintBL(WindowToPrint winToPrint, bool manyPage, int curentPage, int numTotPage)
        {
            string sEnlettre = somme.converti((float)ligneDy.Total_TTC(Liste_Lignes),"Devis");
            winToPrint.blocNomSte.Text = ser_systeme.findById(1).NomSociete;
            winToPrint.blockClient.Text = "Client : " + ClientLabel.Content.ToString();
            winToPrint.blocNumType.Text = "Devis N° : " + String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:0000}", NumDevis_Label.Content);
    winToPrint.blocAdresse.Text = ser_systeme.findById(1).adresse;
                winToPrint.blocTel.Text = "Tel: " + ser_systeme.findById(1).tel
                + "   Fax: " + ser_systeme.findById(1).fax;
                winToPrint.blocTVA.Text = "TVA:  "+ ser_systeme.findById(1).matriculeFiscale+"/"+ ser_systeme.findById(1).codeTVA+"/"+ ser_systeme.findById(1).codeCategorie+"/"+ ser_systeme.findById(1).etbSecondaire;
                winToPrint.blocEmail.Text = "E-mail:  " + ser_systeme.findById(1).email;
                winToPrint.blocTimbre1.Visibility = Visibility.Hidden;
            winToPrint.blocTimbre1.Visibility = Visibility.Hidden;
            winToPrint.blocDate.Text = "Date : " + DateBL.Text;
            winToPrint.numPage.Text = "Page : " + curentPage.ToString() + "/" + numTotPage.ToString();

            if (manyPage == false)
            {
                winToPrint.blocTotalHT.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}",ligneDy.Total_HT(Liste_Lignes));
                winToPrint.bloctotalTva.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ligneDy.Total_tva(Liste_Lignes));
                winToPrint.blocTotalNet.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ligneDy.Total_TTC(Liste_Lignes));
                winToPrint.blocSomme.Text = sEnlettre;
            }


        }

        private void TransformToBl_Click(object sender, RoutedEventArgs e)
        {
          if(devis.Devis_Valide(this) && Devis_Ajouter)
          {

              WindowAjouterBonDeLivraison win = new WindowAjouterBonDeLivraison(devi,null);
              this.Close();
              win.Show();
          }
        }

        private void gestionClientBTN_Click(object sender, RoutedEventArgs e)
        {
            ClientCRUD win = new ClientCRUD();
            win.ShowDialog();
        }
    }
}
