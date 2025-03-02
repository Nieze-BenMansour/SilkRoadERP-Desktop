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
    /// Interaction logic for Win_Commande.xaml
    /// </summary>
    public partial class Win_Commande : Window
    {
        // les services
        LigneCommandeService ser_ligneCo = new LigneCommandeService();
        CommandeService ser_Commande = new CommandeService();
        SystemeService ser_systeme = new SystemeService();
    
        FournisseurService ser_four = new FournisseurService();
        //les instance de travail
        Commande Current_Com = new Commande();
        Fournisseur currentFournisseur = new Fournisseur();
        //logique de la fenetre
        bool co_ajouter;
        bool AltKeyPressed = false;
        LigneDynamique ligneDy = new LigneDynamique();
        List<LignrBLclass> Liste_Lignes = new List<LignrBLclass>();
        PrintEngine printEng = new PrintEngine();
        SommeEnLettres somme = new SommeEnLettres();
        //BonDeReceptionControle BON = new BonDeReceptionControle();
        public Win_Commande(Commande commandeToSend)
        {
            InitializeComponent();
            Current_Com = commandeToSend;
            ligneDy.GridBL = GridCo;//affecter le grid
            ligneDy.Liste_Lignes = Liste_Lignes;// affecter les lignes
            if (Current_Com != null)
            {
                co_ajouter = true;
                NumCo_Label.Content = Current_Com.Num;
                CodeLabel.Content = Current_Com.fournisseurId;
                NomFournisseurLabel.Content = Current_Com.fournisseur.nom;
                DateBC.SelectedDate = Current_Com.date;
             
                currentFournisseur = ser_four.findFournisseurByID((long) Current_Com.fournisseurId);


                foreach (var item in ser_ligneCo.findLigneCommandeByNumCO(Current_Com.Num))
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
                DateBC.SelectedDate = DateTime.Now;
                if (ser_Commande.getAllCommande().Count > 0)
                {
                    NumCo_Label.Content = ser_Commande.getAllCommande()[ser_Commande.getAllCommande().Count - 1].Num + 1;
                } //etat initiale la base donnée est vide pas de max num
                else {
                    NumCo_Label.Content = 1;
                } //etat initiale la base donnée est vide pas de max num
                ligneDy.Liste_Lignes = Liste_Lignes;// affecter les lignes 
                ligneDy.GridBL = GridCo;//affecter le grid
                ligneDy.CreateRowDevis();
            }
        }

        private void Enregistrer_Com_com(object sender, RoutedEventArgs e)
        {
            try
            {

                int num = int.Parse(NumCo_Label.Content.ToString());
                int code_fournisseur = int.Parse(CodeLabel.Content.ToString());
                string date = DateBC.SelectedDate.Value.ToShortDateString();
                if (Current_Com != null)
                {
                    try
                    {
                        Current_Com.fournisseurId = code_fournisseur;
                        Current_Com.date = DateBC.SelectedDate.Value;
                        Current_Com.tot_H_tva = ligneDy.Total_HT(Liste_Lignes);
                        Current_Com.tot_tva = ligneDy.Total_tva(Liste_Lignes);
                        Current_Com.net_payer = ligneDy.Total_TTC(Liste_Lignes);
                        ser_ligneCo.deleteLigneCommandeByNumCO(Current_Com);
                        ser_Commande.editCommande(Current_Com);

                        Ajouter_Lignes_Data_Base();

                        MessageBox.Show("Commande mis a jour !");
                    }
                    catch (Exception) { MessageBox.Show("Probleme Commande!"); }
                }
                if (Current_Com == null)
                {
                    try
                    {
                        Current_Com = new Commande();
                        Current_Com.fournisseurId = code_fournisseur;
                        Current_Com.date = DateBC.SelectedDate.Value;
                        Current_Com.tot_H_tva = ligneDy.Total_HT(Liste_Lignes);
                        Current_Com.tot_tva = ligneDy.Total_tva(Liste_Lignes);
                        Current_Com.net_payer = ligneDy.Total_TTC(Liste_Lignes);

                        ser_Commande.AddCommande(Current_Com);
                        Ajouter_Lignes_Data_Base();
                        co_ajouter = true;
                        MessageBox.Show("Commande enregistré !  " + date);
                    }
                    catch (Exception ec) { MessageBox.Show(ec.Message); }
                }
            }
            catch (Exception)
            {
            }
            
        }
        bool Ajouter_Lignes_Data_Base()
        {
            int num = int.Parse(NumCo_Label.Content.ToString());
            bool test = true;
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
                try
                {
                    LigneCommande ligne = new LigneCommande();
                    ligne.Num_commande = Current_Com.Num;
                    ligne.qte_li = qte_t;
                    ligne.Ref_Produit = ref_t;
                    ligne.designation_li = desi_t;
                    ligne.prix_HT = prix_ht_t;
                    ligne.remise = remise_t;
                    ligne.tot_HT = tot_ht_t;
                    ligne.tva = tva_t;
                    if (currentFournisseur.constructeur)
                    {
                        ligne.tot_TTC = tot_ttc_t + (tot_ttc_t * ser_systeme.findById(1).pourcentageFodec) / 100;
                    }
                    else
                    {
                        ligne.tot_TTC = tot_ttc_t;
                    }
                    ser_ligneCo.AddLigneCommande(ligne);
                }
                catch (InvalidOperationException) { MessageBox.Show("Reference ! non valide !"); return false; }

            }
            return test;
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
                int N_li_page = Liste_Lignes.Count / 34;
                int Res = Liste_Lignes.Count % 34;

                charge_Window_to_Print(N_li_page, Res);
            
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (co_ajouter)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Devis non enregistré", "Alerte", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void getFournisseurButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetFournisseur win = new GetFournisseur();
                win.ShowDialog();
                currentFournisseur = win.fournisseurToSend;
                CodeLabel.Content = win.fournisseurToSend.id;
                NomFournisseurLabel.Content = win.fournisseurToSend.nom;
            }
            catch (Exception)
            {

            }
        }
        private void KeyDown_total_setTVA_setRemise(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F4)
            {
                Enregistrer_Com_com(new Object(), new RoutedEventArgs());
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
                int laster_button = GridCo.Children.Count - 1;
                GridCo.Children.RemoveAt(laster_button);
                ligneDy.CreateRowDevis();

            }
        }

        private void Window_loaded(object sender, RoutedEventArgs e)
        {
            if (Current_Com != null)
                ligneDy.set_event_handler_Devis_Load();
        }
        public void charge_Window_to_Print(int N_li_page, int Res)
        {
            try
            {
                
                List<Canvas> vc = new List<Canvas>();

                if (N_li_page < 1)
                {
                    WindowToPrint winToPrint = new WindowToPrint();
                    for (int i = 1; i <= Res; i++)
                    {
                        printEng.Search(0, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i - 1].refUI.Text;
                        printEng.Search(1, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i - 1].desiUI.Text;
                        printEng.Search(2, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i - 1].qteUI.Text;
                      /*  printEng.Search(3, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i - 1].prixHT_UI.Text;
                        printEng.Search(4, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i - 1].remise_UI.Text;
                        printEng.Search(5, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i - 1].tot_ht_UI.Text;
                        printEng.Search(6, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i - 1].tva_UI.Text;
                        printEng.Search(7, i, winToPrint.ArticleGrid).Text = Liste_Lignes[i - 1].tot_ttc_UI.Text;*/
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
                           
                        }
                        //remlplir les blocs
                        SetWindowToPrintBL(LastwinToPrint, false, N_li_page + 1, N_li_page + 1);
                        vc.Add(LastwinToPrint.CanvasToPrint);
                        LastwinToPrint.Close();
                    }
                }
                printEng.Export(vc);
            //    printEng.Export(new Uri(Directory.GetCurrentDirectory().ToString() + "\\Commande\\" + "Commande-" + NumCo_Label.Content.ToString() + "___" + String.Format("{0:d-M-yyyy}", DateBC.DisplayDate) + ".xps"), vc);
                //fenetre contient le Document View
             /*   WindowBeforePrint wi = new WindowBeforePrint();
                XpsDocument xpsDocument = new XpsDocument(Directory.GetCurrentDirectory().ToString() + "\\Commande\\" + "Commande-" + NumCo_Label.Content.ToString() + "___" + String.Format("{0:d-M-yyyy}", DateBC.DisplayDate) + ".xps", FileAccess.Read);
                wi.document_a_print.Document = xpsDocument.GetFixedDocumentSequence();
                wi.Show();
                xpsDocument.Close();*/
            }
            catch (Exception)
            {
                MessageBox.Show("Prob de print");
            }
        }
        public void SetWindowToPrintBL(WindowToPrint winToPrint, bool manyPage, int curentPage, int numTotPage)
        {
            winToPrint.blocNomSte.Text = ser_systeme.findById(1).NomSociete;
            winToPrint.blockClient.Text = "Fournisseur : " + currentFournisseur.nom;
            winToPrint.blocNumType.Text = "Commande";
            winToPrint.blocAdresse.Text = ser_systeme.findById(1).adresse;
            winToPrint.blocTel.Text = "Tel: " + ser_systeme.findById(1).tel
            + "   Fax: " + ser_systeme.findById(1).fax;
            winToPrint.blocTVA.Text = "TVA:  " + ser_systeme.findById(1).matriculeFiscale + "/" + ser_systeme.findById(1).codeTVA + "/" + ser_systeme.findById(1).codeCategorie + "/" + ser_systeme.findById(1).etbSecondaire;
            winToPrint.blocEmail.Text = "E-mail:  " + ser_systeme.findById(1).email;
            winToPrint.blocTimbre1.Visibility = Visibility.Hidden;
            winToPrint.blocDate.Text = "Date : " + DateBC.Text;
            winToPrint.numPage.Text = "Page : " + curentPage.ToString() + "/" + numTotPage.ToString();

     


        }
    }
}
