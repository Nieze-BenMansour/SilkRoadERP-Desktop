using Domain.Entites;
using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Fournisseur
    {
        public Fournisseur()
        {
            this.BonDeReceptions = new List<BonDeReception>();
            this.FactureFournisseurs = new List<FactureFournisseur>();
            this.EcheanceFournisseurs = new List<EcheanceDesFournisseur>();
            this.Commandes = new List<Commande>();
            this.AvoirFournisseurs = new List<AvoirFournisseur>();
            this.FactureAvoirFournisseurs = new List<FactureAvoirFournisseur>();
          //  this.AvoirFinancierFournisseurs = new List<AvoirFinancierFournisseur>();

        }

        public int id { get; set; }
        public string nom { get; set; }
        public string tel { get; set; }
        public string fax { get; set; }
        public string matricule { get; set; }
        public string code { get; set; }
        public string code_cat { get; set; }
        public string etb_sec { get; set; }

        public string mail { get; set; }
        public string mail_deux { get; set; }
        public bool constructeur { get; set; }
        public string  adresse { get; set; }
        public virtual ICollection<BonDeReception> BonDeReceptions { get; set; }
        public virtual ICollection<FactureFournisseur> FactureFournisseurs { get; set; }
        public virtual ICollection<EcheanceDesFournisseur> EcheanceFournisseurs { get; set; }
        public virtual ICollection<Commande> Commandes { get; set; }
        public virtual ICollection<AvoirFournisseur> AvoirFournisseurs { get; set; }
        public virtual ICollection<FactureAvoirFournisseur> FactureAvoirFournisseurs { get; set; }

       // public virtual ICollection<AvoirFinancierFournisseur> AvoirFinancierFournisseurs { get; set; }
    }
}
