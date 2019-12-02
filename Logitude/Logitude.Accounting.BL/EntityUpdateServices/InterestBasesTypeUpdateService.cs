using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class InterestBasesTypeUpdateService
    {
        protected override void OnCreating(InterestBasesTypePM entityPM, EntityPM entityParentPM)
        {

   
        }

        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        public static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }

        protected override void OnUpdating(InterestBasesTypePM entityPM, InterestBasesType entityPOCO)
        {
            ContactPM contact = GetLoggedContact(entityPM.Tenant);
            bool showLocals = !contact.DontShowLocal;
            if (entityPM.Code != entityPOCO.Code)
            {
                InterestBasesTypeRepository PeriodRepository = new InterestBasesTypeRepository(entityPM.Tenant);
                InterestBasesType Period = PeriodRepository.GetSingleByCode(entityPM.Code, entityPM.Tenant);
                if (Period != null)
                {
                    throw new Exception(TextCodesTranslator.TranslateText("Accounting.General.O.Abasetypewiththesamecodeexists", entityPM.Tenant, showLocals));
                }
            }
        }

        protected override void UpdateComposition(InterestBasesTypePM entityPM)
        {
            InterestBasesPeriodUpdateService mementoLineUpdateService = new InterestBasesPeriodUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            mementoLineUpdateService.UpdateMulti(entityPM.InterestBasesPeriods, entityPM.DeletedInterestBasesPeriods, entityPM, false);
        }
    }
}
