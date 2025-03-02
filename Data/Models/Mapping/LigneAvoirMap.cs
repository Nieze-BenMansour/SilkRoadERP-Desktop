using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Mapping
{
    public class LigneAvoirMap : EntityTypeConfiguration<LigneAvoir>
    {
        public LigneAvoirMap()
        {
            this.HasKey(t => t.Id_li);

            // Relationships
            this.HasRequired(t => t.Avoir)
                .WithMany(t => t.LigneAvoirs)
                .HasForeignKey(d => d.Num_avoir)
                .WillCascadeOnDelete(false);
            this.HasRequired(t => t.Produit)
                .WithMany(t => t.lignesAvoirs)
                .HasForeignKey(d => d.Ref_Produit)
                .WillCascadeOnDelete(false);
        }
    }
}
