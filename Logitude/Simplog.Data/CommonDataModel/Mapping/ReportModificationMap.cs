using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ReportModificationMap : EntityTypeConfiguration<ReportModification>
    {
        public ReportModificationMap()
        {
            this.HasKey(t => new { t.ReportId, t.Tenant });

            this.Property(t => t.ReportId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ReportDocumentId).IsRequired().HasMaxLength(15).IsUnicode(false);

            this.ToTable("ReportModifications");
            this.Property(t => t.ReportId).HasColumnName("PackageId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ReportDocumentId).HasColumnName("ReportDocumentId");

        }
    }
}
