using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Mapping
{
    class FactureAvoirFournisseurMap : EntityTypeConfiguration<FactureAvoirFournisseur>
    {
        public FactureAvoirFournisseurMap()
        {
            // Primary Key
            this.HasKey(t => t.Num);

            // Properties
            // Table & Column Mappings
            this.ToTable("FactureAvoirFournisseur");
            this.Property(t => t.Num).HasColumnName("Num");
            this.Property(t => t.id_fournisseur).HasColumnName("id_fournisseur");
            this.Property(t => t.date).HasColumnName("date");
            this.Property(t => t.Num_FactureFournisseur).HasColumnName("Num_FactureFournisseur");
            // Relationships
            this.HasRequired(t => t.Fournisseur)
                .WithMany(t => t.FactureAvoirFournisseurs)
                .HasForeignKey(d => d.id_fournisseur).WillCascadeOnDelete(false);

            this.HasRequired(t => t.FactureFournisseur)
                .WithMany(t => t.FactureAvoirFournisseurs)
                .HasForeignKey(d => d.Num_FactureFournisseur).WillCascadeOnDelete(false);

            //Not mapped
            this.Ignore(t => t.tot_tva);
            this.Ignore(t => t.tot_H_tva);
            this.Ignore(t => t.tot_ttc);

            this.Property(t => t.Num_FactureFournisseur).IsOptional();

        }

    }
}
