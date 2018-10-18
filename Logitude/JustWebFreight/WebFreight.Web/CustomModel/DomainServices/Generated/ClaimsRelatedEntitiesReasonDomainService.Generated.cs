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
    public partial class ClaimsRelatedEntitiesReasonDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<ClaimsRelatedEntitiesReasonPM> service;
		ICustomContext MyContext;
     	public ClaimsRelatedEntitiesReasonDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<ClaimsRelatedEntitiesReasonPM>), "CustomDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<ClaimsRelatedEntitiesReasonPM>;
		}
       
        public ClaimsRelatedEntitiesReasonPM GetSingleClaimsRelatedEntitiesReasonPM(string claimid, int counterkey, string reasonlisttypecode,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }

            ClaimsRelatedEntitiesReasonQueryService claimsRelatedEntitiesReasonQuery = new ClaimsRelatedEntitiesReasonQueryService(MyContext);
            ClaimsRelatedEntitiesReasonPM claimsRelatedEntitiesReasonPM = claimsRelatedEntitiesReasonQuery.GetSingle(claimid, counterkey, reasonlisttypecode,false,false);
            return claimsRelatedEntitiesReasonPM;
           
        }

         
		public ClaimsRelatedEntitiesReasonList GetSingleClaimsRelatedEntitiesReasonList(string claimid, int counterkey, string reasonlisttypecode,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.ClaimsRelatedEntitiesReason", "READ", tenant);

            if ( MyContext == null)
            {
                 MyContext = CustomContext.GetContext(tenant);
            }

          
            ClaimsRelatedEntitiesReasonListQueryService listService = new ClaimsRelatedEntitiesReasonListQueryService(MyContext);
            return listService.GetSingle(claimid, counterkey, reasonlisttypecode);
        }

		public List<ClaimsRelatedEntitiesReasonList> GetClaimsRelatedEntitiesReasonLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.ClaimsRelatedEntitiesReason", "READ", tenant);
            if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }
            ClaimsRelatedEntitiesReasonListQueryService listService = new ClaimsRelatedEntitiesReasonListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<ClaimsRelatedEntitiesReasonList> GetClaimsRelatedEntitiesReasonsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.ClaimsRelatedEntitiesReason", "READ", tenant);

            if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            ClaimsRelatedEntitiesReasonListQueryService listService = new ClaimsRelatedEntitiesReasonListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetClaimsRelatedEntitiesReasonFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.ClaimsRelatedEntitiesReason", "READ", tenant);
            if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            ClaimsRelatedEntitiesReasonListQueryService queryService = new ClaimsRelatedEntitiesReasonListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }
		
	    public void InsertClaimsRelatedEntitiesReason(ClaimsRelatedEntitiesReasonPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("ClaimsRelatedEntitiesReason", "NEW", entityPM.Tenant);
			if ( MyContext == null)
            {
                MyContext = CustomContext.GetContext(entityPM.Tenant);
            }; 
            ClaimsRelatedEntitiesReasonUpdateService service = new ClaimsRelatedEntitiesReasonUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
					    service.Update(entityPM,true); 

			ObjectTabelRepository objectTabelRepository = new ObjectTabelRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("ClaimsRelatedEntitiesReason", 0, true);
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
			    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id); 
            } 
        }


		public void UpdateClaimsRelatedEntitiesReason(ClaimsRelatedEntitiesReasonPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("ClaimsRelatedEntitiesReason", "UPDATE", entityPM.Tenant);
			if ( MyContext == null)
            {
                MyContext = CustomContext.GetContext(entityPM.Tenant);
            }; 
            ClaimsRelatedEntitiesReasonUpdateService service = new ClaimsRelatedEntitiesReasonUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
						 
		    service.Update(entityPM,true);

			ObjectTabelRepository objectTabelRepository = new ObjectTabelRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("ClaimsRelatedEntitiesReason", 0, true);
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
	 