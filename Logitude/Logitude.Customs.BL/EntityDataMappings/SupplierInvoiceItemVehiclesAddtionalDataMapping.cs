
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.BL.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class SupplierInvoiceItemVehiclesAddtionalDataMapping: IMapping<SupplierInvoiceItemVehiclesAddtionalPM, SupplierInvoiceItemVehiclesAddtional>
   {

        public void CustomPMToPOCO(SupplierInvoiceItemVehiclesAddtionalPM entityPM, SupplierInvoiceItemVehiclesAddtional entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(SupplierInvoiceItemVehiclesAddtionalPM entityPM, SupplierInvoiceItemVehiclesAddtional entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   