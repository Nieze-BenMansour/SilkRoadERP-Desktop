using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class AvoirFournisseur
    {
        public AvoirFournisseur()
        {
            this.LigneAvoirsFrs = new List<LigneAvoirFourniseur>();
        }

        public int Num { get; set; }
        public System.DateTime date { get; set; }
        public decimal tot_H_tva { get; set; }
        public decimal tot_tva { get; set; }
        public decimal net_payer { get; set; }
        public virtual Fournisseur Fournisseur { get; set; }
        public Nullable<int> fournisseurId { get; set; }
        public Nullable<int> Num_FactureAvoirFournisseur { get; set; }
        public virtual FactureAvoirFournisseur FactureAvoirFournisseur { get; set; }
        public virtual ICollection<LigneAvoirFourniseur> LigneAvoirsFrs { get; set; }
        public int Num_AvoirFournisseur { get; set; }
    }
}
