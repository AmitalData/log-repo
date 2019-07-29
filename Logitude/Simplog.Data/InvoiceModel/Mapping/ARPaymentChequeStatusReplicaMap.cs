using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.Mapping
{
   public class ARPaymentChequeStatusReplicaMap: EntityTypeConfiguration<ARPaymentChequeStatusReplica>
    {
        string dbms;
        public ARPaymentChequeStatusReplicaMap()
        {
            this.ToTable("ARPaymentChequeStatuses");

            this.HasKey(t => new { t.Code });

            this.Property(t => t.Code).HasColumnName("Code").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.LocalName).HasColumnName("LocalName").HasMaxLength(60).IsUnicode(true);

            this.Property(t => t.EnglishName).HasColumnName("EnglishName").HasMaxLength(60).IsUnicode(false);

            this.Property(t => t.Inactive).HasColumnName("Inactive");
        }


    }
}
