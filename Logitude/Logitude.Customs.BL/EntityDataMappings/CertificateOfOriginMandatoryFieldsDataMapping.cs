
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
   
   public partial class CertificateOfOriginMandatoryFieldsDataMapping: IMapping<CertificateOfOriginMandatoryFieldsPM, CertificateOfOriginMandatoryFields>
   {

        public void CustomPMToPOCO(CertificateOfOriginMandatoryFieldsPM entityPM, CertificateOfOriginMandatoryFields entityPOCO)
        {
            if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Code))
            {
                entityPOCO.Code = entityPM.Code;
            }
        }

        public void CustomPOCOToPM(CertificateOfOriginMandatoryFieldsPM entityPM, CertificateOfOriginMandatoryFields entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   