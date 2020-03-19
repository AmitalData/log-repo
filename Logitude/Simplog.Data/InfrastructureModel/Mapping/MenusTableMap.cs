using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class MenusTableMap : EntityTypeConfiguration<MenusTable>
    {
        public MenusTableMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.MenuTypeCode)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.Icon)
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.CategoryTypeCode)
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.TextCode)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            this.Property(t => t.UserControlName)
                .HasMaxLength(120)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.FeatureId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.FeatureUniqeCode)
                .HasMaxLength(120)
                .IsUnicode(false); 

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.HtmlView)
                .HasMaxLength(120)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("MenusTables");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.MenuTypeCode).HasColumnName("MenuTypeCode");
            this.Property(t => t.IndexOfOrder).HasColumnName("IndexOfOrder");
            this.Property(t => t.Icon).HasColumnName("Icon");
            this.Property(t => t.CategoryTypeCode).HasColumnName("CategoryTypeCode");
            this.Property(t => t.TextCode).HasColumnName("TextCode");
            this.Property(t => t.UserControlName).HasColumnName("UserControlName");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.FeatureId).HasColumnName("FeatureId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.HtmlView).HasColumnName("HtmlView");
            this.Property(t => t.FeatureUniqeCode).HasColumnName("FeatureUniqeCode");


            // Relationships
            //this.HasOptional(t => t.CategoryType)
            //    .WithMany(t => t.MenusTables)
            //    .HasForeignKey(d => d.CategoryTypeCode);
            //this.HasOptional(t => t.Feature)
            //    .WithMany()
            //    .HasForeignKey(d => d.FeatureId);
            //this.HasRequired(t => t.MenuType)
            //    .WithMany(t => t.MenusTables)
            //    .HasForeignKey(d => d.MenuTypeCode);
            //this.HasOptional(t => t.ObjectTable)
            //    .WithMany(t => t.MenusTables)
            //    .HasForeignKey(d => d.ObjectTableId);

        }
    }
}
