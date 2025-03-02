using Domain.Entites;
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
using Ste.Fenetre.FactureAvoirFournisseurFolder;

namespace Ste.Fenetre.FactureAvoirFournisseurFolder
{
    /// <summary>
    /// Interaction logic for Win_FacturationAvoirFournisseur.xaml
    /// </summary>
    public partial class Win_FacturationAvoirFournisseur : Window
    {
        factureAvoirFournisseurService ser_factureAvFr = new factureAvoirFournisseurService();
        AvoirFournisseurService ser_AvFr = new AvoirFournisseurService();
        LigneAvoirFournisseurService ser_LigneAvFr = new LigneAvoirFournisseurService();
        SystemeService ser_systeme = new SystemeService();

        Fournisseur currentFournisseur;
        FactureAvoirFournisseur currentFactureAvoirFour;

        PrintEngine printEng = new PrintEngine();
        SommeEnLettres somme = new SommeEnLettres();
        public Win_FacturationAvoirFournisseur()
        {
            InitializeComponent();
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

                factureAvoirFournisseurDataGrid.ItemsSource = ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur);
                AvoirFrDataGridNonFacturer.ItemsSource = ser_AvFr.findAvoirFournisseurByFournisseurNonFacturer(currentFournisseur);

                label_total_ht_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_H_tva));
                label_total_tva_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_tva));
                label_total_ttc_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_ttc));

                label_total_ht_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_AvFr.findAvoirFournisseurByFournisseurNonFacturer(currentFournisseur).Sum(t => t.tot_H_tva));
                label_total_tva_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_AvFr.findAvoirFournisseurByFournisseurNonFacturer(currentFournisseur).Sum(t => t.tot_tva));
                label_total_ttc_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_AvFr.findAvoirFournisseurByFournisseurNonFacturer(currentFournisseur).Sum(t => t.net_payer));
            }
            catch (Exception)
            {
                MessageBox.Show("Probleme");
            }
           
        }

        private void Ajouter_FactureAvFr_button_Click(object sender, RoutedEventArgs e)
        {
            Win_FactureAvoirFournisseur win = new Win_FactureAvoirFournisseur(null, currentFournisseur);
            win.ShowDialog();
            factureAvoirFournisseurDataGrid.ItemsSource = null;
            factureAvoirFournisseurDataGrid.ItemsSource = ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur);

            label_total_ht_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_H_tva));
            label_total_tva_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_tva));
            label_total_ttc_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_ttc));
        }

        private void Modifier_factureAvFr_button_Click(object sender, RoutedEventArgs e)
        {
            Win_FactureAvoirFournisseur win = new Win_FactureAvoirFournisseur(currentFactureAvoirFour, currentFournisseur);
            win.ShowDialog();
            factureAvoirFournisseurDataGrid.ItemsSource = null;
            factureAvoirFournisseurDataGrid.ItemsSource = ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur);

            label_total_ht_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_H_tva));
            label_total_tva_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_tva));
            label_total_ttc_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_ttc));
        }

        private void Supprimer_factureAvFr_Click(object sender, RoutedEventArgs e)
        {
            if (currentFactureAvoirFour != null && currentFournisseur != null)
            {
                if (MessageBox.Show("Supprimer facture avoir fournisseur", "Supprimer facture", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //do no stuff
                }
                else
                {
                    foreach (var item in ser_AvFr.findAvoirFournisseurBynumFacture(currentFactureAvoirFour))
                    {
                        AvoirFournisseur avFr = ser_AvFr.findAvoirFournisseurByNum(item.Num);
                        avFr.Num_FactureAvoirFournisseur = null;
                        ser_AvFr.editAvoirFournisseur(avFr);
                    }
                    ser_factureAvFr.deleteFactureAvoirFournisseur(currentFactureAvoirFour);
                    currentFactureAvoirFour = null;
                    labelCurrentFactureAvoirFr.Content = null;

                    factureAvoirFournisseurDataGrid.ItemsSource = null;
                    AvoirFrnDataGridDansLaFacture.ItemsSource = null;
                    AvoirFrDataGridNonFacturer.ItemsSource = null;

                    factureAvoirFournisseurDataGrid.ItemsSource = ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur);
                    AvoirFrDataGridNonFacturer.ItemsSource = ser_AvFr.findAvoirFournisseurByFournisseurNonFacturer(currentFournisseur);

                    label_total_ht_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_H_tva));
                    label_total_tva_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_tva));
                    label_total_ttc_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_ttc));

                    label_total_ht_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_AvFr.findAvoirFournisseurByFournisseurNonFacturer(currentFournisseur).Sum(t => t.tot_H_tva));
                    label_total_tva_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_AvFr.findAvoirFournisseurByFournisseurNonFacturer(currentFournisseur).Sum(t => t.tot_tva));
                    label_total_ttc_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_AvFr.findAvoirFournisseurByFournisseurNonFacturer(currentFournisseur).Sum(t => t.net_payer));

                    label_total_ht_dansFacture.Text = null;
                    label_total_tva_dansFacture.Text = null;
                    label_total_ttc_dansFacture.Text = null;
                }
            }
        }

        private void retenuBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void imprimer_selected_facture_button_Click(object sender, RoutedEventArgs e)
        {
            if (currentFactureAvoirFour != null)
            {
                int total_ligne_frombl = 0;
                int total_bl;

                for (int i = 0; i < ser_AvFr.findAvoirFournisseurBynumFacture(currentFactureAvoirFour).Count; i++)
                {
                    int num_bl_tmp = ser_AvFr.findAvoirFournisseurBynumFacture(currentFactureAvoirFour)[i].Num;
                    total_ligne_frombl += ser_LigneAvFr.findLigneAvoirFrByNumAvoirFr(num_bl_tmp).Count;
                }
                total_bl = ser_AvFr.findAvoirFournisseurBynumFacture(currentFactureAvoirFour).Count;
                int N_li_page = (total_ligne_frombl + total_bl) / 34;
                int Res = (total_ligne_frombl + total_bl) % 34;

                charge_Window_to_Print(N_li_page, Res, (total_ligne_frombl + total_bl));
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

        private void Ajouter_AvFr_vers_factureAvFr_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentFactureAvoirFour != null)
                {
                    AvoirFournisseur AvFrToAffect = (AvoirFournisseur)AvoirFrDataGridNonFacturer.SelectedItem;
                    AvoirFournisseur AvFrFromdb = ser_AvFr.findAvoirFournisseurByNum(AvFrToAffect.Num);
                    AvFrFromdb.Num_FactureAvoirFournisseur = currentFactureAvoirFour.Num;
                    ser_AvFr.editAvoirFournisseur(AvFrFromdb);

                    factureAvoirFournisseurDataGrid.ItemsSource = null;
                    AvoirFrnDataGridDansLaFacture.ItemsSource = null;
                    AvoirFrDataGridNonFacturer.ItemsSource = null;
                    factureAvoirFournisseurDataGrid.ItemsSource = ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur);
                    AvoirFrDataGridNonFacturer.ItemsSource = ser_AvFr.findAvoirFournisseurByFournisseurNonFacturer(currentFournisseur);
                    AvoirFrnDataGridDansLaFacture.ItemsSource = ser_factureAvFr.findAvoirFrBynumFactureAvoirFournisseur(currentFactureAvoirFour);

                    label_total_ht_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_H_tva));
                    label_total_tva_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_tva));
                    label_total_ttc_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_ttc));

                    label_total_ht_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_AvFr.findAvoirFournisseurByFournisseurNonFacturer(currentFournisseur).Sum(t => t.tot_H_tva));
                    label_total_tva_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_AvFr.findAvoirFournisseurByFournisseurNonFacturer(currentFournisseur).Sum(t => t.tot_tva));
                    label_total_ttc_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_AvFr.findAvoirFournisseurByFournisseurNonFacturer(currentFournisseur).Sum(t => t.net_payer));

                    label_total_ht_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureAvoirFour.tot_H_tva);
                    label_total_tva_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureAvoirFour.tot_tva);
                    label_total_ttc_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureAvoirFour.tot_ttc);


                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("BL non selectionné");
            }
        }

        private void annuler_AvFr_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentFactureAvoirFour != null)
                {
                    AvoirFournisseur AvFrToAffect = (AvoirFournisseur)AvoirFrnDataGridDansLaFacture.SelectedItem;
                    AvoirFournisseur AvFrFromdb = ser_AvFr.findAvoirFournisseurByNum(AvFrToAffect.Num);

                    AvFrFromdb.Num_FactureAvoirFournisseur = null;
                    ser_AvFr.editAvoirFournisseur(AvFrFromdb);

                    factureAvoirFournisseurDataGrid.ItemsSource = null;
                    AvoirFrnDataGridDansLaFacture.ItemsSource = null;
                    AvoirFrDataGridNonFacturer.ItemsSource = null;
                    factureAvoirFournisseurDataGrid.ItemsSource = ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur);
                    AvoirFrDataGridNonFacturer.ItemsSource = ser_AvFr.findAvoirFournisseurByFournisseurNonFacturer(currentFournisseur);
                    AvoirFrnDataGridDansLaFacture.ItemsSource = ser_AvFr.findAvoirFournisseurBynumFacture(currentFactureAvoirFour);

                    label_total_ht_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_H_tva));
                    label_total_tva_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_tva));
                    label_total_ttc_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_factureAvFr.findFactureAvoirFournisseurByFournisseur(currentFournisseur).Sum(t => t.tot_ttc));

                    label_total_ht_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_AvFr.findAvoirFournisseurByFournisseurNonFacturer(currentFournisseur).Sum(t => t.tot_H_tva));
                    label_total_tva_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_AvFr.findAvoirFournisseurByFournisseurNonFacturer(currentFournisseur).Sum(t => t.tot_tva));
                    label_total_ttc_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_AvFr.findAvoirFournisseurByFournisseurNonFacturer(currentFournisseur).Sum(t => t.net_payer));

                    label_total_ht_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureAvoirFour.tot_H_tva);
                    label_total_tva_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureAvoirFour.tot_tva);
                    label_total_ttc_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureAvoirFour.tot_ttc);

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
            currentFactureAvoirFour = null;
            currentFournisseur = null;
            AvoirFrDataGridNonFacturer.ItemsSource = null;
            AvoirFrnDataGridDansLaFacture.ItemsSource = null;
            factureAvoirFournisseurDataGrid.ItemsSource = null;
            labelCurrentFactureAvoirFr.Content = null;
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
        }
        public void charge_Window_to_Print(int N_li_page, int Res, int all_ligne)
        {
            if (currentFactureAvoirFour != null)
            {

                //string sEnlettre = somme.converti(float.Parse(ADAB.Get_TotalNet__BL_ByNumFacture(selected_facture).ToString()));
                List<Canvas> vc = new List<Canvas>();

                if (N_li_page < 1)
                {
                    WindowToPrint winToPrint = new WindowToPrint();
                    int va = 1;
                    for (int i = 0; i < ser_AvFr.findAvoirFournisseurBynumFacture(currentFactureAvoirFour).Count; i++)
                    {
                        int num_bl_tmp = ser_AvFr.findAvoirFournisseurBynumFacture(currentFactureAvoirFour)[i].Num;
                        long num_bl_fournisseur_tmp = ser_AvFr.findAvoirFournisseurBynumFacture(currentFactureAvoirFour)[i].Num_AvoirFournisseur;
                        string date_livraisonFour = ser_AvFr.findAvoirFournisseurBynumFacture(currentFactureAvoirFour)[i].date.ToShortDateString();
                        printEng.Search(0, va, winToPrint.ArticleGrid).Text = "Avoir F N° " + num_bl_tmp.ToString();
                        printEng.Search(1, va, winToPrint.ArticleGrid).Text = ser_AvFr.findAvoirFournisseurBynumFacture(currentFactureAvoirFour)[i].date.ToShortDateString();
                        printEng.Search(3, va, winToPrint.ArticleGrid).Text = "Fournisseur : ";
                        printEng.Search(5, va, winToPrint.ArticleGrid).Text = num_bl_fournisseur_tmp.ToString();
                        printEng.Search(7, va, winToPrint.ArticleGrid).Text = date_livraisonFour;
                        va++;
                        for (int j = 0; j < ser_LigneAvFr.findLigneAvoirFrByNumAvoirFr(num_bl_tmp).Count; j++)
                        {
                            printEng.Search(0, va, winToPrint.ArticleGrid).Text = ser_LigneAvFr.findLigneAvoirFrByNumAvoirFr(num_bl_tmp)[j].Ref_Produit;
                            printEng.Search(1, va, winToPrint.ArticleGrid).Text = ser_LigneAvFr.findLigneAvoirFrByNumAvoirFr(num_bl_tmp)[j].designation_li;
                            printEng.Search(2, va, winToPrint.ArticleGrid).Text = ser_LigneAvFr.findLigneAvoirFrByNumAvoirFr(num_bl_tmp)[j].qte_li.ToString();
                            printEng.Search(3, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_LigneAvFr.findLigneAvoirFrByNumAvoirFr(num_bl_tmp)[j].prix_HT);
                            printEng.Search(4, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n2}", ser_LigneAvFr.findLigneAvoirFrByNumAvoirFr(num_bl_tmp)[j].remise);
                            printEng.Search(5, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_LigneAvFr.findLigneAvoirFrByNumAvoirFr(num_bl_tmp)[j].tot_HT);
                            printEng.Search(6, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n2}", ser_LigneAvFr.findLigneAvoirFrByNumAvoirFr(num_bl_tmp)[j].tva);
                            printEng.Search(7, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_LigneAvFr.findLigneAvoirFrByNumAvoirFr(num_bl_tmp)[j].tot_TTC);
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
                    for (int i = 0; i < ser_AvFr.findAvoirFournisseurBynumFacture(currentFactureAvoirFour).Count; i++)
                    {
                        int num_bl_tmp = ser_AvFr.findAvoirFournisseurBynumFacture(currentFactureAvoirFour)[i].Num;
                        long num_bl_fournisseur_tmp = ser_AvFr.findAvoirFournisseurBynumFacture(currentFactureAvoirFour)[i].Num_AvoirFournisseur;
                        string date_livraisonFour = ser_AvFr.findAvoirFournisseurBynumFacture(currentFactureAvoirFour)[i].date.ToShortDateString();

                        printEng.Search(0, va, winToPrint.ArticleGrid).Text = "Avoir F N° " + num_bl_tmp.ToString();
                        printEng.Search(1, va, winToPrint.ArticleGrid).Text = ser_AvFr.findAvoirFournisseurBynumFacture(currentFactureAvoirFour)[i].date.ToShortDateString();
                        printEng.Search(3, va, winToPrint.ArticleGrid).Text = "Fournisseur : ";
                        printEng.Search(5, va, winToPrint.ArticleGrid).Text = num_bl_fournisseur_tmp.ToString();
                        printEng.Search(7, va, winToPrint.ArticleGrid).Text = date_livraisonFour;
                        va++;
                        vaa++;

                        for (int j = 0; j < ser_LigneAvFr.findLigneAvoirFrByNumAvoirFr(num_bl_tmp).Count; j++)
                        {
                            printEng.Search(0, va, winToPrint.ArticleGrid).Text = ser_LigneAvFr.findLigneAvoirFrByNumAvoirFr(num_bl_tmp)[j].Ref_Produit;
                            printEng.Search(1, va, winToPrint.ArticleGrid).Text = ser_LigneAvFr.findLigneAvoirFrByNumAvoirFr(num_bl_tmp)[j].designation_li;
                            printEng.Search(2, va, winToPrint.ArticleGrid).Text = ser_LigneAvFr.findLigneAvoirFrByNumAvoirFr(num_bl_tmp)[j].qte_li.ToString();
                            printEng.Search(3, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_LigneAvFr.findLigneAvoirFrByNumAvoirFr(num_bl_tmp)[j].prix_HT);
                            printEng.Search(4, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n2}", ser_LigneAvFr.findLigneAvoirFrByNumAvoirFr(num_bl_tmp)[j].remise);
                            printEng.Search(5, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_LigneAvFr.findLigneAvoirFrByNumAvoirFr(num_bl_tmp)[j].tot_HT);
                            printEng.Search(6, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n2}", ser_LigneAvFr.findLigneAvoirFrByNumAvoirFr(num_bl_tmp)[j].tva);
                            printEng.Search(7, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_LigneAvFr.findLigneAvoirFrByNumAvoirFr(num_bl_tmp)[j].tot_TTC);
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
            string sEnlettre = somme.converti((float)currentFactureAvoirFour.tot_ttc, "Facture Fournisseur");
            winToPrint.blocNomSte.Text = ser_systeme.findById(1).NomSociete;
            winToPrint.blockClient.Text = "Fournisseur : " + currentFournisseur.nom + "\n" + "Code TVA : " + currentFournisseur.matricule + "/" + currentFournisseur.code + "/" + currentFournisseur.code_cat + "/" + currentFournisseur.etb_sec
                + "\n" + "Num facture Fournisseur :" + "\n" + currentFactureAvoirFour.Num_FactureFournisseur + "\n" + "Date fournisseur :" + "\n" + currentFactureAvoirFour.date.ToShortDateString();
            winToPrint.blocNumType.Text = "       Facture N° : " + String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:0000}", currentFactureAvoirFour.Num);
            winToPrint.blocAdresse.Text = ser_systeme.findById(1).adresse;
            winToPrint.blocTel.Text = "Tel: " + ser_systeme.findById(1).tel
            + "   Fax: " + ser_systeme.findById(1).fax;
            winToPrint.blocTVA.Text = "TVA:  " + ser_systeme.findById(1).matriculeFiscale + "/" + ser_systeme.findById(1).codeTVA + "/" + ser_systeme.findById(1).codeCategorie + "/" + ser_systeme.findById(1).etbSecondaire;
            winToPrint.blocEmail.Text = "E-mail:  " + ser_systeme.findById(1).email;


            winToPrint.blocDate.Text = "Date : " + currentFactureAvoirFour.date.ToShortDateString();
            winToPrint.numPage.Text = "Page : " + curentPage.ToString() + "/" + numTotPage.ToString();

            if (manyPage == false)
            {
                winToPrint.blocTotalHT.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureAvoirFour.tot_H_tva);
                winToPrint.bloctotalTva.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureAvoirFour.tot_tva);
                winToPrint.blocTotalNet.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureAvoirFour.tot_ttc);
                winToPrint.blocSomme.Text = sEnlettre;
                winToPrint.blocTimbre.Text =" - "+ ser_systeme.findById(1).Timbre.ToString();
            }

        }

        private void GetSelectedFactureAvoirFR(object sender, MouseButtonEventArgs e)
        {
            currentFactureAvoirFour = (FactureAvoirFournisseur)factureAvoirFournisseurDataGrid.SelectedItem;
            if (currentFournisseur != null && currentFactureAvoirFour != null)
            {
                currentFactureAvoirFour = (FactureAvoirFournisseur)factureAvoirFournisseurDataGrid.SelectedItem;
                labelCurrentFactureAvoirFr.Content = currentFactureAvoirFour.Num.ToString();
                try
                {
                    AvoirFrnDataGridDansLaFacture.ItemsSource = ser_AvFr.findAvoirFournisseurBynumFacture(currentFactureAvoirFour);
                    label_total_ht_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureAvoirFour.tot_H_tva);
                    label_total_tva_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureAvoirFour.tot_tva);
                    label_total_ttc_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFactureAvoirFour.tot_ttc);
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
