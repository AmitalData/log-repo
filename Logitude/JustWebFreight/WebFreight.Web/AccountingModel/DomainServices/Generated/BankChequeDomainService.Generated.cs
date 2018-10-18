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
    public partial class BankChequeDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<BankChequePM> service;
		IAccountingContext MyContext;
     	public BankChequeDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<BankChequePM>), "AccountingDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<BankChequePM>;
		}
       
        public BankChequePM GetSingleBankChequePM(string id,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = AccountingContext.GetContext(tenant);
            }

            BankChequeQueryService bankChequeQuery = new BankChequeQueryService(MyContext);
            BankChequePM bankChequePM = bankChequeQuery.GetSingle(id,false,false);
            return bankChequePM;
           
        }

         
		public BankChequeList GetSingleBankChequeList(string id,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("BankCheque", "READ", tenant);

            if ( MyContext == null)
            {
                 MyContext = AccountingContext.GetContext(tenant);
            }

          
            BankChequeListQueryService listService = new BankChequeListQueryService(MyContext);
            return listService.GetSingle(id);
        }

		public List<BankChequeList> GetBankChequeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("BankCheque", "READ", tenant);
            if ( MyContext == null)
            {
                  MyContext = AccountingContext.GetContext(tenant);
            }
            BankChequeListQueryService listService = new BankChequeListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<BankChequeList> GetBankChequesFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("BankCheque", "READ", tenant);

            if ( MyContext == null)
            {
                  MyContext = AccountingContext.GetContext(tenant);
            };
            BankChequeListQueryService listService = new BankChequeListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetBankChequeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("BankCheque", "READ", tenant);
            if ( MyContext == null)
            {
                  MyContext = AccountingContext.GetContext(tenant);
            };
            BankChequeListQueryService queryService = new BankChequeListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }
		
	    public void InsertBankCheque(BankChequePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("BankCheque", "NEW", entityPM.Tenant);
			if ( MyContext == null)
            {
                MyContext = AccountingContext.GetContext(entityPM.Tenant);
            }; 
            BankChequeUpdateService service = new BankChequeUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
					    service.Update(entityPM,true); 

			ObjectTabelRepository objectTabelRepository = new ObjectTabelRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("BankCheque", 0, true);
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
			    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id); 
            } 
        }


		public void UpdateBankCheque(BankChequePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("BankCheque", "UPDATE", entityPM.Tenant);
			if ( MyContext == null)
            {
                MyContext = AccountingContext.GetContext(entityPM.Tenant);
            }; 
            BankChequeUpdateService service = new BankChequeUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
						 
		    service.Update(entityPM,true);

			ObjectTabelRepository objectTabelRepository = new ObjectTabelRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("BankCheque", 0, true);
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
	 