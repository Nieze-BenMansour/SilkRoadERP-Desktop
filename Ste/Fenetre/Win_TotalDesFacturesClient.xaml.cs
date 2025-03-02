using Domain.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using Microsoft.Office;


namespace Ste.Fenetre
{
    /// <summary>
    /// Interaction logic for Win_TotalDesFacturesClient.xaml
    /// </summary>
    public partial class Win_TotalDesFacturesClient : Window
    {
        FactureService ser_fac = new FactureService();
        SystemeService ser_systeme = new SystemeService();
        LigneBLService ser_LigneBL = new LigneBLService();
        BonDeLivraisonService ser_bonDeLiv = new BonDeLivraisonService();
        PrintEngine printEng = new PrintEngine();
        SommeEnLettres somme = new SommeEnLettres();
        List<Facture> listeFacture = new List<Facture>();
        public Win_TotalDesFacturesClient()
        {
            InitializeComponent();
            //listeFacture = ser_fac.getAllFacture();

            List<Facture> listefaAnumerotter = new List<Facture>();
            //   listefaAnumerotter = ser_fac.getAllFacture();
            listeFacture = ser_fac.getAllFacture();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


        }

        private void ChercherBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!dateDebutPicker.SelectedDate.Equals(null) && !dateFinPicker.SelectedDate.Equals(null) && dateFinPicker.SelectedDate >= dateDebutPicker.SelectedDate)
            {
                listeFacture.RemoveAll(t => t.date < dateDebutPicker.SelectedDate || t.date > dateFinPicker.SelectedDate);
            }
            if (!ClientTextBlock.Text.Equals("Client non sélectionné"))
            {
                listeFacture.RemoveAll(t => t.id_client.ToString() != CodeClientTextBlock.Text);
            }
            factureDataGrid.ItemsSource = null;
            factureDataGrid.ItemsSource = listeFacture;
            TotalTextBlock.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", listeFacture.Sum(t => t.tot_ttc));
            TotalHorsTVATextBlock.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", listeFacture.Sum(t => t.tot_H_tva));
            TotalTVATextBlock.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", listeFacture.Sum(t => t.tot_tva));
        }

        private void Clientbtn_Click(object sender, RoutedEventArgs e)
        {
            GetClient win = new GetClient();
            win.ShowDialog();
            CodeClientTextBlock.Text = win.clientToSend.Id.ToString();
            ClientTextBlock.Text = win.clientToSend.nom;
        }

        private void FermerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void clearFields()
        {
            dateFinPicker.SelectedDate = null;
            dateDebutPicker.SelectedDate = null;
            ClientTextBlock.Text = "Client non sélectionné";
            CodeClientTextBlock.Text = "";
            listeFacture = ser_fac.getAllFacture();
            factureDataGrid.ItemsSource = null;
            //  bonDeLivraisonDataGrid.ItemsSource = bon
            TotalTextBlock.Text = null;
            TotalHorsTVATextBlock.Text = null;
            TotalTVATextBlock.Text = null;

        }


        private void imprimer_factures(List<Facture> listeFacture)
        {
            List<Canvas> vc = new List<Canvas>();
            foreach (var item in listeFacture)
            {
                int total_ligne_frombl = 0;
                int total_bl;

                foreach (var bl in ser_bonDeLiv.findBonDeLivraisonBynumFacture(item))
                {
                    int num_bl_tmp = bl.Num;
                    total_ligne_frombl += ser_LigneBL.findLigneBlByNumBL(num_bl_tmp).Count;
                }
                total_bl = ser_bonDeLiv.findBonDeLivraisonBynumFacture(item).Count;
                int N_li_page = (total_ligne_frombl + total_bl) / 34;
                int Res = (total_ligne_frombl + total_bl) % 34;

                vc.AddRange(charge_Window_to_Print(N_li_page, Res, (total_ligne_frombl + total_bl), item));
            }
            printEng.Export2(new Uri(Directory.GetCurrentDirectory().ToString() + "\\Facture_Client\\" + "facture_client-" + "___" + ".xps"), vc);
            //fenetre contient le Document View
            WindowBeforePrint wi = new WindowBeforePrint();
            XpsDocument xpsDocument = new XpsDocument(Directory.GetCurrentDirectory().ToString() + "\\Facture_Client\\" + "facture_client-" + "___" + ".xps", FileAccess.Read);
            wi.document_a_print.Document = xpsDocument.GetFixedDocumentSequence();
            wi.Show();
            xpsDocument.Close();
        }

        public List<Canvas> charge_Window_to_Print(int N_li_page, int Res, int all_ligne, Facture currentFacture)
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

                SetWindowToPrintBL(winToPrint, false, 1, 1, currentFacture);
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
                List<BonDeLivraison> listeBl = new List<BonDeLivraison>();
                listeBl = ser_bonDeLiv.findBonDeLivraisonBynumFacture(currentFacture);
                if (listeBl.Count != 0)
                {

                    foreach (var item in listeBl)
                    {
                        printEng.Search(0, va, winToPrint.ArticleGrid).Text = "BL N° " + item.Num.ToString();
                        printEng.Search(1, va, winToPrint.ArticleGrid).Text = item.date.ToShortDateString();

                        if (va == 33)
                        {
                            if (vaa == all_ligne)
                            {
                                SetWindowToPrintBL(winToPrint, false, currentPage, nbrePageConcraite, currentFacture);
                                hleg = true;

                            }
                            if (vaa < all_ligne)
                            {
                                SetWindowToPrintBL(winToPrint, true, currentPage, nbrePageConcraite, currentFacture);
                                currentPage++;
                            }


                            vc.Add(winToPrint.CanvasToPrint);
                            winToPrint.Close();
                            winToPrint = new WindowToPrint();
                            va = 1;
                        }
                        va++;
                        vaa++;
                        List<LigneBL> listeLigneBl = ser_LigneBL.findLigneBlByNumBL(item.Num);
                        if (listeLigneBl.Count != 0)
                        {
                            foreach (var item2 in listeLigneBl)
                            {
                                printEng.Search(0, va, winToPrint.ArticleGrid).Text = item2.Ref_Produit;
                                printEng.Search(1, va, winToPrint.ArticleGrid).Text = item2.designation_li;
                                printEng.Search(2, va, winToPrint.ArticleGrid).Text = item2.qte_li.ToString();
                                printEng.Search(3, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", item2.prix_HT);
                                printEng.Search(4, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n2}", item2.remise);
                                printEng.Search(5, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", item2.tot_HT);
                                printEng.Search(6, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n2}", item2.tva);
                                printEng.Search(7, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", item2.tot_TTC);



                                if (va == 33)
                                {
                                    if (vaa == all_ligne)
                                    {
                                        SetWindowToPrintBL(winToPrint, false, currentPage, nbrePageConcraite, currentFacture);
                                        hleg = true;

                                    }
                                    if (vaa < all_ligne)
                                    {
                                        SetWindowToPrintBL(winToPrint, true, currentPage, nbrePageConcraite, currentFacture);
                                        currentPage++;
                                    }

                                    vc.Add(winToPrint.CanvasToPrint);
                                    winToPrint.Close();
                                    winToPrint = new WindowToPrint();
                                    va = 1;
                                }
                                va += 1;
                                vaa++;

                            }


                        }
                    }
                }
                if (hleg == false)
                {
                    SetWindowToPrintBL(winToPrint, false, currentPage, nbrePageConcraite, currentFacture);
                    vc.Add(winToPrint.CanvasToPrint);
                }
                winToPrint.Close();


            }

            return vc;
        }

        public void SetWindowToPrintBL(WindowToPrint winToPrint, bool manyPage, int curentPage, int numTotPage, Facture currentFacture)
        {
            string sEnlettre = somme.converti((float)currentFacture.tot_ttc, "Facture");
            winToPrint.blocNomSte.Text = ser_systeme.findById(1).NomSociete;
            winToPrint.blockClient.Text = "Client : " + currentFacture.Client.nom + "\n" + "Code TVA : " + currentFacture.Client.matricule + "/" + currentFacture.Client.code + "/" + currentFacture.Client.code_cat + "/" + currentFacture.Client.etb_sec;
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
                winToPrint.blocTotalHT.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFacture.tot_H_tva);
                winToPrint.bloctotalTva.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFacture.tot_tva);
                winToPrint.blocTotalNet.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", currentFacture.tot_ttc);
                winToPrint.blocSomme.Text = sEnlettre;
                winToPrint.blocTimbre.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_systeme.findById(1).Timbre);

                List<LigneBL> listeLigneBl = new List<LigneBL>();
                List<BonDeLivraison> listeBL = ser_bonDeLiv.findBonDeLivraisonBynumFacture(currentFacture);
                foreach (var item in listeBL)
                {
                    listeLigneBl.AddRange(ser_LigneBL.findLigneBlByNumBL(item.Num));
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
                    if (item.tva == 07.00)
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

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {

            if (listeFacture != null)
            {

                imprimer_factures(listeFacture);
            }
        }

        private void actualiseBtn_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
        }

        private void buttonExportExcel_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel._Workbook oWB;
            Microsoft.Office.Interop.Excel._Worksheet oSheet;
            Microsoft.Office.Interop.Excel.Range oRng;
            object misvalue = System.Reflection.Missing.Value;

            //Start Excel and get Application object.
            oXL = new Microsoft.Office.Interop.Excel.Application();
            oXL.Visible = true;

            //Get a new workbook.
            oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(""));
            oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;

          

            //Add table headers going cell by cell.
            oSheet.Cells[1, 1] = "Num";
            oSheet.Cells[1, 2] = "Date";
            oSheet.Cells[1, 3] = "Client";
            oSheet.Cells[1, 4] = "Code Client";
            oSheet.Cells[1, 5] = "MF Client";
            oSheet.Cells[1, 6] = "Base HT 7";
            oSheet.Cells[1, 7] = "Base HT 13";
            oSheet.Cells[1, 8] = "Base HT 19";
            oSheet.Cells[1, 9] = "Total TVA 7";
            oSheet.Cells[1, 10] = "Total TVA 13";
            oSheet.Cells[1, 11] = "Total TVA 19";
            oSheet.Cells[1, 12] = "Total TVA";
            oSheet.Cells[1, 13] = "Total TTC";
          //  oSheet.Cells[1, 14] = "Timbre";


            //Format A1:D1 as bold, vertical alignment = center.
            oSheet.get_Range("A1", "D1").Font.Bold = true;
            oSheet.get_Range("A1", "D1").VerticalAlignment =
                Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

            
            oSheet.Columns[6].NumberFormat = "0,000";
            oSheet.Columns[7].NumberFormat = "0,000";
            oSheet.Columns[8].NumberFormat = "0,000";
            oSheet.Columns[9].NumberFormat = "0,000";
            oSheet.Columns[10].NumberFormat = "0,000";
            oSheet.Columns[11].NumberFormat = "0,000";
            oSheet.Columns[12].NumberFormat = "0,000";
            oSheet.Columns[13].NumberFormat = "0,000";
         //   oSheet.Columns[14].NumberFormat = "0.000";




            // Create an array to multiple values at once.
            string[,] saNames = new string[9000,12];
            int j = 1;
            for (int i = 0; i < listeFacture.Count; i++)
            {


                List<LigneBL> listeLigneBl = new List<LigneBL>();
                List<BonDeLivraison> listeBL = ser_bonDeLiv.findBonDeLivraisonBynumFacture(listeFacture[i]);
                foreach (var item in listeBL)
                {
                    listeLigneBl.AddRange(ser_LigneBL.findLigneBlByNumBL(item.Num));
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
                       // winToPrint.textBlock00.Visibility = Visibility.Visible;
                    }
                    if (item.tva == 07.00)
                    {
                        aa[7] += item.tot_HT;
                      //  winToPrint.textBlock6.Visibility = Visibility.Visible;
                    }
                    if (item.tva == 13.00)
                    {
                        aa[13] += item.tot_HT;
                     //   winToPrint.textBlock12.Visibility = Visibility.Visible;
                    }
                    if (item.tva == 19.00)
                    {
                        aa[19] += item.tot_HT;
                   //     winToPrint.textBlock18.Visibility = Visibility.Visible;
                    }

                }


                j++;
                oSheet.Cells[j, 1] = listeFacture[i].Num;
                oSheet.Cells[j, 2] = listeFacture[i].date;
                oSheet.Cells[j, 3] = listeFacture[i].Client.nom;
                oSheet.Cells[j, 4] = listeFacture[i].Client.Id;
                oSheet.Cells[j, 5] = listeFacture[i].Client.matricule + listeFacture[i].Client.code + listeFacture[i].Client.code_cat + listeFacture[i].Client.etb_sec;

                oSheet.Cells[j, 6] = aa[7] / 1000;
                oSheet.Cells[j, 7] = aa[13] / 1000;
                oSheet.Cells[j, 8] = aa[19] / 1000;

                oSheet.Cells[j, 9] =  Math.Round( ((aa[7]  *  7) / 100) / 1000 , 3);
                oSheet.Cells[j, 10] = Math.Round( ((aa[13] * 13) / 100) / 1000 , 3);
                oSheet.Cells[j, 11] = Math.Round( ((aa[19] * 19) / 100) / 1000 , 3);

                oSheet.Cells[j, 12] = Math.Round(( ((aa[7] * 7) / 100) + ((aa[13] * 13) / 100) + ((aa[19] * 19) / 100) ) / 1000, 3);
                oSheet.Cells[j, 13] = listeFacture[i].tot_ttc / 1000;
            }
            /*
                        saNames[0, 0] = "John";
                        saNames[0, 1] = "Smith";
                        saNames[1, 0] = "Tom";

                        saNames[4, 1] = "Johnson";

                        //Fill A2:B6 with an array of values (First and Last Names).
                        oSheet.get_Range("A2", "B6").Value2 = saNames;
                        */
            //Fill C2:C6 with a relative formula (=A2 & " " & B2).
            //           oRng = oSheet.get_Range("C2", "C6");
            //          oRng.Formula = "=A2 & \" \" & B2";

            //Fill D2:D6 with a formula(=RAND()*100000) and apply format.
                       // oRng = oSheet.get_Range("D2", "D6");
            //         oRng.Formula = "=RAND()*100000";

          //  oRng.NumberFormat = "0.00";

            //AutoFit columns A:D.
            oRng = oSheet.get_Range("A1", "D1");
            oRng.EntireColumn.AutoFit();

            oXL.Visible = false;
            oXL.UserControl = false;
            oWB.SaveAs("c:\\test\\test505.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            oWB.Close();


        }
    }
}
