using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class ObjectFieldValidationMap : EntityTypeConfiguration<ObjectFieldValidation>
    {
        public ObjectFieldValidationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ValidationExpression)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);

            this.Property(t => t.ErrorMessage)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);

            this.Property(t => t.ObjectFieldId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Condition)
                .HasMaxLength(250)
                .IsUnicode(false);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ObjectFieldValidations");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ValidationExpression).HasColumnName("ValidationExpression");
            this.Property(t => t.ErrorMessage).HasColumnName("ErrorMessage");
            this.Property(t => t.ObjectFieldId).HasColumnName("ObjectFieldId");
            this.Property(t => t.ValidationOrder).HasColumnName("ValidationOrder");
            this.Property(t => t.Condition).HasColumnName("Condition");
            this.Property(t => t.Code).HasColumnName("Code");

            // Relationships
            //this.HasRequired(t => t.ObjectField)
            //    .WithMany(t => t.ObjectFieldValidations)
            //    .HasForeignKey(d => d.ObjectFieldId);

        }
    }
}
