using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Models.Mapping
{
    public class FournisseurMap : EntityTypeConfiguration<Fournisseur>
    {
        public FournisseurMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.nom)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.tel)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.fax)
                .HasMaxLength(50);

            this.Property(t => t.matricule)
                .HasMaxLength(50);

            this.Property(t => t.code)
                .HasMaxLength(50);

            this.Property(t => t.code_cat)
                .HasMaxLength(50);

            this.Property(t => t.etb_sec)
                .HasMaxLength(50);
            this.Property(t => t.constructeur)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Fournisseur");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.nom).HasColumnName("nom");
            this.Property(t => t.tel).HasColumnName("tel");
            this.Property(t => t.fax).HasColumnName("fax");
            this.Property(t => t.matricule).HasColumnName("matricule");
            this.Property(t => t.code).HasColumnName("code");
            this.Property(t => t.code_cat).HasColumnName("code_cat");
            this.Property(t => t.etb_sec).HasColumnName("etb_sec");


            
        }
    }
}
