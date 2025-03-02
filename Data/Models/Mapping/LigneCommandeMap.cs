using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Mapping
{
    public class LigneCommandeMap : EntityTypeConfiguration<LigneCommande>
    {
        public LigneCommandeMap()
        {

            this.HasKey(t => t.Id_li);

            // Relationships
            this.HasRequired(t => t.Commande)
                .WithMany(t => t.LigneCommandes)
                .HasForeignKey(d => d.Num_commande)
                .WillCascadeOnDelete(false);
            this.HasRequired(t => t.Produit)
                .WithMany(t => t.lignesCommandes)
                .HasForeignKey(d => d.Ref_Produit)
                .WillCascadeOnDelete(false);
        }
    }
}
