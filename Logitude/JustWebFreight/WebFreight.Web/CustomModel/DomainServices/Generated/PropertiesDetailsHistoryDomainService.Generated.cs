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
    public partial class PropertiesDetailsHistoryDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<PropertiesDetailsHistoryPM> service;
		ICustomContext MyContext;
     	public PropertiesDetailsHistoryDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<PropertiesDetailsHistoryPM>), "CustomDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<PropertiesDetailsHistoryPM>;
		}
       
        public PropertiesDetailsHistoryPM GetSinglePropertiesDetailsHistoryPM(string id,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }

            PropertiesDetailsHistoryQueryService propertiesDetailsHistoryQuery = new PropertiesDetailsHistoryQueryService(MyContext);
            PropertiesDetailsHistoryPM propertiesDetailsHistoryPM = propertiesDetailsHistoryQuery.GetSingle(id,false,false);
            return propertiesDetailsHistoryPM;
           
        }

         
		public PropertiesDetailsHistoryList GetSinglePropertiesDetailsHistoryList(string id,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.PropertiesDetailsHistory", "READ", tenant);

            if ( MyContext == null)
            {
                 MyContext = CustomContext.GetContext(tenant);
            }

          
            PropertiesDetailsHistoryListQueryService listService = new PropertiesDetailsHistoryListQueryService(MyContext);
            return listService.GetSingle(id);
        }

		public List<PropertiesDetailsHistoryList> GetPropertiesDetailsHistoryLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.PropertiesDetailsHistory", "READ", tenant);
            if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }
            PropertiesDetailsHistoryListQueryService listService = new PropertiesDetailsHistoryListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<PropertiesDetailsHistoryList> GetPropertiesDetailsHistorysFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.PropertiesDetailsHistory", "READ", tenant);

            if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            PropertiesDetailsHistoryListQueryService listService = new PropertiesDetailsHistoryListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetPropertiesDetailsHistoryFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.PropertiesDetailsHistory", "READ", tenant);
            if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            PropertiesDetailsHistoryListQueryService queryService = new PropertiesDetailsHistoryListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
				
      
    }
}
	 