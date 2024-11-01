using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class QueryColumnMap : EntityTypeConfiguration<QueryColumn>
    {
        public QueryColumnMap()
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

            this.Property(t => t.ObjectFieldId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.QueryCode)
              .HasMaxLength(200)
              .IsUnicode(false);

            //this.Property(t => t.QueryCode)
            // .IsRequired()
            // .HasMaxLength(200)
            // .IsUnicode(false);

            this.Property(t => t.UserId)
                .HasMaxLength(15)
                .IsUnicode(false);

            //this.Property(t => t.ObjectFieldCode)
            //    .IsRequired()
            //    .HasMaxLength(200)
            //    .IsUnicode(false);

            this.Property(t => t.ObjectFieldCode)
                .HasMaxLength(200)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("QueryColumns");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.QueryId).HasColumnName("QueryId");
            this.Property(t => t.QueryCode).HasColumnName("QueryCode");

            this.Property(t => t.ObjectFieldId).HasColumnName("ObjectFieldId");
            this.Property(t => t.IndexOrder).HasColumnName("IndexOrder");
            this.Property(t => t.ColumnWidth).HasColumnName("ColumnWidth");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.ObjectFieldCode).HasColumnName("ObjectFieldCode");
            // Relationships
            //this.HasRequired(t => t.ObjectField)
            //    .WithMany(t => t.QueryColumns)
            //    .HasForeignKey(d => d.ObjectFieldId);
            //this.HasRequired(t => t.Query)
            //    .WithMany(t => t.QueryColumns)
            //    .HasForeignKey(d => d.QueryId);
            this.HasOptional(t => t.User)
                .WithMany()
                .HasForeignKey(d => d.UserId);

        }
    }
}
