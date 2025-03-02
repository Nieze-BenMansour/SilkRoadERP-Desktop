using Domain.Models;
using Service;
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
using Ste.Fenetre.AvoirFinancierFour;
using Domain.Entites;
using Ste.Fenetre.FactureAvoirFournisseurFolder;

namespace Ste.Fenetre
{
    /// <summary>
    /// Interaction logic for Win_FacturationFournisseur.xaml
    /// </summary>
    public partial class Win_FacturationFournisseur : Window
    {
        factureAvoirFournisseurService ser_factureAvoirfour = new factureAvoirFournisseurService();
        AvoirFinancierFournisseurService ser_avoirfinanFour = new AvoirFinancierFournisseurService();
        FactureFournisseurService ser_facture = new FactureFournisseurService();
        BonDeReceptionService ser_bonDeRe = new BonDeReceptionService();
        LigneBonDeRecepctionService ser_LigneBL = new LigneBonDeRecepctionService();
        SystemeService ser_systeme = new SystemeService();

        Fournisseur currentFournisseur;
        FactureFournisseur currentFactureFour;

        PrintEngine printEng = new PrintEngine();
        SommeEnLettres somme = new SommeEnLettres();

        public Win_FacturationFournisseur()
        {
            InitializeComponent();
            currentFournisseur = null;
            currentFactureFour = null;
        }

        private void Ajouter_Facture_button_Click(object sender, RoutedEventArgs e)
        {
            if (currentFournisseur != null)
            {
                Win_Facturefournisseur win = new Win_Facturefournisseur(null, currentFournisseur);
                win.ShowDialog();
                factureFournisseurDataGrid.ItemsSource = null;
                factureFournisseurDataGrid.ItemsSource = ser_facture.findFactureFournisseurByFournisseur(currentFournisseur);

                label_total_ht_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_H_tva));
                label_total_tva_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_tva));
                label_total_ttc_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_ttc));
            }
        }

        private void Modifier_facture_button_Click(object sender, RoutedEventArgs e)
        {
            if (currentFournisseur != null && currentFactureFour != null)
            {
                Win_Facturefournisseur win = new Win_Facturefournisseur(currentFactureFour, currentFournisseur);
                win.ShowDialog();
                factureFournisseurDataGrid.ItemsSource = null;
                factureFournisseurDataGrid.ItemsSource = ser_facture.findFactureFournisseurByFournisseur(currentFournisseur);

                label_total_ht_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_H_tva));
                label_total_tva_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_tva));
                label_total_ttc_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_ttc));
            }
        }

        private void Supprimer_facture_Click(object sender, RoutedEventArgs e)
        {
            if (currentFactureFour != null && currentFournisseur != null)
            {
                if (MessageBox.Show("Supprimer facture", "Supprimer facture", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //do no stuff
                }
                else
                {
                    foreach (var item in ser_bonDeRe.findBonDeReceptionBynumFactureFour(currentFactureFour))
                    {
                        BonDeReception bon = ser_bonDeRe.findBonDeReceptionByNum(item.Num);
                        bon.Num_Facture_fournisseur = null;
                        ser_bonDeRe.editBonDeReception(bon);
                    }
                    ser_facture.deleteFactureFournisseur(currentFactureFour);
                    currentFactureFour = null;
                    labelCurrentFacture.Content = null;

                    factureFournisseurDataGrid.ItemsSource = null;
                    bonDeReceptionDataGridDansLaFacture.ItemsSource = null;
                    bonDeReceptionDataGridNonFacturer.ItemsSource = null;

                    factureFournisseurDataGrid.ItemsSource = ser_facture.findFactureFournisseurByFournisseur(currentFournisseur);
                    bonDeReceptionDataGridNonFacturer.ItemsSource = ser_bonDeRe.findBonDeReceptionByFournisseurNonFacturer(currentFournisseur);

                    label_total_ht_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_H_tva));
                    label_total_tva_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_tva));
                    label_total_ttc_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_ttc));

                    label_total_ht_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeRe.findBonDeReceptionByFournisseurNonFacturer(currentFournisseur).Sum(t => t.tot_H_tva));
                    label_total_tva_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeRe.findBonDeReceptionByFournisseurNonFacturer(currentFournisseur).Sum(t => t.tot_tva));
                    label_total_ttc_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeRe.findBonDeReceptionByFournisseurNonFacturer(currentFournisseur).Sum(t => t.net_payer));

                    label_total_ht_dansFacture.Text = null;
                    label_total_tva_dansFacture.Text = null;
                    label_total_ttc_dansFacture.Text = null;
                }
            }
        }

        private void Fournisseurbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clearFields();
                GetFournisseur win = new GetFournisseur();
                win.ShowDialog();
                currentFournisseur = win.fournisseurToSend;

                CodeFourTextBlock.Text = currentFournisseur.id.ToString();
                FournisseurTextBlock.Text = currentFournisseur.nom;
                checkBoxConstructeur.IsChecked = currentFournisseur.constructeur;

                factureFournisseurDataGrid.ItemsSource = ser_facture.findFactureFournisseurByFournisseur(currentFournisseur);
                bonDeReceptionDataGridNonFacturer.ItemsSource = ser_bonDeRe.findBonDeReceptionByFournisseurNonFacturer(currentFournisseur);

                label_total_ht_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_H_tva));
                label_total_tva_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_tva));
                label_total_ttc_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_ttc));

                label_total_ht_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeRe.findBonDeReceptionByFournisseurNonFacturer(currentFournisseur).Sum(t => t.tot_H_tva));
                label_total_tva_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeRe.findBonDeReceptionByFournisseurNonFacturer(currentFournisseur).Sum(t => t.tot_tva));
                label_total_ttc_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeRe.findBonDeReceptionByFournisseurNonFacturer(currentFournisseur).Sum(t => t.net_payer));
            }
            catch (Exception)
            {

            }
        }

        private void imprimer_selected_facture_button_Click(object sender, RoutedEventArgs e)
        {
            if (currentFactureFour != null)
            {
                int total_ligne_frombl = 0;
                int total_bl;

                for (int i = 0; i < ser_bonDeRe.findBonDeReceptionBynumFactureFour(currentFactureFour).Count; i++)
                {
                    int num_bl_tmp = ser_bonDeRe.findBonDeReceptionBynumFactureFour(currentFactureFour)[i].Num;
                    total_ligne_frombl += ser_LigneBL.findLigneBonReceptionByNumBC(num_bl_tmp).Count;
                }
                total_bl = ser_bonDeRe.findBonDeReceptionBynumFactureFour(currentFactureFour).Count;
                int N_li_page = (total_ligne_frombl + total_bl) / 34;
                int Res = (total_ligne_frombl + total_bl) % 34;

                charge_Window_to_Print(N_li_page, Res, (total_ligne_frombl + total_bl));
            }
        }




        public void charge_Window_to_Print(int N_li_page, int Res, int all_ligne)
        {
            if (currentFactureFour != null)
            {

                //string sEnlettre = somme.converti(float.Parse(ADAB.Get_TotalNet__BL_ByNumFacture(selected_facture).ToString()));
                List<Canvas> vc = new List<Canvas>();

                if (N_li_page < 1)
                {
                    WindowToPrint winToPrint = new WindowToPrint();
                    int va = 1;
                    for (int i = 0; i < ser_bonDeRe.findBonDeReceptionBynumFactureFour(currentFactureFour).Count; i++)
                    {
                        int num_bl_tmp = ser_bonDeRe.findBonDeReceptionBynumFactureFour(currentFactureFour)[i].Num;
                        long num_bl_fournisseur_tmp = ser_bonDeRe.findBonDeReceptionBynumFactureFour(currentFactureFour)[i].num_BonFournisseur;
                        string date_livraisonFour = ser_bonDeRe.findBonDeReceptionBynumFactureFour(currentFactureFour)[i].dateDeLivraison.ToShortDateString();
                        printEng.Search(0, va, winToPrint.ArticleGrid).Text = "BR N° " + num_bl_tmp.ToString();
                        printEng.Search(1, va, winToPrint.ArticleGrid).Text = ser_bonDeRe.findBonDeReceptionBynumFactureFour(currentFactureFour)[i].date.ToShortDateString();
                        printEng.Search(3, va, winToPrint.ArticleGrid).Text = "Fournisseur : ";
                        printEng.Search(5, va, winToPrint.ArticleGrid).Text = num_bl_fournisseur_tmp.ToString();
                        printEng.Search(7, va, winToPrint.ArticleGrid).Text = date_livraisonFour;
                        va++;
                        for (int j = 0; j < ser_LigneBL.findLigneBonReceptionByNumBC(num_bl_tmp).Count; j++)
                        {
                            printEng.Search(0, va, winToPrint.ArticleGrid).Text = ser_LigneBL.findLigneBonReceptionByNumBC(num_bl_tmp)[j].Ref_Produit;
                            printEng.Search(1, va, winToPrint.ArticleGrid).Text = ser_LigneBL.findLigneBonReceptionByNumBC(num_bl_tmp)[j].designation_li;
                            printEng.Search(2, va, winToPrint.ArticleGrid).Text = ser_LigneBL.findLigneBonReceptionByNumBC(num_bl_tmp)[j].qte_li.ToString();
                            printEng.Search(3, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_LigneBL.findLigneBonReceptionByNumBC(num_bl_tmp)[j].prix_HT);
                            printEng.Search(4, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n2}", ser_LigneBL.findLigneBonReceptionByNumBC(num_bl_tmp)[j].remise);
                            printEng.Search(5, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_LigneBL.findLigneBonReceptionByNumBC(num_bl_tmp)[j].tot_HT);
                            printEng.Search(6, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n2}", ser_LigneBL.findLigneBonReceptionByNumBC(num_bl_tmp)[j].tva);
                            printEng.Search(7, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_LigneBL.findLigneBonReceptionByNumBC(num_bl_tmp)[j].tot_TTC);
                            va++;
                        }
                    }

                    SetWindowToPrintBL(winToPrint, false, 1, 1);
                    winToPrint.Close();
                    vc.Add(winToPrint.CanvasToPrint);

                }
                if (N_li_page >= 1)
                {
                    int nbrePageConcraite, currentPage = 1;
                    if (Res == 0)
                    {
                        nbrePageConcraite = N_li_page;
                    }
                    else
                    {
                        nbrePageConcraite = N_li_page + 1;
                    }
                    bool hleg = false;
                    WindowToPrint winToPrint = new WindowToPrint();
                    int va = 1, vaa = 0;
                    for (int i = 0; i < ser_bonDeRe.findBonDeReceptionBynumFactureFour(currentFactureFour).Count; i++)
                    {
                        int num_bl_tmp = ser_bonDeRe.findBonDeReceptionBynumFactureFour(currentFactureFour)[i].Num;
                        long num_bl_fournisseur_tmp = ser_bonDeRe.findBonDeReceptionBynumFactureFour(currentFactureFour)[i].num_BonFournisseur;
                        string date_livraisonFour = ser_bonDeRe.findBonDeReceptionBynumFactureFour(currentFactureFour)[i].dateDeLivraison.ToShortDateString();

                        printEng.Search(0, va, winToPrint.ArticleGrid).Text = "BR N° " + num_bl_tmp.ToString();
                        printEng.Search(1, va, winToPrint.ArticleGrid).Text = ser_bonDeRe.findBonDeReceptionBynumFactureFour(currentFactureFour)[i].date.ToShortDateString();
                        printEng.Search(3, va, winToPrint.ArticleGrid).Text = "Fournisseur : ";
                        printEng.Search(5, va, winToPrint.ArticleGrid).Text = num_bl_fournisseur_tmp.ToString();
                        printEng.Search(7, va, winToPrint.ArticleGrid).Text = date_livraisonFour;
                        va++;
                        vaa++;

                        for (int j = 0; j < ser_LigneBL.findLigneBonReceptionByNumBC(num_bl_tmp).Count; j++)
                        {
                            printEng.Search(0, va, winToPrint.ArticleGrid).Text = ser_LigneBL.findLigneBonReceptionByNumBC(num_bl_tmp)[j].Ref_Produit;
                            printEng.Search(1, va, winToPrint.ArticleGrid).Text = ser_LigneBL.findLigneBonReceptionByNumBC(num_bl_tmp)[j].designation_li;
                            printEng.Search(2, va, winToPrint.ArticleGrid).Text = ser_LigneBL.findLigneBonReceptionByNumBC(num_bl_tmp)[j].qte_li.ToString();
                            printEng.Search(3, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_LigneBL.findLigneBonReceptionByNumBC(num_bl_tmp)[j].prix_HT);
                            printEng.Search(4, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n2}", ser_LigneBL.findLigneBonReceptionByNumBC(num_bl_tmp)[j].remise);
                            printEng.Search(5, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_LigneBL.findLigneBonReceptionByNumBC(num_bl_tmp)[j].tot_HT);
                            printEng.Search(6, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n2}", ser_LigneBL.findLigneBonReceptionByNumBC(num_bl_tmp)[j].tva);
                            printEng.Search(7, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_LigneBL.findLigneBonReceptionByNumBC(num_bl_tmp)[j].tot_TTC);
                            va++;
                            vaa++;
                            if ((va - 1) % 34 == 0)
                            {
                                if (vaa == all_ligne)
                                {
                                    SetWindowToPrintBL(winToPrint, false, currentPage, nbrePageConcraite);
                                    hleg = true;

                                }
                                if (vaa < all_ligne)
                                {
                                    SetWindowToPrintBL(winToPrint, true, currentPage, nbrePageConcraite);
                                    currentPage++;
                                }

                                vc.Add(winToPrint.CanvasToPrint);
                                winToPrint.Close();
                                winToPrint = new WindowToPrint();
                                va = 1;
                            }

                        }


                    }
                    if (hleg == false)
                    {
                        SetWindowToPrintBL(winToPrint, false, currentPage, nbrePageConcraite);
                        vc.Add(winToPrint.CanvasToPrint);
                    }
                    winToPrint.Close();


                }

                printEng.Export(vc);
                /* printEng.Export(new Uri(Directory.GetCurrentDirectory().ToString() + "\\Facture_Fournisseur\\" + "facture_Fournisseur-" + currentFactureFour.Num.ToString() + "___" + String.Format("{0:d-M-yyyy}", currentFactureFour.date) + ".xps"), vc);
                 //fenetre contient le Document View
                 WindowBeforePrint wi = new WindowBeforePrint();
                 XpsDocument xpsDocument = new XpsDocument(Directory.GetCurrentDirectory().ToString() + "\\Facture_Fournisseur\\" + "facture_Fournisseur-" + currentFactureFour.Num.ToString() + "___" + String.Format("{0:d-M-yyyy}", currentFactureFour.date) + ".xps", FileAccess.Read);
                 wi.document_a_print.Document = xpsDocument.GetFixedDocumentSequence();
                 wi.Show();
                 xpsDocument.Close();*/

            }

        }

        public void SetWindowToPrintBL(WindowToPrint winToPrint, bool manyPage, int curentPage, int numTotPage)
        {
            string sEnlettre = somme.converti((float)currentFactureFour.tot_ttc, "Facture Fournisseur");
            winToPrint.blocNomSte.Text = ser_systeme.findById(1).NomSociete;
            winToPrint.blockClient.Text = "Fournisseur : " + currentFournisseur.nom + "\n" + "Code TVA : " + currentFournisseur.matricule + "/" + currentFournisseur.code + "/" + currentFournisseur.code_cat + "/" + currentFournisseur.etb_sec
                + "\n" + "Num facture Fournisseur :" + "\n" + currentFactureFour.NumFactureFournisseur + "\n" + "Date fournisseur :" + "\n" + currentFactureFour.dateFacturationFournisseur.ToShortDateString();
            winToPrint.blocNumType.Text = "       Facture N° : " + String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:0000}", currentFactureFour.Num);
            winToPrint.blocAdresse.Text = ser_systeme.findById(1).adresse;
            winToPrint.blocTel.Text = "Tel: " + ser_systeme.findById(1).tel
            + "   Fax: " + ser_systeme.findById(1).fax;
            winToPrint.blocTVA.Text = "TVA:  " + ser_systeme.findById(1).matriculeFiscale + "/" + ser_systeme.findById(1).codeTVA + "/" + ser_systeme.findById(1).codeCategorie + "/" + ser_systeme.findById(1).etbSecondaire;
            winToPrint.blocEmail.Text = "E-mail:  " + ser_systeme.findById(1).email;


            winToPrint.blocDate.Text = "Date : " + currentFactureFour.date.ToShortDateString();
            winToPrint.numPage.Text = "Page : " + curentPage.ToString() + "/" + numTotPage.ToString();

            if (manyPage == false)
            {
                winToPrint.blocTotalHT.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureFour.tot_H_tva);
                winToPrint.bloctotalTva.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureFour.tot_tva);
                winToPrint.blocTotalNet.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureFour.tot_ttc);
                winToPrint.blocSomme.Text = sEnlettre;
                winToPrint.blocTimbre.Text = ser_systeme.findById(1).Timbre.ToString();
            }

        }



        private void Actualiser_button_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
        }

        private void FermerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Ajouter_Bl_vers_facture_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentFactureFour != null)
                {
                    BonDeReception bonToAffect = (BonDeReception)bonDeReceptionDataGridNonFacturer.SelectedItem;
                    BonDeReception bonFromdb = ser_bonDeRe.findBonDeReceptionByNum(bonToAffect.Num);
                    bonFromdb.Num_Facture_fournisseur = currentFactureFour.Num;
                    ser_bonDeRe.editBonDeReception(bonFromdb);

                    factureFournisseurDataGrid.ItemsSource = null;
                    bonDeReceptionDataGridDansLaFacture.ItemsSource = null;
                    bonDeReceptionDataGridNonFacturer.ItemsSource = null;
                    factureFournisseurDataGrid.ItemsSource = ser_facture.findFactureFournisseurByFournisseur(currentFournisseur);
                    bonDeReceptionDataGridNonFacturer.ItemsSource = ser_bonDeRe.findBonDeReceptionByFournisseurNonFacturer(currentFournisseur);
                    bonDeReceptionDataGridDansLaFacture.ItemsSource = ser_bonDeRe.findBonDeReceptionBynumFactureFour(currentFactureFour);

                    label_total_ht_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_H_tva));
                    label_total_tva_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_tva));
                    label_total_ttc_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_ttc));

                    label_total_ht_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeRe.findBonDeReceptionByFournisseurNonFacturer(currentFournisseur).Sum(t => t.tot_H_tva));
                    label_total_tva_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeRe.findBonDeReceptionByFournisseurNonFacturer(currentFournisseur).Sum(t => t.tot_tva));
                    label_total_ttc_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeRe.findBonDeReceptionByFournisseurNonFacturer(currentFournisseur).Sum(t => t.net_payer));

                    label_total_ht_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureFour.tot_H_tva);
                    label_total_tva_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureFour.tot_tva);
                    label_total_ttc_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureFour.tot_ttc);


                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("BL non selectionné");
            }
        }

        private void annuler_bl_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentFactureFour != null)
                {
                    BonDeReception bonToAffect = (BonDeReception)bonDeReceptionDataGridDansLaFacture.SelectedItem;
                    BonDeReception bonFromdb = ser_bonDeRe.findBonDeReceptionByNum(bonToAffect.Num);

                    bonFromdb.Num_Facture_fournisseur = null;
                    ser_bonDeRe.editBonDeReception(bonFromdb);

                    factureFournisseurDataGrid.ItemsSource = null;
                    bonDeReceptionDataGridDansLaFacture.ItemsSource = null;
                    bonDeReceptionDataGridNonFacturer.ItemsSource = null;
                    factureFournisseurDataGrid.ItemsSource = ser_facture.findFactureFournisseurByFournisseur(currentFournisseur);
                    bonDeReceptionDataGridNonFacturer.ItemsSource = ser_bonDeRe.findBonDeReceptionByFournisseurNonFacturer(currentFournisseur);
                    bonDeReceptionDataGridDansLaFacture.ItemsSource = ser_bonDeRe.findBonDeReceptionBynumFactureFour(currentFactureFour);

                    label_total_ht_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_H_tva));
                    label_total_tva_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_tva));
                    label_total_ttc_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_ttc));

                    label_total_ht_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeRe.findBonDeReceptionByFournisseurNonFacturer(currentFournisseur).Sum(t => t.tot_H_tva));
                    label_total_tva_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeRe.findBonDeReceptionByFournisseurNonFacturer(currentFournisseur).Sum(t => t.tot_tva));
                    label_total_ttc_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeRe.findBonDeReceptionByFournisseurNonFacturer(currentFournisseur).Sum(t => t.net_payer));

                    label_total_ht_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureFour.tot_H_tva);
                    label_total_tva_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureFour.tot_tva);
                    label_total_ttc_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureFour.tot_ttc);

                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("BL non selectionné");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            clearFields();
        }
        void clearFields()
        {
            currentFactureFour = null;
            currentFournisseur = null;
            bonDeReceptionDataGridNonFacturer.ItemsSource = null;
            bonDeReceptionDataGridDansLaFacture.ItemsSource = null;
            factureFournisseurDataGrid.ItemsSource = null;
            labelCurrentFacture.Content = null;
            CodeFourTextBlock.Text = null;
            FournisseurTextBlock.Text = "Fournisseur non selectionné";

            label_total_ht_facture.Text = null;
            label_total_tva_facture.Text = null;
            label_total_ttc_facture.Text = null;

            label_total_ht_nonFacture.Text = null;
            label_total_tva_nonFacture.Text = null;
            label_total_ttc_nonFacture.Text = null;

            label_total_ht_dansFacture.Text = null;
            label_total_tva_dansFacture.Text = null;
            label_total_ttc_dansFacture.Text = null;


            label_FactureavoirFour.Text = null;
            label_avoirFinancierFour.Text = null;
        }

        private void GetSelectedFacture(object sender, MouseButtonEventArgs e)
        {
            currentFactureFour = (FactureFournisseur)factureFournisseurDataGrid.SelectedItem;
            if (currentFournisseur != null && currentFactureFour != null)
            {
                currentFactureFour = (FactureFournisseur)factureFournisseurDataGrid.SelectedItem;
                labelCurrentFacture.Content = currentFactureFour.Num.ToString();
                try
                {
                    bonDeReceptionDataGridDansLaFacture.ItemsSource = ser_bonDeRe.findBonDeReceptionBynumFactureFour(currentFactureFour);
                    label_total_ht_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureFour.tot_H_tva);
                    label_total_tva_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureFour.tot_tva);
                    label_total_ttc_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureFour.tot_ttc);

                    if (ser_factureAvoirfour.findFactureAvoirFournisseurByNumFactureFour(currentFactureFour) != null)
                    {
                        label_FactureavoirFour.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvoirfour.findFactureAvoirFournisseurByNumFactureFour(currentFactureFour).tot_ttc);
                    }
                    else label_FactureavoirFour.Text = null;
                    if (ser_avoirfinanFour.findAvoirFinancierFournisseurByNumFactureFour(currentFactureFour) != null)
                    {
                        label_avoirFinancierFour.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_avoirfinanFour.findAvoirFinancierFournisseurByNumFactureFour(currentFactureFour).tot_ttc);
                    }
                    else label_avoirFinancierFour.Text = null;
                }
                catch (Exception)
                {

                }
            }
        }

        private void retenuBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentFactureFour != null && currentFournisseur != null)
            {
                charge_Window_to_Print_retenue(currentFactureFour);
            }
        }

        public void charge_Window_to_Print_retenue(FactureFournisseur item)
        {
            if (currentFactureFour != null && currentFournisseur != null)
            {
                FactureAvoirFournisseur facAvoirTmp = null;
                AvoirFinancierFournisseur avFinFourTmp = null;
                List<Canvas> vc = new List<Canvas>();
                decimal tot_net_tmp = 0;
                WindowToPrintRetenueAlaSource winToPrint = new WindowToPrintRetenueAlaSource();

                printEng.Search(0, 0, winToPrint.Gridna).Text = item.NumFactureFournisseur.ToString();
                printEng.Search(1, 0, winToPrint.Gridna).Text = item.dateFacturationFournisseur.ToShortDateString();
                printEng.Search(2, 0, winToPrint.Gridna).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", item.tot_ttc);

                tot_net_tmp = item.tot_ttc;

                if (ser_avoirfinanFour.findAvoirFinancierFournisseurByNumFactureFour(currentFactureFour) != null)
                {
                    avFinFourTmp = ser_avoirfinanFour.findAvoirFinancierFournisseurByNumFactureFour(currentFactureFour);
                    printEng.Search(0, 1, winToPrint.Gridna).Text = "Avoir financier :\n" + avFinFourTmp.NumSurPage.ToString();
                    printEng.Search(1, 1, winToPrint.Gridna).Text = avFinFourTmp.date.ToShortDateString();
                    printEng.Search(2, 1, winToPrint.Gridna).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", avFinFourTmp.tot_ttc);
                    tot_net_tmp -= avFinFourTmp.tot_ttc;
                }
                if (ser_factureAvoirfour.findFactureAvoirFournisseurByNumFactureFour(currentFactureFour) != null)
                {
                    facAvoirTmp = ser_factureAvoirfour.findFactureAvoirFournisseurByNumFactureFour(currentFactureFour);
                    printEng.Search(0, 2, winToPrint.Gridna).Text = "Facture avoir :\n" + facAvoirTmp.Num_FactureAvoirFourSurPAge.ToString();
                    printEng.Search(1, 2, winToPrint.Gridna).Text = facAvoirTmp.date.ToShortDateString();
                    printEng.Search(2, 2, winToPrint.Gridna).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", facAvoirTmp.tot_ttc);
                    tot_net_tmp -= facAvoirTmp.tot_ttc;
                }
                winToPrint.totBrut.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", tot_net_tmp);
                double x = Convert.ToDouble(tot_net_tmp);
                double ret = (x * ser_systeme.findById(1).pourcentageRetenu) / 100;
                decimal y = Convert.ToDecimal(ret);
                tot_net_tmp = (tot_net_tmp - y);


                winToPrint.totRetenue.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", y);
                winToPrint.totNet.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", tot_net_tmp);


                SetWindowToPrintBL(winToPrint);
                winToPrint.Close();
                vc.Add(winToPrint.canvasToPrint);

                printEng.Export(vc);
                /*         printEng.Export(new Uri(Directory.GetCurrentDirectory().ToString() + "\\Retenue\\" + "retenue.xps"), vc);
                         //fenetre contient le Document View
                         WindowBeforePrint wi = new WindowBeforePrint();
                         XpsDocument xpsDocument = new XpsDocument(Directory.GetCurrentDirectory().ToString() + "\\Retenue\\" + "retenue.xps", FileAccess.Read);
                         wi.document_a_print.Document = xpsDocument.GetFixedDocumentSequence();
                         wi.Show();
                         xpsDocument.Close();*/
            }
        }
        public void SetWindowToPrintBL(WindowToPrintRetenueAlaSource winToPrint)
        {
            
            winToPrint.NomPayeur.Text = ser_systeme.findById(1).NomSociete;
            winToPrint.adressPayeur.Text = ser_systeme.findById(1).adresse;
            winToPrint.MatriculePayeur.Text = ser_systeme.findById(1).matriculeFiscale;
            winToPrint.CodeTVAPayeur.Text = ser_systeme.findById(1).codeTVA;
            winToPrint.CodeCatePayeur.Text = ser_systeme.findById(1).codeCategorie;
            winToPrint.etbSecPayeur.Text = ser_systeme.findById(1).etbSecondaire;

            winToPrint.NomBeni.Text = currentFournisseur.nom;
            winToPrint.MatriculeBeni.Text = currentFournisseur.matricule;
            winToPrint.CodeTVABeni.Text = currentFournisseur.code;
            winToPrint.CodeCateBeni.Text = currentFournisseur.code_cat;
            winToPrint.etbSecBeni.Text = currentFournisseur.etb_sec;
            winToPrint.addresseRetenuPayeur.Text = ser_systeme.findById(1).adresseRetenu;
            winToPrint.adresseBeni.Text = currentFournisseur.adresse;
            winToPrint.DateRetenuPayeur_Copy.Text = DateTime.Now.ToShortDateString();

            winToPrint.TauxRetenu.Text = "Retenue "+ser_systeme.findById(1).pourcentageRetenu;
        }

        private void AvoirFinancierBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentFactureFour != null && currentFactureFour != null)
            {
                AvoirFinancierFournisseur avFrFin = ser_avoirfinanFour.findAvoirFinancierFournisseurByNum(currentFactureFour.Num);
                if (currentFactureFour != null && avFrFin == null)
                {
                    Win_ManageAvoirFinancierFournisseur win = new Win_ManageAvoirFinancierFournisseur(currentFactureFour, null);
                    win.ShowDialog();
                }
                else if (currentFactureFour != null && avFrFin != null)
                {
                    Win_ManageAvoirFinancierFournisseur win = new Win_ManageAvoirFinancierFournisseur(currentFactureFour, avFrFin);
                    win.ShowDialog();
                } 
       

            if (ser_factureAvoirfour.findFactureAvoirFournisseurByNumFactureFour(currentFactureFour) != null)
            {
                label_FactureavoirFour.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvoirfour.findFactureAvoirFournisseurByNumFactureFour(currentFactureFour).tot_ttc); 
            }
            else
            {
                label_FactureavoirFour.Text = null;
            }
            if (ser_avoirfinanFour.findAvoirFinancierFournisseurByNumFactureFour(currentFactureFour) != null)
            {
                label_avoirFinancierFour.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_avoirfinanFour.findAvoirFinancierFournisseurByNumFactureFour(currentFactureFour).tot_ttc); 
            }
            else
            {
                label_avoirFinancierFour.Text = null;
            }
            }
        }

        private void AvoirBtn_Click(object sender, RoutedEventArgs e)
        {
            if(currentFournisseur != null && currentFactureFour != null)
            {
                FactureAvoirFournisseur facAvoirFour = ser_factureAvoirfour.findFactureAvoirFournisseurByNumFactureFour(currentFactureFour);
                Win_GetFactureAvoirFournisseur win = new Win_GetFactureAvoirFournisseur(currentFournisseur,facAvoirFour,currentFactureFour);
                win.ShowDialog();
            if (ser_factureAvoirfour.findFactureAvoirFournisseurByNumFactureFour(currentFactureFour) != null)
            {
                label_FactureavoirFour.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvoirfour.findFactureAvoirFournisseurByNumFactureFour(currentFactureFour).tot_ttc);
            }
            else
            {
                label_FactureavoirFour.Text = null;
            }
            if (ser_avoirfinanFour.findAvoirFinancierFournisseurByNumFactureFour(currentFactureFour) != null)
            {
                label_avoirFinancierFour.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_avoirfinanFour.findAvoirFinancierFournisseurByNumFactureFour(currentFactureFour).tot_ttc);
            }
            else
            {
                label_avoirFinancierFour.Text = null;
            }
            }
        }
    }
}
