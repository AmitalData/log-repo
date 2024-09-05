using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Export;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.ReportsWebServices;
using WebFreight.Web.TaxesApprovalModel;
using Logitude.Server.Tools;
using ICSharpCode.SharpZipLib.Checksum;
using Simplog.Server.Infrastructure.Helpers;

namespace CommunicationWorkerRole
{
    public class DataFiles
    {
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
    }

    public class TaxWorkerRole : WorkerEntryPoint
    {
        private bool serviceStarted = true;
        private int interval = 1;
        private int systemErrorCount = 0;
        QueueDescription queueDescription;
        QueueClient client;

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        int tenant = 0;
                        var message = client.Receive(new TimeSpan(0, 0, 30));
                        LastActivity = DateTime.UtcNow;
                        if (message != null)
                        {
                            try
                            {
                                string reportId = message.Properties["ReportId"].ToString();
                                tenant = (int)message.Properties["Tenant"];
                                string email = message.Properties["Email"].ToString();
                                DateTime date1 = (DateTime)message.Properties["Date1"];
                                DateTime date2 = (DateTime)message.Properties["Date2"];
                                string username = message.Properties["UserName"].ToString();

                                this.BuildTaxApprovalDataFiles(date1, date2, tenant, reportId, email, username);
                                message.Complete();
                                LogDoneItemInMemory();
                            }
                            catch (Exception ex)
                            {
                                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "WorkerRole", "", null);
                                if (message.Properties.Keys.Contains("ReportId"))
                                {
                                    string fileId = message.Properties["ReportId"].ToString();
                                    if (fileId != null)
                                    {
                                        message.Abandon();
                                    }
                                    else
                                    {
                                        message.Complete();
                                    }
                                }
                                else
                                {
                                    message.Complete();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "email worker role start", null, null);
                        Thread.Sleep(10000);
                    }
                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "TaxWR";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            try
            {
                string emailQueueName = ThreadedRoleEntryPoint.GetQueueByEnviroment("Taxdataqueue");

                if (!StorageAcountDetails.NameSpaceManager.QueueExists(emailQueueName))
                {
                    queueDescription = new QueueDescription(emailQueueName);
                    queueDescription.MaxSizeInMegabytes = 5120;
                    StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription);
                }

                client = StorageAcountDetails.CreateServiceBusQueueClient(emailQueueName);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "tax worker role start", null, null);
            }

            ServicePointManager.DefaultConnectionLimit = 12;
            RoleEnvironment.Changing += RoleEnvironmentChanging;
            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                e.Cancel = true;
            }
        }

        public void BuildTaxApprovalDataFiles(DateTime date1, DateTime date2, int tenant, string random, string email, string username)
        {
            IInvoiceContext invoiceCotnext = InvoiceContext.GetContext(tenant);
            ARInvoiceRepository invoiceRep = new ARInvoiceRepository(invoiceCotnext);
            APInvoiceRepository apInvoiceRep = new APInvoiceRepository(invoiceCotnext);
            ARPaymentRepository paymentRep = new ARPaymentRepository(invoiceCotnext);

            TenantRepository tenantRep = new TenantRepository(tenant);
            Tenant tenantPoco = tenantRep.GetSingleTenant(tenant);

            List<ARPayment> payments = paymentRep.GetARPayments(tenant).Where(p => (p.RegisterDate >= date1 && p.RegisterDate <= date2) && p.StatusCode != "DR").ToList();
            List<ARInvoice> invoices = invoiceRep.GetIQueryableInvoices(tenant).Where(i => (i.InvoiceDate >= date1 && i.InvoiceDate <= date2) && i.StatusCode != "DR").ToList();
            List<APInvoice> apInvoices = apInvoiceRep.GetIQueryableInvoices(tenant).Where(i => (i.InvoiceDate >= date1 && i.InvoiceDate <= date2) && i.StatusCode != "WA").ToList();

            string computed = this.Compute(date1, date2, payments, invoices, apInvoices, tenantPoco, invoiceCotnext, random, username);
            byte[] BKMVDATAFile1bytearray = this.ConvertEncoding(computed);

            string str = this.BuildA000File(date1, date2, payments, invoices, apInvoices, tenantPoco, random);
            byte[] inifilebytearray = this.ConvertEncoding(str);

            List<DataFiles> files = new List<DataFiles>() {
                    new DataFiles() { FileName = "BKMVDATA.txt", FileData = BKMVDATAFile1bytearray },
                    new DataFiles() { FileName = "INI.txt", FileData = inifilebytearray }
                };

            DateTime currentDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            string format = currentDate.ToString("MMddHHmm");
            string path = tenantPoco.VatNumber + "." + currentDate.Year.ToString().Substring(2, 2) + @"\" + format + "/";
            byte[] zippedfiles = ZipFiles(files, path);

            TaxesApproval.TaxReport reportdataProvider = this.BuildTaxReportFile(date1, date2, payments, invoices, apInvoices, tenantPoco);

            ReportRepository rep = new ReportRepository(tenant);
            Report reportPOc = rep.GetReportByCode("ROPF", tenant);

            ReportsTemplatesWebService reportsTemplatesWebService = new ReportsTemplatesWebService();
            ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(tenant);
            string reportDocumentId = reportsTemplatesVersionRepository.GetReportDocumentIdByReportTemplateId(reportPOc.DefaultTemplateId, tenant);
            byte[] reportTemplate = reportsTemplatesWebService.GetReportTemplate(reportDocumentId, tenant, false);

            if (reportTemplate == null)
            {
                throw new Exception("No report template found : Tax Open Format");
            }
            else
            {
                StiReport report = new StiReport();
                StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "TaxReport", Name = "TaxReport", BusinessObjectValue = reportdataProvider };
                report.RegBusinessObject(currentBusinessObject.Category, currentBusinessObject.Name, currentBusinessObject.BusinessObjectValue);
                report.Load(reportTemplate);
                report.Render(false);
                MemoryStream memStream = new MemoryStream();
                StiPdfExportSettings pdfSettings = new StiPdfExportSettings();
                pdfSettings.ImageResolution = 300;
                pdfSettings.ImageQuality = 100;
                pdfSettings.ImageCompressionMethod = StiPdfImageCompressionMethod.Jpeg;
                report.ExportDocument(StiExportFormat.Pdf, memStream, pdfSettings);

                string folder = reportdataProvider.FileSavingDirectory.Replace(@"C:\OPENFRMT\", "");
                string[] foldername = folder.Split(new[] { "\\" }, StringSplitOptions.None);

                Attachment att1 = new Attachment(new MemoryStream(zippedfiles), "OpenFormat.zip");
                memStream.Seek(0, SeekOrigin.Begin);
                Attachment att2 = new Attachment(memStream, "TaxReport.pdf");

                StringBuilder HtmlTemplate = new StringBuilder();
                HtmlTemplate.Append("<p style='text-align:left;font-family:Calibri;margin-left:15px'>");
                HtmlTemplate.Append("Hi,");
                HtmlTemplate.Append("<br /><br />");
                HtmlTemplate.Append("Open Format files was processed and built for: " + tenantPoco.Company + " / " + tenantPoco.VatNumber + "");
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("For the financial data between the dates: " + date1.ToShortDateString() + " – " + date2.ToShortDateString());
                HtmlTemplate.Append("<br /><br />");
                HtmlTemplate.Append("Attached 2 files:");
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("&ensp;&#9642; A Zip file that contains:");
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("&ensp;&ensp;&ensp;&bull; BKMVDATA.txt");
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("&ensp;&ensp;&ensp;&bull; INI.txt");
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("&ensp;&#9642; A PDF report that summarizes the process results");
                HtmlTemplate.Append("<br /><br />");
                HtmlTemplate.Append(@"Please extract the Zip content to the following path C:\OPENFRMT\");
                HtmlTemplate.Append("<br /><br />");
                HtmlTemplate.Append("Regards,");
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("Logitude World Team;");
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("<img width='258' height='101' src='cid:logo0' />");

                EmailParameters parameters = new EmailParameters()
                {
                    From = SettingUtil.Emails.FromNoReply,
                    To = email,
                     Subject = "Open Format Files / Amital Data",
                    Body = HtmlTemplate.ToString(),
                    IsBodyHtml = true,
                    Attachments = new List<Attachment>() { att1, att2 },
                    SentByUser = null,
                    Tenant = tenant,
                    EmailView = System.Net.Mime.MediaTypeNames.Text.Html,
                };
                EmailingHelper.SendEmail(parameters);

                EmailCommunicationParams emailParams = new EmailCommunicationParams()
                {
                    From = SettingUtil.Emails.FromNoReply,
                    To = email,
                    Subject = "Open Format Files / Amital Data",
                    Tenant = tenant,
                    EmailBody = HtmlTemplate.ToString(),
                    IsBodySecured = true,
                };

                Communications.AddEmailCommunicationLogQueue(emailParams, tenant);
            }
        }

        private string Compute(DateTime date1, DateTime date2, List<ARPayment> payments, List<ARInvoice> invoices, List<APInvoice> apInvoices, Tenant tenant, IInvoiceContext invoiceCotnext, string random, string username)
        {
            ARInvoiceLineRepository invoiceLineRep = new ARInvoiceLineRepository(invoiceCotnext);
            ARInvoiceTotalVATRepository totalVatRep = new ARInvoiceTotalVATRepository(invoiceCotnext);
            APInvoiceLineRepository apInvoiceLineRep = new APInvoiceLineRepository(invoiceCotnext);
            APInvoiceTotalVATRepository apTotalVatRep = new APInvoiceTotalVATRepository(invoiceCotnext);

            CardRepository cardRep = new CardRepository(tenant.Id);
            List<Card> allCards = cardRep.GetCards(tenant.Id).ToList();
            List<Card> myCards = new List<Card>();
            Card billTo = null;
            foreach (ARInvoice item in invoices)
            {
                billTo = allCards.Where(d => d.Id == item.BillToId).FirstOrDefault();

                if (billTo != null)
                {
                    myCards.Add(billTo);
                }
            }

            foreach (APInvoice item in apInvoices)
            {
                billTo = allCards.Where(d => d.Id == item.VendorId).FirstOrDefault();

                if (billTo != null)
                {
                    myCards.Add(billTo);
                }
            }

            foreach (ARPayment item in payments)
            {
                billTo = allCards.Where(d => d.Id == item.BillToId).FirstOrDefault();

                if (billTo != null)
                {
                    myCards.Add(billTo);
                }
            }

            myCards = myCards.Distinct().ToList();

            StringBuilder main = new StringBuilder();
            int counter = 0;
            int apCounter = 0;

            counter += invoices.Count;
            counter += apInvoices.Count;
            counter += (payments.Count) * 2;
            counter += myCards.Count;

            apCounter += invoices.Count;
            apCounter += (payments.Count) * 2;

            IQueryable<ARInvoiceLine> ARLines = invoiceLineRep.GetInvoiceLinesByTenant(tenant.Id);
            IQueryable<APInvoiceLine> APLines = apInvoiceLineRep.GetAPInvoiceLinesByTenant(tenant.Id);

            List<string> ARInvoiceIds = invoices.Select(s => s.Id).ToList();
            List<string> APInvoiceIds = apInvoices.Select(s => s.Id).ToList();

            counter += ARLines.Where(d => ARInvoiceIds.Contains(d.ARInvoiceId)).Count();
            apCounter += ARLines.Where(d => ARInvoiceIds.Contains(d.ARInvoiceId)).Count();
            counter += APLines.Where(d => APInvoiceIds.Contains(d.APInvoiceId)).Count();

            main.AppendLine(BuildA100Line(tenant, random));
            main.AppendLine(BuildB110Line(invoices, apInvoices, payments, tenant, myCards));
            main.AppendLine(BuildC100PaymentLines(myCards, payments, tenant, username));
            main.AppendLine(BuildC100InvoiceLines(((payments.Count) * 2 + myCards.Count) + 1, invoices, tenant, invoiceLineRep, totalVatRep, username, myCards));
            main.AppendLine(BuildC100APInvoiceLines((apCounter + 1 + myCards.Count), apInvoices, tenant, apInvoiceLineRep, apTotalVatRep, username, myCards));
            main.AppendLine(BuildZ900Line((counter + 2), tenant, random));

            return Regex.Replace(main.ToString(), @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
        }

        private string BuildA100Line(Tenant tenant, string random)
        {
            TaxesApproval.A100 a100 = new TaxesApproval.A100();

            a100.RecordCode = "A100";
            a100.RecordLineNumber = "1";

            if (!string.IsNullOrEmpty(tenant.VatNumber))
            {
                if (tenant.VatNumber.Length > 9)
                {
                    a100.TenantVatNumber = CheckVATValidation(tenant.VatNumber.Substring(0, 9));
                }
                else
                {
                    a100.TenantVatNumber = CheckVATValidation(tenant.VatNumber);
                }
            }
            else
            {
                a100.TenantVatNumber = "";
            }

            a100.PrimaryId = random;
            a100.SystemConst = "&OF1.31&";
            a100.FutureUsage = "";

            StringBuilder str = new StringBuilder(95);

            str.Append(a100.RecordCode.PadRight(4));
            str.Append(a100.RecordLineNumber.PadLeft(9, '0'));

            if (a100.TenantVatNumber != null)
            {
                str.Append(a100.TenantVatNumber.PadLeft(9, '0'));
            }
            else
            {
                str.Append(' ', 9);
            }

            str.Append(a100.PrimaryId.PadRight(15));
            str.Append(a100.SystemConst.PadRight(8));
            str.Append(a100.FutureUsage.PadRight(50));

            return str.ToString();
        }

        private string BuildB110Line(List<ARInvoice> arInvoices, List<APInvoice> apInvoices, List<ARPayment> arPayments, Tenant tenant, List<Card> myCards)
        {
            AddressRepository addressRep = new AddressRepository(tenant.Id);
            CurrencyRepository currencyRep = new CurrencyRepository(tenant.Id);
            PartnerTypeRepository typeRep = new PartnerTypeRepository(tenant.Id);

            List<string> codes = new List<string>();
            StringBuilder str = new StringBuilder(376);
            int i = 2;

            foreach (ARInvoice item in arInvoices)
            {
                TaxesApproval.B110 b110 = new TaxesApproval.B110();
                Card card = myCards.Where(d => d.Id == item.BillToId).FirstOrDefault();
                Address address = addressRep.GetSingleAddress(item.BillToAddressId, tenant.Id);
                PartnerType type = typeRep.GetSinglePartnerType(card.PartnerTypeId);

                if (!codes.Contains(card.Code))
                {
                    codes.Add(card.Code);

                    b110.RecordCode = "B110";
                    b110.RecordLineNumber = i++.ToString();
                    b110.EntityTypeCode = card != null ? card.PartnerTypeId : "";
                    b110.EntityTypeName = type != null ? type.Name : "";
                    b110.HouseNumber = "";
                    b110.BranchId = "";
                    b110.BalanceInForeignCurrency = "+00000000000000";
                    b110.ForeignCurrencyCode = "";
                    b110.FutureUsage = "";
                    b110.Field1413 = "";
                    b110.BalanceOpeningDate = "+00000000000000";
                    b110.TotalDebit = "+00000000000000";
                    b110.TotalCredit = "+00000000000000";
                    b110.Field1417 = "";

                    if (address != null)
                    {
                        //Address 1
                        if (!string.IsNullOrEmpty(address.Address1))
                        {
                            if (address.Address1.Length > 50)
                            {
                                b110.Address1 = address.Address1.Substring(0, 50);
                            }
                            else
                            {
                                b110.Address1 = address.Address1;
                            }
                        }
                        else
                        {
                            b110.Address1 = "";
                        }

                        //City
                        if (!string.IsNullOrEmpty(address.City))
                        {
                            if (address.City.Length > 30)
                            {
                                b110.City = address.City.Substring(0, 30);
                            }
                            else
                            {
                                b110.City = address.City;
                            }
                        }
                        else
                        {
                            b110.City = "";
                        }

                        //Zip Code
                        if (!string.IsNullOrEmpty(address.ZipCode))
                        {
                            if (address.ZipCode.Length > 8)
                            {
                                b110.ZipCode = address.ZipCode.Substring(0, 8);
                            }
                            else
                            {
                                b110.ZipCode = address.ZipCode;
                            }
                        }
                        else
                        {
                            b110.ZipCode = "";
                        }

                        //Country Name
                        if (address.Country != null)
                        {
                            if (address.Country.EnglishName.Length > 30)
                            {
                                b110.Country = address.Country.EnglishName.Substring(0, 30);
                            }
                            else
                            {
                                b110.Country = address.Country.EnglishName;
                            }
                        }
                        else
                        {
                            b110.Country = "";
                        }

                        //Country Code
                        if (address.Country != null)
                        {
                            b110.CountryCode = address.Country.Code;
                        }
                        else
                        {
                            b110.CountryCode = "";
                        }
                    }

                    else
                    {
                        b110.Address1 = "";
                        b110.City = "";
                        b110.ZipCode = "";
                        b110.Country = "";
                        b110.CountryCode = "";
                    }

                    if (!string.IsNullOrEmpty(tenant.VatNumber))
                    {
                        if (tenant.VatNumber.Length > 9)
                        {
                            b110.TenantVatNumber = CheckVATValidation(tenant.VatNumber.Substring(0, 9));
                        }
                        else
                        {
                            b110.TenantVatNumber = CheckVATValidation(tenant.VatNumber);
                        }
                    }
                    else
                    {
                        b110.TenantVatNumber = "";
                    }

                    b110.CardId = card != null ? card.Code : "";

                    if (card != null)
                    {
                        if (card.EnglishName.Length > 50)
                        {
                            b110.CustomerName = card.EnglishName.Substring(0, 50);
                        }
                        else
                        {
                            b110.CustomerName = card.EnglishName;
                        }
                    }
                    else
                    {
                        b110.CustomerName = "";
                    }

                    if (card != null)
                    {
                        if (!string.IsNullOrEmpty(card.VatNumber))
                        {
                            if (card.VatNumber.Length > 9)
                            {
                                b110.CustomerVAT = CheckVATValidation(card.VatNumber.Substring(0, 9));
                            }
                            else
                            {
                                b110.CustomerVAT = CheckVATValidation(card.VatNumber);
                            }
                        }
                        else
                        {
                            b110.CustomerVAT = "";
                        }
                    }
                    else
                    {
                        b110.CustomerVAT = "";
                    }

                    str.Append(b110.RecordCode.PadRight(4));
                    str.Append(b110.RecordLineNumber.PadLeft(9, '0'));
                    str.Append(b110.TenantVatNumber.PadLeft(9, '0'));
                    str.Append(b110.CardId.PadLeft(15));
                    str.Append(b110.CustomerName.PadRight(50));
                    str.Append(b110.EntityTypeCode.PadRight(15));
                    str.Append(b110.EntityTypeName.PadRight(30));
                    str.Append(b110.Address1.PadRight(50));
                    str.Append(b110.HouseNumber.PadRight(10));
                    str.Append(b110.City.PadRight(30));
                    str.Append(b110.ZipCode.PadRight(8));
                    str.Append(b110.Country.PadRight(30));
                    str.Append(b110.CountryCode.PadRight(2));
                    str.Append(b110.Field1413.PadRight(15));
                    str.Append(b110.BalanceOpeningDate.PadRight(15));
                    str.Append(b110.TotalDebit);
                    str.Append(b110.TotalCredit);
                    str.Append(b110.Field1417.PadLeft(4, '0'));
                    str.Append(b110.CustomerVAT.PadLeft(9, '0'));
                    str.Append(b110.BranchId.PadRight(7));
                    str.Append(b110.BalanceInForeignCurrency);
                    str.Append(b110.ForeignCurrencyCode.PadRight(3));
                    str.Append(b110.FutureUsage.PadRight(16));
                    str.AppendLine();
                }
            }

            foreach (APInvoice item in apInvoices)
            {
                TaxesApproval.B110 b110 = new TaxesApproval.B110();
                Card card = myCards.Where(d => d.Id == item.VendorId).FirstOrDefault();
                Address address = addressRep.GetMainAddressByCardId(item.VendorId, tenant.Id);
                PartnerType type = typeRep.GetSinglePartnerType(card.PartnerTypeId);

                if (!codes.Contains(card.Code))
                {
                    codes.Add(card.Code);

                    b110.RecordCode = "B110";
                    b110.RecordLineNumber = i++.ToString();
                    b110.CardId = card != null ? card.Code : "";
                    b110.CustomerName = card != null ? card.EnglishName : "";
                    b110.EntityTypeCode = card != null ? card.PartnerTypeId : "";
                    b110.EntityTypeName = type != null ? type.Name : "";
                    b110.HouseNumber = "";
                    b110.Field1413 = "";
                    b110.BalanceOpeningDate = "+00000000000000";
                    b110.TotalDebit = "+00000000000000";
                    b110.TotalCredit = "+00000000000000";
                    b110.Field1417 = "";
                    b110.BranchId = "";
                    b110.BalanceInForeignCurrency = "+00000000000000";
                    b110.ForeignCurrencyCode = "";
                    b110.FutureUsage = "";

                    if (!string.IsNullOrEmpty(tenant.VatNumber))
                    {
                        if (tenant.VatNumber.Length > 9)
                        {
                            b110.TenantVatNumber = tenant.VatNumber.Substring(0, 9);
                        }
                        else
                        {
                            b110.TenantVatNumber = tenant.VatNumber;
                        }
                    }
                    else
                    {
                        b110.TenantVatNumber = "";
                    }

                    if (address != null)
                    {
                        //Address 1
                        if (!string.IsNullOrEmpty(address.Address1))
                        {
                            if (address.Address1.Length > 50)
                            {
                                b110.Address1 = address.Address1.Substring(0, 50);
                            }
                            else
                            {
                                b110.Address1 = address.Address1;
                            }
                        }
                        else
                        {
                            b110.Address1 = "";
                        }

                        //City
                        if (!string.IsNullOrEmpty(address.City))
                        {
                            if (address.City.Length > 30)
                            {
                                b110.City = address.City.Substring(0, 30);
                            }
                            else
                            {
                                b110.City = address.City;
                            }
                        }
                        else
                        {
                            b110.City = "";
                        }

                        //Zip Code
                        if (!string.IsNullOrEmpty(address.ZipCode))
                        {
                            if (address.ZipCode.Length > 8)
                            {
                                b110.ZipCode = address.ZipCode.Substring(0, 8);
                            }
                            else
                            {
                                b110.ZipCode = address.ZipCode;
                            }
                        }
                        else
                        {
                            b110.ZipCode = "";
                        }

                        //Country Name
                        if (address.Country != null)
                        {
                            if (address.Country.EnglishName.Length > 30)
                            {
                                b110.Country = address.Country.EnglishName.Substring(0, 30);
                            }
                            else
                            {
                                b110.Country = address.Country.EnglishName;
                            }
                        }
                        else
                        {
                            b110.Country = "";
                        }

                        //Country Code
                        if (address.Country != null)
                        {
                            b110.CountryCode = address.Country.Code;
                        }
                        else
                        {
                            b110.CountryCode = "";
                        }
                    }

                    else
                    {
                        b110.Address1 = "";
                        b110.City = "";
                        b110.ZipCode = "";
                        b110.Country = "";
                        b110.CountryCode = "";
                    }

                    if (card != null)
                    {
                        if (!string.IsNullOrEmpty(card.VatNumber))
                        {
                            if (card.VatNumber.Length > 9)
                            {
                                b110.CustomerVAT = card.VatNumber.Substring(0, 9);
                            }
                            else
                            {
                                b110.CustomerVAT = card.VatNumber;
                            }
                        }
                        else
                        {
                            b110.CustomerVAT = "";
                        }
                    }
                    else
                    {
                        b110.CustomerVAT = "";
                    }

                    str.Append(b110.RecordCode.PadRight(4));
                    str.Append(b110.RecordLineNumber.PadLeft(9, '0'));
                    str.Append(b110.TenantVatNumber.PadLeft(9, '0'));
                    str.Append(b110.CardId.PadLeft(15));
                    str.Append(b110.CustomerName.PadRight(50));
                    str.Append(b110.EntityTypeCode.PadRight(15));
                    str.Append(b110.EntityTypeName.PadRight(30));
                    str.Append(b110.Address1.PadRight(50));
                    str.Append(b110.HouseNumber.PadRight(10));
                    str.Append(b110.City.PadRight(30));
                    str.Append(b110.ZipCode.PadRight(8));
                    str.Append(b110.Country.PadRight(30));
                    str.Append(b110.CountryCode.PadRight(2));
                    str.Append(b110.Field1413.PadRight(15));
                    str.Append(b110.BalanceOpeningDate.PadRight(15));
                    str.Append(b110.TotalDebit);
                    str.Append(b110.TotalCredit);
                    str.Append(b110.Field1417.PadLeft(4, '0'));
                    str.Append(b110.CustomerVAT.PadLeft(9, '0'));
                    str.Append(b110.BranchId.PadRight(7));
                    str.Append(b110.BalanceInForeignCurrency);
                    str.Append(b110.ForeignCurrencyCode.PadRight(3));
                    str.Append(b110.FutureUsage.PadRight(16));
                    str.AppendLine();
                }
            }

            foreach (ARPayment item in arPayments)
            {
                TaxesApproval.B110 b110 = new TaxesApproval.B110();
                Card card = myCards.Where(d => d.Id == item.BillToId).FirstOrDefault();
                Address address = addressRep.GetSingleAddress(item.BillToAddressId, tenant.Id);
                PartnerType type = typeRep.GetSinglePartnerType(card.PartnerTypeId);

                if (!codes.Contains(card.Code))
                {
                    codes.Add(card.Code);

                    b110.RecordCode = "B110";
                    b110.RecordLineNumber = i++.ToString();
                    b110.CardId = card != null ? card.Code : "";
                    b110.CustomerName = card != null ? card.EnglishName : "";
                    b110.EntityTypeCode = card != null ? card.PartnerTypeId : "";
                    b110.EntityTypeName = type != null ? type.Name : "";
                    b110.HouseNumber = "";
                    b110.Field1413 = "";
                    b110.BalanceOpeningDate = "+00000000000000";
                    b110.TotalDebit = "+00000000000000";
                    b110.TotalCredit = "+00000000000000";
                    b110.Field1417 = "";
                    b110.BranchId = "";
                    b110.BalanceInForeignCurrency = "+00000000000000";
                    b110.ForeignCurrencyCode = "";
                    b110.FutureUsage = "";

                    if (!string.IsNullOrEmpty(tenant.VatNumber))
                    {
                        if (tenant.VatNumber.Length > 9)
                        {
                            b110.TenantVatNumber = tenant.VatNumber.Substring(0, 9);
                        }
                        else
                        {
                            b110.TenantVatNumber = tenant.VatNumber;
                        }
                    }
                    else
                    {
                        b110.TenantVatNumber = "";
                    }

                    if (address != null)
                    {
                        //Address 1
                        if (!string.IsNullOrEmpty(address.Address1))
                        {
                            if (address.Address1.Length > 50)
                            {
                                b110.Address1 = address.Address1.Substring(0, 50);
                            }
                            else
                            {
                                b110.Address1 = address.Address1;
                            }
                        }
                        else
                        {
                            b110.Address1 = "";
                        }

                        //City
                        if (!string.IsNullOrEmpty(address.City))
                        {
                            if (address.City.Length > 30)
                            {
                                b110.City = address.City.Substring(0, 30);
                            }
                            else
                            {
                                b110.City = address.City;
                            }
                        }
                        else
                        {
                            b110.City = "";
                        }

                        //Zip Code
                        if (!string.IsNullOrEmpty(address.ZipCode))
                        {
                            if (address.ZipCode.Length > 8)
                            {
                                b110.ZipCode = address.ZipCode.Substring(0, 8);
                            }
                            else
                            {
                                b110.ZipCode = address.ZipCode;
                            }
                        }
                        else
                        {
                            b110.ZipCode = "";
                        }

                        //Country Name
                        if (address.Country != null)
                        {
                            if (address.Country.EnglishName.Length > 30)
                            {
                                b110.Country = address.Country.EnglishName.Substring(0, 30);
                            }
                            else
                            {
                                b110.Country = address.Country.EnglishName;
                            }
                        }
                        else
                        {
                            b110.Country = "";
                        }

                        //Country Code
                        if (address.Country != null)
                        {
                            b110.CountryCode = address.Country.Code;
                        }
                        else
                        {
                            b110.CountryCode = "";
                        }
                    }

                    else
                    {
                        b110.Address1 = "";
                        b110.City = "";
                        b110.ZipCode = "";
                        b110.Country = "";
                        b110.CountryCode = "";
                    }

                    if (card != null)
                    {
                        if (!string.IsNullOrEmpty(card.VatNumber))
                        {
                            if (card.VatNumber.Length > 9)
                            {
                                b110.CustomerVAT = card.VatNumber.Substring(0, 9);
                            }
                            else
                            {
                                b110.CustomerVAT = card.VatNumber;
                            }
                        }
                        else
                        {
                            b110.CustomerVAT = "";
                        }
                    }
                    else
                    {
                        b110.CustomerVAT = "";
                    }

                    str.Append(b110.RecordCode.PadRight(4));
                    str.Append(b110.RecordLineNumber.PadLeft(9, '0'));
                    str.Append(b110.TenantVatNumber.PadLeft(9, '0'));
                    str.Append(b110.CardId.PadLeft(15));
                    str.Append(b110.CustomerName.PadRight(50));
                    str.Append(b110.EntityTypeCode.PadRight(15));
                    str.Append(b110.EntityTypeName.PadRight(30));
                    str.Append(b110.Address1.PadRight(50));
                    str.Append(b110.HouseNumber.PadRight(10));
                    str.Append(b110.City.PadRight(30));
                    str.Append(b110.ZipCode.PadRight(8));
                    str.Append(b110.Country.PadRight(30));
                    str.Append(b110.CountryCode.PadRight(2));
                    str.Append(b110.Field1413.PadRight(15));
                    str.Append(b110.BalanceOpeningDate.PadRight(15));
                    str.Append(b110.TotalDebit);
                    str.Append(b110.TotalCredit);
                    str.Append(b110.Field1417.PadLeft(4, '0'));
                    str.Append(b110.CustomerVAT.PadLeft(9, '0'));
                    str.Append(b110.BranchId.PadRight(7));
                    str.Append(b110.BalanceInForeignCurrency);
                    str.Append(b110.ForeignCurrencyCode.PadRight(3));
                    str.Append(b110.FutureUsage.PadRight(16));
                    str.AppendLine();
                }
            }

            return str.ToString();
        }

        private string BuildC100PaymentLines(List<Card> myCards, List<ARPayment> payments, Tenant tenant, string username)
        {
            int cardsCount = myCards.Count + 1;

            AddressRepository addressRep = new AddressRepository(tenant.Id);
            CurrencyRepository currencyRep = new CurrencyRepository(tenant.Id);

            StringBuilder str = new StringBuilder(444);
            int i = cardsCount + 1;

            foreach (ARPayment item in payments)
            {
                Address billToAddress = addressRep.GetSingleAddress(item.BillToAddressId, tenant.Id);
                Card billTo = myCards.Where(d => d.Id == item.BillToId).FirstOrDefault();
                Currency paymentCurrency = currencyRep.GetSingleCurrency(item.PaymentCurrencyId, tenant.Id);
                TaxesApproval.C100 c100 = new TaxesApproval.C100();

                c100.RecordCode = "C100";
                c100.RecordLineNumber = i++.ToString();
                c100.DocumentType = "400";
                c100.DocumentNumber = item.PaymentNo;
                c100.DocumentCreateDate = String.Format("{0:yyyyMMdd}", item.CreateDate);
                c100.DocumentCreateHour = String.Format("{0:hhmm}", item.CreateDate);
                c100.BillToName = billTo != null ? billTo.EnglishName : "";
                c100.BillToAddress_HouseNumber = "";
                c100.ValueDate = String.Format("{0:yyyyMMdd}", item.RegisterDate);
                c100.TotalAmount = paymentCurrency.Code != "NIS" ? (item.AmountInLocalCurrency != null ? String.Format("{0:+000000000000.00}", item.AmountInLocalCurrency.Value).Replace(".", "") : "+00000000000000") : "+00000000000000";
                c100.CurrencyCode = paymentCurrency != null ? (paymentCurrency.Code != "NIS" ? paymentCurrency.Code : "") : "";
                c100.AmountBeforeDiscount = String.Format("{0:+000000000000.00}", item.AmountInLocalCurrency.Value).Replace(".", "");
                c100.Discount = "+00000000000000";
                c100.SubTotal = String.Format("{0:+000000000000.00}", item.AmountInLocalCurrency.Value).Replace(".", "");
                c100.VAT = "+00000000000000";
                c100.GrandTotal = String.Format("{0:+000000000000.00}", item.AmountInLocalCurrency.Value).Replace(".", "");
                c100.Field1224 = "";
                c100.BillToId = billTo != null ? billTo.Code : "";
                c100.Field1226 = "";
                c100.Void_CancelledDocument = item.IsClosed ? "1" : "";
                c100.DocumentDate = String.Format("{0:yyyyMMdd}", item.RegisterDate);
                c100.BranchId = "";
                c100.LinkingField = "0000000";
                c100.FutureUsage = "";

                if (!string.IsNullOrEmpty(tenant.VatNumber))
                {
                    if (tenant.VatNumber.Length > 9)
                    {
                        c100.TenantVatNumber = CheckVATValidation(tenant.VatNumber.Substring(0, 9));
                    }
                    else
                    {
                        c100.TenantVatNumber = CheckVATValidation(tenant.VatNumber);
                    }
                }
                else
                {
                    c100.TenantVatNumber = "";
                }

                if (billToAddress != null)
                {
                    //Address 1 - street
                    if (!string.IsNullOrEmpty(billToAddress.Address1))
                    {
                        if (billToAddress.Address1.Length > 50)
                        {
                            c100.BillToAddress_Street = billToAddress.Address1.Substring(0, 50);
                        }
                        else
                        {
                            c100.BillToAddress_Street = billToAddress.Address1;
                        }
                    }
                    else
                    {
                        c100.BillToAddress_Street = "";
                    }

                    //City
                    if (!string.IsNullOrEmpty(billToAddress.City))
                    {
                        if (billToAddress.City.Length > 30)
                        {
                            c100.BillToAddress_City = billToAddress.City.Substring(0, 30);
                        }
                        else
                        {
                            c100.BillToAddress_City = billToAddress.City;
                        }
                    }
                    else
                    {
                        c100.BillToAddress_City = "";
                    }

                    //Zip Code
                    if (!string.IsNullOrEmpty(billToAddress.ZipCode))
                    {
                        if (billToAddress.ZipCode.Length > 8)
                        {
                            c100.BillToAddress_ZipCode = billToAddress.ZipCode.Substring(0, 8);
                        }
                        else
                        {
                            c100.BillToAddress_ZipCode = billToAddress.ZipCode;
                        }
                    }
                    else
                    {
                        c100.BillToAddress_ZipCode = "";
                    }

                    //Country Name
                    if (billToAddress.Country != null)
                    {
                        if (billToAddress.Country.EnglishName.Length > 30)
                        {
                            c100.BillToAddress_CountryName = billToAddress.Country.EnglishName.Substring(0, 30);
                        }
                        else
                        {
                            c100.BillToAddress_CountryName = billToAddress.Country.EnglishName;
                        }
                    }
                    else
                    {
                        c100.BillToAddress_CountryName = "";
                    }

                    //Country Code
                    if (billToAddress.Country != null)
                    {
                        c100.BillToAddress_CountryCode = billToAddress.Country.Code;
                    }
                    else
                    {
                        c100.BillToAddress_CountryCode = "";
                    }

                    //Phone
                    if (!string.IsNullOrEmpty(billToAddress.PhoneNumber))
                    {
                        if (billToAddress.PhoneNumber.Length > 15)
                        {
                            c100.BillToAddress_Telephone = billToAddress.PhoneNumber.Substring(0, 15);
                        }
                        else
                        {
                            c100.BillToAddress_Telephone = billToAddress.PhoneNumber;
                        }
                    }
                    else
                    {
                        c100.BillToAddress_Telephone = "";
                    }
                }

                else
                {
                    c100.BillToAddress_Street = "";
                    c100.BillToAddress_City = "";
                    c100.BillToAddress_ZipCode = "";
                    c100.BillToAddress_CountryName = "";
                    c100.BillToAddress_CountryCode = "";
                    c100.BillToAddress_Telephone = "";
                }

                if (billTo != null)
                {
                    if (!string.IsNullOrEmpty(billTo.VatNumber))
                    {
                        if (billTo.VatNumber.Length > 9)
                        {
                            c100.BillToVAT = CheckVATValidation(billTo.VatNumber.Substring(0, 9));
                        }
                        else
                        {
                            c100.BillToVAT = CheckVATValidation(billTo.VatNumber);
                        }
                    }
                    else
                    {
                        c100.BillToVAT = "";
                    }
                }
                else
                {
                    c100.BillToVAT = "";
                }

                if (!string.IsNullOrEmpty(username))
                {
                    if (username.Length > 9)
                    {
                        c100.UserName = username.Substring(0, 9);
                    }
                    else
                    {
                        c100.UserName = username;
                    }
                }
                else
                {
                    c100.UserName = "";
                }

                str.Append(c100.RecordCode.PadRight(4));
                str.Append(c100.RecordLineNumber.PadLeft(9, '0'));
                str.Append(c100.TenantVatNumber.PadLeft(9, '0'));
                str.Append(c100.DocumentType.PadLeft(3, '0'));
                str.Append(c100.DocumentNumber.PadRight(20));
                str.Append(c100.DocumentCreateDate.PadLeft(8, '0'));
                str.Append(c100.DocumentCreateHour.PadLeft(4, '0'));
                str.Append(c100.BillToName.PadRight(50));
                str.Append(c100.BillToAddress_Street.PadRight(50));
                str.Append(c100.BillToAddress_HouseNumber.PadRight(10));
                str.Append(c100.BillToAddress_City.PadRight(30));
                str.Append(c100.BillToAddress_ZipCode.PadRight(8));
                str.Append(c100.BillToAddress_CountryName.PadRight(30));
                str.Append(c100.BillToAddress_CountryCode.PadRight(2));
                str.Append(c100.BillToAddress_Telephone.PadRight(15));
                str.Append(c100.BillToVAT.PadLeft(9, '0'));
                str.Append(c100.ValueDate.PadLeft(8, '0'));
                str.Append(c100.TotalAmount);
                str.Append(c100.CurrencyCode.PadRight(3));
                str.Append(c100.AmountBeforeDiscount);
                str.Append(c100.Discount);
                str.Append(c100.SubTotal.PadRight(15));
                str.Append(c100.VAT);
                str.Append(c100.GrandTotal);
                str.Append(c100.Field1224.PadRight(12));
                str.Append(c100.BillToId.PadRight(15));
                str.Append(c100.Field1224.PadRight(10));
                str.Append(c100.Void_CancelledDocument.PadRight(1));
                str.Append(c100.DocumentDate.PadLeft(8, '0'));
                str.Append(c100.BranchId.PadRight(7));
                str.Append(c100.UserName.PadRight(9));
                str.Append(c100.LinkingField.PadRight(7));
                str.Append(c100.FutureUsage.PadRight(13));
                str.AppendLine();

                str.Append(BuildD120Lines(i++, item, tenant));
            }

            return str.ToString();
        }

        private string BuildD120Lines(int count, ARPayment payment, Tenant tenant)
        {
            StringBuilder str = new StringBuilder(222);

            TaxesApproval.D120 d120 = new TaxesApproval.D120();

            string paymentMethodCode = "";
            AccountingPaymentMethodRepository repository = new AccountingPaymentMethodRepository(tenant.Id);
            AccountingPaymentMethod method = repository.GetSingleAccountingPaymentMethod(payment.AccountingPaymentMethodId, tenant.Id);
            if (method != null)
            {
                paymentMethodCode = method.Code;
            }

            d120.RecordCode = "D120";
            d120.RecordLineNumber = count.ToString();

            if (!string.IsNullOrEmpty(tenant.VatNumber))
            {
                if (tenant.VatNumber.Length > 9)
                {
                    d120.TenantVatNumber = CheckVATValidation(tenant.VatNumber.Substring(0, 9));
                }
                else
                {
                    d120.TenantVatNumber = CheckVATValidation(tenant.VatNumber);
                }
            }
            else
            {
                d120.TenantVatNumber = "";
            }

            d120.DocumentType = "400";
            d120.DocumentNumber = payment.PaymentNo;
            d120.EntityLineNumber = "0001";

            if (paymentMethodCode == "CA")
            {
                d120.PaymentMethod = "1";
            }
            else if (paymentMethodCode == "CH")
            {
                d120.PaymentMethod = "2";
            }
            else if (paymentMethodCode == "CC")
            {
                d120.PaymentMethod = "3";
            }
            else if (paymentMethodCode == "BT")
            {
                d120.PaymentMethod = "4";
            }
            else
            {
                d120.PaymentMethod = "9";
            }

            /////////////////////////////
            if (paymentMethodCode == "CH")
            {
                double Num;

                bool isBankNum = double.TryParse(payment.Bank, out Num);
                bool isBranchNum = double.TryParse(payment.BankBranch, out Num);
                bool isAccountNum = double.TryParse(payment.Account, out Num);

                if (isBankNum && isBranchNum && isAccountNum)
                {
                    //bank
                    if (payment.Bank.Length > 10)
                    {
                        d120.BankNumber = payment.Bank.Substring(0, 10);
                    }
                    else
                    {
                        d120.BankNumber = payment.Bank;
                    }

                    //branch
                    if (payment.BankBranch.Length > 10)
                    {
                        d120.BranchNumber = payment.BankBranch.Substring(0, 9);
                    }
                    else
                    {
                        d120.BranchNumber = payment.BankBranch;
                    }

                    //account
                    if (payment.Account.Length > 15)
                    {
                        d120.AccountBunber = payment.Account.Substring(0, 5);
                    }
                    else
                    {
                        d120.AccountBunber = payment.Account;
                    }

                    //ChequeOrPaymentRef
                    if (!string.IsNullOrEmpty(payment.ChequeOrPaymentRef))
                    {
                        if (payment.ChequeOrPaymentRef.Length > 10)
                        {
                            d120.ChequeNumber = payment.ChequeOrPaymentRef.Substring(0, 0);
                        }
                        else
                        {
                            d120.ChequeNumber = payment.ChequeOrPaymentRef;
                        }
                    }
                    else
                    {
                        d120.ChequeNumber = "";
                    }

                    d120.PaymentDate = String.Format("{0:yyyyMMdd}", payment.RegisterDate);
                }
                else
                {
                    d120.PaymentMethod = "9";
                    d120.BankNumber = "";
                    d120.BranchNumber = "";
                    d120.AccountBunber = "";
                    d120.ChequeNumber = "";
                    d120.PaymentDate = "";
                }
            }
            else // not CH
            {
                if (!string.IsNullOrEmpty(payment.Bank))
                {
                    if (payment.Bank.Length > 10)
                    {
                        d120.BankNumber = payment.Bank.Substring(0, 9);
                    }
                    else
                    {
                        d120.BankNumber = payment.Bank;
                    }
                }
                else
                {
                    d120.BankNumber = "";
                }

                if (!string.IsNullOrEmpty(payment.BankBranch))
                {
                    if (payment.BankBranch.Length > 10)
                    {
                        d120.BranchNumber = payment.BankBranch.Substring(0, 9);
                    }
                    else
                    {
                        d120.BranchNumber = payment.BankBranch;
                    }
                }
                else
                {
                    d120.BranchNumber = "";
                }

                if (!string.IsNullOrEmpty(payment.Account))
                {
                    if (payment.Account.Length > 15)
                    {
                        d120.AccountBunber = payment.Account.Substring(0, 5);
                    }
                    else
                    {
                        d120.AccountBunber = payment.Account;
                    }
                }
                else
                {
                    d120.AccountBunber = "";
                }

                if (!string.IsNullOrEmpty(payment.ChequeOrPaymentRef))
                {
                    if (payment.ChequeOrPaymentRef.Length > 10)
                    {
                        d120.ChequeNumber = payment.ChequeOrPaymentRef.Substring(0, 0);
                    }
                    else
                    {
                        d120.ChequeNumber = payment.ChequeOrPaymentRef;
                    }
                }
                else
                {
                    d120.ChequeNumber = "";
                }

                d120.PaymentDate = String.Format("{0:yyyyMMdd}", payment.RegisterDate);
            }

            d120.Total = String.Format("{0:+000000000000.00}", payment.AmountInLocalCurrency.Value).Replace(".", "");
            d120.CreditCardCompany = "";
            d120.Field1314 = "";
            d120.CreditType = "";
            d120.Field1320 = "";
            d120.DocumentDate = String.Format("{0:yyyyMMdd}", payment.RegisterDate);
            d120.Field1323 = "0000000";
            d120.Field1324 = "";

            str.Append(d120.RecordCode.PadRight(4));
            str.Append(d120.RecordLineNumber.PadLeft(9, '0'));
            str.Append(d120.TenantVatNumber.PadLeft(9, '0'));
            str.Append(d120.DocumentType.PadLeft(3, '0'));
            str.Append(d120.DocumentNumber.PadRight(20));
            str.Append(d120.EntityLineNumber.PadLeft(4));
            str.Append(d120.PaymentMethod.PadRight(1));
            str.Append(d120.BankNumber.PadLeft(10, '0'));
            str.Append(d120.BranchNumber.PadLeft(10, '0'));
            str.Append(d120.AccountBunber.PadLeft(15, '0'));
            str.Append(d120.ChequeNumber.PadLeft(10, '0'));
            str.Append(d120.PaymentDate.PadLeft(8, '0'));
            str.Append(d120.Total);
            str.Append(d120.CreditCardCompany.PadRight(1));
            str.Append(d120.Field1314.PadRight(20));
            str.Append(d120.CreditType.PadRight(1));
            str.Append(d120.Field1320.PadRight(7));
            str.Append(d120.DocumentDate.PadLeft(8, '0'));
            str.Append(d120.Field1323.PadRight(7));
            str.Append(d120.Field1324.PadRight(60));

            str.AppendLine();

            return str.ToString();
        }

        private string BuildC100InvoiceLines(int count, List<ARInvoice> invoices, Tenant tenant, ARInvoiceLineRepository invoiceLineRep, ARInvoiceTotalVATRepository totalVatRep, string username, List<Card> myCards)
        {
            ContactQuery contactQuery = new ContactQuery(tenant.Id);
            AddressRepository addressRep = new AddressRepository(tenant.Id);
            CurrencyRepository currencyRep = new CurrencyRepository(tenant.Id);

            StringBuilder str = new StringBuilder(444);
            int i = count + 1;

            foreach (ARInvoice item in invoices)
            {
                Address billToAddress = addressRep.GetSingleAddress(item.BillToAddressId, tenant.Id);
                Card billTo = myCards.Where(d => d.Id == item.BillToId).FirstOrDefault();
                Currency invoiceCurrency = currencyRep.GetSingleCurrency(item.InvoiceCurrencyId, tenant.Id);
                List<ARInvoiceTotalVAT> totalVats = totalVatRep.GetInvoiceTotalVatsForInvoice(item.Id, tenant.Id).ToList();

                TaxesApproval.C100 c100 = new TaxesApproval.C100();

                c100.RecordCode = "C100";
                c100.RecordLineNumber = i++.ToString();

                if (!string.IsNullOrEmpty(tenant.VatNumber))
                {
                    if (tenant.VatNumber.Length > 9)
                    {
                        c100.TenantVatNumber = CheckVATValidation(tenant.VatNumber.Substring(0, 9));
                    }
                    else
                    {
                        c100.TenantVatNumber = CheckVATValidation(tenant.VatNumber);
                    }
                }
                else
                {
                    c100.TenantVatNumber = "";
                }

                if (item.ARInvoiceTypeCode == "IN" || item.ARInvoiceTypeCode == "CI")
                {
                    c100.DocumentType = "305";
                    c100.TotalAmount = invoiceCurrency.Code != "NIS" ? (item.AmountInInvoiceCurrency != null ? String.Format("{0:+000000000000.00}", item.AmountInInvoiceCurrency.Value).Replace(".", "") : "+00000000000000") : "+00000000000000";
                    c100.AmountBeforeDiscount = String.Format("{0:+000000000000.00}", item.SubTotalInLocalCurrency.Value).Replace(".", "");
                    c100.SubTotal = String.Format("{0:+000000000000.00}", item.SubTotalInLocalCurrency.Value).Replace(".", "");
                    c100.VAT = totalVats != null ? String.Format("{0:+000000000000.00}", totalVats.Sum(d => d.LocalVATAmount)).Replace(".", "") : "+00000000000000";
                    c100.GrandTotal = String.Format("{0:+000000000000.00}", item.AmountInLocalCurrency.Value).Replace(".", "");
                }
                else if (item.ARInvoiceTypeCode == "MN")
                {
                    c100.DocumentType = "310";
                    c100.TotalAmount = invoiceCurrency.Code != "NIS" ? (item.AmountInInvoiceCurrency != null ? String.Format("{0:+000000000000.00}", item.AmountInInvoiceCurrency.Value).Replace(".", "") : "+00000000000000") : "+00000000000000";
                    c100.AmountBeforeDiscount = String.Format("{0:+000000000000.00}", item.SubTotalInLocalCurrency.Value).Replace(".", "");
                    c100.SubTotal = String.Format("{0:+000000000000.00}", item.SubTotalInLocalCurrency.Value).Replace(".", "");
                    c100.VAT = totalVats != null ? String.Format("{0:+000000000000.00}", totalVats.Sum(d => d.LocalVATAmount)).Replace(".", "") : "+00000000000000";
                    c100.GrandTotal = String.Format("{0:+000000000000.00}", item.AmountInLocalCurrency.Value).Replace(".", "");
                }
                else if (item.ARInvoiceTypeCode == "CD" || item.ARInvoiceTypeCode == "CC")
                {
                    c100.DocumentType = "330";
                    c100.TotalAmount = invoiceCurrency.Code != "NIS" ? (item.AmountInInvoiceCurrency != null ? String.Format("{0:000000000000.00}", item.AmountInInvoiceCurrency.Value).Replace(".", "") : "-00000000000000") : "-00000000000000";
                    c100.AmountBeforeDiscount = String.Format("{0:000000000000.00}", item.SubTotalInLocalCurrency.Value).Replace(".", "");
                    c100.SubTotal = String.Format("{0:000000000000.00}", item.SubTotalInLocalCurrency.Value).Replace(".", "");

                    if (totalVats != null)
                    {
                        double? sum = totalVats.Sum(d => d.LocalVATAmount);

                        if (sum == 0)
                        {
                            c100.VAT = "-00000000000000";
                        }
                        else
                        {
                            c100.VAT = String.Format("{0:000000000000.00}", sum).Replace(".", "");
                        }
                    }
                    else
                    {
                        c100.VAT = "-00000000000000";
                    }

                    c100.GrandTotal = String.Format("{0:000000000000.00}", item.AmountInLocalCurrency.Value).Replace(".", "");
                }

                c100.DocumentNumber = item.InvoiceNumber;
                c100.DocumentCreateDate = String.Format("{0:yyyyMMdd}", item.CreateDate);
                c100.DocumentCreateHour = String.Format("{0:hhmm}", item.CreateDate);
                c100.BillToName = billTo != null ? billTo.EnglishName : "";
                c100.BillToAddress_Street = billToAddress != null ? (billToAddress.Address1 != null ? billToAddress.Address1 : "") : "";
                c100.BillToAddress_HouseNumber = "";
                c100.BillToAddress_City = billToAddress != null ? (billToAddress.City != null ? billToAddress.City : "") : "";

                if (billToAddress != null)
                {
                    if (!string.IsNullOrEmpty(billToAddress.ZipCode))
                    {
                        if (billToAddress.ZipCode.Length > 8)
                        {
                            c100.BillToAddress_ZipCode = billToAddress.ZipCode.Substring(0, 8);
                        }
                        else
                        {
                            c100.BillToAddress_ZipCode = billToAddress.ZipCode;
                        }
                    }
                    else
                    {
                        c100.BillToAddress_ZipCode = "";
                    }
                }
                else
                {
                    c100.BillToAddress_ZipCode = "";
                }

                c100.BillToAddress_CountryName = billToAddress != null ? (billToAddress.Country != null ? billToAddress.Country.EnglishName : "") : "";
                c100.BillToAddress_CountryCode = billToAddress != null ? (billToAddress.Country != null ? billToAddress.Country.Code : "") : "";

                if (billToAddress != null)
                {
                    if (!string.IsNullOrEmpty(billToAddress.PhoneNumber))
                    {
                        if (billToAddress.PhoneNumber.Length > 15)
                        {
                            c100.BillToAddress_Telephone = billToAddress.PhoneNumber.Substring(0, 15);
                        }
                        else
                        {
                            c100.BillToAddress_Telephone = billToAddress.PhoneNumber;
                        }
                    }
                    else
                    {
                        c100.BillToAddress_Telephone = "";
                    }
                }
                else
                {
                    c100.BillToAddress_Telephone = "";
                }

                if (billTo != null)
                {
                    if (!string.IsNullOrEmpty(billTo.VatNumber))
                    {
                        if (billTo.VatNumber.Length > 9)
                        {
                            c100.BillToVAT = CheckVATValidation(billTo.VatNumber.Substring(0, 9));
                        }
                        else
                        {
                            c100.BillToVAT = CheckVATValidation(billTo.VatNumber);
                        }
                    }
                    else
                    {
                        c100.BillToVAT = "";
                    }
                }
                else
                {
                    c100.BillToVAT = "";
                }

                c100.ValueDate = String.Format("{0:yyyyMMdd}", item.InvoiceDate);
                c100.CurrencyCode = invoiceCurrency != null ? (invoiceCurrency.Code != "NIS" ? invoiceCurrency.Code : "") : "";
                c100.Discount = "+00000000000000";
                c100.Field1224 = "";
                c100.BillToId = billTo != null ? billTo.Code : "";
                c100.Field1226 = "";
                c100.Void_CancelledDocument = item.IsCancelled ? "1" : "";
                c100.DocumentDate = String.Format("{0:yyyyMMdd}", item.InvoiceDate);
                c100.BranchId = "";

                if (!string.IsNullOrEmpty(username))
                {
                    if (username.Length > 9)
                    {
                        c100.UserName = username.Substring(0, 9);
                    }
                    else
                    {
                        c100.UserName = username;
                    }
                }
                else
                {
                    c100.UserName = "";
                }

                c100.LinkingField = "0000000";
                c100.FutureUsage = "";

                str.Append(c100.RecordCode.PadRight(4));
                str.Append(c100.RecordLineNumber.PadLeft(9, '0'));
                str.Append(c100.TenantVatNumber.PadLeft(9, '0'));
                str.Append(c100.DocumentType.PadLeft(3, '0'));
                str.Append(c100.DocumentNumber.PadRight(20));
                str.Append(c100.DocumentCreateDate.PadLeft(8, '0'));
                str.Append(c100.DocumentCreateHour.PadLeft(4, '0'));
                str.Append(c100.BillToName.PadRight(50));
                str.Append(c100.BillToAddress_Street.PadRight(50));
                str.Append(c100.BillToAddress_HouseNumber.PadRight(10));
                str.Append(c100.BillToAddress_City.PadRight(30));
                str.Append(c100.BillToAddress_ZipCode.PadRight(8));
                str.Append(c100.BillToAddress_CountryName.PadRight(30));
                str.Append(c100.BillToAddress_CountryCode.PadRight(2));
                str.Append(c100.BillToAddress_Telephone.PadRight(15));
                str.Append(c100.BillToVAT.PadLeft(9, '0'));
                str.Append(c100.ValueDate.PadLeft(8, '0'));
                str.Append(c100.TotalAmount);
                str.Append(c100.CurrencyCode.PadRight(3));
                str.Append(c100.AmountBeforeDiscount);
                str.Append(c100.Discount);
                str.Append(c100.SubTotal.ToString().PadRight(15));
                str.Append(c100.VAT);
                str.Append(c100.GrandTotal);
                str.Append(c100.Field1224.PadRight(12));
                str.Append(c100.BillToId.PadRight(15));
                str.Append(c100.Field1224.PadRight(10));
                str.Append(c100.Void_CancelledDocument.PadRight(1));
                str.Append(c100.DocumentDate.PadLeft(8, '0'));
                str.Append(c100.BranchId.PadRight(7));
                str.Append(c100.UserName.PadRight(9));
                str.Append(c100.LinkingField.PadRight(7));
                str.Append(c100.FutureUsage.PadRight(13));
                str.AppendLine();

                str.Append(BuildD110ARInvoiceLines(i, item, tenant, invoiceLineRep));
                i += invoiceLineRep.GetInvoiceLinesByInvoiceId(item.Id, tenant.Id).Count();
            }

            return str.ToString();
        }

        private string BuildD110ARInvoiceLines(int counter, ARInvoice invoice, Tenant tenant, ARInvoiceLineRepository invoiceLineRep)
        {
            StringBuilder str = new StringBuilder(339);
            List<ARInvoiceLine> lines = invoiceLineRep.GetInvoiceLinesByInvoiceId(invoice.Id, tenant.Id).ToList();
            int j = 1;

            foreach (ARInvoiceLine line in lines)
            {
                TaxesApproval.D110 d110 = new TaxesApproval.D110();
                d110.RecordCode = "D110";
                d110.RecordLineNumber = counter++.ToString();

                if (!string.IsNullOrEmpty(tenant.VatNumber))
                {
                    if (tenant.VatNumber.Length > 9)
                    {
                        d110.TenantVatNumber = CheckVATValidation(tenant.VatNumber.Substring(0, 9));
                    }
                    else
                    {
                        d110.TenantVatNumber = CheckVATValidation(tenant.VatNumber);
                    }
                }
                else
                {
                    d110.TenantVatNumber = "";
                }

                if (invoice.ARInvoiceTypeCode == "IN" || invoice.ARInvoiceTypeCode == "CI")
                {
                    d110.DocumentType = "305";
                    d110.UnitPrice = String.Format("{0:+000000000000.00}", (line.LocalCurrencyAmount / line.Quantity).Value).Replace(".", "");
                    d110.LineAmount = String.Format("{0:+000000000000.00}", line.LocalCurrencyAmount.Value).Replace(".", "");
                }
                else if (invoice.ARInvoiceTypeCode == "MN")
                {
                    d110.DocumentType = "310";
                    d110.UnitPrice = String.Format("{0:+000000000000.00}", (line.LocalCurrencyAmount / line.Quantity).Value).Replace(".", "");
                    d110.LineAmount = String.Format("{0:+000000000000.00}", line.LocalCurrencyAmount.Value).Replace(".", "");
                }
                else if (invoice.ARInvoiceTypeCode == "CD" || invoice.ARInvoiceTypeCode == "CC")
                {
                    d110.DocumentType = "330";
                    d110.UnitPrice = String.Format("{0:000000000000.00}", (line.LocalCurrencyAmount / line.Quantity).Value).Replace(".", "");
                    d110.LineAmount = String.Format("{0:000000000000.00}", line.LocalCurrencyAmount.Value).Replace(".", "");
                }

                d110.DocumentNumber = invoice.InvoiceNumber;
                d110.DocumentLineNumber = j++.ToString();
                d110.BaseDocumentType = "000";
                d110.BaseDocumentNumber = "";
                d110.ServiceType = "1";
                d110.Field1259 = "";

                if (!string.IsNullOrEmpty(line.Description))
                {
                    if (line.Description.Length > 30)
                    {
                        d110.DescriptionOfSservice = line.Description.Substring(0, 30);
                    }
                    else
                    {
                        d110.DescriptionOfSservice = line.Description;
                    }
                }
                else
                {
                    d110.DescriptionOfSservice = "";
                }

                d110.ManifacturerName = "";
                d110.ProductSerialNumber = "";
                d110.Field1263 = "יחידה";
                d110.Quantiy = String.Format("{0:+000000000000.0000}", line.Quantity.Value).Replace(".", "");
                d110.LineDiscount = "+00000000000000";
                d110.LineVATPercentage = String.Format("{0:00.00}", line.VatPercentage.Value).Replace(".", "");
                d110.BranchId = "";
                d110.EntityDate = String.Format("{0:yyyyMMdd}", invoice.InvoiceDate);
                d110.Field1273 = "0000000";
                d110.Field1274 = "";
                d110.FutureUsage = "";

                str.Append(d110.RecordCode.PadRight(4));
                str.Append(d110.RecordLineNumber.PadLeft(9, '0'));
                str.Append(d110.TenantVatNumber.PadLeft(9, '0'));
                str.Append(d110.DocumentType.PadLeft(3, '0'));
                str.Append(d110.DocumentNumber.PadRight(20));
                str.Append(d110.DocumentLineNumber.PadLeft(4, '0'));
                str.Append(d110.BaseDocumentType.PadLeft(3, '0'));
                str.Append(d110.BaseDocumentNumber.PadRight(20));
                str.Append(d110.ServiceType.PadRight(1));
                str.Append(d110.Field1259.PadRight(20));
                str.Append(d110.DescriptionOfSservice.PadRight(30));
                str.Append(d110.ManifacturerName.PadRight(50));
                str.Append(d110.ProductSerialNumber.PadRight(30));
                str.Append(d110.Field1263.PadRight(20));
                str.Append(d110.Quantiy);
                str.Append(d110.UnitPrice);
                str.Append(d110.LineDiscount);
                str.Append(d110.LineAmount);
                str.Append(d110.LineVATPercentage);
                str.Append(d110.BranchId.PadRight(7));
                str.Append(d110.EntityDate.PadLeft(8, '0'));
                str.Append(d110.Field1273.PadRight(7));
                str.Append(d110.Field1274.PadRight(7));
                str.Append(d110.FutureUsage.PadRight(21));

                str.AppendLine();
            }

            return str.ToString();
        }

        private string BuildC100APInvoiceLines(int count, List<APInvoice> invoices, Tenant tenant, APInvoiceLineRepository invoiceLineRep, APInvoiceTotalVATRepository totalVatRep, string username, List<Card> myCards)
        {
            ContactQuery contactQuery = new ContactQuery(tenant.Id);
            AddressRepository addressRep = new AddressRepository(tenant.Id);
            CurrencyRepository currencyRep = new CurrencyRepository(tenant.Id);

            StringBuilder str = new StringBuilder(444);
            int i = count + 1;

            foreach (APInvoice item in invoices)
            {
                Address vendorAddress = addressRep.GetMainAddressByCardId(item.VendorId, tenant.Id);
                Card vendor = myCards.Where(d => d.Id == item.VendorId).FirstOrDefault();
                Currency invoiceCurrency = currencyRep.GetSingleCurrency(item.InvoiceCurrencyId, tenant.Id);
                List<APInvoiceTotalVAT> totalVats = totalVatRep.GetInvoiceTotalVatsByInvoiceId(item.Id, tenant.Id).ToList();

                TaxesApproval.C100 c100 = new TaxesApproval.C100();

                c100.RecordCode = "C100";
                c100.RecordLineNumber = i++.ToString();

                if (!string.IsNullOrEmpty(tenant.VatNumber))
                {
                    if (tenant.VatNumber.Length > 9)
                    {
                        c100.TenantVatNumber = CheckVATValidation(tenant.VatNumber.Substring(0, 9));
                    }
                    else
                    {
                        c100.TenantVatNumber = CheckVATValidation(tenant.VatNumber);
                    }
                }
                else
                {
                    c100.TenantVatNumber = "";
                }

                c100.DocumentType = "700";
                c100.DocumentNumber = item.InvoiceNumber;
                c100.DocumentCreateDate = String.Format("{0:yyyyMMdd}", item.CreateDate);
                c100.DocumentCreateHour = String.Format("{0:hhmm}", item.CreateDate);

                if (vendor != null)
                {
                    if (vendor.EnglishName.Length > 50)
                    {
                        c100.BillToName = vendor.EnglishName.Substring(0, 50);
                    }
                    else
                    {
                        c100.BillToName = vendor.EnglishName;
                    }
                }
                else
                {
                    c100.BillToName = "";
                }

                c100.BillToAddress_Street = vendorAddress != null ? (vendorAddress.Address1 != null ? vendorAddress.Address1 : "") : "";
                c100.BillToAddress_HouseNumber = "";
                c100.BillToAddress_City = vendorAddress != null ? (vendorAddress.City != null ? vendorAddress.City : "") : "";

                if (vendorAddress != null)
                {
                    if (!string.IsNullOrEmpty(vendorAddress.ZipCode))
                    {
                        if (vendorAddress.ZipCode.Length > 8)
                        {
                            c100.BillToAddress_ZipCode = vendorAddress.ZipCode.Substring(0, 8);
                        }
                        else
                        {
                            c100.BillToAddress_ZipCode = vendorAddress.ZipCode;
                        }
                    }
                    else
                    {
                        c100.BillToAddress_ZipCode = "";
                    }
                }
                else
                {
                    c100.BillToAddress_ZipCode = "";
                }

                c100.BillToAddress_CountryName = vendorAddress != null ? (vendorAddress.Country != null ? vendorAddress.Country.EnglishName : "") : "";
                c100.BillToAddress_CountryCode = vendorAddress != null ? (vendorAddress.Country != null ? vendorAddress.Country.Code : "") : "";

                if (vendorAddress != null)
                {
                    if (!string.IsNullOrEmpty(vendorAddress.PhoneNumber))
                    {
                        if (vendorAddress.PhoneNumber.Length > 15)
                        {
                            c100.BillToAddress_Telephone = vendorAddress.PhoneNumber.Substring(0, 15);
                        }
                        else
                        {
                            c100.BillToAddress_Telephone = vendorAddress.PhoneNumber;
                        }
                    }
                    else
                    {
                        c100.BillToAddress_Telephone = "";
                    }
                }
                else
                {
                    c100.BillToAddress_Telephone = "";
                }

                if (vendor != null)
                {
                    if (!string.IsNullOrEmpty(vendor.VatNumber))
                    {
                        if (vendor.VatNumber.Length > 9)
                        {
                            c100.BillToVAT = CheckVATValidation(vendor.VatNumber.Substring(0, 9));
                        }
                        else
                        {
                            c100.BillToVAT = CheckVATValidation(vendor.VatNumber);
                        }
                    }
                    else
                    {
                        c100.BillToVAT = "";
                    }
                }
                else
                {
                    c100.BillToVAT = "";
                }

                c100.ValueDate = String.Format("{0:yyyyMMdd}", item.InvoiceDate);
                c100.CurrencyCode = invoiceCurrency != null ? (invoiceCurrency.Code != "NIS" ? invoiceCurrency.Code : "") : "";
                c100.Discount = "+00000000000000";

                if (item.AmountInInvoiceCurrency > 0)
                {
                    c100.TotalAmount = invoiceCurrency.Code != "NIS" ? (item.AmountInInvoiceCurrency != null ? String.Format("{0:-000000000000.00}", item.AmountInInvoiceCurrency.Value).Replace(".", "") : "-00000000000000") : "-00000000000000";
                }
                else if (item.AmountInInvoiceCurrency < 0)
                {
                    c100.TotalAmount = invoiceCurrency.Code != "NIS" ? (item.AmountInInvoiceCurrency != null ? String.Format("{0:000000000000.00}", item.AmountInInvoiceCurrency.Value).Replace(".", "") : "-00000000000000") : "-00000000000000";
                }

                if (item.SubTotalInLocalCurrency.Value > 0)
                {
                    c100.AmountBeforeDiscount = String.Format("{0:+000000000000.00}", item.SubTotalInLocalCurrency.Value).Replace(".", "");
                    c100.SubTotal = String.Format("{0:+000000000000.00}", item.SubTotalInLocalCurrency.Value).Replace(".", "");
                }
                else if (item.SubTotalInLocalCurrency.Value < 0)
                {
                    c100.AmountBeforeDiscount = String.Format("{0:000000000000.00}", item.SubTotalInLocalCurrency.Value).Replace(".", "");
                    c100.SubTotal = String.Format("{0:000000000000.00}", item.SubTotalInLocalCurrency.Value).Replace(".", "");
                }

                if (totalVats != null)
                {
                    double sum = totalVats.Sum(d => d.LocalVATAmount);
                    if (sum > 0)
                    {
                        c100.VAT = String.Format("{0:+000000000000.00}", totalVats.Sum(d => d.LocalVATAmount)).Replace(".", "");
                    }
                    else if (sum < 0)
                    {
                        c100.VAT = String.Format("{0:000000000000.00}", totalVats.Sum(d => d.LocalVATAmount)).Replace(".", "");
                    }
                    else
                    {
                        if (item.SubTotalInLocalCurrency.Value > 0)
                        {
                            c100.VAT = "+00000000000000";
                        }
                        else
                        {
                            c100.VAT = "-00000000000000";
                        }
                    }
                }
                else
                {
                    c100.VAT = "+00000000000000";
                }

                if (item.AmountInLocalCurrency.Value > 0)
                {
                    c100.GrandTotal = String.Format("{0:+000000000000.00}", item.AmountInLocalCurrency.Value).Replace(".", "");
                }
                else if (item.AmountInLocalCurrency.Value < 0)
                {
                    c100.GrandTotal = String.Format("{0:000000000000.00}", item.AmountInLocalCurrency.Value).Replace(".", "");
                }

                c100.Field1224 = "";
                c100.BillToId = vendor != null ? vendor.Code : "";
                c100.Field1226 = "";
                c100.Void_CancelledDocument = item.IsClosed ? "1" : "";
                c100.DocumentDate = String.Format("{0:yyyyMMdd}", item.InvoiceDate);
                c100.BranchId = "";

                if (!string.IsNullOrEmpty(username))
                {
                    if (username.Length > 9)
                    {
                        c100.UserName = username.Substring(0, 9);
                    }
                    else
                    {
                        c100.UserName = username;
                    }
                }
                else
                {
                    c100.UserName = "";
                }

                c100.LinkingField = "0000000";
                c100.FutureUsage = "";

                str.Append(c100.RecordCode.PadRight(4));
                str.Append(c100.RecordLineNumber.PadLeft(9, '0'));
                str.Append(c100.TenantVatNumber.PadLeft(9, '0'));
                str.Append(c100.DocumentType.PadLeft(3, '0'));
                str.Append(c100.DocumentNumber.PadRight(20));
                str.Append(c100.DocumentCreateDate.PadLeft(8, '0'));
                str.Append(c100.DocumentCreateHour.PadLeft(4, '0'));
                str.Append(c100.BillToName.PadRight(50));
                str.Append(c100.BillToAddress_Street.PadRight(50));
                str.Append(c100.BillToAddress_HouseNumber.PadRight(10));
                str.Append(c100.BillToAddress_City.PadRight(30));
                str.Append(c100.BillToAddress_ZipCode.PadRight(8));
                str.Append(c100.BillToAddress_CountryName.PadRight(30));
                str.Append(c100.BillToAddress_CountryCode.PadRight(2));
                str.Append(c100.BillToAddress_Telephone.PadRight(15));
                str.Append(c100.BillToVAT.PadLeft(9, '0'));
                str.Append(c100.ValueDate.PadLeft(8, '0'));
                str.Append(c100.TotalAmount);
                str.Append(c100.CurrencyCode.PadRight(3));
                str.Append(c100.AmountBeforeDiscount);
                str.Append(c100.Discount);
                str.Append(c100.SubTotal.ToString().PadRight(15));
                str.Append(c100.VAT);
                str.Append(c100.GrandTotal);
                str.Append(c100.Field1224.PadRight(12));
                str.Append(c100.BillToId.PadRight(15));
                str.Append(c100.Field1224.PadRight(10));
                str.Append(c100.Void_CancelledDocument.PadRight(1));
                str.Append(c100.DocumentDate.PadLeft(8, '0'));
                str.Append(c100.BranchId.PadRight(7));
                str.Append(c100.UserName.PadRight(9));
                str.Append(c100.LinkingField.PadRight(7));
                str.Append(c100.FutureUsage.PadRight(13));
                str.AppendLine();

                str.Append(BuildD110APInvoiceLines(i, item, tenant, invoiceLineRep));
                i += invoiceLineRep.GetInvoiceLinesByInvoiceId(item.Id, tenant.Id).Count();
            }
            return str.ToString();
        }

        private string BuildD110APInvoiceLines(int counter, APInvoice invoice, Tenant tenant, APInvoiceLineRepository invoiceLineRep)
        {
            ChargesTypeRepository chargesRep = new ChargesTypeRepository(tenant.Id);

            StringBuilder str = new StringBuilder(339);
            List<APInvoiceLine> lines = invoiceLineRep.GetInvoiceLinesByInvoiceId(invoice.Id, tenant.Id).ToList();
            int j = 1;

            foreach (APInvoiceLine line in lines)
            {
                ChargesType charge = chargesRep.GetSingleChargesType(line.ChargesTypeId, tenant.Id);

                TaxesApproval.D110 d110 = new TaxesApproval.D110();
                d110.RecordCode = "D110";
                d110.RecordLineNumber = counter++.ToString();

                if (!string.IsNullOrEmpty(tenant.VatNumber))
                {
                    if (tenant.VatNumber.Length > 9)
                    {
                        d110.TenantVatNumber = CheckVATValidation(tenant.VatNumber.Substring(0, 9));
                    }
                    else
                    {
                        d110.TenantVatNumber = CheckVATValidation(tenant.VatNumber);
                    }
                }
                else
                {
                    d110.TenantVatNumber = "";
                }

                d110.DocumentType = "700";
                d110.DocumentNumber = invoice.InvoiceNumber;
                d110.DocumentLineNumber = j++.ToString();
                d110.BaseDocumentType = "000";
                d110.BaseDocumentNumber = "";
                d110.ServiceType = "1";
                d110.Field1259 = "";

                if (charge != null)
                {
                    if (!string.IsNullOrEmpty(charge.EnglishName))
                    {
                        if (charge.EnglishName.Length > 30)
                        {
                            d110.DescriptionOfSservice = charge.EnglishName.Substring(0, 30);
                        }
                        else
                        {
                            d110.DescriptionOfSservice = charge.EnglishName;
                        }
                    }
                    else
                    {
                        d110.DescriptionOfSservice = "";
                    }
                }
                else
                {
                    d110.DescriptionOfSservice = "";
                }

                d110.ManifacturerName = "";
                d110.ProductSerialNumber = "";
                d110.Field1263 = "יחידה";
                d110.Quantiy = "+0000000000010000";
                d110.LineDiscount = "+00000000000000";

                if (line.LocalCurrencyAmount > 0)
                {
                    d110.UnitPrice = String.Format("{0:+000000000000.00}", line.LocalCurrencyAmount.Value).Replace(".", "");
                    d110.LineAmount = String.Format("{0:+000000000000.00}", line.LocalCurrencyAmount.Value).Replace(".", "");
                }
                else if (line.LocalCurrencyAmount < 0)
                {
                    d110.UnitPrice = String.Format("{0:000000000000.00}", line.LocalCurrencyAmount.Value).Replace(".", "");
                    d110.LineAmount = String.Format("{0:000000000000.00}", line.LocalCurrencyAmount.Value).Replace(".", "");
                }

                d110.LineVATPercentage = String.Format("{0:00.00}", line.VatPercentage.Value).Replace(".", "");
                d110.BranchId = "";
                d110.EntityDate = String.Format("{0:yyyyMMdd}", invoice.InvoiceDate);
                d110.Field1273 = "0000000";
                d110.Field1274 = "";
                d110.FutureUsage = "";

                str.Append(d110.RecordCode.PadRight(4));
                str.Append(d110.RecordLineNumber.PadLeft(9, '0'));
                str.Append(d110.TenantVatNumber.PadLeft(9, '0'));
                str.Append(d110.DocumentType.PadLeft(3, '0'));
                str.Append(d110.DocumentNumber.PadRight(20));
                str.Append(d110.DocumentLineNumber.PadLeft(4, '0'));
                str.Append(d110.BaseDocumentType.PadLeft(3, '0'));
                str.Append(d110.BaseDocumentNumber.PadRight(20));
                str.Append(d110.ServiceType.PadRight(1));
                str.Append(d110.Field1259.PadRight(20));
                str.Append(d110.DescriptionOfSservice.PadRight(30));
                str.Append(d110.ManifacturerName.PadRight(50));
                str.Append(d110.ProductSerialNumber.PadRight(30));
                str.Append(d110.Field1263.PadRight(20));
                str.Append(d110.Quantiy);
                str.Append(d110.UnitPrice);
                str.Append(d110.LineDiscount);
                str.Append(d110.LineAmount);
                str.Append(d110.LineVATPercentage);
                str.Append(d110.BranchId.PadRight(7));
                str.Append(d110.EntityDate.PadLeft(8, '0'));
                str.Append(d110.Field1273.PadRight(7));
                str.Append(d110.Field1274.PadRight(7));
                str.Append(d110.FutureUsage.PadRight(21));

                str.AppendLine();
            }

            return str.ToString();
        }

        private string BuildZ900Line(int counter, Tenant tenant, string random)
        {
            TaxesApproval.Z900 z900 = new TaxesApproval.Z900();

            z900.RecordCode = "Z900";
            z900.RecordLineNumber = String.Format("{0:000000000}", counter);

            if (!string.IsNullOrEmpty(tenant.VatNumber))
            {
                if (tenant.VatNumber.Length > 9)
                {
                    z900.TenantVatNumber = CheckVATValidation(tenant.VatNumber.Substring(0, 9));
                }
                else
                {
                    z900.TenantVatNumber = CheckVATValidation(tenant.VatNumber);
                }
            }
            else
            {
                z900.TenantVatNumber = "";
            }

            z900.PrimaryId = random;
            z900.SystemConst = "&OF1.31&";
            z900.TotalRecords = String.Format("{0:000000000000000}", counter);
            z900.FutureUsage = "";

            StringBuilder str = new StringBuilder(110);

            str.Append(z900.RecordCode.PadRight(4));
            str.Append(z900.RecordLineNumber);

            if (z900.TenantVatNumber != null) { str.Append(z900.TenantVatNumber.PadLeft(9, '0')); }
            else { str.Append(' ', 9); }

            str.Append(z900.PrimaryId.PadRight(15));
            str.Append(z900.SystemConst.PadRight(8));
            str.Append(z900.TotalRecords.PadRight(15));
            str.Append(z900.FutureUsage.PadRight(50));

            return str.ToString();
        }

        private byte[] ZipFiles(List<DataFiles> files, string path)
        {
            Crc32 crc32 = new Crc32();
            MemoryStream outputMS = new System.IO.MemoryStream();
            ZipOutputStream zipOutput = new ZipOutputStream(outputMS);

            int i = 1;
            foreach (DataFiles file in files)
            {
                ZipEntry entry = new ZipEntry(path + file.FileName);

                entry.Size = file.FileData.Length;

                crc32.Reset();
                crc32.Update(file.FileData);
                entry.Crc = crc32.Value;

                zipOutput.PutNextEntry(entry);
                zipOutput.Write(file.FileData, 0, file.FileData.Length);
                i++;
            }

            zipOutput.Finish();
            zipOutput.Close();

            return outputMS.ToArray();
        }

        //INI
        private string BuildA000File(DateTime date1, DateTime date2, List<ARPayment> payments, List<ARInvoice> invoices, List<APInvoice> apInvoices, Tenant tenant, string random)
        {
            IInvoiceContext invoiceCotnext = InvoiceContext.GetContext(tenant.Id);

            ARInvoiceLineRepository invoiceLineRep = new ARInvoiceLineRepository(invoiceCotnext);
            APInvoiceLineRepository apInvoiceLineRep = new APInvoiceLineRepository(invoiceCotnext);
            AddressRepository addressRep = new AddressRepository(tenant.Id);
            CurrencyRepository currencyRep = new CurrencyRepository(tenant.Id);
            CardRepository cardRep = new CardRepository(tenant.Id);

            Address address = addressRep.GetSingleAddress(tenant.AddressId, tenant.Id);
            Currency currency = currencyRep.GetSingleCurrency(tenant.CurrencyId, tenant.Id);

            List<Card> allCards = cardRep.GetCards(tenant.Id).ToList();
            List<Card> myCards = new List<Card>();
            Card billTo = null;
            foreach (ARInvoice item in invoices)
            {
                billTo = allCards.Where(d => d.Id == item.BillToId).FirstOrDefault();

                if (billTo != null)
                {
                    myCards.Add(billTo);
                }
            }

            foreach (APInvoice item in apInvoices)
            {
                billTo = allCards.Where(d => d.Id == item.VendorId).FirstOrDefault();

                if (billTo != null)
                {
                    myCards.Add(billTo);
                }
            }

            foreach (ARPayment item in payments)
            {
                billTo = allCards.Where(d => d.Id == item.BillToId).FirstOrDefault();

                if (billTo != null)
                {
                    myCards.Add(billTo);
                }
            }

            myCards = myCards.Distinct().ToList();

            int totalcounter = 0;
            int C100Counter = 0;
            int D110Counter = 0;
            int D120Counter = 0;
            int B110Counter = 0;

            totalcounter += invoices.Count;
            totalcounter += apInvoices.Count;
            totalcounter += (payments.Count) * 2;
            totalcounter += myCards.Count;

            foreach (ARInvoice invoice in invoices)
            {
                totalcounter += invoiceLineRep.GetInvoiceLinesByInvoiceId(invoice.Id, tenant.Id).Count();
                D110Counter += invoiceLineRep.GetInvoiceLinesByInvoiceId(invoice.Id, tenant.Id).Count();
            }
            foreach (APInvoice invoice in apInvoices)
            {
                totalcounter += apInvoiceLineRep.GetInvoiceLinesByInvoiceId(invoice.Id, tenant.Id).Count();
                D110Counter += apInvoiceLineRep.GetInvoiceLinesByInvoiceId(invoice.Id, tenant.Id).Count();
            }

            C100Counter = invoices.Count + apInvoices.Count + payments.Count;
            D120Counter = payments.Count;
            B110Counter = myCards.Count;

            TaxesApproval.A000 a000 = new TaxesApproval.A000();
            a000.RecordCode = "A000";
            a000.FutureUsage = "";
            a000.TotalLinesInBKMVDATA = String.Format("{0:000000000000000}", totalcounter + 2);

            if (!string.IsNullOrEmpty(tenant.VatNumber))
            {
                if (tenant.VatNumber.Length > 9)
                {
                    a000.TenantVatNumber = CheckVATValidation(tenant.VatNumber.Substring(0, 9));
                }
                else
                {
                    a000.TenantVatNumber = CheckVATValidation(tenant.VatNumber);
                }
            }
            else
            {
                a000.TenantVatNumber = "";
            }

            a000.MainId = random;
            a000.FixedValue = "&OF1.31&";
            a000.SystemRegistrationNumber = "00196801";
            a000.SystemName = "Logitude World";
            a000.SystemVerion = "2.5";
            a000.LogitudeVATNumber = "514501774";
            a000.SystemManifacturer = "Logitude World LTD";
            a000.SystemType = "2";
            a000.FilesSavingDirectory = "C:\\OPENFRMT\\" + tenant.VatNumber + "." + String.Format("{0:yy}", DateTime.Now) + "\\" + String.Format("{0:MMddhhmm}", DateTime.Now);
            a000.SystemAcctType = "2";
            a000.NeededBalanceLevel = "2";
            a000.Field1016 = "";
            a000.TenantName = tenant.Company;
            a000.TenantAddress1 = address != null ? address.Address1 : "";
            a000.TenantHouseNumber = "";
            a000.TenantCity = address != null ? (address.City != null ? address.City : "") : "";

            if (address != null)
            {
                if (!string.IsNullOrEmpty(address.ZipCode))
                {
                    if (address.ZipCode.Length > 8)
                    {
                        a000.TenantZipCode = address.ZipCode.Substring(0, 8);
                    }
                    else
                    {
                        a000.TenantZipCode = address.ZipCode;
                    }
                }
                else
                {
                    a000.TenantZipCode = "";
                }
            }
            else
            {
                a000.TenantZipCode = "";
            }

            a000.TaxYear = String.Format("{0:yyyy}", date1);
            a000.Startingdate = String.Format("{0:yyyyMMdd}", date1);
            a000.EndDate = String.Format("{0:yyyyMMdd}", date2);
            a000.TodayDate = String.Format("{0:yyyyMMdd}", DateTime.Now);
            a000.TimeNow = String.Format("{0:hhmm}", DateTime.Now);
            a000.LanguageCode = "0";
            a000.Encoding = "1";
            a000.Field1030 = "WINZIP";
            a000.AccountingCurrencyCode = currency != null ? (currency.Code == "NIS" ? "ILS" : currency.Code) : "";
            a000.BranchInfo = "0";

            StringBuilder str = new StringBuilder(466);

            str.Append(a000.RecordCode.PadRight(4));
            str.Append(a000.FutureUsage.PadRight(5));
            str.Append(a000.TotalLinesInBKMVDATA);
            str.Append(a000.TenantVatNumber.PadRight(9));
            str.Append(a000.MainId.PadRight(15));
            str.Append(a000.FixedValue.PadRight(8));
            str.Append(a000.SystemRegistrationNumber.PadLeft(8, '0'));
            str.Append(a000.SystemName.PadRight(20));
            str.Append(a000.SystemVerion.PadRight(20));
            str.Append(a000.LogitudeVATNumber.PadLeft(9, '0'));
            str.Append(a000.SystemManifacturer.PadRight(20));
            str.Append(a000.SystemType.PadLeft(1, '0'));
            str.Append(a000.FilesSavingDirectory.PadRight(50));
            str.Append(a000.SystemAcctType.PadLeft(1, '0'));
            str.Append(a000.NeededBalanceLevel.PadLeft(1, '0'));
            str.Append(a000.TenantVatNumber.PadLeft(9, '0'));
            str.Append(a000.Field1016.PadLeft(9, '0'));
            str.Append(a000.FutureUsage.PadRight(10));
            str.Append(a000.TenantName.PadRight(50));
            str.Append(a000.TenantAddress1.PadRight(50));
            str.Append(a000.TenantHouseNumber.PadRight(10));
            str.Append(a000.TenantCity.PadRight(30));
            str.Append(a000.TenantZipCode.PadRight(8));
            str.Append(a000.TaxYear);
            str.Append(a000.Startingdate);
            str.Append(a000.EndDate);
            str.Append(a000.TodayDate);
            str.Append(a000.TimeNow);
            str.Append(a000.LanguageCode.PadLeft(1, '0'));
            str.Append(a000.Encoding.PadLeft(1, '0'));
            str.Append(a000.Field1030.PadRight(20));
            str.Append(a000.AccountingCurrencyCode.PadRight(3));
            str.Append(a000.BranchInfo.PadLeft(1, '0'));
            str.Append(a000.FutureUsage.PadRight(46));
            str.AppendLine();

            str.Append("A100");
            str.Append("000000000000001");
            str.AppendLine();

            str.Append("B110");
            str.Append(String.Format("{0:000000000000000}", B110Counter));
            str.AppendLine();

            str.Append("C100");
            str.Append(String.Format("{0:000000000000000}", C100Counter));
            str.AppendLine();

            str.Append("D110");
            str.Append(String.Format("{0:000000000000000}", D110Counter));
            str.AppendLine();

            str.Append("D120");
            str.Append(String.Format("{0:000000000000000}", D120Counter));
            str.AppendLine();

            str.Append("Z900");
            str.Append("000000000000001");

            return str.ToString();
        }

        //Report
        private TaxesApproval.TaxReport BuildTaxReportFile(DateTime date1, DateTime date2, List<ARPayment> payments, List<ARInvoice> invoices, List<APInvoice> apInvoices, Tenant tenant)
        {
            IInvoiceContext invoiceCotnext = InvoiceContext.GetContext(tenant.Id);

            ARInvoiceLineRepository invoiceLineRep = new ARInvoiceLineRepository(invoiceCotnext);
            APInvoiceLineRepository apInvoiceLineRep = new APInvoiceLineRepository(invoiceCotnext);
            AddressRepository addressRep = new AddressRepository(tenant.Id);
            CurrencyRepository currencyRep = new CurrencyRepository(tenant.Id);
            CardRepository cardRep = new CardRepository(tenant.Id);

            List<Card> allCards = new List<Card>();
            Address address = addressRep.GetSingleAddress(tenant.AddressId, tenant.Id);
            Currency currency = currencyRep.GetSingleCurrency(tenant.CurrencyId, tenant.Id);

            int C100Counter = 0;
            int D110Counter = 0;
            int D120Counter = 0;
            int B110Counter = 0;

            int Count305 = 0;
            double? Total305 = 0;
            int Count310 = 0;
            double? Total310 = 0;
            int Count330 = 0;
            double? Total330 = 0;
            int Count400 = 0;
            double? Total400 = 0;
            int Count700 = 0;
            double? Total700 = 0;

            foreach (ARInvoice item in invoices)
            {
                Card billTo = cardRep.GetSingleCard(item.BillToId, tenant.Id);
                allCards.Add(billTo);
                D110Counter += invoiceLineRep.GetInvoiceLinesByInvoiceId(item.Id, tenant.Id).Count();

                if (item.ARInvoiceTypeCode == "IN" || item.ARInvoiceTypeCode == "CI")
                {
                    Count305 += 1;
                    Total305 += item.AmountInLocalCurrency;
                }
                else if (item.ARInvoiceTypeCode == "MN")
                {
                    Count310 += 1;
                    Total310 += item.AmountInLocalCurrency;

                }
                else if (item.ARInvoiceTypeCode == "CD" || item.ARInvoiceTypeCode == "CC")
                {
                    Count330 += 1;
                    Total330 += item.AmountInLocalCurrency;
                }
            }

            foreach (APInvoice item in apInvoices)
            {
                Card billTo = cardRep.GetSingleCard(item.VendorId, tenant.Id);
                allCards.Add(billTo);
                D110Counter += apInvoiceLineRep.GetInvoiceLinesByInvoiceId(item.Id, tenant.Id).Count();

                Count700 += 1;
                Total700 += item.AmountInLocalCurrency;
            }

            foreach (ARPayment item in payments)
            {
                Card billTo = cardRep.GetSingleCard(item.BillToId, tenant.Id);
                allCards.Add(billTo);

                Count400 += 1;
                Total400 += item.AmountInLocalCurrency;
            }

            allCards = allCards.Distinct().ToList();
            B110Counter = allCards.Count;

            C100Counter = invoices.Count + apInvoices.Count + payments.Count;
            D120Counter = payments.Count;

            TaxesApproval.TaxReport report = new TaxesApproval.TaxReport();

            report.TodayDate = String.Format("{0:yyyyMMdd}", DateTime.Now);
            report.TenantVATNumber = tenant.VatNumber;
            report.TenantName = tenant.Company;
            report.FileSavingDirectory = "C:\\OPENFRMT\\" + tenant.VatNumber + "." + String.Format("{0:yy}", DateTime.Now) + "\\" + String.Format("{0:MMddhhmm}", DateTime.Now);
            report.FromDate = String.Format("{0:dd.MM.yyyy}", date1);
            report.ToDate = String.Format("{0:dd.MM.yyyy}", date2);
            report.B110Count = B110Counter.ToString();
            report.C100Count = C100Counter.ToString();
            report.D110Count = D110Counter.ToString();
            report.D120Count = D120Counter.ToString();

            report.Count305 = Count305.ToString();
            report.Total305 = Math.Abs(Total305.Value);
            report.Count310 = Count310.ToString();
            report.Total310 = Math.Abs(Total310.Value);
            report.Count330 = Count330.ToString();
            report.Total330 = Math.Abs(Total330.Value);
            report.Count400 = Count400.ToString();
            report.Total400 = Math.Abs(Total400.Value);
            report.Count700 = Count700.ToString();
            report.Total700 = Math.Abs(Total700.Value);

            return report;
        }

        // encoding
        private byte[] ConvertEncoding(string s)
        {
            //38598 --- iso-8859-8-i --- Hebrew (ISO-Logical)
            Encoding iso = Encoding.GetEncoding(38598);
            Encoding utf8 = Encoding.UTF8;

            byte[] temp = utf8.GetBytes(s);
            byte[] isoBytes = Encoding.Convert(utf8, iso, temp);

            return isoBytes;
        }

        private string CheckVATValidation(string vat)
        {
            //for each VAT number that contains letters replace with 999999998
            //for each one that contains no letters make the following validation :
            //1- separate the 9 numbers to an array
            //2- multiply 1 2 1 2 1 2 1 2 1 to the VAT number array cells
            //3- go by the cells one by one , if the number is greater from 9, add both of its digits (check the link in the example)
            //4- sum all the cells
            //5- if the sum MOD 10 = 0 , write as is , else replace with 999999998
            vat = vat.Trim();
            string result = string.Empty;
            double Num;
            bool isVatNum = double.TryParse(vat, out Num);

            if (isVatNum)
            {
                int[] add = { 1, 2, 1, 2, 1, 2, 1, 2, 1 };
                char[] array = vat.ToCharArray();
                int[] res = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int parse = 0;
                int sumRes = 0;

                for (int i = 0; i < array.Length; i++)
                {
                    parse = int.Parse(array[i].ToString());
                    res[i] = add[i] * parse;
                }

                for (int i = 0; i < res.Length; i++)
                {
                    if (res[i] > 9)
                    {
                        int one = 1;
                        int two = res[i] % 10;
                        res[i] = one + two;
                    }
                    sumRes += res[i];
                }

                if (sumRes % 10 == 0)
                {
                    result = vat;
                }
                else
                {
                    result = "999999998";
                }

            }
            else
            {
                result = "999999998";
            }

            return result;
        }
    }
}