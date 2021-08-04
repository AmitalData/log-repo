using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;
using Logitude.ShipmentOrderModule.Data.EntityPOCOs;
using Logitude.ShipmentOrderModule.Data;
 
namespace Logitude.ShipmentOrderModule.Data.EntityMapping
{
 
    public class ShipmentOrderMap : EntityTypeConfiguration<ShipmentOrder>
    {
	    string dbms;
        public ShipmentOrderMap()
        { 
				this.ToTable("ShipmentOrders");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.OrderNumber).HasColumnName("OrderNumber").IsRequired().HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.TransportModeId).HasColumnName("TransportModeId").IsRequired().HasMaxLength(1).IsFixedLength();

            this.Property(t => t.ConsigneeId).HasColumnName("ConsigneeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipperId).HasColumnName("ShipperId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AgentId).HasColumnName("AgentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IncotermId).HasColumnName("IncotermId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AccountManagerId).HasColumnName("AccountManagerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PONumber).HasColumnName("PONumber").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.DescriptionofGoods).HasColumnName("DescriptionofGoods").HasMaxLength(2000).IsUnicode(true);

            this.Property(t => t.ShipmentTypeId).HasColumnName("ShipmentTypeId").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.House).HasColumnName("House").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.VesselId).HasColumnName("VesselId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CustomsAgentId).HasColumnName("CustomsAgentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SpecialServicesTypeId).HasColumnName("SpecialServicesTypeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CustomerRefrences).HasColumnName("CustomerRefrences").HasMaxLength(300).IsUnicode(false);
        }
    }
}
	 