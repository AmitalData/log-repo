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
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data;
using Logitude.CRM.BL;
using Logitude.CRM.Data.EntityLists;
using Logitude.CRM.BL.EntityUpdateServices;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.BL.EntityQueryServices;

namespace WebFreight.Web.CRMModel.DomainServices
{ 

    [EnableClientAccess()]
    public partial class OccasionStatusDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<OccasionStatusPM> service;
		ICRMContext MyContext;
     	public OccasionStatusDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<OccasionStatusPM>), "CRMDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<OccasionStatusPM>;
		}
       
        public OccasionStatusPM GetSingleOccasionStatusPM(string code,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            }

            OccasionStatusQueryService occasionStatusQuery = new OccasionStatusQueryService(MyContext);
            OccasionStatusPM occasionStatusPM = occasionStatusQuery.GetSingle(code,false,false);
            return occasionStatusPM;
           
        }

         
		public OccasionStatusList GetSingleOccasionStatusList(string code,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			 if ( MyContext == null)
            {
                 MyContext = CRMContext.GetContext(tenant);
            }

          
            OccasionStatusListQueryService listService = new OccasionStatusListQueryService(MyContext);
            return listService.GetSingle(code);
        }

		public List<OccasionStatusList> GetOccasionStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			 if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            }
            OccasionStatusListQueryService listService = new OccasionStatusListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<OccasionStatusList> GetOccasionStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			
if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            };
            OccasionStatusListQueryService listService = new OccasionStatusListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetOccasionStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            };
            OccasionStatusListQueryService queryService = new OccasionStatusListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
				
      
    }
}
	 