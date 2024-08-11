using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class SharedManifestTranslationMap : EntityTypeConfiguration<SharedManifestTranslation>
    {
        public SharedManifestTranslationMap()
        {

            // Primary Key
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
               .IsRequired()
               .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.Tenant)
            .IsRequired();

            this.Property(t => t.CreateDate).IsRequired();
            this.Property(t => t.UpdateDate).IsRequired();

            this.Property(t => t.AgentId)
            .HasMaxLength(40)
            .IsUnicode(false);

            this.Property(t => t.AgentCode)
               .HasMaxLength(15)
               .IsUnicode(false).IsRequired();

            this.Property(t => t.MyCode)
                 .HasMaxLength(15)
                 .IsUnicode(false).IsRequired();

            this.Property(t => t.ObjectTableName).IsRequired().HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.CreatedByUserId)
               .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.UpdatedByUserId)
             .HasMaxLength(15)
             .IsUnicode(false);


            this.ToTable("SharedManifestTranslations");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ObjectTableName).HasColumnName("ObjectTableName");
            this.Property(t => t.MyCode).HasColumnName("MyCode");
            this.Property(t => t.AgentCode).HasColumnName("AgentCode");
            this.Property(t => t.AgentId).HasColumnName("AgentId");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");

            // Relationships

            this.HasOptional(t => t.CreatedByUser)
               .WithMany()
               .HasForeignKey(d => d.CreatedByUserId);

            this.HasOptional(t => t.UpdatedByUser)
                .WithMany()
                .HasForeignKey(d => d.UpdatedByUserId);

        }
    }
}
