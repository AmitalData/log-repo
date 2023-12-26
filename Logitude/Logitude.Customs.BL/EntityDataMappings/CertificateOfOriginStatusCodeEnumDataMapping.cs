
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
   
   public partial class CertificateOfOriginStatusCodeEnumDataMapping: IMapping<CertificateOfOriginStatusCodeEnumPM, CertificateOfOriginStatusCodeEnum>
   {

        public void CustomPMToPOCO(CertificateOfOriginStatusCodeEnumPM entityPM, CertificateOfOriginStatusCodeEnum entityPOCO)
        {
            if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Code))
            {
                entityPOCO.Code = entityPM.Code;
            }
        }

        public void CustomPOCOToPM(CertificateOfOriginStatusCodeEnumPM entityPM, CertificateOfOriginStatusCodeEnum entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   