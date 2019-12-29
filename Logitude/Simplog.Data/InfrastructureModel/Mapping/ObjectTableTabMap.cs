using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class ObjectTableTabMap : EntityTypeConfiguration<ObjectTableTab>
    {
        public ObjectTableTabMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.TabNameTextCodeId)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ControlPath)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);

            this.Property(t => t.FeatureId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.TabNameTextCodeCode)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ObjectTableTabs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.TabNameTextCodeId).HasColumnName("TabNameTextCodeId");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.IndexOrder).HasColumnName("IndexOrder");
            this.Property(t => t.ControlPath).HasColumnName("ControlPath");
            this.Property(t => t.FeatureId).HasColumnName("FeatureId");
            this.Property(t => t.TabNameTextCodeCode).HasColumnName("TabNameTextCodeCode");

            // Relationships
            this.HasOptional(t => t.Feature)
                .WithMany()
                .HasForeignKey(d => d.FeatureId);
            //this.HasRequired(t => t.ObjectTable)
            //    .WithMany(t => t.ObjectTableTabs)
            //    .HasForeignKey(d => d.ObjectTableId);
            //this.HasRequired(t => t.TabNameTextCode)
            //    .WithMany(t => t.ObjectTableTabs)
            //    .HasForeignKey(d => d.TabNameTextCodeId);

            this.Property(t => t.HtmlComponentName)
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.HtmlComponentUrl)
                .HasMaxLength(256)
                .IsUnicode(false);
        }
    }
}
