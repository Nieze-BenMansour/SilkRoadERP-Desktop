using Domain.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Ste.Fenetre.ProduitFolder;

namespace Ste
{
    class LigneDynamique
    {
        public Fournisseur four_fromm_update { get; set; }
        ProduitService ser_prod = new ProduitService();
        SystemeService ser_systeme = new SystemeService();
      public  List<LignrBLclass> Liste_Lignes;
      public  Grid GridBL;
        // Work for BL
        public void CreateRow()
        {
            GridBL.Height += 35;
            RowDefinition newRow = new RowDefinition();
            newRow.Height = new GridLength(0, GridUnitType.Auto);
            GridBL.RowDefinitions.Insert(GridBL.RowDefinitions.Count - 1, newRow);

            int rowIndex = GridBL.RowDefinitions.Count - 2;

            LignrBLclass ligne = new LignrBLclass();
            //Set event Handler 
            ligne.b_del.Click += new RoutedEventHandler(Supprimer_Row);
            ligne.b_liste_Article.Click += new RoutedEventHandler(Get_liste_prod);
            ligne.refUI.TextChanged += new RoutedEventHandler(Ref_TextChanged);
            ligne.desiUI.TextChanged += new RoutedEventHandler(desi_TextChanged);
            ligne.prixHT_UI.ValueChanged += new RoutedPropertyChangedEventHandler<object>(prixHT_Value_Changed);
            ligne.prixHT_UI.KeyDown += new KeyEventHandler(Get_prix_Achat);

            ligne.qteUI.ValueChanged += new RoutedPropertyChangedEventHandler<object>(Qte_Value_Changed);
            ligne.remise_UI.TextChanged += new TextChangedEventHandler(Remise_Value_Changed);
            ligne.tot_ht_UI.ValueChanged += new RoutedPropertyChangedEventHandler<object>(TOT_HT_Value_Changed);
            ligne.tva_UI.TextChanged += new TextChangedEventHandler(TVA_Value_Changed);
            //ajouter une ligne a la liste de class lignrBlClass
            Liste_Lignes.Add(ligne);

            ligne.num_Ligne.Content = Liste_Lignes.Count.ToString();

            Row_Charge(ligne.num_Ligne, rowIndex, 0, GridBL);
            Row_Charge(ligne.b_del, rowIndex, 1, GridBL);
            Row_Charge(ligne.b_liste_Article, rowIndex, 2, GridBL);
            Row_Charge(ligne.refUI, rowIndex, 3, GridBL);
            Row_Charge(ligne.desiUI, rowIndex, 4, GridBL);
            Row_Charge(ligne.qteUI, rowIndex, 5, GridBL);
            Row_Charge(ligne.prixHT_UI, rowIndex, 6, GridBL);
            Row_Charge(ligne.remise_UI, rowIndex, 7, GridBL);
            Row_Charge(ligne.tot_ht_UI, rowIndex, 8, GridBL);
            Row_Charge(ligne.tva_UI, rowIndex, 9, GridBL);
            Row_Charge(ligne.tot_ttc_UI, rowIndex, 10, GridBL);
          

            button_plus_BL_load();
            ligne.b_liste_Article.Focus();
            //  addButton.Tag = ligne.num_Ligne;         
        }
        public void CreateRow_BL_Load(LignrBLclass ligne)
        {
            GridBL.Height += 35;
            RowDefinition newRow = new RowDefinition();
            newRow.Height = new GridLength(0, GridUnitType.Auto);
            GridBL.RowDefinitions.Insert(GridBL.RowDefinitions.Count - 1, newRow);

            int rowIndex = GridBL.RowDefinitions.Count - 2;

            //ajouter une ligne a la liste de class lignrBlClass
            Liste_Lignes.Add(ligne);
            //affectation des text box 
            ligne.num_Ligne.Content = Liste_Lignes.Count.ToString();
            Row_Charge(ligne.num_Ligne, rowIndex, 0, GridBL);
            Row_Charge(ligne.b_del, rowIndex, 1, GridBL);
            Row_Charge(ligne.b_liste_Article, rowIndex, 2, GridBL);
            Row_Charge(ligne.refUI, rowIndex, 3, GridBL);
            Row_Charge(ligne.desiUI, rowIndex, 4, GridBL);
            Row_Charge(ligne.qteUI, rowIndex, 5, GridBL);
            Row_Charge(ligne.prixHT_UI, rowIndex, 6, GridBL);
            Row_Charge(ligne.remise_UI, rowIndex, 7, GridBL);
            Row_Charge(ligne.tot_ht_UI, rowIndex, 8, GridBL);
            Row_Charge(ligne.tva_UI, rowIndex, 9, GridBL);
            Row_Charge(ligne.tot_ttc_UI, rowIndex, 10, GridBL);
           
        }
        public void set_event_handler_bl_load()
        {
            //Set event Handler 
            foreach (LignrBLclass ligne in Liste_Lignes)
            {
                ligne.b_del.Click += new RoutedEventHandler(Supprimer_Row);
                ligne.b_liste_Article.Click += new RoutedEventHandler(Get_liste_prod);
                ligne.refUI.TextChanged += new RoutedEventHandler(Ref_TextChanged);
                ligne.desiUI.TextChanged += new RoutedEventHandler(desi_TextChanged);
                ligne.prixHT_UI.ValueChanged += new RoutedPropertyChangedEventHandler<object>(prixHT_Value_Changed);
                ligne.prixHT_UI.KeyDown += new KeyEventHandler(Get_prix_Achat);

                ligne.qteUI.ValueChanged += new RoutedPropertyChangedEventHandler<object>(Qte_Value_Changed);
                ligne.remise_UI.TextChanged += new TextChangedEventHandler(Remise_Value_Changed);
                ligne.tot_ht_UI.ValueChanged += new RoutedPropertyChangedEventHandler<object>(TOT_HT_Value_Changed);
                ligne.tva_UI.TextChanged += new TextChangedEventHandler(TVA_Value_Changed);
            }
        }
        public void button_plus_BL_load()
        {
            int rowIndex = GridBL.RowDefinitions.Count - 2;
            Button addButton = new Button();
            addButton.Content = "+";
            addButton.FontSize = 20;
            addButton.FontWeight = FontWeights.Bold;
            addButton.Click += new RoutedEventHandler(b_Click);

            RowDefinition laster = new RowDefinition();
            laster.Height = new GridLength(0, GridUnitType.Auto);
            GridBL.RowDefinitions.Insert(GridBL.RowDefinitions.Count - 1, laster);

            Grid.SetRow(addButton, rowIndex + 1);
            Grid.SetColumn(addButton, 0);

            GridBL.Children.Add(addButton);
        }
        public void b_Click(object sender, RoutedEventArgs e)
        {
            CreateRow();
            Control button = (Control)sender;
            //  UIElement editControl = (UIElement)button.Tag;
            GridBL.Children.Remove(button);
            //  Grid.SetColumn(editControl, 0);
            //  Grid.SetColumnSpan(editControl, 2);
        }
        //work for Devis
        public void CreateRowDevis()
        {
            GridBL.Height += 35;
            RowDefinition newRow = new RowDefinition();
            newRow.Height = new GridLength(0, GridUnitType.Auto);
            GridBL.RowDefinitions.Insert(GridBL.RowDefinitions.Count - 1, newRow);

            int rowIndex = GridBL.RowDefinitions.Count - 2;

            LignrBLclass ligne = new LignrBLclass();
            //Set event Handler 
            ligne.b_del.Click += new RoutedEventHandler(Supprimer_Row);
            ligne.b_liste_Article.Click += new RoutedEventHandler(Get_liste_prod);
            ligne.refUI.TextChanged += new RoutedEventHandler(Ref_TextChanged);
            ligne.desiUI.TextChanged += new RoutedEventHandler(desi_TextChanged);
            ligne.prixHT_UI.ValueChanged += new RoutedPropertyChangedEventHandler<object>(prixHT_Value_Changed);
            ligne.prixHT_UI.KeyDown += new KeyEventHandler(Get_prix_Achat);

            ligne.qteUI.ValueChanged += new RoutedPropertyChangedEventHandler<object>(Qte_Value_Changed);
            ligne.remise_UI.TextChanged += new TextChangedEventHandler(Remise_Value_Changed);
            ligne.tot_ht_UI.ValueChanged += new RoutedPropertyChangedEventHandler<object>(TOT_HT_Value_Changed);
            ligne.tva_UI.TextChanged += new TextChangedEventHandler(TVA_Value_Changed);
           // ligne.livrer_UI.Click += new RoutedEventHandler(livrer_Checked);
            //ajouter une ligne a la liste de class lignrBlClass
            Liste_Lignes.Add(ligne);

            ligne.num_Ligne.Content = Liste_Lignes.Count.ToString();

            Row_Charge(ligne.num_Ligne, rowIndex, 0, GridBL);
            Row_Charge(ligne.b_del, rowIndex, 1, GridBL);
            Row_Charge(ligne.b_liste_Article, rowIndex, 2, GridBL);
            Row_Charge(ligne.refUI, rowIndex, 3, GridBL);
            Row_Charge(ligne.desiUI, rowIndex, 4, GridBL);
            Row_Charge(ligne.qteUI, rowIndex, 5, GridBL);
            Row_Charge(ligne.prixHT_UI, rowIndex, 6, GridBL);
            Row_Charge(ligne.remise_UI, rowIndex, 7, GridBL);
            Row_Charge(ligne.tot_ht_UI, rowIndex, 8, GridBL);
            Row_Charge(ligne.tva_UI, rowIndex, 9, GridBL);
            Row_Charge(ligne.tot_ttc_UI, rowIndex, 10, GridBL);
           // Row_Charge(ligne.livrer_UI, rowIndex, 11, GridBL);
          //  Row_Charge(ligne.qteLivUI, rowIndex, 12, GridBL);

            button_plus_devis_load();
            ligne.b_liste_Article.Focus();
        }
        public void CreateRowDevisLoad(LignrBLclass ligne)
        {
            GridBL.Height += 35;
            RowDefinition newRow = new RowDefinition();
            newRow.Height = new GridLength(0, GridUnitType.Auto);
            GridBL.RowDefinitions.Insert(GridBL.RowDefinitions.Count - 1, newRow);

            int rowIndex = GridBL.RowDefinitions.Count - 2;

            //ajouter une ligne a la liste de class lignrBlClass
            Liste_Lignes.Add(ligne);
            //affectation des text box 
            ligne.num_Ligne.Content = Liste_Lignes.Count.ToString();
            Row_Charge(ligne.num_Ligne, rowIndex, 0, GridBL);
            Row_Charge(ligne.b_del, rowIndex, 1, GridBL);
            Row_Charge(ligne.b_liste_Article, rowIndex, 2, GridBL);
            Row_Charge(ligne.refUI, rowIndex, 3, GridBL);
            Row_Charge(ligne.desiUI, rowIndex, 4, GridBL);
            Row_Charge(ligne.qteUI, rowIndex, 5, GridBL);
            Row_Charge(ligne.prixHT_UI, rowIndex, 6, GridBL);
            Row_Charge(ligne.remise_UI, rowIndex, 7, GridBL);
            Row_Charge(ligne.tot_ht_UI, rowIndex, 8, GridBL);
            Row_Charge(ligne.tva_UI, rowIndex, 9, GridBL);
            Row_Charge(ligne.tot_ttc_UI, rowIndex, 10, GridBL);

        }
        public void set_event_handler_Devis_Load()
        {
         
            //Set event Handler 
            foreach(LignrBLclass ligne in Liste_Lignes)
            {
            ligne.b_del.Click += new RoutedEventHandler(Supprimer_Row);
            ligne.b_liste_Article.Click += new RoutedEventHandler(Get_liste_prod);
            ligne.refUI.TextChanged += new RoutedEventHandler(Ref_TextChanged);
            ligne.desiUI.TextChanged += new RoutedEventHandler(desi_TextChanged);
            ligne.prixHT_UI.ValueChanged += new RoutedPropertyChangedEventHandler<object>(prixHT_Value_Changed);
            ligne.prixHT_UI.KeyDown += new KeyEventHandler(Get_prix_Achat);

            ligne.qteUI.ValueChanged += new RoutedPropertyChangedEventHandler<object>(Qte_Value_Changed);
            ligne.remise_UI.TextChanged += new TextChangedEventHandler(Remise_Value_Changed);
            ligne.tot_ht_UI.ValueChanged += new RoutedPropertyChangedEventHandler<object>(TOT_HT_Value_Changed);
            ligne.tva_UI.TextChanged += new TextChangedEventHandler(TVA_Value_Changed);
            }
        }
        public void button_plus_devis_load()
        {
            int rowIndex = GridBL.RowDefinitions.Count - 2;
            Button addButton = new Button();
            addButton.Content = "+";
            addButton.FontSize = 20;
            addButton.FontWeight = FontWeights.Bold;
            addButton.Click += new RoutedEventHandler(b_Click_Devis);

            RowDefinition laster = new RowDefinition();
            laster.Height = new GridLength(0, GridUnitType.Auto);
            GridBL.RowDefinitions.Insert(GridBL.RowDefinitions.Count - 1, laster);

            Grid.SetRow(addButton, rowIndex + 1);
            Grid.SetColumn(addButton, 0);

            GridBL.Children.Add(addButton);
        }
        public void b_Click_Devis(object sender, RoutedEventArgs e)
       {
           CreateRowDevis();
           Control button = (Control)sender;
           GridBL.Children.Remove(button);
       }


        //work for Bon de réception
        public void CreateRowBR(Fournisseur four)
        {
            GridBL.Height += 35;
            RowDefinition newRow = new RowDefinition();
            newRow.Height = new GridLength(0, GridUnitType.Auto);
            GridBL.RowDefinitions.Insert(GridBL.RowDefinitions.Count - 1, newRow);

            int rowIndex = GridBL.RowDefinitions.Count - 2;

            LignrBLclass ligne = new LignrBLclass();
            //Set event Handler 
            ligne.b_del.Click += new RoutedEventHandler(Supprimer_Row);
            ligne.b_liste_Article.Click += new RoutedEventHandler(Get_liste_prod);
            ligne.refUI.TextChanged += new RoutedEventHandler(Ref_TextChanged);
            ligne.desiUI.TextChanged += new RoutedEventHandler(desi_TextChanged);
            ligne.prixHT_UI.ValueChanged += new RoutedPropertyChangedEventHandler<object>(prixHT_Value_Changed);
            ligne.prixHT_UI.KeyDown += new KeyEventHandler(Get_prix_Achat);

            ligne.qteUI.ValueChanged += new RoutedPropertyChangedEventHandler<object>(Qte_Value_Changed);
            ligne.remise_UI.TextChanged += new TextChangedEventHandler(Remise_Value_Changed);

            // controle sur la nature du fournisseur constructeur ou nn
            if (four.constructeur)
            {
                ligne.tot_ht_UI.ValueChanged += new RoutedPropertyChangedEventHandler<object>(TOT_HT_Value_Changed_Fodec);
                ligne.tva_UI.TextChanged += new TextChangedEventHandler(TVA_Value_Changed_Fodec);
            }
            else
            {
                ligne.tot_ht_UI.ValueChanged += new RoutedPropertyChangedEventHandler<object>(TOT_HT_Value_Changed);
                ligne.tva_UI.TextChanged += new TextChangedEventHandler(TVA_Value_Changed);
            }

            // ligne.livrer_UI.Click += new RoutedEventHandler(livrer_Checked);
            //ajouter une ligne a la liste de class lignrBlClass
            Liste_Lignes.Add(ligne);

            ligne.num_Ligne.Content = Liste_Lignes.Count.ToString();

            Row_Charge(ligne.num_Ligne, rowIndex, 0, GridBL);
            Row_Charge(ligne.b_del, rowIndex, 1, GridBL);
            Row_Charge(ligne.b_liste_Article, rowIndex, 2, GridBL);
            Row_Charge(ligne.refUI, rowIndex, 3, GridBL);
            Row_Charge(ligne.desiUI, rowIndex, 4, GridBL);
            Row_Charge(ligne.qteUI, rowIndex, 5, GridBL);
            Row_Charge(ligne.prixHT_UI, rowIndex, 6, GridBL);
            Row_Charge(ligne.remise_UI, rowIndex, 7, GridBL);
            Row_Charge(ligne.tot_ht_UI, rowIndex, 8, GridBL);
            Row_Charge(ligne.tva_UI, rowIndex, 9, GridBL);
            Row_Charge(ligne.tot_ttc_UI, rowIndex, 10, GridBL);
            // Row_Charge(ligne.livrer_UI, rowIndex, 11, GridBL);
            //  Row_Charge(ligne.qteLivUI, rowIndex, 12, GridBL);

            button_plus_BR_load(four);
            ligne.b_liste_Article.Focus();
        }
        public void CreateRowBRLoad(LignrBLclass ligne)
        {
            GridBL.Height += 35;
            RowDefinition newRow = new RowDefinition();
            newRow.Height = new GridLength(0, GridUnitType.Auto);
            GridBL.RowDefinitions.Insert(GridBL.RowDefinitions.Count - 1, newRow);

            int rowIndex = GridBL.RowDefinitions.Count - 2;

            //ajouter une ligne a la liste de class lignrBlClass
            Liste_Lignes.Add(ligne);
            //affectation des text box 
            ligne.num_Ligne.Content = Liste_Lignes.Count.ToString();
            Row_Charge(ligne.num_Ligne, rowIndex, 0, GridBL);
            Row_Charge(ligne.b_del, rowIndex, 1, GridBL);
            Row_Charge(ligne.b_liste_Article, rowIndex, 2, GridBL);
            Row_Charge(ligne.refUI, rowIndex, 3, GridBL);
            Row_Charge(ligne.desiUI, rowIndex, 4, GridBL);
            Row_Charge(ligne.qteUI, rowIndex, 5, GridBL);
            Row_Charge(ligne.prixHT_UI, rowIndex, 6, GridBL);
            Row_Charge(ligne.remise_UI, rowIndex, 7, GridBL);
            Row_Charge(ligne.tot_ht_UI, rowIndex, 8, GridBL);
            Row_Charge(ligne.tva_UI, rowIndex, 9, GridBL);
            Row_Charge(ligne.tot_ttc_UI, rowIndex, 10, GridBL);

        }
        public void set_event_handler_BR_Load(Fournisseur four)
        {

            //Set event Handler 
            foreach (LignrBLclass ligne in Liste_Lignes)
            {
                ligne.b_del.Click += new RoutedEventHandler(Supprimer_Row);
                ligne.b_liste_Article.Click += new RoutedEventHandler(Get_liste_prod);
                ligne.refUI.TextChanged += new RoutedEventHandler(Ref_TextChanged);
                ligne.desiUI.TextChanged += new RoutedEventHandler(desi_TextChanged);
                ligne.prixHT_UI.ValueChanged += new RoutedPropertyChangedEventHandler<object>(prixHT_Value_Changed);
                ligne.prixHT_UI.KeyDown += new KeyEventHandler(Get_prix_Achat);

                ligne.qteUI.ValueChanged += new RoutedPropertyChangedEventHandler<object>(Qte_Value_Changed);
                ligne.remise_UI.TextChanged += new TextChangedEventHandler(Remise_Value_Changed);
                // controle sur la nature du fournisseur constructeur ou nn
                if (four.constructeur)
                {
                    ligne.tot_ht_UI.ValueChanged += new RoutedPropertyChangedEventHandler<object>(TOT_HT_Value_Changed_Fodec);
                    ligne.tva_UI.TextChanged += new TextChangedEventHandler(TVA_Value_Changed_Fodec);
                }
                else
                {
                    ligne.tot_ht_UI.ValueChanged += new RoutedPropertyChangedEventHandler<object>(TOT_HT_Value_Changed);
                    ligne.tva_UI.TextChanged += new TextChangedEventHandler(TVA_Value_Changed);
                }
            }
        }
        public void button_plus_BR_load(Fournisseur four)
        {
            four_fromm_update = four ;
            int rowIndex = GridBL.RowDefinitions.Count - 2;
            Button addButton = new Button();
            addButton.Content = "+";
            addButton.FontSize = 20;
            addButton.FontWeight = FontWeights.Bold;
            addButton.Click += new RoutedEventHandler(b_Click_BR);

            RowDefinition laster = new RowDefinition();
            laster.Height = new GridLength(0, GridUnitType.Auto);
            GridBL.RowDefinitions.Insert(GridBL.RowDefinitions.Count - 1, laster);

            Grid.SetRow(addButton, rowIndex + 1);
            Grid.SetColumn(addButton, 0);

            GridBL.Children.Add(addButton);
        }
        public void b_Click_BR(object sender, RoutedEventArgs e)
        {
            CreateRowBR(four_fromm_update);
            Control button = (Control)sender;
            GridBL.Children.Remove(button);
        }

        // fonction row charge affectation de UI a un grid donné
        void Row_Charge(UIElement el, int row_ind, int col, Grid G)
        {
            Grid.SetRow(el, row_ind);
            Grid.SetColumn(el, col);
            Grid.SetRowSpan(el, 1);
            Grid.SetColumnSpan(el, 1); // Change this if you want.
            G.Children.Add(el);
            /*  Grid.SetRow(ligne.refUI, rowIndex);
            Grid.SetColumn(ligne.refUI, 1);
            Grid.SetRowSpan(ligne.refUI, 1);
            Grid.SetColumnSpan(ligne.refUI, 1); // Change this if you want.
            GridBL.Children.Add(ligne.refUI);*/
        }
        // Events
        private void Ref_TextChanged(object sender, EventArgs e)
        {
      //      Ste.DatabaseSteDataSetTableAdapters.ProduitTableAdapter ADAB = new Ste.DatabaseSteDataSetTableAdapters.ProduitTableAdapter();
            var u = sender as AutoCompleteBox;
            for (int i = 0; i < Liste_Lignes.Count; i++)
            {
                if (Liste_Lignes[i].refUI.GetHashCode() == u.GetHashCode())
                {
                    if (u.Text.Length == 0)//888
                    {
                        Liste_Lignes[i].desiUI.TextChanged -= new RoutedEventHandler(desi_TextChanged);
                        Liste_Lignes[i].desiUI.Text = null;
                        Liste_Lignes[i].qteUI.Value = 1;
                        Liste_Lignes[i].prixHT_UI.Text = "000";
                        Liste_Lignes[i].remise_UI.Text = "00,00";
                        Liste_Lignes[i].desiUI.TextChanged += new RoutedEventHandler(desi_TextChanged);
                    }
                    else
                    {
                        try
                        {
                            Liste_Lignes[i].desiUI.TextChanged -= new RoutedEventHandler(desi_TextChanged);
                            Liste_Lignes[i].desiUI.Text = ser_prod.findProduitByRefUnique(u.Text).nom;
                            Liste_Lignes[i].qteUI.Value = 1;
                            Liste_Lignes[i].prixHT_UI.Text = ser_prod.findProduitByRefUnique(u.Text).prix.ToString();
                            Liste_Lignes[i].remise_UI.Text = string.Format("{0:00.00}", ser_prod.findProduitByRefUnique(u.Text).remise);
                            Liste_Lignes[i].desiUI.TextChanged += new RoutedEventHandler(desi_TextChanged);
                        }
                        catch (Exception) { }
                    }

                }
            }
        }
        private void desi_TextChanged(object sender, EventArgs e)
        {
          //  Ste.DatabaseSteDataSetTableAdapters.ProduitTableAdapter ADAB = new Ste.DatabaseSteDataSetTableAdapters.ProduitTableAdapter();
            var u = sender as AutoCompleteBox;
            for (int i = 0; i < Liste_Lignes.Count; i++)
            {

                if (Liste_Lignes[i].desiUI.GetHashCode() == u.GetHashCode())
                {
                    if (u.Text.Length == 0)
                    {
                        Liste_Lignes[i].refUI.TextChanged -= new RoutedEventHandler(Ref_TextChanged);
                        Liste_Lignes[i].refUI.Text = null;
                        Liste_Lignes[i].qteUI.Value = 1;
                        Liste_Lignes[i].prixHT_UI.Text = "000";
                        Liste_Lignes[i].remise_UI.Text = "00,00";
                        Liste_Lignes[i].refUI.TextChanged += new RoutedEventHandler(Ref_TextChanged);
                    }
                    else
                    {
                        try
                        {
                            Liste_Lignes[i].refUI.TextChanged -= new RoutedEventHandler(Ref_TextChanged);
                            Liste_Lignes[i].refUI.Text = ser_prod.findProduitByDesignation(u.Text)[0].refe;
                            Liste_Lignes[i].qteUI.Value = 1;
                            Liste_Lignes[i].prixHT_UI.Text = ser_prod.findProduitByDesignation(u.Text)[0].prix.ToString();
                            Liste_Lignes[i].remise_UI.Text = string.Format("{0:00.00}", ser_prod.findProduitByDesignation(u.Text)[0].remise);
                            Liste_Lignes[i].refUI.TextChanged += new RoutedEventHandler(Ref_TextChanged);
                        }
                        catch (Exception) { }
                    }
                }
            }
        }
        private void prixHT_Value_Changed(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
       //     Ste.DatabaseSteDataSetTableAdapters.ProduitTableAdapter ADAB = new Ste.DatabaseSteDataSetTableAdapters.ProduitTableAdapter();
            var u = sender as Xceed.Wpf.Toolkit.DecimalUpDown;
            for (int i = 0; i < Liste_Lignes.Count; i++)
            {
                if (Liste_Lignes[i].prixHT_UI.GetHashCode() == u.GetHashCode())
                {
                    try
                    {
                        int qte = (int)Liste_Lignes[i].qteUI.Value;
                        decimal prixHT = (decimal)u.Value;
                        float remise = float.Parse(Liste_Lignes[i].remise_UI.Text);
                        decimal tot = prixHT * qte;
                        var non_Round = tot - (tot * (decimal)remise) / 100;
                        var tot_HT = Math.Round(non_Round);
                        Liste_Lignes[i].tot_ht_UI.Text = tot_HT.ToString();
                    }
                    catch (FormatException) { }
                    catch (InvalidOperationException) { }
                }
            }
        }
        private void Qte_Value_Changed(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
         //   Ste.DatabaseSteDataSetTableAdapters.ProduitTableAdapter ADAB = new Ste.DatabaseSteDataSetTableAdapters.ProduitTableAdapter();
            var u = sender as Xceed.Wpf.Toolkit.IntegerUpDown;
            for (int i = 0; i < Liste_Lignes.Count; i++)
            {

                if (Liste_Lignes[i].qteUI.GetHashCode() == u.GetHashCode())
                {
                    try
                    {
                        int qte = (int)u.Value;
                        decimal prixHT = (decimal)Liste_Lignes[i].prixHT_UI.Value;
                        float remise = float.Parse(Liste_Lignes[i].remise_UI.Text);
                        decimal tot = prixHT * qte;
                        var non_Round = tot - (tot * (decimal)remise) / 100;
                        var tot_HT = Math.Round(non_Round);
                        Liste_Lignes[i].tot_ht_UI.Text = tot_HT.ToString();
                        
                    }
                    catch (FormatException) { }
                    catch (InvalidOperationException) { }
                }
            }

        }
        private void Remise_Value_Changed(object sender, EventArgs e)
        {
        //    Ste.DatabaseSteDataSetTableAdapters.ProduitTableAdapter ADAB = new Ste.DatabaseSteDataSetTableAdapters.ProduitTableAdapter();
            var u = sender as Xceed.Wpf.Toolkit.MaskedTextBox;
            for (int i = 0; i < Liste_Lignes.Count; i++)
            {
                if (Liste_Lignes[i].remise_UI.GetHashCode() == u.GetHashCode())
                {
                    try
                    {
                        int qte = (int)Liste_Lignes[i].qteUI.Value;
                        decimal prixHT = (decimal)Liste_Lignes[i].prixHT_UI.Value;
                        float remise = float.Parse(u.Text);
                        decimal tot = prixHT * qte;
                        var non_Round = tot - (tot * (decimal)remise) / 100;
                        var tot_HT = Math.Round(non_Round);
                        Liste_Lignes[i].tot_ht_UI.Text = tot_HT.ToString();
                    }
                    catch (FormatException) { }
                    catch (InvalidOperationException) { }
                }
            }
        }
        private void TOT_HT_Value_Changed(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
        //    Ste.DatabaseSteDataSetTableAdapters.ProduitTableAdapter ADAB = new Ste.DatabaseSteDataSetTableAdapters.ProduitTableAdapter();
            var u = sender as Xceed.Wpf.Toolkit.DecimalUpDown;
            for (int i = 0; i < Liste_Lignes.Count; i++)
            {
                if (Liste_Lignes[i].tot_ht_UI.GetHashCode() == u.GetHashCode())
                {
                    try
                    {
                        float tva = float.Parse(Liste_Lignes[i].tva_UI.Text);
                        decimal totHT = (decimal)u.Value;
                        var tot_TTC = Math.Round(totHT + ((totHT * (decimal)tva) / 100));
                        Liste_Lignes[i].tot_ttc_UI.Text = tot_TTC.ToString();
                    }
                    catch (FormatException) { }
                    catch (InvalidOperationException) { }
                }
            }
        }
        private void TVA_Value_Changed(object sender, EventArgs e)
        {
          //  Ste.DatabaseSteDataSetTableAdapters.ProduitTableAdapter ADAB = new Ste.DatabaseSteDataSetTableAdapters.ProduitTableAdapter();
            var u = sender as Xceed.Wpf.Toolkit.MaskedTextBox;
            for (int i = 0; i < Liste_Lignes.Count; i++)
            {
                if (Liste_Lignes[i].tva_UI.GetHashCode() == u.GetHashCode())
                {
                    try
                    {
                        float tva = float.Parse(u.Text);
                        decimal totHT = (decimal)Liste_Lignes[i].tot_ht_UI.Value;
                        var tot_TTC = Math.Round(totHT + ((totHT * (decimal)tva) / 100));
                        Liste_Lignes[i].tot_ttc_UI.Text = tot_TTC.ToString();
                    }
                    catch (FormatException) { }
                    catch (InvalidOperationException) { }
                }
            }
        }
  
        private void Get_liste_prod(object sender, EventArgs e)
        {
            var lis = sender as Button;
            for (int i = 0; i < Liste_Lignes.Count; i++)
            {
                if (Liste_Lignes[i].b_liste_Article.GetHashCode() == lis.GetHashCode())
                {
                    WindowChercherProduitBL win = new WindowChercherProduitBL();
                    win.ShowDialog();
                    if (win.produitToSend.refe != null)
                    {
                        Liste_Lignes[i].refUI.Text = win.produitToSend.refe;
                    }
                }
            }
        }
        //F1 pressed
        private void Get_prix_Achat(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                
                    var u = sender as Xceed.Wpf.Toolkit.DecimalUpDown;
                    for (int i = 0; i < Liste_Lignes.Count; i++)
                    {
                        if (Liste_Lignes[i].prixHT_UI.GetHashCode() == u.GetHashCode())
                        {

                        try
                        {
                            WindowChercherProduitBL win = new WindowChercherProduitBL();
                            if (Liste_Lignes[i].refUI.Text.Length > 0)
                            {
                                win.REF = Liste_Lignes[i].refUI.Text;
                            }
                            win.refSearch.IsEnabled = false;
                            win.desisearch.IsEnabled = false;
                            win.ShowDialog();
                        }
                        catch (Exception)
                        {

                        }

                        }
                    }
                
               

            }
          else  if (e.Key == Key.F1)
            {
                var u = sender as Xceed.Wpf.Toolkit.DecimalUpDown;
                for (int i = 0; i < Liste_Lignes.Count; i++)
                {
                    if (Liste_Lignes[i].prixHT_UI.GetHashCode() == u.GetHashCode())
                    {

                        if (Liste_Lignes[i].refUI.Text.Length > 0)
                        {
                            Win_ChercherPrixAchatPar_Fournisseur win = new Win_ChercherPrixAchatPar_Fournisseur(Liste_Lignes[i].refUI.Text);
                            win.ShowDialog();
                        }
                        


                    }
                }

            }
        }
        private void Supprimer_Row(object sender, EventArgs e)
        {

            var u = sender as Button;
            for (int i = 0; i < Liste_Lignes.Count; i++)
            {
                if (Liste_Lignes[i].b_del.GetHashCode() == u.GetHashCode())
                {
                    //Row_Delete(i-1, GridBL);
                    //Liste_Lignes.RemoveAt(i);
                    //    GridBL.Children.Remove(Liste_Lignes[i].num_Ligne);
                    GridBL.Children.Remove(Liste_Lignes[i].num_Ligne);
                    GridBL.Children.Remove(Liste_Lignes[i].b_del);
                    GridBL.Children.Remove(Liste_Lignes[i].b_liste_Article);
                    GridBL.Children.Remove(Liste_Lignes[i].refUI);
                    GridBL.Children.Remove(Liste_Lignes[i].desiUI);
                    GridBL.Children.Remove(Liste_Lignes[i].qteUI);
                    GridBL.Children.Remove(Liste_Lignes[i].prixHT_UI);
                    GridBL.Children.Remove(Liste_Lignes[i].remise_UI);
                    GridBL.Children.Remove(Liste_Lignes[i].tot_ht_UI);
                    GridBL.Children.Remove(Liste_Lignes[i].tva_UI);
                    GridBL.Children.Remove(Liste_Lignes[i].tot_ttc_UI);
                  
                    for (int j = i; j < Liste_Lignes.Count; j++)
                    {
                        int jiji = int.Parse(Liste_Lignes[j].num_Ligne.Content.ToString()) - 1;
                        Liste_Lignes[j].num_Ligne.Content = jiji.ToString();
                        //  GridBL.RowDefinitions.Remove(GridBL.RowDefinitions.ElementAt(i));
                    }
                    Liste_Lignes.RemoveAt(i);

                }
            }

        }
        public void Supprimer_ALL_Row()
        {
            for(int i = 0; i < Liste_Lignes.Count; i++)
            {
                GridBL.Children.Remove(Liste_Lignes[i].num_Ligne);
                GridBL.Children.Remove(Liste_Lignes[i].b_del);
                GridBL.Children.Remove(Liste_Lignes[i].b_liste_Article);
                GridBL.Children.Remove(Liste_Lignes[i].refUI);
                GridBL.Children.Remove(Liste_Lignes[i].desiUI);
                GridBL.Children.Remove(Liste_Lignes[i].qteUI);
                GridBL.Children.Remove(Liste_Lignes[i].prixHT_UI);
                GridBL.Children.Remove(Liste_Lignes[i].remise_UI);
                GridBL.Children.Remove(Liste_Lignes[i].tot_ht_UI);
                GridBL.Children.Remove(Liste_Lignes[i].tva_UI);
                GridBL.Children.Remove(Liste_Lignes[i].tot_ttc_UI);
            }
            GridBL.Children.Clear();
            Liste_Lignes.Clear();
          
        }

        //Les Trois Totals
        public decimal Total_HT(List<LignrBLclass> Liste_LigneS)
        {
            decimal ret = 0;
            for (int i = 0; i < Liste_LigneS.Count; i++)
            {
                ret += (decimal)Liste_LigneS[i].tot_ht_UI.Value;
            }
            return ret;
        }
       public decimal Total_tva(List<LignrBLclass> Liste_LigneS)
        {
            decimal ret = 0;
            for (int i = 0; i < Liste_LigneS.Count; i++)
            {
                ret += (decimal)Liste_LigneS[i].tot_ttc_UI.Value - (decimal)Liste_LigneS[i].tot_ht_UI.Value;
            }
            return ret;
        }
       public decimal Total_TTC(List<LignrBLclass> Liste_LigneS)
        {
            decimal ret = 0;
            for (int i = 0; i < Liste_LigneS.Count; i++)
            {
                ret += (decimal)Liste_LigneS[i].tot_ttc_UI.Value;
            }
            return ret;
        }



        // event specéfique pour les bon de reception avec fodec

        private void TOT_HT_Value_Changed_Fodec(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //    Ste.DatabaseSteDataSetTableAdapters.ProduitTableAdapter ADAB = new Ste.DatabaseSteDataSetTableAdapters.ProduitTableAdapter();
            var u = sender as Xceed.Wpf.Toolkit.DecimalUpDown;
            for (int i = 0; i < Liste_Lignes.Count; i++)
            {
                if (Liste_Lignes[i].tot_ht_UI.GetHashCode() == u.GetHashCode())
                {
                    try
                    {
                        float tva = float.Parse(Liste_Lignes[i].tva_UI.Text);
                        decimal totHT = (decimal)u.Value;
                        var tot_TTC = totHT + (totHT * ser_systeme.findById(1).pourcentageFodec) / 100;
                         tot_TTC = Math.Round(tot_TTC + ((tot_TTC * (decimal)tva) / 100));
                        Liste_Lignes[i].tot_ttc_UI.Text = tot_TTC.ToString();
                    }
                    catch (FormatException) { }
                    catch (InvalidOperationException) { }
                }
            }
        }
        private void TVA_Value_Changed_Fodec(object sender, EventArgs e)
        {
            //  Ste.DatabaseSteDataSetTableAdapters.ProduitTableAdapter ADAB = new Ste.DatabaseSteDataSetTableAdapters.ProduitTableAdapter();
            var u = sender as Xceed.Wpf.Toolkit.MaskedTextBox;
            for (int i = 0; i < Liste_Lignes.Count; i++)
            {
                if (Liste_Lignes[i].tva_UI.GetHashCode() == u.GetHashCode())
                {
                    try
                    {
                        float tva = float.Parse(u.Text);
                        decimal totHT = (decimal)Liste_Lignes[i].tot_ht_UI.Value;
                        var tot_TTC = totHT + (totHT * ser_systeme.findById(1).pourcentageFodec) / 100;
                        tot_TTC = Math.Round(tot_TTC + ((tot_TTC * (decimal)tva) / 100));
                        Liste_Lignes[i].tot_ttc_UI.Text = tot_TTC.ToString();
                    }
                    catch (FormatException) { }
                    catch (InvalidOperationException) { }
                }
            }
        }
    }
}
