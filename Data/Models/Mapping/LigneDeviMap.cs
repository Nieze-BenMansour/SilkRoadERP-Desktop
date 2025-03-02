using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Models.Mapping
{
    public class LigneDeviMap : EntityTypeConfiguration<LigneDevi>
    {
        public LigneDeviMap()
        {
            // Primary Key
            this.HasKey(t => t.Id_li);

            // Properties
            this.Property(t => t.Ref_produit)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Designation_li)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("LigneDevis");
            this.Property(t => t.Id_li).HasColumnName("Id_li");
            this.Property(t => t.Num_devis).HasColumnName("Num_devis");
            this.Property(t => t.Ref_produit).HasColumnName("Ref_produit");
            this.Property(t => t.Designation_li).HasColumnName("Designation_li");
            this.Property(t => t.qte_li).HasColumnName("qte_li");
            this.Property(t => t.prix_HT).HasColumnName("prix_HT");
            this.Property(t => t.remise).HasColumnName("remise");
            this.Property(t => t.tot_HT).HasColumnName("tot_HT");
            this.Property(t => t.tva).HasColumnName("tva");
            this.Property(t => t.tot_TTC).HasColumnName("tot_TTC");

            // Relationships
            this.HasRequired(t => t.Devi)
                .WithMany(t => t.LigneDevis)
                .HasForeignKey(d => d.Num_devis).WillCascadeOnDelete(false);
            this.HasRequired(t => t.Produit)
                .WithMany(t => t.LigneDevis)
                .HasForeignKey(d => d.Ref_produit).WillCascadeOnDelete(false);

        }
    }
}
