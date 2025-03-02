using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class EcheanceDesFournisseur
    {
        public int id { get; set; }
        public DateTime dateEcheance { get; set; }
        public long numCheque { get; set; }
        public decimal montant { get; set; }
        public int fournisseur_id { get; set; }

       
        public virtual Fournisseur fournisseur { get; set; }
    }
}
