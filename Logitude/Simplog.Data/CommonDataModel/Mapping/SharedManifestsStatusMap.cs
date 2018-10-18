using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
   public class SharedManifestsStatusMap : EntityTypeConfiguration<SharedManifestsStatus>
    {
        public SharedManifestsStatusMap()
        {
            this.HasKey(t => t.StatusCode);
            this.Property(t => t.StatusCode).IsRequired().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.StatusName).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);


            this.ToTable("SharedManifestsStatuses");
            this.Property(t => t.StatusCode).HasColumnName("StatusCode");
            this.Property(t => t.StatusName).HasColumnName("StatusName");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");

        }
    }
}
