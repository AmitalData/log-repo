using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{

    public partial class SupplierInvoiceItemVehicleModQueryService // moran 20.10.15 - Task 17209 
    {

        public List<SupplierInvoiceItemVehicleModPM> GetSupplierInvoiceItemVehicleModsForSupplierInvoice(string declarationId, int invoiceCounterKey, int invoiceItemLineNumber, int tenant)
        {
            List<SupplierInvoiceItemVehicleMod> supplierInvoiceItemVehicleMods = repository.GetSupplierInvoiceItemVehicleModsForSupplierInvoice(declarationId, invoiceCounterKey, invoiceItemLineNumber, tenant);
            List<SupplierInvoiceItemVehicleModPM> supplierInvoiceItemVehicleModPMs = (from a in supplierInvoiceItemVehicleMods
                                                                                select new SupplierInvoiceItemVehicleModPM()
                                                                                {
                                                                                    DeclarationId = a.DeclarationId,
                                                                                    InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                    AdjustmentTypeCode = a.AdjustmentTypeCode,
                                                                                    DeductAmount = a.DeductAmount,
                                                                                    VehicleLineNumber = a.VehicleLineNumber,
                                                                                    InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                    LineNumber = a.LineNumber,
                                                                                    Tenant = a.Tenant,
                                                                                    
                                                                                }).ToList();
            return supplierInvoiceItemVehicleModPMs;

        }

        public List<SupplierInvoiceItemVehicleModPM> GetSupplierInvoiceItemVehicleModsForSupplierInvoiceForVehicle(string declarationId, int invoiceCounterKey, int invoiceItemLineNumber,int vehicleLineNumber, int tenant)
        {
            List<SupplierInvoiceItemVehicleMod> supplierInvoiceItemVehicleMods = repository.GetSupplierInvoiceItemVehicleModsForSupplierInvoiceForVehicle(declarationId, invoiceCounterKey, invoiceItemLineNumber,vehicleLineNumber, tenant);
            List<SupplierInvoiceItemVehicleModPM> supplierInvoiceItemVehicleModPMs = (from a in supplierInvoiceItemVehicleMods
                                                                                      select new SupplierInvoiceItemVehicleModPM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                          AdjustmentTypeCode = a.AdjustmentTypeCode,
                                                                                          DeductAmount = a.DeductAmount,
                                                                                          VehicleLineNumber = a.VehicleLineNumber,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          LineNumber = a.LineNumber,
                                                                                          Tenant = a.Tenant,

                                                                                      }).ToList();
            return supplierInvoiceItemVehicleModPMs;

        }
    }
}
