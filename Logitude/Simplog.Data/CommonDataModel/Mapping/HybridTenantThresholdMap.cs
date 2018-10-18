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
    public class HybridTenantThresholdMap : EntityTypeConfiguration<HybridTenantThreshold>
    {


        public HybridTenantThresholdMap()
        {
            // Primary Key
            this.HasKey(t => t.Tenant);
            this.Property(t => t.Tenant).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            // Properties
           

            // Table & Column Mappings
            this.ToTable("HybridTenantThresholds");
             
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.FailedThresold).HasColumnName("FailedThresold");
            this.Property(t => t.FailedThresold).HasColumnName("FailedThresold");
         }
    }
}
