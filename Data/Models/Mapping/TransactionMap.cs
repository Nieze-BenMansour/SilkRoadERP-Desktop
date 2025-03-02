using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Models.Mapping
{
    public class TransactionMap : EntityTypeConfiguration<Transaction>
    {
        public TransactionMap()
        {
            // Primary Key
            this.HasKey(t => t.Num_BL);

            // Properties
            this.Property(t => t.Num_BL)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.type)
                .IsRequired()
                ;

            // Table & Column Mappings
            this.ToTable("Transaction");
            this.Property(t => t.Num_BL).HasColumnName("Num_BL");
            this.Property(t => t.type).HasColumnName("type");
            this.Property(t => t.date_tr).HasColumnName("date_tr");
            this.Property(t => t.montant).HasColumnName("montant");

            // Relationships
            this.HasRequired(t => t.BonDeLivraison)
                .WithOptional(t => t.Transaction);

        }
    }
}
