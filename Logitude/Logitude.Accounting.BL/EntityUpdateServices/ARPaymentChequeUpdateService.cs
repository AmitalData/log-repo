using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.Interfaces;
using Logitude.BL.Resolvers;
using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class ARPaymentChequeUpdateService
    {
        protected override void OnCreating(Def.EntityPMs.ARPaymentChequePM entityPM, Server.Tools.EntityPM entityParentPM)
        {
         //   entityPM.Id = IdCounter.GetNumber("ARPaymentCheque", entityPM.Tenant);
           
        }

        protected override void OnUpdating(ARPaymentChequePM entityPM, Data.EntityPOCOs.ARPaymentCheque entityPOCO)
        {
            //TenantQuery tenantQuery = new TenantQuery(entityPM.Tenant);
            //TenantPM tenant = tenantQuery.GetSinglePM(entityPM.Tenant);
            //if (tenant.AccountingActivated)
            //{
            //    if (entityPM.StatusCode != entityPOCO.StatusCode)
            //    {
            //        ARPaymentRepository repo = new ARPaymentRepository(entityPM.Tenant);
            //        IAccountingContext MyContext = AccountingContext.GetContext(entityPM.Tenant);
            //        GLAccountMoreDataUpdateService updateService = new GLAccountMoreDataUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);

            //        GLAccountMoreDataQueryService moreDataQueryService = new GLAccountMoreDataQueryService(entityPM.Tenant);
            //        ARPayment payment = repo.GetSingleNotCancelledARPayment(entityPM.PaymentId, entityPM.Tenant);
            //        if (payment != null)
            //        {
            //            List<ARPayment> payments = repo.GetARPaymentsByBillTo(payment.BillToId, entityPM.Tenant);
            //            List<string> paymentIds = new List<string>();
            //            foreach (var item in payments)
            //            {

            //                paymentIds.Add(item.Id);

            //            }



            //            ARPaymentChequeQueryService queryService = new ARPaymentChequeQueryService(entityPM.Tenant);
            //            List<ARPaymentChequePM> aRPaymentChequePMs = queryService.GetARPaymentChequesByPaymentIds(paymentIds, entityPM.Tenant);
            //            CardRepository cardRepo = new CardRepository(entityPM.Tenant);
            //            Card card = cardRepo.GetSingleCard(payment.BillToId, entityPM.Tenant);

            //            if (card != null)
            //            {
            //                string GLAccountId = card.GLAccountId;

            //                GLAccountMoreDataPM moreDataPM = moreDataQueryService.GetSingle(GLAccountId, false, false);
                        
            //            if ((entityPM.StatusCode == "1" || entityPM.StatusCode == "2") && entityPM.ValueDate > DateTime.Today)
            //            {
            //                    moreDataPM.TotFutureOpenChequesInLocalCur = aRPaymentChequePMs.Sum(d => d.LocalAmount);
            //                    moreDataPM.ChangeSetOp = ChangeSetOperation.Update;
            //                    updateService.Update(moreDataPM, true);
            //                }
            //            else
            //            {
            //                moreDataPM.TotalOpenChequesInLocalCur = aRPaymentChequePMs.Sum(d => d.LocalAmount);
            //                moreDataPM.ChangeSetOp = ChangeSetOperation.Update;
            //                updateService.Update(moreDataPM, true);

            //            }





            //            }
            //        }
            //    }
            //}
        }

        protected override void Trace(Def.EntityPMs.ARPaymentChequePM entityPM, Data.EntityPOCOs.ARPaymentCheque entityPOCO, string changesXml)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                ContactPM loggedContact = GetLoggedContact(entityPM.Tenant);

                if (entityPM.StatusCode == "1")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "ARPT",
                        EntityId = entityPM.Id,
                        UserId = loggedContact.Id,
                        ObjectTableName = "ARPaymentCheque",
                    });
                }
            }
        }

        protected override void AfterUpdating(ARPaymentChequePM chequePM, EntityPM entityParentPM)
        {
            bool isAccountingActivated = CheckIfAccountingIsActivated(chequePM.Tenant);
            if (isAccountingActivated)
            {
                var tenant = chequePM.Tenant;

                ARPayment payment = GetPayment(chequePM.Tenant, chequePM.PaymentId);

                if (payment != null)
                {
                    List<ARPayment> payments = GetBillToPayments(tenant, payment.BillToId);

                    var paymentIds = payments.Select(d => d.Id).ToList();


                    ARPaymentChequeQueryService queryService = new ARPaymentChequeQueryService(chequePM.Tenant);
                    List<ARPaymentChequePM> aRPaymentChequePMs = queryService.GetARPaymentChequesByPaymentIds(paymentIds, chequePM.Tenant);
                    CardRepository cardRepo = new CardRepository(chequePM.Tenant);
                    //Card card = cardRepo.GetSingleCard(payment.BillToId, entityPM.Tenant);


                    //if (card != null)
                    //{
                    string GLAccountId = cardRepo.GetGLAccountIdByCardId(payment.BillToId, chequePM.Tenant);
                    GLAccountMoreDataQueryService moreDataQueryService = new GLAccountMoreDataQueryService(chequePM.Tenant);
                    IAccountingContext MyContext = AccountingContext.GetContext(chequePM.Tenant);
                    GLAccountMoreDataPM moreDataPM = moreDataQueryService.GetSingle(GLAccountId, false, false);
                    GLAccountMoreDataUpdateService updateService = new GLAccountMoreDataUpdateService(MyContext, new Dictionary<string, IContext>(), chequePM.Tenant);
                    moreDataPM.TotFutureOpenChequesInLocalCur = 0;
                    moreDataPM.TotalOpenChequesInLocalCur = 0;
                    foreach (ARPaymentChequePM item in aRPaymentChequePMs)
                    {
                        if (item.StatusCode != "6" && item.StatusCode != "5")
                        {
                            if (item.ValueDate > DateTime.Today)
                            {
                                moreDataPM.TotFutureOpenChequesInLocalCur += item.LocalAmount;

                            }
                            else
                            {
                                moreDataPM.TotalOpenChequesInLocalCur += item.LocalAmount;

                            }
                        }
                    }

                    moreDataPM.ChangeSetOp = ChangeSetOperation.Update;
                    updateService.Update(moreDataPM, true);
                    //   SubmitChanges();
                    //}
                }

            }
        }

        private static List<ARPayment> GetBillToPayments(int tenant, string paymentBillToId)
        {
            ARPaymentRepository repo = new ARPaymentRepository(tenant);
            List<ARPayment> payments = repo.GetARPaymentsByBillTo(paymentBillToId, tenant);
            return payments;
        }

        private static ARPayment GetPayment(int tenant, string paymentId)
        {
            ARPaymentRepository repo = new ARPaymentRepository(tenant);
            ARPayment payment = repo.GetSingleNotCancelledARPayment(paymentId, tenant);
            return payment;
        }

        private bool CheckIfAccountingIsActivated(int tenant)
        {
            TenantPM tenantPM = GetTenant(tenant);
            var isAccountingActivated = tenantPM.AccountingActivated;
            return isAccountingActivated;
        }

        private static TenantPM GetTenant(int _tenant)
        {
            TenantQuery tenantQuery = new TenantQuery(_tenant);
            TenantPM tenant = tenantQuery.GetSinglePM(_tenant);
            return tenant;
        }

        public ContactPM GetLoggedContact(int tenant)
        {

            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }

        public void Save()
        {
            base.SubmitChanges();
        }
        

    }
}
