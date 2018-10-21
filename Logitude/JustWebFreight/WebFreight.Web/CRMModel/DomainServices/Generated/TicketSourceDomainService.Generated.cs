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
    public partial class TicketSourceDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<TicketSourcePM> service;
		ICRMContext MyContext;
     	public TicketSourceDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<TicketSourcePM>), "CRMDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<TicketSourcePM>;
		}
       
        public TicketSourcePM GetSingleTicketSourcePM(string code,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            }

            TicketSourceQueryService ticketSourceQuery = new TicketSourceQueryService(MyContext);
            TicketSourcePM ticketSourcePM = ticketSourceQuery.GetSingle(code,false,false);
            return ticketSourcePM;
           
        }

         
		public TicketSourceList GetSingleTicketSourceList(string code,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			 if ( MyContext == null)
            {
                 MyContext = CRMContext.GetContext(tenant);
            }

          
            TicketSourceListQueryService listService = new TicketSourceListQueryService(MyContext);
            return listService.GetSingle(code);
        }

		public List<TicketSourceList> GetTicketSourceLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			 if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            }
            TicketSourceListQueryService listService = new TicketSourceListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<TicketSourceList> GetTicketSourceFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			
if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            };
            TicketSourceListQueryService listService = new TicketSourceListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetTicketSourceFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            };
            TicketSourceListQueryService queryService = new TicketSourceListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
				
      
    }
}
	 