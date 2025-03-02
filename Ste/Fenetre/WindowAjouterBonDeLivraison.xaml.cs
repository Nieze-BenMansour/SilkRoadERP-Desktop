using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Windows.Xps.Packaging;
using System.Windows.Xps;
using System.IO.Packaging;
using System.IO;
using System.Threading;
using System.Windows.Markup;
using System.Windows.Xps.Serialization;
using Domain.Models;
using Service;
using Ste.Fenetre;
using System.Globalization;
using System.Collections;

namespace Ste
{
    public partial class WindowAjouterBonDeLivraison : Window
    {
        //les services
        FactureService ser_facture = new FactureService();
        BonDeLivraisonService ser = new BonDeLivraisonService();
        DeviService ser_Devi = new DeviService();
        ClientService ser_client = new ClientService();
        LigneBLService ser_ligneBL = new LigneBLService();
        LigneDeviService ser_ligneDevi = new LigneDeviService();
        ProduitService ser_produit = new ProduitService();
        SystemeService ser_systeme = new SystemeService();
        //logique de la fenetre
        LigneDynamique ligneDy = new LigneDynamique();
        BonDeLivraisonControle BON = new BonDeLivraisonControle();
        bool AltKeyPressed = false;
        bool BL_Ajouter = false;
        List<LignrBLclass> Liste_Lignes = new List<LignrBLclass>();
        PrintEngine printEng = new PrintEngine();
        SommeEnLettres somme = new SommeEnLettres();
        //instence de travail
        BonDeLivraison bon;
        Devi devi;
        Client currentCient;
        Facture currentFacture;

        bool altF4Pressed = false;
        bool WindowStatuChanged ;
         
        public WindowAjouterBonDeLivraison(Devi deviToSend,BonDeLivraison bonToSend)
        {
             InitializeComponent();


            WindowStatuChanged = false;



            bon = bonToSend;
             devi = deviToSend;
             ligneDy.GridBL = GridBL;//affecter le grid
             ligneDy.Liste_Lignes = Liste_Lignes;// affecter les lignes

     if (devi == null)
     {
         //il s'agit de modifier num_BL != -1
         if (bon != null)
         {
             BL_Ajouter = true;
                    NumBL_Label.Content = bon.Num;
                    CodeLabel.Content = bon.clientId;
                    ClientLabel.Content = bon.Client.nom;
                    currentCient = ser_client.findClientByID((long)bon.clientId.Value);
                  
                    if (bon.Num_Facture != null)
                    {
                        currentFacture = ser_facture.findFactureByNum(bon.Num_Facture.Value);
                        NumFactureLabel.Content = currentFacture.Num;
                    }
                    DateBL.SelectedDate = bon.date;
                    foreach (var item in ser_ligneBL.findLigneBlByNumBL(bon.Num))
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
                        ligneDy.CreateRow_BL_Load(ligne);
                    }
             ligneDy.button_plus_BL_load();
         }
         //il s'agit d'ajouter bon == null
         else  if(bon == null)
         {
                    try
                    {
                        NumBL_Label.Content = ser.MaxIdBL() + 1;
                        currentCient = null;
                    }
                    catch (Exception)
                    {

                    }

                    ligneDy.CreateRow();// creation du 1er ligne
         }
     }
         // il s'agit d'une transformation de devis a BL
     else if(devi != null)
     {
         foreach(var item in ser_ligneDevi.GetLigneByNumDevis(devi.Num))
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
                ligneDy.button_plus_BL_load();
                CodeLabel.Content = ser_client.findClientByID(devi.id_client).Id;
                ClientLabel.Content = ser_client.findClientByID(devi.id_client).nom;
                currentCient = ser_client.findClientByID(devi.id_client);

                try
                {
                    NumBL_Label.Content = ser.MaxIdBL() + 1;
                }
                catch (Exception)
                {
                             
                }
             }
           
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bon != null || devi != null)
            { ligneDy.set_event_handler_bl_load(); }

            SetEventToAllControllsInThisWindow();

        }

        private void SetEventToAllControllsInThisWindow()
        {
            foreach (Xceed.Wpf.Toolkit.IntegerUpDown tb in FindVisualChildren<Xceed.Wpf.Toolkit.IntegerUpDown>(GridBL))
            {
                tb.ValueChanged += new RoutedPropertyChangedEventHandler<object>(c_ControlChanged);
            }
            foreach (AutoCompleteBox tb in FindVisualChildren<AutoCompleteBox>(GridBL))
            {
                tb.TextChanged += new RoutedEventHandler(c_ControlChanged);
            }
            foreach (Xceed.Wpf.Toolkit.DecimalUpDown tb in FindVisualChildren<Xceed.Wpf.Toolkit.DecimalUpDown>(GridBL))
            {
                tb.ValueChanged += new RoutedPropertyChangedEventHandler<object>(c_ControlChanged);
            }
            foreach (Xceed.Wpf.Toolkit.MaskedTextBox tb in FindVisualChildren<Xceed.Wpf.Toolkit.MaskedTextBox>(GridBL))
            {
                tb.TextChanged += new TextChangedEventHandler(c_ControlChanged);
            }
            foreach (Button tb in FindVisualChildren<Button>(GridBL))
            {
                tb.Click += new RoutedEventHandler(c_ControlChanged);
            }
        }

        private void getClient_Click(object sender, RoutedEventArgs e)
        {
            GetClient win = new GetClient();
            win.ShowDialog();
            CodeLabel.Content = win.clientToSend.Id;
            ClientLabel.Content = win.clientToSend.nom;
            currentCient = ser_client.findClientByID(win.clientToSend.Id);
            currentFacture = null;
            NumFactureLabel.Content = null;
            WindowStatuChanged = true;
        }
     
       //Key down on window
    private void Total_setRemise_setTVA(object sender, KeyEventArgs e)
       {

               
            if (e.Key == Key.F4)
           {
               Ajouter_BL_Data_Base();
           }
           
           if (e.Key == Key.F3)
           {
               WindowTotalBL win = new WindowTotalBL();
               win.totHT.Text = Total_HT().ToString();
               win.totTVA.Text = Total_tva().ToString();
               win.netApayer.Text = Total_TTC().ToString();
                win.ShowDialog();
           }
           if(e.Key == Key.F6)
           {
               int N_li_page = Liste_Lignes.Count / 34;
               int Res = Liste_Lignes.Count % 34;

               charge_Window_to_Print(N_li_page, Res);
           }
            if (Keyboard.Modifiers == ModifierKeys.Alt && e.SystemKey == Key.F4)
            {
                altF4Pressed = true;
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
               if(e.SystemKey == Key.F4)
                {
           //         altF4Pressed = true;
                }
           }
          if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt)
           {
               AltKeyPressed = true;
           }
            if(e.Key == Key.Add)
            {
               int laster_button  =  GridBL.Children.Count - 1;
               GridBL.Children.RemoveAt(laster_button);
               ligneDy.CreateRow();
                
            }

       }

    decimal Total_HT()
       {
           decimal ret=0;
        for(int i=0;i<Liste_Lignes.Count;i++)
        {
            ret += (decimal)Liste_Lignes[i].tot_ht_UI.Value;
        }
        return ret;
       }
    decimal Total_tva()
    {
        decimal ret = 0;
        for (int i = 0; i < Liste_Lignes.Count; i++)
        {
            ret += (decimal)Liste_Lignes[i].tot_ttc_UI.Value - (decimal)Liste_Lignes[i].tot_ht_UI.Value; 
        }
        return ret;
    }
    decimal Total_TTC()
    {
        decimal ret = 0;
        for (int i = 0; i < Liste_Lignes.Count; i++)
        {
            ret += (decimal)Liste_Lignes[i].tot_ttc_UI.Value;
        }
        return ret;
    }

        // il faut le controle sur les ligne avant
    private void Enregistrer_BL_Click(object sender, RoutedEventArgs e)
    {
        Ajouter_BL_Data_Base();
        SetEventToAllControllsInThisWindow();
    }

        void Ajouter_BL_Data_Base()
        {
            if (BON.BL_Valide(this) && BON.Lignes_Valide(Liste_Lignes) && currentFacture != null)
            {
                try
                {
                    //Données Bl
                    int code_Client = int.Parse(CodeLabel.Content.ToString());
                    string date = DateBL.SelectedDate.Value.ToShortDateString();
                    string sEnlettre = somme.converti((float)Total_TTC(), "BL");

                    if (BL_Ajouter == true)
                    {
                        try
                        {
                            bon.clientId = code_Client;
                            bon.date = DateBL.SelectedDate.Value;
                            bon.tot_H_tva = Total_HT();
                            bon.tot_tva = Total_tva();
                            bon.net_payer = Total_TTC();
                            bon.Num_Facture = currentFacture.Num;
                            ser.editBonDeLivraison(bon);
                            Recharger_Stock(bon.Num);

                            ser_ligneBL.deleteLigneBlByNumBL(bon);

                            if (Ajouter_Lignes_Data_Base())
                            {
                                MessageBox.Show("Bon de livraison mis a jour !");
                            }
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                    if (BL_Ajouter == false)
                    {
                        try
                        {
                            bon = new BonDeLivraison();
                            bon.clientId = code_Client;
                            bon.date = DateBL.SelectedDate.Value;
                            bon.tot_H_tva = Total_HT();
                            bon.tot_tva = Total_tva();
                            bon.net_payer = Total_TTC();
                            bon.temp_bl = DateTime.Now.TimeOfDay;
                            bon.Num_Facture = currentFacture.Num;
                            ser.AddBonDeLivraison(bon);

                            if (Ajouter_Lignes_Data_Base())
                            {
                                BL_Ajouter = true;
                                MessageBox.Show("Bon de livraison enregistré !  " + date);
                            }
                        }
                        catch (Exception e) { MessageBox.Show(e.Message); }
                    }
                    WindowStatuChanged = false;
                }
                catch (Exception exx) { MessageBox.Show(exx.Message); }
            }
        }
 /*   void Get_Transaction_And_Show()
    {
        int num = int.Parse(NumBL_Label.Content.ToString());
    //    string date = tr.GetTransaction(num).Rows[0].ItemArray[2].ToString();
        int type = 0;
            string typeS = "Espece";
        if (typeS == "Espece") {  type = 0; } else {  type = 1; }
        WindowTransactionBL win = new WindowTransactionBL(num, DateTime.Now.ToShortDateString(), type, Total_TTC(), true);
        win.ShowDialog();
    }*/
    bool Ajouter_Lignes_Data_Base() 
    {
            try
            {
                Produit produit_tmp = new Produit();
                bool test = true;
                List<Produit> listeProduit = new List<Produit>();
                List<LigneBL> liste = new List<LigneBL>();
                LigneBL ligne = new LigneBL();
                for (int i = 0; i < Liste_Lignes.Count; i++)
                {
                    produit_tmp = ser_produit.findProduitByRefUnique(Liste_Lignes[i].refUI.Text);
                    produit_tmp.qte -= int.Parse(Liste_Lignes[i].qteUI.Value.ToString());
                    //    ser_produit.editProduit(produit_tmp);
                    listeProduit.Add(produit_tmp);
                    ligne.Num_BL = bon.Num;
                    ligne.qte_li = int.Parse(Liste_Lignes[i].qteUI.Value.ToString());
                    ligne.Ref_Produit = Liste_Lignes[i].refUI.Text;
                    ligne.designation_li = Liste_Lignes[i].desiUI.Text;
                    ligne.prix_HT = decimal.Parse(Liste_Lignes[i].prixHT_UI.Value.ToString());
                    ligne.remise = float.Parse(Liste_Lignes[i].remise_UI.Text);
                    ligne.tot_HT = decimal.Parse(Liste_Lignes[i].tot_ht_UI.Value.ToString());
                    ligne.tva = float.Parse(Liste_Lignes[i].tva_UI.Text);
                    ligne.tot_TTC = decimal.Parse(Liste_Lignes[i].tot_ttc_UI.Value.ToString());
                    //ser_ligneBL.AddLigneBL(ligne);
                    liste.Add(ligne);
                    ligne = new LigneBL();

                }
                ser_produit.editManyProduit(listeProduit);
                ser_ligneBL.AddligneBLMany(liste);
                return test;
            }
            catch (InvalidOperationException) { MessageBox.Show("Reference article non valide !"); return false; }
            {

            }
    }
    void Recharger_Stock(int n)
    {
            List<Produit> listeProduit = new List<Produit>();
            Produit produit_tmp = new Produit();
        try
        {
            foreach(var item in ser_ligneBL.findLigneBlByNumBL(n))
                {
                    int qte_L = item.qte_li;
                    produit_tmp = ser_produit.findProduitByRefUnique(item.Ref_Produit);
                    produit_tmp.qte += qte_L;
                    //ser_produit.editProduit(produit_tmp);
                    listeProduit.Add(produit_tmp);
                }
                ser_produit.editManyProduit(listeProduit);
        }
        catch (Exception) { MessageBox.Show("Probleme de stock !"); }
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
           
        if (BL_Ajouter && WindowStatuChanged == false)
        {
        this.Close();
        }
        else if (BL_Ajouter ==false || WindowStatuChanged == true)
        {
                if (MessageBox.Show("Close Application?", "Sortir sans sauvgarder", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    
                }
                else
                {
                    this.Close();
                }
            }
    }
    private void Print_Click(object sender, RoutedEventArgs e)
    {
            if (BL_Ajouter)
            { 
                int N_li_page = Liste_Lignes.Count / 34;
                int Res = Liste_Lignes.Count %  34;
                charge_Window_to_Print(N_li_page, Res);
            }

        }
    public void charge_Window_to_Print(int N_li_page, int Res)
    {
            try
            {

                // probleme i 0-> 34 

                string sEnlettre = somme.converti((float)Total_TTC(), "BL");
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
                printEng.Export( vc);
                //fenetre contient le Document View
                /*WindowBeforePrint wi = new WindowBeforePrint();
                XpsDocument xpsDocument = new XpsDocument(Directory.GetCurrentDirectory().ToString() + "\\BL\\" + "BL-" + NumBL_Label.Content.ToString() + "___" + String.Format("{0:d-M-yyyy}", DateBL.DisplayDate) + ".xps", FileAccess.Read);
                wi.document_a_print.Document = xpsDocument.GetFixedDocumentSequence();
                wi.Show();
                xpsDocument.Close();*/
            }
            catch (Exception)
            {
                MessageBox.Show("Probleme");
            }

    }
    public void SetWindowToPrintBL(WindowToPrint winToPrint,bool manyPage, int curentPage, int numTotPage)
        {
            string sEnlettre = somme.converti((float)Total_TTC(),"BL");
            winToPrint.blocNomSte.Text = ser_systeme.findById(1).NomSociete;
                winToPrint.blockClient.Text = "Client : "+ ClientLabel.Content.ToString();
                winToPrint.blocNumType.Text = "Bon de livraison N° : " + String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:0000}", NumBL_Label.Content);
                winToPrint.blocAdresse.Text = ser_systeme.findById(1).adresse;
                winToPrint.blocTel.Text = "Tel: " + ser_systeme.findById(1).tel
                + "   Fax: " + ser_systeme.findById(1).fax;
                winToPrint.blocTVA.Text = "TVA:  "+ ser_systeme.findById(1).matriculeFiscale+"/"+ ser_systeme.findById(1).codeTVA+"/"+ ser_systeme.findById(1).codeCategorie+"/"+ ser_systeme.findById(1).etbSecondaire;
                winToPrint.blocEmail.Text = "E-mail:  " + ser_systeme.findById(1).email;
                winToPrint.blocTimbre1.Visibility = Visibility.Hidden;
                winToPrint.blocDate.Text = "Date : " + DateBL.Text;
            winToPrint.numPage.Text = "Page : " + curentPage.ToString() + "/" + numTotPage.ToString();
             if(manyPage == false)
             {
                   winToPrint.blocTotalHT.Text =String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", Total_HT());
                   winToPrint.bloctotalTva.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", Total_tva());
                   winToPrint.blocTotalNet.Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", Total_TTC());
                   winToPrint.blocSomme.Text = sEnlettre;

                SortedList<int, decimal> aa = new SortedList<int, decimal>();
                aa.Add(0, 0);
                aa.Add(7, 0);
                aa.Add(13, 0);
                aa.Add(19, 0);
             
                foreach (var item in Liste_Lignes)
                {
                    if( double.Parse( item.tva_UI.Text) == 00.00)
                    {
                        aa[0] += item.tot_ht_UI.Value.Value;
                        winToPrint.textBlock00.Visibility = Visibility.Visible;
                    }
                    if (double.Parse(item.tva_UI.Text) == 07.00)
                    {
                        aa[7] += item.tot_ht_UI.Value.Value;
                        winToPrint.textBlock6.Visibility = Visibility.Visible;
                    }
                    if (double.Parse(item.tva_UI.Text) == 13.00)
                    {
                        aa[13] += item.tot_ht_UI.Value.Value;
                        winToPrint.textBlock12.Visibility = Visibility.Visible;
                    }
                    if (double.Parse(item.tva_UI.Text) == 19.00)
                    {
                        aa[19] += item.tot_ht_UI.Value.Value;
                        winToPrint.textBlock18.Visibility = Visibility.Visible;
                    }
           
                }
                winToPrint.base0textbloc.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", aa[0]);
                winToPrint.base18textbloc.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", aa[19]);
                winToPrint.base6textbloc.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", aa[7] );
                winToPrint.base12textbloc.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", aa[13] );
                winToPrint.taux6textbloc_Copy.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", Math.Round( (aa[7] * 7) / 100));
                winToPrint.taux12textbloc_Copy1.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", Math.Round((aa[13] * 13) / 100));
                winToPrint.taux18textbloc_Copy2.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", Math.Round((aa[19] * 19) / 100));
            }
              

        }

        private void gestionClientBTN_Click(object sender, RoutedEventArgs e)
        {
            ClientCRUD win = new ClientCRUD();
            win.ShowDialog();
        }

        private void getFactureBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentCient != null )
            {
                Win_GetFactureForBl win = new Win_GetFactureForBl(currentCient);
                win.ShowDialog();
                if (win.currentFacture != null)
                { 
                NumFactureLabel.Content = win.currentFacture.Num;
                currentFacture = win.currentFacture;
                }
                WindowStatuChanged = true;
            }
        }

        private void OnClosingNoAltF4(object sender, System.ComponentModel.CancelEventArgs e)
        {
           if(altF4Pressed)
            {
                e.Cancel = true;
                altF4Pressed = false;
            }
           
           
        }
        void c_ControlChanged(object sender, EventArgs e)
        {
            WindowStatuChanged = true;
        }
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        void PrintFacture_Click(object sender, RoutedEventArgs e)
        {
            if(currentFacture != null && BL_Ajouter)
            imprimer_selected_facture_button_Click();
        }
        private void imprimer_selected_facture_button_Click()
        {
            if (currentFacture != null)
            {
                ser = new BonDeLivraisonService();
                ser_facture = new FactureService();
                bon = ser.findBonDeLivraisonByNum(bon.Num);
                currentFacture = ser_facture.findFactureByNum(currentFacture.Num);

                int total_ligne_frombl = 0;
                int total_bl;

                for (int i = 0; i < ser.findBonDeLivraisonBynumFacture(currentFacture).Count; i++)
                {
                    int num_bl_tmp = ser.findBonDeLivraisonBynumFacture(currentFacture)[i].Num;
                    total_ligne_frombl += ser_ligneBL.findLigneBlByNumBL(num_bl_tmp).Count;
                }
                total_bl = ser.findBonDeLivraisonBynumFacture(currentFacture).Count;
                int N_li_page = (total_ligne_frombl + total_bl) / 34;
                int Res = (total_ligne_frombl + total_bl) % 34;

                charge_Window_to_Print(N_li_page, Res, (total_ligne_frombl + total_bl));
            }
        }

        public void charge_Window_to_Print(int N_li_page, int Res, int all_ligne)
        {
            if (currentFacture != null)
            {
                currentFacture = ser_facture.findFactureByNum(currentFacture.Num);
                //string sEnlettre = somme.converti(float.Parse(ADAB.Get_TotalNet__BL_ByNumFacture(selected_facture).ToString()));
                List<Canvas> vc = new List<Canvas>();

                if (N_li_page < 1)
                {
                    WindowToPrint winToPrint = new WindowToPrint();
                    int va = 1;
                    for (int i = 0; i < ser.findBonDeLivraisonBynumFacture(currentFacture).Count; i++)
                    {
                        int num_bl_tmp = ser.findBonDeLivraisonBynumFacture(currentFacture)[i].Num;
                        printEng.Search(0, va, winToPrint.ArticleGrid).Text = "BL N° " + num_bl_tmp.ToString();
                        printEng.Search(1, va, winToPrint.ArticleGrid).Text = ser.findBonDeLivraisonBynumFacture(currentFacture)[i].date.ToShortDateString();
                        va++;
                        for (int j = 0; j < ser_ligneBL.findLigneBlByNumBL(num_bl_tmp).Count; j++)
                        {
                            printEng.Search(0, va, winToPrint.ArticleGrid).Text = ser_ligneBL.findLigneBlByNumBL(num_bl_tmp)[j].Ref_Produit;
                            printEng.Search(1, va, winToPrint.ArticleGrid).Text = ser_ligneBL.findLigneBlByNumBL(num_bl_tmp)[j].designation_li;
                            printEng.Search(2, va, winToPrint.ArticleGrid).Text = ser_ligneBL.findLigneBlByNumBL(num_bl_tmp)[j].qte_li.ToString();
                            printEng.Search(3, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_ligneBL.findLigneBlByNumBL(num_bl_tmp)[j].prix_HT);
                            printEng.Search(4, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n2}", ser_ligneBL.findLigneBlByNumBL(num_bl_tmp)[j].remise);
                            printEng.Search(5, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_ligneBL.findLigneBlByNumBL(num_bl_tmp)[j].tot_HT);
                            printEng.Search(6, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n2}", ser_ligneBL.findLigneBlByNumBL(num_bl_tmp)[j].tva);
                            printEng.Search(7, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_ligneBL.findLigneBlByNumBL(num_bl_tmp)[j].tot_TTC);
                            va++;
                        }
                    }

                    SetWindowToPrintBL2(winToPrint, false, 1, 1);
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
                    for (int i = 0; i < ser.findBonDeLivraisonBynumFacture(currentFacture).Count; i++)
                    {
                        int num_bl_tmp = ser.findBonDeLivraisonBynumFacture(currentFacture)[i].Num;
                        printEng.Search(0, va, winToPrint.ArticleGrid).Text = "BL N° " + num_bl_tmp.ToString();
                        printEng.Search(1, va, winToPrint.ArticleGrid).Text = ser.findBonDeLivraisonBynumFacture(currentFacture)[i].date.ToShortDateString();
                        va++;
                        vaa++;

                        for (int j = 0; j < ser_ligneBL.findLigneBlByNumBL(num_bl_tmp).Count; j++)
                        {
                            printEng.Search(0, va, winToPrint.ArticleGrid).Text = ser_ligneBL.findLigneBlByNumBL(num_bl_tmp)[j].Ref_Produit;
                            printEng.Search(1, va, winToPrint.ArticleGrid).Text = ser_ligneBL.findLigneBlByNumBL(num_bl_tmp)[j].designation_li;
                            printEng.Search(2, va, winToPrint.ArticleGrid).Text = ser_ligneBL.findLigneBlByNumBL(num_bl_tmp)[j].qte_li.ToString();
                            printEng.Search(3, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_ligneBL.findLigneBlByNumBL(num_bl_tmp)[j].prix_HT);
                            printEng.Search(4, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n2}", ser_ligneBL.findLigneBlByNumBL(num_bl_tmp)[j].remise);
                            printEng.Search(5, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_ligneBL.findLigneBlByNumBL(num_bl_tmp)[j].tot_HT);
                            printEng.Search(6, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n2}", ser_ligneBL.findLigneBlByNumBL(num_bl_tmp)[j].tva);
                            printEng.Search(7, va, winToPrint.ArticleGrid).Text = String.Format(System.Globalization.CultureInfo.GetCultureInfo("id-ID"), "{0:n0}", ser_ligneBL.findLigneBlByNumBL(num_bl_tmp)[j].tot_TTC);
                            va++;
                            vaa++;
                            if ((va - 1) % 34 == 0)
                            {
                                if (vaa == all_ligne)
                                {
                                    SetWindowToPrintBL2(winToPrint, false, currentPage, nbrePageConcraite);
                                    hleg = true;

                                }
                                if (vaa < all_ligne)
                                {
                                    SetWindowToPrintBL2(winToPrint, true, currentPage, nbrePageConcraite);
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
                        SetWindowToPrintBL2(winToPrint, false, currentPage, nbrePageConcraite);
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

        public void SetWindowToPrintBL2(WindowToPrint winToPrint, bool manyPage, int curentPage, int numTotPage)
        {
            string sEnlettre = somme.converti((float)currentFacture.tot_ttc, "Facture");
            winToPrint.blocNomSte.Text = ser_systeme.findById(1).NomSociete;
            winToPrint.blockClient.Text = "Client : " + currentCient.nom + "\n" + "Code TVA : " + currentCient.matricule + "/" + currentCient.code + "/" + currentCient.code_cat + "/" + currentCient.etb_sec;
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
                List<BonDeLivraison> listeBL = ser.findBonDeLivraisonBynumFacture(currentFacture);
                foreach (var item in listeBL)
                {
                    listeLigneBl.AddRange(ser_ligneBL.findLigneBlByNumBL(item.Num));
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
    }
}
