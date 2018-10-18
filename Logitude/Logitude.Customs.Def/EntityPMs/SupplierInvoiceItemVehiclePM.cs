using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server; 
using Logitude.Server.Tools; 
using System.Runtime.Serialization; 
using Logitude.Customs.Def.Validators;
  
namespace Logitude.Customs.Def.EntityPMs
{

   public partial class SupplierInvoiceItemVehiclePM : EntityPM
   {// moran 14.3.16 - AMI-55746
#if false
       

       private List<SupplierInvoiceItemVehicleAddPM> supplierInvoiceItemVehicleAdds;

       [Composition]
       [Include]
       [Association("SupplierInvoiceItemVehicleSupplierInvoiceItemVehicleAdds", "DeclarationId,InvoiceCounterKey,InvoiceItemLineNumber,LineNumber", "DeclarationId,InvoiceCounterKey,InvoiceItemLineNumber,LineNumber")]
       [DataMember]
       public virtual List<SupplierInvoiceItemVehicleAddPM> SupplierInvoiceItemVehicleAdds
       {
           get
           {
               if (supplierInvoiceItemVehicleAdds == null)
               {
                   supplierInvoiceItemVehicleAdds = new List<SupplierInvoiceItemVehicleAddPM>();
               }
               return supplierInvoiceItemVehicleAdds;
           }
           set { supplierInvoiceItemVehicleAdds = value; }
       }

       private List<SupplierInvoiceItemVehicleAddPM> deletedSupplierInvoiceItemVehicleAdds;
       public virtual List<SupplierInvoiceItemVehicleAddPM> DeletedSupplierInvoiceItemVehicleAdds
       {
           get
           {
               if (deletedSupplierInvoiceItemVehicleAdds == null)
               {
                   deletedSupplierInvoiceItemVehicleAdds = new List<SupplierInvoiceItemVehicleAddPM>();
               }
               return deletedSupplierInvoiceItemVehicleAdds;
           }
           set { deletedSupplierInvoiceItemVehicleAdds = value; }
       }
#endif
   }
   
}
	 