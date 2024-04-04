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
    public partial class RemarksClassificationDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<RemarksClassificationPM> service;
		ICustomContext MyContext;
     	public RemarksClassificationDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<RemarksClassificationPM>), "CustomDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<RemarksClassificationPM>;
		}
       
        public RemarksClassificationPM GetSingleRemarksClassificationPM(string cb_id,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }

            RemarksClassificationQueryService remarksClassificationQuery = new RemarksClassificationQueryService(MyContext);
            RemarksClassificationPM remarksClassificationPM = remarksClassificationQuery.GetSingle(cb_id,false,false);
            return remarksClassificationPM;
           
        }

         
		public RemarksClassificationList GetSingleRemarksClassificationList(string cb_id,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			             SecurityUtility.CheckContactFeature("Customs.RemarksClassification", "READ", tenant); if ( MyContext == null)
            {
                 MyContext = CustomContext.GetContext(tenant);
            }

          
            RemarksClassificationListQueryService listService = new RemarksClassificationListQueryService(MyContext);
            return listService.GetSingle(cb_id);
        }

		public List<RemarksClassificationList> GetRemarksClassificationLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			             SecurityUtility.CheckContactFeature("Customs.RemarksClassification", "READ", tenant);if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }
            RemarksClassificationListQueryService listService = new RemarksClassificationListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<RemarksClassificationList> GetRemarksClassificationFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			            SecurityUtility.CheckContactFeature("Customs.RemarksClassification", "READ", tenant);
if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            RemarksClassificationListQueryService listService = new RemarksClassificationListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetRemarksClassificationFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			            SecurityUtility.CheckContactFeature("Customs.RemarksClassification", "READ", tenant);if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            RemarksClassificationListQueryService queryService = new RemarksClassificationListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }
		
	    public void InsertRemarksClassification(RemarksClassificationPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
			            SecurityUtility.CheckContactFeature("RemarksClassification", "NEW", entityPM.Tenant);if ( MyContext == null)
            {
                MyContext = CustomContext.GetContext(entityPM.Tenant);
            }; 
            RemarksClassificationUpdateService service = new RemarksClassificationUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
					    service.Update(entityPM,true); 

			ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("RemarksClassification", 0, true);
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
			    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id); 
            } 
        }


		public void UpdateRemarksClassification(RemarksClassificationPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
			            SecurityUtility.CheckContactFeature("RemarksClassification", "UPDATE", entityPM.Tenant);if ( MyContext == null)
            {
                MyContext = CustomContext.GetContext(entityPM.Tenant);
            }; 
            RemarksClassificationUpdateService service = new RemarksClassificationUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
						 
		    service.Update(entityPM,true);

			ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("RemarksClassification", 0, true);
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
	 