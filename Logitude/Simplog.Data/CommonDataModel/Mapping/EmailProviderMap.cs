using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class EmailProviderMap : EntityTypeConfiguration<EmailProvider>
    {
        public EmailProviderMap()
        {
            // Primary Key
            this.HasKey(t => t.ProviderNumber);

            // Properties
            this.Property(t => t.ProviderNumber)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Domain)
                .HasMaxLength(60)
                .IsUnicode(false);

            this.Property(t => t.Password)
                .HasMaxLength(60)
                .IsUnicode(false);

            this.Property(t => t.Port)
                .HasMaxLength(10).IsUnicode(false);
                

            this.Property(t => t.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.Status)
            .HasMaxLength(15)
            .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("EmailProviders");
            this.Property(t => t.ProviderNumber).HasColumnName("ProviderNumber");
            this.Property(t => t.Domain).HasColumnName("Domain");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.Port).HasColumnName("Port");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.LastTestReceivedDate).HasColumnName("LastTestReceivedDate");
            this.Property(t => t.LastTestSendDate).HasColumnName("LastTestSendDate");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.SupportsEmailDelivery).HasColumnName("SupportsEmailDelivery");
        }
    }
}
