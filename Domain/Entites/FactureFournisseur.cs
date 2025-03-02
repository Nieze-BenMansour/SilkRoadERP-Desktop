using Domain.Entites;
using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class FactureFournisseur
    {
        public FactureFournisseur()
        {
            this.BonDeReceptions = new List<BonDeReception>();
            this.FactureAvoirFournisseurs = new List<FactureAvoirFournisseur>();
        }

        public int Num { get; set; }
        public int id_fournisseur { get; set; }
        public bool paye { get; set; }
        public long NumFactureFournisseur { get; set; }
        public System.DateTime dateFacturationFournisseur { get; set; }
        public System.DateTime date { get; set; }
        public decimal tot_H_tva { get; set; }
        public decimal tot_tva { get; set; }
        public decimal tot_ttc { get; set; }
        public virtual ICollection<BonDeReception> BonDeReceptions { get; set; }
        public virtual ICollection<FactureAvoirFournisseur>  FactureAvoirFournisseurs  { get; set; }

        public virtual AvoirFinancierFournisseur AvoirFinancierFournisseur { get; set; }
        public virtual Fournisseur Fournisseur { get; set; }
    }
}
