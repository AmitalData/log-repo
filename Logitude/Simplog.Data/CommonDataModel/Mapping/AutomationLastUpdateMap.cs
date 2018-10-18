using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
   public class AutomationLastUpdateMap : EntityTypeConfiguration<AutomationLastUpdate>
    {
        public AutomationLastUpdateMap()
        {
                 // Primary Key
              this.HasKey(t => new { t.Tenant, t.ObjectTableId });

            // Properties
            this.Property(t => t.Tenant)
                .IsRequired();

            this.Property(t => t.LastUpdateDate)
               .IsRequired();

            this.Property(t => t.ObjectTableId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.HasAutomation);


            // Table & Column Mappings
            this.ToTable("AutomationLastUpdates");
  
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.LastUpdateDate).HasColumnName("LastUpdateDate");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.HasAutomation).HasColumnName("HasAutomation");
      

        }
    }
}
