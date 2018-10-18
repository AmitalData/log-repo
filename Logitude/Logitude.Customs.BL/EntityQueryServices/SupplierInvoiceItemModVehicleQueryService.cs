using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial  class SupplierInvoiceItemModVehicleQueryService
    {

        public List<SupplierInvoiceItemModVehiclePM> GetSupplierInvoiceItemModVehiclesForSupplierInvoice(string declarationId, int invoiceCounterKey, int tenant, List<int> FilterLine = null)
        {
            List<SupplierInvoiceItemModVehicle> supplierInvoiceItemModVehicles = repository.GetSupplierInvoiceItemModVehiclesForSupplierInvoice(declarationId, invoiceCounterKey, tenant, FilterLine);
            List<SupplierInvoiceItemModVehiclePM> supplierInvoiceItemModVehiclePMs = (from a in supplierInvoiceItemModVehicles
                                                                                select new SupplierInvoiceItemModVehiclePM()
                                                                                {
                                                                                    DeclarationId = a.DeclarationId,
                                                                                    InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                    AdjustmentTypeCode = a.AdjustmentTypeCode,
                                                                                    DeductAmount = a.DeductAmount,
                                                                                    InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                   
                                                                                    Tenant = a.Tenant,

                                                                                }).ToList();
            return supplierInvoiceItemModVehiclePMs;

        }

        public List<SupplierInvoiceItemModVehiclePM> GetSupplierInvoiceItemModVehiclesForSupplierInvoiceWithSpecificKeys(string declarationId, int invoiceCounterKey, List<int> itemsLineNumbers, int tenant)
        {
            List<SupplierInvoiceItemModVehicle> supplierInvoiceItemModVehicles = repository.GetSupplierInvoiceItemModVehiclesForSupplierInvoiceWithSpecificKeys(declarationId, invoiceCounterKey,itemsLineNumbers, tenant);
            List<SupplierInvoiceItemModVehiclePM> supplierInvoiceItemModVehiclePMs = (from a in supplierInvoiceItemModVehicles
                                                                                      select new SupplierInvoiceItemModVehiclePM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                          AdjustmentTypeCode = a.AdjustmentTypeCode,
                                                                                          DeductAmount = a.DeductAmount,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,

                                                                                          Tenant = a.Tenant,

                                                                                      }).ToList();
            return supplierInvoiceItemModVehiclePMs;

        }

        public List<SupplierInvoiceItemModVehiclePM> GetSupplierInvoiceItemModVehiclesForSupplierInvoiceItem(string declarationId, int invoiceCounterKey, int lineNumber, int tenant)
        {
            List<SupplierInvoiceItemModVehicle> supplierInvoiceItemModVehicles = repository.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId = declarationId, CounterKey = invoiceCounterKey, LineNumber = lineNumber });
            List<SupplierInvoiceItemModVehiclePM> supplierInvoiceItemModVehiclePMs = (from a in supplierInvoiceItemModVehicles
                                                                                      select new SupplierInvoiceItemModVehiclePM()
                                                                            {
                                                                                DeclarationId = a.DeclarationId,
                                                                                InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                AdjustmentTypeCode=a.AdjustmentTypeCode,
                                                                                InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                Tenant = a.Tenant,

                                                                            }).ToList();
            return supplierInvoiceItemModVehiclePMs;

        }
    }
}
