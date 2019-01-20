using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
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

        public void SetTotalFutureOpenChequesInLocalCurrency(int Tenant)
        {
            List<GlobalTenant> globalTenants;
            using (var scope = TransactionFactory.GetTransaction())
            {
                

                globalTenants = GlobalTenantRepository.GetGlobalTenants();


            }

            using (var scope = TransactionFactory.GetTransaction())
            {
                if (globalTenants != null)
                {
                    GlobalTenant tenantZero = globalTenants.Where(d => d.Id == 0).FirstOrDefault();
                    List<GlobalTenant> upgradableTenants = (from a in globalTenants
                                                            where a.Version != tenantZero.Version && a.Id != 0 && a.Version != -1 && a.IsActive == true
                                                            select a).ToList();
                    if (upgradableTenants.Count > 0)
                    {
                        foreach (GlobalTenant tenant in upgradableTenants)
                        {
                            if (tenant.Id != 0)
                            {
                                List<ARPaymentChequePM> aRPaymentCheques = null;
                                ARPaymentChequeQueryService queryService = new ARPaymentChequeQueryService(tenant.Id);
                                aRPaymentCheques = queryService.GetOpenARPaymentCheques(tenant.Id);
                                //ARPaymentRepository repo = new ARPaymentRepository(tenant.Id);
                                //CardRepository cardRepo = new CardRepository(tenant.Id);
                                GLAccountMoreDataQueryService moreDataQueryService = new GLAccountMoreDataQueryService(tenant.Id);
                                //List<string> paymentIds = new List<string>();
                                //paymentIds = aRPaymentCheques.Where(d=> d.PaymentId != null).Select(d => d.PaymentId).ToList();
                                //List<string> cardIds = repo.GetCardIdsFromPayments(paymentIds, tenant.Id);
                                //List<string> glAccountIds = cardRepo.GetGLAccountIdssByCardIds(cardIds, tenant.Id);
                                //List<GLAccountMoreDataPM> gLAccountMoreDataPMs = moreDataQueryService.GetByGLAccountsIdList(glAccountIds, tenant.Id);
                                IAccountingContext MyContext = AccountingContext.GetContext(tenant.Id);
                                GLAccountMoreDataUpdateService updateService = new GLAccountMoreDataUpdateService(MyContext, new Dictionary<string, IContext>(), tenant.Id);
                                List<ARPaymentChequeFutureData> data = moreDataQueryService.GetARPaymentChequeFutureData(tenant.Id);
                                List<string> glAccountIds = new List<string>();
                                foreach (ARPaymentChequeFutureData item in data)
                                {
                                    if (glAccountIds.Contains(item.GLAccountId))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        glAccountIds.Add(item.GLAccountId);
                                        List<string> paymentIds = data.Where(d => d.GLAccountId == item.GLAccountId).Select(d => d.PaymentId).ToList();

                                        List<ARPaymentChequePM> aRPaymentChequePMs = (from a in aRPaymentCheques
                                                                                      where paymentIds.Contains(a.PaymentId)
                                                                                      select a).ToList(); 
                                        GLAccountMoreDataPM moreDataPM = moreDataQueryService.GetSingle(item.GLAccountId, false, false);
                                        moreDataPM.TotFutureOpenChequesInLocalCur = 0;
                                        moreDataPM.TotalOpenChequesInLocalCur = 0;
                                        foreach (ARPaymentChequePM paymentCheque in aRPaymentChequePMs)
                                        {
                                            if (moreDataPM.TotalOpenChequesInLocalCur == null) moreDataPM.TotalOpenChequesInLocalCur = 0;
                                            if (moreDataPM.TotFutureOpenChequesInLocalCur == null) moreDataPM.TotFutureOpenChequesInLocalCur = 0;
                                            if ((paymentCheque.StatusCode == "2") || (paymentCheque.StatusCode == "1" && paymentCheque.ValueDate > TenantServerConfigration.GetCurrentDateTime(tenant.Id)))
                                            {
                                                moreDataPM.TotFutureOpenChequesInLocalCur += paymentCheque.LocalAmount;

                                            }
                                            else
                                            {
                                                moreDataPM.TotalOpenChequesInLocalCur += paymentCheque.LocalAmount;


                                            }
                                        }


                                        moreDataPM.ChangeSetOp = ChangeSetOperation.Update;
                                        updateService.Update(moreDataPM, true);
                                    }

                                        


                                   




                                }


                                //foreach (ARPaymentChequePM entityPM in aRPaymentCheques)
                                //{




                                //    ARPayment payment = repo.GetSingleNotCancelledARPayment(entityPM.PaymentId, entityPM.Tenant);
                                //    if (payment != null)
                                //    {
                                //        List<ARPayment> payments = repo.GetARPaymentsByBillTo(payment.BillToId, entityPM.Tenant);
                                //        List<string> paymentIds = new List<string>();
                                //        foreach (var item in payments)
                                //        {

                                //            paymentIds.Add(item.Id);

                                //        }




                                //        List<ARPaymentChequePM> aRPaymentChequePMs = aRPaymentCheques.Where(d => paymentIds.Contains(d.PaymentId)).ToList();// queryService.GetARPaymentChequesByPaymentIds(paymentIds, entityPM.Tenant);

                                //        Card card = cardRepo.GetSingleCard(payment.BillToId, entityPM.Tenant);

                                //        if (card != null)
                                //        {
                                //            string GLAccountId = card.GLAccountId;

                                //            GLAccountMoreDataPM moreDataPM = moreDataQueryService.GetSingle(GLAccountId, false, false);




                                //                if (entityPM.StatusCode != "6" && entityPM.StatusCode != "5")
                                //                {

                                //                    if (moreDataPM.TotalOpenChequesInLocalCur == null) moreDataPM.TotalOpenChequesInLocalCur = 0;
                                //                    if (moreDataPM.TotFutureOpenChequesInLocalCur == null) moreDataPM.TotFutureOpenChequesInLocalCur = 0;
                                //                    moreDataPM.TotalOpenChequesInLocalCur += entityPM.LocalAmount;
                                //                    moreDataPM.TotFutureOpenChequesInLocalCur -= entityPM.LocalAmount;



                                //                }

                                //            moreDataPM.ChangeSetOp = ChangeSetOperation.Update;
                                //            updateService.Update(moreDataPM, true);


                                //        }
                                //    }

                                //}
                            }
                        }


                    }
                }

                scope.Complete();
            }

            }



        }
    }

