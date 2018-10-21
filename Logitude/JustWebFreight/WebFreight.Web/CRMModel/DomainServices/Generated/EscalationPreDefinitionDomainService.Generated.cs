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
    public partial class EscalationPreDefinitionDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<EscalationPreDefinitionPM> service;
		ICRMContext MyContext;
     	public EscalationPreDefinitionDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<EscalationPreDefinitionPM>), "CRMDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<EscalationPreDefinitionPM>;
		}
       
        public EscalationPreDefinitionPM GetSingleEscalationPreDefinitionPM(string code,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            }

            EscalationPreDefinitionQueryService escalationPreDefinitionQuery = new EscalationPreDefinitionQueryService(MyContext);
            EscalationPreDefinitionPM escalationPreDefinitionPM = escalationPreDefinitionQuery.GetSingle(code,false,false);
            return escalationPreDefinitionPM;
           
        }

         
		public EscalationPreDefinitionList GetSingleEscalationPreDefinitionList(string code,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			 if ( MyContext == null)
            {
                 MyContext = CRMContext.GetContext(tenant);
            }

          
            EscalationPreDefinitionListQueryService listService = new EscalationPreDefinitionListQueryService(MyContext);
            return listService.GetSingle(code);
        }

		public List<EscalationPreDefinitionList> GetEscalationPreDefinitionLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			 if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            }
            EscalationPreDefinitionListQueryService listService = new EscalationPreDefinitionListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<EscalationPreDefinitionList> GetEscalationPreDefinitionFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			
if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            };
            EscalationPreDefinitionListQueryService listService = new EscalationPreDefinitionListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetEscalationPreDefinitionFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            };
            EscalationPreDefinitionListQueryService queryService = new EscalationPreDefinitionListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
				
      
    }
}
	 