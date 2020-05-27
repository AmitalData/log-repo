using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class DWHBuildStatusMap : EntityTypeConfiguration<DWHBuildStatus>
    {
        
       
        public DWHBuildStatusMap()
        {
            this.Property(t => t.Id).IsRequired().HasMaxLength(40).IsUnicode(false);
     
            // Table & Column Mappings
            this.ToTable("DWHBuildStatus");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DWNextRunTime).HasColumnName("DWNextRunTime");
            this.Property(t => t.LastIncrementalDWUpdateDate).HasColumnName("LastIncrementalDWUpdateDate");
            this.Property(t => t.IsFullBuildDWRunning).HasColumnName("IsFullBuildDWRunning");
            this.Property(t => t.IsIncrementalDWRunning).HasColumnName("IsIncrementalDWRunning");
        }
    }
}
