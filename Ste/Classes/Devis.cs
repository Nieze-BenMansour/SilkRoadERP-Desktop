using Domain.Models;
using Service;
using Ste.Fenetre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ste.Classes
{
    class Devis
    {
        ProduitService ser_prod = new ProduitService();
        List<Produit> toutProduitDansStock = new List<Produit>();
        public bool Devis_Valide(WindowDevis win)
        {
            bool test = true;
            if (win.DateBL.DisplayDate.Year != DateTime.Now.Year)
            {
                MessageBox.Show("Date non valide !", "Alerte", MessageBoxButton.OK, MessageBoxImage.Warning);
                test = false;
                return test;
            }
            if (win.CodeLabel.Content.ToString() == "")
            {
                MessageBox.Show("Client non sélectionné !", "Alerte", MessageBoxButton.OK, MessageBoxImage.Warning);
                test = false;
                return test;
            }
            return test;
        }
       
        public bool Lignes_Valide(List<LignrBLclass> Liste_Lignes)
        {
            toutProduitDansStock = ser_prod.getAllProduit();
            bool sortie = true;
            bool[] findet = new bool[Liste_Lignes.Count];
            for (int i = 0; i < Liste_Lignes.Count; i++)
            {
                findet[i] = false;
            }

            for (int i = 0; i < Liste_Lignes.Count; i++)
            {
                for (int j = 0; j < toutProduitDansStock.Count; j++)
                {
                    if (Liste_Lignes[i].refUI.Text == toutProduitDansStock[j].refe)
                    {
                        findet[i] = true;
                    }
                }
            }
            for (int i = 0; i < Liste_Lignes.Count; i++)
            {
                if (findet[i] == false) { sortie = false; }
            }
            if (sortie == false)
            {
                MessageBox.Show("Reference article non valide !", "Alerte", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return sortie;
        }
    }
}
