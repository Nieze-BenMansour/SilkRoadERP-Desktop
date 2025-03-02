using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Models.Mapping
{
    public class ClientMap : EntityTypeConfiguration<Client>
    {
        public ClientMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Properties
            this.Property(t => t.nom)
                .IsRequired()
                .HasMaxLength(50);


            this.Property(t => t.tel)
                .HasMaxLength(50);

            this.Property(t => t.adresse)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Client");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.nom).HasColumnName("nom");
            this.Property(t => t.tel).HasColumnName("tel");
            this.Property(t => t.adresse).HasColumnName("adresse");
            this.Property(t => t.matricule).HasColumnName("matricule");
            this.Property(t => t.code).HasColumnName("code");
            this.Property(t => t.code_cat).HasColumnName("code_cat");
            this.Property(t => t.etb_sec).HasColumnName("etb_sec");
            this.Property(t => t.mail).HasColumnName("mail");
        }
    }
}
