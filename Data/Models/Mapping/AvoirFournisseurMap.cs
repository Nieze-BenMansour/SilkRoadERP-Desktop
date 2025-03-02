using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Mapping
{
    public class AvoirFournisseurMap : EntityTypeConfiguration<AvoirFournisseur>
    {
        public AvoirFournisseurMap()
        {
            // Primary Key
            this.HasKey(t => t.Num);

            // Properties
            // Table & Column Mappings
            this.ToTable("AvoirFournisseur");
            this.Property(t => t.Num).HasColumnName("Num");
            this.Property(t => t.date).HasColumnName("date");
          //  this.Property(t => t.tot_H_tva).HasColumnName("tot_H_tva");
         //   this.Property(t => t.tot_tva).HasColumnName("tot_tva");
           // this.Property(t => t.net_payer).HasColumnName("net_payer");
            this.Property(t => t.Num_FactureAvoirFournisseur).HasColumnName("Num_FactureAvoirFournisseur");

            // Relationships
            this.HasOptional(t => t.FactureAvoirFournisseur)
                .WithMany(t => t.AvoirFournisseurs)
                .HasForeignKey(d => d.Num_FactureAvoirFournisseur).WillCascadeOnDelete(false);

            this.HasOptional(t => t.Fournisseur)
                .WithMany(t => t.AvoirFournisseurs)
                .HasForeignKey(d => d.fournisseurId).WillCascadeOnDelete(false);


            //Not mapped
            this.Ignore(t => t.tot_tva);
            this.Ignore(t => t.tot_H_tva);
            this.Ignore(t => t.net_payer);

        }
    }
}
