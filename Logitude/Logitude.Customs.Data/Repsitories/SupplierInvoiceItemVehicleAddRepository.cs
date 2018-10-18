 
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
   public partial class SupplierInvoiceItemVehicleAddRepository:IRepository<SupplierInvoiceItemVehicleAdd>
   {

       // moran 14.3.16 - AMI-55746 -->
       public List<SupplierInvoiceItemVehicleAdd> GetMulti(EntityKeyFields entityKeys)
       {

           SupplierInvoiceItemVehicleKeys supplierInvoiceItemVehicleKeys = entityKeys as SupplierInvoiceItemVehicleKeys;

           return (from a in context.SupplierInvoiceItemVehicleAdds
                   where a.DeclarationId == supplierInvoiceItemVehicleKeys.DeclarationId && a.InvoiceCounterKey == supplierInvoiceItemVehicleKeys.InvoiceCounterKey && a.InvoiceItemLineNumber == supplierInvoiceItemVehicleKeys.InvoiceItemLineNumber
                   select a).ToList();
       }

       public List<SupplierInvoiceItemVehicleAdd> GetSupplierInvoiceItemVehicleAddsForSupplierInvoice(string declarationId, int invoiceCounterKey, int invoiceItemLineNumber, int tenant)
       {
           return (from a in context.SupplierInvoiceItemVehicleAdds
                   where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey && a.InvoiceItemLineNumber == invoiceItemLineNumber
                   select a).ToList();
       }

       public void FastDeleteMulti(DeclarationKeys entityKeyFields)
       {

           (context as DbContextBase)
               .DeleteWhere<SupplierInvoiceItemVehicleAdd>(rec => rec.DeclarationId == entityKeyFields.Id);
       }
       // moran 14.3.16 - AMI-55746 <--

       public List<SupplierInvoiceItemVehicleAdd> GetSupplierInvoiceItemVehicleAddsForSupplierInvoiceForVehicle(string declarationId, int invoiceCounterKey, int invoiceItemLineNumber,int vehicleLineNumber, int tenant)
       {
           return (from a in context.SupplierInvoiceItemVehicleAdds
                   where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey && a.InvoiceItemLineNumber == invoiceItemLineNumber && a.LineNumber == vehicleLineNumber
                   select a).ToList();
       }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (context as DbContextBase)
                .DeleteWhere<SupplierInvoiceItemVehicleAdd>(rec => rec.DeclarationId == entityKeyFields.DeclarationId && rec.InvoiceCounterKey == entityKeyFields.InvoiceCounterKey && supplierInvoiceItemsParentsLines.Contains(rec.InvoiceItemLineNumber));

        }
    }

}
   