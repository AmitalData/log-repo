using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class OtherParticipantIdMap : EntityTypeConfiguration<OtherParticipantId>
    {
        public OtherParticipantIdMap()
        {
            this.HasKey(t => t.Code);
            this.Property(t => t.Code).IsRequired().HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);

            this.ToTable("OtherParticipantIds");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
        }
    }
}
