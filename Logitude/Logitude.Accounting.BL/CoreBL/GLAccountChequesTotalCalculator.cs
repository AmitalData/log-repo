using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
    class GLAccountChequesTotalCalculator
    {
        int tenant;
        DateTime endOfTodayDate;
        public GLAccountChequesTotalCalculator(int tenant)
        {
            this.tenant = tenant;
            endOfTodayDate = TenantServerConfigration.GetEndOfTodayDate(tenant);
        }

        public void RecalculateChequesTotalForBillToAccount(string billToAccountId)
        {
            GLAccountMoreDataPM glaccountMoreData = GetGLAccountMoreDataConnectedToBillToAccount(tenant, billToAccountId);
            
            ResetChequesTotals(glaccountMoreData);
            
            List<ARPaymentChequePM> cheques = GetChequesOfPaymentBillToAccount(tenant, billToAccountId);
            foreach (ARPaymentChequePM cheque in cheques)
                AddChequeAmountToTotal(glaccountMoreData, cheque);

            SubmiGLAccountMoreData(tenant, glaccountMoreData);
        }

        private void ResetChequesTotals(GLAccountMoreDataPM glaccountMoreData)
        {
            glaccountMoreData.TotFutureOpenChequesInLocalCur = 0;
            glaccountMoreData.TotalOpenChequesInLocalCur = 0;
        }

        private void AddChequeAmountToTotal(GLAccountMoreDataPM glaccountMoreData, ARPaymentChequePM cheque)
        {
            if (cheque.StatusCode != ARPaymentChequeStatusValues.Redeemed && cheque.StatusCode != ARPaymentChequeStatusValues.ReturnedToCustomer)
            {
                if (cheque.ValueDate > endOfTodayDate)
                    glaccountMoreData.TotFutureOpenChequesInLocalCur += cheque.LocalAmount;
                else
                    glaccountMoreData.TotalOpenChequesInLocalCur += cheque.LocalAmount;
            }
        }

        private GLAccountMoreDataPM GetGLAccountMoreDataConnectedToBillToAccount(int tenant, string billToAccountId)
        {
            string glaccountId = GetBillToGLAccountId(tenant, billToAccountId);
            GLAccountMoreDataPM glaccountMoreData = GetGLAccountMoreDataPM(tenant, glaccountId);
            return glaccountMoreData;
        }

        private List<ARPaymentChequePM> GetChequesOfPaymentBillToAccount(int tenant, string billToAccountId)
        {
            List<ARPayment> payments = GetBillToPayments(tenant, billToAccountId);

            var paymentIds = payments.Select(d => d.Id).ToList();

            List<ARPaymentChequePM> cheques = GetChequesOfPayments(tenant, paymentIds);
            return cheques;
        }


        private void SubmiGLAccountMoreData(int tenant, GLAccountMoreDataPM glaccountMoreDataPM)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            GLAccountMoreDataUpdateService updateService = new GLAccountMoreDataUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);

            glaccountMoreDataPM.ChangeSetOp = ChangeSetOperation.Update;
            updateService.Update(glaccountMoreDataPM, true);
        }

        private GLAccountMoreDataPM GetGLAccountMoreDataPM(int tenant, string GLAccountId)
        {
            GLAccountMoreDataQueryService moreDataQueryService = new GLAccountMoreDataQueryService(tenant);
            GLAccountMoreDataPM moreDataPM = moreDataQueryService.GetSingle(GLAccountId, false, false);
            return moreDataPM;
        }

        private string GetBillToGLAccountId(int tenant, string billTo)
        {
            CardRepository cardRepo = new CardRepository(tenant);
            string GLAccountId = cardRepo.GetGLAccountIdByCardId(billTo, tenant);
            return GLAccountId;
        }

        private List<ARPaymentChequePM> GetChequesOfPayments(int tenant, List<string> paymentIds)
        {
            ARPaymentChequeQueryService queryService = new ARPaymentChequeQueryService(tenant);
            List<ARPaymentChequePM> aRPaymentChequePMs = queryService.GetARPaymentChequesByPaymentIds(paymentIds, tenant);
            return aRPaymentChequePMs;
        }

        private List<ARPayment> GetBillToPayments(int tenant, string paymentBillToId)
        {
            ARPaymentRepository repo = new ARPaymentRepository(tenant);
            List<ARPayment> payments = repo.GetARPaymentsByBillTo(paymentBillToId, tenant);
            return payments;
        }

    }
}
