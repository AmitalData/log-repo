using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class GLAccountMoreDataQueryService : EntityQueryService<GLAccountMoreData, GLAccountMoreDataKeys, GLAccountMoreDataPM, object, GLAccountMoreDataKeys>
    {

        internal List<GLAccountMoreDataPM> GetByGLAccountsIdList(List<string> GLAccountsIdList, int tenant)
        {
            var pms = (from a in repository.GetAll(tenant)
                       where GLAccountsIdList.Contains(a.AccountId) && a.Tenant == tenant
                       select a)
                       .ToList()
                       .Select(a => GetEntityPM(a))
                       .ToList();
            return pms;
        }


        public List<ARPaymentChequeFutureData> GetARPaymentChequeFutureData(int tenant)
        {
            IInvoiceContext invoicecontext = InvoiceContext.GetContext(tenant);
            Simplog.Data.CommonDataModel.ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
            List<ARPayment> payments = (from a in invoicecontext.ARPayments
                                      
                                        where  a.Tenant == tenant && a.StatusCode != "VD"

                                        select a).ToList();

            List<Data.EntityPOCOs.ARPaymentCheque> paymentCheques = (from a in context.ARPaymentCheques

                                        where a.Tenant == tenant

                                        select a).ToList();

            List<string> cardIds = (from a in payments 
                                      join c in paymentCheques on a.Id equals c.PaymentId
                                      select a.BillToId).ToList();

            List<Card> cards= (from a in commoncontext.Cards
                              where cardIds.Contains(a.Id) && a.Tenant== tenant
                              select a).ToList();

            List<string> glAccountIds = (from a in commoncontext.Cards
                                   where a.Tenant == tenant && cardIds.Contains(a.Id)
                                   select a.GLAccountId).ToList();


            List<GLAccountMoreData> gLAccountMoreDataList = (from a in context.GLAccountMoreDatas
                                                            where a.Tenant == tenant && glAccountIds.Contains(a.AccountId)
                                                            select a).ToList();

            List<ARPaymentChequeFutureData> data = (from a in gLAccountMoreDataList
                                                    join c in cards on a.AccountId equals c.GLAccountId
                                                    join p in payments on c.Id equals p.BillToId
                                                    join pc in paymentCheques on p.Id equals pc.PaymentId
                                                  
                                                    select new ARPaymentChequeFutureData()
                                                    {
                                                        GLAccountId = c.GLAccountId,
                                                        PaymentId = p.Id
                                                    }).ToList();
            LedgerTransactionListQueryService ledgerQuery = new LedgerTransactionListQueryService(context);
            var glAccountsForFutureExternalTransactions = ledgerQuery.GetGlAccountsForFutureExternalTransactions(tenant).ToList();
            foreach (var item in glAccountsForFutureExternalTransactions) {
                if (!data.Any(x => x.GLAccountId == item)) {
                    data.Add(new ARPaymentChequeFutureData { GLAccountId = item, PaymentId = null });
                }
            }
            var glAccounts = data.Select(x => x.GLAccountId).ToList();
            var gLAccountsDontHaveARPaymentCheques = (from a in context.GLAccountMoreDatas
                       where a.Tenant == tenant && a.TotFutureOpenChequesInLocalCur > 0 || a.TotalOpenChequesInLocalCur > 0 && !glAccounts.Contains(a.AccountId)
                       select a.AccountId).ToList();
            foreach (var item in gLAccountsDontHaveARPaymentCheques)
            {
                if (!data.Any(x => x.GLAccountId == item))
                {
                    data.Add(new ARPaymentChequeFutureData { GLAccountId = item, PaymentId = null });
                }
            }
            return data;





        }
        public GLAccountMoreDataPM GetSinglePMByAccountId(string accountId, int tenant)
        {
            GLAccountMoreData accountMoreData = repository.GetSingle(accountId, tenant);
            return GetEntityPM(accountMoreData);
        }

        public bool CheckIfGLAccountHasMoreDataRecord(string id, int tenant)
        {
           return (from a in context.GLAccountMoreDatas
             where a.Tenant == tenant && a.AccountId==id
             select a).Any();
        }
    }
}
