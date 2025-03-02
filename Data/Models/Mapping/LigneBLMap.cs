using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Models.Mapping
{
    public class LigneBLMap : EntityTypeConfiguration<LigneBL>
    {
        public LigneBLMap()
        {
            // Primary Key
            this.HasKey(t => t.Id_li);

           

            // Properties
            this.Property(t => t.Ref_Produit)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.designation_li)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("LigneBL");
            this.Property(t => t.Id_li).HasColumnName("Id_li");
            this.Property(t => t.Num_BL).HasColumnName("Num_BL");
            this.Property(t => t.Ref_Produit).HasColumnName("Ref_Produit");
            this.Property(t => t.designation_li).HasColumnName("designation_li");
            this.Property(t => t.qte_li).HasColumnName("qte_li");
            this.Property(t => t.prix_HT).HasColumnName("prix_HT");
            this.Property(t => t.remise).HasColumnName("remise");
            this.Property(t => t.tot_HT).HasColumnName("tot_HT");
            this.Property(t => t.tva).HasColumnName("tva");
            this.Property(t => t.tot_TTC).HasColumnName("tot_TTC");
     

            // Relationships
            this.HasRequired(t => t.BonDeLivraison)
                .WithMany(t => t.LigneBLs)
                .HasForeignKey(d => d.Num_BL)
                .WillCascadeOnDelete(false);
            this.HasRequired(t => t.Produit)
                .WithMany(t => t.LigneBLs)
                .HasForeignKey(d => d.Ref_Produit)
                .WillCascadeOnDelete(false);

        }
    }
}
