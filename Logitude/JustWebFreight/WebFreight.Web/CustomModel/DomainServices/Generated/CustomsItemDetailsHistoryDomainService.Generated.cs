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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.BL;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.BL.EntityQueryServices;

namespace WebFreight.Web.CustomModel.DomainServices
{ 

    [EnableClientAccess()]
    public partial class CustomsItemDetailsHistoryDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<CustomsItemDetailsHistoryPM> service;
		ICustomContext MyContext;
     	public CustomsItemDetailsHistoryDomainService()
		{ 
		  service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<CustomsItemDetailsHistoryPM>), "CustomDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<CustomsItemDetailsHistoryPM>;
		}
       
        public CustomsItemDetailsHistoryPM GetSingleCustomsItemDetailsHistoryPM(string customsitemid, string id,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }

            CustomsItemDetailsHistoryQueryService customsItemDetailsHistoryQuery = new CustomsItemDetailsHistoryQueryService(MyContext);
            CustomsItemDetailsHistoryPM customsItemDetailsHistoryPM = customsItemDetailsHistoryQuery.GetSingle(id,false,false);
            return customsItemDetailsHistoryPM;
           
        }

         
		public CustomsItemDetailsHistoryList GetSingleCustomsItemDetailsHistoryList(string customsitemid, string id,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.CustomsItemDetailsHistory", "READ", tenant);

            if ( MyContext == null)
            {
                 MyContext = CustomContext.GetContext(tenant);
            }

          
            CustomsItemDetailsHistoryListQueryService listService = new CustomsItemDetailsHistoryListQueryService(MyContext);
            return listService.GetSingle( id);
        }

		public List<CustomsItemDetailsHistoryList> GetCustomsItemDetailsHistoryLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.CustomsItemDetailsHistory", "READ", tenant);
            if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }
            CustomsItemDetailsHistoryListQueryService listService = new CustomsItemDetailsHistoryListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<CustomsItemDetailsHistoryList> GetCustomsItemDetailsHistorysFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.CustomsItemDetailsHistory", "READ", tenant);

            if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            CustomsItemDetailsHistoryListQueryService listService = new CustomsItemDetailsHistoryListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetCustomsItemDetailsHistoryFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.CustomsItemDetailsHistory", "READ", tenant);
            if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            CustomsItemDetailsHistoryListQueryService queryService = new CustomsItemDetailsHistoryListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
				
      
    }
}
	 