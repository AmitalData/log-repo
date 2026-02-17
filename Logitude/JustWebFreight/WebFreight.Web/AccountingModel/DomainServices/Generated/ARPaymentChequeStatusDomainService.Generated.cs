using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.ServiceModel.DomainServices.Hosting; 
using System.ServiceModel.DomainServices.Server;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.BL.EntityQueryServices;

namespace WebFreight.Web.AccountingModel.DomainServices
{ 

    [EnableClientAccess()]
    public partial class ARPaymentChequeStatusDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<ARPaymentChequeStatusPM> service;
		IAccountingContext MyContext;
     	public ARPaymentChequeStatusDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<ARPaymentChequeStatusPM>), "AccountingDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<ARPaymentChequeStatusPM>;
		}
       
        public ARPaymentChequeStatusPM GetSingleARPaymentChequeStatusPM(string code,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = AccountingContext.GetContext(tenant);
            }

            ARPaymentChequeStatusQueryService aRPaymentChequeStatusQuery = new ARPaymentChequeStatusQueryService(MyContext);
            ARPaymentChequeStatusPM aRPaymentChequeStatusPM = aRPaymentChequeStatusQuery.GetSingle(code,false,false);
            return aRPaymentChequeStatusPM;
           
        }

         
		public ARPaymentChequeStatusList GetSingleARPaymentChequeStatusList(string code,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			 if ( MyContext == null)
            {
                 MyContext = AccountingContext.GetContext(tenant);
            }

          
            ARPaymentChequeStatusListQueryService listService = new ARPaymentChequeStatusListQueryService(MyContext);
            return listService.GetSingle(code);
        }

		public List<ARPaymentChequeStatusList> GetARPaymentChequeStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			 if ( MyContext == null)
            {
                  MyContext = AccountingContext.GetContext(tenant);
            }
            ARPaymentChequeStatusListQueryService listService = new ARPaymentChequeStatusListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<ARPaymentChequeStatusList> GetARPaymentChequeStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			
if ( MyContext == null)
            {
                  MyContext = AccountingContext.GetContext(tenant);
            };
            ARPaymentChequeStatusListQueryService listService = new ARPaymentChequeStatusListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetARPaymentChequeStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			if ( MyContext == null)
            {
                  MyContext = AccountingContext.GetContext(tenant);
            };
            ARPaymentChequeStatusListQueryService queryService = new ARPaymentChequeStatusListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
				
      
    }
}
	 