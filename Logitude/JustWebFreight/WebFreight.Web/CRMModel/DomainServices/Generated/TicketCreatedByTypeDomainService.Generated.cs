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
    public partial class TicketCreatedByTypeDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<TicketCreatedByTypePM> service;
		ICRMContext MyContext;
     	public TicketCreatedByTypeDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<TicketCreatedByTypePM>), "CRMDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<TicketCreatedByTypePM>;
		}
       
        public TicketCreatedByTypePM GetSingleTicketCreatedByTypePM(string code,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            }

            TicketCreatedByTypeQueryService ticketCreatedByTypeQuery = new TicketCreatedByTypeQueryService(MyContext);
            TicketCreatedByTypePM ticketCreatedByTypePM = ticketCreatedByTypeQuery.GetSingle(code,false,false);
            return ticketCreatedByTypePM;
           
        }

         
		public TicketCreatedByTypeList GetSingleTicketCreatedByTypeList(string code,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			 if ( MyContext == null)
            {
                 MyContext = CRMContext.GetContext(tenant);
            }

          
            TicketCreatedByTypeListQueryService listService = new TicketCreatedByTypeListQueryService(MyContext);
            return listService.GetSingle(code);
        }

		public List<TicketCreatedByTypeList> GetTicketCreatedByTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			 if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            }
            TicketCreatedByTypeListQueryService listService = new TicketCreatedByTypeListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<TicketCreatedByTypeList> GetTicketCreatedByTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			
if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            };
            TicketCreatedByTypeListQueryService listService = new TicketCreatedByTypeListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetTicketCreatedByTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            };
            TicketCreatedByTypeListQueryService queryService = new TicketCreatedByTypeListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
				
      
    }
}
	 