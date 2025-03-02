using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Models.Mapping
{
    public class FactureFournisseurMap : EntityTypeConfiguration<FactureFournisseur>
    {
        public FactureFournisseurMap()
        {
            // Primary Key
            this.HasKey(t => t.Num);

            // Properties
            // Table & Column Mappings
            this.ToTable("FactureFournisseur");
            this.Property(t => t.Num).HasColumnName("Num");
            this.Property(t => t.id_fournisseur).HasColumnName("id_fournisseur");
            this.Property(t => t.date).HasColumnName("date");

            // Relationships
            this.HasRequired(t => t.Fournisseur)
                .WithMany(t => t.FactureFournisseurs)
                .HasForeignKey(d => d.id_fournisseur).WillCascadeOnDelete(false);

            //Not mapped
            this.Ignore(t => t.tot_tva);
            this.Ignore(t => t.tot_H_tva);
            this.Ignore(t => t.tot_ttc);

            this
          .HasRequired(s => s.AvoirFinancierFournisseur) // Mark Address property optional in Student entity
          .WithRequiredPrincipal(ad => ad.FactureFournisseur);

        }
    }
}
