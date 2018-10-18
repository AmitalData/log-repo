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
    public partial class SLALineDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<SLALinePM> service;
		ICRMContext MyContext;
     	public SLALineDomainService()
		{ 
		  service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<SLALinePM>), "CRMDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<SLALinePM>;
		}
       
        public SLALinePM GetSingleSLALinePM(string id,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            }

            SLALineQueryService sLALineQuery = new SLALineQueryService(MyContext);
            SLALinePM sLALinePM = sLALineQuery.GetSingle(id,false,false);
            return sLALinePM;
           
        }

         
		public SLALineList GetSingleSLALineList(string id,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("SLALine", "READ", tenant);

            if ( MyContext == null)
            {
                 MyContext = CRMContext.GetContext(tenant);
            }

          
            SLALineListQueryService listService = new SLALineListQueryService(MyContext);
            return listService.GetSingle(id);
        }

		public List<SLALineList> GetSLALineLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("SLALine", "READ", tenant);
            if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            }
            SLALineListQueryService listService = new SLALineListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<SLALineList> GetSLALinesFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("SLALine", "READ", tenant);

            if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            };
            SLALineListQueryService listService = new SLALineListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetSLALineFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("SLALine", "READ", tenant);
            if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            };
            SLALineListQueryService queryService = new SLALineListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }
		
	    public void InsertSLALine(SLALinePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("SLALine", "NEW", entityPM.Tenant);
			if ( MyContext == null)
            {
                MyContext = CRMContext.GetContext(entityPM.Tenant);
            }; 
            SLALineUpdateService service = new SLALineUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
					    service.Update(entityPM,true); 

			ObjectTabelRepository objectTabelRepository = new ObjectTabelRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("SLALine", 0, true);
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
			    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id); 
            } 
        }


		public void UpdateSLALine(SLALinePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("SLALine", "UPDATE", entityPM.Tenant);
			if ( MyContext == null)
            {
                MyContext = CRMContext.GetContext(entityPM.Tenant);
            }; 
            SLALineUpdateService service = new SLALineUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
						 
		    service.Update(entityPM,true);

			ObjectTabelRepository objectTabelRepository = new ObjectTabelRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("SLALine", 0, true);
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
	 