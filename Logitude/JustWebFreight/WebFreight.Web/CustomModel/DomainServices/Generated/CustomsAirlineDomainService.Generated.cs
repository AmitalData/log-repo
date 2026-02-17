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
    public partial class CustomsAirlineDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<CustomsAirlinePM> service;
		ICustomContext MyContext;
     	public CustomsAirlineDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<CustomsAirlinePM>), "CustomDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<CustomsAirlinePM>;
		}
       
        public CustomsAirlinePM GetSingleCustomsAirlinePM(string id,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }

            CustomsAirlineQueryService customsAirlineQuery = new CustomsAirlineQueryService(MyContext);
            CustomsAirlinePM customsAirlinePM = customsAirlineQuery.GetSingle(id,false,false);
            return customsAirlinePM;
           
        }

         
		public CustomsAirlineList GetSingleCustomsAirlineList(string id,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			             SecurityUtility.CheckContactFeature("Customs.CustomsAirline", "READ", tenant); if ( MyContext == null)
            {
                 MyContext = CustomContext.GetContext(tenant);
            }

          
            CustomsAirlineListQueryService listService = new CustomsAirlineListQueryService(MyContext);
            return listService.GetSingle(id);
        }

		public List<CustomsAirlineList> GetCustomsAirlineLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			             SecurityUtility.CheckContactFeature("Customs.CustomsAirline", "READ", tenant);if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }
            CustomsAirlineListQueryService listService = new CustomsAirlineListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<CustomsAirlineList> GetCustomsAirlineFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			            SecurityUtility.CheckContactFeature("Customs.CustomsAirline", "READ", tenant);
if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            CustomsAirlineListQueryService listService = new CustomsAirlineListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetCustomsAirlineFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			            SecurityUtility.CheckContactFeature("Customs.CustomsAirline", "READ", tenant);if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            CustomsAirlineListQueryService queryService = new CustomsAirlineListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }
		
	    public void InsertCustomsAirline(CustomsAirlinePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
			            SecurityUtility.CheckContactFeature("CustomsAirline", "NEW", entityPM.Tenant);if ( MyContext == null)
            {
                MyContext = CustomContext.GetContext(entityPM.Tenant);
            }; 
            CustomsAirlineUpdateService service = new CustomsAirlineUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
					    service.Update(entityPM,true); 

			ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("CustomsAirline", 0, true);
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
			    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id); 
            } 
        }


		public void UpdateCustomsAirline(CustomsAirlinePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
			            SecurityUtility.CheckContactFeature("CustomsAirline", "UPDATE", entityPM.Tenant);if ( MyContext == null)
            {
                MyContext = CustomContext.GetContext(entityPM.Tenant);
            }; 
            CustomsAirlineUpdateService service = new CustomsAirlineUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
						 
		    service.Update(entityPM,true);

			ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("CustomsAirline", 0, true);
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
	 