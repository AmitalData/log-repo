using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using Stimulsoft.Report.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;

//using WebFreight.Web.Helpers;

namespace Logitude.Accounting.BL.CoreBL
{
    public class TaxDeductionReportService
    {

        public static DocumentsFilingPM Create856File(string taxDeductionReportId, int tenant)
        {
            TaxDeductionReportQueryService taxDeductionReportQueryService = new TaxDeductionReportQueryService(tenant);
            TaxDeductionReportPM taxDeductionReportPM = taxDeductionReportQueryService.GetSingle(taxDeductionReportId, false, false);

            List<string> linesArray = new List<string>();
            //GLAccountQueryService queryService = new GLAccountQueryService(tenant);
            TaxDeductionReportDataProvider deductionReportDataProvider = new TaxDeductionReportDataProvider(taxDeductionReportPM,tenant);
            TaxDeductionReportData data = deductionReportDataProvider.GetTaxDeductionReportData();
            //TaxDeductionReportData data = queryService.GetTaxDeductionReportData(taxDeductionReportPM.TaxYear, tenant);
            string xml = LogitudeXmlSerializer.SerializeObjectToXmlString(data);
            StringBuilder myStringBuilder = new StringBuilder();
            string a = null;
            if (taxDeductionReportPM.Email == "sumaya@logitudeworld.com")
            {
                a = "a";
            }
            //60s
            foreach (ByVendorList item in data.ByVendorList)
            {

                myStringBuilder.Append("");
                if (data.SettingDeductionFileNumber != null)
                {
                    if (data.SettingDeductionFileNumber.Length > 9) data.SettingDeductionFileNumber = data.SettingDeductionFileNumber.Substring(0, 9);
                    myStringBuilder.Append(data.SettingDeductionFileNumber.PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append('0', 9);
                }
                myStringBuilder.Append(a);
                myStringBuilder.Append("96");
                myStringBuilder.Append(taxDeductionReportPM.TaxYear);

                myStringBuilder.Append(a);
                if (item.IsAutonomy)
                {
                    myStringBuilder.Append("2");

                }
                else if (item.IsInternationlPartner)
                {
                    myStringBuilder.Append("5");
                }
                else myStringBuilder.Append("0");

                myStringBuilder.Append(a);
                if (item.DeductionType != null)
                {
                    if (item.DeductionType.Length > 1) item.DeductionType.Substring(0, 1);

                    myStringBuilder.Append( item.DeductionType);
                }
                else
                {
                    myStringBuilder.Append("0");
                }

                myStringBuilder.Append(a);
                if (item.VATNumber != null)
                {
                  
                    if (item.VATNumber.Length > 9) item.VATNumber = item.VATNumber.Substring(0, 9);
                    myStringBuilder.Append( item.VATNumber.PadLeft(9, '0'));
                }

                else
                {
                   

                   myStringBuilder.Append('0', 9);
                }

                myStringBuilder.Append(a);
                if (item.DisplayNumber != null)
                {
                  
                    if (item.DisplayNumber.Length > 14) item.DisplayNumber = item.DisplayNumber.Substring(0, 14);
                    myStringBuilder.Append(item.DisplayNumber.PadLeft(14, '0'));

                }
                else
                {
                   
                    myStringBuilder.Append('0', 14);
                }

                myStringBuilder.Append(a);
                if (item.DeductionFileTypeCode == "08")
                {
                    if (item.EnglishName != null)
                    {
                        if (item.EnglishName.Length > 22) item.EnglishName = item.EnglishName.Substring(0, 22);

                        myStringBuilder.Append(a+ item.EnglishName.ToUpper().PadLeft(22, ' '));
                    }
                }
                else
                {
                    if (item.GLAccountLocalName != null)
                    {
                        if (item.GLAccountLocalName.Length > 22) item.GLAccountLocalName = item.GLAccountLocalName.Substring(0, 22);
                        myStringBuilder.AppendFormat(item.GLAccountLocalName.PadLeft(22, ' '));
                    }

                    else
                    {
                       
                        myStringBuilder.Append(' ', 22);
                    }

                }




                myStringBuilder.Append(a);
                if (item.VendorAddress != null)
                {
                    if (item.VendorAddress.Length > 21) item.VendorAddress = item.VendorAddress.Substring(0, 21);
                   
                    myStringBuilder.Append(item.VendorAddress.PadLeft(21, ' '));
                }

                else
                {
                    myStringBuilder.Append(' ', 21);
                }

                myStringBuilder.Append(a);
                if (item.VendorCity != null)
                {
                    if (item.VendorCity.Length > 13) item.VendorCity = item.VendorCity.Substring(0, 13);

                    myStringBuilder.Append( item.VendorCity.PadLeft(13, ' '));
                }
                else
                {
                    myStringBuilder.Append(' ', 13);
                }
                myStringBuilder.Append(a);
                string totalInLocalCur =item.SumOfAmountInLocalCurrency!= null? item.SumOfAmountInLocalCurrency.Value.ToString():null;
                if (totalInLocalCur != null)
                {
                    if (totalInLocalCur.Length > 11) totalInLocalCur = totalInLocalCur.Substring(0, 11);

                    myStringBuilder.Append( totalInLocalCur.PadLeft(11, '0'));
                }
                else
                {
                    //myStringBuilder.Append("a");

                  myStringBuilder.Append('0', 11);
                }
                // myStringBuilder.Append("a" + item.SumOfAmountInLocalCurrency.Value.ToString().PadLeft(11, '0'));



                //   myStringBuilder.Append("a" + item.SumOfTaxDeductionLocalAmount.Value.ToString().PadLeft(9, '0'));
                myStringBuilder.Append(a);
                string totalTaxInLocalCur =item.SumOfTaxDeductionLocalAmount != null? item.SumOfTaxDeductionLocalAmount.Value.ToString():null;
                if (totalTaxInLocalCur != null)
                {
                    if (totalTaxInLocalCur.Length > 9) totalTaxInLocalCur = totalTaxInLocalCur.Substring(0, 9);

                    myStringBuilder.Append( totalTaxInLocalCur.PadLeft(9, '0'));
                }
                else
                {
                  //  myStringBuilder.Append("a");

                    myStringBuilder.Append('0', 9);
                }

                myStringBuilder.Append(a);
                myStringBuilder.Append("00000000");
                myStringBuilder.Append(a);
                string endYearBalance =  item.EndYearBalance.ToString();
                if (endYearBalance != null)
                {
                    if (endYearBalance.Length > 8) endYearBalance = endYearBalance.Substring(0, 8);

                    myStringBuilder.Append( endYearBalance.PadLeft(8, '0'));
                }
                else
                {

                    myStringBuilder.Append('0', 8);
                }
                myStringBuilder.Append(a);
                if (item.TaxDeductionPercentage != null)
                {
                    if (item.TaxDeductionPercentage.Value.ToString().Length > 2) item.TaxDeductionPercentage.Value.ToString().Substring(0, 2);

                    myStringBuilder.Append( item.TaxDeductionPercentage.Value.ToString().PadLeft(2, '0'));
                }

                else
                {

                    myStringBuilder.Append('0', 2);
                }
                // myStringBuilder.Append("a" + item.TaxDeductionPercentage.Value.ToString().PadLeft(2, '0'));
                myStringBuilder.Append(a);
                string s = item.AssessingOfficerCode + " " + item.AssessingOfficerName;
                if (s.Length > 12) s = s.Substring(0, 12);
                myStringBuilder.Append( s.PadLeft(12, ' '));
                myStringBuilder.Append(a);
                if (item.Occupation != null)
                {
                    if (item.Occupation.Length > 14) item.Occupation = item.Occupation.Substring(0, 14);
                    myStringBuilder.Append( item.Occupation.PadLeft(14, ' '));
                }
                else
                {
                  //  myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 14);
                }
                myStringBuilder.Append(a);
                myStringBuilder.Append(' ', 52);
                myStringBuilder.Append(a);
                if (item.DeductionFileTypeCode != null)
                {
                    if (item.DeductionFileTypeCode.Length > 2) item.DeductionFileTypeCode.Substring(0, 2);

                    myStringBuilder.Append( item.DeductionFileTypeCode.PadLeft(2, '0'));
                }
                else
                {
                   // myStringBuilder.Append("a");

                   myStringBuilder.Append('0', 2);
                }
                myStringBuilder.Append(a);
                myStringBuilder.Append("60");
                myStringBuilder.Append("\r\n");

            }

            //70s


            myStringBuilder.Append(a);
            FullAccountingSettingQueryService fullAccountingSettingQueryService = new FullAccountingSettingQueryService(tenant);
            FullAccountingSettingPM setting = fullAccountingSettingQueryService.GetSingleFullAccountingSetting(tenant);
            if (setting.DeductionFileNumber != null)
            {
                if (setting.DeductionFileNumber.Length > 9) setting.DeductionFileNumber = setting.DeductionFileNumber.Substring(0, 9);

                myStringBuilder.Append(setting.DeductionFileNumber.PadLeft(9, '0'));
            }

            else
            {

                myStringBuilder.Append('0', 9);
            }
            myStringBuilder.Append(a);
            myStringBuilder.Append("96");
            myStringBuilder.Append( taxDeductionReportPM.TaxYear);
            myStringBuilder.Append(a);
            myStringBuilder.Append(' ', 3);
            myStringBuilder.Append(a);
            myStringBuilder.Append("0");
            myStringBuilder.Append(a);
            if (taxDeductionReportPM.IsAdditionalReportExist)
            {
                myStringBuilder.Append("2");
            }
            else
            {

                myStringBuilder.Append("0");
            }
            myStringBuilder.Append(a);
            myStringBuilder.Append("1");
            myStringBuilder.Append(a);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);
            if (tenantPM.VatNumber != null)
            {
                if (tenantPM.VatNumber.Length > 9) tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9);

                myStringBuilder.Append( tenantPM.VatNumber.PadLeft(9, '0'));

            }
            else
            {

                myStringBuilder.Append('0', 9);
            }
            myStringBuilder.Append(a);
            if (data.TotalAmountInLocalCurrency08 != null)
            {
                if (data.TotalAmountInLocalCurrency08.Value.ToString().Length > 12) data.TotalAmountInLocalCurrency08.Value.ToString().Substring(0, 12);

                myStringBuilder.Append( data.TotalAmountInLocalCurrency08.Value.ToString().PadLeft(12, '0'));
            }
            else
            {

                myStringBuilder.Append('0', 12);
            }
            myStringBuilder.Append(a);
            if (data.TotalTaxDeductionInLocalCurrency08 != null)
            {
                if (data.TotalTaxDeductionInLocalCurrency08.Value.ToString().Length > 10) data.TotalTaxDeductionInLocalCurrency08.Value.ToString().Substring(0, 10);

                myStringBuilder.Append(data.TotalTaxDeductionInLocalCurrency08.Value.ToString().PadLeft(10, '0'));
            }
            else
            {

                myStringBuilder.Append('0', 10);
            }
            myStringBuilder.Append(a);
            if (data.TotalEndBalance != null)
            {
                if (data.TotalEndBalance.Value.ToString().Length > 11) data.TotalEndBalance.Value.ToString().Substring(0, 11);

                myStringBuilder.Append( data.TotalEndBalance.Value.ToString().PadLeft(11, '0'));
            }
            else
            {

                myStringBuilder.Append('0', 11);
            }

            // myStringBuilder.Append("a" + data.ByVendorList.Where(d => d.DeductionFileTypeCode == "08").Sum(d => d.SumOfAmountInLocalCurrency));
            // myStringBuilder.Append("a" + data.ByVendorList.Where(d => d.DeductionFileTypeCode == "08").Sum(d => d.SumOfTaxDeductionLocalAmount));




            AddressQuery addressQuery = new AddressQuery(tenant);
            AddressPM address = addressQuery.GetSingleAddressPM(tenantPM.AddressId, tenant);
         
            if (address != null)
            {
                myStringBuilder.Append(a);
                if (address.PhoneNumber != null)
                {
                    if (address.PhoneNumber.Length > 10) address.PhoneNumber = address.PhoneNumber.Substring(0, 10);
                    myStringBuilder.Append( address.PhoneNumber.PadLeft(10, '0'));
                }
                else
                {

                    myStringBuilder.Append('0', 10);
                }
            }
            myStringBuilder.Append(a);
            myStringBuilder.Append(' ', 28);
            myStringBuilder.Append(a);
            if (data.TotalAmountInLocalCurrency != null)
            {
                if (data.TotalAmountInLocalCurrency.Value.ToString().Length > 12) data.TotalAmountInLocalCurrency.Value.ToString().Substring(0, 12);

                myStringBuilder.Append( data.TotalAmountInLocalCurrency.Value.ToString().PadLeft(12, '0'));
            }
            else
            {

                myStringBuilder.Append('0', 12);
            }
            myStringBuilder.Append(a);
            if (data.TotalDeductionInLocalCurrency != null)
            {
                if (data.TotalDeductionInLocalCurrency.Value.ToString().Length > 10) data.TotalDeductionInLocalCurrency.Value.ToString().Substring(0, 10);

                myStringBuilder.Append( data.TotalDeductionInLocalCurrency.Value.ToString().PadLeft(10, '0'));
            }

            else
            {

                myStringBuilder.Append('0', 10);
            }
            myStringBuilder.Append(a);
            myStringBuilder.Append('0', 9);
            myStringBuilder.Append(a);
            myStringBuilder.Append(' ', 9);
            myStringBuilder.Append(a);
            if (data.VendorsCount != null)
            {
                if (data.VendorsCount.Value.ToString().Length > 6) data.VendorsCount.Value.ToString().Substring(0, 5);

                myStringBuilder.Append( data.VendorsCount.Value.ToString().PadLeft(6, '0'));
            }
            else
            {

                myStringBuilder.Append('0', 6);
            }
            myStringBuilder.Append(a);
            if (data.ByVendorList.Count.ToString().Length > 6) data.ByVendorList.Count.ToString().Substring(0, 5);
            myStringBuilder.Append(a);
            myStringBuilder.Append( data.ByVendorList.Count.ToString().PadLeft(6, '0'));

            myStringBuilder.Append(a);
            if (taxDeductionReportPM.Email != null)
            {
                if (taxDeductionReportPM.Email.Length > 50) taxDeductionReportPM.Email = taxDeductionReportPM.Email.Substring(0, 50);
                myStringBuilder.Append( taxDeductionReportPM.Email.PadLeft(50, ' '));
            }
            else
            {

                myStringBuilder.Append(' ', 50);
            }
            myStringBuilder.Append(a);
            myStringBuilder.Append("הקידב");
            myStringBuilder.Append(a);
            myStringBuilder.Append(' ', 6);
            myStringBuilder.Append(a);
            myStringBuilder.Append("70");
            myStringBuilder.Append("\r\n");

            //80s

            foreach (ByMonthList item in data.ByMonthList)
            {
                myStringBuilder.Append(a);
                if (setting.DeductionFileNumber != null)
                {
                    if (setting.DeductionFileNumber.Length > 9) setting.DeductionFileNumber.Substring(0, 9);
                    myStringBuilder.Append( setting.DeductionFileNumber.PadLeft(9, '0'));

                }
                else
                {

                    myStringBuilder.Append('0', 9);
                }
                myStringBuilder.Append(a);
                myStringBuilder.Append("96");
                myStringBuilder.Append(a);
                myStringBuilder.Append(taxDeductionReportPM.TaxYear);
                myStringBuilder.Append(a);
                if (item.Month.ToString().Length < 2)
                {
                    myStringBuilder.Append( item.Month.ToString().PadLeft(2, '0'));
                }
                else
                {
                    myStringBuilder.Append( item.Month);
                }
                myStringBuilder.Append(a);
                if (item.TotalVendors.ToString().Length < 6)
                {
                    myStringBuilder.Append(item.TotalVendors.ToString().PadLeft(6, '0'));
                }
                else
                {
                    myStringBuilder.Append(item.TotalVendors);
                }

                myStringBuilder.Append(a);
                string TotalPaymentsWithoutDivided = item.TotalPaymentsWithoutDivided.ToString();
                if (TotalPaymentsWithoutDivided != null)
                {
                    if (TotalPaymentsWithoutDivided.Length > 12) TotalPaymentsWithoutDivided= TotalPaymentsWithoutDivided.Substring(0, 12);

                    myStringBuilder.Append( TotalPaymentsWithoutDivided.PadLeft(12, '0'));
                }
                else
                {

                    myStringBuilder.Append('0', 12);
                }
                myStringBuilder.Append(a);
                string TotalDeductionsWithoutDivided = item.TotalDeductionsWithoutDivided.ToString();
                if (TotalDeductionsWithoutDivided != null)
                {
                    if (TotalDeductionsWithoutDivided.Length > 12) TotalDeductionsWithoutDivided= TotalDeductionsWithoutDivided.Substring(0, 12);

                    myStringBuilder.Append( TotalDeductionsWithoutDivided.PadLeft(12, '0'));
                }

                else
                {

                    myStringBuilder.Append('0', 12);
                }
                //myStringBuilder.Append("a" + data.ByVendorList.Where(d => d.Month == item.Month && d.DeductionFileTypeCode != "18").Sum(d => d.SumOfAmountInLocalCurrency));
                //myStringBuilder.Append("a" + data.ByVendorList.Where(d => d.Month == item.Month && d.DeductionFileTypeCode != "18").Sum(d => d.SumOfTaxDeductionLocalAmount));
                myStringBuilder.Append(a);
                myStringBuilder.Append('0', 12);
                string TotalDivided = item.TotalDivided.ToString();
                myStringBuilder.Append(a);
                if (TotalDivided  != null)
                {
                    if (TotalDivided.Length > 12) TotalDivided= TotalDivided.Substring(0, 12);

                    myStringBuilder.Append(TotalDivided.PadLeft(12, '0'));
                }
                else
                {

                    myStringBuilder.Append('0', 12);
                }
                myStringBuilder.Append(a);
                string TotalDeductionsFromDivided = item.TotalDeductionsFromDivided.ToString();
                if (TotalDeductionsFromDivided  != null)
                {
                    if (TotalDeductionsFromDivided.Length > 12) TotalDeductionsFromDivided= TotalDeductionsFromDivided.Substring(0, 12);

                    myStringBuilder.Append( TotalDeductionsFromDivided.PadLeft(12, '0'));
                }
                else
                {

                    myStringBuilder.Append('0', 12);
                }
                //myStringBuilder.Append("a" + data.ByVendorList.Where(d => d.Month == item.Month && d.DeductionFileTypeCode == "18").Sum(d => d.SumOfAmountInLocalCurrency));
                //myStringBuilder.Append("a" + data.ByVendorList.Where(d => d.Month == item.Month && d.DeductionFileTypeCode == "18").Sum(d => d.SumOfTaxDeductionLocalAmount));
                myStringBuilder.Append(a);
                myStringBuilder.Append(' ', 131);
                myStringBuilder.Append(a);
                myStringBuilder.Append("80");
                myStringBuilder.Append("\r\n");

            }

         

            DocumentsFilingPM docOut = CreateDocumnetFiling(myStringBuilder, setting.DeductionFileNumber, taxDeductionReportPM);
          
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
                }, tenant);
                scope.Complete();
            }

            return taskExe;

        }


        private static DocumentsFilingPM CreateDocumnetFiling(StringBuilder lines,string DeductionFileNumber, TaxDeductionReportPM taxDeductionReport, bool isFromWR = false)
        {
            // prepare file string
            string file = string.Join(Environment.NewLine, lines);

            // create document
            int tenant = taxDeductionReport.Tenant;
            ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
            DocumentsFilingService docService = new DocumentsFilingService(MyContext, tenant);
            DocumentTypeRepository docTypeReposioty = new DocumentTypeRepository(MyContext);

            ObjectTableRepository tableRep = new ObjectTableRepository(tenant);
            ObjectTable table = tableRep.GetObjectTableByName("TaxDeductionReport", 0, true);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);

            // user
            User loggedUser = GetLoggedUser(tenant);
            DocumentType docType = docTypeReposioty.GetSingleDocumentTypeByCode("TDR856", tenant);

            string _code = CodeCounter.GetNumber("DocumentsFiling", tenant).ToString();
            string deductionfilenumber = GetDeductionFileNumber(DeductionFileNumber);
            string name = "A856." + deductionfilenumber + "." + taxDeductionReport.TaxYear.ToString().Substring(1, 3);
            DocumentsFilingPM document = new DocumentsFilingPM()
            {
                Description = "TDR856 Text File",
                DocumentTypeId = docType.Id,
                Tenant = tenant,
                DirectionCode = "I",
                EntityId = taxDeductionReport.Id,
                EntityNumber = taxDeductionReport.ReportNumber != null ? taxDeductionReport.ReportNumber.ToString() : null,
                ObjectTableId = table.Id,
                Code = _code,
                CreatedByUserId = loggedUser.Id,
                OwnerId = loggedUser.Id,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                UpdatedByUserId = loggedUser.Id,
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                FileExtension = "txt",
                SecurityId = "100",
                FileName = name,
            };

            byte[] bytearray = Encoding.Default.GetBytes(file);
            document.FileData = bytearray;
            docService.Create(document, document.FileData, loggedUser.Id);


            //get document out
            DocumentsFilingQuery queryService = new DocumentsFilingQuery(tenant);
            string code = (Convert.ToInt32(_code) + 1).ToString();
            DocumentsFilingPM docFiling = queryService.GetDocumentsFilingByDocumentCode(code, tenant);


            return docFiling;
        }

        private static string GetDeductionFileNumber(string DeductionFileNumber)
        {
            string deductionfilenumber;
            if (DeductionFileNumber == null)
            {
                deductionfilenumber = "000000000";
            }
            else
            {
                deductionfilenumber = DeductionFileNumber;
            }
            deductionfilenumber = deductionfilenumber.Length > 8 ? deductionfilenumber.Substring(deductionfilenumber.Length - 8) : deductionfilenumber;
            return deductionfilenumber;
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


        //public static DocumentOutPM CreateDocumentOut(string documentTypeId, string entityId, string childEntityId, string childReference, string objectTableId, int tenant, string userId = null)
        //{
        //    try
        //    {
        //        ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
        //        DocumentOutRepository documentOutRepository = new DocumentOutRepository(objectContext);
        //        DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
        //        DocumentOutQuery documentOutQuery = new DocumentOutQuery(documentOutRepository);

        //        DocumentTypePM documentType = documentTypeQuery.GetSinglePM(documentTypeId, tenant);
              

        //        string documentTemplateId = null;
        //        string emailTemplateId = null;

        //        documentTemplateId = documentType.DocumentTypeDefaultReportTemplateId;
        //        emailTemplateId = documentType.DocumentTypeDefaultHTMLTemplateId;


        //        //---------------------------------------- islam
        //        DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(objectContext);
            
        //        if (string.IsNullOrEmpty(userId))
        //        {
                  
                 
        //            User loggedUser = GetLoggedUser(tenant);
                        
        //            if (loggedUser != null)
        //            {
        //                userId = loggedUser.Id;
        //            }
        //        }

        //        DocumentsFiling newDocumentFiling = new DocumentsFiling() { DocumentTypeId = documentTypeId, EntityId = entityId, Tenant = tenant, ObjectTableId = objectTableId, ChildEntityId = childEntityId, ChildEntityReference = childReference, DirectionCode = "O" };

        //        newDocumentFiling.Id = IdCounter.GetNumber("Document", tenant).ToString();
        //        newDocumentFiling.SecurityId = newDocumentFiling.Id + RandomString(10);
        //        newDocumentFiling.Code = CodeCounter.GetNumber("DocumentsFiling", tenant).ToString();
        //        newDocumentFiling.CreatedByUserId = userId;
        //        newDocumentFiling.OwnerId = userId;
        //        newDocumentFiling.UpdatedByUserId = userId;
              
        //        newDocumentFiling.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
        //        newDocumentFiling.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
        //        newDocumentFiling.SearchFields = newDocumentFiling.Code + "," + newDocumentFiling.DirectionCode;
        //        documentsFilingRepository.Add(newDocumentFiling);

        //        //----------------------------------------
        //        DocumentOut newDocument = new DocumentOut() { EmailTemplateId = emailTemplateId, DocumentTemplateId = documentTemplateId, Tenant = tenant, Issued = false, };
        //        newDocument.Id = newDocumentFiling.Id;
        //        documentOutRepository.Add(newDocument);



        //        objectContext.SaveChanges();
        //        string documentTypeOutId = null;
        //        if(documentType.DocumentTypeCopies.Count > 0)
        //        {
        //            documentTypeOutId = documentType.DocumentTypeCopies.FirstOrDefault().Id;
        //        }
        //        //ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
        //        //exportDocumentHelper.ExportDocument2Pdf(documentType.Id, entityId, objectTableId, null, null, newDocument.Id, tenant, documentTypeOutId);

        //        DocumentOutPM docPM = documentOutQuery.GetSinglePM(newDocument.Id, newDocument.Tenant);
        //        return docPM;
        //    }
        //    catch (System.Data.Entity.Validation.DbEntityValidationException e)
        //    {
        //        string Error = "";
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            Error += "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:";
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                //ve.PropertyName, ve.ErrorMessage);

        //                Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
        //            }
        //        }


        //        string authenticateduser = "";

        //        try
        //        {
        //            authenticateduser = Logitude.BL.Security.SecurityUtility.GetAuthenticatedUser();
        //        }

        //        catch
        //        {
        //            authenticateduser = "UnKnown";
        //        }
        //        string ip = "";
        //        if (HttpContext.Current != null && HttpContext.Current.Request != null)
        //        {
        //            string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
        //            if (string.IsNullOrEmpty(currentIP))
        //            {
        //                currentIP = HttpContext.Current.Request.UserHostAddress;
        //            }
        //            ip = currentIP;
        //        }
        //      //  ExceptionHandler.HandleException(new Exception(Error), DateTime.Now, 0, "", authenticateduser, "", ip);
        //        throw new Exception(Error);
        //    }
        //}

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


     


    }
}
