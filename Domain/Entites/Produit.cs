using Domain.Entites;
using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Produit
    {
        public Produit()
        {
            this.LigneBLs = new List<LigneBL>();
            this.LigneBonReceptions = new List<LigneBonReception>();
            this.LigneDevis = new List<LigneDevi>();
            this.lignesAvoirs = new List<LigneAvoir>();
            this.lignesCommandes = new List<LigneCommande>();
            this.lignesAvoirsFournisseur = new List<LigneAvoirFourniseur>();
        }

        public string refe { get; set; }
        public string nom { get; set; }
        public int qte { get; set; }
        public int qteLimite { get; set; }
        public double remise { get; set; }
        public double remiseAchat { get; set; }
        public Nullable<double> TVA { get; set; }
        public Nullable<decimal> prix { get; set; }
        public Nullable<decimal> prixAchat { get; set; }
        public bool visibilite { get; set; }
        public virtual ICollection<LigneBL> LigneBLs { get; set; }
        public virtual ICollection<LigneBonReception> LigneBonReceptions { get; set; }
        public virtual ICollection<LigneDevi> LigneDevis { get; set; }
        public virtual ICollection<LigneAvoir> lignesAvoirs { get; set; }
        public virtual ICollection<LigneCommande> lignesCommandes { get; set; }
        public virtual ICollection<LigneAvoirFourniseur> lignesAvoirsFournisseur { get; set; }


    }
}
