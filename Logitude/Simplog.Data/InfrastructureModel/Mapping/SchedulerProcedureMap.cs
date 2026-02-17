using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class SchedulerProcedureMap : EntityTypeConfiguration<SchedulerProcedure>
    {
        public SchedulerProcedureMap()
        {
            this.HasKey(t => t.Code);
            

            this.Property(t => t.Code)
                 .IsRequired()
                 .HasMaxLength(100)
                 .IsUnicode(false);

            this.Property(t => t.Name)
                 .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.Description)
                .IsOptional()
                .HasMaxLength(1000)
                .IsUnicode(false);
            this.Property(t => t.SearchFields)
               .IsOptional()
               .HasMaxLength(1000)
               .IsUnicode(false);
            this.Property(t => t.IsInternallyDefined)
             .IsOptional();
             
             


            // Table & Column Mappings
            this.ToTable("SchedulerProcedure");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.IsInternallyDefined).HasColumnName("IsInternallyDefined");

        }
    }
}
