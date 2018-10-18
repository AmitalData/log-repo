
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
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.BL.Messaging.Customs;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Customs.BL.EntityDataMappings
{

    public partial class UIMessageDataMapping : IMapping<UIMessagePM, UIMessage>
    {

        public void CustomPMToPOCO(UIMessagePM entityPM, UIMessage entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Code);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Code = entityPM.Code;
            }
        }

        public void CustomPOCOToPM(UIMessagePM entityPM, UIMessage entityPOCO)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            if (authToken != null)
            {
                int tenant = authToken.Tenant;

                UIMessageAdditionalRepository additionalRep = new UIMessageAdditionalRepository(tenant);
                UIMessageAdditional additional = additionalRep.GetSingleAdditionalByCode(entityPM.Code, tenant);
                if (additional != null)
                {
                    entityPM.Tenant = tenant;
                    entityPM.Sort = additional.Sort;
                }
            }
        }
    }


}
   