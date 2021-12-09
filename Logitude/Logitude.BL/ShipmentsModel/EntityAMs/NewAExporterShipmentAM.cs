using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityAMs
{
    public class NewAExporterShipmentAM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public int ExporterTenant { get; set; }
        public string TransportModeId { get; set; }
        public string DirectionId { get; set; } 
        public string CustomerShipmentNumber { get; set; } 
        public CodeProperties Shipper { get; set; } 
        public CodeProperties FromPort { get; set; } 
        public CodeProperties ToPort { get; set; } 
        public string ShipmentTypeId { get; set; } 
        public CodeProperties Customer { get; set; }
        public string ConsigneeName { get; set; } 
        public string InvoiceReference { get; set; }
        public string CustomerReference { get; set; }
        public bool IncludePickup { get; set; }
        public bool IncludeDelivery { get; set; }
        public string Incoterm { get; set; }

        public DateTime? ReqFlightDate { get; set; }
        public int? Quantity { get; set; }
        public double? Weight { get; set; }
        public double? Volume { get; set; }
        public List<Packages> ShipmentPackages { get; set; }
        //public bool SendUpdatesToAgentEnabled { get; set; }

        public string Notes { get; set; }
        public bool IsDangerouseOfGoods { get; set; }
        public CodeProperties Agent { get; set; }

        public CodeProperties Consignee { get; set; }


    }
}
