using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Global.Data.GlobalModel.Mapping
{
    public class ApiCredintialsMap : EntityTypeConfiguration<ApiCredintials>
    {
        public ApiCredintialsMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AllowedIPs).IsRequired().HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.CreateDate);
            this.Property(t => t.HashedPrimaryAccessKey).IsRequired().HasMaxLength(40).IsUnicode(true);
            this.Property(t => t.HashedSeconderyAccessKey).IsRequired().HasMaxLength(40).IsUnicode(true);
            this.Property(t => t.maskedPrimaryAccessKey).IsRequired().HasMaxLength(40).IsUnicode(true);
            this.Property(t => t.maskedSeconderyAccessKey).IsRequired().HasMaxLength(40).IsUnicode(true);
            this.Property(t => t.UpdateDate);
            this.Property(t => t.UsedFor).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.CreatedBy).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.UpdatedBy).IsRequired().HasMaxLength(100).IsUnicode(false);
            //this.Property(t => t.ComputingPartnerId).HasMaxLength(15).IsUnicode(false);
            
            this.ToTable("ApiCredintials");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.AllowedIPs).HasColumnName("AllowedIPs");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.HashedPrimaryAccessKey).HasColumnName("HashedPrimaryAccessKey");
            this.Property(t => t.HashedSeconderyAccessKey).HasColumnName("HashedSeconderyAccessKey");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UsedFor).HasColumnName("UsedFor");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.maskedPrimaryAccessKey).HasColumnName("maskedPrimaryAccessKey");
            this.Property(t => t.maskedSeconderyAccessKey).HasColumnName("maskedSeconderyAccessKey");
            //this.Property(t => t.ComputingPartnerId).HasColumnName("ComputingPartnerId");
            


        }
    }
}
