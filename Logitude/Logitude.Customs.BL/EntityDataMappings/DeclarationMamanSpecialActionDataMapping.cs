
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
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class DeclarationMamanSpecialActionDataMapping: IMapping<DeclarationMamanSpecialActionPM, DeclarationMamanSpecialAction>
   {

        public void CustomPMToPOCO(DeclarationMamanSpecialActionPM entityPM, DeclarationMamanSpecialAction entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(DeclarationMamanSpecialActionPM entityPM, DeclarationMamanSpecialAction entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.MamanSpecialActionName);
            CustomMappedPMProperties.Add(PMPropertyNames.MamanSpecialActionStatusName);

            if (entityPOCO.MamanSpecialActionCode != null)
            {
                MamanSpecialActionQueryService mamanSpecialActionQueryService = new MamanSpecialActionQueryService(entityPOCO.Tenant);
                MamanSpecialActionPM mamanSpecialActionPM = mamanSpecialActionQueryService.GetSingle(entityPOCO.MamanSpecialActionCode, false, true);
                entityPM.MamanSpecialActionName = mamanSpecialActionPM.LocalName;
            }

            if (entityPOCO.MamanSpecialActionStatusCode != null)
            {
                MamanSpecialActionStatusQueryService mamanSpecialActionStatusQueryService = new MamanSpecialActionStatusQueryService(entityPOCO.Tenant);
                MamanSpecialActionStatusPM mamanSpecialActionStatusPM = mamanSpecialActionStatusQueryService.GetSingle(entityPOCO.MamanSpecialActionStatusCode, false, true);
                entityPM.MamanSpecialActionStatusName = mamanSpecialActionStatusPM.LocalName;
            }
        }
   }


}
   