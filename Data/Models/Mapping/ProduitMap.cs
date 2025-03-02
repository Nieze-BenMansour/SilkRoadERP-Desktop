using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Models.Mapping
{
    public class ProduitMap : EntityTypeConfiguration<Produit>
    {
        public ProduitMap()
        {
            // Primary Key
            this.HasKey(t => t.refe);

            // Properties
            this.Property(t => t.refe)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.nom)
                .IsRequired();

            this.Property(t => t.remise)
                .IsRequired();

            this.Property(t => t.TVA)
                .IsRequired();
            this.Property(t => t.remiseAchat)
                .IsRequired();
            this.Property(t => t.prix)
                .IsRequired();
            this.Property(t => t.prixAchat)
                .IsRequired();

            this.Property(t => t.qte)
                .IsRequired();
            this.Property(t => t.qteLimite)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Produit");
            this.Property(t => t.refe).HasColumnName("refe");
            this.Property(t => t.nom).HasColumnName("nom");
            this.Property(t => t.qte).HasColumnName("qte");
            this.Property(t => t.qteLimite).HasColumnName("qteLimite");
            this.Property(t => t.remise).HasColumnName("remise");
            this.Property(t => t.remiseAchat).HasColumnName("remiseAchat");
            this.Property(t => t.TVA).HasColumnName("TVA");
            this.Property(t => t.prix).HasColumnName("prix");
            this.Property(t => t.prixAchat).HasColumnName("prixAchat");
        }
    }
}
