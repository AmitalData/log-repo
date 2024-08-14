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
    public class DWHSettingMap : EntityTypeConfiguration<DWHSetting>
    {

        public DWHSettingMap()
        {
            // Primary Key
            this.HasKey(t => t.Tenant);
            var dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.Tenant).HasDatabaseGeneratedOption(null);
            }
            else
            {
                this.Property(t => t.Tenant).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            }
                

            this.Property(t => t.Server)
                .HasMaxLength(200)
                .IsUnicode(false);

            this.Property(t => t.Password)
                .HasMaxLength(200)
                .IsUnicode(false);

            this.Property(t => t.UserName)
                .HasMaxLength(200)
                .IsUnicode(false);

            this.Property(t => t.Catalog)
                .HasMaxLength(200)
                .IsUnicode(false);

            this.Property(t => t.PrivateUserName)
                .HasMaxLength(200)
               .IsUnicode(false);

            this.ToTable("DWHSettings");

            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ParentTenant).HasColumnName("ParentTenant");
            this.Property(t => t.Server).HasColumnName("Server");
            this.Property(t => t.UserName).HasColumnName("UserName");
            
            if (dbms == "oracle")
            {
                this.Property(t => t.Password).HasColumnName("Password_");
            }
            else
            {
                this.Property(t => t.Password).HasColumnName("Password");
            }
            
            this.Property(t => t.Catalog).HasColumnName("Catalog");
            this.Property(t => t.IsParentTenant).HasColumnName("IsParentTenant");
            this.Property(t => t.PrivateUserName).HasColumnName("PrivateUserName");


        }
    }
}
