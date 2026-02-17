
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


    public class TwoFactorAuthenticationDeviceMap : EntityTypeConfiguration<TwoFactorAuthenticationDevice>
    {
        public TwoFactorAuthenticationDeviceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                 .IsRequired()
                 .HasMaxLength(15)
                 .IsUnicode(false);

            // Properties
            this.Property(t => t.TwoFactorkey)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.UserId)
             .IsRequired()
             .HasMaxLength(15)
             .IsUnicode(false);

              this.Property(t => t.DeviceDescription)
             .IsRequired()
             .HasMaxLength(200)
             .IsUnicode(true);

              this.Property(t => t.LastLoginIP)
             //.IsRequired()
             .HasMaxLength(100)
             .IsUnicode(false);

            this.Property(t => t.AuthenticationCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("TwoFactorAuthenticationDevices");
            this.Property(t => t.TwoFactorkey).HasColumnName("TwoFactorkey");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.LastLoginDate).HasColumnName("LastLoginDate");

            this.Property(t => t.DeviceDescription).HasColumnName("DeviceDescription");
            this.Property(t => t.LastLoginIP).HasColumnName("LastLoginIP");
            this.Property(t => t.InActive).HasColumnName("InActive");

            this.Property(t => t.AuthenticationCode).HasColumnName("AuthenticationCode");
            this.Property(t => t.CodeExpirationDate).HasColumnName("CodeExpirationDate");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.IsVerified).HasColumnName("IsVerified");
            

            this.HasRequired(t => t.User).WithMany().HasForeignKey(d => d.UserId);
        }





    }
}


