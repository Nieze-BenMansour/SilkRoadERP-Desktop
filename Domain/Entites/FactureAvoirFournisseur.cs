using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
     public class FactureAvoirFournisseur
    {
        public FactureAvoirFournisseur()
        {
            this.AvoirFournisseurs = new List<AvoirFournisseur>();
        }

        public int Num { get; set; }
        public int Num_FactureAvoirFourSurPAge { get; set; }

        public int id_fournisseur { get; set; }
        public System.DateTime date { get; set; }
        public decimal tot_H_tva { get; set; }
        public decimal tot_tva { get; set; }
        public decimal tot_ttc { get; set; }
        
        public virtual ICollection<AvoirFournisseur> AvoirFournisseurs { get; set; }
        public virtual Fournisseur Fournisseur { get; set; }
        public Nullable<int> Num_FactureFournisseur { get; set; }
        public virtual FactureFournisseur FactureFournisseur { get; set; }

    }
}
