using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class MonitorServiceLastUpdateMap : EntityTypeConfiguration<MonitorServiceLastUpdate>
    {
        public MonitorServiceLastUpdateMap()
        {
            this.HasKey(t => t.Code);
            this.Property(t => t.Code).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);

            this.ToTable("MonitorServiceLastUpdates");

            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.LastUpdate).HasColumnName("LastUpdate");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
        }
    }
}
