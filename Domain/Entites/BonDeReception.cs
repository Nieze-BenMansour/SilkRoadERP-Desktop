using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class BonDeReception
    {
        public BonDeReception()
        {
            this.LigneBonReceptions = new List<LigneBonReception>();
        }

        public int Num { get; set; }
        public long num_BonFournisseur { get; set; }
        public System.DateTime dateDeLivraison { get; set; }
        public int id_fournisseur { get; set; }
        public System.DateTime date { get; set; }
        public decimal tot_H_tva { get; set; }
        public decimal tot_tva { get; set; }
        public decimal net_payer { get; set; }
        public Nullable<int> Num_Facture_fournisseur { get; set; }
        public virtual FactureFournisseur FactureFournisseur { get; set; }
        public virtual Fournisseur Fournisseur { get; set; }
        public virtual ICollection<LigneBonReception> LigneBonReceptions { get; set; }
    }
}
