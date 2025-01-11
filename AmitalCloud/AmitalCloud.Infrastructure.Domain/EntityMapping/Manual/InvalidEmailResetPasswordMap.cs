
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;

public class InvalidEmailResetPasswordMap : EntityTypeConfiguration<InvalidEmailResetPassword>
{

    public InvalidEmailResetPasswordMap()
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


        this.Property(t => t.Email)
        .HasMaxLength(70)
        .IsUnicode(false);


        // Table & Column Mappings
        this.ToTable("InvalidEmailResetPasswords");
        this.Property(t => t.Id).HasColumnName("Id");
        this.Property(t => t.Email).HasColumnName("Email");
        this.Property(t => t.CreateDate).HasColumnName("CreateDate");
        this.Property(t => t.IP).HasColumnName("IP");
    }
}
