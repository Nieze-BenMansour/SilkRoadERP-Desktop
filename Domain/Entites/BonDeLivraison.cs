using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class BonDeLivraison
    {
        public BonDeLivraison()
        {
            this.LigneBLs = new List<LigneBL>();
        }

        public int Num { get; set; }
       
        public System.DateTime date { get; set; }
        public decimal tot_H_tva { get; set; }
        public decimal tot_tva { get; set; }
        public decimal net_payer { get; set; }
        public TimeSpan temp_bl { get; set; }
        public Nullable<int> Num_Facture { get; set; }
        public virtual Facture Facture { get; set; }
        public virtual Client Client { get; set; }
        public Nullable<int> clientId { get; set; }
        public virtual ICollection<LigneBL> LigneBLs { get; set; }
        public virtual Transaction Transaction { get; set; }
        

    }
}
