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
using Logitude.Customs.BL.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.BL;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.BL.EntityQueryServices;

namespace WebFreight.Web.CustomModel.DomainServices
{ 

    [EnableClientAccess()]
    public partial class ClaimEntityDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<ClaimEntityPM> service;
		ICustomContext MyContext;
     	public ClaimEntityDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<ClaimEntityPM>), "CustomDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<ClaimEntityPM>;
		}
       
        public ClaimEntityPM GetSingleClaimEntityPM(string code,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }

            ClaimEntityQueryService claimEntityQuery = new ClaimEntityQueryService(MyContext);
            ClaimEntityPM claimEntityPM = claimEntityQuery.GetSingle(code,false,false);
            return claimEntityPM;
           
        }

         
		public ClaimEntityList GetSingleClaimEntityList(string code,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.ClaimEntity", "READ", tenant);

            if ( MyContext == null)
            {
                 MyContext = CustomContext.GetContext(tenant);
            }

          
            ClaimEntityListQueryService listService = new ClaimEntityListQueryService(MyContext);
            return listService.GetSingle(code);
        }

		public List<ClaimEntityList> GetClaimEntityLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.ClaimEntity", "READ", tenant);
            if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }
            ClaimEntityListQueryService listService = new ClaimEntityListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<ClaimEntityList> GetClaimEntitysFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.ClaimEntity", "READ", tenant);

            if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            ClaimEntityListQueryService listService = new ClaimEntityListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetClaimEntityFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.ClaimEntity", "READ", tenant);
            if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            ClaimEntityListQueryService queryService = new ClaimEntityListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
				
      
    }
}
	 