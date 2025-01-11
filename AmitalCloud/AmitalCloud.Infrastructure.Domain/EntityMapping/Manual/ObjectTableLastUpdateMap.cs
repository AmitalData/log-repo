using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class ObjectTableLastUpdateMap : EntityTypeConfiguration<ObjectTableLastUpdate>
    {
        public ObjectTableLastUpdateMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.ToTable("ObjectTableLastUpdates");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.LastUpdateDate).HasColumnName("LastUpdateDate");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");

            this.HasRequired(t => t.UpdatedByUser)
               .WithMany()
               .HasForeignKey(d => d.UpdatedByUserId);

            this.HasRequired(t => t.ObjectTable)
             .WithMany()
             .HasForeignKey(d => d.ObjectTableId);
        }
    }
}
