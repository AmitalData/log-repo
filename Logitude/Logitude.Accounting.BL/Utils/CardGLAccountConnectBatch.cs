using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.CoreBL;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Accounting.Data.Repositories;
using System.Diagnostics;
using System.Data.SqlTypes;

namespace Logitude.Accounting.BL.Utils
{
    public class CardGLAccountConnectBatch
    {
        private string _ResponseText;
        private HttpStatusCode _StatusCode;
        private List<string> _badList;
        private int _CustomersMade;
        private int _VendorsMade;
        private int _AllOthersMade;
        private bool _retry;


        public CardGLAccountConnectBatch()
        {
            _ResponseText = "";
            _StatusCode = HttpStatusCode.Accepted;
        }

        public string ResponseText()
        {
            return _ResponseText;
        }

        public HttpStatusCode StatusCode()
        {
            return _StatusCode;
        }
        public void RunCardGLAccountConnect(CardGLAccountConnectArg cardGLAccountConnectArg)
        {
            try
            {
                int tenant = cardGLAccountConnectArg.Tenant;

                _badList = new List<string>();
                _CustomersMade = 0;
                _VendorsMade = 0;
                _AllOthersMade = 0;
                BatchTaskExecutionPM batchTaskExecutionPM = cardGLAccountConnectArg.BatchTask;
                BatchTaskExecutionUpdateService batchTaskExecutionUpdateService = null;
                if (batchTaskExecutionPM != null)
                {
                    batchTaskExecutionUpdateService = GetBatchTaskUpdateServiceInstance(tenant);
                }

                IAccountingContext context = AccountingContext.GetContext(tenant);
                CardQuery cardQueryService = new CardQuery(tenant);
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(context);

                int timespanlimit = 10;


                _retry = true;
                while (_retry)
                {
                    _retry = false;
                    using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(timespanlimit)))
                    {
                        try
                        {
                            TryCustomers(context, tenant, timespanlimit - 1);

                            scope.Complete();
                        }
                        catch (Exception e)
                        {
                            throw; // new Exception("RunCardGLAccountConnect failed while performing TryCustomers ", e);
                        }
                    }
                }


                _retry = true;
                while (_retry)
                {
                    _retry = false;
                    using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(timespanlimit)))
                    {
                        try
                        {
                            TryVendors(context, tenant, timespanlimit - 1);

                            scope.Complete();
                        }
                        catch (Exception e)
                        {
                            throw; // new Exception("RunCardGLAccountConnect failed while performing TryVendors ", e);
                        }
                    }
                }


                _retry = true;
                while (_retry)
                {
                    _retry = false;
                    using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(timespanlimit)))
                    {
                        try
                        {
                            TryAllOthers(context, tenant, timespanlimit - 1);

                            scope.Complete();
                        }
                        catch (Exception e)
                        {
                            throw; // new Exception("RunCardGLAccountConnect failed while performing TryAllOthers ", e);
                        }
                    }
                }



                _ResponseText = $"Made Customers: {_CustomersMade},  Vendors: {_VendorsMade},   All others: {_AllOthersMade}, Errors: {String.Join(", \n", _badList.ToArray())}";
            }
            catch //(Exception e)
            {
                throw;// new Exception("CardGLAccountConnectBatch failure ", e);
            }
        }
        private void TryAllOthers( IAccountingContext context, int tenant, int timeoutinmin)
        {
            int halftime = timeoutinmin / 2;
            var sw = Stopwatch.StartNew();
            var accountRepository = new GLAccountRepository(context);
            var cards = accountRepository.GetAllOtherCardsWithoutGLAccountMatchDisplayNumberReceivable(tenant);
            if (cards != null && cards.Count > 0)
            {
                var myDiff = cards.FirstOrDefault(r => r.ReceivablesAccountingCard != r.AccountNumber);
                if (myDiff != null)
                {
                    throw new Exception($"GetAllOtherCardsWithoutGLAccountMatchDisplayNumberReceivable retrieve {myDiff.ReceivablesAccountingCard}");
                }

                foreach (var cardList in cards)
                {
                    try
                    {
                        ConnectCardToGLAccount(cardList.GLAccountId, cardList.Id, context, tenant);
                        _AllOthersMade++;
                        if (sw.Elapsed.TotalMinutes > halftime)
                        {
                            string errorText = $"Timeout -Operate the method again ";
                            _badList.Add(errorText);
                            _retry = true;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorText = ex.Message;
                        if (ex.InnerException != null && !String.IsNullOrEmpty(ex.InnerException.Message))
                        {
                            errorText += ", " + ex.InnerException.Message;
                        }
                        _badList.Add(errorText);
                    }
                }
            }


            sw = Stopwatch.StartNew();
            cards = accountRepository.GetAllOtherCardsWithoutGLAccountMatchDisplayNumberPayable(tenant);
            if (cards != null && cards.Count > 0)
            {
                var myDiff = cards.FirstOrDefault(r => r.PayablesAccountingCard != r.AccountNumber);
                if (myDiff != null)
                {
                    throw new Exception($"GetAllOtherCardsWithoutGLAccountMatchDisplayNumberPayable retrieve {myDiff.PayablesAccountingCard}");
                }
                foreach (var cardList in cards)
                {
                    try
                    {
                        ConnectCardToGLAccount(cardList.GLAccountId, cardList.Id, context, tenant);
                        _AllOthersMade++;
                        if (sw.Elapsed.TotalMinutes > halftime)
                        {
                            string errorText = $"Timeout -Operate the method again ";
                            _badList.Add(errorText);
                            _retry = true;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorText = ex.Message;
                        if (ex.InnerException != null && !String.IsNullOrEmpty(ex.InnerException.Message))
                        {
                            errorText += ", " + ex.InnerException.Message;
                        }
                        _badList.Add(errorText);
                    }
                }
            }
        }
        private void TryAllOthersOld(CardQuery cardQueryService, GLAccountQueryService gLAccountQueryService, IAccountingContext context, int tenant)
        {
            List<CardList> cards = cardQueryService.GetAllOtherCardsWithoutGLAccount(tenant);
            if (cards != null)
            {
                foreach (CardList cardList in cards)
                {
                    if (cardList != null)
                    {
                        if (!String.IsNullOrEmpty(cardList.PayablesAccountingCard))
                        {
                            List<GLAccount> gLAccountList = gLAccountQueryService.GetByDisplayNumberAndAccType(cardList.PayablesAccountingCard, "1", tenant);
                            if (gLAccountList != null)
                            {
                                GLAccount gLAccount = gLAccountList.FirstOrDefault();
                                if (gLAccount != null)
                                {
                                    try
                                    {
                                        ConnectCardToGLAccount(gLAccount.Id, cardList.Id, context, tenant);
                                        _AllOthersMade++;
                                    }
                                    catch (Exception ex)
                                    {
                                        string errorText = ex.Message;
                                        if (ex.InnerException != null && !String.IsNullOrEmpty(ex.InnerException.Message))
                                        { 
                                            errorText += ", " + ex.InnerException.Message;
                                        }
                                        _badList.Add(errorText);
                                    }
                                }
                                else
                                {
                                    string errorText = $"Card {cardList.Id} has PayablesAccountingCard {cardList.PayablesAccountingCard} that doesn't exist in the cloud";
                                    _badList.Add(errorText);
                                }
                            }
                            else
                            {
                                string errorText = $"Card {cardList.Id} has PayablesAccountingCard {cardList.PayablesAccountingCard} that does not exist in the cloud";
                                _badList.Add(errorText);
                            }
                        }
                        else if (!String.IsNullOrEmpty(cardList.ReceivablesAccountingCard))
                        {
                            List<GLAccount> gLAccountList = gLAccountQueryService.GetByDisplayNumberAndAccType(cardList.ReceivablesAccountingCard, "1", tenant);
                            if (gLAccountList != null)
                            {
                                GLAccount gLAccount = gLAccountList.FirstOrDefault();
                                if (gLAccount != null)
                                {
                                    try
                                    {
                                        ConnectCardToGLAccount(gLAccount.Id, cardList.Id, context, tenant);
                                        _AllOthersMade++;
                                    }
                                    catch (Exception ex)
                                    {
                                        string errorText = ex.Message;
                                        if (ex.InnerException != null && !String.IsNullOrEmpty(ex.InnerException.Message))
                                        {
                                            errorText += ", " + ex.InnerException.Message;
                                        }
                                        _badList.Add(errorText);
                                    }
                                }
                                else
                                {
                                    string errorText = $"Card {cardList.Id} has ReceivablesAccountingCard {cardList.ReceivablesAccountingCard} that doesn't exist in the cloud";
                                    _badList.Add(errorText);
                                }
                            }
                            else
                            {
                                string errorText = $"Card {cardList.Id} has ReceivablesAccountingCard {cardList.ReceivablesAccountingCard} that does not exist in the cloud";
                                _badList.Add(errorText);
                            }
                        }
                        else
                        {
                            string errorText = $"Card {cardList.Id} has empty PayablesAccountingCard and ReceivablesAccountingCard";
                            _badList.Add(errorText);
                        }
                    }
                }
            }
        }

        private void TryVendors( IAccountingContext context, int tenant, int timeoutinmin)
        {
            var sw = Stopwatch.StartNew();
            var accountRepository = new GLAccountRepository(context);
            var vendors = accountRepository.GetVendorCardsWithoutGLAccountMatchDisplayNumber(tenant);
            if (vendors != null && vendors.Count > 0)
            {
                var myDiff = vendors.FirstOrDefault(r => r.PayablesAccountingCard != r.AccountNumber);
                if (myDiff != null)
                {
                    throw new Exception($"GetVendorCardsWithoutGLAccountMatchDisplayNumber retrieve {myDiff.PayablesAccountingCard}");
                }
                foreach (var cardList in vendors)
                {
                    try
                    {
                        ConnectCardToGLAccount(cardList.GLAccountId, cardList.Id, context, tenant);
                        _VendorsMade++;
                        if (sw.Elapsed.TotalMinutes > timeoutinmin)
                        {
                            string errorText = $"Timeout -Operate the method again ";
                            _badList.Add(errorText);
                            _retry = true;
                            break;
                        }

                    }
                    catch (Exception ex)
                    {
                        string errorText = ex.Message;
                        if (ex.InnerException != null && !String.IsNullOrEmpty(ex.InnerException.Message))
                        {
                            errorText += ", " + ex.InnerException.Message;
                        }
                        _badList.Add(errorText);
                    }
                }
                }
        }

        private void TryVendorsOld(CardQuery cardQueryService, GLAccountQueryService gLAccountQueryService, IAccountingContext context, int tenant)
        {
            List<CardList> vendors = cardQueryService.GetVendorCardsWithoutGLAccount(tenant);
            if (vendors != null)
            {
                foreach (CardList cardList in vendors)
                {
                    if (cardList != null)
                    {
                        if (!String.IsNullOrEmpty(cardList.PayablesAccountingCard))
                        {
                            List<GLAccount> gLAccountList = gLAccountQueryService.GetByDisplayNumberAndAccType(cardList.PayablesAccountingCard, "3", tenant);
                            if (gLAccountList != null)
                            {
                                GLAccount gLAccount = gLAccountList.FirstOrDefault();
                                if (gLAccount != null)
                                {
                                    try
                                    {
                                        ConnectCardToGLAccount(gLAccount.Id, cardList.Id, context, tenant);
                                        _VendorsMade++;
                                    }
                                    catch (Exception ex)
                                    {
                                        string errorText = ex.Message;
                                        if (ex.InnerException != null && !String.IsNullOrEmpty(ex.InnerException.Message))
                                        {
                                            errorText += ", " + ex.InnerException.Message;
                                        }
                                        _badList.Add(errorText);
                                    }
                                }
                                else
                                {
                                    string errorText = $"Vendor Card {cardList.Id} has PayablesAccountingCard {cardList.PayablesAccountingCard} that doesn't exist in the cloud";
                                    _badList.Add(errorText);
                                }
                            }
                            else
                            {
                                string errorText = $"Vendor Card {cardList.Id} has PayablesAccountingCard {cardList.PayablesAccountingCard} that does not exist in the cloud";
                                _badList.Add(errorText);
                            }
                        }
                        else
                        {
                            string errorText = $"Vendor Card {cardList.Id} has empty PayablesAccountingCard";
                            _badList.Add(errorText);
                        }
                    }
                }
            }
        }

        private void TryCustomers(IAccountingContext context, int tenant, int timeoutinmin)
        {
            var sw = Stopwatch.StartNew();
            var accountRepository = new GLAccountRepository(context);
            var customers = accountRepository.GetCustomerCardsWithoutGLAccountMatchDisplayNumber(tenant);
            if (customers != null && customers.Count > 0)
            {
                var myDiff = customers.FirstOrDefault(r => r.ReceivablesAccountingCard != r.AccountNumber);
                if (myDiff!=null)
                {
                    throw new Exception($"GetCustomerCardsWithoutGLAccountMatchDisplayNumber retrieve {myDiff.ReceivablesAccountingCard}");
                }
                foreach (var cardList in customers)
                {
                    try
                    {
                        ConnectCardToGLAccount(cardList.GLAccountId, cardList.Id, context, tenant);
                        _CustomersMade++;
                        if (sw.Elapsed.TotalMinutes>= timeoutinmin)
                        {
                            string errorText = $"Timeout -Operate the method again ";
                            _badList.Add(errorText);
                            _retry = true;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorText = ex.Message;
                        if (ex.InnerException != null && !String.IsNullOrEmpty(ex.InnerException.Message))
                        {
                            errorText += ", " + ex.InnerException.Message;
                        }
                        _badList.Add(errorText);
                    }
                }
            }

        }
        private void TryCustomersOld(CardQuery cardQueryService, GLAccountQueryService gLAccountQueryService, IAccountingContext context, int tenant)
        {
            List<CardList> customers = cardQueryService.GetCustomerCardsWithoutGLAccount(tenant);
            if (customers != null)
            {
                foreach (CardList cardList in customers)
                {
                    if (cardList != null)
                    {
                        if (!String.IsNullOrEmpty(cardList.ReceivablesAccountingCard))
                        {
                            List<GLAccount> gLAccountList = gLAccountQueryService.GetByDisplayNumberAndAccType(cardList.ReceivablesAccountingCard, "2", tenant);
                            if (gLAccountList != null)
                            {
                                GLAccount gLAccount = gLAccountList.FirstOrDefault();
                                if (gLAccount != null)
                                {
                                    try
                                    {
                                        ConnectCardToGLAccount(gLAccount.Id, cardList.Id, context, tenant);
                                        _CustomersMade++;
                                    }
                                    catch (Exception ex)
                                    {
                                        string errorText = ex.Message;
                                        if (ex.InnerException != null && !String.IsNullOrEmpty(ex.InnerException.Message))
                                        {
                                            errorText += ", " + ex.InnerException.Message;
                                        }
                                        _badList.Add(errorText);
                                    }
                                }
                                else
                                {
                                    string errorText = $"Customer Card {cardList.Id} has ReceivablesAccountingCard {cardList.ReceivablesAccountingCard} that doesn't exist in the cloud";
                                    _badList.Add(errorText);
                                }
                            }
                            else
                            {
                                string errorText = $"Customer Card {cardList.Id} has ReceivablesAccountingCard {cardList.ReceivablesAccountingCard} that does not exist in the cloud";
                                _badList.Add(errorText);
                            }
                        }
                        else
                        {
                            string errorText = $"Customer Card {cardList.Id} has empty ReceivablesAccountingCard";
                            _badList.Add(errorText);
                        }
                    }
                }
            }

        }
        private void ConnectCardToGLAccount(string accountId, string cardId, IAccountingContext context, int tenant)
        {
            bool skipConnectedCardsValidation = true;
            try
            {
                IAccountingContext MyContext = AccountingContext.GetContext(tenant);
                GLAccountQueryService query = new GLAccountQueryService(context);
                CardGLAccountConnectionArgs args = new CardGLAccountConnectionArgs()
                {
                    AccountId = accountId,
                    CardId = cardId,
                    Tenant = tenant,
                    SkipConnectedCardsValidation = skipConnectedCardsValidation
                };
                query.ConnectCardToGLAccount(args);
            }
            catch (Exception ex)
            {
                throw new Exception($"Connect Card {cardId} To GLAccount {accountId} failed ", ex);
            }
        }

        private BatchTaskExecutionUpdateService GetBatchTaskUpdateServiceInstance(int tenant)
        {
            IInfrastructureContext context = InfrastructureContext.GetContext(tenant);
            BatchTaskExecutionUpdateService batchTaskExecutionUpdateService = new BatchTaskExecutionUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            return batchTaskExecutionUpdateService;
        }


    }
    public class CardGLAccountConnectArg
    {
        public int Tenant { get; set; }
        public BatchTaskExecutionPM BatchTask { get; set; }
    }
}