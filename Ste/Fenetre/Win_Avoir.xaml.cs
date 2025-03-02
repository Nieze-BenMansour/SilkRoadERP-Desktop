using Domain.Entites;
using Domain.Models;
using Service;
using Ste.Classes;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Win_Avoir.xaml
    /// </summary>
    public partial class Win_Avoir : Window
    {

        // les services
        LigneAvoirService ser_ligneAvoir = new LigneAvoirService();
        AvoirService ser_avoir = new AvoirService();
        SystemeService ser_systeme = new SystemeService();
        ProduitService ser_produit = new ProduitService();
        ClientService ser_client = new ClientService();
        //les instance de travail
        Avoir Current_Avoir = new Avoir();
        Client currentClient = new Client();
        //logique de la fenetre
        bool avoir_ajouter;
        bool AltKeyPressed = false;
        LigneDynamique ligneDy = new LigneDynamique();
        List<LignrBLclass> Liste_Lignes = new List<LignrBLclass>();
        PrintEngine printEng = new PrintEngine();
        SommeEnLettres somme = new SommeEnLettres();
        WindowControle avoirControle = new WindowControle();
        public Win_Avoir(Avoir avoirToSend)
        {
            InitializeComponent();
            Current_Avoir = avoirToSend;
            ligneDy.GridBL = GridBC;//affecter le grid
            ligneDy.Liste_Lignes = Liste_Lignes;// affecter les lignes
            if (Current_Avoir != null)
            {
                avoir_ajouter = true;
                NumAvir_Label.Content = Current_Avoir.Num;
                CodeLabel.Content = Current_Avoir.clientId;
                ClientLabel.Content = Current_Avoir.Client.nom;
                DateAvoir.SelectedDate = Current_Avoir.date;
        
                currentClient = ser_client.findClientByID(Current_Avoir.clientId.GetValueOrDefault());


                foreach (var item in ser_ligneAvoir.findLigneBlByNumAvoir(Current_Avoir.Num))
                {
                    LignrBLclass ligne = new LignrBLclass();
                    ligne.refUI.Text = item.Ref_Produit;
                    ligne.desiUI.Text = item.designation_li;
                    ligne.qteUI.Value = item.qte_li;
                    ligne.prixHT_UI.Value = item.prix_HT;
                    ligne.remise_UI.Text = string.Format("{0:00.00}", item.remise);
                    ligne.tot_ht_UI.Value = item.tot_HT;
                    ligne.tva_UI.Text = string.Format("{0:00.00}", item.tva);
                    ligne.tot_ttc_UI.Value = item.tot_TTC;
                    ligneDy.CreateRowDevisLoad(ligne);
                }
                ligneDy.button_plus_devis_load();
            }
            // num_BC = -1 ajout
            else
            {
                DateAvoir.SelectedDate = DateTime.Now;
                ligneDy.Liste_Lignes = Liste_Lignes;// affecter les lignes 
                ligneDy.GridBL = GridBC;//affecter le grid
                ligneDy.CreateRowDevis();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Current_Avoir != null)
                ligneDy.set_event_handler_Devis_Load();
        }

        private void KeyDown_total_setTVA_setRemise(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F4)
            {
                Enregistrer_BC_Click(new Object(), new RoutedEventArgs());
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
                int laster_button = GridBC.Children.Count - 1;
                GridBC.Children.RemoveAt(laster_button);
                ligneDy.CreateRowDevis();

            }
        }

        private void getClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetClient win = new GetClient();
                win.ShowDialog();
                currentClient = win.clientToSend;
                CodeLabel.Content = win.clientToSend.Id;
                ClientLabel.Content = win.clientToSend.nom;
            }
            catch (Exception)
            {

            }
        }

        private void Enregistrer_BC_Click(object sender, RoutedEventArgs e)
        {
            if (avoirControle.BL_Valide(DateAvoir,CodeLabel) && avoirControle.Lignes_Valide(Liste_Lignes))
            {
                int code_client = int.Parse(CodeLabel.Content.ToString());
                string date = DateAvoir.SelectedDate.Value.ToShortDateString();
                if (Current_Avoir != null)
                {
                    try
                    {
                        Current_Avoir.clientId = code_client;
                        Current_Avoir.date = DateAvoir.SelectedDate.Value;
                        Current_Avoir.tot_H_tva = ligneDy.Total_HT(Liste_Lignes);
                        Current_Avoir.tot_tva = ligneDy.Total_tva(Liste_Lignes);
                        Current_Avoir.net_payer = ligneDy.Total_TTC(Liste_Lignes);
                        ser_avoir.editAvoir(Current_Avoir);
                        Recharger_Stock(Current_Avoir.Num);
                        Ajouter_Lignes_Data_Base();

                        MessageBox.Show("Avoir mis a jour !");
                    }
                    catch (Exception) { MessageBox.Show("Probleme !"); }
                }
                if (Current_Avoir == null)
                {
                    try
                    {
                        Current_Avoir = new Avoir();
                        Current_Avoir.clientId = code_client;
                        Current_Avoir.date = DateAvoir.SelectedDate.Value;
                        Current_Avoir.tot_H_tva = ligneDy.Total_HT(Liste_Lignes);
                        Current_Avoir.tot_tva = ligneDy.Total_tva(Liste_Lignes);
                        Current_Avoir.net_payer = ligneDy.Total_TTC(Liste_Lignes);
                        ser_avoir.addAvoir(Current_Avoir);
                        Ajouter_Lignes_Data_Base();
                        avoir_ajouter = true;
                        MessageBox.Show("Avoir enregistré !  " + date);
                    }
                    catch (Exception ec) { MessageBox.Show(ec.Message); }
                } 
            }
            
        }
        bool Ajouter_Lignes_Data_Base()
        {
            bool test = true;
            try
            {

                List<Produit> listeProduit = new List<Produit>();
                List<LigneAvoir> listeLigneAvoir = new List<LigneAvoir>();
                int num = Current_Avoir.Num;

                for (int i = 0; i < Liste_Lignes.Count; i++)
                {
                    try
                    {
                        Produit produit_tmp = ser_produit.findProduitByRefUnique(Liste_Lignes[i].refUI.Text);
                        produit_tmp.qte += int.Parse(Liste_Lignes[i].qteUI.Value.ToString());
                        //ser_produit.editProduit(produit_tmp);
                        listeProduit.Add(produit_tmp);
                        LigneAvoir ligne = new LigneAvoir();
                        ligne.Num_avoir = Current_Avoir.Num;
                        ligne.qte_li = int.Parse(Liste_Lignes[i].qteUI.Value.ToString());
                        ligne.Ref_Produit = Liste_Lignes[i].refUI.Text;
                        ligne.designation_li = Liste_Lignes[i].desiUI.Text;
                        ligne.prix_HT = decimal.Parse(Liste_Lignes[i].prixHT_UI.Value.ToString());
                        ligne.remise = float.Parse(Liste_Lignes[i].remise_UI.Text);
                        ligne.tot_HT = decimal.Parse(Liste_Lignes[i].tot_ht_UI.Value.ToString());
                        ligne.tva = float.Parse(Liste_Lignes[i].tva_UI.Text);
                        ligne.tot_TTC = decimal.Parse(Liste_Lignes[i].tot_ttc_UI.Value.ToString());
                        //ser_ligneAvoir.AddLigneAvoir(ligne);
                        listeLigneAvoir.Add(ligne);
                    }
                    catch (Exception) { MessageBox.Show("Reference ! non valide !"); test = false; }

                }
                ser_produit.editManyProduit(listeProduit);
                ser_ligneAvoir.AddManyLigneAvoir(listeLigneAvoir);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return test;
        }

        void Recharger_Stock(int n)
        {
            try
            {
                List<Produit> listeProduit = new List<Produit>();
                foreach (var item in ser_ligneAvoir.findLigneBlByNumAvoir(n))
                {
                    int qte_L = item.qte_li;
                    Produit produit_tmp = ser_produit.findProduitByRefUnique(item.Ref_Produit);
                    produit_tmp.qte -= qte_L;
                    //ser_produit.editProduit(produit_tmp);
                    listeProduit.Add(produit_tmp);
                }
                ser_produit.editManyProduit(listeProduit);
                ser_ligneAvoir.deleteLigneBlByNumAvoir(Current_Avoir);
            }
            catch (Exception) { MessageBox.Show("Probleme de stock !"); }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            int N_li_page = Liste_Lignes.Count / 34;
            int Res = Liste_Lignes.Count % 34;

            charge_Window_to_Print(N_li_page, Res);
        }
        public void charge_Window_to_Print(int N_li_page, int Res)
        {
            try
            {

                string sEnlettre = somme.converti((float)ligneDy.Total_TTC(Liste_Lignes), "Avoir");
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
              //  printEng.Export(new Uri(Directory.GetCurrentDirectory().ToString() + "\\Avoir\\" + "Avoir-" + NumAvir_Label.Content.ToString() + "___" + String.Format("{0:d-M-yyyy}", DateAvoir.DisplayDate) + ".xps"), vc);
                //fenetre contient le Document View
                /*   WindowBeforePrint wi = new WindowBeforePrint();
                   XpsDocument xpsDocument = new XpsDocument(Directory.GetCurrentDirectory().ToString() + "\\Avoir\\" + "Avoir-" + NumAvir_Label.Content.ToString() + "___" + String.Format("{0:d-M-yyyy}", DateAvoir.DisplayDate) + ".xps", FileAccess.Read);
                   wi.document_a_print.Document = xpsDocument.GetFixedDocumentSequence();
                   wi.Show();
                   xpsDocument.Close();*/

            }
            catch (Exception)
            {
                MessageBox.Show("Probleme");
            }
            
        }
        public void SetWindowToPrintBL(WindowToPrint winToPrint, bool manyPage, int curentPage, int numTotPage)
        {
            string sEnlettre = somme.converti((float)ligneDy.Total_TTC(Liste_Lignes), "BonDeReception");
            winToPrint.blocNomSte.Text = ser_systeme.findById(1).NomSociete;
            winToPrint.blockClient.Text = "Client : " + currentClient.nom;
            winToPrint.blocNumType.Text = "Avoir N° : " + String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:0000}", NumAvir_Label.Content);
            winToPrint.blocAdresse.Text = ser_systeme.findById(1).adresse;
            winToPrint.blocTel.Text = "Tel: " + ser_systeme.findById(1).tel
            + "   Fax: " + ser_systeme.findById(1).fax;
            winToPrint.blocTVA.Text = "TVA:  " + ser_systeme.findById(1).matriculeFiscale + "/" + ser_systeme.findById(1).codeTVA + "/" + ser_systeme.findById(1).codeCategorie + "/" + ser_systeme.findById(1).etbSecondaire;
            winToPrint.blocEmail.Text = "E-mail:  " + ser_systeme.findById(1).email;
            winToPrint.blocTimbre1.Visibility = Visibility.Hidden;
            winToPrint.blocDate.Text = "Date : " + DateAvoir.Text;
            winToPrint.numPage.Text = "Page : " + curentPage.ToString() + "/" + numTotPage.ToString();

            if (manyPage == false)
            {
                winToPrint.blocTotalHT.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ligneDy.Total_HT(Liste_Lignes));
                winToPrint.bloctotalTva.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ligneDy.Total_tva(Liste_Lignes));
                winToPrint.blocTotalNet.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ligneDy.Total_TTC(Liste_Lignes));
                winToPrint.blocSomme.Text = sEnlettre;
            }


        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (avoir_ajouter)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Avoir non enregistré", "Alerte", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
