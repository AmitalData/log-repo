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

    public partial class SupplierInvoiceItemVehicleAddQueryService
    {// moran 14.3.16 - AMI-55746 

        public List<SupplierInvoiceItemVehicleAddPM> GetSupplierInvoiceItemVehicleAddsForSupplierInvoice(string declarationId, int invoiceCounterKey, int invoiceItemLineNumber, int tenant)
        {
            List<SupplierInvoiceItemVehicleAdd> supplierInvoiceItemVehicleAdds = repository.GetSupplierInvoiceItemVehicleAddsForSupplierInvoice(declarationId, invoiceCounterKey, invoiceItemLineNumber, tenant);

            
            List<SupplierInvoiceItemVehicleAddPM> supplierInvoiceItemVehicleAddPMs = (from a in supplierInvoiceItemVehicleAdds
                                                                                          //select new SupplierInvoiceItemVehicleAddPM()
                                                                                          //{
                                                                                          //    DeclarationId = a.DeclarationId,
                                                                                          //    InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                          //    InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          //    LineNumber = a.LineNumber,
                                                                                          //    ChassisNumber = a.ChassisNumber,
                                                                                          //    ChassisPurchaseTax = a.ChassisPurchaseTax,
                                                                                          //    ChassisTax = a.ChassisTax,
                                                                                          //    ChassisVat = a.ChassisVat,
                                                                                          //    EngineNumber = a.EngineNumber,
                                                                                          //    Exempt_type = a.Exempt_type,
                                                                                          //    RichbitNumber = a.RichbitNumber,
                                                                                          //    VehicleModel = a.VehicleModel,
                                                                                          //    VehicleValue = a.VehicleValue,

                                                                                          //}
                                                                                      select  this.GetEntityPM(a)
                                                                                ).ToList();
            return supplierInvoiceItemVehicleAddPMs;

        }

        public List<SupplierInvoiceItemVehicleAddPM> GetSupplierInvoiceItemVehicleAddsForSupplierInvoiceItemVehicle(string declarationId, int invoiceCounterKey, int invoiceItemLineNumber,int lineNumber,int tenant)
        {
            List<SupplierInvoiceItemVehicleAdd> supplierInvoiceItemVehicleAdds = repository.GetSupplierInvoiceItemVehicleAddsForSupplierInvoiceForVehicle(declarationId, invoiceCounterKey, invoiceItemLineNumber,lineNumber, tenant);
            List<SupplierInvoiceItemVehicleAddPM> supplierInvoiceItemVehicleAddPMs = (from a in supplierInvoiceItemVehicleAdds
                                                                                      select
                                                                                      //new SupplierInvoiceItemVehicleAddPM()
                                                                                      //{
                                                                                      //    DeclarationId = a.DeclarationId,
                                                                                      //    InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                      //    InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                      //    LineNumber = a.LineNumber,
                                                                                      //    ChassisNumber = a.ChassisNumber,
                                                                                      //    ChassisPurchaseTax = a.ChassisPurchaseTax,
                                                                                      //    ChassisTax = a.ChassisTax,
                                                                                      //    ChassisVat = a.ChassisVat,
                                                                                      //    EngineNumber = a.EngineNumber,
                                                                                      //    Exempt_type = a.Exempt_type,
                                                                                      //    RichbitNumber = a.RichbitNumber,
                                                                                      //    VehicleModel = a.VehicleModel,
                                                                                      //    VehicleValue = a.VehicleValue,

                                                                                      //}
                                                                                      this.GetEntityPM(a)
                                                                                      ).ToList();
            return supplierInvoiceItemVehicleAddPMs;

        }
    }
}
