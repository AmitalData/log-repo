using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL
{
   public  class TaxDeductionReportService
    {

        public static DocumentsFilingPM Create856File(string taxDeductionReportId, int tenant)
        {
            List<string> linesArray = new List<string>();
            TaxDeductionReportQueryService taxDeductionReportQueryService = new TaxDeductionReportQueryService(tenant);
            TaxDeductionReportPM taxDeductionReportPM = taxDeductionReportQueryService.GetSingle(taxDeductionReportId, false, false);
            GLAccountQueryService queryService = new GLAccountQueryService(tenant);
            List<TaxDeductionReportData> data = queryService.GetTaxDeductionReportData(taxDeductionReportPM.TaxYear, tenant);

            StringBuilder myStringBuilder = new StringBuilder();

            //60s
            foreach (TaxDeductionReportData item in data)
            {
               
                myStringBuilder.Append("");
                if (item.DeductionFileNumber != null)
                {
                    if (item.DeductionFileNumber.Length > 9) item.DeductionFileNumber = item.DeductionFileNumber.Substring(0, 9);
                    myStringBuilder.Append(item.DeductionFileNumber.PadLeft(9, '0'));
                }
                myStringBuilder.Append("96");
                myStringBuilder.Append(taxDeductionReportPM.TaxYear);


                if (item.IsAutonomy)
                {
                    myStringBuilder.Append("2");

                }
                else if (item.IsInternationlPartner)
                {
                    myStringBuilder.Append("5");
                }
                else myStringBuilder.Append("0");

                if (item.DeductionFileTypeCode != null)
                {
                    if (item.DeductionFileTypeCode.Length > 1) item.DeductionFileTypeCode.Substring(0, 1);

                    myStringBuilder.Append(item.DeductionFileTypeCode);
                }

                if (item.VATNumber != null)
                {
                    if (item.VATNumber.Length > 9) item.VATNumber = item.VATNumber.Substring(0, 9);
                    myStringBuilder.Append(item.VATNumber.PadLeft(9, '0'));
                }


                if (item.DisplayNumber != null)
                {
                    if (item.DisplayNumber.Length > 14) item.DisplayNumber = item.DisplayNumber.Substring(0, 14);
                    myStringBuilder.Append(item.DisplayNumber.PadLeft(9, '0'));

                }


                if(item.DeductionType == "08")
                {
                    if (item.EnglishName != null)
                    {
                        if (item.EnglishName.Length > 22) item.EnglishName = item.EnglishName.Substring(0, 22);

                        myStringBuilder.Append(item.EnglishName.ToUpper().PadLeft(22, '0'));
                    }
                }
                else
                {
                    if (item.GLAccountLocalName != null)
                    {
                        if (item.GLAccountLocalName.Length > 22) item.GLAccountLocalName = item.GLAccountLocalName.Substring(0, 22);
                        myStringBuilder.AppendFormat( item.GLAccountLocalName.ToUpper().PadLeft(22, '0'));
                    }
                }

                if (item.VendorAddress != null)
                {
                    if (item.VendorAddress.Length > 13) item.VendorAddress = item.VendorAddress.Substring(0, 13);

                    myStringBuilder.Append(item.VendorAddress.PadLeft(13, '0'));
                }

                myStringBuilder.Append(item.SumOfAmountInLocalCurrency);
                myStringBuilder.Append(item.SumOfTaxDeductionLocalAmount);
                myStringBuilder.Append(item.TaxDeductionPercentage);

                string s = item.AssessingOfficerCode + " " + item.AssessingOfficerName;
                if ( s.Length > 12) s = s.Substring(0, 12);
                myStringBuilder.Append(s.PadLeft(12,'0'));

                if (item.Occupation != null)
                {
                    if (item.Occupation.Length > 14) item.Occupation = item.Occupation.Substring(0, 14);
                    myStringBuilder.Append(item.Occupation.PadLeft(14, '0'));
                }
                myStringBuilder.Append(' ', 52);

                if (item.DeductionType != null)
                {
                    if (item.DeductionType.Length > 2) item.DeductionType.Substring(0, 2);

                    myStringBuilder.Append(item.DeductionType.PadLeft(2, '0'));
                }
                myStringBuilder.Append("60");
                myStringBuilder.Append('\n');

            }

            //70s

            FullAccountingSettingQueryService fullAccountingSettingQueryService = new FullAccountingSettingQueryService(tenant);
            FullAccountingSettingPM setting = fullAccountingSettingQueryService.GetSingleFullAccountingSetting(tenant);
            if (setting.DeductionFileNumber != null) {
                if (setting.DeductionFileNumber.Length > 9) setting.DeductionFileNumber = setting.DeductionFileNumber.Substring(0, 9);

                myStringBuilder.Append(setting.DeductionFileNumber.PadLeft(9, '0'));
              }

            myStringBuilder.Append(taxDeductionReportPM.TaxYear);
            if(taxDeductionReportPM.IsAdditionalReportExist)
            {
                myStringBuilder.Append("2");
            }
            else
            {

                myStringBuilder.Append("0");
            }

            myStringBuilder.Append("1");

            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);
            if(tenantPM.VatNumber != null)
            {
                if (tenantPM.VatNumber.Length > 9) tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9);

                myStringBuilder.Append(tenantPM.VatNumber.PadLeft(9, '0'));

            }

            myStringBuilder.Append(data.Where(d => d.DeductionType == "08").Sum(d => d.SumOfAmountInLocalCurrency));
            myStringBuilder.Append(data.Where(d => d.DeductionType == "08").Sum(d => d.SumOfTaxDeductionLocalAmount));

            AddressQuery addressQuery = new AddressQuery(tenant);
            AddressPM address = addressQuery.GetSingleAddressPM(tenantPM.AddressId, tenant);
            if(address != null)
            {
                if(address.PhoneNumber != null)
                {
                    if (address.PhoneNumber.Length > 10) address.PhoneNumber = address.PhoneNumber.Substring(0, 10);
                    myStringBuilder.Append(address.PhoneNumber.PadLeft(10,'0'));
                }
                
            }
            myStringBuilder.Append(data.Sum(d => d.SumOfAmountInLocalCurrency));
            myStringBuilder.Append(data.Sum(d => d.SumOfTaxDeductionLocalAmount));
            myStringBuilder.Append('0', 9);
            myStringBuilder.Append(' ', 9);
            myStringBuilder.Append( data.Count().ToString().PadLeft(6,'0'));
            myStringBuilder.Append(data.Count().ToString().PadLeft(6, '0'));

            if(taxDeductionReportPM.Email != null)
            {
                if (taxDeductionReportPM.Email.Length > 50) taxDeductionReportPM.Email = taxDeductionReportPM.Email.Substring(0, 50);
                myStringBuilder.Append(taxDeductionReportPM.Email.PadLeft(50, '0'));
            }

            myStringBuilder.Append("בדיקה");
            myStringBuilder.Append(' ', 6);
            myStringBuilder.Append("70");
            myStringBuilder.Append("\n");

            //80

            DocumentsFilingPM docOut = CreateDocumnetFiling(myStringBuilder, taxDeductionReportPM);

            return docOut;
        }

        public static BatchTaskExecutionPM Create856FileInBatch(string taxReportId, int tenant)
        {
            BatchTaskExecutionPM taskExe;
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                // 1- create BTE record
                PNCFileArgs args = new PNCFileArgs() { ReportId = taxReportId, Tenant = tenant };
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(PNCFileArgs));
                serializer.Serialize(stringwriter, args);
                string xmlParameters = stringwriter.ToString();

                taskExe = new BatchTaskExecutionPM()
                {
                    Subject = "Create a flat file for Tax Deduction Report",
                    Tenant = tenant,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    ClassName = "Logitude.Accounting.BL.CoreBL.Batch.BatchTaxDeductionReportService,Logitude.Accounting.BL",
                    CreateDate = DateTime.Now,
                    PrametersXml = xmlParameters,
                    StatusCode = "C",

                };


                IInfrastructureContext MyContext = InfrastructureContext.GetContext(tenant);
                BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                bteUpdateService.Update(taskExe, true);

                // 2- Send to queue
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
                queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant", tenant.ToString() }
                });
                scope.Complete();
            }

            return taskExe;

        }


        private static DocumentsFilingPM CreateDocumnetFiling(StringBuilder lines, TaxDeductionReportPM taxDeductionReport, bool isFromWR = false)
        {
            // prepare file string
            string file = string.Join(Environment.NewLine,lines);

            // create document
            int tenant = taxDeductionReport.Tenant;
            ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
            DocumentsFilingService docService = new DocumentsFilingService(MyContext, tenant);
            DocumentTypeRepository docTypeReposioty = new DocumentTypeRepository(MyContext);

            ObjectTableRepository tableRep = new ObjectTableRepository(tenant);
            ObjectTable table = tableRep.GetObjectTableByName("TaxDeductionReport", 0, true);

            // user
            User loggedUser = GetLoggedUser(tenant);
            DocumentType docType = docTypeReposioty.GetSingleDocumentTypeByCode("TDR856", tenant);

            string _code = CodeCounter.GetNumber("DocumentsFiling", tenant).ToString();
            DocumentsFilingPM document = new DocumentsFilingPM()
            {
                Description = "TDR856 Text File",
                DocumentTypeId = docType.Id,
                Tenant = tenant,
                DirectionCode = "I",
                EntityId = taxDeductionReport.Id,
                EntityNumber = taxDeductionReport.ReportNumber.ToString(),
                ObjectTableId = table.Id,
                Code = _code,
                CreatedByUserId = loggedUser.Id,
                OwnerId = loggedUser.Id,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                UpdatedByUserId = loggedUser.Id,
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                FileExtension = "txt",
                SecurityId = "100",
            };

            document.FileData = file.Select(d => (byte)d).ToArray();
            docService.Create(document, file.Select(d => (byte)d).ToArray(), loggedUser.Id);


            //get document out
            DocumentsFilingQuery queryService = new DocumentsFilingQuery(tenant);
            string code = (Convert.ToInt32(_code) + 1).ToString();
            DocumentsFilingPM docFiling = queryService.GetDocumentsFilingByDocumentCode(code, tenant);


            return docFiling;
        }

        private static Contact GetLoggedContact(int tenant)
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact loggedContact;
            if (HttpContext.Current != null)
            {
                string email = HttpContext.Current.User.Identity.Name;
                loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
            }
            else
            {
                string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
                loggedContact = contactRepository.GetSingleContactByEmail(systemContactEmail, tenant);

            }
            return loggedContact;
        }

        private static User GetLoggedUser(int tenant)
        {
            UserRepository userRepository = new UserRepository(tenant);
            User loggedContact;
            if (HttpContext.Current != null)
            {
                string email = HttpContext.Current.User.Identity.Name;
                loggedContact = userRepository.GetSingleUserByCodeOrEmail(null, email, tenant, true);
            }
            else
            {
                string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
                loggedContact = userRepository.GetSingleUserByCodeOrEmail(null, "system@tenant" + tenant + ".com", tenant, true);

            }
            return loggedContact;
        }



    }
}
