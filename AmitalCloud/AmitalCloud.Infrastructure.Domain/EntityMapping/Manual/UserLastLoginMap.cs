using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class UserLastLoginMap : EntityTypeConfiguration<UserLastLogin>
    {
        public UserLastLoginMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ComputerId)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.WorkEnvironment)
                .HasMaxLength(40)
                .IsUnicode(false);



            // Table & Column Mappings
            this.ToTable("UserLastLogins");
            this.Property(t => t.ComputerId).HasColumnName("ComputerId");
            this.Property(t => t.LoginDateTime).HasColumnName("LoginDateTime");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.WorkEnvironment).HasColumnName("WorkEnvironment");

            // Relationships
            this.HasRequired(t => t.User)
                //.WithOptional(t => t.UserLastLogin) TODO Vladi - relation is not correct
                ;

        }
    }
}
