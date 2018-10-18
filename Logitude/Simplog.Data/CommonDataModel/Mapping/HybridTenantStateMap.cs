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
    public class HybridTenantStateMap : EntityTypeConfiguration<HybridTenantState>
    {
        public HybridTenantStateMap()
        {
            // Primary Key
            this.HasKey(t => t.Tenant);
            this.Property(t => t.Tenant).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
         

            // Table & Column Mappings
            this.ToTable("HybridTenantStates");
            
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.FailedQueue).HasColumnName("FailedQueue");
            this.Property(t => t.WaitingQueue).HasColumnName("WaitingQueue");
            this.Property(t => t.LastUpdateDateTime).HasColumnName("LastUpdateDateTime");
         }
    }
}
