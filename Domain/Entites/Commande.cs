using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class Commande
    {
        public Commande()
        {
            this.LigneCommandes = new List<LigneCommande>();
        }

        public int Num { get; set; }
        public System.DateTime date { get; set; }
        public decimal tot_H_tva { get; set; }
        public decimal tot_tva { get; set; }
        public decimal net_payer { get; set; }
        public virtual Fournisseur fournisseur { get; set; }
        public Nullable< int> fournisseurId { get; set; }
        public virtual ICollection<LigneCommande> LigneCommandes { get; set; }
    }
}
