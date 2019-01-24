using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
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
using Simplog.Data.InvoiceModel;
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
   public class OpenFormatReportService
    {

        public static DocumentsFilingPM CreateBKMVDATAFile(string openFormatReportId, int tenant)
        {
            OpenFormatReportQueryService openFormatReportQueryService = new OpenFormatReportQueryService(tenant);
            OpenFormatReportPM openFormatReportPM = openFormatReportQueryService.GetSingle(openFormatReportId, false, false);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);
            LedgerTransactionQueryService ledgerTransactionQueryService = new LedgerTransactionQueryService(tenant);
            List<B100Data> b100Data = ledgerTransactionQueryService.GetTransactionsByDate( openFormatReportPM.FromDate, openFormatReportPM.ToDate, tenant);
            List<string> userIds = b100Data.Select(d => d.CreatedByUser).ToList();
            UserQuery userQuery = new UserQuery(tenant);
            CurrencyQuery currencyQuery = new CurrencyQuery(tenant);
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
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



            List<string> linesArray = new List<string>();



            StringBuilder myStringBuilder = new StringBuilder();

            // A100
            myStringBuilder.Append("A100");
            myStringBuilder.Append("a000000001");
         //   myStringBuilder.Append(tenantPM.VatNumber);
            if (tenantPM.VatNumber != null)
            {
                if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber= tenantPM.VatNumber.Substring(0, 9); }
                myStringBuilder.Append("a"+tenantPM.VatNumber.PadLeft(9, '0'));
            }
            else
            {
                myStringBuilder.Append('0', 9);
            }

            if (openFormatReportPM.ReportNumber != null)
            {
                if (openFormatReportPM.ReportNumber.Length > 15) { openFormatReportPM.ReportNumber.Substring(0, 15); }
                myStringBuilder.Append("a" + openFormatReportPM.ReportNumber.PadLeft(15, '0'));
            }

            else
            {
                myStringBuilder.Append('0', 15);
            }



            myStringBuilder.Append("a&OF1.31&");
            myStringBuilder.Append(' ', 50);
            myStringBuilder.Append('\n');
            //B100
            int counter = 1;
            foreach (B100Data item in b100Data)
            {
                counter++;
                myStringBuilder.Append("B100");
                if (counter.ToString().Length > 9)
                {
                    counter.ToString().Substring(0, 9);
                    myStringBuilder.Append( counter.ToString().PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append( counter.ToString().PadLeft(9, '0'));
                }

                if (tenantPM.VatNumber != null)
                {
                    if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber= tenantPM.VatNumber.Substring(0, 9); }
                    myStringBuilder.Append(tenantPM.VatNumber.PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append('0', 9);
                }
                if (item.JournalNumber != null)
                {
                    if (item.JournalNumber.Length > 10) { item.JournalNumber.Substring(0, 10); }
                    myStringBuilder.Append( item.JournalNumber.PadLeft(10, '0'));
                }
                else
                {
                    myStringBuilder.Append('0', 10);
                }

                if (item.JournalLineNumber.ToString().Length > 9) { item.JournalLineNumber.ToString().Substring(0, 5); }
                myStringBuilder.Append( item.JournalLineNumber.ToString().PadLeft(5, '0'));

                myStringBuilder.Append(' ', 8);
                myStringBuilder.Append(' ', 15);

                if (item.AccountingEntityReference != null)
                {
                    if (item.AccountingEntityReference.Length > 20) { item.AccountingEntityReference.Substring(0, 20); }
                    myStringBuilder.Append( item.AccountingEntityReference.PadLeft(20, ' '));
                }
                else
                {
                    myStringBuilder.Append(' ', 20);
                }

                if (computingPartnerTranslations != null)
                {
                    var entityPartnerCode = computingPartnerTranslations.Where(d => d.ObjectTableName == "AccountingEntity" && d.OurCode == item.AccountingEntityCode).FirstOrDefault();

                    if (entityPartnerCode != null)
                    {
                        if (entityPartnerCode.PartnerCode.Length > 3) { entityPartnerCode.PartnerCode = entityPartnerCode.PartnerCode.Substring(0, 3); }
                        myStringBuilder.Append(entityPartnerCode.PartnerCode.PadLeft(3, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append("000");

                    }

                }
                else
                {
                    myStringBuilder.Append("000");

                }


                if (item.Reference2 != null)
                {
                    if (item.Reference2.Length > 20) { item.Reference2.Substring(0, 20); }
                    myStringBuilder.Append(item.Reference2.PadLeft(20, ' '));
                }
                else
                {
                    myStringBuilder.Append(' ', 20);
                }

                myStringBuilder.Append("000");
                if (item.Notes != null)
                {
                    if (item.Notes.Length > 50) { item.Notes= item.Notes.Substring(0, 50); }
                    myStringBuilder.Append(item.Notes.PadLeft(50, ' '));
                }
                else
                {
                    myStringBuilder.Append(' ', 50);
                }

                var DocumentDate = String.Format("{0:yyyyMMdd}", item.DocumentDate);
                var AccountingDate = String.Format("{0:yyyyMMdd}", item.AccountingDate);
                var CreateDate = String.Format("{0:yyyyMMdd}", item.CreateDate);

                if (AccountingDate.Length > 8) { AccountingDate.Substring(0, 8); }
                myStringBuilder.Append( AccountingDate.PadLeft(8, '0'));

                if (DocumentDate.Length > 8) { DocumentDate.Substring(0, 8); }
                myStringBuilder.Append( DocumentDate.PadLeft(8, '0'));

                if (item.GLAccountDisplayNumber != null)
                {
                    if (item.GLAccountDisplayNumber.Length > 15) { item.GLAccountDisplayNumber.Substring(0, 15); }
                    myStringBuilder.Append( item.GLAccountDisplayNumber.PadLeft(15, ' '));
                }
                else
                {
                    myStringBuilder.Append(' ', 15);
                }

                if (item.OppositGLAccount != null)
                {
                    if (item.OppositGLAccount.Length > 15) { item.OppositGLAccount.Substring(0, 15); }
                    myStringBuilder.Append( item.OppositGLAccount.PadLeft(15, ' '));
                }
                else
                {
                    myStringBuilder.Append(' ', 15);
                }

                if(item.LocalAmountDebit == 0)
                {
                    myStringBuilder.Append("2");
                   
                }
                else
                {
                    myStringBuilder.Append("1");
                }


                CurrencyPM currency = currencies.Where(d => d.Id == item.CurrencyId).FirstOrDefault();


                if (computingPartnerTranslations != null)
                {

                    var partnerCode = computingPartnerTranslations.Where(d => d.ObjectTableName == "Currency" && d.OurCode == item.AccountingEntityCode).FirstOrDefault();

                    if (partnerCode != null)
                    {
                        if (partnerCode.PartnerCode.Length > 3) { partnerCode.PartnerCode = partnerCode.PartnerCode.Substring(0, 3); }
                        myStringBuilder.Append(partnerCode.PartnerCode.PadLeft(3, ' '));
                    }

                    else
                    {
                        myStringBuilder.Append(' ', 3);
                    }
                }

                else
                {
                    myStringBuilder.Append(' ', 3);
                }
                if ( item.LocalAmountDebit != 0)
                {
                    var LocalAmountDebit = "+" + item.LocalAmountDebit;

                    if (LocalAmountDebit.Length > 15) { LocalAmountDebit.Substring(0, 15); }
                    myStringBuilder.Append( LocalAmountDebit.PadLeft(15, '0'));
                }
                else
                {
                    var LocalAmountCredit = "-" + item.LocalAmountCredit;
                    if (LocalAmountCredit.Length > 15) { LocalAmountCredit.Substring(0, 15); }
                    myStringBuilder.Append( LocalAmountCredit.PadLeft(15, '0'));
                }


                if (item.ForeignAmountDebit != 0)
                {
                    var ForeignAmountDebit = "+" + item.ForeignAmountDebit;

                    if (ForeignAmountDebit.Length > 15) { ForeignAmountDebit.Substring(0, 15); }
                    myStringBuilder.Append( ForeignAmountDebit.PadLeft(15, '0'));
                }
                else
                {
                    var ForignAmountCredit = "-" + item.ForeignAmountCredit;
                    if (ForignAmountCredit.Length > 15) { ForignAmountCredit.Substring(0, 15); }
                    myStringBuilder.Append( ForignAmountCredit.PadLeft(15, '0'));
                }


                myStringBuilder.Append(' ', 12);
                myStringBuilder.Append(' ', 10);
                myStringBuilder.Append(' ', 10);
                myStringBuilder.Append(' ', 7);

                if (CreateDate.Length > 8) { CreateDate.Substring(0, 8); }
                myStringBuilder.Append(CreateDate.PadLeft(8, '0'));

                UserPM user = users.Where(d => d.Id == item.CreatedByUser).FirstOrDefault();
                if (user != null)
                {
                    if (user.Code != null)
                    {
                        if (user.Code.Length > 8) { user.Code = user.Code.Substring(0, 9); }
                        myStringBuilder.Append( user.Code.PadLeft(9, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append(' ', 9);
                    }
                }


                myStringBuilder.Append(' ', 25);


                
                myStringBuilder.Append('\n');
            }

            //B110


            AddressQuery addressQuery = new AddressQuery(tenant);
            CardQuery cardQuery = new CardQuery(tenant);
            List<CardList> cardPMs = cardQuery.GetCardPMsByTenant(tenant).ToList() ;
            List<string> cardIds = cardQuery.GetCardIdsByTenant(tenant);


            List<AddressList> addreses = addressQuery.GetAddressesByCardIds(cardIds,tenant);



            var trailReportParam = new TrailReportParam()
            {
                Tenant = tenant,
                //    MyRevenueExpenseReportLevel = ReportLevel.,
                ToDate = (DateTime)openFormatReportPM.ToDate,
                FromDate = (DateTime)openFormatReportPM.FromDate,
                CurrenciesDetailed = true,
                DetailedControlVendors = true,
                DetailedControlClients = true,
                Category1 = null,
                Category5 = null,
                Suppress_DoNotShowCardWithoutActivity = false,
                IsRevenueExpenseReport = false,
                MyTrailReportLevel = ReportLevel.GLAccount,

            };


            var typeservice = TrailReportFactory.CreateNew(trailReportParam);
            List<TrailReportM> res1 = typeservice.Execute();
            typeservice.Dispose();
           
            IEnumerable< IGrouping<string,TrailReportM>> res = res1.GroupBy(d => d.GLAccountId);
            var result = res.Where(d => d.Key != null).ToDictionary(x => x.Key, x => x);

            foreach (B110Data item in b110Data)
            {
                counter++;
                CardList card = cardPMs.Where(d => d.GLAccountId == item.GLAccountId).FirstOrDefault();
                myStringBuilder.Append("B110");

                if (counter.ToString().Length > 9)
                {
                    counter.ToString().Substring(0, 9);
                    myStringBuilder.Append("a" + counter.ToString().PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append(counter.ToString().PadLeft(9, '0'));
                }
                if (tenantPM.VatNumber != null)
                {
                    if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                    myStringBuilder.Append("a" + tenantPM.VatNumber.PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append('0', 9);
                }
                if (item.DisplayNumber != null)
                {
                    if (item.DisplayNumber.Length > 15) { item.DisplayNumber.Substring(0, 15); }
                    myStringBuilder.Append(item.DisplayNumber.PadLeft(15, ' '));
                }
                else
                {
                    myStringBuilder.Append(' ', 15);
                }
                if (item.LocalName != null)
                {
                    if (item.LocalName.Length > 50) { item.LocalName.Substring(0, 50); }
                    myStringBuilder.Append("a" + item.LocalName.PadLeft(50, ' '));
                }
                else
                {
                    if (item.EnglishName != null)
                    {
                        if (item.EnglishName.Length > 50) { item.EnglishName.Substring(0, 50); }
                        myStringBuilder.Append("a" + item.EnglishName.PadLeft(50, ' '));
                    }
                }
                if (item.ChartOfAccountsCode != null)
                {
                    if (item.ChartOfAccountsCode.Length > 15) { item.ChartOfAccountsCode.Substring(0, 15); }
                    myStringBuilder.Append("a" + item.ChartOfAccountsCode.PadLeft(15, ' '));
                }
                else
                {
                    myStringBuilder.Append(' ', 15);
                }
                if (item.ChartOfAccountsName != null)
                {
                    if (item.ChartOfAccountsName.Length > 30) { item.ChartOfAccountsName.Substring(0, 30); }
                    myStringBuilder.Append("a" + item.ChartOfAccountsName.PadLeft(30, ' '));
                }
                else
                {
                    myStringBuilder.Append(' ', 30);
                }
                if ((item.AccountTypeCode == "2" || item.AccountTypeCode == "3"))
                {
                    
                    var accountCurrency = gLAccountCurrencies.Where(d => d.GLAccountId == item.GLAccountId).FirstOrDefault();
                    if(accountCurrency != null)
                    {
                        card = cardPMs.Where(d => d.GLAccountId == accountCurrency.MainGLAccountId).FirstOrDefault();
                    }


                    if (card != null)
                    {
                        string address = null;
                        var billingaddress = addreses.Where(d => d.CardId == card.Id && d.AddressTypeId == "B").FirstOrDefault();
                        if (billingaddress != null)
                        {
                            address = billingaddress.Address1;
                            if (item.Address1 != null)
                            {
                                if (item.Address1.Length > 50) { item.Address1.Substring(0, 50); }
                                myStringBuilder.Append(item.Address1.PadLeft(50, ' '));
                                if (address != null && address.Length > 51)
                                {
                                    item.Address2 = address.Substring(51, 60);
                                    myStringBuilder.Append("a" + item.Address1.PadLeft(10, ' '));
                                }
                            }
                            else
                            {
                                myStringBuilder.Append(' ', 60);
                            }
                            if (billingaddress.City != null)
                            {
                                if (billingaddress.City.Length > 30) { billingaddress.City = billingaddress.City.Substring(0, 30); }
                                myStringBuilder.Append("a" + billingaddress.City.PadLeft(30, ' '));
                            }
                            else
                            {
                                myStringBuilder.Append(' ', 30);
                            }
                            if (billingaddress.ZipCode != null)
                            {
                                if (billingaddress.ZipCode.Length > 8) { billingaddress.ZipCode.Substring(0, 8); }
                                myStringBuilder.Append("a" + billingaddress.ZipCode.PadLeft(8, ' '));
                            }
                            else
                            {
                                myStringBuilder.Append(' ', 8);
                            }
                            if (billingaddress.CountryName != null)
                            {
                                if (billingaddress.CountryName.Length > 30) { billingaddress.CountryName.Substring(0, 30); }
                                myStringBuilder.Append("a" + billingaddress.CountryName.PadLeft(30, ' '));
                            }
                            else
                            {
                                myStringBuilder.Append(' ', 30);
                            }
                            if (computingPartnerTranslations != null && billingaddress.CountryCode != null)
                            {

                                var partnerCode = computingPartnerTranslations.Where(d => d.ObjectTableName == "Country" && d.OurCode == billingaddress.CountryCode).FirstOrDefault();

                                if (partnerCode != null)
                                {
                                    if (partnerCode.PartnerCode.Length > 15) { partnerCode.PartnerCode = partnerCode.PartnerCode.Substring(0, 15); }
                                    myStringBuilder.Append("a" + partnerCode.PartnerCode.PadLeft(15, ' '));
                                }

                                else
                                {
                                    myStringBuilder.Append(' ', 15);
                                }
                            }
                            else
                            {
                                myStringBuilder.Append(' ', 15);
                            }
                        }
                        if (address == null)
                        {
                            var mainaddress = addreses.Where(d => d.CardId == card.Id && d.AddressTypeId == "M").FirstOrDefault();
                            if (mainaddress != null)
                            {
                                address = mainaddress.Address1;
                                item.Address1 = address;
                                if (item.Address1 != null)
                                {
                                    if (item.Address1.Length > 50) { item.Address1.Substring(0, 50); }
                                    myStringBuilder.Append(item.Address1.PadLeft(50, ' '));
                                    if (address != null && address.Length > 51)
                                    {
                                        item.Address2 = address.Substring(51, 60);
                                        myStringBuilder.Append("a" + item.Address1.PadLeft(10, ' '));
                                    }
                                }
                                else
                                {
                                    myStringBuilder.Append(' ', 60);
                                }
                                if (mainaddress.City != null)
                                {
                                    if (mainaddress.City.Length > 30) { mainaddress.City = mainaddress.City.Substring(0, 30); }
                                    myStringBuilder.Append("a" + mainaddress.City.PadLeft(30, ' '));
                                }
                                else
                                {
                                    myStringBuilder.Append(' ', 30);
                                }
                                if (mainaddress.ZipCode != null)
                                {
                                    if (mainaddress.ZipCode.Length > 8) { mainaddress.ZipCode.Substring(0, 8); }
                                    myStringBuilder.Append("a" + mainaddress.ZipCode.PadLeft(8, ' '));
                                }
                                else
                                {
                                    myStringBuilder.Append(' ', 8);
                                }
                                if (mainaddress.CountryName != null)
                                {
                                    if (mainaddress.CountryName.Length > 30) { mainaddress.CountryName.Substring(0, 30); }
                                    myStringBuilder.Append("a" + mainaddress.CountryName.PadLeft(30, ' '));
                                }
                                else
                                {
                                    myStringBuilder.Append(' ', 30);
                                }
                                if (mainaddress.CountryCode != null)
                                {
                                    if (computingPartnerTranslations != null)
                                    {
                                        var partnerCode = computingPartnerTranslations.Where(d => d.ObjectTableName == "Country" && d.OurCode == mainaddress.CountryCode).FirstOrDefault();

                                        if (partnerCode != null)
                                        {
                                            if (partnerCode.PartnerCode.Length > 15) { partnerCode.PartnerCode = partnerCode.PartnerCode.Substring(0, 15); }
                                            myStringBuilder.Append("a" + partnerCode.PartnerCode.PadLeft(15, ' '));
                                        }
                                        else
                                        {
                                            myStringBuilder.Append(' ', 15);
                                        }
                                    }
                                    else
                                    {
                                        myStringBuilder.Append(' ', 15);
                                    }
                                }
                                else
                                {
                                    myStringBuilder.Append(' ', 15);
                                }
                            }
                        }


                    }
                    else
                    {
                        myStringBuilder.Append(' ', 143);
                    }
                }
                else
                {
                    myStringBuilder.Append(' ', 143);
                }

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
                        if (item.OpeningBalance.ToString().Length > 15) { item.OpeningBalance.ToString().Substring(0, 15); }
                        myStringBuilder.Append("a" + item.OpeningBalance.ToString().PadLeft(15, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append('0', 15);
                    }
                    if (item.TotalDebit != null)
                    {
                        if (item.TotalDebit.ToString().Length > 15) { item.TotalDebit.ToString().Substring(0, 15); }
                        myStringBuilder.Append("a" + item.TotalDebit.ToString().PadLeft(15, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append('0', 15);
                    }
                    if (item.TotalCredit != null)
                    {
                        if (item.TotalCredit.ToString().Length > 15) { item.TotalCredit.ToString().Substring(0, 15); }
                        myStringBuilder.Append("a" + item.TotalCredit.ToString().PadLeft(15, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append('0', 15);
                    }
                }
                else
                {
                    myStringBuilder.Append('0', 45);

                }
                myStringBuilder.Append("a0000");
                if (card != null && card.VatNumber != null)
                {
                    if (card.VatNumber.Length > 9) { card.VatNumber.Substring(0, 9); }
                    myStringBuilder.Append("a" + card.VatNumber.PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append('0', 9);
                }
                myStringBuilder.Append(' ', 7);

                if (item.IsMultiCurrency == false && item.CurrecnyId != tenantPM.CurrencyId)
                {
                   
                    item.OpeningBalanceInForegnCurrency = trailReportM != null ? trailReportM.Select(d => d.ForeignOpenBalance).Sum() : null;
                    if (item.OpeningBalanceInForegnCurrency != null)
                    {
                        if (item.OpeningBalanceInForegnCurrency.ToString().Length > 15) { item.OpeningBalanceInForegnCurrency.ToString().Substring(0, 15); }
                        myStringBuilder.Append("a" + item.OpeningBalanceInForegnCurrency.ToString().PadLeft(15, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append('0', 15);
                    }

                    item.TotalDebitInForeignCurrency = trailReportM != null ? trailReportM.Select(d => d.ForeignDebit).Sum() : null;
                    if (item.TotalDebitInForeignCurrency != null)
                    {
                        if (item.TotalDebitInForeignCurrency.ToString().Length > 15) { item.TotalDebitInForeignCurrency.ToString().Substring(0, 15); }
                        myStringBuilder.Append("a" + item.TotalDebitInForeignCurrency.ToString().PadLeft(15, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append('0', 15);
                    }

                    item.TotalCreditInForeignCurrency = trailReportM != null ? trailReportM.Select(d => d.ForeignCredit).Sum() : null;
                    if (item.TotalCreditInForeignCurrency != null)
                    {
                        if (item.TotalCreditInForeignCurrency.ToString().Length > 15) { item.TotalCreditInForeignCurrency.ToString().Substring(0, 15); }
                        myStringBuilder.Append("a" + item.TotalCreditInForeignCurrency.ToString().PadLeft(15, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append('0', 15);
                    }
                    if (item.CurrencyCode != null)
                    {
                        if (item.CurrencyCode.Length > 3) { item.CurrencyCode.Substring(0, 3); }
                        myStringBuilder.Append("a" + item.CurrencyCode.PadLeft(3, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append(' ', 3);
                    }
                }
                else
                {
                    myStringBuilder.Append('0', 45);
                    myStringBuilder.Append(' ', 3);
                }

                myStringBuilder.Append(' ', 16);
                myStringBuilder.Append('\n');
            }

            //C100

            List<C100Data> C100 = new List<C100Data>();
            List<C100Data> ARC100 = GetARInvoiceC100Data(openFormatReportPM, tenant);
            List<C100Data> APC100 = GetAPInvoiceC100Data(openFormatReportPM, tenant);
            List<C100Data> ARPAymentC100 = GetARPaymentC100Data(openFormatReportPM, tenant);
            List<C100Data> DepositC100 = GetDepositC100Data(openFormatReportPM, tenant);
            ARInvoiceTotalVATQuery aRInvoiceTotalVATQuery = new ARInvoiceTotalVATQuery(tenant);
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

            ARInvoicePaymentQuery aRInvoicePaymentQuery = new ARInvoicePaymentQuery(tenant);
            ARPaymentChequeQueryService aRPaymentChequeQueryService = new ARPaymentChequeQueryService(tenant);
            List<ARInvoicePaymentPM> aRInvoicePayments = aRInvoicePaymentQuery.GetARInvoicePaymentPMsForPaymentIds(arpaymentIds, tenant);
            List<ARPaymentChequePM> cheques = aRPaymentChequeQueryService.GetAllARPaymentChequesByPaymentIds(arpaymentIds, tenant);
            //ARInvoice
            foreach (C100Data item in ARC100)
            {
                counter++;
                myStringBuilder.Append("C100");


                if (counter.ToString().Length > 9)
                {
                    counter.ToString().Substring(0, 9);
                    myStringBuilder.Append("a" + counter.ToString().PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append(counter.ToString().PadLeft(9, '0'));
                }

                if (tenantPM.VatNumber != null)
                {
                    if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                    myStringBuilder.Append("a" + tenantPM.VatNumber.PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 9);
                }
                myStringBuilder.Append(item.DocumentType);

                if (item.DocumentReference != null)
                {
                    if (item.DocumentReference.Length > 20) { item.DocumentReference = item.DocumentReference.Substring(0, 20); }
                    myStringBuilder.Append("a" + item.DocumentReference.PadLeft(20, ' '));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 20);
                }

                var CreateDate = String.Format("{0:yyyyMMdd}", item.DocumentCreateDate);
                var CreateDateTime = item.DocumentCreateDate.Value.ToString("HHmm");

                if (CreateDate.Length > 8) { CreateDate.Substring(0, 8); }
                myStringBuilder.Append("a" + CreateDate.PadLeft(8, '0'));
                if (CreateDateTime.Length > 4) { CreateDateTime.Substring(0, 4); }
                myStringBuilder.Append("a" + CreateDateTime.PadLeft(4, '0'));

                if (item.CustomerVendorName != null)
                {
                    if (item.CustomerVendorName.Length > 50) { item.CustomerVendorName = item.CustomerVendorName.Substring(0, 50); }
                    myStringBuilder.Append("a" + item.CustomerVendorName.PadLeft(50, ' '));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 50);
                }

                string address = item.AddressStreet;
                if (item.AddressStreet != null)
                {
                    if (item.AddressStreet.Length > 50) { item.AddressStreet = item.AddressStreet.Substring(0, 50); }
                    myStringBuilder.Append("a" + item.AddressStreet.PadLeft(50, ' '));

                    if (address != null && address.Length > 51)
                    {
                        item.AddressHomeNO = address.Substring(51, 60);
                        myStringBuilder.Append("a" + item.AddressHomeNO.PadLeft(10, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 10);
                    }
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 50);
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 10);
                }



                if (item.AddressCity != null)
                {
                    if (item.AddressCity.Length > 30) { item.AddressCity = item.AddressCity.Substring(0, 30); }
                    myStringBuilder.Append("a" + item.AddressCity.PadLeft(30, ' '));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 30);
                }


                if (item.AddressZIPCode != null)
                {
                    if (item.AddressZIPCode.Length > 8) { item.AddressZIPCode = item.AddressZIPCode.Substring(0, 8); }
                    myStringBuilder.Append("a" + item.AddressZIPCode.PadLeft(8, ' '));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 8);
                }

                if (item.AddressCountry != null)
                {
                    if (item.AddressCountry.Length > 30) { item.AddressCountry = item.AddressCountry.Substring(0, 30); }
                    myStringBuilder.Append("a" + item.AddressCountry.PadLeft(30, ' '));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 30);
                }

                if (item.AddressCountryCode != null)
                {
                    var partnerCode = computingPartnerTranslationHelper.GetComputingPartnerCodeTranslation(item.AddressCountryCode, "Cust", "Country");

                    if (partnerCode != null)
                    {
                        if (partnerCode.Length > 2) { partnerCode.Substring(0, 2); }
                        myStringBuilder.Append("a" + partnerCode.PadLeft(2, ' '));
                    }

                    else
                    {
                        myStringBuilder.Append("a");

                        myStringBuilder.Append(' ', 2);
                    }
                }
                else
                {
                    myStringBuilder.Append("a");

                    myStringBuilder.Append(' ', 2);
                }
                if (item.CustomeVendorTelephone != null)
                {
                    if (item.CustomeVendorTelephone.Length > 15) { item.CustomeVendorTelephone = item.CustomeVendorTelephone.Substring(0, 15); }
                    myStringBuilder.Append("a" + item.CustomeVendorTelephone.PadLeft(15, ' '));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 30);
                }

                if (item.CustomerVendorVatNumber != null)
                {
                    if (item.CustomerVendorVatNumber.Length > 9) { item.CustomerVendorVatNumber = item.CustomerVendorVatNumber.Substring(0, 9); }
                    myStringBuilder.Append("a" + item.CustomerVendorVatNumber.PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 9);
                }
                var ValueDate = String.Format("{0:yyyyMMdd}", item.ValueDate);
                if (ValueDate != null)
                {

                    myStringBuilder.Append("a" + ValueDate);
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 8);
                }

                myStringBuilder.Append('0', 15);
                myStringBuilder.Append(' ', 3);

                string AmountBFDiscount = item.TotalDocumentsAmountBeforeDiscount.ToString();

                if (AmountBFDiscount != null)
                {
                    if (AmountBFDiscount.Length > 15) { AmountBFDiscount = AmountBFDiscount.Substring(0, 15); }
                    myStringBuilder.Append("a" + AmountBFDiscount.PadLeft(15, '0'));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 15);
                }

                myStringBuilder.Append(' ', 15);
                string AmountAFDiscount = item.TotalDocumentsAmountAfterDiscount.ToString();

                if (AmountAFDiscount != null)
                {
                    if (AmountAFDiscount.Length > 15) { AmountAFDiscount = AmountAFDiscount.Substring(0, 15); }
                    myStringBuilder.Append("a" + AmountAFDiscount.PadLeft(15, '0'));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 15);
                }


                List<ARInvoiceTotalVATPM> aRInvoiceTotalVATs = totalVats.Where(d => d.ARInvoiceId == item.ARInvoiceId).ToList();
                if (aRInvoiceTotalVATs.Count > 0)
                {
                    item.VatAmount = aRInvoiceTotalVATs.Sum(d => d.LocalVATAmount);
                    string vatAmount = item.VatAmount.ToString();
                    if (vatAmount != null)
                    {
                        if (vatAmount.Length > 15) { vatAmount = vatAmount.Substring(0, 15); }
                        myStringBuilder.Append("a" + vatAmount.PadLeft(15, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 15);
                    }
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 15);
                }

                string DocumentAmountAndVATAmount = item.DocumentAmountAndVATAmount.ToString();

                if (DocumentAmountAndVATAmount != null)
                {
                    if (DocumentAmountAndVATAmount.Length > 15) { DocumentAmountAndVATAmount = DocumentAmountAndVATAmount.Substring(0, 15); }
                    myStringBuilder.Append("a" + DocumentAmountAndVATAmount.PadLeft(15, '0'));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 15);
                }

                myStringBuilder.Append('0', 15);

                GLAccountPM gLAccountPM = acccounts.Where(d => d.Id == item.GLAccountId).FirstOrDefault();
                if (gLAccountPM != null)
                {
                    item.CustomerVendorCode = gLAccountPM.DisplayNumber;
                    if (item.CustomerVendorCode != null)
                    {
                        if (item.CustomerVendorCode.Length > 15) { item.CustomerVendorCode = item.CustomerVendorCode.Substring(0, 15); }
                        myStringBuilder.Append("a" + item.CustomerVendorCode.PadLeft(15, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 15);
                    }

                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 15);
                }

                myStringBuilder.Append(' ', 10);
                if (item.IsCancelled)
                {
                    myStringBuilder.Append("1");
                }
                else
                {
                    myStringBuilder.Append("0");
                }
                var DocuemntsReferenceDate = String.Format("{0:yyyyMMdd}", item.DocuemntsReferenceDate);
                if (DocuemntsReferenceDate != null)
                {

                    myStringBuilder.Append("a" + DocuemntsReferenceDate);
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 8);
                }


                myStringBuilder.Append(' ', 7);


                if (item.CreatedbyUser != null)
                {
                    if (item.CreatedbyUser.Length > 9) { item.CreatedbyUser = item.CreatedbyUser.Substring(0, 9); }
                    myStringBuilder.Append("a" + item.CreatedbyUser.PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 9);
                }

                myStringBuilder.Append('0', 7);
                myStringBuilder.Append(' ', 13);
                myStringBuilder.Append('\n');
                //D110
                List<ARInvoiceLinePM> lines = aRInvoiceLinePMs.Where(d => d.ARInvoiceId == item.ARInvoiceId).ToList();
                foreach (ARInvoiceLinePM line in lines)
                {
                    counter++;
                    myStringBuilder.Append("D110");


                    if (counter.ToString().Length > 9)
                    {
                        counter.ToString().Substring(0, 9);
                        myStringBuilder.Append("a" + counter.ToString().PadLeft(9, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append(counter.ToString().PadLeft(9, '0'));
                    }

                    if (tenantPM.VatNumber != null)
                    {
                        if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                        myStringBuilder.Append("a" + tenantPM.VatNumber.PadLeft(9, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 9);
                    }

                    myStringBuilder.Append("305");

                    if (item.DocumentReference != null)
                    {
                        if (item.DocumentReference.Length > 20) { item.DocumentReference = item.DocumentReference.Substring(0, 20); }
                        myStringBuilder.Append("a" + item.DocumentReference.PadLeft(20, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 20);
                    }
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(line.LineNumber.ToString().PadLeft(4, '0'));
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 3);
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 20);
                    myStringBuilder.Append("a");
                    myStringBuilder.Append("1");
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 20);

                    if (line.Description != null)
                    {
                        if (line.Description.Length > 30) { line.Description = line.Description.Substring(0, 30); }
                        myStringBuilder.Append("a" + line.Description.PadLeft(30, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 30);
                    }

                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 50);
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 30);
                    myStringBuilder.Append("a");
                    myStringBuilder.Append("יחידה");
                    myStringBuilder.Append(' ', 15);


                    if (line.Quantity != null)
                    {
                        string quantity = line.Quantity.ToString();
                        if (quantity.Length > 17) { quantity = quantity.Substring(0, 17); }
                        myStringBuilder.Append("a" + quantity.PadLeft(17, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 17);
                    }



                    if (line.UnitPrice != null)
                    {
                        string UnitPrice = line.UnitPrice.ToString();
                        if (UnitPrice.Length > 15) { UnitPrice = UnitPrice.Substring(0, 15); }
                        myStringBuilder.Append("a" + UnitPrice.PadLeft(15, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 15);
                    }


                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 15);

                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 15);


                    if (line.VatPercentage != null)
                    {
                        string VatPercentage = line.VatPercentage.ToString();
                        if (VatPercentage.Length > 4) { VatPercentage = VatPercentage.Substring(0, 4); }
                        myStringBuilder.Append("a" + VatPercentage.PadLeft(4, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 4);
                    }

                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 7);

                    if (DocuemntsReferenceDate != null)
                    {

                        myStringBuilder.Append("a" + DocuemntsReferenceDate);
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 8);
                    }

                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 7);

                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 7);

                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 21);

                    myStringBuilder.Append('\n');
                }
            }

            //APInvoice

            foreach (C100Data item in APC100)
            {
                counter++;
                myStringBuilder.Append("C100");


                if (counter.ToString().Length > 9)
                {
                    counter.ToString().Substring(0, 9);
                    myStringBuilder.Append("a" + counter.ToString().PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append(counter.ToString().PadLeft(9, '0'));
                }

                if (tenantPM.VatNumber != null)
                {
                    if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                    myStringBuilder.Append("a" + tenantPM.VatNumber.PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 9);
                }
                myStringBuilder.Append(item.DocumentType);

                if (item.DocumentReference != null)
                {
                    if (item.DocumentReference.Length > 20) { item.DocumentReference = item.DocumentReference.Substring(0, 20); }
                    myStringBuilder.Append("a" + item.DocumentReference.PadLeft(20, ' '));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 20);
                }

                var CreateDate = String.Format("{0:yyyyMMdd}", item.DocumentCreateDate);
                var CreateDateTime = item.DocumentCreateDate.Value.ToString("HHmm");

                if (CreateDate.Length > 8) { CreateDate.Substring(0, 8); }
                myStringBuilder.Append("a" + CreateDate.PadLeft(8, '0'));
                if (CreateDateTime.Length > 4) { CreateDateTime.Substring(0, 4); }
                myStringBuilder.Append("a" + CreateDateTime.PadLeft(4, '0'));

                if (item.CustomerVendorName != null)
                {
                    if (item.CustomerVendorName.Length > 50) { item.CustomerVendorName = item.CustomerVendorName.Substring(0, 50); }
                    myStringBuilder.Append("a" + item.CustomerVendorName.PadLeft(50, ' '));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 50);
                }



                AddressList billingAddress = addreses.Where(d => d.CardId == item.VendorId && d.AddressTypeId == "B").FirstOrDefault();
                AddressList mainaddress = addreses.Where(d => d.CardId == item.VendorId && d.AddressTypeId == "M").FirstOrDefault();


                string address = null;
                if (billingAddress != null)
                {

                    address = billingAddress.Address1;
                    if (item.AddressStreet != null)
                    {
                        if (item.AddressStreet.Length > 50) { item.AddressStreet.Substring(0, 50); }
                        myStringBuilder.Append(item.AddressStreet.PadLeft(50, ' '));
                        if (address != null && address.Length > 51)
                        {
                            item.AddressHomeNO = address.Substring(51, 60);
                            myStringBuilder.Append("a" + item.AddressHomeNO.PadLeft(10, ' '));
                        }
                        else
                        {
                            myStringBuilder.Append("a");
                            myStringBuilder.Append(' ', 10);
                        }
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 50);
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 10);
                    }

                    if (billingAddress.City != null)
                    {
                        if (billingAddress.City.Length > 30) { billingAddress.City = billingAddress.City.Substring(0, 30); }
                        myStringBuilder.Append("a" + billingAddress.City.PadLeft(30, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append(' ', 30);
                    }
                    if (billingAddress.ZipCode != null)
                    {
                        if (billingAddress.ZipCode.Length > 8) { billingAddress.ZipCode = billingAddress.ZipCode.Substring(0, 8); }
                        myStringBuilder.Append("a" + billingAddress.ZipCode.PadLeft(8, ' '));
                    }
                    else
                    {

                        myStringBuilder.Append("a");

                        myStringBuilder.Append(' ', 8);
                    }
                    if (billingAddress.CountryName != null)
                    {
                        if (billingAddress.CountryName.Length > 30) { billingAddress.CountryName = billingAddress.CountryName.Substring(0, 30); }
                        myStringBuilder.Append("a" + billingAddress.CountryName.PadLeft(30, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append(' ', 30);
                    }
                    if (billingAddress.CountryCode != null)
                    {
                        var partnerCode = computingPartnerTranslationHelper.GetComputingPartnerCodeTranslation(billingAddress.CountryCode, "Cust", "Country");

                        if (partnerCode != null)
                        {
                            if (partnerCode.Length > 2) { partnerCode.Substring(0, 2); }
                            myStringBuilder.Append("a" + partnerCode.PadLeft(2, ' '));
                        }

                        else
                        {
                            myStringBuilder.Append("a");

                            myStringBuilder.Append(' ', 2);
                        }
                    }
                    else
                    {
                        myStringBuilder.Append("a");

                        myStringBuilder.Append(' ', 2);
                    }
                }

                else if (mainaddress != null)
                {



                    address = mainaddress.Address1;
                    item.AddressStreet = address;
                    if (item.AddressStreet != null)
                    {
                        if (item.AddressStreet.Length > 50) { item.AddressStreet = item.AddressStreet.Substring(0, 50); }
                        myStringBuilder.Append("a" + item.AddressStreet.PadLeft(50, ' '));
                        if (address != null && address.Length > 51)
                        {
                            item.AddressHomeNO = address.Substring(51, 60);
                            myStringBuilder.Append("a" + item.AddressHomeNO.PadLeft(10, ' '));
                        }
                        else
                        {
                            myStringBuilder.Append("a");
                            myStringBuilder.Append(' ', 10);
                        }
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 50);
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 10);
                    }
                    if (mainaddress.City != null)
                    {
                        if (mainaddress.City.Length > 30) { mainaddress.City = mainaddress.City.Substring(0, 30); }
                        myStringBuilder.Append("a" + mainaddress.City.PadLeft(30, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append("a");

                        myStringBuilder.Append(' ', 30);
                    }
                    if (mainaddress.ZipCode != null)
                    {
                        if (mainaddress.ZipCode.Length > 8) { mainaddress.ZipCode = mainaddress.ZipCode.Substring(0, 8); }
                        myStringBuilder.Append("a" + mainaddress.ZipCode.PadLeft(8, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 8);
                    }
                    if (mainaddress.CountryName != null)
                    {
                        if (mainaddress.CountryName.Length > 30) { mainaddress.CountryName = mainaddress.CountryName.Substring(0, 30); }
                        myStringBuilder.Append("a" + mainaddress.CountryName.PadLeft(30, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 30);
                    }
                    if (mainaddress.CountryCode != null)
                    {
                        var partnerCode = computingPartnerTranslationHelper.GetComputingPartnerCodeTranslation(mainaddress.CountryCode, "Cust", "Country");

                        if (partnerCode != null)
                        {
                            if (partnerCode.Length > 2) { partnerCode.Substring(0, 2); }
                            myStringBuilder.Append("a" + partnerCode.PadLeft(2, ' '));
                        }

                        else
                        {
                            myStringBuilder.Append("a");

                            myStringBuilder.Append(' ', 2);
                        }
                    }

                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 2);
                    }

                    if (mainaddress.PhoneNumber != null)
                    {
                        if (mainaddress.PhoneNumber.Length > 15) { mainaddress.PhoneNumber = mainaddress.PhoneNumber.Substring(0, 15); }
                        myStringBuilder.Append("a" + mainaddress.PhoneNumber.PadLeft(15, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 15);
                    }
                }


                else
                {
                    myStringBuilder.Append(' ', 145);
                }



                if (item.CustomerVendorVatNumber != null)
                {
                    if (item.CustomerVendorVatNumber.Length > 9) { item.CustomerVendorVatNumber = item.CustomerVendorVatNumber.Substring(0, 9); }
                    myStringBuilder.Append("a" + item.CustomerVendorVatNumber.PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 9);
                }
                var ValueDate = String.Format("{0:yyyyMMdd}", item.ValueDate);
                if (ValueDate != null)
                {

                    myStringBuilder.Append("a" + ValueDate);
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 8);
                }

                myStringBuilder.Append('0', 15);
                myStringBuilder.Append(' ', 3);

                string AmountBFDiscount = item.TotalDocumentsAmountBeforeDiscount.ToString();

                if (AmountBFDiscount != null)
                {
                    if (AmountBFDiscount.Length > 15) { AmountBFDiscount = AmountBFDiscount.Substring(0, 15); }
                    myStringBuilder.Append("a" + AmountBFDiscount.PadLeft(15, '0'));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 15);
                }

                myStringBuilder.Append(' ', 15);
                string AmountAFDiscount = item.TotalDocumentsAmountAfterDiscount.ToString();

                if (AmountAFDiscount != null)
                {
                    if (AmountAFDiscount.Length > 15) { AmountAFDiscount = AmountAFDiscount.Substring(0, 15); }
                    myStringBuilder.Append("a" + AmountAFDiscount.PadLeft(15, '0'));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 15);
                }


                List<APInvoiceTotalVATPM> aPInvoiceTotalVATs = APtotalVats.Where(d => d.APInvoiceId == item.APInvoiceId).ToList();
                if (aPInvoiceTotalVATs.Count > 0)
                {
                    item.VatAmount = aPInvoiceTotalVATs.Sum(d => d.LocalVATAmount);
                    string vatAmount = item.VatAmount.ToString();
                    if (vatAmount != null)
                    {
                        if (vatAmount.Length > 15) { vatAmount = vatAmount.Substring(0, 15); }
                        myStringBuilder.Append("a" + vatAmount.PadLeft(15, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 15);
                    }
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 15);
                }

                string DocumentAmountAndVATAmount = item.DocumentAmountAndVATAmount.ToString();

                if (DocumentAmountAndVATAmount != null)
                {
                    if (DocumentAmountAndVATAmount.Length > 15) { DocumentAmountAndVATAmount = DocumentAmountAndVATAmount.Substring(0, 15); }
                    myStringBuilder.Append("a" + DocumentAmountAndVATAmount.PadLeft(15, '0'));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 15);
                }

                myStringBuilder.Append('0', 15);

                GLAccountPM gLAccountPM = acccounts.Where(d => d.Id == item.GLAccountId).FirstOrDefault();
                if (gLAccountPM != null)
                {
                    item.CustomerVendorCode = gLAccountPM.DisplayNumber;
                    if (item.CustomerVendorCode != null)
                    {
                        if (item.CustomerVendorCode.Length > 15) { item.CustomerVendorCode = item.CustomerVendorCode.Substring(0, 15); }
                        myStringBuilder.Append("a" + item.CustomerVendorCode.PadLeft(15, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 15);
                    }

                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 15);
                }

                myStringBuilder.Append(' ', 10);
                if (item.IsCancelled)
                {
                    myStringBuilder.Append("1");
                }
                else
                {
                    myStringBuilder.Append("0");
                }
                var DocuemntsReferenceDate = String.Format("{0:yyyyMMdd}", item.DocuemntsReferenceDate);
                if (DocuemntsReferenceDate != null)
                {

                    myStringBuilder.Append("a" + DocuemntsReferenceDate);
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 8);
                }


                myStringBuilder.Append(' ', 7);


                if (item.CreatedbyUser != null)
                {
                    if (item.CreatedbyUser.Length > 9) { item.CreatedbyUser = item.CreatedbyUser.Substring(0, 9); }
                    myStringBuilder.Append("a" + item.CreatedbyUser.PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 9);
                }

                myStringBuilder.Append('0', 7);
                myStringBuilder.Append(' ', 13);
                myStringBuilder.Append('\n');

                //D110
                List<APInvoiceLinePM> lines = aPInvoiceLinePMs.Where(d => d.APInvoiceId == item.APInvoiceId).ToList();

                foreach (APInvoiceLinePM line in lines)
                {
                    counter++;
                    myStringBuilder.Append("D110");


                    if (counter.ToString().Length > 9)
                    {
                        counter.ToString().Substring(0, 9);
                        myStringBuilder.Append("a" + counter.ToString().PadLeft(9, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append(counter.ToString().PadLeft(9, '0'));
                    }

                    if (tenantPM.VatNumber != null)
                    {
                        if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                        myStringBuilder.Append("a" + tenantPM.VatNumber.PadLeft(9, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 9);
                    }

                    myStringBuilder.Append("700");

                    if (item.DocumentReference != null)
                    {
                        if (item.DocumentReference.Length > 20) { item.DocumentReference = item.DocumentReference.Substring(0, 20); }
                        myStringBuilder.Append("a" + item.DocumentReference.PadLeft(20, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 20);
                    }
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(line.LineNumber.ToString().PadLeft(4, '0'));
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 3);
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 20);
                    myStringBuilder.Append("a");
                    myStringBuilder.Append("1");
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 20);

                    if (line.Description != null)
                    {
                        if (line.Description.Length > 30) { line.Description = line.Description.Substring(0, 30); }
                        myStringBuilder.Append("a" + line.Description.PadLeft(30, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 30);
                    }

                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 50);
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 30);
                    myStringBuilder.Append("a");
                    myStringBuilder.Append("יחידה");
                    myStringBuilder.Append(' ', 15);

                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 17);
                    //if (line.Quantity != null)
                    //{
                    //    string quantity = line.Quantity.ToString();
                    //    if (quantity.Length > 17) { quantity = quantity.Substring(0, 17); }
                    //    myStringBuilder.Append("a" + quantity.PadLeft(17, '0'));
                    //}
                    //else
                    //{
                    //    myStringBuilder.Append("a");
                    //    myStringBuilder.Append('0', 17);
                    //}

                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 15);

                    //if (line.UnitPrice != null)
                    //{
                    //    string UnitPrice = line.UnitPrice.ToString();
                    //    if (UnitPrice.Length > 15) { UnitPrice = UnitPrice.Substring(0, 15); }
                    //    myStringBuilder.Append("a" + UnitPrice.PadLeft(15, '0'));
                    //}
                    //else
                    //{
                    //    myStringBuilder.Append("a");
                    //    myStringBuilder.Append('0', 15);
                    //}


                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 15);

                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 15);


                    if (line.VatPercentage != null)
                    {
                        string VatPercentage = line.VatPercentage.ToString();
                        if (VatPercentage.Length > 4) { VatPercentage = VatPercentage.Substring(0, 4); }
                        myStringBuilder.Append("a" + VatPercentage.PadLeft(4, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 4);
                    }

                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 7);

                    if (DocuemntsReferenceDate != null)
                    {

                        myStringBuilder.Append("a" + DocuemntsReferenceDate);
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 8);
                    }

                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 7);

                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 7);

                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 21);

                    myStringBuilder.Append('\n');
                }
            }

            //ARPayment

            foreach (C100Data item in ARPAymentC100)
            {

                counter++;
                myStringBuilder.Append("C100");


                if (counter.ToString().Length > 9)
                {
                    counter.ToString().Substring(0, 9);
                    myStringBuilder.Append("a" + counter.ToString().PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append(counter.ToString().PadLeft(9, '0'));
                }

                if (tenantPM.VatNumber != null)
                {
                    if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                    myStringBuilder.Append("a" + tenantPM.VatNumber.PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 9);
                }
                myStringBuilder.Append(item.DocumentType);

                if (item.DocumentReference != null)
                {
                    if (item.DocumentReference.Length > 20) { item.DocumentReference = item.DocumentReference.Substring(0, 20); }
                    myStringBuilder.Append("a" + item.DocumentReference.PadLeft(20, ' '));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 20);
                }

                var CreateDate = String.Format("{0:yyyyMMdd}", item.DocumentCreateDate);
                var CreateDateTime = item.DocumentCreateDate.Value.ToString("HHmm");

                if (CreateDate.Length > 8) { CreateDate.Substring(0, 8); }
                myStringBuilder.Append("a" + CreateDate.PadLeft(8, '0'));
                if (CreateDateTime.Length > 4) { CreateDateTime.Substring(0, 4); }
                myStringBuilder.Append("a" + CreateDateTime.PadLeft(4, '0'));

                if (item.CustomerVendorName != null)
                {
                    if (item.CustomerVendorName.Length > 50) { item.CustomerVendorName = item.CustomerVendorName.Substring(0, 50); }
                    myStringBuilder.Append("a" + item.CustomerVendorName.PadLeft(50, ' '));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 50);
                }


                AddressList billingAddress = addreses.Where(d => d.CardId == item.VendorId && d.AddressTypeId == "B").FirstOrDefault();
                AddressList mainaddress = addreses.Where(d => d.CardId == item.VendorId && d.AddressTypeId == "M").FirstOrDefault();


                string address = null;
                if (billingAddress != null)
                {

                    address = billingAddress.Address1;
                    if (item.AddressStreet != null)
                    {
                        if (item.AddressStreet.Length > 50) { item.AddressStreet.Substring(0, 50); }
                        myStringBuilder.Append(item.AddressStreet.PadLeft(50, ' '));
                        if (address != null && address.Length > 51)
                        {
                            item.AddressHomeNO = address.Substring(51, 60);
                            myStringBuilder.Append("a" + item.AddressHomeNO.PadLeft(10, ' '));
                        }
                        else
                        {
                            myStringBuilder.Append("a");
                            myStringBuilder.Append(' ', 10);
                        }
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 50);
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 10);
                    }
                    if (billingAddress.City != null)
                    {
                        if (billingAddress.City.Length > 30) { billingAddress.City = billingAddress.City.Substring(0, 30); }
                        myStringBuilder.Append("a" + billingAddress.City.PadLeft(30, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append(' ', 30);
                    }
                    if (billingAddress.ZipCode != null)
                    {
                        if (billingAddress.ZipCode.Length > 8) { billingAddress.ZipCode = billingAddress.ZipCode.Substring(0, 8); }
                        myStringBuilder.Append("a" + billingAddress.ZipCode.PadLeft(8, ' '));
                    }
                    else
                    {

                        myStringBuilder.Append("a");

                        myStringBuilder.Append(' ', 8);
                    }
                    if (billingAddress.CountryName != null)
                    {
                        if (billingAddress.CountryName.Length > 30) { billingAddress.CountryName = billingAddress.CountryName.Substring(0, 30); }
                        myStringBuilder.Append("a" + billingAddress.CountryName.PadLeft(30, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append(' ', 30);
                    }
                    if (billingAddress.CountryCode != null)
                    {
                        var partnerCode = computingPartnerTranslationHelper.GetComputingPartnerCodeTranslation(billingAddress.CountryCode, "Cust", "Country");

                        if (partnerCode != null)
                        {
                            if (partnerCode.Length > 2) { partnerCode.Substring(0, 2); }
                            myStringBuilder.Append("a" + partnerCode.PadLeft(2, ' '));
                        }

                        else
                        {
                            myStringBuilder.Append("a");

                            myStringBuilder.Append(' ', 2);
                        }
                    }
                    else
                    {
                        myStringBuilder.Append("a");

                        myStringBuilder.Append(' ', 2);
                    }
                }

                else if (mainaddress != null)
                {



                    address = mainaddress.Address1;
                    item.AddressStreet = address;
                    if (item.AddressStreet != null)
                    {
                        if (item.AddressStreet.Length > 50) { item.AddressStreet = item.AddressStreet.Substring(0, 50); }
                        myStringBuilder.Append("a" + item.AddressStreet.PadLeft(50, ' '));
                        if (address != null && address.Length > 51)
                        {
                            item.AddressHomeNO = address.Substring(51, 60);
                            myStringBuilder.Append("a" + item.AddressHomeNO.PadLeft(10, ' '));
                        }
                        else
                        {
                            myStringBuilder.Append("a");
                            myStringBuilder.Append(' ', 10);
                        }
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 50);
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 10);
                    }
                    if (mainaddress.City != null)
                    {
                        if (mainaddress.City.Length > 30) { mainaddress.City = mainaddress.City.Substring(0, 30); }
                        myStringBuilder.Append("a" + mainaddress.City.PadLeft(30, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append("a");

                        myStringBuilder.Append(' ', 30);
                    }
                    if (mainaddress.ZipCode != null)
                    {
                        if (mainaddress.ZipCode.Length > 8) { mainaddress.ZipCode = mainaddress.ZipCode.Substring(0, 8); }
                        myStringBuilder.Append("a" + mainaddress.ZipCode.PadLeft(8, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 8);
                    }
                    if (mainaddress.CountryName != null)
                    {
                        if (mainaddress.CountryName.Length > 30) { mainaddress.CountryName = mainaddress.CountryName.Substring(0, 30); }
                        myStringBuilder.Append("a" + mainaddress.CountryName.PadLeft(30, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 30);
                    }
                    if (mainaddress.CountryCode != null)
                    {
                        var partnerCode = computingPartnerTranslationHelper.GetComputingPartnerCodeTranslation(mainaddress.CountryCode, "Cust", "Country");

                        if (partnerCode != null)
                        {
                            if (partnerCode.Length > 2) { partnerCode.Substring(0, 2); }
                            myStringBuilder.Append("a" + partnerCode.PadLeft(2, ' '));
                        }

                        else
                        {
                            myStringBuilder.Append("a");

                            myStringBuilder.Append(' ', 2);
                        }
                    }

                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 2);
                    }

                    if (mainaddress.PhoneNumber != null)
                    {
                        if (mainaddress.PhoneNumber.Length > 15) { mainaddress.PhoneNumber = mainaddress.PhoneNumber.Substring(0, 15); }
                        myStringBuilder.Append("a" + mainaddress.PhoneNumber.PadLeft(15, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 15);
                    }
                }


                else
                {
                    myStringBuilder.Append(' ', 145);
                }



                if (item.CustomerVendorVatNumber != null)
                {
                    if (item.CustomerVendorVatNumber.Length > 9) { item.CustomerVendorVatNumber = item.CustomerVendorVatNumber.Substring(0, 9); }
                    myStringBuilder.Append("a" + item.CustomerVendorVatNumber.PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 9);
                }
                var ValueDate = String.Format("{0:yyyyMMdd}", item.ValueDate);
                if (ValueDate != null)
                {

                    myStringBuilder.Append("a" + ValueDate);
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 8);
                }

                myStringBuilder.Append('0', 15);
                myStringBuilder.Append(' ', 3);
                myStringBuilder.Append("a");
                myStringBuilder.Append('0', 15);


                myStringBuilder.Append(' ', 15);
                myStringBuilder.Append("a");
                myStringBuilder.Append('0', 15);

                myStringBuilder.Append("a");
                myStringBuilder.Append('0', 15);



                string DocumentAmountAndVATAmount = item.DocumentAmountAndVATAmount.ToString();

                if (DocumentAmountAndVATAmount != null)
                {
                    if (DocumentAmountAndVATAmount.Length > 15) { DocumentAmountAndVATAmount = DocumentAmountAndVATAmount.Substring(0, 15); }
                    myStringBuilder.Append("a" + DocumentAmountAndVATAmount.PadLeft(15, '0'));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 15);
                }

                myStringBuilder.Append('0', 15);

                GLAccountPM gLAccountPM = acccounts.Where(d => d.Id == item.GLAccountId).FirstOrDefault();
                if (gLAccountPM != null)
                {
                    item.CustomerVendorCode = gLAccountPM.DisplayNumber;
                    if (item.CustomerVendorCode != null)
                    {
                        if (item.CustomerVendorCode.Length > 15) { item.CustomerVendorCode = item.CustomerVendorCode.Substring(0, 15); }
                        myStringBuilder.Append("a" + item.CustomerVendorCode.PadLeft(15, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 15);
                    }

                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 15);
                }

                myStringBuilder.Append(' ', 10);
                if (item.IsCancelled)
                {
                    myStringBuilder.Append("1");
                }
                else
                {
                    myStringBuilder.Append("0");
                }
                var DocuemntsReferenceDate = String.Format("{0:yyyyMMdd}", item.DocuemntsReferenceDate);
                if (DocuemntsReferenceDate != null)
                {

                    myStringBuilder.Append("a" + DocuemntsReferenceDate);
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 8);
                }


                myStringBuilder.Append(' ', 7);


                if (item.CreatedbyUser != null)
                {
                    if (item.CreatedbyUser.Length > 9) { item.CreatedbyUser = item.CreatedbyUser.Substring(0, 9); }
                    myStringBuilder.Append("a" + item.CreatedbyUser.PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 9);
                }

                myStringBuilder.Append('0', 7);
                myStringBuilder.Append(' ', 13);
                myStringBuilder.Append('\n');

                //D120
                List<ARInvoicePaymentPM> lines = aRInvoicePayments.Where(d => d.ARPaymentId == item.ARPaymentId).ToList();

                foreach (ARInvoicePaymentPM line in lines)
                {
                    counter++;
                    myStringBuilder.Append("D120");
                    ARPaymentChequePM cheque = cheques.Where(d => d.PaymentId == item.ARPaymentId).FirstOrDefault();

                    if (counter.ToString().Length > 9)
                    {
                        counter.ToString().Substring(0, 9);
                        myStringBuilder.Append("a" + counter.ToString().PadLeft(9, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append(counter.ToString().PadLeft(9, '0'));
                    }

                    if (tenantPM.VatNumber != null)
                    {
                        if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                        myStringBuilder.Append("a" + tenantPM.VatNumber.PadLeft(9, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 9);
                    }

                    myStringBuilder.Append("400");

                    if (item.DocumentReference != null)
                    {
                        if (item.DocumentReference.Length > 20) { item.DocumentReference = item.DocumentReference.Substring(0, 20); }
                        myStringBuilder.Append("a" + item.DocumentReference.PadLeft(20, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 20);
                    }
                    myStringBuilder.Append("a");
                    if (item.ARPaymentMethod == "Cheque")
                    {

                        if (cheque != null)
                        {
                            string lineNumber = cheque.LineNumber.ToString();
                            if (lineNumber.Length > 4)
                            {
                                lineNumber = lineNumber.Substring(0, 4);
                            }
                            myStringBuilder.Append("a" + lineNumber.PadLeft(4, '0'));
                        }
                        else
                        {
                            myStringBuilder.Append("a");
                            myStringBuilder.Append('0', 4);
                        }
                    }
                    else
                    {
                        myStringBuilder.Append("a0001");
                    }

                    myStringBuilder.Append("a");

                    if (item.ARPaymentMethod == "Chash")
                    {
                        myStringBuilder.Append("1");
                    }
                    else if (item.ARPaymentMethod == "Cheque")
                    {
                        myStringBuilder.Append("2");
                    }
                    else if (item.ARPaymentMethod == "Credit Card")
                    {
                        myStringBuilder.Append("3");
                    }
                    else if (item.ARPaymentMethod == "Bank Transfer")
                    {
                        myStringBuilder.Append("4");
                    }
                    else
                    {
                        myStringBuilder.Append("0");
                    }

                    if (item.ARPaymentMethod == "Cheque")
                    {
                        if (cheque != null)
                        {

                            if (cheque.BankId != null && cheque.BankId.Length > 10)
                            {
                                cheque.BankId = cheque.BankId.Substring(0, 10);
                            }
                            myStringBuilder.Append("a" + cheque.BankId.PadLeft(10, '0'));

                            if (cheque.BankBranch != null && cheque.BankBranch.Length > 10)
                            {
                                cheque.BankBranch = cheque.BankBranch.Substring(0, 10);
                            }
                            myStringBuilder.Append("a" + cheque.BankBranch.PadLeft(10, '0'));

                            if (cheque.BankAccount != null && cheque.BankAccount.Length > 10)
                            {
                                cheque.BankAccount = cheque.BankAccount.Substring(0, 10);
                            }
                            myStringBuilder.Append("a" + cheque.BankAccount.PadLeft(10, '0'));

                            if (cheque.ChequeNumber != null && cheque.ChequeNumber.Length > 10)
                            {
                                cheque.ChequeNumber = cheque.ChequeNumber.Substring(0, 10);
                            }
                            myStringBuilder.Append("a" + cheque.ChequeNumber.PadLeft(10, '0'));

                            if (cheque.ChequeNumber != null && cheque.ChequeNumber.Length > 10)
                            {
                                cheque.ChequeNumber = cheque.ChequeNumber.Substring(0, 10);
                            }
                            myStringBuilder.Append("a" + cheque.ChequeNumber.PadLeft(10, '0'));

                            var valueDate = String.Format("{0:yyyyMMdd}", cheque.ValueDate);
                            if (valueDate != null)
                            {
                                myStringBuilder.Append("a" + valueDate);
                            }
                            else
                            {
                                myStringBuilder.Append("a");
                                myStringBuilder.Append('0', 8);
                            }

                            string amount = cheque.LocalAmount.ToString();
                            if (amount.Length > 15)
                            {
                                amount = amount.Substring(0, 15);
                            }
                            myStringBuilder.Append("a" + amount.PadLeft(15, '0'));

                        }
                        else
                        {
                            myStringBuilder.Append("a");
                            myStringBuilder.Append('0', 10);
                            myStringBuilder.Append("a");
                            myStringBuilder.Append('0', 10);
                            myStringBuilder.Append("a");
                            myStringBuilder.Append('0', 10);
                            myStringBuilder.Append("a");
                            myStringBuilder.Append('0', 10);
                            myStringBuilder.Append("a");
                            myStringBuilder.Append('0', 8);
                            myStringBuilder.Append("a");
                            myStringBuilder.Append('0', 15);
                        }
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 10);
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 10);
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 10);
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 10);
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 8);
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 15);
                    }

                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 1);
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 10);
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 1);
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 7);

                    if (DocuemntsReferenceDate != null)
                    {

                        myStringBuilder.Append("a" + DocuemntsReferenceDate);
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 8);
                    }
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 7);
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 60);

                    myStringBuilder.Append('\n');
                }
            }

            //Deposit
            foreach (C100Data item in DepositC100)
            {

                counter++;
                myStringBuilder.Append("C100");


                if (counter.ToString().Length > 9)
                {
                    counter.ToString().Substring(0, 9);
                    myStringBuilder.Append("a" + counter.ToString().PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append(counter.ToString().PadLeft(9, '0'));
                }

                if (tenantPM.VatNumber != null)
                {
                    if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                    myStringBuilder.Append("a" + tenantPM.VatNumber.PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 9);
                }
                myStringBuilder.Append(item.DocumentType);

                if (item.DocumentReference != null)
                {
                    if (item.DocumentReference.Length > 20) { item.DocumentReference = item.DocumentReference.Substring(0, 20); }
                    myStringBuilder.Append("a" + item.DocumentReference.PadLeft(20, ' '));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 20);
                }

                var CreateDate = String.Format("{0:yyyyMMdd}", item.DocumentCreateDate);
                var CreateDateTime = item.DocumentCreateDate.Value.ToString("HHmm");

                if (CreateDate.Length > 8) { CreateDate.Substring(0, 8); }
                myStringBuilder.Append("a" + CreateDate.PadLeft(8, '0'));
                if (CreateDateTime.Length > 4) { CreateDateTime.Substring(0, 4); }
                myStringBuilder.Append("a" + CreateDateTime.PadLeft(4, '0'));

                if (item.CustomerVendorName != null)
                {
                    if (item.CustomerVendorName.Length > 50) { item.CustomerVendorName = item.CustomerVendorName.Substring(0, 50); }
                    myStringBuilder.Append("a" + item.CustomerVendorName.PadLeft(50, ' '));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 50);
                }



                myStringBuilder.Append("a");
                myStringBuilder.Append(' ', 50);
                myStringBuilder.Append("a");
                myStringBuilder.Append(' ', 10);
                myStringBuilder.Append("a");
                myStringBuilder.Append(' ', 30);
                myStringBuilder.Append("a");
                myStringBuilder.Append(' ', 8);
                myStringBuilder.Append("a");
                myStringBuilder.Append(' ', 30);
                myStringBuilder.Append("a");
                myStringBuilder.Append(' ', 2);
                myStringBuilder.Append("a");
                myStringBuilder.Append(' ', 15);


                myStringBuilder.Append("a");
                myStringBuilder.Append('0', 9);

                var ValueDate = String.Format("{0:yyyyMMdd}", item.ValueDate);
                if (ValueDate != null)
                {

                    myStringBuilder.Append("a" + ValueDate);
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 8);
                }

                myStringBuilder.Append('0', 15);
                myStringBuilder.Append(' ', 3);
                myStringBuilder.Append("a");
                myStringBuilder.Append('0', 15);


                myStringBuilder.Append(' ', 15);
                myStringBuilder.Append("a");
                myStringBuilder.Append('0', 15);

                myStringBuilder.Append("a");
                myStringBuilder.Append('0', 15);



                string DocumentAmountAndVATAmount = item.DocumentAmountAndVATAmount.ToString();

                if (DocumentAmountAndVATAmount != null)
                {
                    if (DocumentAmountAndVATAmount.Length > 15) { DocumentAmountAndVATAmount = DocumentAmountAndVATAmount.Substring(0, 15); }
                    myStringBuilder.Append("a" + DocumentAmountAndVATAmount.PadLeft(15, '0'));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 15);
                }

                myStringBuilder.Append('0', 15);
                myStringBuilder.Append("a");
                myStringBuilder.Append('0', 15);


                myStringBuilder.Append(' ', 10);
                if (item.IsCancelled)
                {
                    myStringBuilder.Append("1");
                }
                else
                {
                    myStringBuilder.Append("0");
                }
                var DocuemntsReferenceDate = String.Format("{0:yyyyMMdd}", item.DocuemntsReferenceDate);
                if (DocuemntsReferenceDate != null)
                {

                    myStringBuilder.Append("a" + DocuemntsReferenceDate);
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 8);
                }


                myStringBuilder.Append(' ', 7);


                if (item.CreatedbyUser != null)
                {
                    if (item.CreatedbyUser.Length > 9) { item.CreatedbyUser = item.CreatedbyUser.Substring(0, 9); }
                    myStringBuilder.Append("a" + item.CreatedbyUser.PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 9);
                }

                myStringBuilder.Append('0', 7);
                myStringBuilder.Append(' ', 13);
                myStringBuilder.Append('\n');

                //D120
                List<BankDepositLinePM> lines = depositLines.Where(d => d.DepositId == item.DepositId).ToList();

                foreach (BankDepositLinePM line in lines)
                {
                    counter++;
                    myStringBuilder.Append("D120");


                    if (counter.ToString().Length > 9)
                    {
                        counter.ToString().Substring(0, 9);
                        myStringBuilder.Append("a" + counter.ToString().PadLeft(9, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append(counter.ToString().PadLeft(9, '0'));
                    }

                    if (tenantPM.VatNumber != null)
                    {
                        if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber = tenantPM.VatNumber.Substring(0, 9); }
                        myStringBuilder.Append("a" + tenantPM.VatNumber.PadLeft(9, '0'));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 9);
                    }

                    myStringBuilder.Append("420");

                    if (item.DocumentReference != null)
                    {
                        if (item.DocumentReference.Length > 20) { item.DocumentReference = item.DocumentReference.Substring(0, 20); }
                        myStringBuilder.Append("a" + item.DocumentReference.PadLeft(20, ' '));
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append(' ', 20);
                    }
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(line.Line.ToString().PadLeft(4, '0'));


                    if (item.CashBookType != null)
                    {
                        if (item.CashBookType.Length > 1) { item.CashBookType = item.CashBookType.Substring(0, 1); }
                        myStringBuilder.Append("a" + item.CashBookType);
                    }
                    else
                    {
                        myStringBuilder.Append("a");
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
                            myStringBuilder.Append("a" + bank.PadLeft(10, '0'));

                        }
                        else
                        {
                            myStringBuilder.Append("a");
                            myStringBuilder.Append('0', 10);
                        }
                        string branch = line.Branch;

                        if (branch != null)
                        {
                            if (branch.Length > 10)
                            {
                                branch = branch.Substring(0, 10);
                            }
                            myStringBuilder.Append("a" + branch.PadLeft(10, '0'));

                        }
                        else
                        {
                            myStringBuilder.Append("a");
                            myStringBuilder.Append('0', 10);
                        }
                        string accountNumber = line.AccountNumber;

                        if (accountNumber != null)
                        {
                            if (accountNumber.Length > 15)
                            {
                                accountNumber = accountNumber.Substring(0, 15);
                            }
                            myStringBuilder.Append("a" + accountNumber.PadLeft(15, '0'));

                        }
                        else
                        {
                            myStringBuilder.Append("a");
                            myStringBuilder.Append('0', 15);
                        }

                        string chequeNumber = line.ChequeNumber;
                        if (chequeNumber != null)
                        {
                            if (chequeNumber.Length > 10)
                            {
                                chequeNumber = chequeNumber.Substring(0, 10);
                            }
                            myStringBuilder.Append("a" + chequeNumber.PadLeft(10, '0'));

                        }
                        else
                        {
                            myStringBuilder.Append("a");
                            myStringBuilder.Append('0', 10);
                        }

                        var DueDate = String.Format("{0:yyyyMMdd}", line.DueDate);
                        if (DueDate != null)
                        {
                            myStringBuilder.Append("a" + DueDate);

                        }
                        else
                        {
                            myStringBuilder.Append('0', 8);
                        }


                        string localAmount = line.LocalAmount.ToString();
                        if (localAmount != null)
                        {
                            if (localAmount.Length > 15)
                            {
                                localAmount = localAmount.Substring(0, 15);
                            }
                            myStringBuilder.Append("a" + localAmount.PadLeft(15, '0'));

                        }
                        else
                        {
                            myStringBuilder.Append("a");
                            myStringBuilder.Append('0', 15);
                        }
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 10);
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 10);
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 15);
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 10);
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 8);
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 15);

                    }

                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0');


                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 20);
                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0');
                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 7);

                    if (DocuemntsReferenceDate != null)
                    {

                        myStringBuilder.Append("a" + DocuemntsReferenceDate);
                    }
                    else
                    {
                        myStringBuilder.Append("a");
                        myStringBuilder.Append('0', 8);
                    }

                    myStringBuilder.Append("a");
                    myStringBuilder.Append('0', 7);

                    myStringBuilder.Append("a");
                    myStringBuilder.Append(' ', 60);

                    myStringBuilder.Append('\n');
                }


            }


            DocumentsFilingPM docOut = CreateDocumnetFiling(myStringBuilder, openFormatReportPM);

            return docOut;

            
            
           
            




            //DocumentsFilingPM docOut = CreateDocumnetFiling(myStringBuilder, openFormatReportPM);

            //return docOut;



        }

        public static BatchTaskExecutionPM CreateBKMVDATAFileInBatch(string taxReportId, int tenant)
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
                });
                scope.Complete();
            }

            return taskExe;

        }


        private static DocumentsFilingPM CreateDocumnetFiling(StringBuilder lines, OpenFormatReportPM openFormatReport, bool isFromWR = false)
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
            User loggedUser = GetLoggedUser(tenant);
            DocumentType docType = docTypeReposioty.GetSingleDocumentTypeByCode("BKMV", tenant);

            string _code = CodeCounter.GetNumber("DocumentsFiling", tenant).ToString();
           // string name = "A856." + tenantPM.VatNumber + "." + taxDeductionReport.TaxYear.ToString().Substring(1, 3);
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
                CreatedByUserId = loggedUser.Id,
                OwnerId = loggedUser.Id,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                UpdatedByUserId = loggedUser.Id,
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                FileExtension = "txt",
                SecurityId = "100",
                FileName = "BKMVDATA",
            };

            byte[] bytearray = Encoding.Unicode.GetBytes(file);
            document.FileData = bytearray;
            docService.Create(document, document.FileData, loggedUser.Id);


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


        public static List<C100Data> GetARInvoiceC100Data(OpenFormatReportPM openFormatReportPM,int tenant)
        {
            IInvoiceContext invoiceContext = InvoiceContext.GetContext(tenant);
            List<C100Data> c100s = (from a in invoiceContext.ARInvoices
                                    where (a.InvoiceDate >= openFormatReportPM.FromDate && a.InvoiceDate <= openFormatReportPM.ToDate) && a.Tenant == tenant
                                    select new C100Data()
                                    {
                                        ARInvoiceId = a.Id,
                                        DocumentType = "305",
                                        DocumentReference = a.InvoiceNumber,
                                        DocumentCreateDate = a.CreateDate,
                                        CustomerVendorName = a.BillTo.LocalName != null ? a.BillTo.LocalName : a.BillTo.EnglishName,
                                        AddressStreet = a.BillToAddress != null? a.BillToAddress.Address1:null,
                                        AddressCity = a.BillToAddress != null ? a.BillToAddress.City : null,
                                        AddressZIPCode = a.BillToAddress != null? a.BillToAddress.ZipCode: null,
                                        AddressCountry = a.BillToAddress.Country.LocalName != null ? a.BillToAddress.Country.LocalName : a.BillToAddress.Country.EnglishName,
                                        AddressCountryCode = a.BillToAddress != null ? a.BillToAddress.Country.Code : null,
                                        CustomeVendorTelephone = a.BillToAddress != null ? a.BillToAddress.PhoneNumber : null,
                                        CustomerVendorVatNumber = a.BillTo != null ? a.BillTo.VatNumber : null,
                                        ValueDate = a.DueDate,
                                        TotalDocumentsAmountBeforeDiscount = a.SubTotalInLocalCurrency,
                                        TotalDocumentsAmountAfterDiscount = a.SubTotalInLocalCurrency,
                                        DocumentAmountAndVATAmount = a.AmountInLocalCurrency,
                                        DocuemntsReferenceDate = a.InvoiceDate,
                                        CreatedbyUser= a.CreatedByUser.Contact.LocalName != null? a.CreatedByUser.Contact.LocalName : a.CreatedByUser.Contact.EnglishName,
                                        GLAccountId = a.BillTo.GLAccountId,
                                        IsCancelled = a.IsCancelled,

                                    }).ToList();


            return c100s;
        }

        public static List<C100Data> GetAPInvoiceC100Data(OpenFormatReportPM openFormatReportPM, int tenant)
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
                                        //AddressStreet = a.BillToAddress != null ? a.BillToAddress.Address1 : null,
                                        //AddressCity = a.BillToAddress != null ? a.BillToAddress.City : null,
                                        //AddressZIPCode = a.BillToAddress != null ? a.BillToAddress.ZipCode : null,
                                        //AddressCountry = a.BillToAddress != null ? a.BillToAddress.Country.EnglishName : null,
                                        //AddressCountryCode = a.BillToAddress != null ? a.BillToAddress.Country.Code : null,
                                        //CustomeVendorTelephone = a.BillToAddress != null ? a.BillToAddress.PhoneNumber : null,
                                        CustomerVendorVatNumber = a.VendorCard != null ? a.VendorCard.VatNumber : null,
                                        ValueDate = a.DueDate,
                                        TotalDocumentsAmountBeforeDiscount = a.SubTotalInLocalCurrency,
                                        TotalDocumentsAmountAfterDiscount = a.SubTotalInLocalCurrency,
                                        DocumentAmountAndVATAmount = a.AmountInLocalCurrency,
                                        DocuemntsReferenceDate = a.InvoiceDate,
                                        CreatedbyUser = a.CreatedByUser.Contact.LocalName != null ? a.CreatedByUser.Contact.LocalName : a.CreatedByUser.Contact.EnglishName,
                                        GLAccountId = a.VendorCard.GLAccountId,
                                        IsCancelled = false,
                                        VendorId =a.VendorId,
                                    }).ToList();


            return c100s;
        }

        public static List<C100Data> GetARPaymentC100Data(OpenFormatReportPM openFormatReportPM, int tenant)
        {
            IInvoiceContext invoiceContext = InvoiceContext.GetContext(tenant);
            List<C100Data> c100s = (from a in invoiceContext.ARPayments
                                    where ((a.ValueDate >= openFormatReportPM.FromDate && a.ValueDate <= openFormatReportPM.ToDate) || (a.RegisterDate >= openFormatReportPM.FromDate && a.RegisterDate <= openFormatReportPM.ToDate)) && a.Tenant == tenant
                                    select new C100Data()
                                    {
                                        ARPaymentId = a.Id,
                                        DocumentType = "400",
                                        DocumentReference = a.PaymentNo,
                                        DocumentCreateDate = a.CreateDate,
                                        CustomerVendorName = a.BillToCard.LocalName != null ? a.BillToCard.LocalName : a.BillToCard.EnglishName,
                                        //AddressStreet = a.BillToAddress != null ? a.BillToAddress.Address1 : null,
                                        //AddressCity = a.BillToAddress != null ? a.BillToAddress.City : null,
                                        //AddressZIPCode = a.BillToAddress != null ? a.BillToAddress.ZipCode : null,
                                        //AddressCountry = a.BillToAddress != null ? a.BillToAddress.Country.EnglishName : null,
                                        //AddressCountryCode = a.BillToAddress != null ? a.BillToAddress.Country.Code : null,
                                        //CustomeVendorTelephone = a.BillToAddress != null ? a.BillToAddress.PhoneNumber : null,
                                        CustomerVendorVatNumber = a.BillToCard != null ? a.BillToCard.VatNumber : null,
                                        ValueDate = a.ValueDate,
                                        TotalDocumentsAmountBeforeDiscount = null,
                                        TotalDocumentsAmountAfterDiscount = null,
                                        DocumentAmountAndVATAmount = a.AmountInLocalCurrency,
                                        DocuemntsReferenceDate = a.ValueDate,
                                        CreatedbyUser = a.CreatedByUser.Contact.LocalName != null ? a.CreatedByUser.Contact.LocalName : a.CreatedByUser.Contact.EnglishName,
                                        GLAccountId = a.BillToCard.GLAccountId,
                                        IsCancelled = false,
                                        VendorId = a.BillToId,
                                        ARPaymentMethod = a.AccountingPaymentMethod != null? a.AccountingPaymentMethod.Name : null,
                                       
                                    }).ToList();


            return c100s;
        }

        public static List<C100Data> GetDepositC100Data(OpenFormatReportPM openFormatReportPM, int tenant)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            List<C100Data> c100s = (from a in accountingContext.BankDeposits
                                    where (a.AccountingDate >= openFormatReportPM.FromDate && a.AccountingDate <= openFormatReportPM.ToDate) && a.Tenant == tenant
                                    select new C100Data()
                                    {
                                       
                                        DocumentType = "420",
                                        DocumentReference = a.DepositNumber.ToString(),
                                        DocumentCreateDate = a.CreateDate,
                                        CustomerVendorName =null,
                                        //AddressStreet = a.BillToAddress != null ? a.BillToAddress.Address1 : null,
                                        //AddressCity = a.BillToAddress != null ? a.BillToAddress.City : null,
                                        //AddressZIPCode = a.BillToAddress != null ? a.BillToAddress.ZipCode : null,
                                        //AddressCountry = a.BillToAddress != null ? a.BillToAddress.Country.EnglishName : null,
                                        //AddressCountryCode = a.BillToAddress != null ? a.BillToAddress.Country.Code : null,
                                        //CustomeVendorTelephone = a.BillToAddress != null ? a.BillToAddress.PhoneNumber : null,
                                        CustomerVendorVatNumber = null,
                                        ValueDate = a.AccountingDate,
                                        TotalDocumentsAmountBeforeDiscount = null,
                                        TotalDocumentsAmountAfterDiscount = null,
                                        DocumentAmountAndVATAmount =(double?) a.LocalDepositAmount,
                                        DocuemntsReferenceDate = a.AccountingDate,
                                        CreatedbyUser = a.CreatedByUser.Contact.LocalName != null ? a.CreatedByUser.Contact.LocalName : a.CreatedByUser.Contact.EnglishName,
                                        GLAccountId = null,
                                        IsCancelled = a.IsCanceled,
                                        VendorId =null,
                                        DepositId = a.Id,
                                        CashBookType = a.CashBook.CashBookTypeCode,
                                    }).ToList();


            return c100s;
        }

    }
}
