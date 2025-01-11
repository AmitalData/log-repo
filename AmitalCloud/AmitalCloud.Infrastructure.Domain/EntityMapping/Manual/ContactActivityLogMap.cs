using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class ContactActivityLogMap : EntityTypeConfiguration<ContactActivityLog>
    {
        public ContactActivityLogMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.Module)
                .HasMaxLength(100)
                .IsRequired()
                .IsUnicode(false);

            this.Property(t => t.Activity)
                .HasMaxLength(150)
                .IsRequired()
                .IsUnicode(false);

            this.Property(t => t.ContactId)
               .IsRequired()
               .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.LogDateTime)
                .IsRequired();

            this.Property(t => t.CardId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.PartnerTypeId)
               .HasMaxLength(2)
               .IsUnicode(false);

              this.Property(t => t.Via)
               .HasMaxLength(20)
               .IsUnicode(false);
           
            this.ToTable("ContactActivityLogs");

            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Module).HasColumnName("Module");
            this.Property(t => t.Activity).HasColumnName("Activity");
            this.Property(t => t.ContactId).HasColumnName("ContactId");
            this.Property(t => t.LogDateTime).HasColumnName("LogDateTime");
            this.Property(t => t.IsSharedLogisticsContact).HasColumnName("IsSharedLogisticsContact");
            this.Property(t => t.CardId).HasColumnName("CardId");
            this.Property(t => t.PartnerTypeId).HasColumnName("PartnerTypeId");
            this.Property(t => t.Via).HasColumnName("Via"); 
        }
    }
}