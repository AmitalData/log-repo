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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.BL.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.BL.EntityQueryServices;

namespace WebFreight.Web.AccountingModel.DomainServices
{ 

    [EnableClientAccess()]
    public partial class DepositDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<DepositPM> service;
		IAccountingContext MyContext;
     	public DepositDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<DepositPM>), "AccountingDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<DepositPM>;
		}
       
        public DepositPM GetSingleDepositPM(string id,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = AccountingContext.GetContext(tenant);
            }

            DepositQueryService depositQuery = new DepositQueryService(MyContext);
            DepositPM depositPM = depositQuery.GetSingle(id,false,false);
            return depositPM;
           
        }

         
		public DepositList GetSingleDepositList(string id,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Deposit", "READ", tenant);

            if ( MyContext == null)
            {
                 MyContext = AccountingContext.GetContext(tenant);
            }

          
            DepositListQueryService listService = new DepositListQueryService(MyContext);
            return listService.GetSingle(id);
        }

		public List<DepositList> GetDepositLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Deposit", "READ", tenant);
            if ( MyContext == null)
            {
                  MyContext = AccountingContext.GetContext(tenant);
            }
            DepositListQueryService listService = new DepositListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<DepositList> GetDepositsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Deposit", "READ", tenant);

            if ( MyContext == null)
            {
                  MyContext = AccountingContext.GetContext(tenant);
            };
            DepositListQueryService listService = new DepositListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetDepositFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Deposit", "READ", tenant);
            if ( MyContext == null)
            {
                  MyContext = AccountingContext.GetContext(tenant);
            };
            DepositListQueryService queryService = new DepositListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }
		
	    public void InsertDeposit(DepositPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("Deposit", "NEW", entityPM.Tenant);
			if ( MyContext == null)
            {
                MyContext = AccountingContext.GetContext(entityPM.Tenant);
            }; 
            DepositUpdateService service = new DepositUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
					    service.Update(entityPM,true); 

			ObjectTabelRepository objectTabelRepository = new ObjectTabelRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Deposit", 0, true);
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
			    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id); 
            } 
        }


		public void UpdateDeposit(DepositPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("Deposit", "UPDATE", entityPM.Tenant);
			if ( MyContext == null)
            {
                MyContext = AccountingContext.GetContext(entityPM.Tenant);
            }; 
            DepositUpdateService service = new DepositUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
						 
		    service.Update(entityPM,true);

			ObjectTabelRepository objectTabelRepository = new ObjectTabelRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Deposit", 0, true);
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
	 