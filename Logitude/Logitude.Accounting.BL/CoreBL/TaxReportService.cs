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
        private static List<CustomTaxReportData> ledgerTransactons;
        private static List<LedgerTransaction> journalsTransactions;
        private static List<Simplog.Data.CommonDataModel.EntityPOCOs.Card> cards;
        private static List<GLAccountPM> oppositeAccounts;
        private static List<GLAccountPM> gLAccounts;
        private static List<GLAccountCurrencyPM> gLAccountCurrencies;
        private static List<GLAccountPM> parentGLAccounts;
        const string StatusCode_VATAmountInTheRecordIsHigherThanThePercentageOfVATAllowed = "9";
        const int maxAllowedLinesCount = 3000;
        const string CreatedStatusCode = "C";
        static bool CheckLastNineAreNine(string input)
        {
            if (string.IsNullOrEmpty(input) || input.Length < 9)
            {
                return false; // If string length is less than 9, it's impossible for the last 9 characters to be '9'
            }

            string lastNine = input.Substring(input.Length - 9); // Get the last 9 characters

            foreach (char c in lastNine)
            {
                if (c != '9')
                {
                    return false; // If any character in the last 9 is not '9', return false
                }
            }

            return true; // All characters are '9'
        }
        public static List<TaxReportLinePM> CreateTaxReportLines(TaxReportPM taxReport, int tenant)
        {

            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
            FullAccountingSetting setting = GetTenantFullAccountingSetting(tenant);
            JournalQueryService journalQueryService = new JournalQueryService(tenant);
            JournalRepository journalRepository = new JournalRepository(tenant);
            LedgerTransactionRepository ledgerTransactionRepository = new LedgerTransactionRepository(tenant);
            ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(tenant);
            GLAccountCurrencyQueryService gLAccountCurrencyQueryService = new GLAccountCurrencyQueryService(tenant);
            APInvoiceTotalVATQuery myTotalVATQuery = new APInvoiceTotalVATQuery(tenant);
            APInvoiceQuery aPInvoiceQueryService = new APInvoiceQuery(tenant);
            CardRepository cardRepository = new CardRepository(tenant);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);

            List<CustomTaxReportData> TaxReportJournalData = journalRepository.GetARInvoiceJournals(taxReport.TaxReportMonth, tenant);




            List<string> AccountingEntiyIds = TaxReportJournalData.Select(d => d.AccountingEntityId).ToList();

            List<ARInvoice> invoices = aRInvoiceRepository.GetARInvoicesByIds(taxReport.Tenant, AccountingEntiyIds);
            List<string> cardIds = invoices.Select(d => d.BillToId).ToList();

            cards = cardRepository.GetCardsByIds(cardIds, tenant).ToList();



            List<TaxReportLinePM> reportLinesList = new List<TaxReportLinePM>();

            //Outputs

            decimal? VatAmount = 0;
            decimal? InvoiceAmount = 0;
            string transmitStatus;
            foreach (CustomTaxReportData taxData in TaxReportJournalData)
            {
                string vatNumber = null;
                transmitStatus = "1";
                var exist = reportLinesList.Where(d => d.JournalId == taxData.Id).Any();
                if (!exist)
                {
                    string outputreference = null;
                    ARInvoice invoice = invoices.Where(d => d.Id == taxData.AccountingEntityId).FirstOrDefault();
                    if (invoice != null)
                    {
                        transmitStatus = SetTransmitStaus(invoice.InvoiceDate.Value, taxReport.TaxReportMonth, setting);
                        VatAmount = invoice.TotalVAT != null ? invoice.TotalVAT : 0;
                        InvoiceAmount = invoice.TotaVatableAmountForTaxReport != null ? invoice.TotaVatableAmountForTaxReport : 0;
                        //if (invoice.InvoiceNumber.Length > 9)
                        //{
                        //    outputreference = invoice.InvoiceNumber.Substring(invoice.InvoiceNumber.Length - 9);
                        //}
                        //else
                        //{
                        outputreference = invoice.CustomerRef != null ? invoice.CustomerRef : invoice.InvoiceNumber;
                        //}

                        if (!string.IsNullOrEmpty(invoice.VatNumber))
                        {
                            vatNumber = invoice.VatNumber;
                            vatNumber = ModifyVatNumber(vatNumber);
                        }

                        TaxReportLinePM line = new TaxReportLinePM()
                        {
                            VatNumber = vatNumber,
                            Reference = outputreference,
                            OriginalReference = invoice.InvoiceNumber,
                            ReferecneGroup = null,
                            ReferenceDate = invoice.InvoiceDate,
                            JournalId = taxData.Id,
                            OutputOrInput = "O",
                            VatAmount = Math.Round(VatAmount.Value, MidpointRounding.AwayFromZero),
                            VatAmountRound = VatAmount.Value - Math.Round(VatAmount.Value, MidpointRounding.AwayFromZero),
                            VatableInvoiceAmount = Math.Round(InvoiceAmount.Value, MidpointRounding.AwayFromZero),
                            TotalInvoiceAmount = invoice.TotalAmountForTaxReport != null ? Math.Round(invoice.TotalAmountForTaxReport.Value, MidpointRounding.AwayFromZero) : 0,
                            SubTotalInLocalCurrency = invoice.SubTotalInLocalCurrency != null ? Math.Round(invoice.SubTotalInLocalCurrency.Value, MidpointRounding.AwayFromZero) : 0,
                            IsManuallyChanged = false,
                            TransmitStatusCode = transmitStatus,
                            TaxReportId = taxReport.Id,
                            ChangeSetOp = ChangeSetOperation.Insert,
                            LastUpdateDateTime = DateTime.Now,
                            UpdatedByUserId = taxReport.UpdatedByUserId,
                            UpdatedBUserName = taxReport.UpdatedByUserName,
                            Tenant = tenant,
                            TaxReportDate = taxReport.TaxReportMonth,
                            LedgerTransactionId = taxData.LedgerTransactionId,
                            ConfirmationNumber = invoice.ConfirmationNumber,
                        };
                        Simplog.Data.CommonDataModel.EntityPOCOs.Card card = cards.Where(d => d.Id == invoice.BillToId).FirstOrDefault();

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
                        //  UpdateJournalAdditionalDataRecord(line, null);
                    }
                }
            }


            //inputs
            ledgerTransactons = ledgerTransactionRepository.GetLedgerTransactionsForTaxReport(taxReport.TaxReportMonth, tenant);

            List<string> JournalIds = ledgerTransactons.Where(d => d.JournalId != null).Select(d => d.JournalId).ToList();
            List<JournalPM> journalPMs = journalQueryService.GetJournalsByIds(JournalIds, tenant);
            journalsTransactions = ledgerTransactionRepository.GetLedgerTransactionsByJournalIds(JournalIds, tenant);

            bool isEquipment = false;
            List<string> glAccountIds = journalsTransactions.Select(d => d.AccountId).ToList();
            List<string> oppositeglAccountIds = ledgerTransactons.Select(d => d.OppositGLAccount).ToList();
            gLAccountCurrencies = gLAccountCurrencyQueryService.GetGLAccountCurrenciesByAccountIds(tenant, oppositeglAccountIds);

            oppositeAccounts = gLAccountQueryService.GetByGLAccountsIdList(oppositeglAccountIds, tenant);
            gLAccounts = gLAccountQueryService.GetByGLAccountsIdList(glAccountIds, tenant);
            gLAccountCurrencies.AddRange(gLAccountCurrencyQueryService.GetGLAccountCurrenciesByAccountIds(tenant, glAccountIds));
            parentGLAccounts = gLAccountQueryService.GetByGLAccountsIdList(gLAccountCurrencies.Select(d => d.MainGLAccountId).ToList(), tenant);

            List<string> apInvoiceIds = ledgerTransactons.Where(d => d.AccountingEntity == "4").Select(d => d.AccountingEntityId).ToList();
            List<APInvoicePM> voidedAPInvoices = aPInvoiceQueryService.GetVoidedAPInvoicesByIds(apInvoiceIds, tenant, taxReport.TaxReportMonth);
            List<APInvoicePM> allAPInvoices = aPInvoiceQueryService.GetAllAPInvoicesByIds(apInvoiceIds, tenant);

            glAccountIds.AddRange(oppositeglAccountIds);
            glAccountIds.AddRange(parentGLAccounts.Select(d => d.Id).ToList());
            cards = cardRepository.GetCardsByGLAccountIds(glAccountIds, tenant).ToList();

            //List<string> ids = new List<string>();
            //ids = apinvoicesNotInTaxMonthAndNotVoided.Select(d => d.Id).ToList();
            //totalvats = new List<APInvoiceTotalVATPM>();
            //totalvats = myTotalVATQuery.GetTotalVATs(ids, tenant);



            foreach (CustomTaxReportData transaction in ledgerTransactons.Where(x => x.TaxReportId == null || x.TransmitStatusCode == "2" || x.TransmitStatusCode == null))
            {
                VatNumber = null;
                InputVatAmount = 0;
                InputInvoiceAmount = 0;

                bool voidedAPInvoiceTaxMonthTransaction = CheckIfAPInvoiceTaxMonthTransactionIsVoided(taxReport, voidedAPInvoices, transaction);

                GLAccountPM account = GetAccountByLedgerTransaction(transaction);
                card = GetGLAccountCard(account);

                if (account != null)
                {
                    if (account.AccountTypeCode == "3" || account.AccountTypeCode == "2" || account.AccountTypeCode == "1")
                    {
                        VatNumber = card != null ? card.VatNumber : null;
                        VatNumber = ModifyVatNumber(VatNumber);
                    }
                }
                VatNumber = VatNumber == null ? "000000000" : VatNumber;
                SetVatAmounts(transaction);


                string transmitStatusCode = SetTransmitStatusByDocumentDate(transaction.ReferenceDate, setting.VATreportEveryTwoMonths);

                if (voidedAPInvoices.Any(x => x.Id == transaction.AccountingEntityId))
                {
                    transmitStatusCode = SetTransmitStatusForVoidedInvoiceTransaction(ledgerTransactons, transaction, transmitStatusCode);
                }

                APInvoicePM aPInvoicePM = allAPInvoices.Find(a => a.Id == transaction.AccountingEntityId);
                TaxReportLinePM inputReportLine = new TaxReportLinePM()
                {

                    VatNumber = VatNumber,
                    Reference = transaction.Reference,
                    OriginalReference = transaction.Reference,
                    ReferenceDate = transaction.ReferenceDate,
                    ReferecneGroup = null,
                    JournalId = transaction.JournalId,
                    OutputOrInput = "I",
                    VatAmount = Math.Round(InputVatAmount.Value, MidpointRounding.AwayFromZero),
                    VatAmountRound = InputVatAmount.Value - Math.Round(InputVatAmount.Value, MidpointRounding.AwayFromZero),
                    VatableInvoiceAmount = 0,// Math.Round(InputInvoiceAmount.Value, MidpointRounding.AwayFromZero),
                    TotalInvoiceAmount = Math.Round(InputInvoiceAmount.Value, MidpointRounding.AwayFromZero),
                    SubTotalInLocalCurrency = Math.Round(((double)InputInvoiceAmount.Value), MidpointRounding.AwayFromZero),
                    IsEquipment = aPInvoicePM != null ? aPInvoicePM.IsEquipment : account != null ? account.IsEquipmentVendor : false,
                    IsManuallyChanged = true,
                    TaxReportId = taxReport.Id,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    LastUpdateDateTime = DateTime.Now,
                    UpdatedByUserId = taxReport.UpdatedByUserId,
                    UpdatedBUserName = taxReport.UpdatedByUserName,
                    Tenant = tenant,
                    TransmitStatusCode = transmitStatusCode,
                    TaxReportDate = taxReport.TaxReportMonth,
                    JournalLineNumber = transaction.JournalLineNumber,
                    LedgerTransactionId = transaction.LedgerTransactionId,
                    ConfirmationNumber = aPInvoicePM != null ? aPInvoicePM.ConfirmationNumber : null,

                };

                JournalPM journal = journalPMs.Where(d => d.Id == transaction.JournalId && d.TaxReportJournalLineNumber == transaction.JournalLineNumber).FirstOrDefault();
                if (inputReportLine.ConfirmationNumber == null && journal.ConfirmationNumber != null)
                {
                    inputReportLine.ConfirmationNumber = journal.ConfirmationNumber;
                }

                if (aPInvoicePM != null && CheckLastNineAreNine(aPInvoicePM.ConfirmationNumber))
                {
                    inputReportLine.LineTypeCode = "H";
                    inputReportLine.ConfirmationNumber = null;
                }
                else if (aPInvoicePM != null && aPInvoicePM.VATNumber == tenantPM.VatNumber)
                {
                    inputReportLine.LineTypeCode = "C";
                }
                else if (journal.LineCounter > 0 && journal.LineCreditAccountId != null && journal.LineCreditAccountId == setting.CustomsGLAccountId)
                {

                    inputReportLine.LineTypeCode = "R";
                    inputReportLine.VatNumber = inputReportLine.Reference;
                    inputReportLine.Reference = "000000000";
                    inputReportLine.ReferecneGroup = "0000";
                   

				}
                else if (card != null && card.IsAutonomy)
                {
                    inputReportLine.LineTypeCode = "P";
                }

                else if ((journal.LineCreditAccountTypeCode == "3" && (account != null && account.Smallcashbook == true)))
                {
                    inputReportLine.LineTypeCode = "K";
                }
                else if ((journal.LineCreditAccountTypeCode == "3" && (account != null && account.ReportingAsAnotherDocument == true)))
                {

                    inputReportLine.LineTypeCode = "H";
                }
                else
                {
                    inputReportLine.LineTypeCode = "T";
                }


                reportLinesList.Add(inputReportLine);
                //     UpdateJournalAdditionalDataRecord(inputReportLine, transaction);

            }


            CalculateReportTotals(taxReport, reportLinesList);

            taxReport.TaxableOutputsWithDiffPercent = 0;
            taxReport.OutputTaxAmountWithDiffPercent = 0;


            taxReport.StatusCode = "D";

            DateTime stopLogAt = new DateTime(2023, 06, 01);
            string text = "TaxReportService.CreateTaxReportLines(*1*): " + taxReport.Id + " taxReport.StatusCode : " + taxReport.StatusCode;
            ULog(text, stopLogAt);

            taxReport.ProcessEndDate = DateTime.Now;


            // saving report
            MarkCreatedDuplicateLines(taxReport, reportLinesList);
            UpdateTaxReport(taxReport);
            reportLinesList = HandleTaxReportLines(taxReport, reportLinesList);
            return reportLinesList;
        }

        private static void ULog(string text, DateTime stopLogAt)
        {
            string log_text = text + System.Environment.NewLine;
            log_text = log_text + String.Format("{0:HH:mm:ss.ffff}", DateTime.Now.ToString()) + System.Environment.NewLine;
            System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
            log_text = log_text + t.ToString();
            LogitudeSettings.HandleLogMe(log_text, false, "TaxReportPMToPOCO", new DateTime(2023, 6, 1));
        }


        private static void MarkCreatedDuplicateLines(TaxReportPM taxReportPM, List<TaxReportLinePM> taxReportLines)
        {
            //List<TaxReportLinePM> taxReportLines = GetTaxReportLines(taxReportPM.Id, taxReportPM.Tenant);
            if (taxReportLines != null && taxReportLines.Count > 0)
            {
                int i = 1;
                taxReportLines.ForEach(l =>
                {
                    l.Line = i++;
                });

                var dup = taxReportLines.GroupBy(ln =>
                new
                {
                    VatNumber = ln.VatNumber,
                    Reference = ln.Reference
                }).OrderByDescending(g => g.Key.VatNumber).ThenBy(g => g.Key.Reference);

                List<DupLines> duplicates = null;

                var duplicates_indicative = dup.Select(g => new DupLines
                {
                    VatNumber = g.Key.VatNumber,
                    Reference = g.Key.Reference,
                    LineNumbers = g.OrderBy(x => x.Line).Skip(1).Select(x => x.Line)
                })
                .Where(r => r.LineNumbers.Count() >= 1).Select(r => r.VatNumber + "~" + r.Reference).ToList();

                if (duplicates_indicative != null && duplicates_indicative.Count > 0)
                {
                    duplicates = dup.Where(g => duplicates_indicative.Contains(g.Key.VatNumber + "~" + g.Key.Reference)).Select(g => new DupLines
                    {
                        VatNumber = g.Key.VatNumber,
                        Reference = g.Key.Reference,
                        LineNumbers = g.OrderBy(x => x.Line).Select(x => x.Line)
                    })
                    .Where(r => r.LineNumbers.Count() >= 1).ToList();
                }


                //    List<TaxReportLinePM> removeDupLines = new List<TaxReportLinePM>();


                if (duplicates != null && duplicates.Count > 0)
                {

                    List<int> all_dup_line_nos = new List<int>();
                    foreach (var item in duplicates)
                    {
                        all_dup_line_nos.AddRange(item.LineNumbers);
                    }

                    //       List<TaxReportLinePM> duplicateLines = new List<TaxReportLinePM>();

                    foreach (var oneLine in taxReportLines)
                    {
                        if (all_dup_line_nos.Contains(oneLine.Line))
                        {
                            //  oneLine.ChangeSetOp = ChangeSetOperation.Update;
                            oneLine.StatusCode = TaxReportLineStatusValues.DuplicateThereisanothertransactionwiththesameVATNoandReference;
                            //   duplicateLines.Add(oneLine);
                        }
                        else if (oneLine.StatusCode == TaxReportLineStatusValues.DuplicateThereisanothertransactionwiththesameVATNoandReference)
                        {
                            //   oneLine.ChangeSetOp = ChangeSetOperation.Update;
                            oneLine.StatusCode = "6";
                            //    removeDupLines.Add(oneLine);
                        }

                    }

                    //   if (duplicateLines.Count > 0 || removeDupLines.Count > 0)
                    //   {
                    //  accountingContext = AccountingContext.GetContext(taxReportPM.Tenant);
                    //  taxReportLineUpdateService = new TaxReportLineUpdateService(accountingContext, new Dictionary<string, IContext>(), taxReportPM.Tenant);
                    //  if (duplicateLines.Count > 0) taxReportLineUpdateService.UpdateMulti(duplicateLines, new List<TaxReportLinePM>(), taxReportPM, true);
                    //  if (removeDupLines.Count > 0) taxReportLineUpdateService.UpdateMulti(removeDupLines, new List<TaxReportLinePM>(), taxReportPM, true);
                    //   }
                }
                else
                {
                    foreach (var linePM in taxReportLines)
                    {
                        if (linePM.StatusCode == TaxReportLineStatusValues.DuplicateThereisanothertransactionwiththesameVATNoandReference)
                        {
                            linePM.StatusCode = "6";
                            //     removeDupLines.Add(linePM);
                        }
                    }
                    //      if (removeDupLines.Count > 0)
                    //      {
                    // accountingContext = AccountingContext.GetContext(taxReportPM.Tenant);
                    // taxReportLineUpdateService = new TaxReportLineUpdateService(accountingContext, new Dictionary<string, IContext>(), taxReportPM.Tenant);
                    // taxReportLineUpdateService.UpdateMulti(removeDupLines, new List<TaxReportLinePM>(), taxReportPM, true);
                    //      }

                }

                taxReportLines.ForEach(l =>
                {
                    l.Line = 0;
                });
            }

        }


        private class DupLines
        {
            public string VatNumber { get; set; }
            public string Reference { get; set; }
            public IEnumerable<int> LineNumbers { get; set; }

        }
        private static string SetTransmitStaus(DateTime invoiceDate, DateTime taxReportMonth, FullAccountingSetting setting)
        {
            DateTime invoiceDateVal = new DateTime(invoiceDate.Year, invoiceDate.Month, 1);
            DateTime taxReportDate = new DateTime(taxReportMonth.Year, taxReportMonth.Month, 1);
            DateTime taxReportDatePreviousMonth = taxReportDate.AddMonths(-1);

            if ((setting.VATreportEveryTwoMonths && invoiceDateVal != taxReportDate && invoiceDateVal != taxReportDatePreviousMonth)
                || (!setting.VATreportEveryTwoMonths && invoiceDateVal != taxReportDate))
            {
                return TaxReportLineTransmitStatusValues.WithoutTransmit;
            }
            return TaxReportLineTransmitStatusValues.Fortransmit;
        }

        private static string SetTransmitStatusForVoidedInvoiceTransaction(List<CustomTaxReportData> ledgerTransactons, CustomTaxReportData transaction, string transmitStatusCode)
        {
            if (!string.IsNullOrEmpty(transaction.OriginalJournalId))
            {
                var orginalJournal = ledgerTransactons.Where(x => x.JournalId == transaction.OriginalJournalId).FirstOrDefault();
                if (orginalJournal == null)
                {

                    LedgerTransactionRepository ledgerTransactionRepository = new LedgerTransactionRepository(transaction.Tenant);
                    var journalAdditionalData = ledgerTransactionRepository.GetJournalAdditionalDataByJournalId(transaction.OriginalJournalId, transaction.Tenant);
                    transmitStatusCode = journalAdditionalData.TaxReportTransmitStatusCode == TaxReportLineTransmitStatusValues.Fortransmit
                        ? TaxReportLineTransmitStatusValues.Fortransmit
                        : TaxReportLineTransmitStatusValues.Notfortransmitatall;
                }
                else
                {
                    transmitStatusCode = orginalJournal.TransmitStatusCode == TaxReportLineTransmitStatusValues.Fortransmit
                        ? TaxReportLineTransmitStatusValues.Fortransmit
                        : TaxReportLineTransmitStatusValues.Notfortransmitatall;
                }
            }
            else if (transaction.IsVoided == true && transaction.TransmitStatusCode != TaxReportLineTransmitStatusValues.Fortransmit)
            {
                transmitStatusCode = TaxReportLineTransmitStatusValues.Notfortransmitatall;
            }
            return transmitStatusCode;
        }

        private static void UpdateTaxReport(TaxReportPM taxReport)
        {
            taxReport.ChangeSetOp = ChangeSetOperation.Update;
            IAccountingContext accountingContext = AccountingContext.GetContext(taxReport.Tenant);
            TaxReportUpdateService updateService = new TaxReportUpdateService(accountingContext, new Dictionary<string, IContext>(), taxReport.Tenant);
            updateService.Update(taxReport, true, TimeSpan.FromMinutes(60));
        }
        private static List<TaxReportLinePM> HandleTaxReportLines(TaxReportPM taxReport, List<TaxReportLinePM> taxReportLines)
        {
            if (taxReportLines.Count > maxAllowedLinesCount)
            {
                return InsertMoreThanMaxAllowedLinesCount(taxReportLines, taxReport);
            }
            else
            {
                return InsertTaxReportLines(taxReport, taxReportLines, 0);
            }
        }
        private static List<TaxReportLinePM> InsertTaxReportLines(TaxReportPM taxReport, List<TaxReportLinePM> taxReportLines, int startIndex)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(60)))
            {
                int count = startIndex;
                foreach (TaxReportLinePM linePM in taxReportLines)
                {
                    ++count;
                    MapTaxReportLinePMFields(linePM, taxReport, count);
                    SaveTaxReportLine(linePM);
                }
                scope.Complete();
                return taxReportLines;

            }
        }
        private static void SaveTaxReportLine(TaxReportLinePM taxReportLine)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(taxReportLine.Tenant);
            TaxReportLineUpdateService lineUpdateService = new TaxReportLineUpdateService(accountingContext, new Dictionary<string, IContext>(), taxReportLine.Tenant);
            lineUpdateService.Update(taxReportLine, true, TimeSpan.FromMinutes(60));//the problem is here it loops on more than 3000  lines and updates them one by one ,each update will have to get single tenant and get single currency along with multible db gets which make the db to time out for the opened transaction
        }
        private static List<TaxReportLinePM> InsertMoreThanMaxAllowedLinesCount(List<TaxReportLinePM> taxReportLines, TaxReportPM taxReport)
        {
            List<TaxReportLinePM> createdLines = new List<TaxReportLinePM>();

            for (int i = 0; i < taxReportLines.Count; i += maxAllowedLinesCount)
            {
                createdLines = createdLines.Concat(InsertTaxReportLinesInMaxAllowedListRange(taxReportLines, i, taxReport)).ToList();
            }
            return createdLines;
        }
        private static List<TaxReportLinePM> InsertTaxReportLinesInMaxAllowedListRange(List<TaxReportLinePM> taxReportLines, int startIndex, TaxReportPM taxReport)
        {
            return InsertTaxReportLines(taxReport, taxReportLines.GetRange(startIndex, Math.Min(maxAllowedLinesCount, taxReportLines.Count - startIndex)), startIndex);
        }
        private static void MapTaxReportLinePMFields(TaxReportLinePM reportLinePM, TaxReportPM taxReport, int count)
        {
            reportLinePM.Line = count;
            reportLinePM.ChangeSetOp = ChangeSetOperation.Insert;
            reportLinePM.UpdatedByUserId = taxReport.UpdatedByUserId;
            bool isTotalInvoiceAmountAndVatAmountHaveOppositeSigns = (reportLinePM.TotalInvoiceAmount > 0 && reportLinePM.VatAmount < 0) || (reportLinePM.TotalInvoiceAmount < 0 && reportLinePM.VatAmount > 0);
            if (isTotalInvoiceAmountAndVatAmountHaveOppositeSigns)
                reportLinePM.StatusCode = StatusCode_VATAmountInTheRecordIsHigherThanThePercentageOfVATAllowed;

        }


        private static bool CheckIfAPInvoiceTaxMonthTransactionIsVoided(TaxReportPM taxReport, List<APInvoicePM> voidedAPInvoices, TaxReportData transaction)
        {
            bool transactionIsAPInvoice = transaction.AccountingEntity == AccountingEntityValues.APInvoice;
            bool transactionIsInTaxMonth = CheckIfTransactionInTaxMonth(taxReport.TaxReportMonth, transaction);
            bool apinvoiceIsVoid = voidedAPInvoices.Where(d => d.Id == transaction.AccountingEntityId).Any();

            return transactionIsAPInvoice && transactionIsInTaxMonth && apinvoiceIsVoid;
        }

        private static bool CheckIfTransactionInTaxMonth(DateTime taxReportMonth, TaxReportData transaction)
        {
            DateTime startOfTaxMonth = new DateTime(taxReportMonth.Year, taxReportMonth.Month, 1, 0, 0, 0);
            DateTime endOfTaxMonth = new DateTime(taxReportMonth.Year, taxReportMonth.Month, DateTime.DaysInMonth(taxReportMonth.Year, taxReportMonth.Month), 23, 59, 59, 59);

            bool transactionInTaxMonth = transaction.ReferenceDate >= startOfTaxMonth
                                            && transaction.ReferenceDate <= endOfTaxMonth;
            return transactionInTaxMonth;
        }

        private static FullAccountingSetting GetTenantFullAccountingSetting(int tenant)
        {
            FullAccountingSettingRepository fullAccountingSettingRepository = new FullAccountingSettingRepository(tenant);
            return fullAccountingSettingRepository.GetSingleFullAccountingSetting(tenant);
        }
        private static Simplog.Data.CommonDataModel.EntityPOCOs.Card GetGLAccountCard(GLAccountPM account)
        {
            Simplog.Data.CommonDataModel.EntityPOCOs.Card card = null;
            if (account != null)
            {
                List<Simplog.Data.CommonDataModel.EntityPOCOs.Card> glAccountCards = cards.Where(d => d.GLAccountId == account.Id).OrderBy(d => d.Code).ToList();
                if (glAccountCards.Count != 0)
                {
                    if (glAccountCards.Count > 1)
                    {
                        foreach (Simplog.Data.CommonDataModel.EntityPOCOs.Card item in glAccountCards)
                        {
                            if (item.VatNumber != null)
                            {
                                card = item;

                                break;
                            }
                        }
                    }
                    else card = glAccountCards.First();
                }
            }
            return card;
        }
        private static string ModifyVatNumber(string vatNumber)
        {
            return (vatNumber != null && vatNumber.Length >= 9) ? vatNumber.Substring(0, 9) : vatNumber;

        }

        private static GLAccountPM GetAccountByLedgerTransaction(TaxReportData transaction)
        {
            GLAccountPM account = null;
            if (transaction.OppositGLAccount != null)
            {
                account = oppositeAccounts.Where(d => d.Id == transaction.OppositGLAccount).FirstOrDefault();
            }
            else
            {
                LedgerTransaction ledgerTransaction = journalsTransactions.Where(d => d.JournalId == transaction.JournalId && d.Reference1 == transaction.Reference && d.LocalAmountCredit != 0 && d.Account.ChartOfAccountsTypeCode != "5").FirstOrDefault();
                account = ledgerTransaction != null ? gLAccounts.Where(d => d.Id == ledgerTransaction.AccountId).FirstOrDefault() : null;
            }
            if (account != null)
            {
                GLAccountCurrencyPM glAccountCurrency = gLAccountCurrencies.Where(d => d.GLAccountId == account.Id).FirstOrDefault();
                if (glAccountCurrency != null)
                    account = parentGLAccounts.Where(d => d.Id == glAccountCurrency.MainGLAccountId).FirstOrDefault();
            }
            return account;
        }
        static void SetVatFieldsForAPInvoiceTransaction(APInvoicePM aPInvoice)
        {
            aPInvoice.TotalVATs = totalvats.Where(d => d.APInvoiceId == aPInvoice.Id).ToList();
            VatNumber = aPInvoice.VATNumber;
            if (aPInvoice.StatusCode == "AC")
            {
                InputVatAmount = (decimal?)aPInvoice.TotalVATs.Sum(d => d.LocalVATAmount) * -1;
                InputInvoiceAmount = (decimal?)aPInvoice.SubTotalInLocalCurrency * -1 ?? 0;
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


        private static void SetVatNumber(Simplog.Data.CommonDataModel.EntityPOCOs.Card card)
        {
            if (card != null)
            {
                GLAccountPM account = oppositeAccounts.Where(d => d.Id == card.GLAccountId && d.AccountTypeCode == "3").FirstOrDefault();
                VatNumber = account != null ? card.VatNumber : null;
            }
        }
        private static void SetVatAmounts(TaxReportData report)
        {

            InputVatAmount = report.LocalAmountDebit;

            //if (card != null && card.PartnerTypeId == PartnerTypeValues.Vendor)
            //    VatNumber = card.VatNumber;

            var transactionSum = journalsTransactions.Where(d => d.JournalId == report.JournalId && d.Reference1 == report.Reference).Sum(d => d.LocalAmountCredit);
            InputInvoiceAmount = transactionSum - InputVatAmount;

        }

        private static string SetTransmitStatusByDocumentDate(DateTime referenceDate, bool vatReportEveryTwoMonths)
        {
            DateTime dateBefore210Days = DateTime.Now.AddDays(-210);
            DateTime dateBefore180Days = DateTime.Now.AddDays(-180);
            DateTime last180days = new DateTime(dateBefore180Days.Year, dateBefore180Days.Month, 1);
            DateTime last210days = new DateTime(dateBefore210Days.Year, dateBefore210Days.Month, 1);
            if ((vatReportEveryTwoMonths && referenceDate <= last210days) || (!vatReportEveryTwoMonths && referenceDate <= last180days))
            {
                return "3";
            }
            else return "1";
        }

        public static BatchTaskExecutionPM CancelTaxReportInBatch(string taxReportId, int tenant)
        {
            BatchTaskExecutionPM taskExe;
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                // create BTE record
                PNCFileArgs args = new PNCFileArgs() { ReportId = taxReportId, Tenant = tenant };
                string xmlParameters = SerializeParameters(args);

                taskExe = CreateBatchTaskExecutionPM(tenant, xmlParameters);

                SubmitBatchTask(tenant, taskExe);

                SendBatchTaskToQueue(tenant, taskExe, scope);
            }

            return taskExe;

        }
        private static string SerializeParameters(PNCFileArgs args)
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(PNCFileArgs));
            serializer.Serialize(stringwriter, args);
            string xmlParameters = stringwriter.ToString();
            return xmlParameters;
        }
        private static BatchTaskExecutionPM CreateBatchTaskExecutionPM(int tenant, string xmlParameters)
        {
            return new BatchTaskExecutionPM()
            {
                Subject = "Cancel Tax Report",
                Tenant = tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ClassName = "Logitude.Accounting.BL.CoreBL.Batch.BatchCancelTaxReportTask,Logitude.Accounting.BL",
                CreateDate = DateTime.Now,
                PrametersXml = xmlParameters,
                StatusCode = CreatedStatusCode,

            };
        }
        private static void SubmitBatchTask(int tenant, BatchTaskExecutionPM taskExe)
        {
            IInfrastructureContext MyContext = InfrastructureContext.GetContext(tenant);
            BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
            bteUpdateService.Update(taskExe, true);
        }
        private static void SendBatchTaskToQueue(int tenant, BatchTaskExecutionPM taskExe, TransactionScope scope)
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
            queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant", tenant.ToString() }
                }, tenant);
            scope.Complete();
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
                }, tenant);
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

                        //TotalInvoiceAmount
                        myStringBuilder.Append(FormatDecimal(lineList.TotalInvoiceAmount, 10, showLocalError: showLocal, includeSign: true, truncateDecimal: true));

                        myStringBuilder.Append("+");
                        myStringBuilder.Append(FormatStringEnd(lineList.ConfirmationNumber, 9, paddingDigit: '0'));

                        myStringBuilder.Append("000000000");

                        myStringBuilder.AppendLine();

                    }


                    //
                    // Line: [last one]
                    //
                    string lastLine = "";
                    lastLine += "X";
                    lastLine += taxReport.VatNumber?.PadLeft(9, '0');
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
                    if (taxReport.StatusCode != "J")
                    {
                        taxReport.StatusCode = "T"; // T- Transmitted

                        DateTime stopLogAt = new DateTime(2023, 06, 01);
                        string text = "TaxReportService.CreatePNC874File(*1*): " + taxReport.Id + " taxReport.StatusCode : " + taxReport.StatusCode;
                        ULog(text, stopLogAt);
                    }
                    taxReport.NeedsRebulid = false;
                    updateService.Update(taxReport, true);


                }
                catch (ApplicationException ex)
                {
                    //update entity
                    TaxReportUpdateService updateService = new TaxReportUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                    taxReport.ChangeSetOp = ChangeSetOperation.Update;
                    taxReport.NeedsRebulid = true;
                    if (taxReport.StatusCode != "J")
                    {
                        taxReport.StatusCode = "T"; // T- Transmitted

                        DateTime stopLogAt = new DateTime(2023, 06, 01);
                        string text = "TaxReportService.CreatePNC874File(*2*): " + taxReport.Id + " taxReport.StatusCode : " + taxReport.StatusCode;
                        ULog(text, stopLogAt);
                    }
                    updateService.Update(taxReport, true);

                    throw ex;
                }

                DocumentsFilingPM docOut = CreateDocumnetFiling(myStringBuilder, taxReport);

                scope.Complete();

                return docOut;
            }
        }
        //public static TaxReportPM CreatetTaxReportLine(TaxReportPM taxReport)
        //{
        //    IAccountingContext context = AccountingContext.GetContext(taxReport.Tenant);

        //    TaxReportLineListQueryService reportLineListQueryService = new TaxReportLineListQueryService(context);
        //    List<TaxReportLineList> lines = reportLineListQueryService.GetReportLines(taxReport.Id, taxReport.Tenant).ToList();
        //    int count = lines.Count;
        //    TaxReportLinePM taxReportLine = new TaxReportLinePM()
        //    {
        //        IsExternalLine = true,
        //        VatNumber = VatNumber,
        //        Reference = reference,
        //        ReferenceDate = DateTime.Today,
        //        ReferecneGroup = referenceGroup,
        //        Line =++count,
        //        OutputOrInput = "O",
        //        VatAmount =(decimal?) 200,// Math.Round(InputVatAmount.Value, MidpointRounding.AwayFromZero),
        //        VatableInvoiceAmount =(decimal?) 17,// Math.Round(InputInvoiceAmount.Value, MidpointRounding.AwayFromZero),
        //        IsEquipment = true,
        //        IsManuallyChanged = true,
        //        TaxReportId = taxReport.Id,
        //        ChangeSetOp = ChangeSetOperation.Insert,
        //        LastUpdateDateTime = DateTime.Now,
        //        UpdatedByUserId = taxReport.UpdatedByUserId,
        //        Tenant = taxReport.Tenant,
        //        TransmitStatusCode = "1",
        //        TaxReportDate = taxReport.TaxReportMonth
        //    };
        //    TaxReportUpdateService updateService = new TaxReportUpdateService(context, new Dictionary<string, IContext>(), taxReport.Tenant);
        //    TaxReportLineUpdateService lineUpdateService = new TaxReportLineUpdateService(context, new Dictionary<string, IContext>(), taxReport.Tenant);
        //    taxReportLine.ChangeSetOp = ChangeSetOperation.Insert;
        //    taxReportLine.UpdatedByUserId = taxReport.UpdatedByUserId;
        //    lineUpdateService.Update(taxReportLine, true, TimeSpan.FromMinutes(60));//the problem is here it loops on more than 3000  lines and updates them one by one ,each update will have to get single tenant and get single currency along with multible db gets which make the db to time out for the opened transaction

        //    taxReport.ChangeSetOp = ChangeSetOperation.Update;
        //    updateService.Update(taxReport, true, TimeSpan.FromMinutes(60));
        //    return taxReport;
        //}


        public static void CalculateReportTotals(TaxReportPM taxReportPM, List<TaxReportLinePM> lines)
        {
            if (lines.Count > 0)
            {
                var outputLines = lines.Where(d => d.OutputOrInput == "O");
                var inputLines = lines.Where(d => d.OutputOrInput == "I");

                // OUTPUT
                taxReportPM.TaxableOutputAmount = outputLines.Where(d => d.TransmitStatusCode == TaxReportLineTransmitStatusValues.Fortransmit).Sum(d => d.VatableInvoiceAmount);
                taxReportPM.OutputTaxAmount = outputLines.Where(d => d.TransmitStatusCode == TaxReportLineTransmitStatusValues.Fortransmit &&
                                                                     d.VatAmount != 0).Sum(d => d.VatAmount);
                taxReportPM.OutputTaxAmountRound = outputLines.Where(d => d.TransmitStatusCode == TaxReportLineTransmitStatusValues.Fortransmit &&
                                                                     d.VatAmountRound != 0).Sum(d => d.VatAmountRound);
                taxReportPM.ExemptTaxableOutput = outputLines.Where(d => d.TransmitStatusCode == TaxReportLineTransmitStatusValues.Fortransmit).Sum(d => d.TotalInvoiceAmount - d.VatableInvoiceAmount);
                taxReportPM.OutputLinesCount = outputLines.Where(d => d.TransmitStatusCode == TaxReportLineTransmitStatusValues.Fortransmit).Count();

                // INPUTS
                taxReportPM.InputsTaxAmountRound = inputLines.Where(d => d.TransmitStatusCode == TaxReportLineTransmitStatusValues.Fortransmit
                                                                         ).Sum(d => d.VatAmountRound);
                taxReportPM.OtherInputsTaxAmount = inputLines.Where(d => d.TransmitStatusCode == TaxReportLineTransmitStatusValues.Fortransmit &&
                                                                         d.IsEquipment == false).Sum(d => d.VatAmount);
                taxReportPM.EquipmentInputsTaxAmount = inputLines.Where(d => d.TransmitStatusCode == TaxReportLineTransmitStatusValues.Fortransmit &&
                                                                             d.IsEquipment == true).Sum(d => d.VatAmount);
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


        private static string FormatStringEnd(string str, int wordSize, char paddingDigit = ' ')
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
                str = str.Substring(str.Length - wordSize);
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
                }, tenant);
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
    public struct TaxReportLineType
    {
        public const string Input = "I";
        public const string Output = "O";


    }
}
