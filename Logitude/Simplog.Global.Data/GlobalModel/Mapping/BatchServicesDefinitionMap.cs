using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Mapping
{
    public class BatchServicesDefinitionMap : EntityTypeConfiguration<BatchServicesDefinition>
    {

        public BatchServicesDefinitionMap()
        {
            this.HasKey(t => t.Code);
            this.Property(t => t.Code).IsRequired().HasMaxLength(40).IsUnicode(false);
            //this.Property(t => t.InActive);
            //this.Property(t => t.NumberOfThreads);
            this.Property(t => t.ClassName).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.Parameter1).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.Parameter2).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.QueueDefinitionCode).HasMaxLength(200).IsUnicode(false);


            this.ToTable("BatchServicesDefinitions");
            this.Property(t => t.Code).HasColumnName("Code");
            //this.Property(t => t.InActive).HasColumnName("InActive");
            //this.Property(t => t.NumberOfThreads).HasColumnName("NumberOfThreads");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.Parameter1).HasColumnName("Parameter1");
            this.Property(t => t.Parameter2).HasColumnName("Parameter2");
            this.Property(t => t.QueueDefinitionCode).HasColumnName("QueueDefinitionCode");

            this.HasRequired(t => t.BatchServicesDefinitionMods).WithRequiredPrincipal(d => d.BatchServicesDefinition);
            


        }

    }
}
