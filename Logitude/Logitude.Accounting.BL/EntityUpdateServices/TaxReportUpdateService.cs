using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.Data.EntityListQueryServices;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class TaxReportUpdateService
    {
        protected override void OnCreating(TaxReportPM entityPM, EntityPM entityParentPM)
        {

            entityPM.Id = IdCounter.GetNumber("TaxReport", entityPM.Tenant);
            entityPM.CreateDate = DateTime.Now;
            entityPM.CreatedByUserId = AuthenticationUtil.ResolveUserId(entityPM.Tenant);
            DateTime date = entityPM.TaxReportMonth.AddMonths(1);
          
        
            entityPM.LastUpdateDate = new DateTime(date.Year, date.Month, 15);
            entityPM.UpdatedByUserId = AuthenticationUtil.ResolveUserId(entityPM.Tenant);
            TenantQuery tenantQuery = new TenantQuery(entityPM.Tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(entityPM.Tenant);
            entityPM.VatNumber = tenantPM.VatNumber;
            entityPM.TaxableOutputsWithDiffPercent = 0;
            entityPM.NeedsRebulid = true;
            entityPM.StatusCode = "P";
            entityPM.ProcessStartDate = DateTime.Now;
            entityPM.TaxReportNumber = entityPM.TaxReportMonth.Month.ToString() + entityPM.Year.ToString();
            entityPM.IsNew = true;
            Validate(entityPM);
        }

        //protected override void UpdateComposition(TaxReportPM entityPM)
        //{
           
        //    var taxReportLineUpdateService = new TaxReportLineUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
        //    taxReportLineUpdateService.UpdateMulti(entityPM.TaxReportLines, entityPM.DeletedTaxReportLines, entityPM, true);


        //    base.UpdateComposition(entityPM);
        //}
        protected override void Validate(TaxReportPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                TaxReportRepository repo = new TaxReportRepository(entityPM.Tenant);
                AccountingPeriodQueryService accountingPeriodQueryService = new AccountingPeriodQueryService(entityPM.Tenant);
                bool exist = repo.CheckIfTaxReportExist(entityPM.TaxReportMonth.Month, entityPM.Year, entityPM.Tenant);
                bool higherDateReportExist = repo.CheckIfTaxReportWithHigherDateExist(entityPM.TaxReportMonth.Month, entityPM.Year, entityPM.Tenant);
                List<AccountingPeriodList> accountingPeriods = accountingPeriodQueryService.GetAccountingPeriodListByYearAndType(entityPM.Year, "1", entityPM.Tenant);
                ContactPM contact = GetLoggedContact(entityPM.Tenant) ?? new ContactPM();
                var openMonth = accountingPeriods.Where(d => (d.ClosedMonth < entityPM.TaxReportMonth.Month && d.OpenMonth > entityPM.TaxReportMonth.Month) || d.OpenMonth == entityPM.TaxReportMonth.Month ).Any();

                bool showLocals = !contact.DontShowLocal;
                if (exist == true)
                {
                    throw new ApplicationException(TranslateTextsClass.Translate("Accounting.O.ReportExist", entityPM.Tenant, showLocals));
                }
                if (higherDateReportExist == true)
                {
                    throw new ApplicationException(TranslateTextsClass.Translate("Accounting.O.HigherMonthReport", entityPM.Tenant, showLocals));
                }

                if (openMonth)
                {

                    throw new ApplicationException(TranslateTextsClass.Translate("Accounting.O.ReportWithClosedMonth", entityPM.Tenant, showLocals));
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
            ContactPM loggedContact = new ContactQuery(tenant).GetContactByEmailOnly(
                //SecurityUtility.GetAuthenticatedUser()
                AuthenticationUtil.ResolveUserIdentityName(tenant)
                , tenant);
            if (loggedContact == null)
            {
                loggedContact = new ContactQuery(tenant).GetContactByEmailOnly("system@tenant" + tenant + ".com", tenant);
            }
            loggedContact = loggedContact ?? new Logitude.BL.CommonDataModel.EntityPMs.ContactPM() { DontShowLocal = true };
            return loggedContact;
        }

        protected override void AfterUpdating(TaxReportPM entityPM, EntityPM entityParentPM) {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                TaxReportService.CreateTaxReportLines(entityPM, entityPM.Tenant);
            }
        }

        protected override void OnUpdating(TaxReportPM entityPM, TaxReport entityPOCO)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(entityPM.Tenant);
            TaxReportLineListQueryService reportLineListQueryService = new TaxReportLineListQueryService(accountingContext);

            if (entityPOCO.IsCancelled == false && entityPM.IsCancelled == true)
            {
                // canceled!!
                entityPM.StatusCode = "C"; // C- Cancelled מבוטל
            }

            if(entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                // recalculate totals
                List<TaxReportLineList> lines = reportLineListQueryService.GetReportLines(entityPM.Id, entityPOCO.Tenant);


                entityPM.TaxableOutputAmount = lines.Where(d => d.OutputOrInput == "O" && d.VatAmount != 0).Sum(d => d.VatableInvoiceAmount);
                entityPM.OutputTaxAmount = lines.Where(d => d.OutputOrInput == "O" && d.VatAmount != 0).Sum(d => d.VatAmount);
                entityPM.ExemptTaxableOutput = lines.Where(d => d.OutputOrInput == "O" && d.VatAmount != 0 && d.StatusCode == "6").Sum(d => d.VatableInvoiceAmount);
                entityPM.OutputLinesCount = lines.Where(d => d.OutputOrInput == "O").Count();
                entityPM.OtherInputsTaxAmount = lines.Where(d => d.OutputOrInput == "I" && d.StatusCode == "6" && d.IsEquipment == false).Sum(d => d.VatAmount);
                entityPM.InputLinesCount = lines.Where(d => d.OutputOrInput == "I").Count();
                entityPM.EquipmentInputsTaxAmount = lines.Where(d => d.OutputOrInput == "I" && d.StatusCode == "6" && d.IsEquipment == true).Sum(d => d.VatAmount);

                //updates
                if (!entityPM.IsNew)
                {
                    entityPM.LastUpdateDate = DateTime.Now;
                }
                entityPM.UpdatedByUserId = AuthenticationUtil.ResolveUserId(entityPM.Tenant);

            }


            base.OnUpdating(entityPM, entityPOCO);
        }

        protected override void Trace(TaxReportPM entityPM, TaxReport entityPOCO, string changesXml)
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
                    ObjectTableName = "TaxReport",
                    IsAddedManually = false,
                    EventTypeCode = "CREV",
                    Notes = "",
                };
                EventTracer.CreateTraceEvent(eventTracerArgs);
            }
            else
            {
                if (entityPM.IsCancelled != entityPOCO.IsCancelled)
                {
                    //create trace event with created type.
                    EventTracerArgs eventTracerArgs = new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = loggedContact.Id,
                        ObjectTableName = "TaxReport",
                        IsAddedManually = false,
                        EventTypeCode = "CNCL",
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
                        ObjectTableName = "TaxReport",
                        IsAddedManually = false,
                        EventTypeCode = "APRV",
                        Notes = "",
                    };
                    EventTracer.CreateTraceEvent(eventTracerArgs);
                }
            }

            base.Trace(entityPM, entityPOCO, changesXml);
        }
    }
}
