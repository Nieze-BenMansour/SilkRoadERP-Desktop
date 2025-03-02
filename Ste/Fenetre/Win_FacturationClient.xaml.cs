using Service;
using Domain.Models;
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
using System.IO;
using System.Windows.Xps.Packaging;
using System.Collections;
using System.Globalization;

namespace Ste.Fenetre
{
    /// <summary>
    /// Interaction logic for Win_FacturationClient.xaml
    /// </summary>
    public partial class Win_FacturationClient : Window
    {
        FactureService ser_facture = new FactureService();
        BonDeLivraisonService ser_bonDeLiv = new BonDeLivraisonService();
        LigneBLService ser_LigneBL = new LigneBLService();
        SystemeService ser_systeme = new SystemeService();

        Client currentClient;
        Facture currentFacture;

        PrintEngine printEng = new PrintEngine();
        SommeEnLettres somme = new SommeEnLettres();
        public Win_FacturationClient()
        {
            InitializeComponent();
            
        }

        private void FermerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            clearFields();
        }

        private void Client_Button_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
            GetClient win = new GetClient();
            win.ShowDialog();
            currentClient = win.clientToSend;

            codeClient.Text = currentClient.Id.ToString();
            nomClient.Text = currentClient.nom;

            factureDataGrid.ItemsSource = ser_facture.findFactureByClient(currentClient);
            bonDeLivraisonDataGrid_NonFacturer.ItemsSource = ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient);

            label_total_ht_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_H_tva));
            label_total_tva_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_tva));
            label_total_ttc_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_ttc));

            label_total_ht_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient).Sum(t => t.tot_H_tva));
            label_total_tva_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient).Sum(t => t.tot_tva));
            label_total_ttc_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient).Sum(t => t.net_payer));
        }

        private void GetSelectedFacture(object sender, MouseButtonEventArgs e)
        {
            if (currentClient != null)
            {
                try
                {
                    currentFacture = (Facture)factureDataGrid.SelectedItem;
                labelCurrentFacture.Content = currentFacture.Num.ToString();
               
                    bonDeLivraisonDataGrid_DansFacture.ItemsSource = ser_bonDeLiv.findBonDeLivraisonBynumFacture(currentFacture);
                    label_total_ht_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFacture.tot_H_tva);
                    label_total_tva_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFacture.tot_tva);
                    label_total_ttc_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFacture.tot_ttc);
                }
                catch (Exception)
                {

                }
            }
        }

        private void Actualiser_button_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
        }
        private void clearFields()
        {
            currentClient = null;
            currentFacture = null;
            bonDeLivraisonDataGrid_DansFacture.ItemsSource = null;
            bonDeLivraisonDataGrid_NonFacturer.ItemsSource = null;
            factureDataGrid.ItemsSource = null;
            labelCurrentFacture.Content = null;
            codeClient.Text = null;
            nomClient.Text = null;

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

        private void Ajouter_Bl_vers_facture_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentFacture != null)
                {
                    /*   BonDeLivraison bonToAffect = (BonDeLivraison)bonDeLivraisonDataGrid_NonFacturer.SelectedItem;
                       BonDeLivraison bonFromdb = ser_bonDeLiv.findBonDeLivraisonByNum(bonToAffect.Num);
                       bonFromdb.Num_Facture = currentFacture.Num;
                       ser_bonDeLiv.editBonDeLivraison(bonFromdb);*/
                    IList  listeToAffect = bonDeLivraisonDataGrid_NonFacturer.SelectedItems;
                    foreach(var item in listeToAffect)
                    {
                        BonDeLivraison tmmp = (BonDeLivraison)item;
                        BonDeLivraison bonFromdb = ser_bonDeLiv.findBonDeLivraisonByNum(tmmp.Num);
                        bonFromdb.Num_Facture = currentFacture.Num;
                        ser_bonDeLiv.editBonDeLivraison(bonFromdb);
                    }

                    factureDataGrid.ItemsSource = null;
                    bonDeLivraisonDataGrid_DansFacture.ItemsSource = null;
                    bonDeLivraisonDataGrid_NonFacturer.ItemsSource = null;
                    factureDataGrid.ItemsSource = ser_facture.findFactureByClient(currentClient);
                    bonDeLivraisonDataGrid_NonFacturer.ItemsSource = ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient);
                    bonDeLivraisonDataGrid_DansFacture.ItemsSource = ser_bonDeLiv.findBonDeLivraisonBynumFacture(currentFacture);

                    label_total_ht_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_H_tva));
                    label_total_tva_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_tva));
                    label_total_ttc_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_ttc));

                    label_total_ht_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient).Sum(t => t.tot_H_tva));
                    label_total_tva_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient).Sum(t => t.tot_tva));
                    label_total_ttc_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient).Sum(t => t.net_payer));

                    label_total_ht_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFacture.tot_H_tva);
                    label_total_tva_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFacture.tot_tva);
                    label_total_ttc_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFacture.tot_ttc);


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
                if (currentFacture != null)
                {

                    IList listeTodelete = bonDeLivraisonDataGrid_DansFacture.SelectedItems;
                    foreach (var item in listeTodelete)
                    {
                        BonDeLivraison tmmp = (BonDeLivraison)item;
                        BonDeLivraison bonFromdb = ser_bonDeLiv.findBonDeLivraisonByNum(tmmp.Num);
                        bonFromdb.Num_Facture = null;
                        ser_bonDeLiv.editBonDeLivraison(bonFromdb);
                    }

    

                    factureDataGrid.ItemsSource = null;
                    bonDeLivraisonDataGrid_DansFacture.ItemsSource = null;
                    bonDeLivraisonDataGrid_NonFacturer.ItemsSource = null;
                    factureDataGrid.ItemsSource = ser_facture.findFactureByClient(currentClient);
                    bonDeLivraisonDataGrid_NonFacturer.ItemsSource = ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient);
                    bonDeLivraisonDataGrid_DansFacture.ItemsSource = ser_bonDeLiv.findBonDeLivraisonBynumFacture(currentFacture);

                    label_total_ht_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_H_tva));
                    label_total_tva_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_tva));
                    label_total_ttc_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_ttc));

                    label_total_ht_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient).Sum(t => t.tot_H_tva));
                    label_total_tva_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient).Sum(t => t.tot_tva));
                    label_total_ttc_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient).Sum(t => t.net_payer));

                    label_total_ht_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFacture.tot_H_tva);
                    label_total_tva_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFacture.tot_tva);
                    label_total_ttc_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFacture.tot_ttc);

                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("BL non selectionné");
            }
        }

        private void Ajouter_Facture_button_Click(object sender, RoutedEventArgs e)
        {
            if (currentClient != null)
            {
                Window_Facture_A_M win = new Window_Facture_A_M(null, currentClient);
                win.ShowDialog();
                factureDataGrid.ItemsSource = null;
                factureDataGrid.ItemsSource = ser_facture.findFactureByClient(currentClient);

                label_total_ht_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_H_tva));
                label_total_tva_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_tva));
                label_total_ttc_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_ttc));
            }
        }

        private void Modifier_facture_button_Click(object sender, RoutedEventArgs e)
        {
            if (currentClient != null && currentFacture != null)
            {
                Window_Facture_A_M win = new Window_Facture_A_M(currentFacture, currentClient);
                win.ShowDialog();
                factureDataGrid.ItemsSource = null;
                factureDataGrid.ItemsSource = ser_facture.findFactureByClient(currentClient);

                label_total_ht_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_H_tva));
                label_total_tva_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_tva));
                label_total_ttc_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_ttc));
            }
        }

        private void Supprimer_facture_Click(object sender, RoutedEventArgs e)
        {
            if (currentFacture != null && currentClient != null)
            {
                if (MessageBox.Show("Supprimer facture", "Supprimer facture", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //do no stuff
                }
                else
                {
                    foreach (var item in ser_bonDeLiv.findBonDeLivraisonBynumFacture(currentFacture))
                    {
                        BonDeLivraison bon = ser_bonDeLiv.findBonDeLivraisonByNum(item.Num);
                        bon.Num_Facture = null;
                        ser_bonDeLiv.editBonDeLivraison(bon);
                    }
                    ser_facture.deleteFacture(currentFacture);
                    currentFacture = null;
                    labelCurrentFacture.Content = null;

                    factureDataGrid.ItemsSource = null;
                    bonDeLivraisonDataGrid_DansFacture.ItemsSource = null;
                    bonDeLivraisonDataGrid_NonFacturer.ItemsSource = null;

                    factureDataGrid.ItemsSource = ser_facture.findFactureByClient(currentClient);
                    bonDeLivraisonDataGrid_NonFacturer.ItemsSource = ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient);

                    label_total_ht_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_H_tva));
                    label_total_tva_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_tva));
                    label_total_ttc_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_ttc));

                    label_total_ht_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient).Sum(t => t.tot_H_tva));
                    label_total_tva_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient).Sum(t => t.tot_tva));
                    label_total_ttc_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient).Sum(t => t.net_payer));

                    label_total_ht_dansFacture.Text = null;
                    label_total_tva_dansFacture.Text = null;
                    label_total_ttc_dansFacture.Text = null;
                }
            }
        }
   
        private void imprimer_selected_facture_button_Click(object sender, RoutedEventArgs e)
        {
           if (currentFacture != null)
           {
               int total_ligne_frombl = 0;
               int total_bl;

               for (int i = 0; i < ser_bonDeLiv.findBonDeLivraisonBynumFacture(currentFacture).Count; i++)
               {
                    int num_bl_tmp = ser_bonDeLiv.findBonDeLivraisonBynumFacture(currentFacture)[i].Num;
                    total_ligne_frombl += ser_LigneBL.findLigneBlByNumBL(num_bl_tmp).Count;
               }
               total_bl = ser_bonDeLiv.findBonDeLivraisonBynumFacture(currentFacture).Count;
               int N_li_page = (total_ligne_frombl + total_bl) / 34;
               int Res = (total_ligne_frombl + total_bl) % 34;

               charge_Window_to_Print(N_li_page, Res, (total_ligne_frombl + total_bl));
           }
        }

        public void charge_Window_to_Print(int N_li_page, int Res, int all_ligne)
        {
           if (currentFacture != null)
           {

               //string sEnlettre = somme.converti(float.Parse(ADAB.Get_TotalNet__BL_ByNumFacture(selected_facture).ToString()));
               List<Canvas> vc = new List<Canvas>();

               if (N_li_page < 1)
               {
                   WindowToPrint winToPrint = new WindowToPrint();
                   int va = 1;
                   for (int i = 0; i < ser_bonDeLiv.findBonDeLivraisonBynumFacture(currentFacture).Count; i++)
                   {
                        int num_bl_tmp = ser_bonDeLiv.findBonDeLivraisonBynumFacture(currentFacture)[i].Num;
                       printEng.Search(0, va, winToPrint.ArticleGrid).Text = "BL N° " + num_bl_tmp.ToString();
                       printEng.Search(1, va, winToPrint.ArticleGrid).Text = ser_bonDeLiv.findBonDeLivraisonBynumFacture(currentFacture)[i].date.ToShortDateString();
                       va++;
                       for (int j = 0; j < ser_LigneBL.findLigneBlByNumBL(num_bl_tmp).Count; j++)
                       {
                            printEng.Search(0, va, winToPrint.ArticleGrid).Text = ser_LigneBL.findLigneBlByNumBL(num_bl_tmp)[j].Ref_Produit;
                            printEng.Search(1, va, winToPrint.ArticleGrid).Text = ser_LigneBL.findLigneBlByNumBL(num_bl_tmp)[j].designation_li;
                            printEng.Search(2, va, winToPrint.ArticleGrid).Text = ser_LigneBL.findLigneBlByNumBL(num_bl_tmp)[j].qte_li.ToString();
                            printEng.Search(3, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_LigneBL.findLigneBlByNumBL(num_bl_tmp)[j].prix_HT);
                           printEng.Search(4, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n2}", ser_LigneBL.findLigneBlByNumBL(num_bl_tmp)[j].remise);
                           printEng.Search(5, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_LigneBL.findLigneBlByNumBL(num_bl_tmp)[j].tot_HT);
                           printEng.Search(6, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n2}", ser_LigneBL.findLigneBlByNumBL(num_bl_tmp)[j].tva);
                           printEng.Search(7, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_LigneBL.findLigneBlByNumBL(num_bl_tmp)[j].tot_TTC);
                           va++;
                       }
                   }

                   SetWindowToPrintBL(winToPrint, false,1,1);
                   winToPrint.Close();
                   vc.Add(winToPrint.CanvasToPrint);

               }
               if (N_li_page >= 1)
               {
                    int nbrePageConcraite,currentPage =1;
                    if(Res == 0)
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
                   for (int i = 0; i < ser_bonDeLiv.findBonDeLivraisonBynumFacture(currentFacture).Count; i++)
                   {
                        int num_bl_tmp = ser_bonDeLiv.findBonDeLivraisonBynumFacture(currentFacture)[i].Num;
                        printEng.Search(0, va, winToPrint.ArticleGrid).Text = "BL N° " + num_bl_tmp.ToString();
                        printEng.Search(1, va, winToPrint.ArticleGrid).Text = ser_bonDeLiv.findBonDeLivraisonBynumFacture(currentFacture)[i].date.ToShortDateString();
                        va++;
                       vaa++;

                       for (int j = 0; j < ser_LigneBL.findLigneBlByNumBL(num_bl_tmp).Count; j++)
                       {
                            printEng.Search(0, va, winToPrint.ArticleGrid).Text = ser_LigneBL.findLigneBlByNumBL(num_bl_tmp)[j].Ref_Produit;
                            printEng.Search(1, va, winToPrint.ArticleGrid).Text = ser_LigneBL.findLigneBlByNumBL(num_bl_tmp)[j].designation_li;
                            printEng.Search(2, va, winToPrint.ArticleGrid).Text = ser_LigneBL.findLigneBlByNumBL(num_bl_tmp)[j].qte_li.ToString();
                            printEng.Search(3, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_LigneBL.findLigneBlByNumBL(num_bl_tmp)[j].prix_HT);
                            printEng.Search(4, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n2}", ser_LigneBL.findLigneBlByNumBL(num_bl_tmp)[j].remise);
                            printEng.Search(5, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_LigneBL.findLigneBlByNumBL(num_bl_tmp)[j].tot_HT);
                            printEng.Search(6, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n2}", ser_LigneBL.findLigneBlByNumBL(num_bl_tmp)[j].tva);
                            printEng.Search(7, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_LigneBL.findLigneBlByNumBL(num_bl_tmp)[j].tot_TTC);
                            va++;
                           vaa++;
                           if (va >= 34)
                           {
                               if (vaa == all_ligne)
                               {
                                    SetWindowToPrintBL(winToPrint, false,currentPage,nbrePageConcraite);
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
            /*   printEng.Export(new Uri(Directory.GetCurrentDirectory().ToString() + "\\Facture_Client\\" + "facture_client-" + currentFacture.Num.ToString() + "___" + String.Format("{0:d-M-yyyy}", currentFacture.date) + ".xps"), vc);
               //fenetre contient le Document View
               WindowBeforePrint wi = new WindowBeforePrint();
               XpsDocument xpsDocument = new XpsDocument(Directory.GetCurrentDirectory().ToString() + "\\Facture_Client\\" + "facture_client-" + currentFacture.Num.ToString() + "___" + String.Format("{0:d-M-yyyy}", currentFacture.date) + ".xps", FileAccess.Read);
               wi.document_a_print.Document = xpsDocument.GetFixedDocumentSequence();
               wi.Show();
               xpsDocument.Close();*/

           }

        }

        public void SetWindowToPrintBL(WindowToPrint winToPrint, bool manyPage, int curentPage, int numTotPage)
        {
           string sEnlettre = somme.converti((float) currentFacture.tot_ttc, "Facture");
           winToPrint.blocNomSte.Text = ser_systeme.findById(1).NomSociete;
           winToPrint.blockClient.Text = "Client : " + currentClient.nom + "\n" + "Code TVA : " + currentClient.matricule + "/" + currentClient.code + "/" + currentClient.code_cat + "/" + currentClient.etb_sec; 
           winToPrint.blocNumType.Text = "       Facture N° : " + String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:0000}", currentFacture.Num);
            winToPrint.blocAdresse.Text = ser_systeme.findById(1).adresse;
            winToPrint.blocTel.Text = "Tel: " + ser_systeme.findById(1).tel
            + "   Fax: " + ser_systeme.findById(1).fax;
            winToPrint.blocTVA.Text = "TVA:  " + ser_systeme.findById(1).matriculeFiscale + "/" + ser_systeme.findById(1).codeTVA + "/" + ser_systeme.findById(1).codeCategorie + "/" + ser_systeme.findById(1).etbSecondaire;
            winToPrint.blocEmail.Text = "E-mail:  " + ser_systeme.findById(1).email;


            winToPrint.blocDate.Text = "Date : " + currentFacture.date.ToShortDateString();
            winToPrint.numPage.Text = "Page : " + curentPage.ToString() + "/" + numTotPage.ToString();

            if (manyPage == false)
           {
               winToPrint.blocTotalHT.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}",currentFacture.tot_H_tva);
               winToPrint.bloctotalTva.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFacture.tot_tva);
               winToPrint.blocTotalNet.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFacture.tot_ttc);
               winToPrint.blocSomme.Text = sEnlettre;
               winToPrint.blocTimbre.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_systeme.findById(1).Timbre);

                List<LigneBL> listeLigneBl = new List<LigneBL>();
                List<BonDeLivraison> listeBL= ser_bonDeLiv.findBonDeLivraisonBynumFacture(currentFacture);
                foreach(var item in listeBL)
                {
                    listeLigneBl.AddRange( ser_LigneBL.findLigneBlByNumBL(item.Num));
                }


                SortedList<int, decimal> aa = new SortedList<int, decimal>();
                aa.Add(0, 0);
                aa.Add(7, 0);
                aa.Add(13, 0);
                aa.Add(19, 0);

                foreach (var item in listeLigneBl)
                {
                    if (item.tva == 00.00)
                    {
                        aa[0] += item.tot_HT;
                        winToPrint.textBlock00.Visibility = Visibility.Visible;
                    }
                    if (item.tva == 7.00)
                    {
                        aa[7] += item.tot_HT;
                        winToPrint.textBlock6.Visibility = Visibility.Visible;
                    }
                    if (item.tva == 13.00)
                    {
                        aa[13] += item.tot_HT;
                        winToPrint.textBlock12.Visibility = Visibility.Visible;
                    }
                    if (item.tva == 19.00)
                    {
                        aa[19] += item.tot_HT;
                        winToPrint.textBlock18.Visibility = Visibility.Visible;
                    }

                }
                winToPrint.base0textbloc.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", aa[0]);
                winToPrint.base18textbloc.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", aa[19]);
                winToPrint.base6textbloc.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", aa[7]);
                winToPrint.base12textbloc.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", aa[13]);
                winToPrint.taux6textbloc_Copy.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", Math.Round((aa[7] * 7) / 100));
                winToPrint.taux12textbloc_Copy1.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", Math.Round((aa[13] * 13) / 100));
                winToPrint.taux18textbloc_Copy2.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", Math.Round((aa[19] * 19) / 100));
            }

        }

        private void SmartBtn_Click(object sender, RoutedEventArgs e)
        {
     /*       if(currentClient != null)
            { 
            Win_facturationClientParMois win = new Win_facturationClientParMois();
                win.clientOur = currentClient;
            win.ShowDialog();
            factureDataGrid.ItemsSource = null;
            bonDeLivraisonDataGrid_DansFacture.ItemsSource = null;
            bonDeLivraisonDataGrid_NonFacturer.ItemsSource = null;
            factureDataGrid.ItemsSource = ser_facture.findFactureByClient(currentClient);
            bonDeLivraisonDataGrid_NonFacturer.ItemsSource = ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient);
           // bonDeLivraisonDataGrid_DansFacture.ItemsSource = ser_bonDeLiv.findBonDeLivraisonBynumFacture(currentFacture);

            label_total_ht_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_H_tva));
            label_total_tva_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_tva));
            label_total_ttc_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_ttc));

            label_total_ht_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient).Sum(t => t.tot_H_tva));
            label_total_tva_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient).Sum(t => t.tot_tva));
            label_total_ttc_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient).Sum(t => t.net_payer));
            
            
            }*/
        }


        private void RetenueBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentFacture != null)
            {
                List<Facture> listeToSend = new List<Facture>();
                //  IList<FactureFournisseur> listeToSend = (IList<FactureFournisseur>)factureFournisseurDataGrid.SelectedItems;
                var selected = factureDataGrid.SelectedItems;

                foreach (var item in selected)
                {
                    var dog = item as Facture;
                    listeToSend.Add(dog);
                }
                charge_Window_to_Print_retenue(listeToSend); 
            }
        }

        public void charge_Window_to_Print_retenue(List<Facture> listeFac)
        {
            if (currentClient != null && listeFac.Count <= 6)
            {

                //string sEnlettre = somme.converti(float.Parse(ADAB.Get_TotalNet__BL_ByNumFacture(selected_facture).ToString()));
                List<Canvas> vc = new List<Canvas>();
                decimal tot_ret_tmp = 0;
                decimal tot_net_tmp = 0;
                WindowToPrintRetenueAlaSource winToPrint = new WindowToPrintRetenueAlaSource();
                int va = 0;
                foreach (Facture item in listeFac)
                {
                    printEng.Search(0, va, winToPrint.Gridna).Text = item.Num.ToString();
                    printEng.Search(1, va, winToPrint.Gridna).Text = item.date.ToShortDateString();

                    double x = Convert.ToDouble(item.tot_ttc);
                    double ret = (x * ser_systeme.findById(1).pourcentageRetenu) / 100;
                    decimal y = Convert.ToDecimal(ret);
                    tot_ret_tmp += y;
                    tot_net_tmp += (item.tot_ttc - y);
                    printEng.Search(2, va, winToPrint.Gridna).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", item.tot_ttc);
                    printEng.Search(3, va, winToPrint.Gridna).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", y);
                    printEng.Search(4, va, winToPrint.Gridna).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", item.tot_ttc - y);
                    va++;
                }
                winToPrint.totBrut.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", listeFac.Sum(t => t.tot_ttc));
                SetWindowToPrintBL(winToPrint);
                winToPrint.totRetenue.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", tot_ret_tmp);
                winToPrint.totNet.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", tot_net_tmp);

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

            winToPrint.NomPayeur.Text = currentClient.nom;
            winToPrint.adressPayeur.Text = currentClient.adresse;
            winToPrint.MatriculePayeur.Text = currentClient.matricule;
            winToPrint.CodeTVAPayeur.Text = currentClient.code;
            winToPrint.CodeCatePayeur.Text = currentClient.code_cat;
            winToPrint.etbSecPayeur.Text = currentClient.etb_sec;

            winToPrint.NomBeni.Text = ser_systeme.findById(1).NomSociete;
            winToPrint.MatriculeBeni.Text = ser_systeme.findById(1).matriculeFiscale;
            winToPrint.CodeTVABeni.Text = ser_systeme.findById(1).codeTVA;
            winToPrint.CodeCateBeni.Text = ser_systeme.findById(1).codeCategorie;
            winToPrint.etbSecBeni.Text = ser_systeme.findById(1).etbSecondaire;
            winToPrint.addresseRetenuPayeur.Text = ser_systeme.findById(1).adresseRetenu;
            winToPrint.adresseBeni.Text = ser_systeme.findById(1).adresse;
            winToPrint.DateRetenuPayeur_Copy.Text = DateTime.Now.ToShortDateString();

            winToPrint.TauxRetenu.Text = "Retenue " + ser_systeme.findById(1).pourcentageRetenu;
        }

        private void ChangerClientBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentClient != null && currentFacture != null)
            {
                Win_ChangeClientDeFacture win = new Win_ChangeClientDeFacture(currentFacture);
                win.ShowDialog();
                factureDataGrid.ItemsSource = null;
                bonDeLivraisonDataGrid_DansFacture.ItemsSource = null;
                bonDeLivraisonDataGrid_NonFacturer.ItemsSource = null;
              
                factureDataGrid.ItemsSource = ser_facture.findFactureByClient(currentClient);
                bonDeLivraisonDataGrid_NonFacturer.ItemsSource = ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient);

                label_total_ht_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_H_tva));
                label_total_tva_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_tva));
                label_total_ttc_facture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_facture.findFactureByClient(currentClient).Sum(t => t.tot_ttc));

                label_total_ht_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient).Sum(t => t.tot_H_tva));
                label_total_tva_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient).Sum(t => t.tot_tva));
                label_total_ttc_nonFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_bonDeLiv.findBonDeLivraisonByClientNonFacturer(currentClient).Sum(t => t.net_payer));

                label_total_ht_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFacture.tot_H_tva);
                label_total_tva_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFacture.tot_tva);
                label_total_ttc_dansFacture.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFacture.tot_ttc);
            }

        }
    }
}
