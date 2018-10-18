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
    public partial class SLAHeaderDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<SLAHeaderPM> service;
		ICRMContext MyContext;
     	public SLAHeaderDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<SLAHeaderPM>), "CRMDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<SLAHeaderPM>;
		}
       
        public SLAHeaderPM GetSingleSLAHeaderPM(string id,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            }

            SLAHeaderQueryService sLAHeaderQuery = new SLAHeaderQueryService(MyContext);
            SLAHeaderPM sLAHeaderPM = sLAHeaderQuery.GetSingle(id,false,false);
            return sLAHeaderPM;
           
        }

         
		public SLAHeaderList GetSingleSLAHeaderList(string id,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			             SecurityUtility.CheckContactFeature("SLAHeader", "READ", tenant); if ( MyContext == null)
            {
                 MyContext = CRMContext.GetContext(tenant);
            }

          
            SLAHeaderListQueryService listService = new SLAHeaderListQueryService(MyContext);
            return listService.GetSingle(id);
        }

		public List<SLAHeaderList> GetSLAHeaderLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			             SecurityUtility.CheckContactFeature("SLAHeader", "READ", tenant);if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            }
            SLAHeaderListQueryService listService = new SLAHeaderListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<SLAHeaderList> GetSLAHeaderFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			            SecurityUtility.CheckContactFeature("SLAHeader", "READ", tenant);
if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            };
            SLAHeaderListQueryService listService = new SLAHeaderListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetSLAHeaderFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			            SecurityUtility.CheckContactFeature("SLAHeader", "READ", tenant);if ( MyContext == null)
            {
                  MyContext = CRMContext.GetContext(tenant);
            };
            SLAHeaderListQueryService queryService = new SLAHeaderListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }
		
	    public void InsertSLAHeader(SLAHeaderPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
			            SecurityUtility.CheckContactFeature("SLAHeader", "NEW", entityPM.Tenant);if ( MyContext == null)
            {
                MyContext = CRMContext.GetContext(entityPM.Tenant);
            }; 
            SLAHeaderUpdateService service = new SLAHeaderUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
			
            List<SLALinePM> SLALinesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.SLALines).Cast<SLALinePM>().ToList();
            foreach (SLALinePM SLALine in SLALinesChangeSet)
            {  
                entityPM.SLALines.Where(d => d.Id == SLALine.Id).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;  
            }
        
		
            List<SLAEscalationPM> SLAEscalationsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.SLAEscalations).Cast<SLAEscalationPM>().ToList();
            foreach (SLAEscalationPM SLALine in SLAEscalationsChangeSet)
            {  
                entityPM.SLAEscalations.Where(d => d.Id == SLALine.Id).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert; 
            List<SLAEscalationRecepientPM> SLAEscalationRecepientsChangeSet = ChangeSet.GetAssociatedChanges(SLAEscalation, d => d.SLAEscalationRecepients).Cast<SLAEscalationRecepientPM>().ToList();
            foreach (SLAEscalationRecepientPM SLAEscalationRecepient in SLAEscalationRecepientsChangeSet)
            {  
                SLAEscalation.SLAEscalationRecepients.Where(d => d.Id == SLAEscalationRecepient.Id).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;  
            }
        
		 
            }
        
		
            List<SLALinePM> SLALinesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.SLALines).Cast<SLALinePM>().ToList();
            foreach (SLALinePM SLAEscalation in SLALinesChangeSet)
            {  
                entityPM.SLALines.Where(d => d.Id == SLAEscalation.Id).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;  
            }
        
		
            List<SLAEscalationPM> SLAEscalationsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.SLAEscalations).Cast<SLAEscalationPM>().ToList();
            foreach (SLAEscalationPM SLAEscalation in SLAEscalationsChangeSet)
            {  
                entityPM.SLAEscalations.Where(d => d.Id == SLAEscalation.Id).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert; 
            List<SLAEscalationRecepientPM> SLAEscalationRecepientsChangeSet = ChangeSet.GetAssociatedChanges(SLAEscalation, d => d.SLAEscalationRecepients).Cast<SLAEscalationRecepientPM>().ToList();
            foreach (SLAEscalationRecepientPM SLAEscalationRecepient in SLAEscalationRecepientsChangeSet)
            {  
                SLAEscalation.SLAEscalationRecepients.Where(d => d.Id == SLAEscalationRecepient.Id).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;  
            }
        
		 
            }
        
				    service.Update(entityPM,true); 

			ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("SLAHeader", 0, true);
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
			    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id); 
            } 
        }


		public void UpdateSLAHeader(SLAHeaderPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
			            SecurityUtility.CheckContactFeature("SLAHeader", "UPDATE", entityPM.Tenant);if ( MyContext == null)
            {
                MyContext = CRMContext.GetContext(entityPM.Tenant);
            }; 
            SLAHeaderUpdateService service = new SLAHeaderUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
			
			SetSLALineChangeSet(entityPM); 
			SetSLAEscalationChangeSet(entityPM); 			 
		    service.Update(entityPM,true);

			ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("SLAHeader", 0, true);
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
			    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id); 
            } 
        } 

		
			 
		private void SetSLALineChangeSet(SLAHeaderPM entityPM)
        {
            List<SLALinePM> SLALinesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.SLALines).Cast<SLALinePM>().ToList();

            foreach (SLALinePM itemPM in SLALinesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        { 
                            SLALinePM currentItemPM = entityPM.SLALines.Where(d => d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;                            
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                           SLALinePM currentItemPM = entityPM.SLALines.Where(d => d.Id == itemPM.Id).FirstOrDefault();
						    currentItemPM.ChangeSetOp = ChangeSetOperation.Update; 
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            SLALinePM currentItemPM = new SLALinePM()
                            {
                                ChangeSetOp = ChangeSetOperation.Delete, 
		                	    Id = itemPM.Id,  

                            };

                            entityPM.DeletedSLALines.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                           SLALinePM currentItemPM = entityPM.SLALines.Where(d => d.Id == itemPM.Id).FirstOrDefault();
						   currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                           break;
                        }
                }
            }
        } 
		
		private void SetSLAEscalationChangeSet(SLAHeaderPM entityPM)
        {
            List<SLAEscalationPM> SLAEscalationsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.SLAEscalations).Cast<SLAEscalationPM>().ToList();

            foreach (SLAEscalationPM itemPM in SLAEscalationsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        { 
                            SLAEscalationPM currentItemPM = entityPM.SLAEscalations.Where(d => d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert; 
		                	SetSLAEscalationRecepientChangeSet(currentItemPM);
			                                           
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                           SLAEscalationPM currentItemPM = entityPM.SLAEscalations.Where(d => d.Id == itemPM.Id).FirstOrDefault();
						    currentItemPM.ChangeSetOp = ChangeSetOperation.Update; 
		                	SetSLAEscalationRecepientChangeSet(currentItemPM);
			                
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            SLAEscalationPM currentItemPM = new SLAEscalationPM()
                            {
                                ChangeSetOp = ChangeSetOperation.Delete, 
		                	    Id = itemPM.Id,  

                            };

                            entityPM.DeletedSLAEscalations.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                           SLAEscalationPM currentItemPM = entityPM.SLAEscalations.Where(d => d.Id == itemPM.Id).FirstOrDefault();
						   currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                           break;
                        }
                }
            }
        } 
		
		private void SetSLAEscalationRecepientChangeSet(SLAEscalationPM entityPM)
        {
            List<SLAEscalationRecepientPM> SLAEscalationRecepientsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.SLAEscalationRecepients).Cast<SLAEscalationRecepientPM>().ToList();

            foreach (SLAEscalationRecepientPM itemPM in SLAEscalationRecepientsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        { 
                            SLAEscalationRecepientPM currentItemPM = entityPM.SLAEscalationRecepients.Where(d => d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;                            
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                           SLAEscalationRecepientPM currentItemPM = entityPM.SLAEscalationRecepients.Where(d => d.Id == itemPM.Id).FirstOrDefault();
						    currentItemPM.ChangeSetOp = ChangeSetOperation.Update; 
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            SLAEscalationRecepientPM currentItemPM = new SLAEscalationRecepientPM()
                            {
                                ChangeSetOp = ChangeSetOperation.Delete, 
		                	    Id = itemPM.Id,  

                            };

                            entityPM.DeletedSLAEscalationRecepients.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                           SLAEscalationRecepientPM currentItemPM = entityPM.SLAEscalationRecepients.Where(d => d.Id == itemPM.Id).FirstOrDefault();
						   currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                           break;
                        }
                }
            }
        } 
				
      
    }
}
	 