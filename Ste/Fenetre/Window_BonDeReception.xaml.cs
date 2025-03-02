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
    /// Logique d'interaction pour Window_BonDeReception.xaml
    /// </summary>
    public partial class Window_BonDeReception : Window
    {
        // les services
        LigneBonDeRecepctionService ser_ligneBC = new LigneBonDeRecepctionService();
        BonDeReceptionService ser_bonDeRec = new BonDeReceptionService();
        SystemeService ser_systeme = new SystemeService();
        ProduitService ser_produit = new ProduitService();
        FournisseurService ser_four = new FournisseurService();
        //les instance de travail
        BonDeReception Current_bon = new BonDeReception();
        Fournisseur currentFournisseur = null;
        //logique de la fenetre
        bool BC_ajouter;
        bool AltKeyPressed = false;
        LigneDynamique ligneDy = new LigneDynamique();
        List<LignrBLclass> Liste_Lignes = new List<LignrBLclass>();
        PrintEngine printEng = new PrintEngine();
        SommeEnLettres somme = new SommeEnLettres();
        BonDeReceptionControle BON = new BonDeReceptionControle();
        

        public Window_BonDeReception(BonDeReception bon_toSend)
        {
            InitializeComponent();
            Current_bon = bon_toSend;
            ligneDy.GridBL = GridBC;//affecter le grid
            ligneDy.Liste_Lignes = Liste_Lignes;// affecter les lignes
            if (Current_bon != null)
            {
                BC_ajouter = true;
                NumBC_Label.Content = Current_bon.Num;
                CodeLabel.Content = Current_bon.id_fournisseur;
                NomFournisseurLabel.Content = Current_bon.Fournisseur.nom;
                DateBC.SelectedDate = Current_bon.date;
                datePickerLivraion.SelectedDate = Current_bon.dateDeLivraison;
                textBoxNumBlFournisseur.Text = Current_bon.num_BonFournisseur.ToString();
                currentFournisseur = ser_four.findFournisseurByID(Current_bon.id_fournisseur);
                currentFournisseur = Current_bon.Fournisseur;
                

                foreach (var item in ser_ligneBC.findLigneBonReceptionByNumBC(Current_bon.Num))
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
                    ligneDy.CreateRowBRLoad(ligne);
                }
                ligneDy.button_plus_BR_load(Current_bon.Fournisseur);
            }
            // num_BC = -1 ajout
            else
            {
                DateBC.SelectedDate = DateTime.Now;
                datePickerLivraion.SelectedDate = DateTime.Now;
                NumBC_Label.Content = ser_bonDeRec.MaxIdBC() + 1;
                ligneDy.Liste_Lignes = Liste_Lignes;// affecter les lignes 
                ligneDy.GridBL = GridBC;//affecter le grid
               // ligneDy.CreateRowBR(Current_bon.Fournisseur);
            }
        }

        private void getFournisseurButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ligneDy.Supprimer_ALL_Row();
                GetFournisseur win = new GetFournisseur();
                win.ShowDialog();
                currentFournisseur = win.fournisseurToSend;
                CodeLabel.Content = win.fournisseurToSend.id;
                NomFournisseurLabel.Content = win.fournisseurToSend.nom;
                ligneDy.CreateRowBR(win.fournisseurToSend);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Current_bon != null)
                ligneDy.set_event_handler_BR_Load(Current_bon.Fournisseur);
        }

        private void Enregistrer_BC_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string date = DateBC.SelectedDate.Value.ToShortDateString();
                long number= 0;
                if (Current_bon != null && long.TryParse( textBoxNumBlFournisseur.Text,out number ))
                {
                    try
                    {
                        Current_bon = ser_bonDeRec.findBonDeReceptionByNum(Current_bon.Num);
                        Current_bon.id_fournisseur = currentFournisseur.id;
                        Current_bon.date = DateBC.SelectedDate.Value;
                        Current_bon.dateDeLivraison = datePickerLivraion.SelectedDate.Value;
                        Current_bon.num_BonFournisseur = long.Parse(textBoxNumBlFournisseur.Text);
                        ser_bonDeRec.editBonDeReception(Current_bon);
                        Recharger_Stock(Current_bon.Num);
                        Ajouter_Lignes_Data_Base();

                        MessageBox.Show("Bon de réception mis a jour !");
                    }
                    catch (Exception) { MessageBox.Show("Probleme Bon de reception!"); }
                }
                if (Current_bon == null && long.TryParse(textBoxNumBlFournisseur.Text, out number))
                {
                    try
                    {
                        Current_bon = new BonDeReception();
                        Current_bon.id_fournisseur = currentFournisseur.id;
                        Current_bon.date = DateBC.SelectedDate.Value;
                        Current_bon.dateDeLivraison = datePickerLivraion.SelectedDate.Value;
                        Current_bon.num_BonFournisseur = long.Parse(textBoxNumBlFournisseur.Text);
                        Current_bon.Num_Facture_fournisseur = null;
                        ser_bonDeRec.AddBonDeReception(Current_bon);
                        Ajouter_Lignes_Data_Base();
                        BC_ajouter = true;
                        MessageBox.Show("Bon de reception enregistré !  " + date);
                    }
                    catch (Exception ec) { MessageBox.Show(ec.Message); }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Probleme !");
            }
            
        }

        bool Ajouter_Lignes_Data_Base()
        {
            bool test = true;
            try
            {
                List<Produit> listeProduit = new List<Produit>();
                List<LigneBonReception> listeLigneBR = new List<LigneBonReception>();
                int num = int.Parse(NumBC_Label.Content.ToString());
         
                for (int i = 0; i < Liste_Lignes.Count; i++)
                {
                    decimal tot_ttc_t = decimal.Parse(Liste_Lignes[i].tot_ttc_UI.Value.ToString());
                    try
                    {
                        Produit produit_tmp = ser_produit.findProduitByRefUnique(Liste_Lignes[i].refUI.Text);
                        produit_tmp.qte += int.Parse(Liste_Lignes[i].qteUI.Value.ToString());
                        listeProduit.Add(produit_tmp);
                        LigneBonReception ligne = new LigneBonReception();
                        ligne.Num_BonRec = Current_bon.Num;
                        ligne.qte_li = int.Parse(Liste_Lignes[i].qteUI.Value.ToString());
                        ligne.Ref_Produit = Liste_Lignes[i].refUI.Text;
                        ligne.designation_li = Liste_Lignes[i].desiUI.Text;
                        ligne.prix_HT = decimal.Parse(Liste_Lignes[i].prixHT_UI.Value.ToString());
                        ligne.remise = float.Parse(Liste_Lignes[i].remise_UI.Text);
                        ligne.tot_HT = decimal.Parse(Liste_Lignes[i].tot_ht_UI.Value.ToString());
                        ligne.tva = float.Parse(Liste_Lignes[i].tva_UI.Text);
                   
                        ligne.tot_TTC = tot_ttc_t;
                        listeLigneBR.Add(ligne);
                    }
                    catch (Exception) { MessageBox.Show("Reference ! non valide !"); return false; }

                }
                ser_produit.editManyProduit(listeProduit);
                ser_ligneBC.AddManyLigneBonDeReception(listeLigneBR);
                
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
                foreach (var item in ser_ligneBC.findLigneBonReceptionByNumBC(n))
                {
                    int qte_L = item.qte_li;
                    Produit produit_tmp = ser_produit.findProduitByRefUnique(item.Ref_Produit);
                    produit_tmp.qte -= qte_L;
                    // ser_produit.editProduit(produit_tmp); 
                    listeProduit.Add(produit_tmp);
                }
                ser_produit.editManyProduit(listeProduit);
                ser_ligneBC.deleteLigneBonReceptionByNumBC(Current_bon);
            }
            catch (SqlException) { MessageBox.Show("Probleme de stock !"); }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            if(BC_ajouter)
            { 
            int N_li_page = Liste_Lignes.Count / 34;
            int Res = Liste_Lignes.Count % 34;
            charge_Window_to_Print(N_li_page, Res);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (BC_ajouter)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Bon de réception non enregistré", "Alerte", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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
                if (currentFournisseur != null)
                {
                    int laster_button = GridBC.Children.Count - 1;
                    GridBC.Children.RemoveAt(laster_button);
                    ligneDy.CreateRowBR(currentFournisseur);
                }
            }
        }
        public void charge_Window_to_Print(int N_li_page, int Res)
        {
            try
            {

                string sEnlettre = somme.converti((float)ligneDy.Total_TTC(Liste_Lignes), "Bon de réception");
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
            //    printEng.Export(new Uri(Directory.GetCurrentDirectory().ToString() + "\\Bon_De_Reception\\" + "Bon_De_Reception-" + NumBC_Label.Content.ToString() + "___" + String.Format("{0:d-M-yyyy}", DateBC.DisplayDate) + ".xps"), vc);
                //fenetre contient le Document View
                /*    WindowBeforePrint wi = new WindowBeforePrint();
                    XpsDocument xpsDocument = new XpsDocument(Directory.GetCurrentDirectory().ToString() + "\\Bon_De_Reception\\" + "Bon_De_Reception-" + NumBC_Label.Content.ToString() + "___" + String.Format("{0:d-M-yyyy}", DateBC.DisplayDate) + ".xps", FileAccess.Read);
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
            winToPrint.blockClient.Text = "Fournisseur : " + currentFournisseur.nom + "\n"
                          + "Code TVA : " + currentFournisseur.matricule + "/" + currentFournisseur.code + "/" + currentFournisseur.code_cat + "/" + currentFournisseur.etb_sec + "\n"
                          + "Num bon fournisseur : " +Current_bon.num_BonFournisseur+"\n"+"Date livraison : "+Current_bon.dateDeLivraison.ToShortDateString();
            winToPrint.blocNumType.Text = "Bon de réception N° : " + String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:0000}", NumBC_Label.Content);
            winToPrint.blocAdresse.Text = ser_systeme.findById(1).adresse;
            winToPrint.blocTel.Text = "Tel: " + ser_systeme.findById(1).tel
            + "   Fax: " + ser_systeme.findById(1).fax;
            winToPrint.blocTVA.Text = "TVA:  " + ser_systeme.findById(1).matriculeFiscale + "/" + ser_systeme.findById(1).codeTVA + "/" + ser_systeme.findById(1).codeCategorie + "/" + ser_systeme.findById(1).etbSecondaire;
            winToPrint.blocEmail.Text = "E-mail:  " + ser_systeme.findById(1).email;
            winToPrint.blocTimbre1.Visibility = Visibility.Hidden;
            winToPrint.blocDate.Text = "Date : " + DateBC.Text;
            winToPrint.numPage.Text = "Page : " + curentPage.ToString() + "/" + numTotPage.ToString();

            if (manyPage == false)
            {
                winToPrint.blocTotalHT.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ligneDy.Total_HT(Liste_Lignes));
                winToPrint.bloctotalTva.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ligneDy.Total_tva(Liste_Lignes));
                winToPrint.blocTotalNet.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ligneDy.Total_TTC(Liste_Lignes));
                winToPrint.blocSomme.Text = sEnlettre;
            }


        }
        private void PreviewTextInput_nie(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;

            }
        }


    }
}
