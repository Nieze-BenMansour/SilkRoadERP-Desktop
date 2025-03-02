using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Models.Mapping
{
    public class SystemeMap : EntityTypeConfiguration<Systeme>
    {
        public SystemeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.NomSociete)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.adresse)
                .IsRequired();

            this.Property(t => t.tel)
                .IsRequired();

            this.Property(t => t.codeTVA)
                .IsRequired();

            


            // Table & Column Mappings
            this.ToTable("Systeme");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.NomSociete).HasColumnName("NomSociete");
            this.Property(t => t.Timbre).HasColumnName("Timbre");
            this.Property(t => t.adresse).HasColumnName("adresse");
            this.Property(t => t.tel).HasColumnName("tel");
            this.Property(t => t.fax).HasColumnName("fax");
            this.Property(t => t.email).HasColumnName("email");
            this.Property(t => t.codeTVA).HasColumnName("codeTVA");
        }
    }
}
