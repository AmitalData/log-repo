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
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;
using System.Xml.Serialization;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Server.Tools.QueueService;
using Logitude.Accounting.BL.CloseTables;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class TaxReportUpdateService
    {
        protected override void OnCreating(TaxReportPM entityPM, EntityPM entityParentPM)
        {

            entityPM.Id = IdCounter.GetNumber("TaxReport", entityPM.Tenant);
            entityPM.CreateDate = DateTime.Now;
            entityPM.CreatedByUserId = AuthenticationUtil.ResolveUserId(entityPM.Tenant);
            entityPM.TaxReportMonth = new DateTime(entityPM.TaxReportMonth.Year, entityPM.TaxReportMonth.Month, 1);
            DateTime date = entityPM.TaxReportMonth.AddMonths(1);
            FullAccountingSettingPM setting = GetFullAccountingSetting(entityPM.Tenant);
            entityPM.LastUpdateDate = new DateTime(date.Year, date.Month, 15);
            entityPM.UpdatedByUserId = AuthenticationUtil.ResolveUserId(entityPM.Tenant);
            entityPM.UpdatedByUserName = GetLoggedContact(entityPM.Tenant).LocalName != null ? GetLoggedContact(entityPM.Tenant).LocalName : GetLoggedContact(entityPM.Tenant).EnglishName ;
            TenantQuery tenantQuery = new TenantQuery(entityPM.Tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(entityPM.Tenant);
            entityPM.VatNumber = setting.ConsolidationVAT != null ? setting.ConsolidationVAT : tenantPM.VatNumber;
            entityPM.TaxableOutputsWithDiffPercent = 0;
            entityPM.NeedsRebulid = true;
            entityPM.StatusCode = "P";
            entityPM.ProcessStartDate = DateTime.Now;
            entityPM.TaxReportNumber = entityPM.TaxReportMonth.Month.ToString() + entityPM.Year.ToString();
            entityPM.IsNew = true;
            entityPM.CreatedInTwoMonthsLogic = setting.VATreportEveryTwoMonths;
            Validate(entityPM);
        }
        private FullAccountingSettingPM GetFullAccountingSetting(int tenant)
        {
            FullAccountingSettingQueryService settingQueryService = new FullAccountingSettingQueryService(tenant);
            return settingQueryService.GetSingleFullAccountingSetting(tenant);
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
                int count = repo.ReportCount(entityPM.Tenant);
                FullAccountingSettingPM fullAccountingSettingPM = GetFullAccountingSetting(entityPM.Tenant);

                if (count > 0)
                {
                    AccountingPeriodQueryService accountingPeriodQueryService = new AccountingPeriodQueryService(entityPM.Tenant);
                    DateTime taxReportDate = new DateTime(entityPM.Year, entityPM.TaxReportMonth.Month, 1);
                    bool exist = repo.CheckIfTaxReportExist(taxReportDate, entityPM.Tenant, fullAccountingSettingPM.VATreportEveryTwoMonths);
                    bool previousCompletedExist = repo.CheckIfPreviousReportExist(taxReportDate, entityPM.Tenant, fullAccountingSettingPM.VATreportEveryTwoMonths);
                    bool previousNotCompletedExist = repo.CheckIfPreviousNotCompReportExist(taxReportDate, entityPM.Tenant, fullAccountingSettingPM.VATreportEveryTwoMonths);

                    bool higherDateReportExist = repo.CheckIfTaxReportWithHigherDateExist(entityPM.TaxReportMonth.Month, entityPM.Year, entityPM.Tenant);

                    ContactPM contact = GetLoggedContact(entityPM.Tenant) ?? new ContactPM();

                    bool showLocals = !contact.DontShowLocal;

                    if (exist == true)
                    {
                        throw new ApplicationException(TranslateTextsClass.Translate("Accounting.O.ReportExist", entityPM.Tenant, showLocals));
                    }
                    if (higherDateReportExist == true)
                    {
                        throw new ApplicationException(TranslateTextsClass.Translate("Accounting.O.HigherMonthReport", entityPM.Tenant, showLocals));
                    }

                    if (previousNotCompletedExist)
                    {
                        throw new ApplicationException(TranslateTextsClass.Translate("Accounting.O.NotCompletedReportExist", entityPM.Tenant, showLocals));
                    }

                    if (!previousCompletedExist)
                    {
                        if (fullAccountingSettingPM.VATreportEveryTwoMonths)
                        {
                            throw new ApplicationException(TranslateTextsClass.Translate("Accounting.O.CompletedReportExistForPreviousTwoMonths", entityPM.Tenant, showLocals));
                        }
                        throw new ApplicationException(TranslateTextsClass.Translate("Accounting.O.CompletedReportExist", entityPM.Tenant, showLocals));
                    }
                }

            }

            base.Validate(entityPM);
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


        protected override void AfterUpdating(TaxReportPM entityPM, EntityPM entityParentPM)
        {
            //IAccountingContext accountingContext = AccountingContext.GetContext(entityPM.Tenant);
            //TaxReportLineListQueryService reportLineListQueryService = new TaxReportLineListQueryService(accountingContext);
            //TaxReportUpdateService taxReportUpdateService = new TaxReportUpdateService(accountingContext, new Dictionary<string, IContext>(), Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {

               //TaxReportService.CreateTaxReportFileInBatch(entityPM.Id, entityPM.Tenant);

            }
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                //List<TaxReportLinePM> lines = TaxReportService.CreateTaxReportLines(entityPM, entityPM.Tenant);
                //TaxReportService.CalculateReportTotals(entityPM, lines);
                //entityPM.ChangeSetOp = ChangeSetOperation.Update;
                //taxReportUpdateService.Update(entityPM, true);
            }

            UpdateReportStatus(entityPM);

        }

        private void UpdateReportStatus(TaxReportPM taxReportPM)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(taxReportPM.Tenant);
            TaxReportLineListQueryService reportLineListQueryService = new TaxReportLineListQueryService(accountingContext);
            TaxReportUpdateService taxReportUpdateService = new TaxReportUpdateService(accountingContext, new Dictionary<string, IContext>(), Tenant);

            List<TaxReportLineList> lines = reportLineListQueryService.GetReportLines(taxReportPM.Id, taxReportPM.Tenant).ToList();

            if (taxReportPM.StatusCode != VatReportStatusValues.Cancelled && taxReportPM.StatusCode != VatReportStatusValues.Transmitted)
            {
                bool hasErrors = lines.Any(d => d.StatusCode != "6"); // 6- Ready for transmit
                if (hasErrors && taxReportPM.StatusCode != VatReportStatusValues.Error)
                {
                    taxReportPM.StatusCode = VatReportStatusValues.Error;

                    taxReportPM.ChangeSetOp = ChangeSetOperation.Update;
                    taxReportUpdateService.Update(taxReportPM, true);
                }
                else if (!hasErrors && taxReportPM.StatusCode == VatReportStatusValues.Error)
                {
                    taxReportPM.ChangeSetOp = ChangeSetOperation.Update;
                    taxReportPM.StatusCode = VatReportStatusValues.Draft;
                    taxReportUpdateService.Update(taxReportPM, true);
                }
            }

        }

        protected override void OnUpdating(TaxReportPM entityPM, TaxReport entityPOCO)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(entityPM.Tenant);
            TaxReportLineListQueryService reportLineListQueryService = new TaxReportLineListQueryService(accountingContext);
            TaxReportUpdateService taxReportUpdateService = new TaxReportUpdateService(accountingContext, new Dictionary<string, IContext>(), Tenant);

            if (entityPOCO.IsCancelled == false && entityPM.IsCancelled == true)
            {
                // canceled!!C:\source\log-repo\Logitude\JustWebFreight\WebFreight.Web\obj\
                CancelTaxReport(entityPM);
                return;
            }
            if(entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {

                //updates
                if (!entityPM.IsNew)
                {
                    entityPM.LastUpdateDate = DateTime.Now;

                    // update totals
                    List<TaxReportLineList> lines = reportLineListQueryService.GetReportLines(entityPM.Id, entityPM.Tenant).ToList();
                    List<TaxReportLinePM> linesPM = new List<TaxReportLinePM>();
                    lines.ForEach(d =>
                    {
                        var item = new TaxReportLinePM()
                        {
                            Tenant = d.Tenant,
                            LastUpdateDateTime = d.LastUpdateDateTime,
                            UpdatedByUserId = d.UpdatedByUserId,
                            SearchFields = d.SearchFields,
                            TaxReportId = d.TaxReportId,
                            Line = d.Line,
                            OutputOrInput = d.OutputOrInput,
                            LineTypeCode = d.LineTypeCode,
                            VatNumber = d.VatNumber,
                            Reference = d.Reference,
                            ReferecneGroup = d.ReferecneGroup,
                            ReferenceDate = d.ReferenceDate,
                            VatAmount = d.VatAmount,
                            VatableInvoiceAmount = d.VatableInvoiceAmount,
                            StatusCode = d.StatusCode,
                            TransmitStatusCode = d.TransmitStatusCode,
                            JournalId = d.JournalId,
                            IsManuallyChanged = d.IsManuallyChanged,
                            IsEquipment = d.IsEquipment,
                            StatusLocalName = d.StatusLocalName,
                            StatusEnglishName = d.StatusEnglishName,
                            JournalNumber = d.JournalNumber,
                            TotalInvoiceAmount = d.TotalInvoiceAmount
                        };
                        linesPM.Add(item);
                    });
                    TaxReportService.CalculateReportTotals(entityPM, linesPM);
                   
                } 
               // entityPM.UpdatedByUserId = AuthenticationUtil.ResolveUserId(entityPM.Tenant);
                ContactPM loggedContact = GetLoggedContact(entityPM.Tenant);
                entityPM.UpdatedByUserName = loggedContact.LocalName != null ? loggedContact.LocalName : loggedContact.EnglishName;
            }

            base.OnUpdating(entityPM, entityPOCO);
        }

        private void CancelTaxReport(TaxReportPM entityPM)
        {
            CheckLaterReports(entityPM);

            entityPM.StatusCode = "C"; // C- Cancelled מבוטל

            ResetJournalAdditionalDatasFields(entityPM);

        }

        private void ResetJournalAdditionalDatasFields(TaxReportPM entityPM)
        {
            List<JournalAdditionalDataPM> journalAdditionalDataPMs = GetJournalAdditionalDataByTaxReportId(entityPM);
            UpdateJournalAdditionalDatas(journalAdditionalDataPMs);

        }
        private static List<JournalAdditionalDataPM> GetJournalAdditionalDataByTaxReportId(TaxReportPM taxReport)
        {

            JournalAdditionalDataQueryService additionalDataQueryService = new JournalAdditionalDataQueryService(taxReport.Tenant);
            return additionalDataQueryService.GetJournalAdditionalDataPMsByTaxReportId(taxReport.Id, taxReport.Tenant);
        }

        private static void UpdateJournalAdditionalDatas(List<JournalAdditionalDataPM> journalAdditionalDataPMs)
        {
            foreach (JournalAdditionalDataPM journalAdditionalData in journalAdditionalDataPMs)
            {
                journalAdditionalData.TaxReportTransmitStatusCode = null;
                journalAdditionalData.TaxReportId = null;
                journalAdditionalData.ChangeSetOp = ChangeSetOperation.Update;
                SaveChangesOnJournalAdditionalData(journalAdditionalData);
            }

        }
        private static void UpdateJournalJournalAdditionalData(TaxReportLinePM taxReportLine)
        {
            JournalAdditionalDataPM journalAdditionalDataPM = GetJournalAdditionalDataPM(taxReportLine);
            if (journalAdditionalDataPM != null)
            {
                journalAdditionalDataPM = MapJournalAdditionalDataPM(journalAdditionalDataPM, taxReportLine);
                SaveChangesOnJournalAdditionalData(journalAdditionalDataPM);

            }
        }

        private static void SaveChangesOnJournalAdditionalData(JournalAdditionalDataPM journalAdditionalDataPM)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(journalAdditionalDataPM.Tenant);
            JournalAdditionalDataUpdateService journalAdditionalDataUpdateService = new JournalAdditionalDataUpdateService(MyContext, new Dictionary<string, IContext>(), journalAdditionalDataPM.Tenant);
            journalAdditionalDataUpdateService.Update(journalAdditionalDataPM, true);
        }

        private static JournalAdditionalDataPM MapJournalAdditionalDataPM(JournalAdditionalDataPM journalAdditionalDataPM, TaxReportLinePM taxReportLine)
        {
            journalAdditionalDataPM.TaxReportTransmitStatusCode = null;
            journalAdditionalDataPM.TaxReportId = null;
            journalAdditionalDataPM.ChangeSetOp = ChangeSetOperation.Update;
            return journalAdditionalDataPM;
        }
        private static JournalAdditionalDataPM GetJournalAdditionalDataPM(TaxReportLinePM taxReportLine)
        {
            JournalAdditionalDataQueryService additionalDataQueryService = new JournalAdditionalDataQueryService(taxReportLine.Tenant);
            return additionalDataQueryService.GetSingle(taxReportLine.JournalId, taxReportLine.JournalLineNumber, false, false);
        }
  

        private void CheckLaterReports(TaxReportPM entityPM)
        {
            bool showLocal = LoggedContactResolver.GetLoggedContactShowLocal(entityPM.Tenant);
            

            TaxReportQueryService reportQuery = new TaxReportQueryService(entityPM.Tenant);
            List<TaxReport> futureReports = reportQuery.GetFutureActiveReports(entityPM.CreateDate, entityPM.Tenant);
            if (futureReports.Any())
                throw new ApplicationException(TextCodesTranslator.TranslateText("TaxReport.O.CancelLaterReports", entityPM.Tenant, showLocal));
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
