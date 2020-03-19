using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.Interfaces;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class TaxDeductionReportUpdateService
    {


        protected override void OnCreating(TaxDeductionReportPM entityPM, EntityPM entityParentPM)
        {
            entityPM.CreateDate = DateTime.Now;

            entityPM.UpdateDate = DateTime.Now;
            entityPM.UpdatedByUserId = AuthenticationUtil.ResolveUserId(entityPM.Tenant);
            entityPM.StatusTypeCode = "1";
            entityPM.CreatedByUserId = AuthenticationUtil.ResolveUserId(entityPM.Tenant);
            entityPM.ReportNumber = CodeCounter.GetNumber("TaxDeductionReport", entityPM.Tenant).ToString();

            FullAccountingSettingQueryService fullAccountingSettingQueryService = new FullAccountingSettingQueryService(entityPM.Tenant);
            FullAccountingSettingPM setting = fullAccountingSettingQueryService.GetSingleFullAccountingSetting(entityPM.Tenant);

            if(setting != null && setting.DeductionFileNumber == null)
            {
                ContactPM contact = GetLoggedContact(entityPM.Tenant) ?? new ContactPM();
                bool showLocals = !contact.DontShowLocal;
                throw new Exception(TranslateTextsClass.Translate("Accounting.O.DeductionFileNumberNotFound", entityPM.Tenant, showLocals));


            }
            Validate(entityPM);
        }



        protected override void Trace(TaxDeductionReportPM entityPM, TaxDeductionReport entityPOCO, string changesXml)
        {
           ContactPM loggedContact = GetLoggedContact(entityPM.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                //create trace event with created type.
                EventTracerArgs eventTracerArgs = new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = loggedContact.Id,
                    ObjectTableName = "TaxDeductionReport",
                    IsAddedManually = false,
                    EventTypeCode = "CREV",
                    Notes = "",
                };
                EventTracer.CreateTraceEvent(eventTracerArgs);
            }
            else
            {

                EventTracerArgs eventTracerArgs = new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = loggedContact.Id,
                    ObjectTableName = "TaxDeductionReport",
                    IsAddedManually = false,
                    EventTypeCode = "UPEV",
                    Notes = "",
                };
                EventTracer.CreateTraceEvent(eventTracerArgs);

            }

            base.Trace(entityPM, entityPOCO, changesXml);
        }


        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }


        public static ContactPM GetLoggedContact(int tenant)
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


        protected override void AfterUpdating(TaxDeductionReportPM entityPM, EntityPM entityParentPM)
        {
            base.AfterUpdating(entityPM, entityParentPM);



            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPM.StatusTypeCode = "2";
                entityPM.ChangeSetOp = ChangeSetOperation.Update;
                this.Update(entityPM, true);

                TaxDeductionReportService.Create856FileInBatch(entityPM.Id, entityPM.Tenant);
           
            }

        }
    }

}
