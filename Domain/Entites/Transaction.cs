using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Transaction
    {
        public int Num_BL { get; set; }
        public int type { get; set; }
        public System.DateTime date_tr { get; set; }
        public decimal montant { get; set; }
        public virtual BonDeLivraison BonDeLivraison { get; set; }
    }
}
