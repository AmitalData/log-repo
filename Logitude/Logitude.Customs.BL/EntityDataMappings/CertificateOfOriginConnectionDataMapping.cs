
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
   
   public partial class CertificateOfOriginConnectionDataMapping: IMapping<CertificateOfOriginConnectionPM, CertificateOfOriginConnection>
   {

        public void CustomPMToPOCO(CertificateOfOriginConnectionPM entityPM, CertificateOfOriginConnection entityPOCO)
        {

            entityPOCO.Id = entityPM.Id;
            entityPOCO.Active = entityPM.Active;
            entityPOCO.Tenant = entityPM.Tenant;
            entityPOCO.CooReason = entityPM.CooReason;
            entityPOCO.CooStatus = entityPM.CooStatus;
        }

        public void CustomPOCOToPM(CertificateOfOriginConnectionPM entityPM, CertificateOfOriginConnection entityPOCO)
        {
            entityPM.Id = entityPOCO.Id;
            entityPM.Active = entityPOCO.Active;
            entityPM.Tenant = entityPOCO.Tenant;
            entityPM.CooReason = entityPOCO.CooReason;
            entityPM.CooStatus = entityPOCO.CooStatus;
        }
   }


}
   