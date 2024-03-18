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
    public partial class CB_CustomsItemDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<CB_CustomsItemPM> service;
		ICustomContext MyContext;
     	public CB_CustomsItemDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<CB_CustomsItemPM>), "CustomDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<CB_CustomsItemPM>;
		}
       
        public CB_CustomsItemPM GetSingleCB_CustomsItemPM(int id,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }

            CB_CustomsItemQueryService cB_CustomsItemQuery = new CB_CustomsItemQueryService(MyContext);
            CB_CustomsItemPM cB_CustomsItemPM = cB_CustomsItemQuery.GetSingle(id,false,false);
            return cB_CustomsItemPM;
           
        }

         
		public CB_CustomsItemList GetSingleCB_CustomsItemList(int id,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			             SecurityUtility.CheckContactFeature("Customs.CB_CustomsItem", "READ", tenant); if ( MyContext == null)
            {
                 MyContext = CustomContext.GetContext(tenant);
            }

          
            CB_CustomsItemListQueryService listService = new CB_CustomsItemListQueryService(MyContext);
            return listService.GetSingle(id);
        }

		public List<CB_CustomsItemList> GetCB_CustomsItemLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			             SecurityUtility.CheckContactFeature("Customs.CB_CustomsItem", "READ", tenant);if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }
            CB_CustomsItemListQueryService listService = new CB_CustomsItemListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<CB_CustomsItemList> GetCB_CustomsItemFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			            SecurityUtility.CheckContactFeature("Customs.CB_CustomsItem", "READ", tenant);
if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            CB_CustomsItemListQueryService listService = new CB_CustomsItemListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetCB_CustomsItemFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			            SecurityUtility.CheckContactFeature("Customs.CB_CustomsItem", "READ", tenant);if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            CB_CustomsItemListQueryService queryService = new CB_CustomsItemListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
				
      
    }
}
	 