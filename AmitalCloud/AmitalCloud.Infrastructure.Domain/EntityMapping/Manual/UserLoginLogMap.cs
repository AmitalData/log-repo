using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class UserLoginLogMap : EntityTypeConfiguration<UserLoginLog>
    {
        public UserLoginLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.IP)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Browser)
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.UserId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.UserAgent)
                .HasMaxLength(400)
                .IsUnicode(true);

            this.Property(t => t.ComputerId)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("UserLoginLogs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.IP).HasColumnName("IP");
            this.Property(t => t.Browser).HasColumnName("Browser");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.LocalDateTime).HasColumnName("LocalDateTime");
            this.Property(t => t.GMTDateTime).HasColumnName("GMTDateTime");
            this.Property(t => t.UserAgent).HasColumnName("UserAgent");
            this.Property(t => t.ComputerId).HasColumnName("ComputerId");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany()
                .HasForeignKey(d => d.UserId);

        }
    }
}
