using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Mapping
{
    public class LigneAvoirFournisseurMap : EntityTypeConfiguration<LigneAvoirFourniseur>
    {
        public LigneAvoirFournisseurMap()
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
            this.ToTable("LigneAvoirFournisseur");
            this.Property(t => t.Id_li).HasColumnName("Id_li");
            this.Property(t => t.Num_avoirFr).HasColumnName("Num_AvoirFr");
            this.Property(t => t.Ref_Produit).HasColumnName("Ref_Produit");
            this.Property(t => t.designation_li).HasColumnName("designation_li");
            this.Property(t => t.qte_li).HasColumnName("qte_li");
            this.Property(t => t.prix_HT).HasColumnName("prix_HT");
            this.Property(t => t.remise).HasColumnName("remise");
            this.Property(t => t.tot_HT).HasColumnName("tot_HT");
            this.Property(t => t.tva).HasColumnName("tva");
            this.Property(t => t.tot_TTC).HasColumnName("tot_TTC");


            // Relationships
            this.HasRequired(t => t.AvoirFr)
                .WithMany(t => t.LigneAvoirsFrs)
                .HasForeignKey(d => d.Num_avoirFr)
                .WillCascadeOnDelete(false);
            this.HasRequired(t => t.Produit)
                .WithMany(t => t.lignesAvoirsFournisseur)
                .HasForeignKey(d => d.Ref_Produit)
                .WillCascadeOnDelete(false);

        }
    }
}
