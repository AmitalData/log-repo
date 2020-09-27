using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ReferantExceptionUpdateService
    {

        protected override void OnCreating(ReferantExceptionPM entityPM, EntityPM entityParentPM)
        {

            base.OnCreating(entityPM, entityParentPM);
        }


        protected override void OnUpdating(ReferantExceptionPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            if (entityPM != null)
            {
                ExceptionReasonQueryService exceptionReasonQueryService = new ExceptionReasonQueryService(entityPM.Tenant);
                ExceptionReasonPM exceptionReasonPm = exceptionReasonQueryService.GetSingle(entityPM.ExceptionReasonsCode, false, false);
                if (exceptionReasonPm.UnifreightStatusCode != null)
                {
                    if (entityPM.Status == "A")
                    {
                        UpdateUnifreight(entityPM, exceptionReasonPm);
                    }
                }
            }
            base.OnUpdating(entityPM);
        }


        protected override void AfterUpdating(ReferantExceptionPM entityPM, EntityPM entityParentPM)
        {
            ICustomContext context = MainContext as CustomContext;

            DeclarationReferantDataPM declarationReferantDataPM = new DeclarationReferantDataPM();
            DeclarationReferantDataQueryService declarationReferantDataQueryService = new  DeclarationReferantDataQueryService(entityPM.Tenant);
            DeclarationReferantDataUpdateService declarationReferantDataUpdateService = new DeclarationReferantDataUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);

            declarationReferantDataPM = declarationReferantDataQueryService.GetSingle(entityPM.DeclarationId, false, false);// 
            if(declarationReferantDataPM!= null)
            {
  List< ReferantExceptionPM> referantExceptionPMs = new List<ReferantExceptionPM>();
            ReferantExceptionQueryService referantExceptionQueryService = new ReferantExceptionQueryService(entityPM.Tenant);
            ReferantExceptionUpdateService referantExceptionUpdateService = new ReferantExceptionUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
            referantExceptionPMs = referantExceptionQueryService.GetByDecId(entityPM.DeclarationId);
            if (referantExceptionPMs!= null)
            {

                string exceptions = "";
                foreach (var item in referantExceptionPMs)
                {
                    if (item.Status == "A")
                        exceptions += item.ExceptionReasonsCode + ",";
                }

                declarationReferantDataPM.ExceptionReasonsList = exceptions;
                declarationReferantDataPM.ChangeSetOp = ChangeSetOperation.Update;
                declarationReferantDataUpdateService.Update(declarationReferantDataPM, true);
            }
            }
          
            base.AfterUpdating(entityPM, entityParentPM);

        }

    }
}
