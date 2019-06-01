
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.Dashboard
{
    public class GLAccountDashboard
    {
        public //List<KeyValuePair<string, decimal>> 
       List<ChartOfAccountBalanceM>
       GetCardsLocalBalanceGByChartOfAccountsTypeCode(int tenant,bool byAccountingDateBalance1, string CollectorId)
        {
            var context = AccountingContext.GetContext(tenant);
            var myChartOfAccountsTypeRepository = new ChartOfAccountsTypeRepository(context);
            var myGLAccountRepository = new GLAccountRepository(context);
            var allCOATCloseTableWithoutTenant = myChartOfAccountsTypeRepository.GetAll();//CloseTableWithoutTenant
            string AllAccountTypeCode = "";
            IQueryable<GLAccountAndMoreDTO> qGLAccountAndMoreDTO = myGLAccountRepository
                //.GetQAllCards(tenant); -- return null
                .GetQAllByAccountTypeCode(tenant, AllAccountTypeCode);
            if (!String.IsNullOrWhiteSpace(CollectorId))
            {
                var qs =new GLAccountQueryService(tenant);
                var qString=qs.GetQGLAccIdByCollectorId(tenant, CollectorId, "1");
                qGLAccountAndMoreDTO = (from a in qGLAccountAndMoreDTO
                                        join accId in qString
                                        on a.Id equals accId
                                        select a
                 );
            }
            var qAllCards_SumBalnceInLocalGroupByCOATCode =
                (
                from a in myGLAccountRepository
                //.GetQAllCards(tenant)
                .GetQAllByAccountTypeCode(tenant, AllAccountTypeCode)
            group a by a.ChartOfAccountsTypeCode into g

                select new  //GLAccount() { EnglishName
                //Tuple<string, decimal>()
                //GLAccountCurrencyBalance
                {
                    Key = g.Key,
                    Value = g.Sum(a =>  //a.BalanceInLocalCurrency
                    byAccountingDateBalance1 ? (decimal)a.BalanceInLocalCurrency : (decimal)a.LocalBalanceInDue
                    )
                }

                );
            List<ChartOfAccountBalanceM> list =
                (
                from coat1 in allCOATCloseTableWithoutTenant
                join kv in qAllCards_SumBalnceInLocalGroupByCOATCode
                on coat1.Code equals kv.Key into kvT
                from kvr in kvT.DefaultIfEmpty()
                select
                new ChartOfAccountBalanceM()
                {
                    Parentid = coat1.Code,
                    ParentName = coat1.LocalName,
                    LeafLocalAmount = kvr.Value
                }
                )
                .ToList();
            list.ForEach(r =>
            {
                r.LeafLocalAmount = r.LeafLocalAmount ?? 0;

            });
            return list;

        }

        public List<ChartOfAccountBalanceM> TreeMapGLAccountBanlanceByCOA(int tenant, string BalanceByDateType, 
            string CollectorId, string CallBackCOATypeCode, string CallBackParentCOAId/*, bool twoLevel*/)
        {
            bool byAccountingDateBalance = SelectOptionTotalBy(BalanceByDateType);
            List<ChartOfAccountBalanceM> list = null;
            try
            {


                if (string.IsNullOrEmpty(CallBackCOATypeCode))/// 1st Top Call
                {
                    list = GetCardsLocalBalanceGByChartOfAccountsTypeCode(tenant, byAccountingDateBalance, CollectorId);
                    return list;
                }

                //CallBackCOATypeCode, string CallBackCOAId

                var context = AccountingContext.GetContext(tenant);
                var myChartOfAccountRepository = new ChartOfAccountRepository(context);
                IQueryable<ChartOfAccount> myCAO_ByTypeParentID = null;
                if (string.IsNullOrWhiteSpace(CallBackParentCOAId))//GOOD TO REMMEMBER -CAN BE NULL- 2nd Level!!
                {
                    myCAO_ByTypeParentID = myChartOfAccountRepository.GetByTypeParentID(CallBackCOATypeCode, null);
                }
                else
                {
                    myCAO_ByTypeParentID = myChartOfAccountRepository.GetByTypeParentID(CallBackCOATypeCode, CallBackParentCOAId);
                }
                myCAO_ByTypeParentID = myCAO_ByTypeParentID.Where(r => r.Tenant == tenant);



                var myGLAccountRepository = new GLAccountRepository(context);
                var myGLAccount_ByCOAType = myGLAccountRepository.GetByCOATypeCodeCOATypeId(tenant, CallBackCOATypeCode, null);
                if (!String.IsNullOrWhiteSpace(CollectorId))
                {

                    myGLAccount_ByCOAType =
                         (from a in myGLAccount_ByCOAType
                          join card in (context as AccountingContext).Cards.Where(r => r.Tenant == tenant)
                  on a.Id equals card.GLAccountId
                          join cust in (context as AccountingContext).Customers
                          .Where(r => r.CollectorId == CollectorId && r.Tenant == tenant)
                          on card.Id equals cust.Id
                          select a);
                }

                var qTotalBalanceInLocalCurrencyGChartOfAccountsId =
                    (from glAcc in myGLAccount_ByCOAType
                     group glAcc by glAcc.ChartOfAccountsId into g
                     select new
                     {
                         ChartOfAccountsId = g.Key,
                         TotalBalanceInLocalCurrency = g.Sum(j =>
                         byAccountingDateBalance ? (decimal)j.BalanceInLocalCurrency : (decimal)j.LocalBalanceInDue)

                     }
                 );

                var showEmptyCOA = true;
                IQueryable<ChartOfAccountBalanceM> qJoin = null;
                if (showEmptyCOA)
                {
                    qJoin =
                    (
                    from coa in myCAO_ByTypeParentID
                    join totalBalance in qTotalBalanceInLocalCurrencyGChartOfAccountsId
                    on coa.Id equals totalBalance.ChartOfAccountsId into gj
                    from canbeemptyGLAcc in gj.DefaultIfEmpty()
                    select new ChartOfAccountBalanceM
                    {
                        Parentid = CallBackCOATypeCode,
                        ChildId = coa.Id,
                        ChildName = coa.LocalName,
                        LeafLocalAmount = (canbeemptyGLAcc == null ? 0 : canbeemptyGLAcc.TotalBalanceInLocalCurrency)
                    }
                    );
                }
                else
                {
                    qJoin =
                    (
                    from coa in myCAO_ByTypeParentID
                    join totalBalance in qTotalBalanceInLocalCurrencyGChartOfAccountsId
                    on coa.Id equals totalBalance.ChartOfAccountsId
                    select new ChartOfAccountBalanceM
                    {
                        Parentid = CallBackCOATypeCode,
                        ChildId = coa.Id,
                        ChildName = coa.LocalName,
                        LeafLocalAmount = totalBalance.TotalBalanceInLocalCurrency
                    }
                    );
                }
                qJoin = qJoin.Where(c => c.LeafLocalAmount != 0);
                list = qJoin.ToList();
                return list;

            }
            finally
            {
                if (list != null)
                {
                    list.ForEach(c =>
                    {
                        c.ParentName = c.ParentName ?? "";
                        c.ParentName = c.ParentName.Replace('"', ' ');
                        c.ChildName = c.ChildName ?? "";
                        c.ChildName = c.ChildName.Replace('"', ' ');
                    });
                }

            }


        }

        private static bool SelectOptionTotalBy(string ByBalance)
        {
            bool byAccountingDateBalance1;
            switch (ByBalance)
            {

                case "AccountingDateBalance1":
                case ""://Default 
                    {
                        byAccountingDateBalance1 = true;
                    }
                    break;
                case "DueDateBalance2":
                    {
                        byAccountingDateBalance1 = false;
                    }
                    break;
                default:
                    throw new Exception("OptionByBalance  AccountingDateBalance1/DueDateBalance2");
                    break;
            }

            return byAccountingDateBalance1;
        }
    }
}
