using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{

    public class EventRemarkMap : EntityTypeConfiguration<EventRemark>
        {
            string dbms;
            public EventRemarkMap()
            {
                this.ToTable("EventRemarks");
                this.HasKey(t => new { t.Id });
                this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);
                this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();
                this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();
                this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);
                this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);
                this.Property(t => t.EventTypeId).HasColumnName("EventTypeId").HasMaxLength(15).IsUnicode(false);
                this.Property(t => t.PartnerTypeId).HasColumnName("PartnerTypeId").HasMaxLength(2).IsFixedLength();
                this.Property(t => t.IsChoose).HasColumnName("IsChoose");
            }
        }
}



