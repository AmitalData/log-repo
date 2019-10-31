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
        public void RunCardGLAccountConnect(int tenant)
        {
            try
            {
                _badList = new List<string>();
                _CustomersMade = 0;
                _VendorsMade = 0;
                _AllOthersMade = 0;

                IAccountingContext context = AccountingContext.GetContext(tenant);
                CardQuery cardQueryService = new CardQuery(tenant);
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(context);

                using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(5)))
                {
                    try
                    {
                        TryCustomers(cardQueryService, gLAccountQueryService, context, tenant);

                        scope.Complete();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("RunCardGLAccountConnect failed while performing TryCustomers ", e);
                    }
                }

                using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(5)))
                {
                    try
                    {
                        TryVendors(cardQueryService, gLAccountQueryService, context, tenant);

                        scope.Complete();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("RunCardGLAccountConnect failed while performing TryVendors ", e);
                    }
                }

                using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(5)))
                {
                    try
                    {
                        TryAllOthers(cardQueryService, gLAccountQueryService, context, tenant);

                        scope.Complete();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("RunCardGLAccountConnect failed while performing TryAllOthers ", e);
                    }
                }




                _ResponseText = $"Made Customers: {_CustomersMade},  Vendors: {_VendorsMade},   All others: {_AllOthersMade}, Errors: {String.Join(", ", _badList.ToArray())}";
            }
            catch (Exception e)
            {
                throw new Exception("CardGLAccountConnectBatch failure ", e);
            }
        }

        private void TryAllOthers(CardQuery cardQueryService, GLAccountQueryService gLAccountQueryService, IAccountingContext context, int tenant)
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
                            List<GLAccount> gLAccountList = gLAccountQueryService.GetByDisplayNumberAndAccType(cardList.PayablesAccountingCard, "3", tenant);
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
                            List<GLAccount> gLAccountList = gLAccountQueryService.GetByDisplayNumberAndAccType(cardList.ReceivablesAccountingCard, "3", tenant);
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

        private void TryVendors(CardQuery cardQueryService, GLAccountQueryService gLAccountQueryService, IAccountingContext context, int tenant)
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

        private void TryCustomers(CardQuery cardQueryService, GLAccountQueryService gLAccountQueryService, IAccountingContext context, int tenant)
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
            bool skipConnectedCardsValidation = false;
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


    }
}