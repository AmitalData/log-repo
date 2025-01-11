using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class BatchServicesDefinitionModsMap : EntityTypeConfiguration<BatchServicesDefinitionMods>
    {

        public BatchServicesDefinitionModsMap()
        {
            this.HasKey(t => t.Code);
            this.Property(t => t.Code).IsRequired().HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.InActive);
            this.Property(t => t.NumberOfThreads); 
            //this.Property(t => t.Parameter1).HasMaxLength(25).IsUnicode(false);
            //this.Property(t => t.Parameter2).HasMaxLength(25).IsUnicode(false);

            this.ToTable("BatchServicesDefinitionMods");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.NumberOfThreads).HasColumnName("NumberOfThreads"); 
            //this.Property(t => t.Parameter1).HasColumnName("Parameter1");
            //this.Property(t => t.Parameter2).HasColumnName("Parameter2");
            this.HasRequired(t => t.BatchServicesDefinition);

        }

    }
}
