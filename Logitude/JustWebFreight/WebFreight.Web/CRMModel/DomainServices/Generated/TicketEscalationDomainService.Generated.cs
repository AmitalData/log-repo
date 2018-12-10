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
    public partial class TicketEscalationDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<TicketEscalationPM> service;
		ICRMContext MyContext;
     	public TicketEscalationDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<TicketEscalationPM>), "CRMDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<TicketEscalationPM>;
		}
       
        public TicketEscalationPM GetSingleTicketEscalationPM(string id,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            }

            TicketEscalationQueryService ticketEscalationQuery = new TicketEscalationQueryService(MyContext);
            TicketEscalationPM ticketEscalationPM = ticketEscalationQuery.GetSingle(id,false,false);
            return ticketEscalationPM;
           
        }

         
		public TicketEscalationList GetSingleTicketEscalationList(string id,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			             SecurityUtility.CheckContactFeature("TicketEscalation", "READ", tenant); if ( MyContext == null)
            {
                 MyContext = CRMContext.GetContext(tenant);
            }

          
            TicketEscalationListQueryService listService = new TicketEscalationListQueryService(MyContext);
            return listService.GetSingle(id);
        }

		public List<TicketEscalationList> GetTicketEscalationLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			             SecurityUtility.CheckContactFeature("TicketEscalation", "READ", tenant);if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            }
            TicketEscalationListQueryService listService = new TicketEscalationListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<TicketEscalationList> GetTicketEscalationFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			            SecurityUtility.CheckContactFeature("TicketEscalation", "READ", tenant);
if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            };
            TicketEscalationListQueryService listService = new TicketEscalationListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetTicketEscalationFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			            SecurityUtility.CheckContactFeature("TicketEscalation", "READ", tenant);if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            };
            TicketEscalationListQueryService queryService = new TicketEscalationListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }
		
	    public void InsertTicketEscalation(TicketEscalationPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
			            SecurityUtility.CheckContactFeature("TicketEscalation", "NEW", entityPM.Tenant);if ( MyContext == null)
            {
                MyContext = CRMContext.GetContext(entityPM.Tenant);
            }; 
            TicketEscalationUpdateService service = new TicketEscalationUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
					    service.Update(entityPM,true); 

			ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("TicketEscalation", 0, true);
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
			    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id); 
            } 
        }


		public void UpdateTicketEscalation(TicketEscalationPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
			            SecurityUtility.CheckContactFeature("TicketEscalation", "UPDATE", entityPM.Tenant);if ( MyContext == null)
            {
                MyContext = CRMContext.GetContext(entityPM.Tenant);
            }; 
            TicketEscalationUpdateService service = new TicketEscalationUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
						 
		    service.Update(entityPM,true);

			ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("TicketEscalation", 0, true);
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
			    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id); 
            } 
        } 

		
			 		
      
    }
}
	 