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
    public partial class CourtInstanceDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<CourtInstancePM> service;
		ICustomContext MyContext;
     	public CourtInstanceDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<CourtInstancePM>), "CustomDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<CourtInstancePM>;
		}
       
        public CourtInstancePM GetSingleCourtInstancePM(string code,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }

            CourtInstanceQueryService courtInstanceQuery = new CourtInstanceQueryService(MyContext);
            CourtInstancePM courtInstancePM = courtInstanceQuery.GetSingle(code,false,false);
            return courtInstancePM;
           
        }

         
		public CourtInstanceList GetSingleCourtInstanceList(string code,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			 if ( MyContext == null)
            {
                 MyContext = CustomContext.GetContext(tenant);
            }

          
            CourtInstanceListQueryService listService = new CourtInstanceListQueryService(MyContext);
            return listService.GetSingle(code);
        }

		public List<CourtInstanceList> GetCourtInstanceLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			 if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }
            CourtInstanceListQueryService listService = new CourtInstanceListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<CourtInstanceList> GetCourtInstanceFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			
if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            CourtInstanceListQueryService listService = new CourtInstanceListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetCourtInstanceFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            CourtInstanceListQueryService queryService = new CourtInstanceListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
				
      
    }
}
	 