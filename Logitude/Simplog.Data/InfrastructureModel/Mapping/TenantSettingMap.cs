using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class TenantSettingMap : EntityTypeConfiguration<TenantSetting>
    {
        public TenantSettingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.SettingCode)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.SettingValue)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Prefix)
                .HasMaxLength(10)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("TenantSettings");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.SettingCode).HasColumnName("SettingCode");
            this.Property(t => t.SettingValue).HasColumnName("SettingValue");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.Prefix).HasColumnName("Prefix");
            this.Property(t => t.DontIncludeDirects).HasColumnName("DontIncludeDirects");
            this.Property(t => t.IsDocumentFilingByEmailEnabled).HasColumnName("IsDocumentFilingByEmailEnabled");

            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
          if (dbms == "oracle")
          {
              this.Property(t => t.Size).HasColumnName("Size_");
          }
          //#else
          else
          {
              this.Property(t => t.Size).HasColumnName("Size");
          }
//#endif
            // Relationships
            //this.HasRequired(t => t.ObjectTable)
            //    .WithMany(t => t.TenantSettings)
            //    .HasForeignKey(d => d.ObjectTableId);

        }
    }
}
