using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentDocsFieldMap : EntityTypeConfiguration<ShipmentDocsField>
    {
        public ShipmentDocsFieldMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            

            // Table & Column Mappings
            this.ToTable("ShipmentDocsFields");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.IsPODReceived).HasColumnName("IsPODReceived");
            this.Property(t => t.PODReceivedDate).HasColumnName("PODReceivedDate");
            this.Property(t => t.IsCommercialInvoiceReceived).HasColumnName("IsCommercialInvoiceReceived");
            this.Property(t => t.CommercialInvoiceReceivedDate).HasColumnName("CommercialInvoiceReceivedDate");
            this.Property(t => t.IsPackingListReceived).HasColumnName("IsPackingListReceived");
            this.Property(t => t.PackingListReceivedDate).HasColumnName("PackingListReceivedDate");
            this.Property(t => t.IsBOLReceived).HasColumnName("IsBOLReceived");
            this.Property(t => t.BOLReceivedDate).HasColumnName("BOLReceivedDate");
            this.Property(t => t.IsMasterBOLReceived).HasColumnName("IsMasterBOLReceived");
            this.Property(t => t.MasterBOLReceivedDate).HasColumnName("MasterBOLReceivedDate");
            this.Property(t => t.IsArrivalNoticeReceived).HasColumnName("IsArrivalNoticeReceived");
            this.Property(t => t.ArrivalNoticeReceivedDate).HasColumnName("ArrivalNoticeReceivedDate");
        }
    }
}
