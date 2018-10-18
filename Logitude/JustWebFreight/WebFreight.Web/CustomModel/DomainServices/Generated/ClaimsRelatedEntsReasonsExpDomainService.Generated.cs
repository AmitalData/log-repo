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
    public partial class ClaimsRelatedEntsReasonsExpDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<ClaimsRelatedEntsReasonsExpPM> service;
		ICustomContext MyContext;
     	public ClaimsRelatedEntsReasonsExpDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<ClaimsRelatedEntsReasonsExpPM>), "CustomDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<ClaimsRelatedEntsReasonsExpPM>;
		}
       
        public ClaimsRelatedEntsReasonsExpPM GetSingleClaimsRelatedEntsReasonsExpPM(string claimid, int counterkey, string reasonlisttypecode, string claimexplanationtypecode,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }

            ClaimsRelatedEntsReasonsExpQueryService claimsRelatedEntsReasonsExpQuery = new ClaimsRelatedEntsReasonsExpQueryService(MyContext);
            ClaimsRelatedEntsReasonsExpPM claimsRelatedEntsReasonsExpPM = claimsRelatedEntsReasonsExpQuery.GetSingle(claimid, counterkey, reasonlisttypecode, claimexplanationtypecode,false,false);
            return claimsRelatedEntsReasonsExpPM;
           
        }

         
		public ClaimsRelatedEntsReasonsExpList GetSingleClaimsRelatedEntsReasonsExpList(string claimid, int counterkey, string reasonlisttypecode, string claimexplanationtypecode,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.ClaimsRelatedEntsReasonsExp", "READ", tenant);

            if ( MyContext == null)
            {
                 MyContext = CustomContext.GetContext(tenant);
            }

          
            ClaimsRelatedEntsReasonsExpListQueryService listService = new ClaimsRelatedEntsReasonsExpListQueryService(MyContext);
            return listService.GetSingle(claimid, counterkey, reasonlisttypecode, claimexplanationtypecode);
        }

		public List<ClaimsRelatedEntsReasonsExpList> GetClaimsRelatedEntsReasonsExpLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.ClaimsRelatedEntsReasonsExp", "READ", tenant);
            if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }
            ClaimsRelatedEntsReasonsExpListQueryService listService = new ClaimsRelatedEntsReasonsExpListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<ClaimsRelatedEntsReasonsExpList> GetClaimsRelatedEntsReasonsExpsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.ClaimsRelatedEntsReasonsExp", "READ", tenant);

            if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            ClaimsRelatedEntsReasonsExpListQueryService listService = new ClaimsRelatedEntsReasonsExpListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetClaimsRelatedEntsReasonsExpFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.ClaimsRelatedEntsReasonsExp", "READ", tenant);
            if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            ClaimsRelatedEntsReasonsExpListQueryService queryService = new ClaimsRelatedEntsReasonsExpListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }
		
	    public void InsertClaimsRelatedEntsReasonsExp(ClaimsRelatedEntsReasonsExpPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("ClaimsRelatedEntsReasonsExp", "NEW", entityPM.Tenant);
			if ( MyContext == null)
            {
                MyContext = CustomContext.GetContext(entityPM.Tenant);
            }; 
            ClaimsRelatedEntsReasonsExpUpdateService service = new ClaimsRelatedEntsReasonsExpUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
					    service.Update(entityPM,true); 

			ObjectTabelRepository objectTabelRepository = new ObjectTabelRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("ClaimsRelatedEntsReasonsExp", 0, true);
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
			    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id); 
            } 
        }


		public void UpdateClaimsRelatedEntsReasonsExp(ClaimsRelatedEntsReasonsExpPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("ClaimsRelatedEntsReasonsExp", "UPDATE", entityPM.Tenant);
			if ( MyContext == null)
            {
                MyContext = CustomContext.GetContext(entityPM.Tenant);
            }; 
            ClaimsRelatedEntsReasonsExpUpdateService service = new ClaimsRelatedEntsReasonsExpUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
						 
		    service.Update(entityPM,true);

			ObjectTabelRepository objectTabelRepository = new ObjectTabelRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("ClaimsRelatedEntsReasonsExp", 0, true);
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
	 