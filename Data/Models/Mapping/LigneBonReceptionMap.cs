using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Models.Mapping
{
    public class LigneBonReceptionMap : EntityTypeConfiguration<LigneBonReception>
    {
        public LigneBonReceptionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id_ligne);

            // Properties
            this.Property(t => t.Ref_Produit)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.designation_li)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("LigneBonReception");
            this.Property(t => t.Id_ligne).HasColumnName("Id_ligne");
            this.Property(t => t.Num_BonRec).HasColumnName("Num_BonRec");
            this.Property(t => t.Ref_Produit).HasColumnName("Ref_Produit");
            this.Property(t => t.designation_li).HasColumnName("designation_li");
            this.Property(t => t.qte_li).HasColumnName("qte_li");
            this.Property(t => t.prix_HT).HasColumnName("prix_HT");
            this.Property(t => t.remise).HasColumnName("remise");
            this.Property(t => t.tot_HT).HasColumnName("tot_HT");
            this.Property(t => t.tva).HasColumnName("tva");
            this.Property(t => t.tot_TTC).HasColumnName("tot_TTC");

            // Relationships
         this.HasRequired(t => t.BonDeReception)
               .WithMany(t => t.LigneBonReceptions)
               .HasForeignKey(d => d.Num_BonRec).WillCascadeOnDelete(false);
            this.HasRequired(t => t.Produit)
                .WithMany(t => t.LigneBonReceptions)
                .HasForeignKey(d => d.Ref_Produit).WillCascadeOnDelete(false);

            // Not Mapped
            this.Ignore(t => t.NetTtcUnitaire);
            this.Ignore(t => t.prixHtFodec);

            this.Property(t => t.Num_BonRec).IsOptional();

        }
    }
}
