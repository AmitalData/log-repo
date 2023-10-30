using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.Accounting.BL.Utils
{
    public class FutureOpenChequesBatch
    {

        private string _ResponseText;
        private HttpStatusCode _StatusCode;

        public FutureOpenChequesBatch()
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

        public void SetTotalFutureOpenChequesInLocalCurrency(int tenant = 0)
        {

            
            CardRepository cardRepository = new CardRepository(tenant);
            List<Card> cards = cardRepository.GetAllActivityCardByTenant(tenant);

            IAccountingContext MyContext = AccountingContext.GetContext(tenant);

            GLAccountMoreDataRepository gLAccountMoreDataRepository = new GLAccountMoreDataRepository(tenant);
            GLAccountMoreDataQueryService moreDataQueryService = new GLAccountMoreDataQueryService(tenant);

            foreach (var card in cards)
            {
                
                    List<LedgerTransactionList> allChecks = gLAccountMoreDataRepository.GetAllChecks(card.Id, card.Tenant, isFuture: false, withoutDate: true);


                    GLAccountMoreData glAccountMoreData = gLAccountMoreDataRepository.GetSingle(card.GLAccountId, card.Tenant);
                    GLAccountMoreDataPM moreDataPM = moreDataQueryService.GetEntityPM(glAccountMoreData);
                    if (moreDataPM != null) 
                    {
                        moreDataPM.ChangeSetOp = ChangeSetOperation.Update;

                        moreDataPM.TotFutureOpenChequesInLocalCur = allChecks.Where(x => x.PaymentValueDate > DateTime.Today).Sum(x => (decimal?)x.CalculatedLocalAmount) ?? 0;
                        moreDataPM.TotalOpenChequesInLocalCur = allChecks.Where(x => x.PaymentValueDate <= DateTime.Today).Sum(x => (decimal?)x.CalculatedLocalAmount) ?? 0;


                        GLAccountMoreDataUpdateService gLAccountMoreDataUpdateService = new GLAccountMoreDataUpdateService(MyContext, new Dictionary<string, IContext>(), card.Tenant);
                        gLAccountMoreDataUpdateService.Update(moreDataPM, true);

                    }





            }






        }



    }
}

