using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Models.Mapping
{
    public class BonDeReceptionMap : EntityTypeConfiguration<BonDeReception>
    {
        public BonDeReceptionMap()
        {
            // Primary Key
            this.HasKey(t => t.Num);

            // Properties
            // Table & Column Mappings
            this.ToTable("BonDeReception");
            this.Property(t => t.Num).HasColumnName("Num");
            this.Property(t => t.id_fournisseur).HasColumnName("id_fournisseur");
            this.Property(t => t.date).HasColumnName("date");
           
            this.Property(t => t.Num_Facture_fournisseur).HasColumnName("Num_Facture_fournisseur");
            this.Property(t => t.num_BonFournisseur).HasColumnName("Num_Bon_fournisseur").IsRequired();
            this.Property(t => t.dateDeLivraison).HasColumnName("date_livraison").IsRequired();

            // Relationships
            this.HasOptional(t => t.FactureFournisseur)
                .WithMany(t => t.BonDeReceptions)
                .HasForeignKey(d => d.Num_Facture_fournisseur).WillCascadeOnDelete(false);
            this.HasRequired(t => t.Fournisseur)
                .WithMany(t => t.BonDeReceptions)
                .HasForeignKey(d => d.id_fournisseur).WillCascadeOnDelete(false);

            //Not mapped
            this.Ignore(t => t.tot_tva);
            this.Ignore(t => t.tot_H_tva);
            this.Ignore(t => t.net_payer);

        }
    }
}
