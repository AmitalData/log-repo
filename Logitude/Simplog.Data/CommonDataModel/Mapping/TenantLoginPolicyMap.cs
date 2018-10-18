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


    public class TenantLoginPolicyMap : EntityTypeConfiguration<TenantLoginPolicy>
    {
        public TenantLoginPolicyMap()
        {
            // Primary Key
            this.HasKey(t => t.Tenant);

            this.Property(t => t.Tenant).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Properties
            this.Property(t => t.LoginPolicyCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            this.Property(t => t.TwoFactorInternalIPs)
             .HasMaxLength(200)
             .IsUnicode(false);

            this.Property(t => t.AllowedIPs)
          .HasMaxLength(200)
          .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("TenantLoginPolicies");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.LoginPolicyCode).HasColumnName("LoginPolicyCode");
            this.Property(t => t.IsEnabledForSpecificUsers).HasColumnName("IsEnabledForSpecificUsers");
            this.Property(t => t.TwoFactorInternalIPs).HasColumnName("TwoFactorInternalIPs");
            this.Property(t => t.KeepUserLoggedIn).HasColumnName("KeepUserLoggedIn");
            this.Property(t => t.ExcludeInternalIPs).HasColumnName("ExcludeInternalIPs");
            this.Property(t => t.AllowedIPs).HasColumnName("AllowedIPs");
            this.Property(t => t.SessionTimeout).HasColumnName("SessionTimeout");
            this.HasRequired(t => t.LoginPolicy).WithMany().HasForeignKey(d => d.LoginPolicyCode);
        }



       

    }
}

