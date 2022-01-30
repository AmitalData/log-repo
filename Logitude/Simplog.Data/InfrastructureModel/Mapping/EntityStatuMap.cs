using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class EntityStatuMap : EntityTypeConfiguration<EntityStatus>
    {
        public EntityStatuMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);


            this.Property(t => t.DisplayName)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.EntityStatusTypeCode)
                .HasMaxLength(3)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("EntityStatus");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.StatusWeight).HasColumnName("StatusWeight");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.DisplayName).HasColumnName("DisplayName");
            this.Property(t => t.AutomaticLastUpdateDate).HasColumnName("AutomaticLastUpdateDate");
            this.Property(t => t.StatusLocalWeight).HasColumnName("StatusLocalWeight");
            this.Property(t => t.EntityStatusTypeCode).HasColumnName("EntityStatusTypeCode");
            this.Property(t => t.AllowPartial).HasColumnName("AllowPartial");
            // Relationships
            //this.HasRequired(t => t.ObjectTable)
            //    .WithMany(t => t.EntityStatus)
            //    .HasForeignKey(d => d.ObjectTableId);
            this.HasOptional(t => t.EntityStatusType).WithMany().HasForeignKey(d => d.EntityStatusTypeCode);

        }
    }
}
