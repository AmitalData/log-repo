using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.DataContract;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.CloseTables;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.CustomsMessaging.Common.Gen;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;
using Contact = Simplog.Data.CommonDataModel.EntityPOCOs.Contact;
using DocumentType = Simplog.Data.CommonDataModel.EntityPOCOs.DocumentType;
using User = Simplog.Data.CommonDataModel.EntityPOCOs.User;

namespace Logitude.Accounting.BL.CoreBL
{
    public class TaxReportService
    {
        public TaxReportService()
        {

        }

        public static string FilePath = @"E:\PCN874.txt";
        private static Simplog.Data.CommonDataModel.EntityPOCOs.Card card;
        private static List<TaxReportData> ledgerTransactons;
        private static List<LedgerTransaction> journalsTransactions;
        public static List<TaxReportLinePM> CreateTaxReportLines(TaxReportPM taxReport, int tenant)
        {

                JournalRepository journalRepository = new JournalRepository(tenant);
                List<TaxReportData> TaxReportJournalData = journalRepository.GetARInvoiceJournals(taxReport.TaxReportMonth, tenant);

                ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(tenant);

               
                APInvoiceQuery aPInvoiceQueryService = new APInvoiceQuery(tenant);
                CardRepository cardRepository = new CardRepository(tenant);
                TenantQuery tenantQuery = new TenantQuery(tenant);
                TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);
                List<string> AccountingEntiyIds = TaxReportJournalData.Select(d => d.AccountingEntityId).ToList();

                List<ARInvoice> invoices = aRInvoiceRepository.GetARInvoicesByIds(taxReport.Tenant, AccountingEntiyIds);
                List<string> cardIds = invoices.Select(d => d.BillToId).ToList();

                List<Simplog.Data.CommonDataModel.EntityPOCOs.Card> cards = cardRepository.GetCardsByIds(cardIds, tenant).ToList();



                List<TaxReportLinePM> reportLinesList = new List<TaxReportLinePM>();

                //Outputs

                decimal? VatAmount = 0;
                decimal? InvoiceAmount = 0;
              
                foreach (TaxReportData a in TaxReportJournalData)
                {
                    string vatNumber = null;
                    var exist = reportLinesList.Where(d => d.JournalId == a.Id).Any();
                    if (!exist)
                    {
                        string outputreference = null;
                        ARInvoice invoice = invoices.Where(d => d.Id == a.AccountingEntityId).FirstOrDefault();
                        if (invoice != null)
                        {
                            VatAmount = invoice.TotalVAT != null ? invoice.TotalVAT : 0;
                            InvoiceAmount = invoice.TotalAmountForTaxReport != null ? invoice.TotalAmountForTaxReport : 0;
                            if (invoice.InvoiceNumber.Length == 9)
                            {
                                outputreference = invoice.InvoiceNumber.Substring(invoice.InvoiceNumber.Length - 9);
                            }
                            else
                            {
                                outputreference = invoice.InvoiceNumber;
                            }

                            if (!string.IsNullOrEmpty(invoice.VatNumber))
                            {
                                vatNumber = invoice.VatNumber;
                            }
                          
                            TaxReportLinePM line = new TaxReportLinePM()
                            {
                                VatNumber = vatNumber,
                                Reference = outputreference,
                                ReferecneGroup = "0000",
                                ReferenceDate = invoice.InvoiceDate,
                                JournalId = a.Id,
                                OutputOrInput = "O",
                                VatAmount = Math.Round(VatAmount.Value, MidpointRounding.AwayFromZero),
                                VatableInvoiceAmount = Math.Round(InvoiceAmount.Value, MidpointRounding.AwayFromZero),
                                IsManuallyChanged = false,
                                TransmitStatusCode = "1",
                                TaxReportId = taxReport.Id,
                                ChangeSetOp = ChangeSetOperation.Insert,
                                LastUpdateDateTime = DateTime.Now,
                                UpdatedByUserId = taxReport.UpdatedByUserId,
                                Tenant = tenant,


                            };
                            Simplog.Data.CommonDataModel.EntityPOCOs.Card card = cards.Where(d => d.Id == invoice.BillToId).FirstOrDefault();



                            //if (line.VatNumber == null)
                            //{
                            //  //  line.VatNumber = "999999999";

                            //}

                            if (line.VatNumber == tenantPM.VatNumber)
                            {
                                line.LineTypeCode = "M";

                            }
                            else if (card != null && card.IsAutonomy)
                            {
                                line.LineTypeCode = "I";
                            }
                            else
                            {
                                line.LineTypeCode = "S";
                            }

                            reportLinesList.Add(line);
                        }
                    }
                }


                //inputs
                LedgerTransactionRepository ledgerTransactionRepository = new LedgerTransactionRepository(tenant);
                ledgerTransactons = ledgerTransactionRepository.GetLedgerTransactionsForTaxReport(taxReport.TaxReportMonth, tenant);
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
                FullAccountingSettingRepository fullAccountingSettingRepository = new FullAccountingSettingRepository(tenant);
                FullAccountingSetting setting = fullAccountingSettingRepository.GetSingleFullAccountingSetting(tenant);
                JournalQueryService journalQueryService = new JournalQueryService(tenant);
               
              
                bool isEquipment = false;
                APInvoicePM aPInvoice = null;

                List<string> glAccountIds = ledgerTransactons.Select(d => d.OppositGLAccount).ToList();
                List<GLAccountPM> glAccounts = gLAccountQueryService.GetByGLAccountsIdList(glAccountIds, tenant);
                cards = cardRepository.GetCardsByGLAccountIds(glAccountIds, tenant).ToList();

                List<string> apInvoiceIds = ledgerTransactons.Where(d => d.AccountingEntity == "4").Select(d => d.AccountingEntityId).ToList();
                List<APInvoicePM> aPInvoices = aPInvoiceQueryService.GetAPInvoicesByIds(apInvoiceIds, tenant, taxReport.TaxReportMonth);

                List<string> JournalIds = ledgerTransactons.Where(d => d.JournalId != null).Select(d => d.JournalId).ToList();
                List<JournalPM> journalPMs = journalQueryService.GetJournalsByIds(JournalIds, tenant);
                journalsTransactions = ledgerTransactionRepository.GetLedgerTransactionsByJournalIds(JournalIds, tenant);
                List<string> ids = new List<string>();
                ids = aPInvoices.Select(d => d.Id).ToList();
                APInvoiceTotalVATQuery myTotalVATQuery = new APInvoiceTotalVATQuery(tenant);
                totalvats = new List<APInvoiceTotalVATPM>();
                totalvats = myTotalVATQuery.GetTotalVATs(ids, tenant);



            foreach (TaxReportData a in ledgerTransactons)
            {
                VatNumber = null;
                InputVatAmount = 0;
                InputInvoiceAmount = 0;
                card = cards.Where(d => d.GLAccountId == a.OppositGLAccount).FirstOrDefault();
                if (a.AccountingEntity == AccountingEntityValues.APInvoice)
                {
                    aPInvoice = aPInvoices.Where(d => d.Id == a.AccountingEntityId).FirstOrDefault();

                    if (aPInvoice != null)
                    {
                        SetVatFieldsForAPInvoiceTransaction(aPInvoice);
                    }
                    else
                    {
                        continue;
                    }
                }

                else if (card != null && card.PartnerTypeId == PartnerTypeValues.Customer)
                {
                    if (card.VatNumber == tenantPM.VatNumber)
                    {
                        VatNumber = card.VatNumber;
                    }
                    else
                    {
                        SetVatAmounts(a);
                    }


                }
                else
                {
                    SetVatAmounts(a);
                }

                if (VatNumber == null)
                    VatNumber = "000000000";

                GLAccountPM gLAccountPM = glAccounts.Where(d => d.Id == a.OppositGLAccount).FirstOrDefault();
                if (gLAccountPM != null)
                {
                    if (gLAccountPM.IsEquipmentVendor)
                    {
                        isEquipment = true;
                    }

                }
                SetReferenceFields(a.Reference);
                string transmitStatusCode = SetTransmitStatusByDocumentDate(a.ReferenceDate);
                TaxReportLinePM taxReportLine = new TaxReportLinePM()
                {

                    VatNumber = VatNumber,
                    Reference = reference,
                    ReferenceDate = a.ReferenceDate,
                    ReferecneGroup = referenceGroup,
                    JournalId = a.JournalId,
                    OutputOrInput = "I",
                    VatAmount = Math.Round(InputVatAmount.Value, MidpointRounding.AwayFromZero),
                    VatableInvoiceAmount = Math.Round(InputInvoiceAmount.Value, MidpointRounding.AwayFromZero),
                    IsEquipment = isEquipment,
                    IsManuallyChanged = true,
                    TaxReportId = taxReport.Id,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    LastUpdateDateTime = DateTime.Now,
                    UpdatedByUserId = taxReport.UpdatedByUserId,
                    Tenant = tenant,
                    TransmitStatusCode = transmitStatusCode,


                };




                JournalPM journal = journalPMs.Where(d => d.Id == a.JournalId).FirstOrDefault();
                string CreditAccountId = null;
                string accountTypeCode = null;
                //if (journal.JournalLines.Count > 0)
                //{
                //    CreditAccountId = journal.JournalLines.FirstOrDefault().CreditAccountId;
                //    accountTypeCode = journal.JournalLines.FirstOrDefault().AccountTypeCode;
                //}



                //  GLAccountPM account = gLAccountQueryService.GetSingle(CreditAccountId, false, false);

               
                    if (aPInvoice != null && (aPInvoice.VATNumber == tenantPM.VatNumber))
                    {
                        taxReportLine.LineTypeCode = "C";
                    }

                    else if ( journal.LineCounter>0 && journal.LineCreditAccountId == setting.CustomsGLAccountId)
                    {

                        taxReportLine.LineTypeCode = "R";

                    }
                    else if (card != null && card.IsAutonomy)
                    {
                        taxReportLine.LineTypeCode = "P";
                    }

                    else if (journal.LineCreditAccountTypeCode != "3" || (journal.LineCreditAccountTypeCode == "3" && (gLAccountPM!= null && gLAccountPM.Smallcashbook==true)))
                    {
                        taxReportLine.LineTypeCode = "K";
                    }
                    else
                    {
                        taxReportLine.LineTypeCode = "T";
                    }




                    reportLinesList.Add(taxReportLine);


                }


                CalculateReportTotals(taxReport, reportLinesList);

                taxReport.TaxableOutputsWithDiffPercent = 0;
                taxReport.OutputTaxAmountWithDiffPercent = 0;

                //taxReport.AmountForPayRefund = taxReport.OutputTaxAmount - (taxReport.OtherInputsTaxAmount + taxReport.EquipmentInputsTaxAmount);
                //if (taxReport.AmountForPayRefund == null)
                //{
                //    taxReport.AmountForPayRefund = 0;
                //}
                taxReport.StatusCode = "D";
                taxReport.ProcessEndDate = DateTime.Now;


                // saving report
                taxReport.ChangeSetOp = ChangeSetOperation.Update;

            using (TransactionScope scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(60)))
            {

                IAccountingContext MyContext = AccountingContext.GetContext(taxReport.Tenant);
                TaxReportUpdateService updateService = new TaxReportUpdateService(MyContext, new Dictionary<string, IContext>(), taxReport.Tenant);
                TaxReportLineUpdateService lineUpdateService = new TaxReportLineUpdateService(MyContext, new Dictionary<string, IContext>(), taxReport.Tenant);
                updateService.Update(taxReport, true, TimeSpan.FromMinutes(60));

               // saving lines
                int count = 0;
                //int submitChangesCounter
                foreach (TaxReportLinePM linePM in reportLinesList)
                {
                    linePM.Line = ++count;
                    linePM.ChangeSetOp = ChangeSetOperation.Insert;
                    linePM.UpdatedByUserId = taxReport.UpdatedByUserId;
                    
                    lineUpdateService.Update(linePM, true,TimeSpan.FromMinutes(60));//the problem is here it loops on more than 3000  lines and updates them one by one ,each update will have to get single tenant and get single currency along with multible db gets which make the db to time out for the opened transaction
                }


                scope.Complete();

                return reportLinesList;

            }


        }

        static void SetVatFieldsForAPInvoiceTransaction(APInvoicePM aPInvoice)
        {
            aPInvoice.TotalVATs = totalvats.Where(d => d.APInvoiceId == aPInvoice.Id).ToList();
            VatNumber = aPInvoice.VATNumber;
            if (aPInvoice.StatusCode == "AC")
            {
                InputVatAmount = (decimal?)aPInvoice.TotalVATs.Sum(d => d.LocalVATAmount)*-1;
                InputInvoiceAmount = (decimal?)aPInvoice.SubTotalInLocalCurrency*-1 ?? 0;
            }
            else if (aPInvoice.StatusCode == "AD")
            {
                InputVatAmount = (decimal?)aPInvoice.TotalVATs.Sum(d => d.LocalVATAmount);
                InputInvoiceAmount = (decimal?)aPInvoice.SubTotalInLocalCurrency ?? 0;
            }
        }
        static List<APInvoiceTotalVATPM> totalvats;
        static string VatNumber = null;
        static decimal? InputVatAmount = 0;
        static decimal? InputInvoiceAmount = 0;
        static string  reference = null;
        static string referenceGroup = null;
        private static void SetReferenceFields(string Reference)
        {
            reference = null;
            referenceGroup = null;
            if (Reference != null)
            {
                if (Reference.Contains("-"))
                {

                    Reference = Reference.Replace("-", "");
                }
                if (Reference.Length > 20)
                {
                    Reference = Reference.Substring(0, 19);
                }
            
                Regex isMatche = new Regex("([A-Za-z])");
                bool letters = isMatche.IsMatch(Reference);
                if (letters)
                {
                    for (int i = 0; i < Reference.Length; i++)
                    {
                        string d = Reference.Substring(i, 1);
                        MatchCollection match = Regex.Matches(d, @"^[a-zA-Z]*$");
                        if (match.Count != 0)
                        {
                            referenceGroup = referenceGroup + d;// Reference.Substring(0, i);

                        }
                        else
                        {
                            reference = Reference.Substring(i, Reference.Length - i);
                            break;
                        }
                       
                    }
                }
                else
                {
                    reference = Reference;
                    referenceGroup = "0000";
                }

            }
            else
            {
                referenceGroup = "0000";
            }
        }

       private static void SetVatAmounts(TaxReportData report)
        {
            InputVatAmount = report.LocalAmountDebit;

            if (card != null && card.PartnerTypeId == PartnerTypeValues.Vendor)
                VatNumber = card.VatNumber;
          
            var transactionSum = journalsTransactions.Where(d => d.JournalId == report.JournalId && d.Reference1 == report.Reference).Sum(d => d.LocalAmountCredit);
            InputInvoiceAmount = transactionSum - InputVatAmount;

        }

        private static string SetTransmitStatusByDocumentDate(DateTime referenceDate)
        {
            DateTime date = DateTime.Now.AddDays(-180);
            DateTime last180days = new DateTime(date.Year, date.Month, 1);
            if (referenceDate <= last180days)
            {
                return "3";
            }
            else return "1";
        }

        public static BatchTaskExecutionPM CreatePNCFileInBatch(string taxReportId, int tenant)
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
                    Subject = "Create PNC Flat file for Tax Report",
                    Tenant = tenant,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    ClassName = "Logitude.Accounting.BL.CoreBL.Batch.BatchTaxReportService,Logitude.Accounting.BL",
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

        public static DocumentsFilingPM CreatePNC874File(string taxReportId, int tenant, string createUserId)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                //get tax report
                TaxReportQueryService reportQS = new TaxReportQueryService(tenant);
                TaxReportPM taxReport = reportQS.GetSingle(taxReportId, false, false);
                IAccountingContext MyContext = AccountingContext.GetContext(tenant);

                //DECLARATIONS
                StringBuilder myStringBuilder = new StringBuilder();
                List<string> errors = new List<string>();

                //Get logged user -> show local
                Contact user = GetContact(createUserId, tenant);
                bool showLocal = !user.DontShowLocalLabels;

                if (errors.Count > 0)
                {
                    string errorsString = "";
                    foreach (var error in errors)
                    {
                        errorsString += error + ';';
                    }
                    throw new ApplicationException(errorsString.TrimEnd(';'));
                }

                try
                {



                    //
                    // Line: [1] 
                    //

                    string firstLine = "";
                    myStringBuilder.Append("O");

                    myStringBuilder.Append(FormatString(taxReport.VatNumber, 9, paddingDigit: '0'));

                    myStringBuilder.Append(taxReport.TaxReportMonth == null ? "000000" : taxReport.TaxReportMonth.ToString("yyyyMM"));
                    myStringBuilder.Append("1");
                    myStringBuilder.Append(taxReport.CreateDate.ToString("yyyyMMdd"));

                    //TotalTaxableOutputAmount
                    myStringBuilder.Append(FormatDecimal(taxReport.TaxableOutputAmount, 11, true, true, showLocal));

                    //OutputTaxAmount
                    myStringBuilder.Append(FormatDecimal(taxReport.OutputTaxAmount, 9, true, true, showLocal));

                    //TaxableOutputsWithDiffPercent
                    myStringBuilder.Append("+");
                    myStringBuilder.Append(FormatDecimal(taxReport.TaxableOutputsWithDiffPercent, 11, false, true, showLocal));

                    //OutputTaxAmountWithDiffPercent
                    myStringBuilder.Append("+");
                    myStringBuilder.Append(FormatDecimal(taxReport.OutputTaxAmountWithDiffPercent, 9, showLocalError: showLocal, includeSign: false, truncateDecimal: true));

                    //OutputLinesCount
                    myStringBuilder.Append(FormatInt(taxReport.OutputLinesCount, 9, showLocal));

                    //ExemptTaxableOutput
                    myStringBuilder.Append(FormatDecimal(taxReport.ExemptTaxableOutput, 11, includeSign: true, truncateDecimal: true, showLocalError: showLocal));

                    //OtherInputsTaxAmount
                    myStringBuilder.Append(FormatDecimal(taxReport.OtherInputsTaxAmount, 9, showLocalError: showLocal, includeSign: true, truncateDecimal: true));

                    //EquipmentInputsTaxAmount
                    myStringBuilder.Append(FormatDecimal(taxReport.EquipmentInputsTaxAmount, 9, showLocalError: showLocal, includeSign: true, truncateDecimal: true));

                    //InputLinesCount
                    myStringBuilder.Append(FormatInt(taxReport.InputLinesCount, 9, showLocal));

                    //AmountForPayRefund
                    myStringBuilder.Append(FormatDecimal(taxReport.AmountForPayRefund, 11, showLocalError: showLocal, includeSign: true, truncateDecimal: true));

                    myStringBuilder.AppendLine();


                    //
                    // Line: [report lines]
                    //

                    TaxReportLineListQueryService trLineQS = new TaxReportLineListQueryService(MyContext);
                    IQueryable<TaxReportLineList> linesIQ = trLineQS.GetReportLines(taxReport.Id, tenant);
                    List<TaxReportLineList> lines = linesIQ.Where(a => a.TransmitStatusCode == "1").OrderBy(d => d.Line).ToList();

                    foreach (TaxReportLineList lineList in lines)
                    {
                        // validations
                        if (lineList.ReferenceDate == null) throw new ApplicationException("Reference Date is empty! line:" + lineList.Line);

                        //create line 
                        myStringBuilder.Append(lineList.LineTypeCode);

                        myStringBuilder.Append(FormatString(lineList.VatNumber, 9, paddingDigit: '0'));

                        myStringBuilder.Append(lineList.ReferenceDate.Value.ToString("yyyyMMdd"));

                        myStringBuilder.Append(FormatString(lineList.ReferecneGroup, 4, paddingDigit: '0'));

                        myStringBuilder.Append(FormatString(lineList.Reference, 9, paddingDigit: '0'));

                        //VatAmount
                        myStringBuilder.Append(FormatDecimal(lineList.VatAmount, 9, showLocalError: showLocal, includeSign: false, truncateDecimal: true));

                        //VatableInvoiceAmount
                        myStringBuilder.Append(FormatDecimal(lineList.VatableInvoiceAmount, 10, showLocalError: showLocal, includeSign: true, truncateDecimal: true));


                        myStringBuilder.Append("000000000");

                        myStringBuilder.AppendLine();

                    }


                    //
                    // Line: [last one]
                    //
                    string lastLine = "";
                    lastLine += "X";
                    lastLine += taxReport.VatNumber.PadLeft(9, '0');
                    myStringBuilder.Append(lastLine);



                    //// write to a file
                    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(FilePath))
                    //{
                    //    foreach (string line in linesArray)
                    //    {
                    //        file.WriteLine(line);

                    //    }
                    //}

                    //update entity
                    TaxReportUpdateService updateService = new TaxReportUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                    taxReport.ChangeSetOp = ChangeSetOperation.Update;
                    taxReport.StatusCode = "T"; // T- Transmitted
                    taxReport.NeedsRebulid = false;
                    updateService.Update(taxReport, true);


                }
                catch (ApplicationException ex)
                {
                    //update entity
                    TaxReportUpdateService updateService = new TaxReportUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                    taxReport.ChangeSetOp = ChangeSetOperation.Update;
                    taxReport.NeedsRebulid = true;
                    taxReport.StatusCode = "T"; // T- Transmitted
                    updateService.Update(taxReport, true);

                    throw ex;
                }

                DocumentsFilingPM docOut = CreateDocumnetFiling(myStringBuilder, taxReport);

                scope.Complete();

                return docOut;
            }
        }
        public static void CalculateReportTotals(TaxReportPM taxReportPM, List<TaxReportLinePM> lines)
        {
            if (lines.Count > 0)
            {
                var outputLines = lines.Where(d => d.OutputOrInput == "O");
                var inputLines = lines.Where(d => d.OutputOrInput == "I");

                // OUTPUT
                taxReportPM.TaxableOutputAmount = outputLines.Where(d => d.VatAmount != 0).Sum(d => d.VatableInvoiceAmount);
                taxReportPM.OutputTaxAmount = outputLines.Where(d => d.VatAmount != 0).Sum(d => d.VatAmount);
                taxReportPM.ExemptTaxableOutput = outputLines.Where(d => d.VatAmount == 0 && d.TransmitStatusCode == TaxReportLineTransmitStatusValues.Fortransmit).Sum(d => d.VatableInvoiceAmount);
                taxReportPM.OutputLinesCount = outputLines.Count();

                // INPUTS
                taxReportPM.OtherInputsTaxAmount = inputLines.Where(d => d.TransmitStatusCode == TaxReportLineTransmitStatusValues.Fortransmit && d.IsEquipment == false).Sum(d => d.VatAmount);
                taxReportPM.EquipmentInputsTaxAmount = inputLines.Where(d => d.IsEquipment == true).Sum(d => d.VatAmount);
                taxReportPM.InputLinesCount = inputLines.Where(d => d.TransmitStatusCode == TaxReportLineTransmitStatusValues.Fortransmit).Count();
            
                taxReportPM.AmountForPayRefund = taxReportPM.OutputTaxAmount - (taxReportPM.OtherInputsTaxAmount + taxReportPM.EquipmentInputsTaxAmount);
                if (taxReportPM.AmountForPayRefund == null) taxReportPM.AmountForPayRefund = 0;

            }
        }


        private static DocumentsFilingPM CreateDocumnetFiling(StringBuilder lines, TaxReportPM taxReport, bool isFromWR = false)
        {
            // prepare file string
            string file = string.Join(Environment.NewLine, lines);

            // create document
            int tenant = taxReport.Tenant;
            ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
            DocumentsFilingService docService = new DocumentsFilingService(MyContext, tenant);
            DocumentTypeRepository docTypeReposioty = new DocumentTypeRepository(MyContext);

            ObjectTableRepository tableRep = new ObjectTableRepository(tenant);
            ObjectTable table = tableRep.GetObjectTableByName("TaxReport", 0, true);

            // user
            User loggedUser = GetLoggedUser(tenant);
            DocumentType docType = docTypeReposioty.GetSingleDocumentTypeByCode("PCN874", tenant);

            if (docType == null)
                throw new ApplicationException("There is no document type for this report!");

            string _code = CodeCounter.GetNumber("DocumentsFiling", tenant).ToString();
            DocumentsFilingPM document = new DocumentsFilingPM()
            {
                Description = "PCN874 Text File",
                DocumentTypeId = docType.Id,
                Tenant = tenant,
                DirectionCode = "I",
                EntityId = taxReport.Id,
                EntityNumber = taxReport.TaxReportNumber,
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

            document.FileData = file.Select(d => Convert.ToByte(d)).ToArray();
            var files = file.Select(d => Convert.ToByte(d)).ToArray();


            docService.Create(document, files, loggedUser.Id);


            //get document out
            DocumentsFilingQuery queryService = new DocumentsFilingQuery(tenant);
            string code = (Convert.ToInt32(_code) + 1).ToString();
            DocumentsFilingPM docFiling = queryService.GetDocumentsFilingByDocumentCode(code, tenant);


            return docFiling;
        }

        private static Contact GetContact(string createUserId, int tenant)
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact loggedContact;
            loggedContact = contactRepository.GetSingleContact(createUserId, tenant);
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

        private static string FormatDecimal(decimal? number, int wordSize, bool includeSign = false, bool truncateDecimal = true, bool showLocalError = false)
        {
            string result = "";

            //catch nulls
            if (!number.HasValue)
            {
                number = 0;
            }


            //truncate
            if (truncateDecimal)
            {
                number = Math.Round(number.Value);
            }

            //big size
            if (number.ToString().Length > wordSize)
            {

                string msg = TextCodesTranslator.TranslateText("TaxReport.O.FileBigNumber", 0, showLocalError);
                throw new ApplicationException(msg);
            }

            //sign
            if (includeSign)
            {
                result += number >= 0 ? '+' : '-';
            }

            //abs
            number = Math.Abs(number.Value);

            //padding left
            result += number.ToString().PadLeft(wordSize, '0');

            return result;
        }
        private static string FormatInt(int? number, int wordSize, bool showLocal = false)
        {
            string result = "";

            //catch nulls
            if (!number.HasValue)
            {
                number = 0;
            }

            //big size
            if (number.ToString().Length > wordSize)
            {
                string msg = TextCodesTranslator.TranslateText("TaxReport.O.FileBigNumber", 0, showLocal);
                throw new ApplicationException(msg);
            }

            //abs
            number = Math.Abs(number.Value);

            //padding left
            result += number.ToString().PadLeft(wordSize, '0');

            return result;
        }
        private static string FormatString(string str, int wordSize, char paddingDigit = ' ')
        {
            string result = "";

            //catch nulls
            if (string.IsNullOrEmpty(str))
            {
                str = paddingDigit.ToString();
            }

            //big size
            if (str.Length > wordSize)
            {
                str = str.Substring(0, wordSize);
                //throw new ApplicationException("There is a string with big value!");
            }

            //padding left
            result += str.PadLeft(wordSize, paddingDigit);

            return result;
        }


        public static BatchTaskExecutionPM CreateTaxReportFileInBatch(string taxReportId, int tenant)
        {
           
            PNCFileArgs args = new PNCFileArgs() { ReportId = taxReportId, Tenant = tenant };
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(PNCFileArgs));
            serializer.Serialize(stringwriter, args);
            string xmlParameters = stringwriter.ToString();
            BatchTaskExecutionPM taskExe = null;
            taskExe = new BatchTaskExecutionPM()
            {
                Subject = "Create a new Tax Report",
                Tenant = tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ClassName = "Logitude.Accounting.BL.CoreBL.Batch.BatchCreateTaxReportService,Logitude.Accounting.BL",
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
            return taskExe;
        }


        }
    public class PNCFileArgs
    {
        //public TaxReportPM ReportPM { get; set; }
        public string ReportId { get; set; }
        public int Tenant { get; set; }
        public bool TestingMode { get; set; }
    }
}
