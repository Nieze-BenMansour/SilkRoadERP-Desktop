﻿using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Mapping
{
    public class AvoirMap : EntityTypeConfiguration<Avoir>
    {
        public AvoirMap()
        {
            this.HasKey(t => t.Num);
            // Relationships

            this.HasOptional(t => t.Client)
                .WithMany(t => t.Avoirs)
                .HasForeignKey(d => d.clientId).WillCascadeOnDelete(false);

            //Not mapped
            this.Ignore(t => t.tot_tva);
            this.Ignore(t => t.tot_H_tva);
            this.Ignore(t => t.net_payer);

        }
    }
}
