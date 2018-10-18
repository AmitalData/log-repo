 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class SupplierInvoiceItemVehicleModRepository:IRepository<SupplierInvoiceItemVehicleMod>
   {
       // moran 20.10.15 - Task 17209 -->
        public List<SupplierInvoiceItemVehicleMod> GetMulti(EntityKeyFields entityKeys)
        {

           SupplierInvoiceItemVehicleKeys supplierInvoiceItemKeys = entityKeys as SupplierInvoiceItemVehicleKeys;

            return (from a in context.SupplierInvoiceItemVehicleMods
                   where a.DeclarationId == supplierInvoiceItemKeys.DeclarationId && a.InvoiceCounterKey == supplierInvoiceItemKeys.InvoiceCounterKey && a.InvoiceItemLineNumber == supplierInvoiceItemKeys.InvoiceItemLineNumber && a.VehicleLineNumber == supplierInvoiceItemKeys.LineNumber
                    select a).ToList();
        }

        public List<SupplierInvoiceItemVehicleMod> GetSupplierInvoiceItemVehicleModsForSupplierInvoice(string declarationId, int invoiceCounterKey, int invoiceItemLineNumber, int tenant)
        {
            return (from a in context.SupplierInvoiceItemVehicleMods
                    where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey && a.InvoiceItemLineNumber == invoiceItemLineNumber
                    select a).ToList();
        }
       // moran 20.10.15 - Task 17209 <--


        public void FastDeleteMulti(DeclarationKeys entityKeyFields)
        {

            (context as DbContextBase)
                .DeleteWhere<SupplierInvoiceItemVehicleMod>(rec => rec.DeclarationId == entityKeyFields.Id);
        }

        public List<SupplierInvoiceItemVehicleMod> GetSupplierInvoiceItemVehicleModsForSupplierInvoiceForVehicle(string declarationId, int invoiceCounterKey, int invoiceItemLineNumber,int vehicleLineNumber, int tenant)
        {
            return (from a in context.SupplierInvoiceItemVehicleMods
                    where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey && a.InvoiceItemLineNumber == invoiceItemLineNumber&& a.VehicleLineNumber==vehicleLineNumber
                    select a).ToList();
        }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (context as DbContextBase)
                .DeleteWhere<SupplierInvoiceItemVehicleMod>(rec => rec.DeclarationId == entityKeyFields.DeclarationId && rec.InvoiceCounterKey == entityKeyFields.InvoiceCounterKey && supplierInvoiceItemsParentsLines.Contains(rec.InvoiceItemLineNumber));

        }
        public int? GetMaxLineNumber(string declarationId, int invoiceCounterKey, int invoiceItemLineNumber, int vehicleLineNumber, int tenant)
        {
            return (from a in 
                        context.SupplierInvoiceItemVehicleMods
                    where a.DeclarationId == declarationId && 
                    a.InvoiceCounterKey == invoiceCounterKey && a.InvoiceItemLineNumber == invoiceItemLineNumber &&
                    a.VehicleLineNumber == vehicleLineNumber
                    select a).Max(d => (int?)d.LineNumber) ?? 0;
        }
    }

}
   