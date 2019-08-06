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
    public class CustomsInterfaceSettingMap : EntityTypeConfiguration<CustomsInterfaceSetting>
    {
        public CustomsInterfaceSettingMap()
        {
            this.HasKey(t => t.Tenant);
            this.Property(t => t.Tenant).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.LocalCustomsInterfaceCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.ImportToUSAInterfaceCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.ExportFromUSAInterfaceCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.LocalCompanyId).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.LocalUserId).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.LocalPassword).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.ArtemusOutSettingsId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ArtemusInSettingsId).HasMaxLength(15).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("CustomsInterfaceSettings");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.LocalCustomsInterfaceCode).HasColumnName("LocalCustomsInterfaceCode");
            this.Property(t => t.LocalCompanyId).HasColumnName("LocalCompanyId");
            this.Property(t => t.LocalUserId).HasColumnName("LocalUserId");
            this.Property(t => t.LocalPassword).HasColumnName("LocalPassword");            
            this.Property(t => t.ImportToUSAInterfaceCode).HasColumnName("ImportToUSAInterfaceCode");
            this.Property(t => t.ExportFromUSAInterfaceCode).HasColumnName("ExportFromUSAInterfaceCode");
            this.Property(t => t.ArtemusOutSettingsId).HasColumnName("ArtemusOutSettingsId");
            this.Property(t => t.ArtemusInSettingsId).HasColumnName("ArtemusInSettingsId");
            this.Property(t => t.AMCAirStartDate).HasColumnName("AMCAirStartDate");
            this.Property(t => t.AMCOceanStartDate).HasColumnName("AMCOceanStartDate");

            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.ActivateCustomsManagementInShipments).HasColumnName("ActivateCustManagInShipments");

            }
            //#elseelse
            else
            {
                this.Property(t => t.ActivateCustomsManagementInShipments).HasColumnName("ActivateCustomsManagementInShipments");
            }

            //Relationships
            this.HasOptional(t => t.LocalCustomsInterface).WithMany().HasForeignKey(d => d.LocalCustomsInterfaceCode);
            this.HasOptional(t => t.ImportToUSAInterface).WithMany().HasForeignKey(d => d.ImportToUSAInterfaceCode);
            this.HasOptional(t => t.ExportFromUSAInterface).WithMany().HasForeignKey(d => d.ExportFromUSAInterfaceCode);
            this.HasOptional(t => t.ArtemusOutSettings).WithMany().HasForeignKey(d => d.ArtemusOutSettingsId);
            this.HasOptional(t => t.ArtemusInSettings).WithMany().HasForeignKey(d => d.ArtemusInSettingsId);
        }
    }
}
