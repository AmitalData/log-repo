using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class GeneralLockMap
        : EntityTypeConfiguration<GeneralLock>
    {
        public GeneralLockMap()
        {
            
            // Primary Key
            this.HasKey(t => new { t.GeneralKey, t.Tenant });

            // Properties
            this.Property(t => t.GeneralKey)
                .IsRequired()
                .HasMaxLength(128)
                .IsUnicode(false);

            this.Property(t => t.Tenant)
                .IsRequired();

          

            // Table & Column Mappings
            this.ToTable("GeneralLocks");
            this.Property(t => t.GeneralKey).HasColumnName("GeneralKey");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
           
        
        }
    }
}
