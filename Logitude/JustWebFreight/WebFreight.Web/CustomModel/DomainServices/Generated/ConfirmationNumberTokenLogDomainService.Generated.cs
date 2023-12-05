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
    public partial class ConfirmationNumberTokenLogDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<ConfirmationNumberTokenLogPM> service;
		ICustomContext MyContext;
     	public ConfirmationNumberTokenLogDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<ConfirmationNumberTokenLogPM>), "CustomDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<ConfirmationNumberTokenLogPM>;
		}
       
        public ConfirmationNumberTokenLogPM GetSingleConfirmationNumberTokenLogPM(string id,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }

            ConfirmationNumberTokenLogQueryService confirmationNumberTokenLogQuery = new ConfirmationNumberTokenLogQueryService(MyContext);
            ConfirmationNumberTokenLogPM confirmationNumberTokenLogPM = confirmationNumberTokenLogQuery.GetSingle(id,false,false);
            return confirmationNumberTokenLogPM;
           
        }

         
		public ConfirmationNumberTokenLogList GetSingleConfirmationNumberTokenLogList(string id,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			             SecurityUtility.CheckContactFeature("Customs.ConfirmationNumberTokenLog", "READ", tenant); if ( MyContext == null)
            {
                 MyContext = CustomContext.GetContext(tenant);
            }

          
            ConfirmationNumberTokenLogListQueryService listService = new ConfirmationNumberTokenLogListQueryService(MyContext);
            return listService.GetSingle(id);
        }

		public List<ConfirmationNumberTokenLogList> GetConfirmationNumberTokenLogLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			             SecurityUtility.CheckContactFeature("Customs.ConfirmationNumberTokenLog", "READ", tenant);if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }
            ConfirmationNumberTokenLogListQueryService listService = new ConfirmationNumberTokenLogListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<ConfirmationNumberTokenLogList> GetConfirmationNumberTokenLogFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			            SecurityUtility.CheckContactFeature("Customs.ConfirmationNumberTokenLog", "READ", tenant);
if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            ConfirmationNumberTokenLogListQueryService listService = new ConfirmationNumberTokenLogListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetConfirmationNumberTokenLogFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			            SecurityUtility.CheckContactFeature("Customs.ConfirmationNumberTokenLog", "READ", tenant);if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            ConfirmationNumberTokenLogListQueryService queryService = new ConfirmationNumberTokenLogListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }
		
	    public void InsertConfirmationNumberTokenLog(ConfirmationNumberTokenLogPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
			            SecurityUtility.CheckContactFeature("ConfirmationNumberTokenLog", "NEW", entityPM.Tenant);if ( MyContext == null)
            {
                MyContext = CustomContext.GetContext(entityPM.Tenant);
            }; 
            ConfirmationNumberTokenLogUpdateService service = new ConfirmationNumberTokenLogUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
					    service.Update(entityPM,true); 

			ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("ConfirmationNumberTokenLog", 0, true);
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
			    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id); 
            } 
        }


		public void UpdateConfirmationNumberTokenLog(ConfirmationNumberTokenLogPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
			            SecurityUtility.CheckContactFeature("ConfirmationNumberTokenLog", "UPDATE", entityPM.Tenant);if ( MyContext == null)
            {
                MyContext = CustomContext.GetContext(entityPM.Tenant);
            }; 
            ConfirmationNumberTokenLogUpdateService service = new ConfirmationNumberTokenLogUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
						 
		    service.Update(entityPM,true);

			ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("ConfirmationNumberTokenLog", 0, true);
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
	 