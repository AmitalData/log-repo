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
    public partial class ReconcileExternalPageStatusDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<ReconcileExternalPageStatusPM> service;
		IAccountingContext MyContext;
     	public ReconcileExternalPageStatusDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<ReconcileExternalPageStatusPM>), "AccountingDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<ReconcileExternalPageStatusPM>;
		}
       
        public ReconcileExternalPageStatusPM GetSingleReconcileExternalPageStatusPM(string code,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = AccountingContext.GetContext(tenant);
            }

            ReconcileExternalPageStatusQueryService reconcileExternalPageStatusQuery = new ReconcileExternalPageStatusQueryService(MyContext);
            ReconcileExternalPageStatusPM reconcileExternalPageStatusPM = reconcileExternalPageStatusQuery.GetSingle(code,false,false);
            return reconcileExternalPageStatusPM;
           
        }

         
		public ReconcileExternalPageStatusList GetSingleReconcileExternalPageStatusList(string code,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			 if ( MyContext == null)
            {
                 MyContext = AccountingContext.GetContext(tenant);
            }

          
            ReconcileExternalPageStatusListQueryService listService = new ReconcileExternalPageStatusListQueryService(MyContext);
            return listService.GetSingle(code);
        }

		public List<ReconcileExternalPageStatusList> GetReconcileExternalPageStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			 if ( MyContext == null)
            {
                  MyContext = AccountingContext.GetContext(tenant);
            }
            ReconcileExternalPageStatusListQueryService listService = new ReconcileExternalPageStatusListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<ReconcileExternalPageStatusList> GetReconcileExternalPageStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			
if ( MyContext == null)
            {
                  MyContext = AccountingContext.GetContext(tenant);
            };
            ReconcileExternalPageStatusListQueryService listService = new ReconcileExternalPageStatusListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetReconcileExternalPageStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			if ( MyContext == null)
            {
                  MyContext = AccountingContext.GetContext(tenant);
            };
            ReconcileExternalPageStatusListQueryService queryService = new ReconcileExternalPageStatusListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
				
      
    }
}
	 