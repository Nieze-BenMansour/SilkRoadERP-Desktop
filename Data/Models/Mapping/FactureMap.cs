using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Models.Mapping
{
    public class FactureMap : EntityTypeConfiguration<Facture>
    {
        public FactureMap()
        {
            // Primary Key
            this.HasKey(t => t.Num);

            //Not mapped
            this.Ignore(t => t.tot_tva);
            this.Ignore(t => t.tot_H_tva);
            this.Ignore(t => t.tot_ttc);

            // Properties
            // Table & Column Mappings
            this.ToTable("Facture");
            this.Property(t => t.Num).HasColumnName("Num");
            this.Property(t => t.id_client).HasColumnName("id_client");
            this.Property(t => t.date).HasColumnName("date");

            // Relationships
            this.HasRequired(t => t.Client)
                .WithMany(t => t.Factures)
                .HasForeignKey(d => d.id_client).WillCascadeOnDelete(false);

        }
    }
}
