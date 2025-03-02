using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class AvoirFinancierFournisseur
    {
//public int id { get; set; }
        public int NumSurPage { get; set; }
        public DateTime date { get; set; }
        public string Description { get; set; }
        public decimal tot_ttc { get; set; }       
        public int Num  { get; set; }
        public FactureFournisseur FactureFournisseur { get; set; }
    }
}
