using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Devi
    {
        public Devi()
        {
            this.LigneDevis = new List<LigneDevi>();
        }

        public int Num { get; set; }
        public int id_client { get; set; }
        public System.DateTime date { get; set; }
        public decimal tot_H_tva { get; set; }
        public decimal tot_tva { get; set; }
        public decimal tot_ttc { get; set; }
        public virtual Client Client { get; set; }
        public virtual ICollection<LigneDevi> LigneDevis { get; set; }
    }
}
