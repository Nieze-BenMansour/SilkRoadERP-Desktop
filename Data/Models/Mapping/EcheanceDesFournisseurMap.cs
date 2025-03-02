using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Mapping
{
    public class EcheanceDesFournisseurMap : EntityTypeConfiguration<EcheanceDesFournisseur>
    {
        
        public EcheanceDesFournisseurMap() {

            this.HasKey(t => t.id);

         this.HasRequired(t => t.fournisseur)
                .WithMany(t => t.EcheanceFournisseurs)
                .HasForeignKey(d => d.fournisseur_id).WillCascadeOnDelete(false);
        }
    }
}
