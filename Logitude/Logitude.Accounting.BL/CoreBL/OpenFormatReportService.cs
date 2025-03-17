using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Enums;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.Resolvers;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL
{
    public class OpenFormatReportService
    {
        private int rowsCount;
        private string random;
        private int B100Count;
        private int B110Count;
        private int C100Count;
        private int D110Count;
        private int D120Count;
        bool isQuantity;
        private string a;
        public int ARinvoiceTotalRecords;
        public decimal ARinvoiceTotalAmount;
        public int CreditARinvoiceTotalRecords;
        public decimal CreditARinvoiceTotalAmount;
        public int ARpaymentTotalRecords;
        public decimal ARpaymentTotalAmount;
        public int DepositTotalRecords;
        public decimal DepositTotalAmount;
        public int APinvoiceTotalRecords;
        public decimal APinvoiceTotalAmount;
        public TenantPM tenantPM;
        private CardQuery cardQuery;
        private int Tenant;
        static string ProcessVatNumber(string input)
        {
            string numericPart = Regex.Match(input, @"^\d+").Value;
            // Pad with zeros from the left until the length of 9
            string paddedNumeric = numericPart.PadLeft(9, '0');
            return paddedNumeric;
        }

        public void AddSBline(ref StringBuilder sb, object[] variables)
        {
            // Generate the format string dynamically without any delimiters
            StringBuilder formatBuilder = new StringBuilder();
            for (int i = 0; i < variables.Length; i++)
            {
                formatBuilder.Append($"{{{i}}}");  // Add placeholder without delimiter
            }
            string formatString = formatBuilder.ToString();
            // Append formatted string to StringBuilder
            sb.AppendFormat(CultureInfo.InvariantCulture, formatString, variables);
            sb.AppendLine();
        }
        public DocumentsFilingPM CreateBKMVDATAFile(string openFormatReportId, int tenant, bool TestingMode)
        {
            Tenant = tenant;
            B100Count = 0;
            B110Count = 0;
            C100Count = 0;
            D110Count = 0;
            D120Count = 0;
            rowsCount = 0;
            isQuantity = false;
            a = null;
            ARinvoiceTotalRecords = 0;
            ARinvoiceTotalAmount = 0;
            CreditARinvoiceTotalRecords = 0;
            CreditARinvoiceTotalAmount = 0;
            ARpaymentTotalRecords = 0;
            ARpaymentTotalAmount = 0;
            DepositTotalRecords = 0;
            DepositTotalAmount = 0;
            APinvoiceTotalRecords = 0;
            APinvoiceTotalAmount = 0;
            tenantPM = null;

            OpenFormatReportQueryService openFormatReportQueryService = new OpenFormatReportQueryService(tenant);
            OpenFormatReportPM openFormatReportPM = openFormatReportQueryService.GetSingle(openFormatReportId, false, false);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            tenantPM = tenantQuery.GetSinglePM(tenant);
            LedgerTransactionQueryService ledgerTransactionQueryService = new LedgerTransactionQueryService(tenant);
            List<B100Data> b100Data = ledgerTransactionQueryService.GetTransactionsByDate(openFormatReportPM.FromDate, openFormatReportPM.ToDate, tenant);
            List<string> userIds = b100Data.Select(d => d.CreatedByUser).ToList();
            UserQuery userQuery = new UserQuery(tenant);
            CurrencyQuery currencyQuery = new CurrencyQuery(tenant);
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
            cardQuery = new CardQuery(tenant);
            List<B110Data> b110Data = gLAccountQueryService.GetB110sForGLAccounts(tenant);


            List<UserPM> users = userQuery.GetUserPMsByUserIds(userIds, tenant).ToList();
            List<CurrencyPM> currencies = currencyQuery.GetCurrenciesByTenantPM(tenant).ToList(); ;

            ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(tenant);
            List<GLAccountCurrencyPM> gLAccountCurrencies = new List<GLAccountCurrencyPM>();
            GLAccountCurrencyQueryService gLAccountCurrencyQueryService = new GLAccountCurrencyQueryService(tenant);
            gLAccountCurrencies = gLAccountCurrencyQueryService.GetTenantCurrenciesAccount(tenant);

            ComputingPartnerTranslationHelper computingPartnerTranslationHelper = new ComputingPartnerTranslationHelper(tenant);
            List<ComputingPartnerTranslationPM> computingPartnerTranslations = new List<ComputingPartnerTranslationPM>();
            computingPartnerTranslations = computingPartnerTranslationHelper.GetComputingPartnerCodeTranslations("Cust", tenant);

            if (TestingMode)
            {
                a = "a";
            }


            List<string> linesArray = new List<string>();



            StringBuilder myStringBuilder = new StringBuilder();

            // A100
            myStringBuilder.Append("A100");
            myStringBuilder.Append(a);
            myStringBuilder.Append("000000001");

            if (tenantPM.VatNumber != null)
            {
                if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                myStringBuilder.Append(a + tenantPM.VatNumber.PadLeft(9, '0'));
            }
            else
            {
                myStringBuilder.Append('0', 9);
            }

            Random Random = new Random();
            string num = Random.Next().ToString();
            random = num;
            if (num.Length > 15) { num = num.Substring(0, 15); }

            myStringBuilder.Append(a + num.PadLeft(15, '0'));

            myStringBuilder.Append(a);
            myStringBuilder.Append("&OF1.31&");
            myStringBuilder.Append(' ', 50);
            myStringBuilder.AppendLine();
            //B100
            int counter = 1;
            string Code = "B100",
                Counter = "".PadLeft(9, '0'),
                VatNumber = "".PadLeft(9, '0'),
                LedgerTransactionId = "".PadLeft(10, '0'),
                JournalLineNumber = "".PadLeft(5, '0'),
                JournalNumber = "".PadLeft(8, '0'),
                Type = "".PadLeft(15, ' '),
                Reference1 = "".PadLeft(20, ' '),
                Reference1Type = "".PadLeft(3, '0'),
                Reference2 = "".PadLeft(20, ' '),
                Reference2Type = "".PadLeft(3, '0'),
                Notes = "".PadLeft(50, ' '),
                AccountingDate = "".PadLeft(8, '0'),
                DocumentDate = "".PadLeft(8, '0'),
                Account = "".PadLeft(15, ' '),
                OppositeAccount = "".PadLeft(15, ' '),
                Sign = "1",
                CurrencyCode = "".PadLeft(3, ' '),
                Sum = "+".PadRight(14, '0'),
                ForeignSum = "+".PadRight(14, '0'),
                Amount = "".PadLeft(12, ' '),
                Fit1 = "".PadLeft(10, ' '),
                Fit2 = "".PadLeft(10, ' '),
                Branch = "".PadLeft(7, ' '),
                CreationDate = "".PadLeft(8, '0'),
                User = "".PadLeft(9, ' '),
                FutureDetails = "".PadLeft(25, ' ');
            foreach (B100Data item in b100Data)
            {
                counter++;
                B100Count++;
                if (counter.ToString().Length > 9)
                {
                    Counter = counter.ToString().Substring(0, 9).PadLeft(9, '0');
                }
                else
                {
                    Counter = counter.ToString().PadLeft(9, '0');
                }

                if (tenantPM.VatNumber != null)
                {
                    if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                    VatNumber = tenantPM.VatNumber.PadLeft(9, '0');
                }

                if (item.LedgerTransactionId != null)
                {
                    string[] ledgerId = item.LedgerTransactionId.ToString().Split('-');
                    if (ledgerId[1].Length > 10) { ledgerId[1] = ledgerId[1].Substring(0, 10); }
                    LedgerTransactionId = ledgerId[1].PadLeft(10, '0');
                }

                JournalLineNumber = item.JournalLineNumber.ToString();
                if (JournalLineNumber.Length > 5) { JournalLineNumber = JournalLineNumber.Substring(0, 5); }
                JournalLineNumber = JournalLineNumber.PadLeft(5, '0');

                if (item.JournalNumber != null)
                {
                    if (item.JournalNumber.Length > 8) { item.JournalNumber = item.JournalNumber.Substring(0, 8); }
                    JournalNumber = item.JournalNumber.PadLeft(8, '0');
                }


                if (item.AccountingEntityReference != null)
                {
                    if (item.AccountingEntityReference.Length > 20) { item.AccountingEntityReference = item.AccountingEntityReference.Substring(0, 20); }
                    Reference1 = item.AccountingEntityReference.PadLeft(20, ' ');
                }


                Reference1Type = item.AccountingEntityReferenceType.PadLeft(3, '0');


                if (item.Reference2 != null)
                {
                    if (item.Reference2.Length > 20) { item.Reference2 = item.Reference2.Substring(0, 20); }
                    Reference2 = item.Reference2.PadLeft(20, ' ');
                }

                if (item.Notes != null)
                {
                    if (item.Notes.Length > 50) { item.Notes = item.Notes.Substring(0, 50); }
                    Notes = item.Notes.PadLeft(50, ' ');
                }

                AccountingDate = String.Format("{0:yyyyMMdd}", item.AccountingDate);

                if (AccountingDate.Length > 8) { AccountingDate = AccountingDate.Substring(0, 8); }
                AccountingDate = AccountingDate.PadLeft(8, '0');

                DocumentDate = String.Format("{0:yyyyMMdd}", item.DocumentDate);

                if (DocumentDate.Length > 8) { DocumentDate = DocumentDate.Substring(0, 8); }
                DocumentDate = DocumentDate.PadLeft(8, '0');

                if (item.GLAccountDisplayNumber != null)
                {
                    if (item.GLAccountDisplayNumber.Length > 15) { item.GLAccountDisplayNumber = item.GLAccountDisplayNumber.Substring(0, 15); }
                    Account = item.GLAccountDisplayNumber.PadLeft(15, ' ');
                }

                if (item.OppositGLAccount != null)
                {
                    if (item.OppositGLAccount.Length > 15) { item.OppositGLAccount = item.OppositGLAccount.Substring(0, 15); }
                    OppositeAccount = item.OppositGLAccount.PadLeft(15, ' ');
                }

                if (item.LocalAmountDebit == 0)
                {
                    Sign = "2";

                }
                else
                {
                    Sign = "1";
                }


                CurrencyPM currency = currencies.Where(d => d.Id == item.CurrencyId).FirstOrDefault();

                if (currency != null)
                {
                    if (computingPartnerTranslations != null)
                    {

                        var partnerCode = computingPartnerTranslations.Where(d => d.ObjectTableName == "Currency" && d.OurCode == currency.Code).FirstOrDefault();

                        if (partnerCode != null && !string.IsNullOrEmpty(partnerCode.PartnerCode))
                        {

                            if (partnerCode.PartnerCode.Length > 3) { partnerCode.PartnerCode = partnerCode.PartnerCode.Substring(0, 3); }
                            CurrencyCode = partnerCode.PartnerCode.PadLeft(3, ' ');
                        }


                        else
                        {
                            CurrencyCode = currency.Code.PadLeft(3, ' ');
                        }
                    }

                    else
                    {
                        CurrencyCode = currency.Code.PadLeft(3, ' ');
                    }
                    if (CurrencyCode == "NIS")
                    {
                        CurrencyCode = "ILS";
                    }
                }

                //Sum
                if (item.LocalAmountDebit != 0)
                {
                    if (item.LocalAmountDebit >= 0)
                    {
                        string LocalAmountDebit = Format(item.LocalAmountDebit);  
                        if (LocalAmountDebit.Length > 14) { LocalAmountDebit = LocalAmountDebit.Substring(0, 14); }
                        Sum = "+" + LocalAmountDebit.PadLeft(14, '0');
                    }
                    else
                    {
                        var LocalAmountDebit = Format(item.LocalAmountDebit); 
                        if (LocalAmountDebit.Length > 14) { LocalAmountDebit = LocalAmountDebit.Substring(0, 14); }
                        Sum = "-" + LocalAmountDebit.PadLeft(14, '0');
                    }
                }

                else
                {
                    if (item.LocalAmountCredit >= 0)
                    {
                        var LocalAmountCredit = Format(item.LocalAmountCredit);  
                        if (LocalAmountCredit.Length > 14) { LocalAmountCredit = LocalAmountCredit.Substring(0, 14); }
                        Sum = "+" + LocalAmountCredit.PadLeft(14, '0');
                    }
                    else if (item.LocalAmountCredit < 0)
                    {
                        var LocalAmountCredit = Format(item.LocalAmountCredit); 
                        if (LocalAmountCredit.Length > 14) { LocalAmountCredit = LocalAmountCredit.Substring(0, 14); }
                        Sum = "-" + LocalAmountCredit.PadLeft(14, '0');
                    }
                }


                //ForeignSum
                if (item.LocalAmountDebit != 0)
                {
                    if (item.ForeignAmountDebit >= 0)
                    {
                        string ForeignAmountDebit = Format(item.ForeignAmountDebit); 
                        if (ForeignAmountDebit.Length > 14) { ForeignAmountDebit = ForeignAmountDebit.Substring(0, 14); }
                        ForeignSum = "+" + ForeignAmountDebit.PadLeft(14, '0');
                    }
                    else if (item.ForeignAmountDebit < 0)
                    {
                        string ForeignAmountDebit = Format(item.ForeignAmountDebit); 
                        if (ForeignAmountDebit.Length > 14) { ForeignAmountDebit = ForeignAmountDebit.Substring(0, 14); }
                        ForeignSum = "-" + ForeignAmountDebit.PadLeft(14, '0');
                    }
                }
                else
                {
                    if (item.ForeignAmountCredit >= 0)
                    {
                        string ForeignAmountCredit = Format(item.ForeignAmountCredit);  
                        if (ForeignAmountCredit.Length > 14) { ForeignAmountCredit = ForeignAmountCredit.Substring(0, 14); }
                        ForeignSum = "+" + ForeignAmountCredit.PadLeft(14, '0');
                    }
                    else if (item.ForeignAmountCredit < 0)
                    {

                        string ForeignAmountCredit = Format(item.ForeignAmountCredit);     
                        if (ForeignAmountCredit.Length > 14) { ForeignAmountCredit = ForeignAmountCredit.Substring(0, 14); }
                        ForeignSum = "-" + ForeignAmountCredit.PadLeft(14, '0');
                    }
                }

                CreationDate = String.Format("{0:yyyyMMdd}", item.CreateDate);
                if (CreationDate.Length > 8) { CreationDate = CreationDate.Substring(0, 8); }
                CreationDate = CreationDate.PadLeft(8, '0');

                UserPM user = users.Where(d => d.Id == item.CreatedByUser).FirstOrDefault();
                if (user != null)
                {
                    if (user.Code != null)
                    {
                        if (user.Code.Length > 9) { user.Code = user.Code.Substring(0, 9); }
                        User = user.Code.PadLeft(9, ' ');
                    }
                    else if (user.EnglishName != null)
                    {
                        if (user.EnglishName.Length > 9) { user.EnglishName = user.EnglishName.Substring(0, 9); }
                        User = user.EnglishName.PadLeft(9, ' ');
                    }
                }
                object[] variables = new object[]{
                    Code, Counter, VatNumber, LedgerTransactionId, JournalLineNumber, JournalNumber, Type, Reference1, Reference1Type, Reference2, Reference2Type, Notes,
                    AccountingDate, DocumentDate, Account, OppositeAccount, Sign, CurrencyCode, Sum, ForeignSum, Amount, Fit1, Fit2,
                    Branch, CreationDate, User, FutureDetails};
                AddSBline(ref myStringBuilder, variables);
            }



            //B110


            AddressQuery addressQuery = new AddressQuery(tenant);
            cardQuery = new CardQuery(tenant);

            var trailReportParam = new TrailReportParam()
            {
                Tenant = tenant,
                ToDate = (DateTime)openFormatReportPM.ToDate,
                FromDate = (DateTime)openFormatReportPM.FromDate,
                CurrenciesDetailed = true,
                DetailedControlVendors = true,
                DetailedControlClients = true,
                DetailedControlJob = true,
                DetailedControlFile = true,
                Category1 = null,
                Category2 = null,
                Category3 = null,
                Category4 = null,
                Category5 = null,
                Suppress_DoNotShowCardWithoutActivity = false,
                IsRevenueExpenseReport = false,
                MyTrailReportLevel = ReportLevel.GLAccount,
                IsTrialBalanceReport = false,

            };


            var typeservice = TrailReportFactory.CreateNew(trailReportParam);
            List<TrailReportM> res1;
            try
            {
                res1 = typeservice.Execute();
            }
            finally
            {
                
            }


            List<string> includedGLAccounts = GetIncludedGLAccounts(res1, openFormatReportPM, tenant);

            IEnumerable<IGrouping<string, TrailReportM>> res = res1.GroupBy(d => d.GLAccountId);


            var result = res.Where(d => d.Key != null).ToDictionary(x => x.Key, x => x);
            HashSet<string> includedGLAccountsHash = new HashSet<string>(includedGLAccounts);    
            b110Data = b110Data.Where(d => includedGLAccountsHash.Contains(d.GLAccountId)).ToList();

            List<CardList> cardLists = GetCardsByGLAccountIds(includedGLAccounts.Where(d => d != null).ToList(), tenant);
            List<AddressList> addreses = addressQuery.GetAddressesByCardIds(cardLists.Select(d => d.Id).ToList(), tenant);
            foreach (B110Data item in b110Data)
            {
                counter++;
                B110Count++;

                CardList card = cardLists.Where(d => d.GLAccountId == item.GLAccountId).FirstOrDefault();
                myStringBuilder.Append("B110");

                if (counter.ToString().Length > 9)
                {
                    counter.ToString().Substring(0, 9);
                    myStringBuilder.Append(a + counter.ToString().PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append(a);

                    myStringBuilder.Append(counter.ToString().PadLeft(9, '0'));
                }
                if (tenantPM.VatNumber != null)
                {
                    if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                    myStringBuilder.Append(a + tenantPM.VatNumber.PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append(a);
                    myStringBuilder.Append('0', 9);
                }
                if (item.DisplayNumber != null)
                {
                    if (item.DisplayNumber.Length > 15) { item.DisplayNumber = item.DisplayNumber.Substring(0, 15); }
                    myStringBuilder.Append(item.DisplayNumber.PadLeft(15, ' '));
                }
                else
                {
                    myStringBuilder.Append(a);
                    myStringBuilder.Append(' ', 15);
                }
                if (item.LocalName != null)
                {
                    if (item.LocalName.Length > 50) { item.LocalName = item.LocalName.Substring(0, 50); }
                    myStringBuilder.Append(a + item.LocalName.PadLeft(50, ' '));
                }
                else
                {
                    if (item.EnglishName != null)
                    {
                        if (item.EnglishName.Length > 50) { item.EnglishName = item.EnglishName.Substring(0, 50); }
                        myStringBuilder.Append(a + item.EnglishName.PadLeft(50, ' '));
                    }
                }
                if (item.ChartOfAccountsCode != null)
                {
                    if (item.ChartOfAccountsCode.Length > 15) { item.ChartOfAccountsCode = item.ChartOfAccountsCode.Substring(0, 15); }
                    myStringBuilder.Append(a + item.ChartOfAccountsCode.PadLeft(15, ' '));
                }
                else
                {
                    myStringBuilder.Append(a);
                    myStringBuilder.Append(' ', 15);
                }
                if (item.ChartOfAccountsName != null)
                {
                    if (item.ChartOfAccountsName.Length > 30) { item.ChartOfAccountsName = item.ChartOfAccountsName.Substring(0, 30); }
                    myStringBuilder.Append(a + item.ChartOfAccountsName.PadLeft(30, ' '));
                }
                else
                {
                    myStringBuilder.Append(a);
                    myStringBuilder.Append(' ', 30);
                }
                if ((item.AccountTypeCode == "2" || item.AccountTypeCode == "3"))
                {

                    var accountCurrency = gLAccountCurrencies.Where(d => d.GLAccountId == item.GLAccountId).FirstOrDefault();
                    if (accountCurrency != null)
                    {
                        card = cardLists.Where(d => d.GLAccountId == accountCurrency.MainGLAccountId).FirstOrDefault();
                    }


                    if (card != null)
                    {
                        string address = null;
                        var billingaddress = addreses.Where(d => d.CardId == card.Id && d.AddressTypeId == "B").FirstOrDefault();
                        var mainaddress = addreses.Where(d => d.CardId == card.Id && d.AddressTypeId == "M").FirstOrDefault();
                        if (billingaddress == null)
                        {
                            if (mainaddress == null) myStringBuilder.Append(' ', 130);
                            else billingaddress = mainaddress;
                        }
                        if (billingaddress != null)
                        {
                            address = billingaddress.Address1;
                            item.Address1 = address;
                            if (item.Address1 != null)
                            {
                                if (item.Address1.Length > 50) { item.Address1 = item.Address1.Substring(0, 50); }
                                myStringBuilder.Append(item.Address1.PadLeft(50, ' '));
                                if (address.Length > 60)
                                {

                                    item.Address2 = address.Substring(51, 10);
                                    myStringBuilder.Append(a + item.Address2.PadLeft(10, ' '));
                                }
                                else if (address.Length > 51)
                                {
                                    int s = address.Length - 50;
                                    item.Address2 = address.Substring(50, s);
                                    myStringBuilder.Append(a + item.Address2.PadLeft(10, ' '));
                                }
                                else
                                {
                                    myStringBuilder.Append(a);
                                    myStringBuilder.Append(' ', 10);
                                }
                            }
                            else
                            {
                                if (address == null)
                                {
                                    if (mainaddress != null)
                                    {
                                        address = mainaddress.Address1;
                                        item.Address1 = address;
                                        if (item.Address1 != null)
                                        {


                                            if (item.Address1.Length > 50)
                                            { item.Address1 = item.Address1.Substring(0, 50); }
                                            myStringBuilder.Append(item.Address1.PadLeft(50, ' '));

                                            if (address.Length > 60)
                                            {
                                                item.Address2 = address.Substring(51, 10);
                                                myStringBuilder.Append(a + item.Address2.PadLeft(10, ' '));
                                            }
                                            else if (address.Length > 51)
                                            {
                                                int s = address.Length - 50;
                                                item.Address2 = address.Substring(50, s);
                                                myStringBuilder.Append(a + item.Address2.PadLeft(10, ' '));
                                            }
                                            else
                                            {
                                                myStringBuilder.Append(a);
                                                myStringBuilder.Append(' ', 10);
                                            }


                                        }
                                        else
                                        {
                                            myStringBuilder.Append(a);
                                            myStringBuilder.Append(' ', 50);
                                            myStringBuilder.Append(a);
                                            myStringBuilder.Append(' ', 10);
                                        }
                                    }
                                }
                            }
                            if (billingaddress.City != null)
                            {
                                if (billingaddress.City.Length > 30) { billingaddress.City = billingaddress.City.Substring(0, 30); }
                                myStringBuilder.Append(a + billingaddress.City.PadLeft(30, ' '));
                            }
                            else
                            {
                                if (mainaddress?.City != null)
                                {
                                    if (mainaddress.City.Length > 30) { mainaddress.City = mainaddress.City.Substring(0, 30); }
                                    myStringBuilder.Append(a + mainaddress.City.PadLeft(30, ' '));
                                }
                                else
                                {
                                    myStringBuilder.Append(a);
                                    myStringBuilder.Append(' ', 30);
                                }
                            }
                            if (billingaddress.ZipCode != null)
                            {
                                if (billingaddress.ZipCode.Length > 8) { billingaddress.ZipCode = billingaddress.ZipCode.Substring(0, 8); }
                                myStringBuilder.Append(a + billingaddress.ZipCode.PadLeft(8, ' '));
                            }
                            else
                            {
                                if (mainaddress?.ZipCode != null)
                                {
                                    if (mainaddress.ZipCode.Length > 8) { mainaddress.ZipCode = mainaddress.ZipCode.Substring(0, 8); }
                                    myStringBuilder.Append(a + mainaddress.ZipCode.PadLeft(8, ' '));
                                }
                                else
                                {
                                    myStringBuilder.Append(a);
                                    myStringBuilder.Append(' ', 8);
                                }
                            }
                            if (billingaddress.CountryName != null)
                            {
                                if (billingaddress.CountryName.Length > 30) { billingaddress.CountryName = billingaddress.CountryName.Substring(0, 30); }
                                myStringBuilder.Append(a + billingaddress.CountryName.PadLeft(30, ' '));
                            }
                            else
                            {
                                if (mainaddress?.CountryName != null)
                                {
                                    if (mainaddress.CountryName.Length > 30) { mainaddress.CountryName = mainaddress.CountryName.Substring(0, 30); }
                                    myStringBuilder.Append(a + mainaddress.CountryName.PadLeft(30, ' '));
                                }
                                else
                                {
                                    myStringBuilder.Append(a);
                                    myStringBuilder.Append(' ', 30);
                                }
                            }
                            if (computingPartnerTranslations != null && billingaddress.CountryCode != null)
                            {

                                var partnerCode = computingPartnerTranslations.Where(d => d.ObjectTableName == "Country" && d.OurCode == billingaddress.CountryCode).FirstOrDefault();

                                if (partnerCode != null)
                                {
                                    if (partnerCode.PartnerCode.Length > 2) { partnerCode.PartnerCode = partnerCode.PartnerCode.Substring(0, 2); }
                                    myStringBuilder.Append(a + partnerCode.PartnerCode.PadLeft(2, ' '));
                                }

                                else
                                {
                                    myStringBuilder.Append(a);
                                    myStringBuilder.Append(' ', 2);
                                }
                            }
                            else
                            {
                                if (mainaddress?.CountryCode != null)
                                {
                                    if (computingPartnerTranslations != null)
                                    {
                                        var partnerCode = computingPartnerTranslations.Where(d => d.ObjectTableName == "Country" && d.OurCode == mainaddress.CountryCode).FirstOrDefault();

                                        if (partnerCode != null)
                                        {
                                            if (partnerCode.PartnerCode.Length > 2) { partnerCode.PartnerCode = partnerCode.PartnerCode.Substring(0, 2); }
                                            myStringBuilder.Append(a + partnerCode.PartnerCode.PadLeft(2, ' '));
                                        }
                                        else
                                        {
                                            myStringBuilder.Append(a);
                                            myStringBuilder.Append(' ', 2);
                                        }
                                    }
                                    else
                                    {
                                        myStringBuilder.Append(a);
                                        myStringBuilder.Append(' ', 2);
                                    }
                                }
                                else
                                {
                                    myStringBuilder.Append(a);
                                    myStringBuilder.Append(' ', 2);
                                }
                            }
                        }

                    }
                    else
                    {
                        myStringBuilder.Append(a);
                        myStringBuilder.Append(' ', 130);
                    }
                }
                else
                {
                    myStringBuilder.Append(a);
                    myStringBuilder.Append(' ', 130);
                }
                myStringBuilder.Append(a);
                myStringBuilder.Append(' ', 15);
                IGrouping<string, TrailReportM> trailReportM = null;
                if (result.ContainsKey(item.GLAccountId))
                {
                    trailReportM = result[item.GLAccountId];
                }
                if (trailReportM != null)
                {
                    item.OpeningBalance = trailReportM.Select(d => d.LocalOpenBalance).Sum();
                    item.TotalDebit = trailReportM.Select(d => d.LocalDebit).Sum();
                    item.TotalCredit = trailReportM.Select(d => d.LocalCredit).Sum();
                    if (item.OpeningBalance != null)
                    {
                        string OpeningBalance = Format(item.OpeningBalance.Value);
                        if (item.OpeningBalance < 0)
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append("-");

                            if (OpeningBalance.Length > 14) { OpeningBalance = OpeningBalance.Substring(0, 14); }
                            myStringBuilder.Append(OpeningBalance.PadLeft(14, '0'));
                        }

                        else if (item.OpeningBalance > 0)
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append("+");

                            if (OpeningBalance.Length > 14) { OpeningBalance = OpeningBalance.Substring(0, 14); }
                            myStringBuilder.Append(OpeningBalance.PadLeft(14, '0'));
                        }
                        else
                        {

                            myStringBuilder.Append(a);
                            myStringBuilder.Append("+");

                            myStringBuilder.Append('0', 14);
                        }

                    }
                    else
                    {
                        myStringBuilder.Append(a);

                        myStringBuilder.Append(' ', 15);
                    }
                    if (item.TotalDebit != null)
                    {
                        string totalDebit = Format(item.TotalDebit.Value);  
                        if (item.TotalDebit < 0)
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append("-");
                            if (totalDebit.Length > 14) { totalDebit = totalDebit.Substring(0, 14); }
                            myStringBuilder.Append(totalDebit.PadLeft(14, '0'));
                        }

                        else if (item.TotalDebit > 0)
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append("+");
                            if (totalDebit.Length > 14) { totalDebit = totalDebit.Substring(0, 14); }
                            myStringBuilder.Append(totalDebit.PadLeft(14, '0'));
                        }
                        else
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append("+");
                            myStringBuilder.Append('0', 14);
                        }

                    }
                    else
                    {
                        myStringBuilder.Append(a);
                        myStringBuilder.Append(' ', 15);
                    }
                    if (item.TotalCredit != null)
                    {
                        string totalCredit = Format(item.TotalCredit.Value); 

                        if (item.TotalCredit < 0)
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append("-");
                            if (totalCredit.Length > 14) { totalCredit = totalCredit.Substring(0, 14); }
                            myStringBuilder.Append(totalCredit.PadLeft(14, '0'));
                        }
                        else if (item.TotalCredit > 0)
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append("+");
                            if (totalCredit.Length > 14) { totalCredit = totalCredit.Substring(0, 14); }
                            myStringBuilder.Append(totalCredit.PadLeft(14, '0'));
                        }
                        else
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append("+");
                            myStringBuilder.Append('0', 14);
                        }
                    }
                    else
                    {
                        myStringBuilder.Append(a);
                        myStringBuilder.Append(' ', 15);
                    }
                }
                else
                {
                    myStringBuilder.Append(a);
                    myStringBuilder.Append(' ', 45);

                }
                myStringBuilder.Append(a);
                myStringBuilder.Append("0000");
                if (card != null && card.VatNumber != null)
                {
                    if (card.VatNumber.Length > 9) { card.VatNumber = card.VatNumber.Substring(0, 9); }
                    myStringBuilder.Append(a + card.VatNumber.PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append(a);
                    myStringBuilder.Append('0', 9);
                }
                myStringBuilder.Append(a);
                myStringBuilder.Append(' ', 7);

                if (item.IsMultiCurrency == false && item.CurrecnyId != tenantPM.CurrencyId)
                {

                    item.OpeningBalanceInForegnCurrency = trailReportM != null ? trailReportM.Select(d => d.ForeignOpenBalance).Sum() : null;
                    if (item.OpeningBalanceInForegnCurrency != null)
                    {
                        string OpeningBalanceInForegnCurrency = Format(item.OpeningBalanceInForegnCurrency.Value);
                        if (item.OpeningBalanceInForegnCurrency < 0)
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append("-");
                            if (OpeningBalanceInForegnCurrency.Length > 14) { OpeningBalanceInForegnCurrency = OpeningBalanceInForegnCurrency.Substring(0, 14); }
                            myStringBuilder.Append(OpeningBalanceInForegnCurrency.PadLeft(14, '0'));
                        }
                        else if (item.OpeningBalanceInForegnCurrency > 0)
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append("+");
                            if (OpeningBalanceInForegnCurrency.Length > 14) { OpeningBalanceInForegnCurrency = OpeningBalanceInForegnCurrency.Substring(0, 14); }
                            myStringBuilder.Append(OpeningBalanceInForegnCurrency.PadLeft(14, '0'));
                        }

                        else
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append("+");
                            myStringBuilder.Append('0', 14);
                        }

                    }
                    else
                    {
                        myStringBuilder.Append(a);
                        myStringBuilder.Append(' ', 15);
                    }


                    if (item.CurrencyCode != null)
                    {
                        if (item.CurrencyCode.Length > 3) { item.CurrencyCode = item.CurrencyCode.Substring(0, 3); }
                        if (item.CurrencyCode == "NIS")
                        {
                            item.CurrencyCode = "ILS";
                        }
                        myStringBuilder.Append(a + item.CurrencyCode.PadLeft(3, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append(a);
                        myStringBuilder.Append(' ', 3);
                    }
                }
                else
                {
                    myStringBuilder.Append(a);
                    myStringBuilder.Append(' ', 15);
                    myStringBuilder.Append(a);
                    myStringBuilder.Append(' ', 3);
                }
                myStringBuilder.Append(a);
                myStringBuilder.Append(' ', 16);
                myStringBuilder.AppendLine();
            }

            //C100

            List<C100Data> C100 = new List<C100Data>();
            List<C100Data> ARC100 = GetARInvoiceC100Data(openFormatReportPM, tenant);
            List<C100Data> APC100 = GetAPInvoiceC100Data(openFormatReportPM, tenant);
            List<C100Data> ARPAymentC100 = GetARPaymentC100Data(openFormatReportPM, tenant);
            List<C100Data> DepositC100 = GetDepositC100Data(openFormatReportPM, tenant);

            ARInvoiceTotalVATQuery aRInvoiceTotalVATQuery = new ARInvoiceTotalVATQuery(tenant);
            BankAccountQueryService bankAccountQueryService = new BankAccountQueryService(tenant);
            List<string> ARInvoiceIDs = ARC100.Select(d => d.ARInvoiceId).ToList();
            List<ARInvoiceTotalVATPM> totalVats = aRInvoiceTotalVATQuery.GetTotalVATs(ARInvoiceIDs, tenant);
            List<string> depositIds = DepositC100.Select(d => d.DepositId).ToList();
            List<string> arpaymentIds = ARPAymentC100.Select(d => d.ARPaymentId).ToList();



            C100 = ARC100.Concat(APC100).Concat(ARPAymentC100).ToList();
            List<string> vandorIDs = C100.Select(d => d.VendorId).ToList();

            addreses = addressQuery.GetAddressesByCardIds(vandorIDs, tenant);
            APInvoiceTotalVATQuery aPInvoiceTotalVATQuery = new APInvoiceTotalVATQuery(tenant);
            List<string> APInvoiceIDs = APC100.Select(d => d.APInvoiceId).ToList();
            List<APInvoiceTotalVATPM> APtotalVats = aPInvoiceTotalVATQuery.GetTotalVATs(APInvoiceIDs, tenant);

            List<string> GLAccountIds = C100.Select(d => d.GLAccountId).ToList();
            List<GLAccountPM> acccounts = gLAccountQueryService.GetGLAccountsByIds(GLAccountIds, tenant);

            ARInvoiceLineQuery aRInvoiceLineQuery = new ARInvoiceLineQuery(tenant);
            List<ARInvoiceLinePM> aRInvoiceLinePMs = aRInvoiceLineQuery.GetInvoiceLinePMsByInvoiceIds(ARInvoiceIDs, tenant);
            APInvoiceLineQuery aPInvoiceLineQuery = new APInvoiceLineQuery(tenant);
            List<APInvoiceLinePM> aPInvoiceLinePMs = aPInvoiceLineQuery.GetInvoiceLinesByInvoiceIds(APInvoiceIDs, tenant);
            BankDepositLineQueryService bankDepositLineQueryService = new BankDepositLineQueryService(tenant);
            List<BankDepositLinePM> depositLines = bankDepositLineQueryService.GetDepositLinePMsByDepositIds(depositIds, tenant);


            ARPaymentChequeQueryService aRPaymentChequeQueryService = new ARPaymentChequeQueryService(tenant);

            List<ARPaymentChequePM> cheques = aRPaymentChequeQueryService.GetAllARPaymentChequesByPaymentIds(arpaymentIds, tenant);
            //ARInvoice
            string C100Code = "C100",
                   C100Counter = a + "".PadLeft(9, '0'),
                   C100VatNumber = a + "".PadLeft(9, '0'),
                   C100DocumentType = a + "".PadLeft(3, '0'),
                   C100DocumentReference = a + "".PadLeft(20, ' '),
                   C100DocumentCreateDate = a + "".PadLeft(8, '0'),
                   C100DocumentTime = a + "".PadLeft(4, '0'),
                   C100Name = a + "".PadLeft(50, ' '),
                   C100Street = a + "".PadLeft(50, ' '),
                   C100HouseNumber = a + "".PadLeft(10, ' '),
                   C100City = a + "".PadLeft(30, ' '),
                   C100Mikud = a + "".PadLeft(8, ' '),
                   C100Country = a + "".PadLeft(30, ' '),
                   C100CountryCode = a + "".PadLeft(2, ' '),
                   C100Phone = a + "".PadLeft(15, ' '),
                   C100VatClient = a + "".PadLeft(9, '0'),
                   C100ValueDate = a + "".PadLeft(8, '0'),
                   C100FinalSum = a + "+".PadRight(15, '0'),
                   C100Currency = a + "".PadLeft(3, ' '),
                   C100SumBeforeDiscount = a + "".PadLeft(15, '0'),
                   C100Discount = a + "".PadLeft(15, ' '),
                   C100SumAfterDiscount = a + "".PadLeft(15, '0'),
                   C100VatSum = a + "".PadLeft(15, ' '),
                   C100SumIncludeVat = a + "".PadLeft(15, '0'),
                   C100NikuiBamakor = a + "".PadLeft(12, ' '),
                   C100Key = a + "".PadLeft(15, ' '),
                   C100Fit = a + "".PadLeft(10, ' '),
                   C100Cancled = a + "".PadLeft(1, ' '),
                   C100DocumentDate = a + "".PadLeft(8, '0'),
                   C100Branch = a + "".PadLeft(7, ' '),
                   C100User = a + "".PadLeft(9, ' '),
                   C100Mekasher = a + "".PadLeft(7, ' '),
                   C100Future = a + "".PadLeft(13, ' ');
            foreach (C100Data item in ARC100)
            {
                counter++;
                C100Count++;
                if (counter.ToString().Length > 9)
                {
                    C100Counter = a + counter.ToString().Substring(0, 9).PadLeft(9, '0');
                }
                else
                {
                    C100Counter = a + counter.ToString().PadLeft(9, '0');
                }

                if (tenantPM.VatNumber != null)
                {
                    if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                    C100VatNumber = a + tenantPM.VatNumber.PadLeft(9, '0');
                }
                C100DocumentType = a + item.DocumentType;

                if (item.DocumentReference != null)
                {
                    if (item.DocumentReference.Length > 20) { item.DocumentReference = item.DocumentReference.Substring(0, 20); }
                    C100DocumentReference = a + item.DocumentReference.PadLeft(20, ' ');
                }

                var CreateDate = String.Format("{0:yyyyMMdd}", item.DocumentCreateDate);
                var CreateDateTime = item.DocumentCreateDate.Value.ToString("HHmm");

                if (CreateDate.Length > 8) { CreateDate = CreateDate.Substring(0, 8); }
                C100DocumentCreateDate = a + CreateDate.PadLeft(8, '0');
                if (CreateDateTime.Length > 4) { CreateDateTime = CreateDateTime.Substring(0, 4); }
                C100DocumentTime = a + CreateDateTime.PadLeft(4, '0');

                if (item.CustomerVendorName != null)
                {
                    if (item.CustomerVendorName.Length > 50) { item.CustomerVendorName = item.CustomerVendorName.Substring(0, 50); }
                    C100Name = a + item.CustomerVendorName.PadLeft(50, ' ');
                }

                string address = item.AddressStreet;
                if (item.AddressStreet != null)
                {
                    if (item.AddressStreet.Length > 50) { item.AddressStreet = item.AddressStreet.Substring(0, 50); }
                    C100Street = a + item.AddressStreet.PadLeft(50, ' ');

                    if (address != null)
                    {
                        if (address.Length > 60)
                        {
                            item.AddressHomeNO = address.Substring(51, 10);
                            C100HouseNumber = a + item.AddressHomeNO.PadLeft(10, ' ');
                        }
                        else if (address.Length > 51)
                        {
                            int s = address.Length - 50;
                            item.AddressHomeNO = address.Substring(50, s);
                            C100HouseNumber = a + item.AddressHomeNO.PadLeft(10, ' ');
                        }
                    }
                }



                if (item.AddressCity != null)
                {
                    if (item.AddressCity.Length > 30) { item.AddressCity = item.AddressCity.Substring(0, 30); }
                    C100City = a + item.AddressCity.PadLeft(30, ' ');
                }


                if (item.AddressZIPCode != null)
                {
                    if (item.AddressZIPCode.Length > 8) { item.AddressZIPCode = item.AddressZIPCode.Substring(0, 8); }
                    C100Mikud = a + item.AddressZIPCode.PadLeft(8, ' ');
                }

                if (item.AddressCountry != null)
                {
                    if (item.AddressCountry.Length > 30) { item.AddressCountry = item.AddressCountry.Substring(0, 30); }
                    C100Country = a + item.AddressCountry.PadLeft(30, ' ');
                }

                if (item.AddressCountryCode != null)
                {
                    var partnerCode = computingPartnerTranslationHelper.GetComputingPartnerCodeTranslation(item.AddressCountryCode, "Cust", "Country");

                    if (partnerCode != null)
                    {
                        if (partnerCode.Length > 2) { partnerCode = partnerCode.Substring(0, 2); }
                        C100CountryCode = a + partnerCode.PadLeft(2, ' ');
                    }
                }
                if (item.CustomeVendorTelephone != null)
                {
                    if (item.CustomeVendorTelephone.Length > 15) { item.CustomeVendorTelephone = item.CustomeVendorTelephone.Substring(0, 15); }
                    C100Phone = a + item.CustomeVendorTelephone.PadLeft(15, ' ');
                }

                if (item.CustomerVendorVatNumber != null)
                {
                    C100VatClient = a + ProcessVatNumber(item.CustomerVendorVatNumber);
                }
                var ValueDate = String.Format("{0:yyyyMMdd}", item.ValueDate);
                if (ValueDate != null)
                {

                    C100ValueDate = a + ValueDate;
                }

                string AmountBFDiscount = Format((decimal)item.TotalDocumentsAmountBeforeDiscount); 

                if (AmountBFDiscount != null)
                {
                    if (AmountBFDiscount.Length > 15) { AmountBFDiscount = AmountBFDiscount.Substring(0, 15); }

                    if (item.TotalDocumentsAmountBeforeDiscount < 0)
                    {
                        C100SumBeforeDiscount = a + "-" + AmountBFDiscount.PadLeft(14, '0');
                    }
                    else if (item.TotalDocumentsAmountBeforeDiscount >= 0)
                    {
                        C100SumBeforeDiscount = a + "+" + AmountBFDiscount.PadLeft(14, '0');
                    }
                }
                string AmountAFDiscount = Format((decimal)item.TotalDocumentsAmountAfterDiscount); 

                if (AmountAFDiscount != null)
                {
                    if (AmountAFDiscount.Length > 15) { AmountAFDiscount = AmountAFDiscount.Substring(0, 15); }
                    if (item.TotalDocumentsAmountAfterDiscount < 0)
                    {
                        C100SumAfterDiscount = a + "-" + AmountAFDiscount.PadLeft(14, '0');
                    }
                    else if (item.TotalDocumentsAmountAfterDiscount >= 0)
                    {
                        C100SumAfterDiscount = a + "+" + AmountAFDiscount.PadLeft(14, '0');
                    }
                }
                if (item.DocumentReference == "1291")
                {

                }

                List<ARInvoiceTotalVATPM> aRInvoiceTotalVATs = totalVats.Where(d => d.ARInvoiceId == item.ARInvoiceId).ToList();
                if (aRInvoiceTotalVATs.Count > 0)
                {
                    item.VatAmount = aRInvoiceTotalVATs.Sum(d => d.LocalVATAmount);
                    string vatAmount = Format((decimal)item.VatAmount); 
                    if (vatAmount != null)
                    {
                        if (vatAmount.Length > 15) { vatAmount = vatAmount.Substring(0, 15); }
                        if (item.VatAmount < 0)
                        {
                            C100VatSum = a + "-" + vatAmount.PadLeft(14, '0');

                        }
                        else if (item.VatAmount >= 0)
                        {
                            C100VatSum = a + "+" + vatAmount.PadLeft(14, '0');
                        }
                    }
                    else
                    {
                        myStringBuilder.Append(a);
                        myStringBuilder.Append(' ', 15);
                    }
                }
                string DocumentAmountAndVATAmount = Format((decimal)item.DocumentAmountAndVATAmount); 

                if (DocumentAmountAndVATAmount != null)
                {
                    if (item.DocumentAmountAndVATAmount >= 0)
                    {
                        C100SumIncludeVat = a + "+";
                        if (DocumentAmountAndVATAmount.Length > 14) { DocumentAmountAndVATAmount = DocumentAmountAndVATAmount.Substring(0, 14); }
                        C100SumIncludeVat += DocumentAmountAndVATAmount.PadLeft(14, '0');
                    }

                    else if (item.DocumentAmountAndVATAmount < 0)
                    {
                        C100SumIncludeVat = a + "-";
                        if (DocumentAmountAndVATAmount.Length > 14) { DocumentAmountAndVATAmount = DocumentAmountAndVATAmount.Substring(0, 14); }
                        C100SumIncludeVat += DocumentAmountAndVATAmount.PadLeft(14, '0');
                    }
                }
                GLAccountPM gLAccountPM = acccounts.Where(d => d.Id == item.GLAccountId).FirstOrDefault();
                if (gLAccountPM != null)   
                {
                    item.CustomerVendorCode = gLAccountPM.DisplayNumber;
                    if (item.CustomerVendorCode != null)
                    {
                        if (item.CustomerVendorCode.Length > 15) { item.CustomerVendorCode = item.CustomerVendorCode.Substring(0, 15); }
                        C100Key = a + item.CustomerVendorCode.PadLeft(15, ' ');
                    }

                }
                if (item.IsCancelled)
                {
                    C100Cancled = "1";
                }
                else
                {
                    C100Cancled = "0";
                }
                var DocuemntsReferenceDate = String.Format("{0:yyyyMMdd}", item.DocuemntsReferenceDate);
                if (DocuemntsReferenceDate != null)
                {

                    C100DocumentDate = a + DocuemntsReferenceDate;
                }
                if (item.CreatedbyUser != null)
                {
                    if (item.CreatedbyUser.Length > 9) { item.CreatedbyUser = item.CreatedbyUser.Substring(0, 9); }
                    C100User = item.CreatedbyUser.PadLeft(9, ' ');
                }

                string c100 = C100Count.ToString();
                if (c100.Length > 7) { c100 = c100.Substring(0, 7); }
                C100Mekasher = a + c100.PadLeft(7, '0');
                object[] variables = new object[]{
                   C100Code,C100Counter,C100VatNumber,C100DocumentType,C100DocumentReference,
                   C100DocumentCreateDate,C100DocumentTime,C100Name,C100Street,C100HouseNumber,
                   C100City,C100Mikud,C100Country,C100CountryCode,C100Phone,C100VatClient,
                   C100ValueDate,C100FinalSum,C100Currency,C100SumBeforeDiscount,C100Discount,
                   C100SumAfterDiscount,C100VatSum,C100SumIncludeVat,C100NikuiBamakor,C100Key,
                   C100Fit,C100Cancled,C100DocumentDate,C100Branch,C100User,C100Mekasher,C100Future
                };
                AddSBline(ref myStringBuilder, variables);
                //D110
                string D110Code = "D110",
                       D110Counter = a + "".PadLeft(9, '0'),
                       D110VatNumber = a + "".PadLeft(9, '0'),
                       D110DocumentType = a + "".PadLeft(3, '0'),
                       D110DocumentReference = a + "".PadLeft(20, ' '),
                       D110LineNumber = a + "0".PadLeft(4, '0'),
                       D110BaseType = a + "0".PadLeft(3, '0'),
                       D110BaseNumber = a + "".PadLeft(20, ' '),
                       D110TransactionType = a + "1",
                       D110Makat = a + "0".PadLeft(20, ' '),
                       D110Description = a + "".PadLeft(30, ' '),
                       D110ProducerName = a + "".PadLeft(50, ' '),
                       D110MisparSiduri = a + "".PadLeft(30, ' '),
                       D110Mida = a + "".PadLeft(15, ' ') + "יחידה", 
                       D110Quantity = a + "".PadLeft(17, '0'),
                       D110UnitPrice = a + "".PadLeft(15, '0'),
                       D110Discount = a + "+".PadRight(15, '0'),
                       D110Sum = a + "+".PadRight(15, '0'),
                       D110VatPercentage = a + "".PadLeft(4, '0'),
                       D110Branch = a + "".PadLeft(7, ' '),
                       D110DocumentDate = a + "".PadLeft(8, '0'),
                       D110MekasherLakoteret = a + "".PadLeft(7, ' '),
                       D110MezaheSnif = a + "".PadLeft(7, ' '),
                       D110FutureData = a + "".PadLeft(21, ' ');

                List<ARInvoiceLinePM> lines = aRInvoiceLinePMs.Where(d => d.ARInvoiceId == item.ARInvoiceId).ToList();
                foreach (ARInvoiceLinePM line in lines)
                {
                    D110Count++;
                    counter++;
                    if (counter.ToString().Length > 9)
                    {
                        D110Counter = a + counter.ToString().Substring(0, 9).PadLeft(9, '0');
                    }
                    else
                    {
                        D110Counter = a + counter.ToString().PadLeft(9, '0');
                    }

                    if (tenantPM.VatNumber != null)
                    {
                        if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                        D110VatNumber = a + tenantPM.VatNumber.PadLeft(9, '0');
                    }
                    if (item.DocumentType.Length > 3)
                    {
                        item.DocumentType = item.DocumentType.Substring(0, 3).PadLeft(3, '0');
                    }
                    D110DocumentType = a + item.DocumentType.PadLeft(3, '0');

                    if (item.DocumentReference != null)
                    {
                        if (item.DocumentReference.Length > 20) { item.DocumentReference = item.DocumentReference.Substring(0, 20); }
                        D110DocumentReference = a + item.DocumentReference.PadLeft(20, ' ');
                    }
                    if (line.LineNumber.ToString().Length > 4)
                    {
                        D110LineNumber = a + line.LineNumber.ToString().Substring(0, 4);
                    }
                    D110LineNumber = a + line.LineNumber.ToString().PadLeft(4, '0');
                    if (line.Description != null)
                    {
                        if (line.Description.Length > 30) { line.Description = line.Description.Substring(0, 30); }
                        D110Description = a + line.Description.PadLeft(30, ' ');
                    }


                    if (line.Quantity != null)
                    {
                        isQuantity = true;
                        string quantity = Format((decimal)line.Quantity, true); 
                        if (line.Quantity > 0)
                        {
                            D110Quantity = a + "+";
                            if (quantity.Length > 16) { quantity = quantity.Substring(0, 16); }
                            D110Quantity += quantity.PadLeft(16, '0');
                        }
                        else if (line.Quantity < 0)
                        {
                            D110Quantity = a + "+";
                            if (quantity.Length > 16) { quantity = quantity.Substring(0, 16); }
                            D110Quantity += quantity.PadLeft(16, '0');
                        }
                        isQuantity = false;
                    }
                    if (line.Quantity == null || line.Quantity == 0)
                    {
                        line.Quantity = 1;
                    }
                    double? UnitPrice = line.LocalCurrencyAmount / line.Quantity;

                    if (UnitPrice != null)
                    {
                        string unitPrice = Format((decimal)UnitPrice); 

                        if (UnitPrice < 0)
                        {
                            D110UnitPrice = a + "-";
                            if (unitPrice.Length > 14) { unitPrice = unitPrice.Substring(0, 14); }
                            D110UnitPrice += unitPrice.PadLeft(14, '0');
                        }
                        else if (UnitPrice >= 0)
                        {
                            D110UnitPrice = a + "+";
                            if (unitPrice.Length > 14) { unitPrice = unitPrice.Substring(0, 14); }
                            D110UnitPrice += unitPrice.PadLeft(14, '0');
                        }

                    }

                    if (line.LocalCurrencyAmount != null)
                    {
                        string LocalCurrencyAmount = Format((decimal)line.LocalCurrencyAmount); 
                        if (LocalCurrencyAmount.Length > 14) { LocalCurrencyAmount = LocalCurrencyAmount.Substring(0, 14); }


                        if (line.LocalCurrencyAmount >= 0)
                        {
                            D110Sum = a + "+";
                            D110Sum += LocalCurrencyAmount.PadLeft(14, '0');
                        }

                        else if (line.LocalCurrencyAmount < 0)
                        {
                            D110Sum = a + "-";
                            D110Sum += LocalCurrencyAmount.PadLeft(14, '0');
                        }
                    }
                    if (line.VatPercentage != null)
                    {
                        string VatPercentage = Format((decimal)line.VatPercentage, false, true);
                        if (VatPercentage.Length > 4) { VatPercentage = VatPercentage.Substring(0, 4); }
                        D110VatPercentage = a + VatPercentage.PadRight(4, '0');
                    }
                    if (DocuemntsReferenceDate != null)
                    {
                        D110DocumentDate = a + DocuemntsReferenceDate;
                    }

                    if (c100.Length > 7) { c100 = c100.Substring(0, 7); }
                    D110MekasherLakoteret = a + c100.PadLeft(7, '0');

                    variables = new object[]{
                    D110Code, D110Counter, D110VatNumber, D110DocumentType, D110DocumentReference, D110LineNumber, D110BaseType, D110BaseNumber, D110TransactionType, D110Makat, D110Description, D110ProducerName,
                    D110MisparSiduri, D110Mida, D110Quantity, D110UnitPrice, D110Discount, D110Sum, D110VatPercentage, D110Branch, D110DocumentDate, D110MekasherLakoteret, D110MezaheSnif,
                    D110FutureData};
                    AddSBline(ref myStringBuilder, variables);
                }
            }

            //APInvoice
            C100Code = "C100";
            C100Counter = a + "".PadLeft(9, '0');
            C100VatNumber = a + "".PadLeft(9, '0');
            C100DocumentType = a + "".PadLeft(3, '0');
            C100DocumentReference = a + "".PadLeft(20, ' ');
            C100DocumentCreateDate = a + "".PadLeft(8, '0');
            C100DocumentTime = a + "".PadLeft(4, '0');
            C100Name = a + "".PadLeft(50, ' ');
            C100Street = a + "".PadLeft(50, ' ');
            C100HouseNumber = a + "".PadLeft(10, ' ');
            C100City = a + "".PadLeft(30, ' ');
            C100Mikud = a + "".PadLeft(8, ' ');
            C100Country = a + "".PadLeft(30, ' ');
            C100CountryCode = a + "".PadLeft(2, ' ');
            C100Phone = a + "".PadLeft(15, ' ');
            C100VatClient = a + "".PadLeft(9, '0');
            C100ValueDate = a + "".PadLeft(8, '0');
            C100FinalSum = a + "+".PadRight(15, '0');
            C100Currency = a + "".PadLeft(3, ' ');
            C100SumBeforeDiscount = a + "".PadLeft(15, '0');
            C100Discount = a + "".PadLeft(15, ' ');
            C100SumAfterDiscount = a + "".PadLeft(15, '0');
            C100VatSum = a + "".PadLeft(15, ' ');
            C100SumIncludeVat = a + "".PadLeft(15, '0');
            C100NikuiBamakor = a + "".PadLeft(12, ' ');
            C100Key = a + "".PadLeft(15, ' ');
            C100Fit = a + "".PadLeft(10, ' ');
            C100Cancled = a + "".PadLeft(1, ' ');
            C100DocumentDate = a + "".PadLeft(8, '0');
            C100Branch = a + "".PadLeft(7, ' ');
            C100User = a + "".PadLeft(9, ' ');
            C100Mekasher = a + "".PadLeft(7, ' ');
            C100Future = a + "".PadLeft(13, ' ');
            foreach (C100Data item in APC100)
            {
                counter++;
                C100Count++;
                if (counter.ToString().Length > 9)
                {
                    C100Counter = a + counter.ToString().Substring(0, 9).PadLeft(9, '0');
                }
                else
                {
                    C100Counter = a + counter.ToString().PadLeft(9, '0');
                }

                if (tenantPM.VatNumber != null)
                {
                    if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                    C100VatNumber = a + tenantPM.VatNumber.PadLeft(9, '0');
                }
                C100DocumentType = a + item.DocumentType;

                if (item.DocumentReference != null)
                {
                    if (item.DocumentReference.Length > 20) { item.DocumentReference = item.DocumentReference.Substring(0, 20); }
                    C100DocumentReference = a + item.DocumentReference.PadLeft(20, ' ');
                }
                var CreateDate = String.Format("{0:yyyyMMdd}", item.DocumentCreateDate);
                var CreateDateTime = item.DocumentCreateDate.Value.ToString("HHmm");

                if (CreateDate.Length > 8) { CreateDate.Substring(0, 8); }
                C100DocumentCreateDate = a + CreateDate.PadLeft(8, '0');
                if (CreateDateTime.Length > 4) { CreateDateTime.Substring(0, 4); }
                C100DocumentTime = a + CreateDateTime.PadLeft(4, '0');

                if (item.CustomerVendorName != null)
                {
                    if (item.CustomerVendorName.Length > 50) { item.CustomerVendorName = item.CustomerVendorName.Substring(0, 50); }
                    C100Name = a + item.CustomerVendorName.PadLeft(50, ' ');
                }


                AddressList billingAddress = addreses.Where(d => d.CardId == item.VendorId && d.AddressTypeId == "B").FirstOrDefault();
                AddressList mainaddress = addreses.Where(d => d.CardId == item.VendorId && d.AddressTypeId == "M").FirstOrDefault();


                string address = null;
                if (billingAddress != null)
                {

                    address = billingAddress.Address1;
                    item.AddressStreet = address;
                    if (item.AddressStreet != null)
                    {
                        if (item.AddressStreet.Length > 50) { item.AddressStreet = item.AddressStreet.Substring(0, 50); }
                        C100Street = item.AddressStreet.PadLeft(50, ' ');
                        if (address.Length > 60)
                        {
                            item.AddressHomeNO = address.Substring(51, 10);
                            C100HouseNumber = a + item.AddressHomeNO.PadLeft(10, ' ');
                        }
                        else if (address.Length > 51)
                        {
                            int s = address.Length - 50;
                            item.AddressHomeNO = address.Substring(50, s);
                            C100HouseNumber = a + item.AddressHomeNO.PadLeft(10, ' ');
                        }
                    }
                    if (billingAddress.City != null)
                    {
                        if (billingAddress.City.Length > 30) { billingAddress.City = billingAddress.City.Substring(0, 30); }
                        C100City = a + billingAddress.City.PadLeft(30, ' ');
                    }
                    if (billingAddress.ZipCode != null)
                    {
                        if (billingAddress.ZipCode.Length > 8) { billingAddress.ZipCode = billingAddress.ZipCode.Substring(0, 8); }
                        C100Mikud = a + billingAddress.ZipCode.PadLeft(8, ' ');
                    }
                    if (billingAddress.CountryName != null)
                    {
                        if (billingAddress.CountryName.Length > 30) { billingAddress.CountryName = billingAddress.CountryName.Substring(0, 30); }
                        C100Country = a + billingAddress.CountryName.PadLeft(30, ' ');
                    }
                    if (billingAddress.CountryCode != null)
                    {
                        var partnerCode = computingPartnerTranslationHelper.GetComputingPartnerCodeTranslation(billingAddress.CountryCode, "Cust", "Country");

                        if (partnerCode != null)
                        {
                            if (partnerCode.Length > 2) { partnerCode = partnerCode.Substring(0, 2); }
                            C100CountryCode = a + partnerCode.PadLeft(2, ' ');
                        }
                    }
                }

                else if (mainaddress != null)
                {



                    address = mainaddress.Address1;
                    item.AddressStreet = address;
                    if (item.AddressStreet != null)
                    {
                        if (item.AddressStreet.Length > 50) { item.AddressStreet = item.AddressStreet.Substring(0, 50); }
                        C100Street = a + item.AddressStreet.PadLeft(50, ' ');
                        if (address.Length > 60)
                        {
                            item.AddressHomeNO = address.Substring(51, 10);
                            C100HouseNumber = a + item.AddressHomeNO.PadLeft(10, ' ');
                        }
                        else if (address.Length > 51)
                        {
                            int s = address.Length - 50;
                            item.AddressHomeNO = address.Substring(50, s);
                            C100HouseNumber = a + item.AddressHomeNO.PadLeft(10, ' ');
                        }
                    }
                    if (mainaddress.City != null)
                    {
                        if (mainaddress.City.Length > 30) { mainaddress.City = mainaddress.City.Substring(0, 30); }
                        C100City = a + mainaddress.City.PadLeft(30, ' ');
                    }
                    if (mainaddress.ZipCode != null)
                    {
                        if (mainaddress.ZipCode.Length > 8) { mainaddress.ZipCode = mainaddress.ZipCode.Substring(0, 8); }
                        C100Mikud = a + mainaddress.ZipCode.PadLeft(8, ' ');
                    }
                    if (mainaddress.CountryName != null)
                    {
                        if (mainaddress.CountryName.Length > 30) { mainaddress.CountryName = mainaddress.CountryName.Substring(0, 30); }
                        C100Country = a + mainaddress.CountryName.PadLeft(30, ' ');
                    }
                    if (mainaddress.CountryCode != null)
                    {
                        var partnerCode = computingPartnerTranslationHelper.GetComputingPartnerCodeTranslation(mainaddress.CountryCode, "Cust", "Country");

                        if (partnerCode != null)
                        {
                            if (partnerCode.Length > 2) { partnerCode = partnerCode.Substring(0, 2); }
                            C100CountryCode = a + partnerCode.PadLeft(2, ' ');
                        }
                    }

                    if (mainaddress.PhoneNumber != null)
                    {
                        if (mainaddress.PhoneNumber.Length > 15) { mainaddress.PhoneNumber = mainaddress.PhoneNumber.Substring(0, 15); }
                        C100Phone = a + mainaddress.PhoneNumber.PadLeft(15, ' ');
                    }
                }

                if (item.CustomerVendorVatNumber != null)
                {
                    C100VatClient = a + ProcessVatNumber(item.CustomerVendorVatNumber);
                }
                var ValueDate = String.Format("{0:yyyyMMdd}", item.ValueDate);
                if (ValueDate != null)
                {

                    C100ValueDate = a + ValueDate;
                }
                string AmountBFDiscount = Format((decimal)item.TotalDocumentsAmountBeforeDiscount); 

                if (AmountBFDiscount != null)
                {
                    if (AmountBFDiscount.Length > 15) { AmountBFDiscount = AmountBFDiscount.Substring(0, 15); }
                    if (item.TotalDocumentsAmountBeforeDiscount < 0)
                    {
                        C100SumBeforeDiscount = a + "-" + AmountBFDiscount.PadLeft(14, '0');

                    }
                    else if (item.TotalDocumentsAmountBeforeDiscount >= 0)
                    {
                        C100SumBeforeDiscount = a + "+" + AmountBFDiscount.PadLeft(14, '0');
                    }
                }
                string AmountAFDiscount = Format((decimal)item.TotalDocumentsAmountAfterDiscount); 


                if (AmountAFDiscount != null)
                {
                    if (AmountAFDiscount.Length > 15) { AmountAFDiscount = AmountAFDiscount.Substring(0, 15); }



                    if (item.TotalDocumentsAmountAfterDiscount < 0)
                    {
                        C100SumAfterDiscount = a + "-" + AmountAFDiscount.PadLeft(14, '0');
                    }
                    else if (item.TotalDocumentsAmountAfterDiscount >= 0)
                    {
                        C100SumAfterDiscount = a + "+" + AmountAFDiscount.PadLeft(14, '0');
                    }
                }
                List<APInvoiceTotalVATPM> aPInvoiceTotalVATs = APtotalVats.Where(d => d.APInvoiceId == item.APInvoiceId).ToList();
                if (aPInvoiceTotalVATs.Count > 0)
                {
                    item.VatAmount = aPInvoiceTotalVATs.Sum(d => d.LocalVATAmount);
                    string vatAmount = Format((decimal)item.VatAmount); 
                    if (vatAmount != null)
                    {
                        if (vatAmount.Length > 15) { vatAmount = vatAmount.Substring(0, 15); }



                        if (item.VatAmount < 0)
                        {
                            C100VatSum = a + "-" + vatAmount.PadLeft(14, '0');
                        }
                        else if (item.VatAmount >= 0)
                        {
                            C100VatSum = a + "+" + vatAmount.PadLeft(14, '0');
                        }
                    }
                }

                string DocumentAmountAndVATAmount = Format((decimal)item.DocumentAmountAndVATAmount); 

                if (DocumentAmountAndVATAmount != null)
                {
                    if (item.DocumentAmountAndVATAmount >= 0)
                    {
                        C100SumIncludeVat = a + "+";
                        if (DocumentAmountAndVATAmount.Length > 14) { DocumentAmountAndVATAmount = DocumentAmountAndVATAmount.Substring(0, 14); }
                        C100SumIncludeVat += DocumentAmountAndVATAmount.PadLeft(14, '0');
                    }

                    else if (item.DocumentAmountAndVATAmount < 0)
                    {
                        C100SumIncludeVat = a + "-";
                        if (DocumentAmountAndVATAmount.Length > 14) { DocumentAmountAndVATAmount = DocumentAmountAndVATAmount.Substring(0, 14); }
                        C100SumIncludeVat += DocumentAmountAndVATAmount.PadLeft(14, '0');
                    }
                }
                GLAccountPM gLAccountPM = acccounts.Where(d => d.Id == item.GLAccountId).FirstOrDefault();
                if (gLAccountPM != null)
                {
                    item.CustomerVendorCode = gLAccountPM.DisplayNumber;
                    if (item.CustomerVendorCode != null)
                    {
                        if (item.CustomerVendorCode.Length > 15) { item.CustomerVendorCode = item.CustomerVendorCode.Substring(0, 15); }
                        C100Key = a + item.CustomerVendorCode.PadLeft(15, ' ');
                    }
                }
                if (item.IsCancelled)
                {
                    C100Cancled = "1";
                }
                else
                {
                    C100Cancled = "0";
                }
                var DocuemntsReferenceDate = String.Format("{0:yyyyMMdd}", item.DocuemntsReferenceDate);
                if (DocuemntsReferenceDate != null)
                {

                    C100DocumentDate = a + DocuemntsReferenceDate;
                }
                if (item.CreatedbyUser != null)
                {
                    if (item.CreatedbyUser.Length > 9) { item.CreatedbyUser = item.CreatedbyUser.Substring(0, 9); }
                    C100User = a + item.CreatedbyUser.PadLeft(9, ' ');
                }
                string c100 = C100Count.ToString();
                if (c100.Length > 7) { c100 = c100.Substring(0, 7); }
                C100Mekasher = a + c100.PadLeft(7, '0');
                object[] variables = new object[]{
                   C100Code,C100Counter,C100VatNumber,C100DocumentType,C100DocumentReference,
                   C100DocumentCreateDate,C100DocumentTime,C100Name,C100Street,C100HouseNumber,
                   C100City,C100Mikud,C100Country,C100CountryCode,C100Phone,C100VatClient,
                   C100ValueDate,C100FinalSum,C100Currency,C100SumBeforeDiscount,C100Discount,
                   C100SumAfterDiscount,C100VatSum,C100SumIncludeVat,C100NikuiBamakor,C100Key,
                   C100Fit,C100Cancled,C100DocumentDate,C100Branch,C100User,C100Mekasher,C100Future
                };
                AddSBline(ref myStringBuilder, variables);
                //D110
                List<APInvoiceLinePM> lines = aPInvoiceLinePMs.Where(d => d.APInvoiceId == item.APInvoiceId).ToList();
                string D110Code = "D110",
                       D110Counter = a + "".PadLeft(9, '0'),
                       D110VatNumber = a + "".PadLeft(9, '0'),
                       D110DocumentType = a + "".PadLeft(3, '0'),
                       D110DocumentReference = a + "".PadLeft(20, ' '),
                       D110LineNumber = a + "0".PadLeft(4, '0'),
                       D110BaseType = a + "0".PadLeft(3, '0'),
                       D110BaseNumber = a + "".PadLeft(20, ' '),
                       D110TransactionType = a + "1",
                       D110Makat = a + "0".PadLeft(20, ' '),
                       D110Description = a + "".PadLeft(30, ' '),
                       D110ProducerName = a + "".PadLeft(50, ' '),
                       D110MisparSiduri = a + "".PadLeft(30, ' '),
                       D110Mida = a + "".PadLeft(15, ' ') + "יחידה",
                       D110Quantity = a + "+" + "".PadLeft(11, '0') + "10000",
                       D110UnitPrice = a + "".PadLeft(15, '0'),
                       D110Discount = a + "+".PadRight(15, '0'),
                       D110Sum = a + "+".PadRight(15, '0'),
                       D110VatPercentage = a + "".PadLeft(4, '0'),
                       D110Branch = a + "".PadLeft(7, ' '),
                       D110DocumentDate = a + "".PadLeft(8, '0'),
                       D110MekasherLakoteret = a + "".PadLeft(7, ' '),
                       D110MezaheSnif = a + "".PadLeft(7, ' '),
                       D110FutureData = a + "".PadLeft(21, ' ');
                foreach (APInvoiceLinePM line in lines)
                {
                    counter++;
                    D110Count++;
                    if (counter.ToString().Length > 9)
                    {
                        D110Counter = a + counter.ToString().Substring(0, 9).PadLeft(9, '0');
                    }
                    else
                    {
                        D110Counter = a + counter.ToString().PadLeft(9, '0');
                    }

                    if (item.CustomerVendorVatNumber != null)
                    {
                        D110VatNumber = a + ProcessVatNumber(item.CustomerVendorVatNumber);
                    }

                    D110DocumentType = a + "700";

                    if (item.DocumentReference != null)
                    {
                        if (item.DocumentReference.Length > 20) { item.DocumentReference = item.DocumentReference.Substring(0, 20); }
                        D110DocumentReference = a + item.DocumentReference.PadLeft(20, ' ');
                    }
                    if (line.LineNumber.ToString().Length > 4)
                    {
                        D110LineNumber = a + line.LineNumber.ToString().Substring(0, 4);
                    }
                    D110LineNumber = a + line.LineNumber.ToString().PadLeft(4, '0');

                    if (line.Description != null)
                    {
                        if (line.Description.Length > 30) { line.Description = line.Description.Substring(0, 30); }
                        D110Description = a + line.Description.PadLeft(30, ' ');
                    }

                    if (line.LocalCurrencyAmount != null)
                    {
                        string LocalCurrencyAmount = Format((decimal)line.LocalCurrencyAmount); 
                        if (LocalCurrencyAmount.Length > 15) { LocalCurrencyAmount = LocalCurrencyAmount.Substring(0, 15); }

                        if (line.LocalCurrencyAmount >= 0)
                        {
                            D110UnitPrice = a + "+";
                            D110UnitPrice += LocalCurrencyAmount.PadLeft(14, '0');
                        }

                        else if (line.LocalCurrencyAmount < 0)
                        {
                            D110UnitPrice = a + "-";
                            D110UnitPrice += LocalCurrencyAmount.PadLeft(14, '0');
                        }
                    }

                    if (line.LocalCurrencyAmount != null)
                    {

                        string LocalCurrencyAmount = Format((decimal)line.LocalCurrencyAmount); 
                        if (LocalCurrencyAmount.Length > 15) { LocalCurrencyAmount = LocalCurrencyAmount.Substring(0, 15); }

                        if (line.LocalCurrencyAmount < 0)
                        {
                            D110Sum = a + "-";
                            D110Sum += LocalCurrencyAmount.PadLeft(14, '0');

                        }

                        else if (line.LocalCurrencyAmount >= 0)
                        {
                            D110Sum = a + "+";
                            D110Sum += LocalCurrencyAmount.PadLeft(14, '0');

                        }

                    }

                    if (line.VatPercentage != null)
                    {
                        string VatPercentage = Format((decimal)line.VatPercentage, false, true);
                        if (VatPercentage.Length > 4) { VatPercentage = VatPercentage.Substring(0, 4); }
                        D110VatPercentage = a + VatPercentage.PadRight(4, '0');
                    }
                    if (DocuemntsReferenceDate != null)
                    {
                        D110DocumentDate = a + DocuemntsReferenceDate;
                    }
                    if (c100.Length > 7) { c100 = c100.Substring(0, 7); }
                    D110MekasherLakoteret = a + c100.PadLeft(7, '0');
                    variables = new object[] {
                    D110Code, D110Counter, D110VatNumber, D110DocumentType, D110DocumentReference, D110LineNumber, D110BaseType, D110BaseNumber, D110TransactionType, D110Makat, D110Description, D110ProducerName,
                    D110MisparSiduri, D110Mida, D110Quantity, D110UnitPrice, D110Discount, D110Sum, D110VatPercentage, D110Branch, D110DocumentDate, D110MekasherLakoteret, D110MezaheSnif,
                    D110FutureData};
                    AddSBline(ref myStringBuilder, variables);
                }
            }

            //ARPayment
            C100Code = "C100";
            C100Counter = a + "".PadLeft(9, '0');
            C100VatNumber = a + "".PadLeft(9, '0');
            C100DocumentType = a + "".PadLeft(3, '0');
            C100DocumentReference = a + "".PadLeft(20, ' ');
            C100DocumentCreateDate = a + "".PadLeft(8, '0');
            C100DocumentTime = a + "".PadLeft(4, '0');
            C100Name = a + "".PadLeft(50, ' ');
            C100Street = a + "".PadLeft(50, ' ');
            C100HouseNumber = a + "".PadLeft(10, ' ');
            C100City = a + "".PadLeft(30, ' ');
            C100Mikud = a + "".PadLeft(8, ' ');
            C100Country = a + "".PadLeft(30, ' ');
            C100CountryCode = a + "".PadLeft(2, ' ');
            C100Phone = a + "".PadLeft(15, ' ');
            C100VatClient = a + "".PadLeft(9, '0');
            C100ValueDate = a + "".PadLeft(8, '0');
            C100FinalSum = a + "+".PadRight(15, '0');
            C100Currency = a + "".PadLeft(3, ' ');
            C100SumBeforeDiscount = a + "".PadLeft(15, '0');
            C100Discount = a + "".PadLeft(15, ' ');
            C100SumAfterDiscount = a + "".PadLeft(15, '0');
            C100VatSum = a + "".PadLeft(15, ' ');
            C100SumIncludeVat = a + "".PadLeft(15, '0');
            C100NikuiBamakor = a + "".PadLeft(12, ' ');
            C100Key = a + "".PadLeft(15, ' ');
            C100Fit = a + "".PadLeft(10, ' ');
            C100Cancled = a + "".PadLeft(1, ' ');
            C100DocumentDate = a + "".PadLeft(8, '0');
            C100Branch = a + "".PadLeft(7, ' ');
            C100User = a + "".PadLeft(9, ' ');
            C100Mekasher = a + "".PadLeft(7, ' ');
            C100Future = a + "".PadLeft(13, ' ');
            foreach (C100Data item in ARPAymentC100)
            {
                C100Count++;
                counter++;
                if (counter.ToString().Length > 9)
                {
                    C100Counter = a + counter.ToString().Substring(0, 9).ToString().PadLeft(9, '0');
                }
                else
                {
                    C100Counter = a + counter.ToString().PadLeft(9, '0');
                }

                if (tenantPM.VatNumber != null)
                {
                    if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                    C100VatNumber = a + tenantPM.VatNumber.PadLeft(9, '0');
                }
                C100DocumentType = item.DocumentType;

                if (item.DocumentReference != null)
                {
                    if (item.DocumentReference.Length > 20) { item.DocumentReference = item.DocumentReference.Substring(0, 20); }
                    C100DocumentReference = a + item.DocumentReference.PadLeft(20, ' ');
                }

                var CreateDate = String.Format("{0:yyyyMMdd}", item.DocumentCreateDate);
                var CreateDateTime = item.DocumentCreateDate.Value.ToString("HHmm");

                if (CreateDate.Length > 8) { CreateDate.Substring(0, 8); }
                C100DocumentCreateDate = a + CreateDate.PadLeft(8, '0');
                if (CreateDateTime.Length > 4) { CreateDateTime.Substring(0, 4); }
                C100DocumentTime = a + CreateDateTime.PadLeft(4, '0');

                if (item.CustomerVendorName != null)
                {
                    if (item.CustomerVendorName.Length > 50) { item.CustomerVendorName = item.CustomerVendorName.Substring(0, 50); }
                    C100Name = a + item.CustomerVendorName.PadLeft(50, ' ');
                }


                AddressList billingAddress = addreses.Where(d => d.CardId == item.VendorId && d.AddressTypeId == "B").FirstOrDefault();
                AddressList mainaddress = addreses.Where(d => d.CardId == item.VendorId && d.AddressTypeId == "M").FirstOrDefault();


                string address = null;
                if (billingAddress != null)
                {

                    address = billingAddress.Address1;
                    item.AddressStreet = address;
                    if (item.AddressStreet != null)
                    {
                        if (item.AddressStreet.Length > 50) { item.AddressStreet = item.AddressStreet.Substring(0, 50); }
                        C100Street = a + item.AddressStreet.PadLeft(50, ' ');
                        if (address.Length > 60)
                        {
                            item.AddressHomeNO = address.Substring(51, 10);
                            C100HouseNumber = a + item.AddressHomeNO.PadLeft(10, ' ');
                        }
                        else if (address.Length > 51)
                        {
                            int s = address.Length - 50;
                            item.AddressHomeNO = address.Substring(50, s);
                            C100HouseNumber = a + item.AddressHomeNO.PadLeft(10, ' ');
                        }

                    }
                    if (billingAddress.City != null)
                    {
                        if (billingAddress.City.Length > 30) { billingAddress.City = billingAddress.City.Substring(0, 30); }
                        C100City = a + billingAddress.City.PadLeft(30, ' ');
                    }
                    if (billingAddress.ZipCode != null)
                    {
                        if (billingAddress.ZipCode.Length > 8) { billingAddress.ZipCode = billingAddress.ZipCode.Substring(0, 8); }
                        C100Mikud = a + billingAddress.ZipCode.PadLeft(8, ' ');
                    }
                    if (billingAddress.CountryName != null)
                    {
                        if (billingAddress.CountryName.Length > 30) { billingAddress.CountryName = billingAddress.CountryName.Substring(0, 30); }
                        C100Country = a + billingAddress.CountryName.PadLeft(30, ' ');
                    }
                    if (billingAddress.CountryCode != null)
                    {
                        var partnerCode = computingPartnerTranslationHelper.GetComputingPartnerCodeTranslation(billingAddress.CountryCode, "Cust", "Country");

                        if (partnerCode != null)
                        {
                            if (partnerCode.Length > 2) { partnerCode = partnerCode.Substring(0, 2); }
                            C100CountryCode = a + partnerCode.PadLeft(2, ' ');
                        }
                    }

                    if (billingAddress.PhoneNumber != null)
                    {
                        if (billingAddress.PhoneNumber.Length > 15) { billingAddress.PhoneNumber = billingAddress.PhoneNumber.Substring(0, 15); }
                        C100Phone = a + billingAddress.PhoneNumber.PadLeft(15, ' ');
                    }
                }

                else if (mainaddress != null)
                {



                    address = mainaddress.Address1;
                    item.AddressStreet = address;
                    if (item.AddressStreet != null)
                    {
                        if (item.AddressStreet.Length > 50) { item.AddressStreet = item.AddressStreet.Substring(0, 50); }
                        C100Street = a + item.AddressStreet.PadLeft(50, ' ');
                        if (address.Length > 60)
                        {
                            item.AddressHomeNO = address.Substring(51, 10);
                            C100HouseNumber = a + item.AddressHomeNO.PadLeft(10, ' ');
                        }
                        else if (address.Length > 51)
                        {
                            int s = address.Length - 50;
                            item.AddressHomeNO = address.Substring(50, s);
                            C100HouseNumber = a + item.AddressHomeNO.PadLeft(10, ' ');
                        }
                    }
                    if (mainaddress.City != null)
                    {
                        if (mainaddress.City.Length > 30) { mainaddress.City = mainaddress.City.Substring(0, 30); }
                        C100City = a + mainaddress.City.PadLeft(30, ' ');
                    }
                    if (mainaddress.ZipCode != null)
                    {
                        if (mainaddress.ZipCode.Length > 8) { mainaddress.ZipCode = mainaddress.ZipCode.Substring(0, 8); }
                        C100Mikud = a + mainaddress.ZipCode.PadLeft(8, ' ');
                    }
                    if (mainaddress.CountryName != null)
                    {
                        if (mainaddress.CountryName.Length > 30) { mainaddress.CountryName = mainaddress.CountryName.Substring(0, 30); }
                        C100Country = a + mainaddress.CountryName.PadLeft(30, ' ');
                    }
                    if (mainaddress.CountryCode != null)
                    {
                        var partnerCode = computingPartnerTranslationHelper.GetComputingPartnerCodeTranslation(mainaddress.CountryCode, "Cust", "Country");

                        if (partnerCode != null)
                        {
                            if (partnerCode.Length > 2) { partnerCode = partnerCode.Substring(0, 2); }
                            C100CountryCode = a + partnerCode.PadLeft(2, ' ');
                        }
                    }

                    if (mainaddress.PhoneNumber != null)
                    {
                        if (mainaddress.PhoneNumber.Length > 15) { mainaddress.PhoneNumber = mainaddress.PhoneNumber.Substring(0, 15); }
                        C100Phone = a + mainaddress.PhoneNumber.PadLeft(15, ' ');
                    }
                }



                if (item.CustomerVendorVatNumber != null)
                {
                    C100VatClient = a + ProcessVatNumber(item.CustomerVendorVatNumber);
                }
                var ValueDate = String.Format("{0:yyyyMMdd}", item.ValueDate);
                if (ValueDate != null)
                {

                    C100ValueDate = a + ValueDate;
                }
                string DocumentAmountAndVATAmount = Format((decimal)item.DocumentAmountAndVATAmount); 

                if (DocumentAmountAndVATAmount != null)
                {
                    if (item.DocumentAmountAndVATAmount >= 0)
                    {
                        C100SumAfterDiscount = a + "+";
                        C100SumBeforeDiscount = a + "+";
                        C100SumIncludeVat = a + "+";
                        if (DocumentAmountAndVATAmount.Length > 14) { DocumentAmountAndVATAmount = DocumentAmountAndVATAmount.Substring(0, 14); }
                        C100SumIncludeVat += DocumentAmountAndVATAmount.PadLeft(14, '0');
                        C100SumAfterDiscount += DocumentAmountAndVATAmount.PadLeft(14, '0');
                        C100SumBeforeDiscount += DocumentAmountAndVATAmount.PadLeft(14, '0');


                    }

                    else if (item.DocumentAmountAndVATAmount < 0)
                    {
                        C100SumAfterDiscount = a + "-";
                        C100SumBeforeDiscount = a + "-";
                        C100SumIncludeVat = a + "-";
                        if (DocumentAmountAndVATAmount.Length > 14) { DocumentAmountAndVATAmount = DocumentAmountAndVATAmount.Substring(0, 14); }
                        C100SumIncludeVat += DocumentAmountAndVATAmount.PadLeft(14, '0');
                        C100SumAfterDiscount += DocumentAmountAndVATAmount.PadLeft(14, '0');
                        C100SumBeforeDiscount += DocumentAmountAndVATAmount.PadLeft(14, '0');
                    }
                }

                GLAccountPM gLAccountPM = acccounts.Where(d => d.Id == item.GLAccountId).FirstOrDefault();
                if (gLAccountPM != null)
                {
                    item.CustomerVendorCode = gLAccountPM.DisplayNumber;
                    if (item.CustomerVendorCode != null)
                    {
                        if (item.CustomerVendorCode.Length > 15) { item.CustomerVendorCode = item.CustomerVendorCode.Substring(0, 15); }
                        C100Key = a + item.CustomerVendorCode.PadLeft(15, ' ');
                    }

                }
                if (item.IsCancelled)
                {
                    C100Cancled = "1";
                }
                else
                {
                    C100Cancled = "0";
                }
                var DocuemntsReferenceDate = String.Format("{0:yyyyMMdd}", item.DocuemntsReferenceDate);
                if (DocuemntsReferenceDate != null)
                {

                    C100DocumentDate = a + DocuemntsReferenceDate;
                }
                if (item.CreatedbyUser != null)
                {
                    if (item.CreatedbyUser.Length > 9) { item.CreatedbyUser = item.CreatedbyUser.Substring(0, 9); }
                    C100User = item.CreatedbyUser.PadLeft(9, ' ');
                }
                C100SumBeforeDiscount = C100SumIncludeVat;
                C100SumAfterDiscount = C100SumIncludeVat;
                string c100 = C100Count.ToString();
                if (c100.Length > 7) { c100 = c100.Substring(0, 7); }
                C100Mekasher = a + c100.PadLeft(7, '0');
                object[] variables = new object[]{
                   C100Code,C100Counter,C100VatNumber,C100DocumentType,C100DocumentReference,
                   C100DocumentCreateDate,C100DocumentTime,C100Name,C100Street,C100HouseNumber,
                   C100City,C100Mikud,C100Country,C100CountryCode,C100Phone,C100VatClient,
                   C100ValueDate,C100FinalSum,C100Currency,C100SumBeforeDiscount,C100Discount,
                   C100SumAfterDiscount,C100VatSum,C100SumIncludeVat,C100NikuiBamakor,C100Key,
                   C100Fit,C100Cancled,C100DocumentDate,C100Branch,C100User,C100Mekasher,C100Future
                };
                AddSBline(ref myStringBuilder, variables);
                //D120
                List<ARPaymentChequePM> lines = cheques.Where(d => d.PaymentId == item.ARPaymentId).ToList();

                if (lines.Count > 0)
                {
                    foreach (ARPaymentChequePM line in lines)
                    {
                        counter++;
                        D120Count++;
                        myStringBuilder.Append("D120");


                        if (counter.ToString().Length > 9)
                        {
                            counter.ToString().Substring(0, 9);
                            myStringBuilder.Append(a + counter.ToString().PadLeft(9, '0'));
                        }
                        else
                        {
                            myStringBuilder.Append(counter.ToString().PadLeft(9, '0'));
                        }

                        if (tenantPM.VatNumber != null)
                        {
                            if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                            myStringBuilder.Append(a + tenantPM.VatNumber.PadLeft(9, '0'));
                        }
                        else
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append('0', 9);
                        }

                        myStringBuilder.Append("400");

                        if (item.DocumentReference != null)
                        {
                            if (item.DocumentReference.Length > 20) { item.DocumentReference = item.DocumentReference.Substring(0, 20); }
                            myStringBuilder.Append(a + item.DocumentReference.PadLeft(20, ' '));
                        }
                        else
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append(' ', 20);
                        }
                        myStringBuilder.Append(a);
                        if (item.ARPaymentMethod == "Cheque")
                        {


                            string lineNumber = line.LineNumber.ToString();
                            if (lineNumber.Length > 4)
                            {
                                lineNumber = lineNumber.Substring(0, 4);
                            }
                            myStringBuilder.Append(a + lineNumber.PadLeft(4, '0'));

                        }
                        else
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append("0001");
                        }

                        myStringBuilder.Append(a);
                        if (item.ARPaymentMethod != null)
                        {
                            if (item.ARPaymentMethod == "Cash")
                            {
                                myStringBuilder.Append("1");
                            }
                            else if (item.ARPaymentMethod == "Cheque")
                            {
                                myStringBuilder.Append("2");
                            }
                            else if (item.ARPaymentMethod.ToLower() == "credit card")
                            {
                                myStringBuilder.Append("3");
                            }
                            else if (item.ARPaymentMethod.ToLower() == "bank transfer")
                            {
                                myStringBuilder.Append("4");
                            }
                            else
                            {
                                myStringBuilder.Append("0");
                            }
                        }
                        else
                        {
                            myStringBuilder.Append("0");
                        }

                        if (item.ARPaymentMethod == "Cheque")
                        {

                            if (line.BankId != null)
                            {
                                if (line.BankId.Length > 10)
                                {
                                    line.BankId = line.BankId.Substring(0, 10);
                                }

                                myStringBuilder.Append(a + line.BankId.PadLeft(10, '0'));
                            }
                            else
                            {
                                myStringBuilder.Append(a);
                                myStringBuilder.Append('0', 10);

                            }
                            if (line.BankBranch != null)
                            {
                                if (line.BankBranch.Length > 10)
                                {
                                    line.BankBranch = line.BankBranch.Substring(0, 10);
                                }

                                myStringBuilder.Append(a + line.BankBranch.PadLeft(10, '0'));
                            }
                            else
                            {
                                myStringBuilder.Append(a);
                                myStringBuilder.Append('0', 10);

                            }
                            if (line.BankAccount != null)
                            {
                                if (line.BankAccount.Length > 15)
                                {
                                    line.BankAccount = line.BankAccount.Substring(0, 15);
                                }

                                myStringBuilder.Append(a + line.BankAccount.PadLeft(15, '0'));
                            }
                            else
                            {
                                myStringBuilder.Append(a);
                                myStringBuilder.Append('0', 15);

                            }
                            if (line.ChequeNumber != null)
                            {
                                if (line.ChequeNumber.Length > 10)
                                {
                                    line.ChequeNumber = line.ChequeNumber.Substring(0, 10);
                                }

                                myStringBuilder.Append(a + line.ChequeNumber.PadLeft(10, '0'));
                            }
                            else
                            {
                                myStringBuilder.Append(a);
                                myStringBuilder.Append('0', 10);

                            }

                            var valueDate = String.Format("{0:yyyyMMdd}", line.ValueDate);
                            if (valueDate != null)
                            {
                                myStringBuilder.Append(a + valueDate);
                            }
                            else
                            {
                                myStringBuilder.Append(a);
                                myStringBuilder.Append('0', 8);
                            }



                            string Localamount = Format((decimal)line.LocalAmount); 

                            if (Localamount != null)
                            {
                                if (line.LocalAmount > 0)
                                {
                                    myStringBuilder.Append(a);
                                    myStringBuilder.Append("+");
                                    if (Localamount.Length > 14) { Localamount = Localamount.Substring(0, 14); }
                                    myStringBuilder.Append(a + Localamount.PadLeft(14, '0'));
                                }

                                else if (line.LocalAmount < 0)
                                {
                                    myStringBuilder.Append(a);
                                    myStringBuilder.Append("-");
                                    if (Localamount.Length > 14) { Localamount = Localamount.Substring(0, 14); }
                                    myStringBuilder.Append(a + Localamount.PadLeft(14, '0'));
                                }
                                else
                                {
                                    myStringBuilder.Append(a);
                                    myStringBuilder.Append('0', 15);
                                }
                            }
                            else
                            {
                                myStringBuilder.Append(a);
                                myStringBuilder.Append('0', 15);
                            }



                        }
                        else
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append('0', 10);
                            myStringBuilder.Append(a);
                            myStringBuilder.Append('0', 10);
                            myStringBuilder.Append(a);
                            myStringBuilder.Append('0', 15);
                            myStringBuilder.Append(a);
                            myStringBuilder.Append('0', 10);
                            myStringBuilder.Append(a);
                            myStringBuilder.Append('0', 8);
                            myStringBuilder.Append(a);
                            myStringBuilder.Append('0', 15);
                        }

                        myStringBuilder.Append(a);
                        myStringBuilder.Append('0', 1);
                        myStringBuilder.Append(a);
                        myStringBuilder.Append(' ', 20);
                        myStringBuilder.Append(a);
                        myStringBuilder.Append('0', 1);
                        myStringBuilder.Append(a);
                        myStringBuilder.Append(' ', 7);

                        if (DocuemntsReferenceDate != null)
                        {

                            myStringBuilder.Append(a + DocuemntsReferenceDate);
                        }
                        else
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append('0', 8);
                        }
                        myStringBuilder.Append(a);


                        if (c100.Length > 7) { c100 = c100.Substring(0, 7); }
                        myStringBuilder.Append(a + c100.PadLeft(7, '0'));
                        myStringBuilder.Append(a);
                        myStringBuilder.Append(' ', 60);

                        myStringBuilder.AppendLine();
                    }
                }
                else
                {

                    counter++;
                    D120Count++;
                    myStringBuilder.Append("D120");


                    if (counter.ToString().Length > 9)
                    {
                        counter.ToString().Substring(0, 9);
                        myStringBuilder.Append(a + counter.ToString().PadLeft(9, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append(counter.ToString().PadLeft(9, '0'));
                    }

                    if (tenantPM.VatNumber != null)
                    {
                        if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                        myStringBuilder.Append(a + tenantPM.VatNumber.PadLeft(9, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append(a);
                        myStringBuilder.Append('0', 9);
                    }

                    myStringBuilder.Append("400");


                    if (item.DocumentReference != null)
                    {
                        if (item.DocumentReference.Length > 20) { item.DocumentReference = item.DocumentReference.Substring(0, 20); }
                        myStringBuilder.Append(a + item.DocumentReference.PadLeft(20, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append(a);
                        myStringBuilder.Append(' ', 20);
                    }
                    myStringBuilder.Append(a);


                    myStringBuilder.Append(a);
                    myStringBuilder.Append("0001");


                    myStringBuilder.Append(a);
                    if (item.ARPaymentMethod != null)
                    {
                        if (item.ARPaymentMethod.ToLower() == "cash")
                        {
                            myStringBuilder.Append("1");
                        }

                        else if (item.ARPaymentMethod.ToLower() == "credit card")
                        {
                            myStringBuilder.Append("3");
                        }
                        else if (item.ARPaymentMethod.ToLower() == "bank transfer")
                        {
                            myStringBuilder.Append("4");
                        }
                        else
                        {
                            myStringBuilder.Append("0");
                        }

                    }
                    else
                    {
                        myStringBuilder.Append("0");
                    }



                    myStringBuilder.Append(a);
                    myStringBuilder.Append(' ', 10);
                    myStringBuilder.Append(a);
                    myStringBuilder.Append(' ', 10);
                    myStringBuilder.Append(a);
                    myStringBuilder.Append(' ', 15);
                    myStringBuilder.Append(a);
                    myStringBuilder.Append(' ', 10);
                    myStringBuilder.Append(a);
                    var valueDate = String.Format("{0:yyyyMMdd}", item.ValueDate);
                    if (valueDate != null)
                    {
                        myStringBuilder.Append(a + valueDate);
                    }
                    else
                    {
                        myStringBuilder.Append(a);
                        myStringBuilder.Append('0', 8);
                    }


                    myStringBuilder.Append(a);
                    string Localamount = Format((decimal)item.DocumentAmountAndVATAmount); 

                    if (Localamount != null)
                    {
                        if (item.DocumentAmountAndVATAmount > 0)
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append("+");
                            if (Localamount.Length > 14) { Localamount = Localamount.Substring(0, 14); }
                            myStringBuilder.Append(a + Localamount.PadLeft(14, '0'));
                        }

                        else if (item.DocumentAmountAndVATAmount < 0)
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append("-");
                            if (Localamount.Length > 14) { Localamount = Localamount.Substring(0, 14); }
                            myStringBuilder.Append(a + Localamount.PadLeft(14, '0'));
                        }
                        else
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append('0', 15);
                        }
                    }
                    else
                    {
                        myStringBuilder.Append(a);
                        myStringBuilder.Append('0', 15);
                    }


                    myStringBuilder.Append(a);
                    myStringBuilder.Append('0', 1);
                    myStringBuilder.Append(a);
                    myStringBuilder.Append(' ', 20);
                    myStringBuilder.Append(a);
                    myStringBuilder.Append('0', 1);
                    myStringBuilder.Append(a);
                    myStringBuilder.Append(' ', 7);

                    if (DocuemntsReferenceDate != null)
                    {

                        myStringBuilder.Append(a + DocuemntsReferenceDate);
                    }
                    else
                    {
                        myStringBuilder.Append(a);
                        myStringBuilder.Append('0', 8);
                    }
                    myStringBuilder.Append(a);


                    if (c100.Length > 7) { c100 = c100.Substring(0, 7); }
                    myStringBuilder.Append(a + c100.PadLeft(7, '0'));
                    myStringBuilder.Append(a);
                    myStringBuilder.Append(' ', 60);

                    myStringBuilder.AppendLine();
                }

            }
            C100Code = "C100";
            C100Counter = a + "".PadLeft(9, '0');
            C100VatNumber = a + "".PadLeft(9, '0');
            C100DocumentType = a + "".PadLeft(3, '0');
            C100DocumentReference = a + "".PadLeft(20, ' ');
            C100DocumentCreateDate = a + "".PadLeft(8, '0');
            C100DocumentTime = a + "".PadLeft(4, '0');
            C100Name = a + "".PadLeft(50, ' ');
            C100Street = a + "".PadLeft(50, ' ');
            C100HouseNumber = a + "".PadLeft(10, ' ');
            C100City = a + "".PadLeft(30, ' ');
            C100Mikud = a + "".PadLeft(8, ' ');
            C100Country = a + "".PadLeft(30, ' ');
            C100CountryCode = a + "".PadLeft(2, ' ');
            C100Phone = a + "".PadLeft(15, ' ');
            C100VatClient = a + "".PadLeft(9, '0');
            C100ValueDate = a + "".PadLeft(8, '0');
            C100FinalSum = a + "+".PadRight(15, '0');
            C100Currency = a + "".PadLeft(3, ' ');
            C100SumBeforeDiscount = a + "".PadLeft(15, '0');
            C100Discount = a + "".PadLeft(15, ' ');
            C100SumAfterDiscount = a + "".PadLeft(15, '0');
            C100VatSum = a + "".PadLeft(15, ' ');
            C100SumIncludeVat = a + "".PadLeft(15, '0');
            C100NikuiBamakor = a + "".PadLeft(12, ' ');
            C100Key = a + "".PadLeft(15, ' ');
            C100Fit = a + "".PadLeft(10, ' ');
            C100Cancled = a + "".PadLeft(1, ' ');
            C100DocumentDate = a + "".PadLeft(8, '0');
            C100Branch = a + "".PadLeft(7, ' ');
            C100User = a + "".PadLeft(9, ' ');
            C100Mekasher = a + "".PadLeft(7, ' ');
            C100Future = a + "".PadLeft(13, ' ');
            //Deposit
            foreach (C100Data item in DepositC100)
            {
                C100Count++;
                counter++;
                if (counter.ToString().Length > 9)
                {
                    C100Counter = a + counter.ToString().Substring(0, 9).ToString().PadLeft(9, '0');
                }
                else
                {
                    C100Counter = a + counter.ToString().PadLeft(9, '0');
                }

                if (tenantPM.VatNumber != null)
                {
                    if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                    C100VatNumber = a + tenantPM.VatNumber.PadLeft(9, '0');
                }

                C100DocumentType = item.DocumentType;

                if (item.DocumentReference != null)
                {
                    if (item.DocumentReference.Length > 20) { item.DocumentReference = item.DocumentReference.Substring(0, 20); }
                    C100DocumentReference = a + item.DocumentReference.PadLeft(20, ' ');
                }

                var CreateDate = String.Format("{0:yyyyMMdd}", item.DocumentCreateDate);
                var CreateDateTime = item.DocumentCreateDate.Value.ToString("HHmm");

                if (CreateDate.Length > 8) { CreateDate.Substring(0, 8); }
                C100DocumentCreateDate = a + CreateDate.PadLeft(8, '0');
                if (CreateDateTime.Length > 4) { CreateDateTime.Substring(0, 4); }
                C100DocumentTime = a + CreateDateTime.PadLeft(4, '0');

                BankAccountPM bankAccountPM = bankAccountQueryService.GetSingle(item.CustomerVendorName, false, false);
                if (bankAccountPM != null)
                {
                    item.CustomerVendorName = bankAccountPM.LocalName != null ? bankAccountPM.LocalName : bankAccountPM.EnglishName;

                }
                else item.CustomerVendorName = null;

                if (item.CustomerVendorName != null)
                {
                    if (item.CustomerVendorName.Length > 50) { item.CustomerVendorName = item.CustomerVendorName.Substring(0, 50); }
                    C100Name = a + item.CustomerVendorName.PadLeft(50, ' ');
                }
                var ValueDate = String.Format("{0:yyyyMMdd}", item.ValueDate);
                if (ValueDate != null)
                {

                    C100ValueDate = a + ValueDate;
                }
                string DocumentAmountAndVATAmount = Format((decimal)item.DocumentAmountAndVATAmount); 

                if (DocumentAmountAndVATAmount != null)
                {
                    if (item.DocumentAmountAndVATAmount >= 0)
                    {
                        C100SumIncludeVat = a + "+";
                        if (DocumentAmountAndVATAmount.Length > 14) { DocumentAmountAndVATAmount = DocumentAmountAndVATAmount.Substring(0, 14); }
                        C100SumIncludeVat += DocumentAmountAndVATAmount.PadLeft(14, '0');
                    }

                    else if (item.DocumentAmountAndVATAmount < 0)
                    {
                        C100SumIncludeVat = a + "-";
                        if (DocumentAmountAndVATAmount.Length > 14) { DocumentAmountAndVATAmount = DocumentAmountAndVATAmount.Substring(0, 14); }
                        C100SumIncludeVat += DocumentAmountAndVATAmount.PadLeft(14, '0');
                    }
                }
                if (item.IsCancelled)
                {
                    C100Cancled = "1";
                }
                else
                {
                    C100Cancled = "0";
                }
                var DocuemntsReferenceDate = String.Format("{0:yyyyMMdd}", item.DocuemntsReferenceDate);
                if (DocuemntsReferenceDate != null)
                {

                    C100DocumentDate = a + DocuemntsReferenceDate;
                }

                if (item.CreatedbyUser != null)
                {
                    if (item.CreatedbyUser.Length > 9) { item.CreatedbyUser = item.CreatedbyUser.Substring(0, 9); }
                    C100User = a + item.CreatedbyUser.PadLeft(9, ' ');
                }
                C100SumBeforeDiscount = C100SumIncludeVat;
                C100SumAfterDiscount = C100SumIncludeVat;

                if (item.CustomerVendorCode != null)
                {
                    if (item.CustomerVendorCode.Length > 15) { item.CustomerVendorCode = item.CustomerVendorCode.Substring(0, 15); }
                    C100Key = a + item.CustomerVendorCode.PadLeft(15, ' ');
                }

                string c100 = C100Count.ToString();
                if (c100.Length > 7) { c100 = c100.Substring(0, 7); }
                C100Mekasher = a + c100.PadLeft(7, '0');
                object[] variables = new object[]{
                   C100Code,C100Counter,C100VatNumber,C100DocumentType,C100DocumentReference,
                   C100DocumentCreateDate,C100DocumentTime,C100Name,C100Street,C100HouseNumber,
                   C100City,C100Mikud,C100Country,C100CountryCode,C100Phone,C100VatClient,
                   C100ValueDate,C100FinalSum,C100Currency,C100SumBeforeDiscount,C100Discount,
                   C100SumAfterDiscount,C100VatSum,C100SumIncludeVat,C100NikuiBamakor,C100Key,
                   C100Fit,C100Cancled,C100DocumentDate,C100Branch,C100User,C100Mekasher,C100Future
                };
                AddSBline(ref myStringBuilder, variables);
                //D120

                if (item.CashBookType == "1")
                {
                    counter++;
                    D120Count++;
                    myStringBuilder.Append("D120");


                    if (counter.ToString().Length > 9)
                    {
                        counter.ToString().Substring(0, 9);
                        myStringBuilder.Append(a + counter.ToString().PadLeft(9, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append(counter.ToString().PadLeft(9, '0'));
                    }

                    if (tenantPM.VatNumber != null)
                    {
                        if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                        myStringBuilder.Append(a + tenantPM.VatNumber.PadLeft(9, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append(a);
                        myStringBuilder.Append('0', 9);
                    }

                    myStringBuilder.Append("420");

                    if (item.DocumentReference != null)
                    {
                        if (item.DocumentReference.Length > 20) { item.DocumentReference = item.DocumentReference.Substring(0, 20); }
                        myStringBuilder.Append(a + item.DocumentReference.PadLeft(20, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append(a);
                        myStringBuilder.Append(' ', 20);
                    }
                    myStringBuilder.Append(a);
                    myStringBuilder.Append("0001");


                    if (item.CashBookType != null)
                    {
                        if (item.CashBookType.Length > 1) { item.CashBookType = item.CashBookType.Substring(0, 1); }
                        myStringBuilder.Append(a + item.CashBookType);
                    }
                    else
                    {
                        myStringBuilder.Append(a);
                        myStringBuilder.Append('0', 1);
                    }




                    myStringBuilder.Append(a);
                    myStringBuilder.Append('0', 10);
                    myStringBuilder.Append(a);
                    myStringBuilder.Append('0', 10);
                    myStringBuilder.Append(a);
                    myStringBuilder.Append('0', 15);
                    myStringBuilder.Append(a);
                    myStringBuilder.Append('0', 10);
                    myStringBuilder.Append(a);
                    myStringBuilder.Append('0', 8);

                    string localAmount = Format((decimal)item.DocumentAmountAndVATAmount);



                    if (localAmount != null)
                    {
                        if (item.DocumentAmountAndVATAmount > 0)
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append("+");
                            if (localAmount.Length > 14) { localAmount = localAmount.Substring(0, 14); }
                            myStringBuilder.Append(a + localAmount.PadLeft(14, '0'));
                        }

                        else if (item.DocumentAmountAndVATAmount < 0)
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append("-");
                            if (localAmount.Length > 14) { localAmount = localAmount.Substring(0, 14); }
                            myStringBuilder.Append(a + localAmount.PadLeft(14, '0'));
                        }
                        else
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append('0', 15);
                        }
                    }
                    else
                    {
                        myStringBuilder.Append(a);
                        myStringBuilder.Append('0', 15);
                    }




                    myStringBuilder.Append(a);
                    myStringBuilder.Append('0');


                    myStringBuilder.Append(a);
                    myStringBuilder.Append(' ', 20);
                    myStringBuilder.Append(a);
                    myStringBuilder.Append('0');
                    myStringBuilder.Append(a);
                    myStringBuilder.Append(' ', 7);

                    if (DocuemntsReferenceDate != null)
                    {

                        myStringBuilder.Append(a + DocuemntsReferenceDate);
                    }
                    else
                    {
                        myStringBuilder.Append(a);
                        myStringBuilder.Append('0', 8);
                    }

                    myStringBuilder.Append(a);


                    if (c100.Length > 7) { c100 = c100.Substring(0, 7); }
                    myStringBuilder.Append(a + c100.PadLeft(7, '0'));

                    myStringBuilder.Append(a);
                    myStringBuilder.Append(' ', 60);

                    myStringBuilder.AppendLine();
                }

                else
                {
                    List<BankDepositLinePM> lines = depositLines.Where(d => d.DepositId == item.DepositId).ToList();


                    foreach (BankDepositLinePM line in lines)
                    {
                        counter++;
                        D120Count++;
                        myStringBuilder.Append("D120");


                        if (counter.ToString().Length > 9)
                        {
                            counter.ToString().Substring(0, 9);
                            myStringBuilder.Append(a + counter.ToString().PadLeft(9, '0'));
                        }
                        else
                        {
                            myStringBuilder.Append(counter.ToString().PadLeft(9, '0'));
                        }

                        if (tenantPM.VatNumber != null)
                        {
                            if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                            myStringBuilder.Append(a + tenantPM.VatNumber.PadLeft(9, '0'));
                        }
                        else
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append('0', 9);
                        }

                        myStringBuilder.Append("420");

                        if (item.DocumentReference != null)
                        {
                            if (item.DocumentReference.Length > 20) { item.DocumentReference = item.DocumentReference.Substring(0, 20); }
                            myStringBuilder.Append(a + item.DocumentReference.PadLeft(20, ' '));
                        }
                        else
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append(' ', 20);
                        }
                        myStringBuilder.Append(a);
                        myStringBuilder.Append(line.Line.ToString().PadLeft(4, '0'));


                        if (item.CashBookType != null)
                        {
                            if (item.CashBookType.Length > 1) { item.CashBookType = item.CashBookType.Substring(0, 1); }
                            myStringBuilder.Append(a + item.CashBookType);
                        }
                        else
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append('0', 1);
                        }


                        if (item.CashBookType == "2")
                        {
                            string bank = line.Bank;
                            if (bank != null)
                            {
                                if (bank.Length > 10)
                                {
                                    bank = bank.Substring(0, 10);
                                }
                                myStringBuilder.Append(a + bank.PadLeft(10, '0'));

                            }
                            else
                            {
                                myStringBuilder.Append(a);
                                myStringBuilder.Append('0', 10);
                            }
                            string branch = line.Branch;

                            if (branch != null)
                            {
                                if (branch.Length > 10)
                                {
                                    branch = branch.Substring(0, 10);
                                }
                                myStringBuilder.Append(a + branch.PadLeft(10, '0'));

                            }
                            else
                            {
                                myStringBuilder.Append(a);
                                myStringBuilder.Append('0', 10);
                            }
                            string accountNumber = line.AccountNumber;

                            if (accountNumber != null)
                            {
                                if (accountNumber.Length > 15)
                                {
                                    accountNumber = accountNumber.Substring(0, 15);
                                }
                                myStringBuilder.Append(a + accountNumber.PadLeft(15, '0'));

                            }
                            else
                            {
                                myStringBuilder.Append(a);
                                myStringBuilder.Append('0', 15);
                            }

                            string chequeNumber = line.ChequeNumber;
                            if (chequeNumber != null)
                            {
                                if (chequeNumber.Length > 10)
                                {
                                    chequeNumber = chequeNumber.Substring(0, 10);
                                }
                                myStringBuilder.Append(a + chequeNumber.PadLeft(10, '0'));

                            }
                            else
                            {
                                myStringBuilder.Append(a);
                                myStringBuilder.Append('0', 10);
                            }

                            var DueDate = String.Format("{0:yyyyMMdd}", line.DueDate);
                            if (DueDate != null)
                            {
                                myStringBuilder.Append(a + DueDate);

                            }
                            else
                            {
                                myStringBuilder.Append('0', 8);
                            }




                            string Localamount = Format((decimal)line.LocalAmount);  

                            if (Localamount != null)
                            {
                                if (line.LocalAmount > 0)
                                {
                                    myStringBuilder.Append(a);
                                    myStringBuilder.Append("+");
                                    if (Localamount.Length > 14) { Localamount = Localamount.Substring(0, 14); }
                                    myStringBuilder.Append(a + Localamount.PadLeft(14, '0'));
                                }

                                else if (line.LocalAmount < 0)
                                {
                                    myStringBuilder.Append(a);
                                    myStringBuilder.Append("-");
                                    if (Localamount.Length > 14) { Localamount = Localamount.Substring(0, 14); }
                                    myStringBuilder.Append(a + Localamount.PadLeft(14, '0'));
                                }
                                else
                                {
                                    myStringBuilder.Append(a);
                                    myStringBuilder.Append('0', 15);
                                }
                            }
                            else
                            {
                                myStringBuilder.Append(a);
                                myStringBuilder.Append('0', 15);
                            }




                        }
                        else
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append('0', 10);
                            myStringBuilder.Append(a);
                            myStringBuilder.Append('0', 10);
                            myStringBuilder.Append(a);
                            myStringBuilder.Append('0', 15);
                            myStringBuilder.Append(a);
                            myStringBuilder.Append('0', 10);
                            myStringBuilder.Append(a);
                            myStringBuilder.Append('0', 8);


                            string Localamount = Format((decimal)item.DocumentAmountAndVATAmount);  

                            if (Localamount != null)
                            {
                                if (item.DocumentAmountAndVATAmount > 0)
                                {
                                    myStringBuilder.Append(a);
                                    myStringBuilder.Append("+");
                                    if (Localamount.Length > 14) { Localamount = Localamount.Substring(0, 14); }
                                    myStringBuilder.Append(a + Localamount.PadLeft(14, '0'));
                                }

                                else if (item.DocumentAmountAndVATAmount < 0)
                                {
                                    myStringBuilder.Append(a);
                                    myStringBuilder.Append("-");
                                    if (Localamount.Length > 14) { Localamount = Localamount.Substring(0, 14); }
                                    myStringBuilder.Append(a + Localamount.PadLeft(14, '0'));
                                }
                                else
                                {
                                    myStringBuilder.Append(a);
                                    myStringBuilder.Append('0', 15);
                                }
                            }
                            else
                            {
                                myStringBuilder.Append(a);
                                myStringBuilder.Append('0', 15);
                            }


                        }

                        myStringBuilder.Append(a);
                        myStringBuilder.Append('0');


                        myStringBuilder.Append(a);
                        myStringBuilder.Append(' ', 20);
                        myStringBuilder.Append(a);
                        myStringBuilder.Append('0');
                        myStringBuilder.Append(a);
                        myStringBuilder.Append(' ', 7);

                        if (DocuemntsReferenceDate != null)
                        {

                            myStringBuilder.Append(a + DocuemntsReferenceDate);
                        }
                        else
                        {
                            myStringBuilder.Append(a);
                            myStringBuilder.Append('0', 8);
                        }

                        myStringBuilder.Append(a);


                        if (c100.Length > 7) { c100 = c100.Substring(0, 7); }
                        myStringBuilder.Append(a + c100.PadLeft(7, '0'));

                        myStringBuilder.Append(a);
                        myStringBuilder.Append(' ', 60);

                        myStringBuilder.AppendLine();
                    }
                }

            }

            //Z900

            myStringBuilder.Append("Z900");

            counter++;
            rowsCount = counter;

            if (counter.ToString().Length > 9)
            {
                counter.ToString().Substring(0, 9);
                myStringBuilder.Append(a + counter.ToString().PadLeft(9, '0'));
            }
            else
            {
                myStringBuilder.Append(counter.ToString().PadLeft(9, '0'));
            }

            if (tenantPM.VatNumber != null)
            {
                if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                myStringBuilder.Append(a + tenantPM.VatNumber.PadLeft(9, '0'));
            }
            else
            {
                myStringBuilder.Append(a);
                myStringBuilder.Append('0', 9);
            }


            if (random.Length > 15) { random = random.Substring(0, 15); }
            myStringBuilder.Append(a + random.PadLeft(15, '0'));

            myStringBuilder.Append("&OF1.31&");

            if (counter.ToString().Length > 15)
            {
                counter.ToString().Substring(0, 15);
                myStringBuilder.Append(a + counter.ToString().PadLeft(15, '0'));
            }
            else
            {
                myStringBuilder.Append(counter.ToString().PadLeft(15, '0'));
            }

            myStringBuilder.Append(' ', 50);


            ARinvoiceTotalAmount = ARC100.Where(d => d.DocumentType == "305").Sum(D => (decimal)D.TotalDocumentsAmountAfterDiscount);
            CreditARinvoiceTotalAmount = ARC100.Where(d => d.DocumentType == "330").Sum(d => (decimal)d.TotalDocumentsAmountAfterDiscount);
            ARpaymentTotalAmount = ARPAymentC100.Sum(d => (decimal)d.DocumentAmountAndVATAmount);
            DepositTotalAmount = DepositC100.Sum(d => (decimal)d.DocumentAmountAndVATAmount);
            APinvoiceTotalAmount = APC100.Sum(d => (decimal)d.TotalDocumentsAmountAfterDiscount);
            ARinvoiceTotalRecords = ARC100.Where(d => d.DocumentType == "305").Count();
            CreditARinvoiceTotalRecords = ARC100.Where(d => d.DocumentType == "330").Count();
            ARpaymentTotalRecords = ARPAymentC100.Count();
            DepositTotalRecords = DepositC100.Count();
            APinvoiceTotalRecords = APC100.Count();




            DocumentsFilingPM docOut = CreateDocumnetFiling(myStringBuilder, openFormatReportPM);

            return docOut;





        }

        private List<CardList> GetCardsForAccountIdsInMaxAllowedListRange(List<string> accountIds, int startIndex, int maxAllowedAccountsCount)
        {
            return cardQuery.GetCardsByGLAccountIds(accountIds.GetRange(startIndex, Math.Min(maxAllowedAccountsCount, accountIds.Count - startIndex)), Tenant);
        }

        private List<CardList> GetCardsForMoreThan3000Accounts(List<string> accountIds, int tenant)
        {
            List<CardList> cards = new List<CardList>();
            int maxAllowedAccountsCount = 3000;
            for (int i = 0; i < accountIds.Count; i += maxAllowedAccountsCount)
            {
                cards = cards.Concat(GetCardsForAccountIdsInMaxAllowedListRange(accountIds, i, maxAllowedAccountsCount)).ToList();
            }
            return cards;
        }
        private List<CardList> GetCardsByGLAccountIds(List<string> accountIds, int tenant)
        {
            List<CardList> cards = new List<CardList>();
            if (accountIds.Count > 3000)
            {
                cards = GetCardsForMoreThan3000Accounts(accountIds, tenant);
            }
            else
            {
                cards = cardQuery.GetCardsByGLAccountIds(accountIds, tenant);
            }
            return cards;
        }
        private List<string> GetIncludedGLAccounts(List<TrailReportM> result, OpenFormatReportPM openFormatReport, int tenant)
        {
            var zeroVlauesList = result.Where(d => d.LocalOpenBalance == 0 && d.LocalDebit == 0 && d.LocalCredit == 0).ToList();
            List<string> zeroVlauesGLAccounts = new List<string>();
            if (zeroVlauesList != null)
            {
                zeroVlauesGLAccounts = zeroVlauesList.Select(d => d.GLAccountId).ToList();
            }

            List<string> exceptedGLAccounts = new List<string>();
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
            exceptedGLAccounts = gLAccountQueryService.GetGLAccountsWithoutLedgerTransactions(zeroVlauesGLAccounts, openFormatReport, tenant);
            var exceptedGLAccountsSet = new HashSet<string>(exceptedGLAccounts);  
            result = result.Where(d => !exceptedGLAccountsSet.Contains(d.GLAccountId)).ToList();
            List<string> includedGLAccounts = new List<string>();
            if (includedGLAccounts != null)
            {
                includedGLAccounts = result.Select(d => d.GLAccountId).Distinct().ToList();
            }

            return includedGLAccounts;
        }
        public BatchTaskExecutionPM CreateBKMVDATAFileInBatch(string taxReportId, int tenant)
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
                    Subject = "Create a flat file for Open Format Report",
                    Tenant = tenant,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    ClassName = "Logitude.Accounting.BL.CoreBL.Batch.BatchOpenFormatReportService,Logitude.Accounting.BL",
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


        private DocumentsFilingPM CreateDocumnetFiling(StringBuilder lines, OpenFormatReportPM openFormatReport, bool isFromWR = false)
        {
            // prepare file string
            string file = string.Join(Environment.NewLine, lines);


            // create document
            int tenant = openFormatReport.Tenant;
            ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
            DocumentsFilingService docService = new DocumentsFilingService(MyContext, tenant);
            DocumentTypeRepository docTypeReposioty = new DocumentTypeRepository(MyContext);

            ObjectTableRepository tableRep = new ObjectTableRepository(tenant);
            ObjectTable table = tableRep.GetObjectTableByName("OpenFormatReport", 0, true);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);

            // user
            ContactQuery contactQuery = new ContactQuery(tenant);

            ContactPM contact = contactQuery.GetSinglePM(openFormatReport.CreatedByUserId, tenant);
            if (contact == null)
            {
                contact = contactQuery.GetSinglePM(openFormatReport.CreatedByUserId, 0);
            }

            DocumentType docType = docTypeReposioty.GetSingleDocumentTypeByCode("BKMV", tenant);



            string error = TranslateTextsClass.Translate("Accounting.O.DocumentTypeNotFound", tenant, !contact.DontShowLocal);

            if (error != null)
            {
                error = error.Replace("X", "BKMV");
            }
            if (docType == null)
            {
                throw new ApplicationException(error);
            }

            string _code = CodeCounter.GetNumber("DocumentsFiling", tenant).ToString();
            DocumentsFilingPM document = new DocumentsFilingPM()
            {
                Description = "BKMVDATA Text File",
                DocumentTypeId = docType.Id,
                Tenant = tenant,
                DirectionCode = "I",
                EntityId = openFormatReport.Id,
                EntityNumber = openFormatReport.ReportNumber != null ? openFormatReport.ReportNumber.ToString() : null,
                ObjectTableId = table.Id,
                Code = _code,
                CreatedByUserId = contact.Id,
                OwnerId = contact.Id,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                UpdatedByUserId = contact.Id,
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                FileExtension = "txt",
                SecurityId = "100",
                FileName = "BKMVDATA",
            };




            byte[] bytearray = Encoding.GetEncoding("Windows-1255").GetBytes(file);

            document.FileData = bytearray;



            docService.Create(document, document.FileData, contact.Id);


            //get document out
            DocumentsFilingQuery queryService = new DocumentsFilingQuery(tenant);
            string code = (Convert.ToInt32(_code) + 1).ToString();
            DocumentsFilingPM docFiling = queryService.GetDocumentsFilingByDocumentCode(code, tenant);


            return docFiling;
        }




        public List<C100Data> GetARInvoiceC100Data(OpenFormatReportPM openFormatReportPM, int tenant)
        {
            IInvoiceContext invoiceContext = InvoiceContext.GetContext(tenant);
            List<C100Data> c100s = (from a in invoiceContext.ARInvoices
                                    where (a.InvoiceDate >= openFormatReportPM.FromDate && a.InvoiceDate <= openFormatReportPM.ToDate) && a.Tenant == tenant
                                    select new C100Data()
                                    {
                                        ARInvoiceId = a.Id,
                                        DocumentType = a.ARInvoiceType.Code != "CD" ? "305" : "330",
                                        DocumentReference = a.InvoiceNumber,
                                        DocumentCreateDate = a.CreateDate,
                                        CustomerVendorName = a.BillTo.LocalName != null ? a.BillTo.LocalName : a.BillTo.EnglishName,
                                        AddressStreet = a.BillToAddress != null ? a.BillToAddress.Address1 : null,
                                        AddressCity = a.BillToAddress != null ? a.BillToAddress.City : null,
                                        AddressZIPCode = a.BillToAddress != null ? a.BillToAddress.ZipCode : null,
                                        AddressCountry = a.BillToAddress.Country.LocalName != null ? a.BillToAddress.Country.LocalName : a.BillToAddress.Country.EnglishName,
                                        AddressCountryCode = a.BillToAddress != null ? a.BillToAddress.Country.Code : null,
                                        CustomeVendorTelephone = a.BillToAddress != null ? a.BillToAddress.PhoneNumber : null,
                                        CustomerVendorVatNumber = a.BillTo != null ? a.BillTo.VatNumber : null,
                                        ValueDate = a.DueDate,
                                        TotalDocumentsAmountBeforeDiscount = a.SubTotalInLocalCurrency,
                                        TotalDocumentsAmountAfterDiscount = a.SubTotalInLocalCurrency,
                                        DocumentAmountAndVATAmount = a.AmountInLocalCurrency,
                                        DocuemntsReferenceDate = a.InvoiceDate,
                                        CreatedbyUser = a.CreatedByUser.Code != null ? a.CreatedByUser.Code : a.CreatedByUser.Contact.EnglishName,
                                        GLAccountId = a.BillTo.GLAccountId,
                                        IsCancelled = a.IsCancelled,


                                    }).ToList();


            return c100s;
        }


        public List<C100Data> GetAPInvoiceC100Data(OpenFormatReportPM openFormatReportPM, int tenant)
        {
            IInvoiceContext invoiceContext = InvoiceContext.GetContext(tenant);
            List<C100Data> c100s = (from a in invoiceContext.APInvoices
                                    where ((a.InvoiceDate >= openFormatReportPM.FromDate && a.InvoiceDate <= openFormatReportPM.ToDate) || (a.AccountingDate >= openFormatReportPM.FromDate && a.AccountingDate <= openFormatReportPM.ToDate)) && a.Tenant == tenant
                                    select new C100Data()
                                    {
                                        APInvoiceId = a.Id,
                                        DocumentType = "700",
                                        DocumentReference = a.InvoiceNumber,
                                        DocumentCreateDate = a.CreateDate,
                                        CustomerVendorName = a.VendorCard.LocalName != null ? a.VendorCard.LocalName : a.VendorCard.EnglishName,
                                        CustomerVendorVatNumber = a.VendorCard != null ? a.VendorCard.VatNumber : null,
                                        ValueDate = a.DueDate,
                                        TotalDocumentsAmountBeforeDiscount = a.SubTotalInLocalCurrency,
                                        TotalDocumentsAmountAfterDiscount = a.SubTotalInLocalCurrency,
                                        DocumentAmountAndVATAmount = a.AmountInLocalCurrency,
                                        DocuemntsReferenceDate = a.InvoiceDate,
                                        CreatedbyUser = a.CreatedByUser.Code != null ? a.CreatedByUser.Code : a.CreatedByUser.Contact.EnglishName,
                                        GLAccountId = a.VendorCard.GLAccountId,
                                        IsCancelled = a.StatusCode == "VD" ? true : false,
                                        VendorId = a.VendorId,


                                    }).ToList();


            return c100s;
        }

        public List<C100Data> GetARPaymentC100Data(OpenFormatReportPM openFormatReportPM, int tenant)
        {
            IInvoiceContext invoiceContext = InvoiceContext.GetContext(tenant);
            List<C100Data> c100s = (from a in invoiceContext.ARPayments
                                    where ((a.RegisterDate >= openFormatReportPM.FromDate && a.RegisterDate <= openFormatReportPM.ToDate) || (a.RegisterDate >= openFormatReportPM.FromDate && a.RegisterDate <= openFormatReportPM.ToDate)) && a.Tenant == tenant
                                    select new C100Data()
                                    {
                                        ARPaymentId = a.Id,
                                        DocumentType = "400",
                                        DocumentReference = a.PaymentNo,
                                        DocumentCreateDate = a.CreateDate,
                                        CustomerVendorName = a.BillToCard.LocalName != null ? a.BillToCard.LocalName : a.BillToCard.EnglishName,
                                        CustomerVendorVatNumber = a.BillToCard != null ? a.BillToCard.VatNumber : null,
                                        ValueDate = a.ValueDate,
                                        TotalDocumentsAmountBeforeDiscount = null,
                                        TotalDocumentsAmountAfterDiscount = null,
                                        DocumentAmountAndVATAmount = a.AmountInLocalCurrency,
                                        DocuemntsReferenceDate = a.RegisterDate,
                                        CreatedbyUser = a.CreatedByUser.Code != null ? a.CreatedByUser.Code : a.CreatedByUser.Contact.EnglishName,
                                        GLAccountId = a.BillToCard.GLAccountId,
                                        IsCancelled = a.StatusCode == "VD" ? true : false,
                                        VendorId = a.BillToId,
                                        ARPaymentMethod = a.AccountingPaymentMethod != null ? a.AccountingPaymentMethod.Name : null,

                                    }).ToList();


            return c100s;
        }

        public List<C100Data> GetDepositC100Data(OpenFormatReportPM openFormatReportPM, int tenant)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            List<C100Data> c100s;

            if (openFormatReportPM.FromDate == openFormatReportPM.ToDate)
            {
                openFormatReportPM.ToDate = openFormatReportPM.ToDate.AddHours(23).AddMinutes(59).AddSeconds(59);

            }

            c100s = (from a in accountingContext.BankDeposits

                     join baTmp in accountingContext.BankAccounts
                         on a.DepositBankAccountId equals baTmp.Id into baGroup
                     from ba in baGroup.DefaultIfEmpty()

                     join glAccountTmp in accountingContext.GLAccounts
                        on ba.GLAccountId equals glAccountTmp.Id into glAccountGroup
                     from glAccount in glAccountGroup.DefaultIfEmpty()

                     where (a.AccountingDate >= openFormatReportPM.FromDate && a.AccountingDate <= openFormatReportPM.ToDate) && a.Tenant == tenant
                     select new C100Data()
                     {

                         DocumentType = "420",
                         DocumentReference = a.DepositNumber.ToString(),
                         DocumentCreateDate = a.CreateDate,
                         CustomerVendorName = a.DepositBankAccountId,
                         CustomerVendorCode = glAccount.Id != null ? glAccount.DisplayNumber : null,
                         CustomerVendorVatNumber = null,
                         ValueDate = a.AccountingDate,
                         TotalDocumentsAmountBeforeDiscount = null,
                         TotalDocumentsAmountAfterDiscount = null,
                         DocumentAmountAndVATAmount = (double)a.LocalDepositAmount,
                         DocuemntsReferenceDate = a.AccountingDate,
                         CreatedbyUser = a.CreatedByUser.Code != null ? a.CreatedByUser.Code : a.CreatedByUser.Contact.EnglishName,
                         GLAccountId = null,
                         IsCancelled = a.IsCanceled,
                         VendorId = null,
                         DepositId = a.Id,
                         CashBookType = a.CashBook.CashBookTypeCode,

                     }).ToList();




            return c100s;
        }


        public DocumentsFilingPM CreateINIFile(string openFormatReportId, int tenant)
        {
            OpenFormatReportQueryService openFormatReportQueryService = new OpenFormatReportQueryService(tenant);
            OpenFormatReportPM openFormatReportPM = openFormatReportQueryService.GetSingle(openFormatReportId, false, false);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);
            FullAccountingSettingQueryService fullAccountingSettingQueryService = new FullAccountingSettingQueryService(tenant);
            FullAccountingSettingPM setting = fullAccountingSettingQueryService.GetSingleFullAccountingSetting(tenant);
            FullAccountingSettingPM tenant0settings = fullAccountingSettingQueryService.GetSingleFullAccountingSetting(0);

            AddressQuery addressQuery = new AddressQuery(tenant);
            AddressPM address = addressQuery.GetSingleAddressPM(tenantPM.AddressId, tenant);


            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("A000");
            stringBuilder.Append(' ', 5);

            if (rowsCount.ToString().Length > 15)
            {
                rowsCount.ToString().Substring(0, 15);
                stringBuilder.Append(a + rowsCount.ToString().PadLeft(15, '0'));
            }
            else
            {
                stringBuilder.Append(rowsCount.ToString().PadLeft(15, '0'));
            }
            if (tenantPM.VatNumber != null)
            {
                if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                stringBuilder.Append(a + tenantPM.VatNumber.PadLeft(9, '0'));
            }
            else
            {
                stringBuilder.Append(a);
                stringBuilder.Append('0', 9);
            }

            if (random.Length > 15) { random = random.Substring(0, 15); }
            stringBuilder.Append(a + random.PadLeft(15, '0'));
            stringBuilder.Append("&OF1.31&");
            stringBuilder.Append("00074406");
            stringBuilder.Append(' ', 6);
            stringBuilder.Append("Unifreight Acc");


            if (tenant0settings != null)
            {
                if (tenant0settings.SoftwareVersion != null)
                {
                    if (tenant0settings.SoftwareVersion.Length > 20) { tenant0settings.SoftwareVersion = tenant0settings.SoftwareVersion.Substring(0, 20); }
                    stringBuilder.Append(a + tenant0settings.SoftwareVersion.PadLeft(20, ' '));
                }
                else
                {
                    stringBuilder.Append(a);
                    stringBuilder.Append(' ', 20);
                }
            }
            else
            {
                stringBuilder.Append(a);
                stringBuilder.Append(' ', 20);
            }

            stringBuilder.Append("511262073");
            stringBuilder.Append(' ', 5);
            stringBuilder.Append("Amital Data LTD");
            stringBuilder.Append("2");

            var date = DateTime.Now;
            string dateFormat = String.Format("{0:MMddhhmm}", date);

            string filePath = @"D:\OPENFRMT\" + tenantPM.VatNumber + "." + date.Year.ToString().Substring(2, 2) + @"\" + dateFormat;

            if (filePath.ToString().Length > 50)
            {
                filePath.ToString().Substring(0, 50);
                stringBuilder.Append(a + filePath.ToString().PadLeft(50, ' '));
            }
            else
            {
                stringBuilder.Append(a + filePath.ToString().PadLeft(50, ' '));
            }

            stringBuilder.Append("2");
            stringBuilder.Append("2");
            if (tenantPM.VatNumber != null)
            {
                if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                stringBuilder.Append(a + tenantPM.VatNumber.PadLeft(9, '0'));
            }
            else
            {
                stringBuilder.Append(a);
                stringBuilder.Append('0', 9);
            }


            if (setting != null)
            {
                if (setting.DeductionFileNumber != null)
                {
                    if (setting.DeductionFileNumber.Length > 9) { setting.DeductionFileNumber = setting.DeductionFileNumber.Substring(0, 9); }
                    stringBuilder.Append(a + setting.DeductionFileNumber.PadLeft(9, '0'));
                }
                else
                {
                    stringBuilder.Append(a);
                    stringBuilder.Append('0', 9);
                }
            }
            else
            {
                stringBuilder.Append(a);
                stringBuilder.Append('0', 9);
            }
            stringBuilder.Append('0', 10);
            if (tenantPM.Company != null)
            {
                if (tenantPM.Company.Length > 50) { tenantPM.Company = tenantPM.Company.Substring(0, 50); }
                stringBuilder.Append(a + tenantPM.Company.PadLeft(50, ' '));
            }
            else
            {
                stringBuilder.Append(a);
                stringBuilder.Append(' ', 50);
            }

            if (address != null && address.Address1 != null)
            {
                if (address.Address1.Length > 50) { address.Address1 = address.Address1.Substring(0, 50); }
                stringBuilder.Append(a + address.Address1.PadLeft(50, ' '));
            }
            else
            {
                stringBuilder.Append(a);
                stringBuilder.Append(' ', 50);
            }
            stringBuilder.Append(a);
            stringBuilder.Append(' ', 10);
            if (address != null)
            {
                if (address.City != null)
                {
                    if (address.City.Length > 30) { address.City = address.City.Substring(0, 30); }
                    stringBuilder.Append(a + address.City.PadLeft(30, ' '));
                }

                else
                {
                    stringBuilder.Append(a);
                    stringBuilder.Append(' ', 30);
                }

                if (address.ZipCode != null)
                {
                    if (address.ZipCode.Length > 8) { address.ZipCode = address.ZipCode.Substring(0, 8); }
                    stringBuilder.Append(a + address.ZipCode.PadLeft(8, ' '));
                }

                else
                {
                    stringBuilder.Append(a);
                    stringBuilder.Append(' ', 8);
                }
            }
            else
            {
                stringBuilder.Append(a);
                stringBuilder.Append(' ', 30);
                stringBuilder.Append(a);
                stringBuilder.Append(' ', 8);
            }
            stringBuilder.Append(a);
            stringBuilder.Append(' ', 4);
            var fromDate = String.Format("{0:yyyyMMdd}", openFormatReportPM.FromDate);
            var toDate = String.Format("{0:yyyyMMdd}", openFormatReportPM.ToDate);
            dateFormat = String.Format("{0:yyyyMMdd}", date);

            stringBuilder.Append(fromDate);
            stringBuilder.Append(toDate);
            stringBuilder.Append(dateFormat);
            var CurrenteDateTime = date.ToString("HHmm");
            stringBuilder.Append(CurrenteDateTime);
            stringBuilder.Append("0");
            stringBuilder.Append("1");
            stringBuilder.Append(' ', 14);
            stringBuilder.Append("WinZip");
            stringBuilder.Append("ILS");
            stringBuilder.Append("0");
            stringBuilder.Append("ריק");
            stringBuilder.Append(' ', 43);

            stringBuilder.AppendLine();
            stringBuilder.Append("A100");
            stringBuilder.Append('0', 14);
            stringBuilder.Append("1");


            stringBuilder.AppendLine();
            stringBuilder.Append("C100");
            if (C100Count.ToString().Length > 15)
            {
                C100Count.ToString().Substring(0, 15);
                stringBuilder.Append(a + C100Count.ToString().PadLeft(15, '0'));
            }
            else
            {
                stringBuilder.Append(a + C100Count.ToString().PadLeft(15, '0'));
            }
            stringBuilder.AppendLine();

            stringBuilder.Append("D110");
            if (D110Count.ToString().Length > 15)
            {
                D110Count.ToString().Substring(0, 15);
                stringBuilder.Append(a + D110Count.ToString().PadLeft(15, '0'));
            }
            else
            {
                stringBuilder.Append(a + D110Count.ToString().PadLeft(15, '0'));
            }

            stringBuilder.AppendLine();

            stringBuilder.Append("D120");
            if (D120Count.ToString().Length > 15)
            {
                D120Count.ToString().Substring(0, 15);
                stringBuilder.Append(a + D120Count.ToString().PadLeft(15, '0'));
            }
            else
            {
                stringBuilder.Append(a + D120Count.ToString().PadLeft(15, '0'));
            }
            stringBuilder.AppendLine();
            stringBuilder.Append("B100");
            if (B100Count.ToString().Length > 15)
            {
                B100Count.ToString().Substring(0, 15);
                stringBuilder.Append(a + B100Count.ToString().PadLeft(15, '0'));
            }
            else
            {
                stringBuilder.Append(a + B100Count.ToString().PadLeft(15, '0'));
            }
            stringBuilder.AppendLine();
            stringBuilder.Append("B110");
            if (B110Count.ToString().Length > 15)
            {
                B110Count.ToString().Substring(0, 15);
                stringBuilder.Append(a + B110Count.ToString().PadLeft(15, '0'));
            }
            else
            {
                stringBuilder.Append(a + B110Count.ToString().PadLeft(15, '0'));
            }
            stringBuilder.AppendLine();



            stringBuilder.Append("M100");
            stringBuilder.Append('0', 15);

            stringBuilder.AppendLine();

            stringBuilder.Append("Z900");
            stringBuilder.Append('0', 14);
            stringBuilder.Append("1");


            DocumentsFilingPM docOut = CreateINIDocumnetFiling(stringBuilder, openFormatReportPM);

            FillPDFReportXML(openFormatReportPM);


            return docOut;






        }
        private DocumentsFilingPM CreateINIDocumnetFiling(StringBuilder lines, OpenFormatReportPM openFormatReport, bool isFromWR = false)
        {
            // prepare file string
            string file = string.Join(Environment.NewLine, lines);

            // create document
            int tenant = openFormatReport.Tenant;
            ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
            DocumentsFilingService docService = new DocumentsFilingService(MyContext, tenant);
            DocumentTypeRepository docTypeReposioty = new DocumentTypeRepository(MyContext);

            ObjectTableRepository tableRep = new ObjectTableRepository(tenant);
            ObjectTable table = tableRep.GetObjectTableByName("OpenFormatReport", 0, true);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);

            // user
            ContactQuery contactQuery = new ContactQuery(tenant);

            ContactPM contact = contactQuery.GetSinglePM(openFormatReport.CreatedByUserId, tenant);
            if (contact == null)
            {
                contact = contactQuery.GetSinglePM(openFormatReport.CreatedByUserId, 0);
            }

            DocumentType docType = docTypeReposioty.GetSingleDocumentTypeByCode("INI", tenant);



            string error = TranslateTextsClass.Translate("Accounting.O.DocumentTypeNotFound", tenant, !contact.DontShowLocal);

            if (error != null)
            {
                error = error.Replace("X", "INI");
            }
            if (docType == null)
            {
                throw new ApplicationException(error);
            }

            string _code = CodeCounter.GetNumber("DocumentsFiling", tenant).ToString();
            DocumentsFilingPM document = new DocumentsFilingPM()
            {
                Description = "INI Text File",
                DocumentTypeId = docType.Id,
                Tenant = tenant,
                DirectionCode = "I",
                EntityId = openFormatReport.Id,
                EntityNumber = openFormatReport.ReportNumber != null ? openFormatReport.ReportNumber.ToString() : null,
                ObjectTableId = table.Id,
                Code = _code,
                CreatedByUserId = contact.Id,
                OwnerId = contact.Id,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                UpdatedByUserId = contact.Id,
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                FileExtension = "txt",
                SecurityId = "100",
                FileName = "INI",
            };

            byte[] bytearray = Encoding.GetEncoding("Windows-1255").GetBytes(file);
            document.FileData = bytearray;
            docService.Create(document, document.FileData, contact.Id);


            //get document out
            DocumentsFilingQuery queryService = new DocumentsFilingQuery(tenant);
            string code = (Convert.ToInt32(_code) + 1).ToString();
            DocumentsFilingPM docFiling = queryService.GetDocumentsFilingByDocumentCode(code, tenant);


            return docFiling;
        }

        public void FillPDFReportXML(OpenFormatReportPM openFormatReportPM)
        {
            OpenFormatReportDataProvider pDFRerportXMLData = new OpenFormatReportDataProvider();
            pDFRerportXMLData.OpenFormatTotalRecords = new List<OpenFormatTotalRecord>();




            pDFRerportXMLData.OpenFormatTotalRecords.Add(new OpenFormatTotalRecord()
            {
                RecordCode = "A100",
                RecordDescreption = "רשומה פתיחה",
                RecordTotal = 1,


            });

            if (C100Count > 0)
            {
                pDFRerportXMLData.OpenFormatTotalRecords.Add(new OpenFormatTotalRecord()
                {
                    RecordCode = "C100",
                    RecordDescreption = "כותרת מסמך",
                    RecordTotal = C100Count,


                });
            }

            if (D110Count > 0)
            {
                pDFRerportXMLData.OpenFormatTotalRecords.Add(new OpenFormatTotalRecord()
                {
                    RecordCode = "D110",
                    RecordDescreption = "פרטי מסמך",
                    RecordTotal = D110Count,


                });
            }




            if (D120Count > 0)
            {
                pDFRerportXMLData.OpenFormatTotalRecords.Add(new OpenFormatTotalRecord()
                {
                    RecordCode = "D120",
                    RecordDescreption = "פרטי קבלות",
                    RecordTotal = D120Count,


                });

            }


            if (B100Count > 0)
            {

                pDFRerportXMLData.OpenFormatTotalRecords.Add(new OpenFormatTotalRecord()
                {
                    RecordCode = "B100",
                    RecordDescreption = "תנועות בהנהלת חשבונות",
                    RecordTotal = B100Count,


                });
            }





            if (B110Count > 0)
            {
                pDFRerportXMLData.OpenFormatTotalRecords.Add(new OpenFormatTotalRecord()
                {
                    RecordCode = "B110",
                    RecordDescreption = "רשומה פתיחה",
                    RecordTotal = B110Count,


                });
            }


            pDFRerportXMLData.OpenFormatTotalRecords.Add(new OpenFormatTotalRecord()
            {
                RecordCode = "M100",
                RecordDescreption = "פריט במלאי",
                RecordTotal = 0,


            });



            pDFRerportXMLData.OpenFormatTotalRecords.Add(new OpenFormatTotalRecord()
            {
                RecordCode = "Z900",
                RecordDescreption = "רשומת סיום",
                RecordTotal = 1,


            });


            pDFRerportXMLData.OpenFormatTotalAmounts = new List<OpenFormatTotalAmounts>();


            if (ARinvoiceTotalRecords > 0)
            {
                pDFRerportXMLData.OpenFormatTotalAmounts.Add(new OpenFormatTotalAmounts()
                {
                    RecordCode = "305",
                    RecordType = "חשבונית -מס",
                    TotalRecords = ARinvoiceTotalRecords,
                    TotalAmount = ARinvoiceTotalAmount,

                });
            }




            if (CreditARinvoiceTotalRecords > 0)
            {
                pDFRerportXMLData.OpenFormatTotalAmounts.Add(new OpenFormatTotalAmounts()
                {
                    RecordCode = "330",
                    RecordType = "חשבונית מס זיכוי",
                    TotalRecords = CreditARinvoiceTotalRecords,
                    TotalAmount = CreditARinvoiceTotalAmount,

                });
            }



            if (ARpaymentTotalRecords > 0)
            {
                pDFRerportXMLData.OpenFormatTotalAmounts.Add(new OpenFormatTotalAmounts()
                {
                    RecordCode = "400",
                    RecordType = "קבלה",
                    TotalRecords = ARpaymentTotalRecords,
                    TotalAmount = ARpaymentTotalAmount,

                });
            }


            if (DepositTotalRecords > 0)
            {
                pDFRerportXMLData.OpenFormatTotalAmounts.Add(new OpenFormatTotalAmounts()
                {
                    RecordCode = "420",
                    RecordType = "הפקדת בנק",
                    TotalRecords = DepositTotalRecords,
                    TotalAmount = DepositTotalAmount,

                });
            }



            if (APinvoiceTotalRecords > 0)
            {
                pDFRerportXMLData.OpenFormatTotalAmounts.Add(new OpenFormatTotalAmounts()
                {
                    RecordCode = "700",
                    RecordType = "חשבונית מס רכש",
                    TotalRecords = APinvoiceTotalRecords,
                    TotalAmount = APinvoiceTotalAmount,

                });
            }

            pDFRerportXMLData.CompanyName = tenantPM.Company;
            pDFRerportXMLData.VatNumber = tenantPM.VatNumber;

            string dateFormat = String.Format("{0:MMddHHmm}", openFormatReportPM.CreateDate);
            string vatNumber = tenantPM.VatNumber != null ? tenantPM.VatNumber : "000000000";
            pDFRerportXMLData.Path = @"D:\OPENFRMT\" + tenantPM.VatNumber + "." + openFormatReportPM.CreateDate.Year.ToString().Substring(2, 2) + @"\" + dateFormat;


            openFormatReportPM.PDFRerportXML = LogitudeXmlSerializer.SerializeObjectToXmlString(pDFRerportXMLData);

            IAccountingContext accountingContext = AccountingContext.GetContext(openFormatReportPM.Tenant);
            OpenFormatReportUpdateService openFormatReportUpdateService = new OpenFormatReportUpdateService(accountingContext, new Dictionary<string, IContext>(), openFormatReportPM.Tenant);
            openFormatReportPM.StatusTypeCode = "3";
            openFormatReportPM.ChangeSetOp = ChangeSetOperation.Update;
            openFormatReportUpdateService.Update(openFormatReportPM, true);


        }


        public string Format(decimal value, bool quantity = false, bool isVat = false)
        {

            string formated = Math.Abs(value).ToString().Replace(".", string.Empty);
            string[] sub = value.ToString().Split('.');

            if (quantity)
            {
                formated = formated + "00";
            }
            if (sub.Count() == 1)
            {

                formated = formated + "00";
            }
            else if ((sub.Count() > 1) && isVat)
            {
                formated = "0" + formated;
            }
            else if (sub.Length > 1 && sub[1].Length == 1)
            {
                formated = formated + "0";
            }


            return formated;
        }
    }
}
