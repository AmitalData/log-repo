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



    }
}
