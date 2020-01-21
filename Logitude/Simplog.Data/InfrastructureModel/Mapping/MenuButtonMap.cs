using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class MenuButtonMap : EntityTypeConfiguration<MenuButton>
    {
        public MenuButtonMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.LabelTextCodeId)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);

            this.Property(t => t.ParentMenuButtonId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.EventCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            this.Property(t => t.MenuButtonGroupId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.FeatureId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.MenuButtonType)
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.DropDownControl)
                .HasMaxLength(250)
                .IsUnicode(false);

            this.Property(t => t.Style)
                .HasMaxLength(50)
                .IsUnicode(false);

            this.Property(t => t.ControlPath)
             .HasMaxLength(250)
             .IsUnicode(false);
            this.Property(t => t.HtmlComponentPath)
            .HasMaxLength(250)
            .IsUnicode(false);
            this.Property(t => t.FeatureUniqeCode)
                .HasMaxLength(120)
                .IsUnicode(false);

            this.Property(t => t.LabelTextCodeCode)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("MenuButtons");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.LabelTextCodeId).HasColumnName("LabelTextCodeId");
            this.Property(t => t.ParentMenuButtonId).HasColumnName("ParentMenuButtonId");
            this.Property(t => t.EventCode).HasColumnName("EventCode");
            this.Property(t => t.LabelTextCodeCode).HasColumnName("LabelTextCodeCode");

            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.MenuButtonGroupId).HasColumnName("MenuButtonGroupId");
            this.Property(t => t.FeatureId).HasColumnName("FeatureId");
            this.Property(t => t.MenuButtonType).HasColumnName("MenuButtonType");
            this.Property(t => t.DropDownControl).HasColumnName("DropDownControl");
            this.Property(t => t.Style).HasColumnName("Style");
            this.Property(t => t.Width).HasColumnName("Width");
            this.Property(t => t.ControlPath).HasColumnName("ControlPath");
            this.Property(t => t.HtmlComponentPath).HasColumnName("HtmlComponentPath");
            this.Property(t => t.FeatureUniqeCode).HasColumnName("FeatureUniqeCode");

            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
           if (dbms == "oracle")
           {
               this.Property(t => t.Index).HasColumnName("Index_");
           }
           //#else
           else
           {
               this.Property(t => t.Index).HasColumnName("Index");
           }
//#endif
            // Relationships
            //this.HasOptional(t => t.Feature)
            //    .WithMany()
            //    .HasForeignKey(d => d.FeatureId);
            this.HasRequired(t => t.MenuButtonGroup)
                .WithMany(t => t.MenuButtons)
                .HasForeignKey(d => d.MenuButtonGroupId);
            this.HasOptional(t => t.ParentMenuButton)
                .WithMany(t => t.ChildrenMenuButtons)
                .HasForeignKey(d => d.ParentMenuButtonId);
            //this.HasRequired(t => t.TextCode)
            //    .WithMany(t => t.MenuButtons)
            //    .HasForeignKey(d => d.LabelTextCodeId);

        }
    }
}
