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
            entityPM.UpdatedByUserName = GetLoggedContact(entityPM.Tenant).LocalName != null ? GetLoggedContact(entityPM.Tenant).LocalName : GetLoggedContact(entityPM.Tenant).EnglishName;
            TenantQuery tenantQuery = new TenantQuery(entityPM.Tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(entityPM.Tenant);
            entityPM.VatNumber = setting.ConsolidationVAT != null ? setting.ConsolidationVAT : tenantPM.VatNumber;
            entityPM.TaxableOutputsWithDiffPercent = 0;
            entityPM.NeedsRebulid = true;
            entityPM.StatusCode = "P";


            DateTime stopLogAt = new DateTime(2023, 06, 01);
            string text = "TaxReportUpdateService.OnCreating(*1*): " + entityPM.Id + " entityPM.StatusCode : " + entityPM.StatusCode;
            ULog(text, stopLogAt);


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
                    bool isTaxReportExist = repo.CheckIfTaxReportExist(taxReportDate, entityPM.Tenant);
                    bool previousCompletedExist = repo.CheckIfPreviousReportExist(taxReportDate, entityPM.Tenant, fullAccountingSettingPM.VATreportEveryTwoMonths);
                    bool previousNotCompletedExist = repo.CheckIfPreviousNotCompReportExist(taxReportDate, entityPM.Tenant, fullAccountingSettingPM.VATreportEveryTwoMonths);

                    bool higherDateReportExist = repo.CheckIfTaxReportWithHigherDateExist(entityPM.TaxReportMonth.Month, entityPM.Year, entityPM.Tenant);

                    ContactPM contact = GetLoggedContact(entityPM.Tenant) ?? new ContactPM();

                    bool showLocals = !contact.DontShowLocal;

                    if (isTaxReportExist == true)
                    {
                        throw new ApplicationException(TranslateTextsClass.Translate("Accounting.O.ReportExist", entityPM.Tenant, showLocals));
                    }

                    if (fullAccountingSettingPM.VATreportEveryTwoMonths)
                    {
                        bool isTaxReportExistForPreviousMonth = repo.CheckIfTaxReportExistForPreviousMonth(taxReportDate, entityPM.Tenant);
                        if (isTaxReportExistForPreviousMonth)
                        {
                            throw new ApplicationException(TranslateTextsClass.Translate("Accounting.O.ReportExistForPreviousMonth", entityPM.Tenant, showLocals));
                        }
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




        private static List<TaxReportLinePM> GetTaxReportLines(string taxReportId, int tenant)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            TaxReportQueryService taxReportQuery = new TaxReportQueryService(accountingContext);
            var taxReportLinesPM = taxReportQuery.GetReportLinesPMs(taxReportId, tenant);
            return taxReportLinesPM;
        }


        public void MarkDuplicateLines(TaxReportPM taxReportPM)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(taxReportPM.Tenant);
            TaxReportQueryService taxReportQuery = new TaxReportQueryService(accountingContext);
            TaxReportLineQueryService taxReportLineQuery = new TaxReportLineQueryService(accountingContext);

            List<int> notToSendKeyList = new List<int>();
            List<int> duplicateKeyList = new List<int>();
            List<DuplicateRows> duplicateRows = taxReportQuery.GetDuplicateRows(taxReportPM.Id, taxReportPM.Tenant);            

            duplicateRows.GroupBy(x => x.VatNumber + "_" + x.Reference)
                .ToList().ForEach((group) =>
            {
                DuplicateRows firstRow = group.First();

                bool notToSend = (firstRow.AccountingEntityCode == "1" || firstRow.AccountingEntityCode == "4") &&
                    group.Any(x => x.IsVoided.HasValue && x.IsVoided.Value) &&
                    group.Any(x => !x.IsVoided.HasValue || !x.IsVoided.Value) &&
                    group.All(x => x.AccountingEntityCode == firstRow.AccountingEntityCode &&
                        x.AccountingEntityId == firstRow.AccountingEntityId &&
                        x.ReferenceDate.HasValue && firstRow.ReferenceDate.HasValue &&
                        x.ReferenceDate.Value.Month == firstRow.ReferenceDate.Value.Month &&
                        x.ReferenceDate.Value.Year == firstRow.ReferenceDate.Value.Year);

                List<int> lineList = group.Select(x => x.Line).ToList();

                if (notToSend)
                    notToSendKeyList.AddRange(lineList);
                else 
                    duplicateKeyList.AddRange(lineList);
            });

            var taxReportLines = taxReportQuery.GetReportLines(taxReportPM.Id, taxReportPM.Tenant).ToList().Select(x => taxReportLineQuery.GetEntityPM(x,true)).ToList();
            List<TaxReportLinePM> updateList = new List<TaxReportLinePM>();
            taxReportLines.ForEach(row =>
            {
                bool isUpdate = false;
                
                if (notToSendKeyList.Contains(row.Line))
                {                    
                    isUpdate = true;
                    row.TransmitStatusCode = TaxReportLineTransmitStatusValues.Notfortransmitforthisreport;
                }
                else if (duplicateKeyList.Contains(row.Line))
                {
                    isUpdate = true;
                    row.StatusCode = TaxReportLineStatusValues.DuplicateThereisanothertransactionwiththesameVATNoandReference;
                }
                else if (row.StatusCode == TaxReportLineStatusValues.DuplicateThereisanothertransactionwiththesameVATNoandReference)
                {
                    isUpdate = true;
                    row.StatusCode = TaxReportLineStatusValues.Readyfortransmit;
                }

                if (isUpdate)
                {
                    row.ChangeSetOp = ChangeSetOperation.Update;
                    if (row.IsManuallyChanged == false) row.IsManuallyChanged = null;
                    updateList.Add(row);
                }
            });

            new TaxReportLineUpdateService(accountingContext, new Dictionary<string, IContext>(), taxReportPM.Tenant)
                .UpdateMulti(updateList, new List<TaxReportLinePM>(), taxReportPM, true);
        }
        private class DupLines
        {
            public string VatNumber { get; set; }
            public string Reference { get; set; }
            public IEnumerable<int> LineNumbers { get; set; }

        }
        private void UpdateReportStatus(TaxReportPM taxReportPM)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(taxReportPM.Tenant);
            TaxReportLineListQueryService reportLineListQueryService = new TaxReportLineListQueryService(accountingContext);
            TaxReportUpdateService taxReportUpdateService = new TaxReportUpdateService(accountingContext, new Dictionary<string, IContext>(), Tenant);

            List<TaxReportLineList> lines = reportLineListQueryService.GetReportLines(taxReportPM.Id, taxReportPM.Tenant).ToList();

            if (taxReportPM.StatusCode != VatReportStatusValues.TransmittedAndClosingJournal &&  taxReportPM.StatusCode != VatReportStatusValues.Cancelled && taxReportPM.StatusCode != VatReportStatusValues.Transmitted && taxReportPM.StatusCode != VatReportStatusValues.CancelationInProgress && taxReportPM.StatusCode != VatReportStatusValues.CancelationFailed)
            {
                bool hasErrors = lines.Any(d => d.StatusCode != "6"); // 6- Ready for transmit
                if (hasErrors && taxReportPM.StatusCode != VatReportStatusValues.Error)
                {
                    taxReportPM.StatusCode = VatReportStatusValues.Error;

                    DateTime stopLogAt = new DateTime(2023, 06, 01);
                    string text = "TaxReportUpdateService.UpdateReportStatus(*1*): " + taxReportPM.Id + " taxReportPM.StatusCode : " + taxReportPM.StatusCode;
                    ULog(text, stopLogAt);


                    taxReportPM.ChangeSetOp = ChangeSetOperation.Update;
                    taxReportUpdateService.Update(taxReportPM, true);
                }
                else if (!hasErrors && taxReportPM.StatusCode == VatReportStatusValues.Error)
                {
                    taxReportPM.ChangeSetOp = ChangeSetOperation.Update;
                    taxReportPM.StatusCode = VatReportStatusValues.Draft;

                    DateTime stopLogAt = new DateTime(2023, 06, 01);
                    string text = "TaxReportUpdateService.UpdateReportStatus(*2*): " + taxReportPM.Id + " taxReportPM.StatusCode : " + taxReportPM.StatusCode;
                    ULog(text, stopLogAt);

                    taxReportUpdateService.Update(taxReportPM, true);
                }
            }

        }


        private static void ULog(string text, DateTime stopLogAt)
        {
            string log_text = text + System.Environment.NewLine;
            log_text = log_text + String.Format("{0:HH:mm:ss.ffff}", DateTime.Now.ToString()) + System.Environment.NewLine;
            System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
            log_text = log_text + t.ToString();
            LogitudeSettings.HandleLogMe(log_text, false, "TaxReportPMToPOCO", new DateTime(2023, 6, 1));
        }

        protected override void OnUpdating(TaxReportPM entityPM, TaxReport entityPOCO)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(entityPM.Tenant);
            TaxReportLineListQueryService reportLineListQueryService = new TaxReportLineListQueryService(accountingContext);
            TaxReportUpdateService taxReportUpdateService = new TaxReportUpdateService(accountingContext, new Dictionary<string, IContext>(), Tenant);
            ///***
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
                    MarkDuplicateLines(entityPM); 
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
                            TotalInvoiceAmount = d.TotalInvoiceAmount,
                            VatAmountRound = d.VatAmountRound,
                            SubTotalInLocalCurrency=d.SubTotalInLocalCurrency,
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

            DateTime stopLogAt = new DateTime(2023, 06, 01);
            string text = "TaxReportUpdateService.CancelTaxReport(*1*): " + entityPM.Id + " entityPM.StatusCode : " + entityPM.StatusCode;
            ULog(text, stopLogAt);

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
            VatReportStatusQueryService queryService = new VatReportStatusQueryService(entityPOCO.Tenant);
            ContactPM loggedContact = GetLoggedContact(entityPM.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                //create trace event with created type.
                EventTracerArgs eventTracerArgs = new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = entityPM.CreatedByUserId,
                    ObjectTableName = "TaxReport",
                    IsAddedManually = false,
                    EventTypeCode = "CREV",
                    Notes = "",
                };
                EventTracer.CreateTraceEvent(eventTracerArgs);
            }
            if (
                 (entityPM.StatusCode == VatReportStatusValues.CancelationInProgress && entityPOCO.StatusCode == VatReportStatusValues.Draft) ||
                 (entityPM.StatusCode == VatReportStatusValues.CancelationInProgress && entityPOCO.StatusCode == VatReportStatusValues.Error) ||
                 (entityPM.StatusCode == VatReportStatusValues.CancelationInProgress && entityPOCO.StatusCode == VatReportStatusValues.Transmitted) ||
                 (entityPM.StatusCode == VatReportStatusValues.Cancelled && entityPOCO.StatusCode == VatReportStatusValues.TransmittedAndClosingJournal) ||
                 (entityPM.StatusCode == VatReportStatusValues.TransmittedAndClosingJournal && entityPOCO.StatusCode == VatReportStatusValues.Transmitted) ||
                 (entityPM.StatusCode == VatReportStatusValues.Draft && entityPOCO.StatusCode == VatReportStatusValues.Transmitted) ||
                 (entityPM.StatusCode == VatReportStatusValues.Transmitted && entityPOCO.StatusCode == VatReportStatusValues.Draft) ||
                 (entityPM.StatusCode == VatReportStatusValues.CancelationFailed && entityPOCO.StatusCode == VatReportStatusValues.CancelationInProgress) ||
                 (entityPM.StatusCode == VatReportStatusValues.Cancelled && entityPOCO.StatusCode == VatReportStatusValues.CancelationFailed) ||
                 (entityPM.StatusCode == VatReportStatusValues.Error && entityPOCO.StatusCode == VatReportStatusValues.Draft) ||
                 (entityPM.StatusCode == VatReportStatusValues.Draft && entityPOCO.StatusCode == VatReportStatusValues.Error) ||
                 (entityPM.StatusCode == VatReportStatusValues.Error && entityPOCO.StatusCode == VatReportStatusValues.InProgress) ||
                 (entityPM.StatusCode == VatReportStatusValues.Draft && entityPOCO.StatusCode == VatReportStatusValues.InProgress)
                )
            {

                VatReportStatusPM oldStatus = queryService.GetSingle(entityPOCO.StatusCode, false, false);
                var OldStatusEnglishName = oldStatus.EnglishName;
                VatReportStatusPM newStatus = queryService.GetSingle(entityPM.StatusCode, false, false);
                var NewStatusEnglishName = newStatus.EnglishName;


                string notes = TranslateTextsClass.Translate("TaxReportStatus", entityPOCO.Tenant) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPOCO.Tenant) + OldStatusEnglishName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPOCO.Tenant) + NewStatusEnglishName;
                EventTracerArgs eventTracerArgs = new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = entityPM.UpdatedByUserId,
                    ObjectTableName = "TaxReport",
                    IsAddedManually = false,
                    EventTypeCode = "UPEV",
                    Notes = notes,
                };
                EventTracer.CreateTraceEvent(eventTracerArgs);
            }
           
                if (entityPM.IsCancelled != entityPOCO.IsCancelled)
                {
                    VatReportStatusPM oldStatus = queryService.GetSingle(entityPOCO.StatusCode, false, false);
                    var OldStatusEnglishName = oldStatus.EnglishName;
                    VatReportStatusPM newStatus = queryService.GetSingle(entityPM.StatusCode, false, false);
                    var NewStatusEnglishName = newStatus.EnglishName;

                    string notes = TranslateTextsClass.Translate("TaxReportStatus", entityPOCO.Tenant) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPOCO.Tenant) + OldStatusEnglishName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPOCO.Tenant) + NewStatusEnglishName;
                    //create trace event with created type.
                    EventTracerArgs eventTracerArgs = new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                       Tenant = entityPM.Tenant,
                        UserId = entityPM.UpdatedByUserId,
                        ObjectTableName = "TaxReport",
                        IsAddedManually = false,
                        EventTypeCode = "CNCL",
                        Notes = notes,
                    };
                   EventTracer.CreateTraceEvent(eventTracerArgs);
                    CreateTraceEventWhenTransmittedReportReturnToDraft(entityPM, loggedContact);
                }

            base.Trace(entityPM, entityPOCO, changesXml);
        }
        private void CreateTraceEventWhenTransmittedReportReturnToDraft(TaxReportPM entityPM, ContactPM loggedContact)
        {
            if(EntityPOCO.StatusCode == VatReportStatusValues.Transmitted && entityPM.StatusCode == VatReportStatusValues.Draft)
            {
                EventTracerArgs eventTracerArgs = new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = entityPM.UpdatedByUserId,
                    ObjectTableName = "TaxReport",
                    IsAddedManually = false,
                    EventTypeCode = "RTDR",
                    Notes = "",
                };
                EventTracer.CreateTraceEvent(eventTracerArgs);
            }
        }
    }
}
