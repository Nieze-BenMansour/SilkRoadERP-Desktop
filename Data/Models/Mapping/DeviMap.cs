using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Models.Mapping
{
    public class DeviMap : EntityTypeConfiguration<Devi>
    {
        public DeviMap()
        {
            // Primary Key
            this.HasKey(t => t.Num);

            // Properties
            // Table & Column Mappings
            this.ToTable("Devis");
            this.Property(t => t.Num).HasColumnName("Num");
            this.Property(t => t.id_client).HasColumnName("id_client");
            this.Property(t => t.date).HasColumnName("date");
            this.Property(t => t.tot_H_tva).HasColumnName("tot_H_tva");
            this.Property(t => t.tot_tva).HasColumnName("tot_tva");
            this.Property(t => t.tot_ttc).HasColumnName("tot_ttc");

            // Relationships
            this.HasRequired(t => t.Client)
                .WithMany(t => t.Devis)
                .HasForeignKey(d => d.id_client);

        }
    }
}
