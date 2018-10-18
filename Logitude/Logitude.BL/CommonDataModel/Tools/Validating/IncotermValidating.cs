
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class IncotermValidating
    {
        public static void Validate(EntityPMs.IncotermPM entityPM, bool isNew)
        {
            if (isNew)
            {
                var objectContext = CommonDataContext.GetContext(entityPM.Tenant);
                var incotermRepository = new IncotermRepository(objectContext);

                bool exist = (from a in incotermRepository.GetIncoterms(entityPM.Tenant)
                              where a.Code == entityPM.Code && a.Tenant == entityPM.Tenant
                              select a).Any();

                if (exist)
                {
                    string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", entityPM.Tenant);
                    msg = msg.Replace("%Entity", "Incoterm");
                    throw new Exception(msg);
                }
            }
        }
    }
}