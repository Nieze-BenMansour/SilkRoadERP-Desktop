using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class LigneBonReception
    {
        public int Id_ligne { get; set; }
        public Nullable <int> Num_BonRec { get; set; }
        public string Ref_Produit { get; set; }
        public string designation_li { get; set; }
        public int qte_li { get; set; }
        public decimal prix_HT { get; set; }
        public double remise { get; set; }
        public decimal tot_HT { get; set; }
        public double tva { get; set; }
        public decimal tot_TTC { get; set; }

        public decimal NetTtcUnitaire { get; set; }
        public decimal prixHtFodec { get; set; }
        public virtual BonDeReception BonDeReception { get; set; }
        public virtual Produit Produit { get; set; }
    }
}
