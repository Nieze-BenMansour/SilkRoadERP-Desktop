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
using Domain.Entites;
using Domain.Models;
using Service;
using Ste.Classes;

namespace Ste.Fenetre
{
    /// <summary>
    /// Interaction logic for Win_AvoirFournisseur.xaml
    /// </summary>
    public partial class Win_AvoirFournisseur : Window
    {
        // les services
        LigneAvoirFournisseurService ser_ligneAvFr = new LigneAvoirFournisseurService();
        AvoirFournisseurService ser_AvFr = new AvoirFournisseurService();
        SystemeService ser_systeme = new SystemeService();
        ProduitService ser_produit = new ProduitService();
        FournisseurService ser_four = new FournisseurService();
        //les instance de travail
        AvoirFournisseur Current_AvFr = new AvoirFournisseur();
        Fournisseur currentFournisseur = null;
        //logique de la fenetre
        bool AvFR_ajouter;
        bool AltKeyPressed = false;
        LigneDynamique ligneDy = new LigneDynamique();
        List<LignrBLclass> Liste_Lignes = new List<LignrBLclass>();
        PrintEngine printEng = new PrintEngine();
        SommeEnLettres somme = new SommeEnLettres();
        BonDeReceptionControle BON = new BonDeReceptionControle();
        public Win_AvoirFournisseur(AvoirFournisseur avFr_toSend)
        {
            InitializeComponent();
            Current_AvFr = avFr_toSend;
            ligneDy.GridBL = GridAvFR;//affecter le grid
            ligneDy.Liste_Lignes = Liste_Lignes;// affecter les lignes
            if (Current_AvFr != null)
            {
                AvFR_ajouter = true;
                NumAvFr_Label.Content = Current_AvFr.Num;
                CodeLabel.Content = Current_AvFr.fournisseurId;
                NomFournisseurLabel.Content = Current_AvFr.Fournisseur.nom;
                datePickerAvfr.SelectedDate = Current_AvFr.date;
                textBoxNumaVfrFournisseur.Text = Current_AvFr.Num_AvoirFournisseur.ToString();
                currentFournisseur = ser_four.findFournisseurByID((long)Current_AvFr.fournisseurId);


                foreach (var item in ser_ligneAvFr.findLigneAvoirFrByNumAvoirFr(Current_AvFr.Num))
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
                ligneDy.button_plus_BR_load(Current_AvFr.Fournisseur);
            }
            // num_BC = -1 ajout
            else
            {
                try
                {
                    datePickerAvfr.SelectedDate = DateTime.Now;
                    NumAvFr_Label.Content = ser_AvFr.MaxIdBL() + 1;                 
                    ligneDy.Liste_Lignes = Liste_Lignes;
                    ligneDy.GridBL = GridAvFR;
                }
                catch (Exception)
                {
                    
                }                
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
            if (Current_AvFr != null)
                ligneDy.set_event_handler_BR_Load(Current_AvFr.Fournisseur);
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            if (AvFR_ajouter)
            {
                int N_li_page = Liste_Lignes.Count / 34;
                int Res = Liste_Lignes.Count % 34;
                charge_Window_to_Print(N_li_page, Res);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (AvFR_ajouter)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Avoir fournisseur non enregistré", "Alerte", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Enregistrer_AvFr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int code_fournisseur = int.Parse(CodeLabel.Content.ToString());
                string date = datePickerAvfr.SelectedDate.Value.ToShortDateString();
                long number = 0;
                if (Current_AvFr != null && long.TryParse(textBoxNumaVfrFournisseur.Text, out number))
                {
                    try
                    {
                        Current_AvFr = ser_AvFr.findAvoirFournisseurByNum(Current_AvFr.Num);
                        Current_AvFr.fournisseurId = code_fournisseur;
                        Current_AvFr.date = datePickerAvfr.SelectedDate.Value;
                        Current_AvFr.date = datePickerAvfr.SelectedDate.Value;
                        Current_AvFr.Num_AvoirFournisseur = int.Parse(textBoxNumaVfrFournisseur.Text);
                        ser_AvFr.editAvoirFournisseur(Current_AvFr);
                        Recharger_Stock(Current_AvFr.Num);
                        Ajouter_Lignes_Data_Base();

                        MessageBox.Show("Bon de réception mis a jour !");
                    }
                    catch (Exception) { MessageBox.Show("Probleme Bon de reception!"); }
                }
                if (Current_AvFr == null && long.TryParse(textBoxNumaVfrFournisseur.Text, out number))
                {
                    try
                    {
                        Current_AvFr = new AvoirFournisseur();
                        Current_AvFr.fournisseurId = code_fournisseur;
                        Current_AvFr.date = datePickerAvfr.SelectedDate.Value;
                        Current_AvFr.Num_AvoirFournisseur = int.Parse(textBoxNumaVfrFournisseur.Text);
                        Current_AvFr.Num_FactureAvoirFournisseur = null;
                        ser_AvFr.AddAvoirFournisseur(Current_AvFr);
                        Ajouter_Lignes_Data_Base();
                        AvFR_ajouter = true;
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
        private void PreviewTextInput_nie(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;

            }
        }
        private void Total_setRemise_setTVA(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F4)
            {
                Enregistrer_AvFr_Click(new Object(), new RoutedEventArgs());
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
                    int laster_button = GridAvFR.Children.Count - 1;
                    GridAvFR.Children.RemoveAt(laster_button);
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
                          + "Num Avoir fournisseur : " + Current_AvFr.Num + "\n" + "Date livraison : " + Current_AvFr.date.ToShortDateString();
            winToPrint.blocNumType.Text = "Bon de réception N° : " + String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:0000}", Current_AvFr.Num);
            winToPrint.blocAdresse.Text = ser_systeme.findById(1).adresse;
            winToPrint.blocTel.Text = "Tel: " + ser_systeme.findById(1).tel
            + "   Fax: " + ser_systeme.findById(1).fax;
            winToPrint.blocTVA.Text = "TVA:  " + ser_systeme.findById(1).matriculeFiscale + "/" + ser_systeme.findById(1).codeTVA + "/" + ser_systeme.findById(1).codeCategorie + "/" + ser_systeme.findById(1).etbSecondaire;
            winToPrint.blocEmail.Text = "E-mail:  " + ser_systeme.findById(1).email;
            winToPrint.blocTimbre1.Visibility = Visibility.Hidden;
        //    winToPrint.blocDate.Text = "Date : " + DateBC.Text;
            winToPrint.numPage.Text = "Page : " + curentPage.ToString() + "/" + numTotPage.ToString();

            if (manyPage == false)
            {
                winToPrint.blocTotalHT.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ligneDy.Total_HT(Liste_Lignes));
                winToPrint.bloctotalTva.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ligneDy.Total_tva(Liste_Lignes));
                winToPrint.blocTotalNet.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ligneDy.Total_TTC(Liste_Lignes));
                winToPrint.blocSomme.Text = sEnlettre;
            }


        }
        bool Ajouter_Lignes_Data_Base()
        {
            bool test = true;
            try
            {
                List<Produit> listeProduit = new List<Produit>();
                List<LigneAvoirFourniseur> listeLLigneAvoirFourniseur = new List<LigneAvoirFourniseur>();
                for (int i = 0; i < Liste_Lignes.Count; i++)
                {
                    decimal tot_ttc_t = decimal.Parse(Liste_Lignes[i].tot_ttc_UI.Value.ToString());
                    try
                    {
                        Produit produit_tmp = ser_produit.findProduitByRefUnique(Liste_Lignes[i].refUI.Text);
                        produit_tmp.qte -= int.Parse(Liste_Lignes[i].qteUI.Value.ToString());
                        listeProduit.Add(produit_tmp);
                        LigneAvoirFourniseur ligne = new LigneAvoirFourniseur();
                        ligne.Num_avoirFr = Current_AvFr.Num;
                        ligne.qte_li = int.Parse(Liste_Lignes[i].qteUI.Value.ToString());
                        ligne.Ref_Produit = Liste_Lignes[i].refUI.Text;
                        ligne.designation_li = Liste_Lignes[i].desiUI.Text;
                        ligne.prix_HT = decimal.Parse(Liste_Lignes[i].prixHT_UI.Value.ToString());
                        ligne.remise = float.Parse(Liste_Lignes[i].remise_UI.Text);
                        ligne.tot_HT = decimal.Parse(Liste_Lignes[i].tot_ht_UI.Value.ToString());
                        ligne.tva = float.Parse(Liste_Lignes[i].tva_UI.Text);
                        ligne.tot_TTC = tot_ttc_t;
                        listeLLigneAvoirFourniseur.Add(ligne);
                    }
                    catch (Exception) { MessageBox.Show("Reference ! non valide !"); return false; }

                }
                ser_produit.editManyProduit(listeProduit);
                ser_ligneAvFr.AddligneAvoirFrMany(listeLLigneAvoirFourniseur);

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
                foreach (var item in ser_ligneAvFr.findLigneAvoirFrByNumAvoirFr(n))
                {
                    int qte_L = item.qte_li;
                    Produit produit_tmp = ser_produit.findProduitByRefUnique(item.Ref_Produit);
                    produit_tmp.qte += qte_L;
                    // ser_produit.editProduit(produit_tmp); 
                    listeProduit.Add(produit_tmp);
                }
                ser_produit.editManyProduit(listeProduit);
                ser_ligneAvFr.deleteLigneAvoirFourByNumAvoirFr(Current_AvFr);
            }
            catch (Exception) { MessageBox.Show("Probleme de stock !"); }
        }
    }
}

