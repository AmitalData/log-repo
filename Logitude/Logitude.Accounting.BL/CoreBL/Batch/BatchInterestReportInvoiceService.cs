using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.CoreBL.InterestReport;
using Logitude.Accounting.BL.CoreBL.Mapping;
using Logitude.Accounting.BL.CoreBL.ReverseEngineer;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.InterestService;
using Logitude.Accounting.BL.InterestService.HelperClasses;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.Resolvers;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL.Batch
{
    public class BatchInterestReportInvoiceService : BatchTaskExecutionsService
    {
        private InterestReportQueryService interestReportQueryService;
        private UserPM userPM;
        private BatchTaskExecutionPM BatchTaskExecution;
        private string InterestReportObjectTableId;
        private string ARInvoiceObjectTableId;
        private int Tenant; 
        public BatchInterestReportInvoiceService(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
            BatchTaskExecution = batchTaskExecution;

        }

        public override void RunCode()
        {
            try
            {
                InterestReportArguments interestReportArgs = GetInterestReportArgs();
                this.Tenant = interestReportArgs.Tenant;
                interestReportQueryService = new InterestReportQueryService(interestReportArgs.Tenant);
                UserQuery userQuery = new UserQuery(interestReportArgs.Tenant);
                userPM = userQuery.GetSinglePMByEmail(interestReportArgs.Email, interestReportArgs.Tenant);
                if (userPM == null)
                {
                    userPM = userQuery.GetSinglePMByEmail(interestReportArgs.Email, 0);
                }
                CreateInvoicesForInterestReports(interestReportArgs);
            }
            catch (Exception e)
            {
                throw new Exception("\n" + e.Message);
            }
        }


        private void CreateInvoicesForInterestReports(InterestReportArguments interestReportArguments)
        {
            InterestReportArgs args = new InterestReportArgs();
            args.Tenant = interestReportArguments.Tenant;
            args.Email = interestReportArguments.Email;
            args.InvoiceDate = interestReportArguments.InvoiceDate;
            args.CloseWithoutInvoice = interestReportArguments.CloseWithoutInvoice;
            if (interestReportArguments.AllSelected)
            {
                List<InterestReportPM> interestReports = interestReportQueryService.GetNotInvoicedInterestReportsByDates(interestReportArguments.FromDate, interestReportArguments.ToDate, interestReportArguments.Tenant, interestReportArguments.ExcludedIds == null ? new List<string>() : interestReportArguments.ExcludedIds);
                List<InterestReportPM> interestReportsFillteredByCategory = interestReportQueryService.GetNotInvoicedInterestReportsByCategory(interestReports, interestReportArguments);
                List<InterestReportLinesByDatePM> LinesByDatesForSelectedReports = interestReportQueryService.GetFirstAndLastInterestReportLineByDatesForInterestReports(interestReportsFillteredByCategory.Select(s=>s.Id).ToList()).ToList();
             
                foreach (InterestReportPM report in interestReportsFillteredByCategory)
                {

                    report.InterestReportLinesByDates = LinesByDatesForSelectedReports.Where(s=>s.InterestReportId == report.Id).ToList();
                    CreateInvoiceForReportPM(args, interestReportArguments, report);

                }
            }
            else
            {
                List<InterestReportLinesByDatePM> LinesByDatesForSelectedReports = interestReportQueryService.GetFirstAndLastInterestReportLineByDatesForInterestReports(interestReportArguments.SelectedIds).ToList();

                foreach (string ReportId in interestReportArguments.SelectedIds)
                {

                    InterestReportPM  interestReport = interestReportQueryService.GetSingle(ReportId, false,true);
                    interestReport.InterestReportLinesByDates = LinesByDatesForSelectedReports.Where(s=>s.InterestReportId== ReportId).ToList();
                    CreateInvoiceForReportPM(args, interestReportArguments, interestReport);
                }
            }

        }

        private void CreateInvoiceForReportPM(InterestReportArgs args, InterestReportArguments interestReportArguments, InterestReportPM interestReport)
        {
            args.ReportNumber = interestReport.ReportNumber;
            args.InterestReportId = interestReport.Id;
            args.Tenant = interestReportArguments.Tenant;
            if (interestReport.InterestReportStatusCode == "1" || interestReport.InterestReportStatusCode == "9")
            {
                UpdateInterestReportsStatues(interestReport, interestReportArguments.Tenant, "8");
                UpdateAndCreateInvoiceForInterestReport(args, interestReport);
            }
        }
        private void UpdateAndCreateInvoiceForInterestReport(InterestReportArgs interestReportArgs, InterestReportPM interestReport)
        {
            try
            {
                List<InterestReportLinesByDatePM> LinesByDatesForSelectedReport = interestReport.InterestReportLinesByDates;
                interestReport = interestReportQueryService.GetSingle(interestReportArgs.InterestReportId, false, true);
                interestReport.InterestReportLinesByDates = LinesByDatesForSelectedReport;
                CreateInvoiceForInterestReport(interestReportArgs, interestReport);
            }

            catch (Exception e)
            {
                BatchTaskExecution.ErrorLog += "\n" + "Report # " + interestReport.ReportNumber + " " + e.Message;
                UpdateInterestReportsStatues(interestReport, interestReportArgs.Tenant, "9", null, e.Message);
            }


        }
        private InterestReportArguments GetInterestReportArgs()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(InterestReportArguments));
            InterestReportArguments interestReportArgs = serializer.Deserialize(stringReader) as InterestReportArguments;
            return interestReportArgs;
        }
        private void CreateInvoiceForInterestReport(InterestReportArgs interestReportArgs, InterestReportPM interestReport)
        {
            if (interestReport.TotalAmount == null || interestReport.TotalAmount <= interestReport.GLAccountMinimumInterest|| interestReportArgs.CloseWithoutInvoice)
            {
                IAccountingContext iAccountingContext = AccountingContext.GetContext(interestReportArgs.Tenant);
                InterestReportService interestReportService = new InterestReportService();
                interestReport = interestReportService.PutConfirmCreateInvoice(interestReport, interestReportArgs.Tenant, iAccountingContext);
            }
            else
            {
                ARInvoicePM aRInvoicePM = FullMapInvoice(interestReportArgs, interestReport);
                CheckVatNumber(interestReportArgs.Tenant, aRInvoicePM);
                IInvoiceContext invoiceContext = InvoiceContext.GetContext(interestReportArgs.Tenant);
                ARInvoiceService invoiceService = new ARInvoiceService(invoiceContext, interestReportArgs.Tenant, interestReportArgs.Email);
                invoiceService.Create(aRInvoicePM);
                BuildDocumentsForNewInvoice(aRInvoicePM, interestReport);
                if (aRInvoicePM.IsFromInterestBatchInvoice && IsFullAccountingActivated(interestReportArgs.Tenant))
                {
                    var DocumentsFilingId = getDocumentsFilingId(aRInvoicePM);
                    aRInvoicePM.DocumentFilingId = DocumentsFilingId;
                    invoiceService.Update(aRInvoicePM);
                }
                UpdateInterestReportsStatues(interestReport, interestReportArgs.Tenant, "2", aRInvoicePM);
            }
        }
        private bool IsFullAccountingActivated(int tenant)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            bool isFullAccountingActivated = tenantPOCO.AccountingActivated;
            return isFullAccountingActivated;
        }
        private string getDocumentsFilingId(ARInvoicePM aRInvoicePM)
        {
            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(aRInvoicePM.Tenant);
            DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(aRInvoicePM.Tenant);
            var documentTypeId = documentTypeQuery.GetDocumentTypeIdByCode("999G", aRInvoicePM.Tenant);
            string objectTableId = ObjectTableRepository.GetObjectTableByName("ARInvoice");
            DocumentsFilingPM myResult = documentsFilingQuery.GetDocumentsFilingPMByEntityIdAndObjectTableIdAndDocumentTypeId(aRInvoicePM.Id, objectTableId, documentTypeId, aRInvoicePM.Tenant);
            return myResult.Id;
        }
        private void BuildDocumentsForNewInvoice(ARInvoicePM aRInvoicePM , InterestReportPM interestReport)
        {
            string ARInvoiceChildEntityReference = !string.IsNullOrEmpty(aRInvoicePM.InvoiceNumber) ? aRInvoicePM.InvoiceNumber : "Draft: " + aRInvoicePM.DraftNumber;
            BuildDocument(aRInvoicePM.Id, "999G", ARInvoiceObjectTableId, ARInvoiceChildEntityReference);
            BuildDocument(interestReport.Id, "ITDT", InterestReportObjectTableId, interestReport.ReportNumber);
        }
        private void BuildDocument(string EntityId,string DocumentCode,string ObjecTableId,string ChildEntityReference)
        {
            DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery((int)Tenant);
            string documentTypeId = documentTypeQuery.GetDocumentTypeListIdByCodeAndTenant(DocumentCode, (int)Tenant);
            BuildDocsOutService buildDocsOutService = new BuildDocsOutService();
            BuildDocsOutArgs buildDocsOutArgs = new BuildDocsOutArgs()
            {
                EntityId = EntityId,
                Tenant = Tenant,
                ChildEntityId = null,
                LoggedUserId = userPM.Id,
                ChildEntityReference = ChildEntityReference,
                ChildObjectTableId = null,
                ObjectTableId = ObjecTableId,
                DocumentTypeId = documentTypeId,
            };

            buildDocsOutService.BuildDocsOut(buildDocsOutArgs);

        }
        private void CheckVatNumber(int Tenant, ARInvoicePM aRInvoicePM)
        {
            bool isFieldRequired = false;
            AccountingSettingRepository accountingSettingsRepository = new AccountingSettingRepository(Tenant);
            AccountingSetting iAccountingSetting = accountingSettingsRepository.GetSingleAccountSetting(Tenant);

            if (iAccountingSetting.IsVatNumberMandatoryInAR)
            {
                if (string.IsNullOrEmpty(aRInvoicePM.VatNumber))
                {
                    isFieldRequired = true;
                }
                if (isFieldRequired == true)
                {
                    ContactPM contactLocal = GetLoggedContact(Tenant);
                    bool showLocals = !contactLocal.DontShowLocal;
                    string ErrorMessage = TextCodesTranslator.TranslateText("General.M.FieldIsRequired", Tenant, showLocals);
                    ErrorMessage = ErrorMessage.Replace("%FieldName", TextCodesTranslator.TranslateText("ARInvoice.F.VatNumber", Tenant, showLocals));
                    throw new Exception(ErrorMessage);
                }
            }
        }
        private void UpdateInterestReportsStatues(InterestReportPM interestReportPM, int Tenant, string Statues, ARInvoicePM aRInvoicePM = null, string InvoiceFailureReason = null)
        {
            var accountingContext = AccountingContext.GetContext(Tenant);

            interestReportPM.InterestReportStatusCode = Statues;
            interestReportPM.UpdatedByUserId = userPM.Id;
            interestReportPM.UpdateDateTime = TenantServerConfigration.GetCurrentDateTime(Tenant);
            if (InvoiceFailureReason != null)
            {
                interestReportPM.InvoiceFailureReason = InvoiceFailureReason;

            }
            if (aRInvoicePM != null)
            {
                interestReportPM.ARinvoiceId = aRInvoicePM.Id;
                interestReportPM.InvoiceAmount = (decimal?)aRInvoicePM.AmountInLocalCurrency;
            }
            interestReportPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            InterestReportUpdateService service = new InterestReportUpdateService(accountingContext, new Dictionary<string, IContext>(), Tenant);
            interestReportPM.IsUpdatedFromBatch = true;
            service.Update(interestReportPM, true);

        }

        private ARInvoicePM FullMapInvoice(InterestReportArgs interestReportArgs, InterestReportPM interestReport)
        {
            CardQuery cardQueryService = new CardQuery(interestReportArgs.Tenant);
            CardPM cardPM = cardQueryService.GetSinglePM(interestReport.CustomerId, interestReportArgs.Tenant);

            TenantQuery tenantQuery = new TenantQuery(interestReportArgs.Tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(interestReportArgs.Tenant);

 
            ObjectTableQuery objectTableQuery = new ObjectTableQuery(interestReportArgs.Tenant);


            ChargesTypeQuery chargesTypeQuery = new ChargesTypeQuery(interestReportArgs.Tenant);
            List<ChargesTypePM> chargesTypes = chargesTypeQuery.GetChargesTypesByCode("INT", interestReportArgs.Tenant);
            ChargesTypePM chargesType = GetChargesType(chargesTypes, interestReportArgs);
            

            VatTypePercentageQuery vatTypePercentageQuery = new VatTypePercentageQuery(interestReportArgs.Tenant);
            VatTypePercentagePM vatTypePercentagePM = vatTypePercentageQuery.GetVatTypePercentagesForVatType(interestReportArgs.Tenant, chargesType.VatTypeId).ToList()[0];


            InterestReportObjectTableId = objectTableQuery.GetObjectTableIdByName("InterestReport");
            ARInvoiceObjectTableId = objectTableQuery.GetObjectTableIdByName("ARInvoice");
            string email = "system@tenant" + interestReportArgs.Tenant.ToString() + ".com";
            AuthenticationUtil.AuthenticatedUserEmail = email;

            InterestReportInvoiceMapping interestReportInvoiceMapping = new InterestReportInvoiceMapping();
            ARInvoicePM aRInvoicePM = interestReportInvoiceMapping.MapARInvoice(interestReportArgs, interestReport, tenantPM, userPM, cardPM);
            aRInvoicePM.InvoiceDate = interestReportArgs.InvoiceDate;
            ARInvoiceEntityPM aRInvoiceEntityPM = interestReportInvoiceMapping.MapARInvoiceEntity(interestReportArgs, InterestReportObjectTableId);
            ARInvoiceLinePM aRInvoiceLinePM = interestReportInvoiceMapping.MapARInvoiceLine(interestReport, tenantPM, chargesType, vatTypePercentagePM);

            aRInvoicePM.InvoiceEntities.Add(aRInvoiceEntityPM);
            aRInvoicePM.InvoiceLines.Add(aRInvoiceLinePM);

            return aRInvoicePM;
 

        }

        private ChargesTypePM GetChargesType(List<ChargesTypePM> chargesTypes, InterestReportArgs interestReportArgs)
        {
            ChargesTypePM chargesType = null;
            if (chargesTypes == null || chargesTypes.Count == 0)
            {
                ContactPM contactLocal = GetLoggedContact(interestReportArgs.Tenant);
                bool showLocals = !contactLocal.DontShowLocal;
                string ErrorMessage = TextCodesTranslator.TranslateText("General.M.FieldIsRequired", interestReportArgs.Tenant, showLocals);
                ErrorMessage = ErrorMessage.Replace("%FieldName", TextCodesTranslator.TranslateText("ARInvoiceLine.F.ChargesTypeId", interestReportArgs.Tenant, showLocals));
                throw new Exception(ErrorMessage);

            }
            else
            {
                List<ChargesTypePM> ActivechargesType = chargesTypes.Where(s => s.InActive == false).ToList();
                if (ActivechargesType == null || ActivechargesType.Count == 0)
                {
                    ContactPM contactLocal = GetLoggedContact(interestReportArgs.Tenant);
                    bool showLocals = !contactLocal.DontShowLocal;
                    string ErrorMessage = TextCodesTranslator.TranslateText("General.O.FieldIsInactive", interestReportArgs.Tenant, showLocals);
                    ErrorMessage = ErrorMessage.Replace("%FieldName", TextCodesTranslator.TranslateText("ARInvoiceLine.F.ChargesTypeId", interestReportArgs.Tenant, showLocals));
                    throw new Exception(ErrorMessage);

                }
                else
                {
                    chargesType = chargesTypes.Where(s => s.InActive == false).FirstOrDefault();

                }

            }
            return chargesType;
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
    }
}
