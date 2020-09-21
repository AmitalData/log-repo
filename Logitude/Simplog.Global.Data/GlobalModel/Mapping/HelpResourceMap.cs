using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Mapping
{
    public class HelpResourceMap : EntityTypeConfiguration<HelpResource>
    {
        public HelpResourceMap()
        {
            // Primary Key
            this.HasKey(t => t.Code);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(15)
                 .IsUnicode(false);
            
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.Language)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false);

            this.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false);

            this.Property(t => t.Category)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false);

            this.Property(t => t.VideoURL)
                .HasMaxLength(250)
                .IsUnicode(false);

            this.Property(t => t.Duration)
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.FileName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.Property(t => t.FeatureCode)
                .HasMaxLength(40)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("HelpResources");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.Language).HasColumnName("Language");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.VideoURL).HasColumnName("VideoURL");
            this.Property(t => t.Duration).HasColumnName("Duration");
            this.Property(t => t.FileName).HasColumnName("FileName");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.IsNew).HasColumnName("IsNew");
            this.Property(t => t.FeatureCode).HasColumnName("FeatureCode");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Inactive).HasColumnName("Inactive");
        }
    }
}
