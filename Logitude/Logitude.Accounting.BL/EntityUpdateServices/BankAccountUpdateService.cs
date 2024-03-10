using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityPOCOs;
//using Logitude.BL.CommonDataModel.EntityPMs;
//using Logitude.BL.CommonDataModel.EntityQueries;
//using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class BankAccountUpdateService : EntityUpdateService<BankAccount, BankAccountPM, EntityPM>
    {
        protected override void OnCreating(BankAccountPM entityPM, EntityPM entityParentPM)
        {
            BankAccountOnCreatingService bankAccountOnCreatingService = new BankAccountOnCreatingService(MainContext as IAccountingContext);
            bankAccountOnCreatingService.OnCreating(entityPM);
        }

        protected override void OnUpdating(BankAccountPM entityPM, BankAccount entityPOCO)
        {
            BankAccountOnUpdatingService bankAccountOnUpdatingUpdateService = new BankAccountOnUpdatingService(MainContext as IAccountingContext);
            bankAccountOnUpdatingUpdateService.OnUpdating(entityPM, entityPOCO);
            base.OnUpdating(entityPM, entityPOCO);
        }

        protected override void Trace(BankAccountPM entityPM, BankAccount entityPOCO, string changesXml)
        {
            BankAccountTraceEventService traceEventService = new BankAccountTraceEventService(MainContext as IAccountingContext);
            if (!entityPM.IsBankPageEvent && entityPOCO.ChequeCounter + 1 != entityPM.ChequeCounter)
            {
                traceEventService.Trace(entityPM, entityPOCO, changesXml);
            }
            List<TraceEventResponse> responses = traceEventService.TraceEventResponses;//for later user.
            traceEventService.InsertTraceEvents();
            base.Trace(entityPM, entityPOCO, changesXml);
        }
        protected override void UpdateComposition(BankAccountPM entityPM)
        {
            ChequeCounterSerialUpdateService chequeCounterSerialUpdateService = new ChequeCounterSerialUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            chequeCounterSerialUpdateService.UpdateMulti(entityPM.ChequeCounterSerials, entityPM.DeletedChequeCounterSerials, entityPM, false);
            base.UpdateComposition(entityPM);
        }
        protected override void Validate(BankAccountPM entityPM)
        {
            BankAccountValidateService validateService = new BankAccountValidateService(MainContext as IAccountingContext);
            validateService.Validate(entityPM);
            if (validateService.ErrorsList.Count > 0)
            {
                foreach(string error in validateService.ErrorsList)
                {
                    this.ErrorsList.Add(error);
                }
            }
            base.Validate(entityPM);
        }

        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }

        private static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }

            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


        private static string GetOldNewEventNote(string fieldName, string oldValue, string newValue)
        {
            string oldLabel = TranslateTextsClass.Translate("Accounting.General.O.OldValue",0);
            string newLabel = TranslateTextsClass.Translate("Accounting.General.O.NewValue",0);
            string note = string.Format("{0} {1} {2} {3} {4}{5}",fieldName, oldLabel,oldValue??"", newLabel,newValue ?? "",Environment.NewLine);
            return note;
        }
    }

}
