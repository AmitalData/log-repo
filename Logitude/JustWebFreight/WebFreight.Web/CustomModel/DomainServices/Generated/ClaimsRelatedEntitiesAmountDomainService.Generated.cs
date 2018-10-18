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
    public partial class ClaimsRelatedEntitiesAmountDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<ClaimsRelatedEntitiesAmountPM> service;
		ICustomContext MyContext;
     	public ClaimsRelatedEntitiesAmountDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<ClaimsRelatedEntitiesAmountPM>), "CustomDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<ClaimsRelatedEntitiesAmountPM>;
		}
       
        public ClaimsRelatedEntitiesAmountPM GetSingleClaimsRelatedEntitiesAmountPM(string claimid, int counterkey, int lineno,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }

            ClaimsRelatedEntitiesAmountQueryService claimsRelatedEntitiesAmountQuery = new ClaimsRelatedEntitiesAmountQueryService(MyContext);
            ClaimsRelatedEntitiesAmountPM claimsRelatedEntitiesAmountPM = claimsRelatedEntitiesAmountQuery.GetSingle(claimid, counterkey, lineno,false,false);
            return claimsRelatedEntitiesAmountPM;
           
        }

         
		public ClaimsRelatedEntitiesAmountList GetSingleClaimsRelatedEntitiesAmountList(string claimid, int counterkey, int lineno,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.ClaimsRelatedEntitiesAmount", "READ", tenant);

            if ( MyContext == null)
            {
                 MyContext = CustomContext.GetContext(tenant);
            }

          
            ClaimsRelatedEntitiesAmountListQueryService listService = new ClaimsRelatedEntitiesAmountListQueryService(MyContext);
            return listService.GetSingle(claimid, counterkey, lineno);
        }

		public List<ClaimsRelatedEntitiesAmountList> GetClaimsRelatedEntitiesAmountLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.ClaimsRelatedEntitiesAmount", "READ", tenant);
            if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }
            ClaimsRelatedEntitiesAmountListQueryService listService = new ClaimsRelatedEntitiesAmountListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<ClaimsRelatedEntitiesAmountList> GetClaimsRelatedEntitiesAmountsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.ClaimsRelatedEntitiesAmount", "READ", tenant);

            if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            ClaimsRelatedEntitiesAmountListQueryService listService = new ClaimsRelatedEntitiesAmountListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetClaimsRelatedEntitiesAmountFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.ClaimsRelatedEntitiesAmount", "READ", tenant);
            if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            ClaimsRelatedEntitiesAmountListQueryService queryService = new ClaimsRelatedEntitiesAmountListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }
		
	    public void InsertClaimsRelatedEntitiesAmount(ClaimsRelatedEntitiesAmountPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("ClaimsRelatedEntitiesAmount", "NEW", entityPM.Tenant);
			if ( MyContext == null)
            {
                MyContext = CustomContext.GetContext(entityPM.Tenant);
            }; 
            ClaimsRelatedEntitiesAmountUpdateService service = new ClaimsRelatedEntitiesAmountUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
					    service.Update(entityPM,true); 

			ObjectTabelRepository objectTabelRepository = new ObjectTabelRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("ClaimsRelatedEntitiesAmount", 0, true);
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
			    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id); 
            } 
        }


		public void UpdateClaimsRelatedEntitiesAmount(ClaimsRelatedEntitiesAmountPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("ClaimsRelatedEntitiesAmount", "UPDATE", entityPM.Tenant);
			if ( MyContext == null)
            {
                MyContext = CustomContext.GetContext(entityPM.Tenant);
            }; 
            ClaimsRelatedEntitiesAmountUpdateService service = new ClaimsRelatedEntitiesAmountUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
						 
		    service.Update(entityPM,true);

			ObjectTabelRepository objectTabelRepository = new ObjectTabelRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("ClaimsRelatedEntitiesAmount", 0, true);
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
	 