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
    public partial class DeclarationCargoSplitDomainService : LogitudeDomainService
    {
	    IDomainServiceUpdateClass<DeclarationCargoSplitPM> service;
		ICustomContext MyContext;
     	public DeclarationCargoSplitDomainService()
		{ 
		  //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<DeclarationCargoSplitPM>), "CustomDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<DeclarationCargoSplitPM>;
		}
       
        public DeclarationCargoSplitPM GetSingleDeclarationCargoSplitPM(string id,int tenant)
        {
            if (MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }

            DeclarationCargoSplitQueryService declarationCargoSplitQuery = new DeclarationCargoSplitQueryService(MyContext);
            DeclarationCargoSplitPM declarationCargoSplitPM = declarationCargoSplitQuery.GetSingle(id,false,false);
            return declarationCargoSplitPM;
           
        }

         
		public DeclarationCargoSplitList GetSingleDeclarationCargoSplitList(string id,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			             SecurityUtility.CheckContactFeature("Customs.DeclarationCargoSplit", "READ", tenant); if ( MyContext == null)
            {
                 MyContext = CustomContext.GetContext(tenant);
            }

          
            DeclarationCargoSplitListQueryService listService = new DeclarationCargoSplitListQueryService(MyContext);
            return listService.GetSingle(id);
        }

		public List<DeclarationCargoSplitList> GetDeclarationCargoSplitLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			             SecurityUtility.CheckContactFeature("Customs.DeclarationCargoSplit", "READ", tenant);if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            }
            DeclarationCargoSplitListQueryService listService = new DeclarationCargoSplitListQueryService(MyContext);
            return listService.GetList(tenant);
        }
       
	    public List<DeclarationCargoSplitList> GetDeclarationCargoSplitFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			            SecurityUtility.CheckContactFeature("Customs.DeclarationCargoSplit", "READ", tenant);
if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            DeclarationCargoSplitListQueryService listService = new DeclarationCargoSplitListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

	    public int GetDeclarationCargoSplitFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
			            SecurityUtility.CheckContactFeature("Customs.DeclarationCargoSplit", "READ", tenant);if ( MyContext == null)
            {
                  MyContext = CustomContext.GetContext(tenant);
            };
            DeclarationCargoSplitListQueryService queryService = new DeclarationCargoSplitListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }
		
	    public void InsertDeclarationCargoSplit(DeclarationCargoSplitPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
			            SecurityUtility.CheckContactFeature("DeclarationCargoSplit", "NEW", entityPM.Tenant);if ( MyContext == null)
            {
                MyContext = CustomContext.GetContext(entityPM.Tenant);
            }; 
            DeclarationCargoSplitUpdateService service = new DeclarationCargoSplitUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
			
            List<DecCargoSplitConPM> DecCargoSplitConsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.DecCargoSplitCons).Cast<DecCargoSplitConPM>().ToList();
            foreach (DecCargoSplitConPM DecCargoSplitCon in DecCargoSplitConsChangeSet)
            {  
                entityPM.DecCargoSplitCons.Where(d => d.DeclarationCargoSplitId == DecCargoSplitCon.DeclarationCargoSplitId && d.LineNumber == DecCargoSplitCon.LineNumber).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert; 
            List<DecCargoSplitConsItemPM> DecCargoSplitConsItemsChangeSet = ChangeSet.GetAssociatedChanges(DecCargoSplitCon, d => d.DecCargoSplitConsItems).Cast<DecCargoSplitConsItemPM>().ToList();
            foreach (DecCargoSplitConsItemPM DecCargoSplitConsItem in DecCargoSplitConsItemsChangeSet)
            {  
                DecCargoSplitCon.DecCargoSplitConsItems.Where(d => d.DeclarationCargoSplitId == DecCargoSplitConsItem.DeclarationCargoSplitId && d.DecCargoSplitConsLineNo == DecCargoSplitConsItem.DecCargoSplitConsLineNo && d.ItemLine == DecCargoSplitConsItem.ItemLine).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert; 
            List<DecCargoSplitConsPackDetPM> DecCargoSplitConsPackDetsChangeSet = ChangeSet.GetAssociatedChanges(DecCargoSplitConsItem, d => d.DecCargoSplitConsPackDets).Cast<DecCargoSplitConsPackDetPM>().ToList();
            foreach (DecCargoSplitConsPackDetPM DecCargoSplitConsPackDet in DecCargoSplitConsPackDetsChangeSet)
            {  
                DecCargoSplitConsItem.DecCargoSplitConsPackDets.Where(d => d.DeclarationCargoSplitId == DecCargoSplitConsPackDet.DeclarationCargoSplitId && d.DecCargoSplitConsLineNo == DecCargoSplitConsPackDet.DecCargoSplitConsLineNo && d.DecCargoSplitConsItemLine == DecCargoSplitConsPackDet.DecCargoSplitConsItemLine && d.PackageLine == DecCargoSplitConsPackDet.PackageLine).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;  
            }
        
		 
            }
        
		 
            }
        
		
            List<DecCargoSplitCargoIdentifierPM> DecCargoSplitCargoIdentifiersChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.DecCargoSplitCargoIdentifiers).Cast<DecCargoSplitCargoIdentifierPM>().ToList();
            foreach (DecCargoSplitCargoIdentifierPM DecCargoSplitCon in DecCargoSplitCargoIdentifiersChangeSet)
            {  
                entityPM.DecCargoSplitCargoIdentifiers.Where(d => d.DeclarationCargoSplitId == DecCargoSplitCon.DeclarationCargoSplitId && d.LineNumber == DecCargoSplitCon.LineNumber).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;  
            }
        
		
            List<DecCargoSplitConPM> DecCargoSplitConsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.DecCargoSplitCons).Cast<DecCargoSplitConPM>().ToList();
            foreach (DecCargoSplitConPM DecCargoSplitCargoIdentifier in DecCargoSplitConsChangeSet)
            {  
                entityPM.DecCargoSplitCons.Where(d => d.DeclarationCargoSplitId == DecCargoSplitCargoIdentifier.DeclarationCargoSplitId && d.LineNumber == DecCargoSplitCargoIdentifier.LineNumber).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert; 
            List<DecCargoSplitConsItemPM> DecCargoSplitConsItemsChangeSet = ChangeSet.GetAssociatedChanges(DecCargoSplitCon, d => d.DecCargoSplitConsItems).Cast<DecCargoSplitConsItemPM>().ToList();
            foreach (DecCargoSplitConsItemPM DecCargoSplitConsItem in DecCargoSplitConsItemsChangeSet)
            {  
                DecCargoSplitCon.DecCargoSplitConsItems.Where(d => d.DeclarationCargoSplitId == DecCargoSplitConsItem.DeclarationCargoSplitId && d.DecCargoSplitConsLineNo == DecCargoSplitConsItem.DecCargoSplitConsLineNo && d.ItemLine == DecCargoSplitConsItem.ItemLine).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert; 
            List<DecCargoSplitConsPackDetPM> DecCargoSplitConsPackDetsChangeSet = ChangeSet.GetAssociatedChanges(DecCargoSplitConsItem, d => d.DecCargoSplitConsPackDets).Cast<DecCargoSplitConsPackDetPM>().ToList();
            foreach (DecCargoSplitConsPackDetPM DecCargoSplitConsPackDet in DecCargoSplitConsPackDetsChangeSet)
            {  
                DecCargoSplitConsItem.DecCargoSplitConsPackDets.Where(d => d.DeclarationCargoSplitId == DecCargoSplitConsPackDet.DeclarationCargoSplitId && d.DecCargoSplitConsLineNo == DecCargoSplitConsPackDet.DecCargoSplitConsLineNo && d.DecCargoSplitConsItemLine == DecCargoSplitConsPackDet.DecCargoSplitConsItemLine && d.PackageLine == DecCargoSplitConsPackDet.PackageLine).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;  
            }
        
		 
            }
        
		 
            }
        
		
            List<DecCargoSplitCargoIdentifierPM> DecCargoSplitCargoIdentifiersChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.DecCargoSplitCargoIdentifiers).Cast<DecCargoSplitCargoIdentifierPM>().ToList();
            foreach (DecCargoSplitCargoIdentifierPM DecCargoSplitCargoIdentifier in DecCargoSplitCargoIdentifiersChangeSet)
            {  
                entityPM.DecCargoSplitCargoIdentifiers.Where(d => d.DeclarationCargoSplitId == DecCargoSplitCargoIdentifier.DeclarationCargoSplitId && d.LineNumber == DecCargoSplitCargoIdentifier.LineNumber).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;  
            }
        
				    service.Update(entityPM,true); 

			ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("DeclarationCargoSplit", 0, true);
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
			    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id); 
            } 
        }


		public void UpdateDeclarationCargoSplit(DeclarationCargoSplitPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
			            SecurityUtility.CheckContactFeature("DeclarationCargoSplit", "UPDATE", entityPM.Tenant);if ( MyContext == null)
            {
                MyContext = CustomContext.GetContext(entityPM.Tenant);
            }; 
            DeclarationCargoSplitUpdateService service = new DeclarationCargoSplitUpdateService(MyContext,new Dictionary<string,IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
			
			SetDecCargoSplitConChangeSet(entityPM); 
			SetDecCargoSplitCargoIdentifierChangeSet(entityPM); 			 
		    service.Update(entityPM,true);

			ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("DeclarationCargoSplit", 0, true);
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
			    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id); 
            } 
        } 

		
			 
		private void SetDecCargoSplitConChangeSet(DeclarationCargoSplitPM entityPM)
        {
            List<DecCargoSplitConPM> DecCargoSplitConsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.DecCargoSplitCons).Cast<DecCargoSplitConPM>().ToList();

            foreach (DecCargoSplitConPM itemPM in DecCargoSplitConsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        { 
                            DecCargoSplitConPM currentItemPM = entityPM.DecCargoSplitCons.Where(d => d.DeclarationCargoSplitId == itemPM.DeclarationCargoSplitId && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert; 
		                	SetDecCargoSplitConsItemChangeSet(currentItemPM);
			                                           
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                           DecCargoSplitConPM currentItemPM = entityPM.DecCargoSplitCons.Where(d => d.DeclarationCargoSplitId == itemPM.DeclarationCargoSplitId && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
						    currentItemPM.ChangeSetOp = ChangeSetOperation.Update; 
		                	SetDecCargoSplitConsItemChangeSet(currentItemPM);
			                
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            DecCargoSplitConPM currentItemPM = new DecCargoSplitConPM()
                            {
                                ChangeSetOp = ChangeSetOperation.Delete, 
		                	    DeclarationCargoSplitId = itemPM.DeclarationCargoSplitId, 
		                	    LineNumber = itemPM.LineNumber,  

                            };

                            entityPM.DeletedDecCargoSplitCons.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                           DecCargoSplitConPM currentItemPM = entityPM.DecCargoSplitCons.Where(d => d.DeclarationCargoSplitId == itemPM.DeclarationCargoSplitId && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
						   currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                           break;
                        }
                }
            }
        } 
		
		private void SetDecCargoSplitCargoIdentifierChangeSet(DeclarationCargoSplitPM entityPM)
        {
            List<DecCargoSplitCargoIdentifierPM> DecCargoSplitCargoIdentifiersChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.DecCargoSplitCargoIdentifiers).Cast<DecCargoSplitCargoIdentifierPM>().ToList();

            foreach (DecCargoSplitCargoIdentifierPM itemPM in DecCargoSplitCargoIdentifiersChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        { 
                            DecCargoSplitCargoIdentifierPM currentItemPM = entityPM.DecCargoSplitCargoIdentifiers.Where(d => d.DeclarationCargoSplitId == itemPM.DeclarationCargoSplitId && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;                            
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                           DecCargoSplitCargoIdentifierPM currentItemPM = entityPM.DecCargoSplitCargoIdentifiers.Where(d => d.DeclarationCargoSplitId == itemPM.DeclarationCargoSplitId && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
						    currentItemPM.ChangeSetOp = ChangeSetOperation.Update; 
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            DecCargoSplitCargoIdentifierPM currentItemPM = new DecCargoSplitCargoIdentifierPM()
                            {
                                ChangeSetOp = ChangeSetOperation.Delete, 
		                	    DeclarationCargoSplitId = itemPM.DeclarationCargoSplitId, 
		                	    LineNumber = itemPM.LineNumber,  

                            };

                            entityPM.DeletedDecCargoSplitCargoIdentifiers.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                           DecCargoSplitCargoIdentifierPM currentItemPM = entityPM.DecCargoSplitCargoIdentifiers.Where(d => d.DeclarationCargoSplitId == itemPM.DeclarationCargoSplitId && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
						   currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                           break;
                        }
                }
            }
        } 
				
      
    }
}
	 