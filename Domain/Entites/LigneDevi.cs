using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class LigneDevi
    {
        public int Id_li { get; set; }
        public int Num_devis { get; set; }
        public string Ref_produit { get; set; }
        public string Designation_li { get; set; }
        public int qte_li { get; set; }
        public decimal prix_HT { get; set; }
        public double remise { get; set; }
        public decimal tot_HT { get; set; }
        public double tva { get; set; }
        public decimal tot_TTC { get; set; }
        public virtual Devi Devi { get; set; }
        public virtual Produit Produit { get; set; }
    }
}
