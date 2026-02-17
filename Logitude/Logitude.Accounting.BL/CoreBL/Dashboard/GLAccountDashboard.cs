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
       GetCardsLocalBalanceGByChartOfAccountsTypeCode(int tenant)
        {
            var context = AccountingContext.GetContext(tenant);
            var myChartOfAccountsTypeRepository = new ChartOfAccountsTypeRepository(context);
            var myGLAccountRepository = new GLAccountRepository(context);
            var allCOATCloseTableWithoutTenant = myChartOfAccountsTypeRepository.GetAll();//CloseTableWithoutTenant

            var qAllCards_SumBalnceInLocalGroupByCOATCode =
                (
                from a in myGLAccountRepository.GetQAllCards(tenant)
                group a by a.ChartOfAccountsTypeCode into g

                select new  //GLAccount() { EnglishName
                //Tuple<string, decimal>()
                //GLAccountCurrencyBalance
                {
                    Key = g.Key,
                    Value = g.Sum(a => a.BalanceInLocalCurrency)
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

        public List<ChartOfAccountBalanceM> TreeMapGLAccountBanlanceByCOA(int tenant, string ByBalance, 
            string MyCollector, string CallBackCOATypeCode, string CallBackParentCOAId, bool twoLevel)
        {
            List<ChartOfAccountBalanceM> list = null;
            try
            {


                if (string.IsNullOrEmpty(CallBackCOATypeCode))/// 1st Top Call
                {
                    list = GetCardsLocalBalanceGByChartOfAccountsTypeCode(tenant);
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



                var myGLAccountRepository = new GLAccountRepository(context);
                var myGLAccount_ByCOAType = myGLAccountRepository.GetByCOATypeCodeCOATypeId(tenant, CallBackCOATypeCode, null);


                var qTotalBalanceInLocalCurrencyGChartOfAccountsId =
                    (from glAcc in myGLAccount_ByCOAType
                     group glAcc by glAcc.ChartOfAccountsId into g
                     select new
                     {
                         ChartOfAccountsId = g.Key,
                         TotalBalanceInLocalCurrency = g.Sum(j => (decimal)j.BalanceInLocalCurrency)

                     }
                 );

                var showEmptyCOA = false;
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
    }
}
