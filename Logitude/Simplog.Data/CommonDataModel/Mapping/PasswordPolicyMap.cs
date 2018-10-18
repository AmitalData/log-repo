using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class PasswordPolicyMap : EntityTypeConfiguration<PasswordPolicy>
    {
        public PasswordPolicyMap()
        {
            // Primary Key
            this.HasKey(t => t.Code);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.PasswordStrength)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(true);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("PasswordPolicies");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.PasswordStrength).HasColumnName("PasswordStrength");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
        }
    }
}
