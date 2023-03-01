using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CourierDeclarationStatusesViewMap : EntityTypeConfiguration<CourierDeclarationStatusesView>
    {

        public CourierDeclarationStatusesViewMap()
        {
            this.HasKey(t => new { t.CourierMasterId });

            this.Property(t => t.CourierMasterId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Tenant).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.QuantityNoDocuments).HasColumnName("QuantityNoDocuments");
            this.Property(t => t.QuantityNoClassification).HasColumnName("QuantityNoClassification");
            this.Property(t => t.QuantityNoManifest).HasColumnName("QuantityNoManifest");
            this.Property(t => t.QuantityNoDeclaration).HasColumnName("QuantityNoDeclaration");
            this.Property(t => t.DocumentStatusCode).HasColumnName("DocumentStatusCode");
            this.Property(t => t.CourierPaymentStatusCode).HasColumnName("CourierPaymentStatusCode");
            this.Property(t => t.CourierDeclarationStatusCode).HasColumnName("CourierDeclarationStatusCode");
            this.Property(t => t.CourierManifestStatusCode).HasColumnName("CourierManifestStatusCode");
            this.Property(t => t.IsCourierMissingClassification).HasColumnName("IsCourierMissingClassification");
        }

    }
}
