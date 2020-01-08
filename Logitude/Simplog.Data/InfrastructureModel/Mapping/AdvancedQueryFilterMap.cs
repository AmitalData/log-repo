using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class AdvancedQueryFilterMap : EntityTypeConfiguration<AdvancedQueryFilter>
    {
        public AdvancedQueryFilterMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.QueryId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.QueryCode)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);

            this.Property(t => t.ObjectFieldId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.PredefinedValue)
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.PredefinedValue2)
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.Operator)
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.UserId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ObjectFieldCode)
                 .IsRequired()
                 .HasMaxLength(200)
                 .IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("AdvancedQueryFilters");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.QueryId).HasColumnName("QueryId");
            this.Property(t => t.QueryCode).HasColumnName("QueryCode");
            this.Property(t => t.ObjectFieldId).HasColumnName("ObjectFieldId");
            this.Property(t => t.IsPredefined).HasColumnName("IsPredefined");
            this.Property(t => t.PredefinedValue).HasColumnName("PredefinedValue");
            this.Property(t => t.PredefinedValue2).HasColumnName("PredefinedValue2");
            this.Property(t => t.Operator).HasColumnName("Operator");
            this.Property(t => t.IndexOrder).HasColumnName("IndexOrder");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.ObjectFieldCode).HasColumnName("ObjectFieldCode");
            // Relationships
            this.HasOptional(t => t.User)
                .WithMany()
                .HasForeignKey(d => d.UserId);
            //this.HasRequired(t => t.ObjectField)
            //    .WithMany(t => t.AdvancedQueryFilters)
            //    .HasForeignKey(d => d.ObjectFieldId);
            //this.HasRequired(t => t.Query)
            //    .WithMany(t => t.AdvancedQueryFilters)
            //    .HasForeignKey(d => d.QueryId);

        }
    }
}
