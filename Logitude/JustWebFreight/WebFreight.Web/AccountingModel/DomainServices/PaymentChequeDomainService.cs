using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.Security;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System.Collections.Generic;
using System.ServiceModel.DomainServices.Hosting;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using SecurityUtility = WebFreight.Web.Security.SecurityUtility;

namespace WebFreight.Web.AccountingModel.DomainServices
{

    [EnableClientAccess()]
    public partial class PaymentChequeDomainService : LogitudeDomainService
    {
        IDomainServiceUpdateClass<PaymentChequePM> service;
        IAccountingContext MyContext;
        public PaymentChequeDomainService()
        {
            //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<PaymentChequePM>), "AccountingDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<PaymentChequePM>;
        }

        public PaymentChequePM GetSinglePaymentChequePM(string id, int tenant)
        {
            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(tenant);
            }

            PaymentChequeQueryService paymentChequeQuery = new PaymentChequeQueryService(MyContext);
            PaymentChequePM paymentChequePM = paymentChequeQuery.GetSingle(id, false, false);
            return paymentChequePM;

        }


        public PaymentChequeList GetSinglePaymentChequeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PaymentCheque", "READ", tenant); if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(tenant);
            }


            PaymentChequeListQueryService listService = new PaymentChequeListQueryService(MyContext);
            return listService.GetSingle(id);
        }

        public List<PaymentChequeList> GetPaymentChequeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PaymentCheque", "READ", tenant); if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(tenant);
            }
            PaymentChequeListQueryService listService = new PaymentChequeListQueryService(MyContext);
            return listService.GetList(tenant);
        }

        public List<PaymentChequeList> GetPaymentChequeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PaymentCheque", "READ", tenant);
            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(tenant);
            }
            ;
            PaymentChequeListQueryService listService = new PaymentChequeListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetPaymentChequeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PaymentCheque", "READ", tenant); if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(tenant);
            }
            ;
            PaymentChequeListQueryService queryService = new PaymentChequeListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertPaymentCheque(PaymentChequePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("PaymentCheque", "NEW", entityPM.Tenant); if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(entityPM.Tenant);
            }
            ;
            PaymentChequeUpdateService service = new PaymentChequeUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            service.Update(entityPM, true);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("PaymentCheque", 0, true);
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
            }
        }


        public void UpdatePaymentCheque(PaymentChequePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("PaymentCheque", "UPDATE", entityPM.Tenant); if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(entityPM.Tenant);
            }
            ;
            PaymentChequeUpdateService service = new PaymentChequeUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

            service.Update(entityPM, true);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("PaymentCheque", 0, true);
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
            }
        }




    }
}
