using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class LigneBL
    {
        public int Id_li { get; set; }
        public int Num_BL { get; set; }
        public string Ref_Produit { get; set; }
        public string designation_li { get; set; }
        public int qte_li { get; set; }
        public decimal prix_HT { get; set; }
        public double remise { get; set; }
        public decimal tot_HT { get; set; }
        public double tva { get; set; }
        public decimal tot_TTC { get; set; }
       
        public virtual BonDeLivraison BonDeLivraison { get; set; }
        public virtual Produit Produit { get; set; }
    }
}
