using Domain.Models;
using Service;
using Ste.Fenetre.Reporting;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for Win_ManageBonDeReception.xaml
    /// </summary>
    public partial class Win_ManageBonDeReception : Window
    {
        BonDeReceptionService ser_bonDeRec = new BonDeReceptionService();
        FournisseurService ser_fourniss = new FournisseurService();
        List<BonDeReception> BonDeReceptions = new List<BonDeReception>();
        LigneBonDeRecepctionService ser_ligneBonRec = new LigneBonDeRecepctionService();
        PrintEngine printEng = new PrintEngine();

        public Win_ManageBonDeReception()
        {
            InitializeComponent();
            clearFields();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            

        }

        private void ChercherBtn_Click(object sender, RoutedEventArgs e)
        {
            BonDeReceptions = ser_bonDeRec.getAllBonDeReception();
            bonDeReceptionDataGrid.ItemsSource = null;

            if (!dateDebutPicker.SelectedDate.Equals(null) && !dateFinPicker.SelectedDate.Equals(null) && dateFinPicker.SelectedDate >= dateDebutPicker.SelectedDate)
            {
                BonDeReceptions.RemoveAll(t => t.dateDeLivraison < dateDebutPicker.SelectedDate || t.dateDeLivraison > dateFinPicker.SelectedDate);
            }
            if (!FournisseurTextBlock.Text.Equals("Fournisseur non selectionné"))
            {
                BonDeReceptions.RemoveAll(t => t.id_fournisseur.ToString() != CodeFourTextBlock.Text);
            }

            if (radioFacturer.IsChecked == true)
            {
                BonDeReceptions.RemoveAll(t => t.Num_Facture_fournisseur == null);
            }
            else if (radioPasEncore.IsChecked == true)
            {
                BonDeReceptions.RemoveAll(t => t.Num_Facture_fournisseur != null);
            }
            
                foreach (var item in BonDeReceptions)
         {
             List<LigneBonReception> listeLignes = new List<LigneBonReception>();
             listeLignes = ser_ligneBonRec.findLigneBonReceptionByNumBC(item.Num);
             item.tot_H_tva = listeLignes.Sum(t => t.tot_HT);
             item.net_payer = listeLignes.Sum(t => t.tot_TTC);
             item.tot_tva = listeLignes.Sum(t => t.tot_TTC) - listeLignes.Sum(t => t.tot_HT);
         }
            bonDeReceptionDataGrid.ItemsSource = BonDeReceptions;
            TotalTextBlock.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", BonDeReceptions.Sum(t => t.net_payer));
            TotalHorsTVATextBlock.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", BonDeReceptions.Sum(t => t.tot_H_tva));
            TotalTVATextBlock.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", BonDeReceptions.Sum(t => t.tot_tva));
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
        }

        private void FermerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void GetSelectedBon(object sender, MouseButtonEventArgs e)
        {
            try
            {
                BonDeReception bon = (BonDeReception)bonDeReceptionDataGrid.SelectedItem;
                if(bon != null)
                { 
                Window_BonDeReception win = new Window_BonDeReception(bon);
                win.ShowDialog();
                ChercherBtn_Click(new object(), new RoutedEventArgs());
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Probleme !");
            }
        }

        private void Fournisseurbtn_Click(object sender, RoutedEventArgs e)
        {
            GetFournisseur win = new GetFournisseur();
            win.ShowDialog();
            CodeFourTextBlock.Text = win.fournisseurToSend.id.ToString();
            FournisseurTextBlock.Text = win.fournisseurToSend.nom;
        }
        private void clearFields()
        {
            dateFinPicker.SelectedDate = null;
            dateDebutPicker.SelectedDate = null;
            FournisseurTextBlock.Text = "Fournisseur non selectionné";
            CodeFourTextBlock.Text = "";
            radioTous.IsChecked = true;
            bonDeReceptionDataGrid.ItemsSource = null;
        
        }

        private void ReportingBtn_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<Fournisseur, decimal> GeneratedListFournisseur = new Dictionary<Fournisseur, decimal>();
            foreach(var item in ser_fourniss.getAllFournisseur())
            {
                IEnumerable<BonDeReception> listTmp = BonDeReceptions.Where(t => t.id_fournisseur == item.id);
                GeneratedListFournisseur.Add(item, listTmp.Sum(t => t.net_payer));
            }
            
            int N_li_page = GeneratedListFournisseur.Count / 34;
            int Res = GeneratedListFournisseur.Count % 34;
            charge_Window_to_Print(N_li_page, Res, GeneratedListFournisseur);
        }
        public void charge_Window_to_Print(int N_li_page, int Res , Dictionary<Fournisseur, decimal> GeneratedListFournisseur)
        {
       
                // probleme i 0-> 34 

        //        string sEnlettre = somme.converti((float)Total_TTC(), "BL");
                List<Canvas> vc = new List<Canvas>();
                decimal k = 0;
                if (N_li_page < 1)
                {
                    int i = 1;
                    WindowToPrintReporting winToPrint = new WindowToPrintReporting();
                    foreach (var item in ser_fourniss.getAllFournisseur())
                    {
                        printEng.Search(1, i, winToPrint.ArticleGrid).Text = item.nom;
                    IEnumerable<BonDeReception> listTmp = BonDeReceptions.Where(t => t.id_fournisseur == item.id);
                    printEng.Search(3, i, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", listTmp.Sum(t => t.net_payer)) ;
                        i++;
                    }
                winToPrint.textBlockdate1.Text = dateDebutPicker.SelectedDate.Value.ToShortDateString();
                winToPrint.textBlockdate2.Text = dateFinPicker.SelectedDate.Value.ToShortDateString();
                winToPrint.textBlockTotal.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", BonDeReceptions.Sum(t => t.net_payer));
                winToPrint.Close();
                    vc.Add(winToPrint.CanvasToPrint);

                }
        /*        if (N_li_page >= 1)
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
                        }
                        //remlplir les blocs
                        SetWindowToPrintBL(LastwinToPrint, false, N_li_page + 1, N_li_page + 1);
                        vc.Add(LastwinToPrint.CanvasToPrint);
                        LastwinToPrint.Close();
                    }

                }*/
                printEng.Export(vc);
                //fenetre contient le Document View
                /*WindowBeforePrint wi = new WindowBeforePrint();
                XpsDocument xpsDocument = new XpsDocument(Directory.GetCurrentDirectory().ToString() + "\\BL\\" + "BL-" + NumBL_Label.Content.ToString() + "___" + String.Format("{0:d-M-yyyy}", DateBL.DisplayDate) + ".xps", FileAccess.Read);
                wi.document_a_print.Document = xpsDocument.GetFixedDocumentSequence();
                wi.Show();
                xpsDocument.Close();*/
       
        }
    }
}
