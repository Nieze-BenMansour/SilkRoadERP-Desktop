using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class LigneAvoir
    {
        public int Id_li { get; set; }
        public int Num_avoir { get; set; }
        public string Ref_Produit { get; set; }
        public string designation_li { get; set; }
        public int qte_li { get; set; }
        public decimal prix_HT { get; set; }
        public double remise { get; set; }
        public decimal tot_HT { get; set; }
        public double tva { get; set; }
        public decimal tot_TTC { get; set; }

        public virtual Avoir Avoir { get; set; }
        public virtual Produit Produit { get; set; }
    }
}
