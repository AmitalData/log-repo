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
    public partial class EmployeeGroupDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<EmployeeGroupPM> service;
		ICRMContext MyContext;
     	public EmployeeGroupDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<EmployeeGroupPM>), "CRMDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<EmployeeGroupPM>;
		}
       
        public EmployeeGroupPM GetSingleEmployeeGroupPM(string id,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            }

            EmployeeGroupQueryService employeeGroupQuery = new EmployeeGroupQueryService(MyContext);
            EmployeeGroupPM employeeGroupPM = employeeGroupQuery.GetSingle(id,false,false);
            return employeeGroupPM;
           
        }

         
		public EmployeeGroupList GetSingleEmployeeGroupList(string id,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			             SecurityUtility.CheckContactFeature("EmployeeGroup", "READ", tenant); if ( MyContext == null)
            {
                 MyContext = CRMContext.GetContext(tenant);
            }

          
            EmployeeGroupListQueryService listService = new EmployeeGroupListQueryService(MyContext);
            return listService.GetSingle(id);
        }

		public List<EmployeeGroupList> GetEmployeeGroupLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			             SecurityUtility.CheckContactFeature("EmployeeGroup", "READ", tenant);if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            }
            EmployeeGroupListQueryService listService = new EmployeeGroupListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<EmployeeGroupList> GetEmployeeGroupFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			            SecurityUtility.CheckContactFeature("EmployeeGroup", "READ", tenant);
if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            };
            EmployeeGroupListQueryService listService = new EmployeeGroupListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetEmployeeGroupFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			            SecurityUtility.CheckContactFeature("EmployeeGroup", "READ", tenant);if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            };
            EmployeeGroupListQueryService queryService = new EmployeeGroupListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }
		
	    public void InsertEmployeeGroup(EmployeeGroupPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
			            SecurityUtility.CheckContactFeature("EmployeeGroup", "NEW", entityPM.Tenant);if ( MyContext == null)
            {
                MyContext = CRMContext.GetContext(entityPM.Tenant);
            }; 
            EmployeeGroupUpdateService service = new EmployeeGroupUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
			
            List<EmployeeGroupLinePM> EmployeeGroupLinesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.EmployeeGroupLines).Cast<EmployeeGroupLinePM>().ToList();
            foreach (EmployeeGroupLinePM EmployeeGroupLine in EmployeeGroupLinesChangeSet)
            {  
                entityPM.EmployeeGroupLines.Where(d => d.Id == EmployeeGroupLine.Id).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;  
            }
        
				    service.Update(entityPM,true); 

			ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("EmployeeGroup", 0, true);
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
			    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id); 
            } 
        }


		public void UpdateEmployeeGroup(EmployeeGroupPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
			            SecurityUtility.CheckContactFeature("EmployeeGroup", "UPDATE", entityPM.Tenant);if ( MyContext == null)
            {
                MyContext = CRMContext.GetContext(entityPM.Tenant);
            }; 
            EmployeeGroupUpdateService service = new EmployeeGroupUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
			
			SetEmployeeGroupLineChangeSet(entityPM); 			 
		    service.Update(entityPM,true);

			ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("EmployeeGroup", 0, true);
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
			    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id); 
            } 
        } 

		
			 
		private void SetEmployeeGroupLineChangeSet(EmployeeGroupPM entityPM)
        {
            List<EmployeeGroupLinePM> EmployeeGroupLinesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.EmployeeGroupLines).Cast<EmployeeGroupLinePM>().ToList();

            foreach (EmployeeGroupLinePM itemPM in EmployeeGroupLinesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        { 
                            EmployeeGroupLinePM currentItemPM = entityPM.EmployeeGroupLines.Where(d => d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;                            
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                           EmployeeGroupLinePM currentItemPM = entityPM.EmployeeGroupLines.Where(d => d.Id == itemPM.Id).FirstOrDefault();
						    currentItemPM.ChangeSetOp = ChangeSetOperation.Update; 
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            EmployeeGroupLinePM currentItemPM = new EmployeeGroupLinePM()
                            {
                                ChangeSetOp = ChangeSetOperation.Delete, 
		                	    Id = itemPM.Id,  

                            };

                            entityPM.DeletedEmployeeGroupLines.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                           EmployeeGroupLinePM currentItemPM = entityPM.EmployeeGroupLines.Where(d => d.Id == itemPM.Id).FirstOrDefault();
						   currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                           break;
                        }
                }
            }
        } 
				
      
    }
}
	 