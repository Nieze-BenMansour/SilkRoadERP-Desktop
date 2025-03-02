using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Windows.Controls;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Service;

namespace Ste
{

   public class LignrBLclass 
    {
       public AutoCompleteBox refUI, desiUI ;
       public Xceed.Wpf.Toolkit.IntegerUpDown qteUI;
       public Xceed.Wpf.Toolkit.DecimalUpDown prixHT_UI, tot_ht_UI, tot_ttc_UI;
       public Xceed.Wpf.Toolkit.MaskedTextBox remise_UI, tva_UI;
       
       public Button b_liste_Article,b_del;
       public Label num_Ligne;

       public  LignrBLclass()
        {
            //     Ste.DatabaseSteDataSetTableAdapters.ProduitTableAdapter ADAB = new Ste.DatabaseSteDataSetTableAdapters.ProduitTableAdapter();
            ProduitService ser_prod = new ProduitService();
            //Label ***  num_Ligne  ***
            num_Ligne = new Label();
            num_Ligne.FontSize = 17;
            num_Ligne.FontWeight = FontWeights.Bold;
           //Button del button
            b_del = new Button();
            b_del.Content = "S";
           //Button ***  b_liste_Article  ***
            b_liste_Article = new Button();
            b_liste_Article.Content = "--";
           //Autocomplete reference
            refUI = new AutoCompleteBox();
            refUI.Height = 30;
            refUI.FontSize = 18;
            refUI.FontWeight = FontWeights.SemiBold;
            var RefList = new List<string>();
            foreach (var row in ser_prod.getAllProduit())
            {
                RefList.Add(row.refe);
            }
            refUI.ItemsSource = RefList;
            
            //Autocomplete designation
            desiUI = new AutoCompleteBox();
            desiUI.Height = 30;
            desiUI.FontSize = 18;
            desiUI.FontWeight = FontWeights.SemiBold; 
            var DesiList = new List<string>();
            foreach (var row in ser_prod.getAllProduit())
            {
                DesiList.Add(row.nom);
            }
            desiUI.ItemsSource = DesiList;
            //IntegerUpDown ***qte***
            qteUI = new Xceed.Wpf.Toolkit.IntegerUpDown();
            qteUI.Height = 30;
            qteUI.AllowSpin = false;
            qteUI.FontSize = 18;
            qteUI.FontWeight = FontWeights.SemiBold;
            qteUI.Minimum = 0;
            qteUI.Value = 0;
            qteUI.PreviewTextInput += new TextCompositionEventHandler(PreviewTextInput_nie);
            qteUI.LostFocus += new RoutedEventHandler(Qte_Lost_Focus);
            //decimalUpDown ***prixHT_UI***
            prixHT_UI = new Xceed.Wpf.Toolkit.DecimalUpDown();
            prixHT_UI.AllowSpin = false;
            prixHT_UI.Height = 30;
            prixHT_UI.FontSize = 18;
            prixHT_UI.FontWeight = FontWeights.SemiBold;
            prixHT_UI.Minimum = 0;
            prixHT_UI.Value = 0;
            prixHT_UI.PreviewTextInput += new TextCompositionEventHandler(PreviewTextInput_nie);
            prixHT_UI.FormatString = "N0";
            //MaskedTexbox ***remise_UI***
            remise_UI = new Xceed.Wpf.Toolkit.MaskedTextBox();
            remise_UI.Height = 30;
            remise_UI.FontSize = 18;
            remise_UI.FontWeight = FontWeights.SemiBold;
            remise_UI.Mask = "aa.aa";
            remise_UI.TextAlignment = TextAlignment.Right;
            remise_UI.Text = "00,00";
            remise_UI.AutoSelectBehavior = Xceed.Wpf.Toolkit.AutoSelectBehavior.OnFocus;
            remise_UI.PreviewTextInput += new TextCompositionEventHandler(PreviewTextInput_nie);
            remise_UI.LostFocus += new RoutedEventHandler(Remise_tva_Lost_Focus);
            //Label ***tot_ht_UI***
            tot_ht_UI = new Xceed.Wpf.Toolkit.DecimalUpDown();
            tot_ht_UI.Height = 30;
            tot_ht_UI.FontSize = 18;
            tot_ht_UI.FontWeight = FontWeights.SemiBold;
            tot_ht_UI.TextAlignment = TextAlignment.Right;
            tot_ht_UI.IsEnabled = false;
            tot_ht_UI.Foreground = Brushes.Black ;
            tot_ht_UI.FormatString = "N0";
            //MaskedTexbox ***tva_UI***
            tva_UI = new Xceed.Wpf.Toolkit.MaskedTextBox();
            tva_UI.Height = 30;
            tva_UI.FontSize = 18;
            tva_UI.FontWeight = FontWeights.SemiBold;
            tva_UI.Mask = "aa.aa";
            tva_UI.TextAlignment = TextAlignment.Right;
            tva_UI.Text = "19,00";
            tva_UI.AutoSelectBehavior = Xceed.Wpf.Toolkit.AutoSelectBehavior.OnFocus;
            tva_UI.PreviewTextInput += new TextCompositionEventHandler(PreviewTextInput_nie);
            tva_UI.LostFocus += new RoutedEventHandler(Remise_tva_Lost_Focus);
            //Label ***tot_ttc_UI***
            tot_ttc_UI =new Xceed.Wpf.Toolkit.DecimalUpDown();
            tot_ttc_UI.Height = 30;
            tot_ttc_UI.FontSize = 18;
            tot_ttc_UI.FontWeight = FontWeights.SemiBold;
            tot_ttc_UI.TextAlignment = TextAlignment.Right;
            tot_ttc_UI.IsEnabled = false;
            tot_ttc_UI.Foreground = Brushes.Black;
            tot_ttc_UI.FormatString = "N0";
        }
       private void PreviewTextInput_nie(object sender, TextCompositionEventArgs e)
       {
           try
           {
               if (!char.IsDigit(e.Text, e.Text.Length - 1))
               {
                   e.Handled = true;

               }
           }
           catch (ArgumentOutOfRangeException) { }
       }
       private void Remise_tva_Lost_Focus(object sender, RoutedEventArgs e)
       {
           float nu = 0;
           Xceed.Wpf.Toolkit.MaskedTextBox tb = sender as Xceed.Wpf.Toolkit.MaskedTextBox;
           if ( ! float.TryParse(tb.Text,out nu) )
           {
               tb.Text = "00,00";
              
           }
       }
       private void Qte_Lost_Focus(object sender, RoutedEventArgs e)
       {
          
           Xceed.Wpf.Toolkit.IntegerUpDown tb = sender as Xceed.Wpf.Toolkit.IntegerUpDown;
           if (tb.Value == null)
           {
               tb.Value = 1;

           }
       }
    }
}
