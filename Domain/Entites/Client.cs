using Domain.Entites;
using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Client
    {
        public Client()
        {
            this.BonDeLivraisons = new List<BonDeLivraison>();
            this.Devis = new List<Devi>();
            this.Factures = new List<Facture>();
            this.Avoirs = new List<Avoir>();
        }

        public int Id { get; set; }
        public string nom { get; set; }
        public string tel { get; set; }
        public string adresse { get; set; }
        public string matricule { get; set; }
        public string code { get; set; }
        public string code_cat { get; set; }
        public string etb_sec { get; set; }
        public string mail { get; set; }
        public virtual ICollection<BonDeLivraison> BonDeLivraisons { get; set; }
        public virtual ICollection<Devi> Devis { get; set; }
        public virtual ICollection<Facture> Factures { get; set; }
        public ICollection<Avoir> Avoirs { get; set; }
    }
}
