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
    public partial class CB_TariffComputedDataDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<CB_TariffComputedDataPM> service;
		ICustomContext MyContext;
     	public CB_TariffComputedDataDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<CB_TariffComputedDataPM>), "CustomDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<CB_TariffComputedDataPM>;
		}
       
        public CB_TariffComputedDataPM GetSingleCB_TariffComputedDataPM(string cb_id,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }

            CB_TariffComputedDataQueryService cB_TariffComputedDataQuery = new CB_TariffComputedDataQueryService(MyContext);
            CB_TariffComputedDataPM cB_TariffComputedDataPM = cB_TariffComputedDataQuery.GetSingle(cb_id,false,false);
            return cB_TariffComputedDataPM;
           
        }

         
		public CB_TariffComputedDataList GetSingleCB_TariffComputedDataList(string cb_id,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			             SecurityUtility.CheckContactFeature("Customs.CB_TariffComputedData", "READ", tenant); if ( MyContext == null)
            {
                 MyContext = CustomContext.GetContext(tenant);
            }

          
            CB_TariffComputedDataListQueryService listService = new CB_TariffComputedDataListQueryService(MyContext);
            return listService.GetSingle(cb_id);
        }

		public List<CB_TariffComputedDataList> GetCB_TariffComputedDataLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			             SecurityUtility.CheckContactFeature("Customs.CB_TariffComputedData", "READ", tenant);if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }
            CB_TariffComputedDataListQueryService listService = new CB_TariffComputedDataListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<CB_TariffComputedDataList> GetCB_TariffComputedDataFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			            SecurityUtility.CheckContactFeature("Customs.CB_TariffComputedData", "READ", tenant);
if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            CB_TariffComputedDataListQueryService listService = new CB_TariffComputedDataListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetCB_TariffComputedDataFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			            SecurityUtility.CheckContactFeature("Customs.CB_TariffComputedData", "READ", tenant);if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            CB_TariffComputedDataListQueryService queryService = new CB_TariffComputedDataListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
				
      
    }
}
	 