using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class PrepaidCollect
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public bool DisplayInLOV { get; set; }
        public string SearchFields { get; set; }

        //public List<Incoterm> FreightIncoterms { get; set; }
        //public List<Incoterm> OtherChargesIncoterms { get; set; }

      

        //public List<Shipment> FreightShipments { get; set; }
        //public List<Shipment> OtherShipments { get; set; }
        //public List<ShipmentReceivable> ShipmentReceivables { get; set; }

        //public List<Tenant> ExportFreightTenants { get; set; }
        //public List<Tenant> ExportOtherTenants { get; set; }
        //public List<Tenant> ImportFreightTenants { get; set; }
        //public List<Tenant> ImportOtherTenants { get; set; }
        //public List<ARInvoice> ARInvoices { get; set; }

        //public List<ShipmentPayable> ShipmentPayables { get; set; }
        //public List<ShipmentAWBPrintOnly> ShipmentAWBPrintOnlies { get; set; }
        //public List<Tenant> MasterExportFreightTenants { get; set; }
        //public List<Tenant> MasterExportOtherTenants { get; set; }
        //public List<Tenant> MasterImportFreightTenants { get; set; }
        //public List<Tenant> MasterImportOtherTenants { get; set; }
        

    }
}