using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Mapping
{
    class AvoirFinancierFournisseurMap : EntityTypeConfiguration<AvoirFinancierFournisseur>
    {
        public AvoirFinancierFournisseurMap()
        {
                 this.HasKey(t => t.Num );



            // Relationships


            /*   this.HasOptional(t => t.Fournisseur)
                   .WithMany(t => t.AvoirFinancierFournisseurs)
                   .HasForeignKey(d => d.id_fournisseur).WillCascadeOnDelete(false);*/

   

        }
    }
}
