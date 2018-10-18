
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
   
   public partial class ClientDrivingLicenseTypeDataMapping: IMapping<ClientDrivingLicenseTypePM, ClientDrivingLicenseType>
   {

        public void CustomPMToPOCO(ClientDrivingLicenseTypePM entityPM, ClientDrivingLicenseType entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.ClientId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.ClientDrivingLicenseLine);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DriversLicenseTypeCode);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.ClientId = entityPM.ClientId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.ClientDrivingLicenseLine = entityPM.ClientDrivingLicenseLine;
                entityPOCO.DriversLicenseTypeCode = entityPM.DriversLicenseTypeCode;
            }
        }

        public void CustomPOCOToPM(ClientDrivingLicenseTypePM entityPM, ClientDrivingLicenseType entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   