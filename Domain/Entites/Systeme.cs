using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Systeme
    {
        public int Id { get; set; }
        public string NomSociete { get; set; }
        public decimal Timbre { get; set; }
        public string adresse { get; set; }
        public string tel { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string matriculeFiscale { get; set; }
        public string codeTVA { get; set; }
        public string codeCategorie { get; set; }
        public string etbSecondaire { get; set; }
        public decimal pourcentageFodec { get; set; }
        public string adresseRetenu { get; set; }
        public double pourcentageRetenu { get; set; }
    }
}
