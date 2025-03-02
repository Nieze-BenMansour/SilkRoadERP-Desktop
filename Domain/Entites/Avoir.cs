using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class Avoir
    {
        public Avoir()
        {
            this.LigneAvoirs = new List<LigneAvoir>();
        }

        public int Num { get; set; }
        public System.DateTime date { get; set; }
        public decimal tot_H_tva { get; set; }
        public decimal tot_tva { get; set; }
        public decimal net_payer { get; set; }
        public virtual Client Client { get; set; }
        public Nullable< int> clientId { get; set; }
        public virtual ICollection<LigneAvoir> LigneAvoirs { get; set; }
    }
}
