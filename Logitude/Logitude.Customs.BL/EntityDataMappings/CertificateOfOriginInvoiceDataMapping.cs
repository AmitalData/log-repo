
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CertificateOfOriginInvoiceDataMapping: IMapping<CertificateOfOriginInvoicePM, CertificateOfOriginInvoice>
   {

        public void CustomPMToPOCO(CertificateOfOriginInvoicePM entityPM, CertificateOfOriginInvoice entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Id))
                {
                    entityPOCO.Id = entityPM.Id;

                }
            }

        }

        public void CustomPOCOToPM(CertificateOfOriginInvoicePM entityPM, CertificateOfOriginInvoice entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   