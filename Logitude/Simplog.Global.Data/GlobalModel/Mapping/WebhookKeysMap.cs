using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Mapping
{
    public class WebhookKeysMap : EntityTypeConfiguration<WebhookKeys>
    {
        
        public WebhookKeysMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);

            // Properties
            this.Property(t => t.Tenant)
                .IsRequired();

            this.Property(t => t.InActive)
              .IsRequired();

            this.Property(t => t.AccessKey)
        .HasMaxLength(200)
        .IsUnicode(false);

            this.Property(t => t.PartnerName)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode(false);

            this.Property(t => t.CreatedByUserName)
          .IsRequired()
          .HasMaxLength(150)
          .IsUnicode(false);

            this.Property(t => t.UpdatedByUserName)
          .IsRequired()
          .HasMaxLength(150)
          .IsUnicode(false);

            this.Property(t => t.Description)
         .HasMaxLength(200)
         .IsUnicode(false);
             

            // Table & Column Mappings
            this.ToTable("WebhookKeys");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.AccessKey).HasColumnName("AccessKey");
            this.Property(t => t.PartnerName).HasColumnName("PartnerName");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t =>t.CreatedByUserName).HasColumnName("CreatedByUserName");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdatedByUserName).HasColumnName("UpdatedByUserName");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.Description).HasColumnName("Description");

        }
    }
}
