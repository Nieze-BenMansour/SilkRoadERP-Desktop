using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Models.Mapping
{
    public class BonDeLivraisonMap : EntityTypeConfiguration<BonDeLivraison>
    {
        public BonDeLivraisonMap()
        {
            // Primary Key
            this.HasKey(t => t.Num);

            // Properties
            // Table & Column Mappings
            this.ToTable("BonDeLivraison");
            this.Property(t => t.Num).HasColumnName("Num");
            this.Property(t => t.date).HasColumnName("date");
            this.Property(t => t.tot_H_tva).HasColumnName("tot_H_tva");
            this.Property(t => t.tot_tva).HasColumnName("tot_tva");
            this.Property(t => t.net_payer).HasColumnName("net_payer");
            this.Property(t => t.Num_Facture).HasColumnName("Num_Facture");

            // Relationships
            this.HasOptional(t => t.Facture)
                .WithMany(t => t.BonDeLivraisons)
                .HasForeignKey(d => d.Num_Facture).WillCascadeOnDelete(false);

            this.HasOptional(t => t.Client)
                .WithMany(t => t.BonDeLivraisons)
                .HasForeignKey(d => d.clientId).WillCascadeOnDelete(false);

        }
    }
}
