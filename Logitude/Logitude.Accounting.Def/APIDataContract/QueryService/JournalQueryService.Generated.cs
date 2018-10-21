using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.QuoteModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.Helpers;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;

 namespace Logitude.Accounting.BL.APIDataContract.ApiV1
{ 
   public partial class JournalQueryService
   {
   
		IAccountingContext  context;
		//JournalService service; 
		
		Logitude.Accounting.BL.EntityQueryServices.JournalQueryService query; 

        public JournalQueryService(int tenant)
        {
				    context = AccountingContext.GetContext(tenant); 
			//service = new JournalService(context, tenant); 
			query = new Logitude.Accounting.BL.EntityQueryServices.JournalQueryService(tenant);
        }

		
		public Journal GetJournalById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Journal with Id " + Id + " doesn't exist");

				return JournalDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Journal JournalDataMapping(JournalPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new Journal(); 
				   temp.Id = MyEntityPM.Id;
				   temp.Tenant = MyEntityPM.Tenant;
				   temp.JournalNumber = MyEntityPM.JournalNumber;
				   temp.CreateDate = MyEntityPM.CreateDate;
				   temp.AccountingDate = MyEntityPM.AccountingDate;
				   temp.ExternalNo = MyEntityPM.ExternalNo;
				   temp.UpdateDate = MyEntityPM.UpdateDate;
				   temp.ApproveDate = MyEntityPM.ApproveDate;
				   temp.AccountingEntityReference = MyEntityPM.AccountingEntityReference;
				   temp.VoidDate = MyEntityPM.VoidDate;			  
				   if(MyEntityPM.UpdatedByUserId != null)
				   {
					   UserQueryService UserService0 = new UserQueryService(Tenant);
					   					   temp.UpdatedByUser = UserService0.GetUserById(MyEntityPM.UpdatedByUserId,Tenant); 
			       
					   				   }
				   
				   temp.ExternalSystem = MyEntityPM.ExternalSystem;
				   temp.QueueId = MyEntityPM.QueueId;
				   temp.IsVoided = MyEntityPM.IsVoided;			  
				   if(MyEntityPM.VoidedByUserId != null)
				   {
					   UserQueryService UserService1 = new UserQueryService(Tenant);
					   					   temp.VoidedByUser = UserService1.GetUserById(MyEntityPM.VoidedByUserId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.OriginalJournalId != null)
				   {
					   JournalQueryService JournalService2 = new JournalQueryService(Tenant);
					   					   temp.OriginalJournal = JournalService2.GetJournalById(MyEntityPM.OriginalJournalId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.ApprovedByUserId != null)
				   {
					   UserQueryService UserService3 = new UserQueryService(Tenant);
					   					   temp.ApprovedByUser = UserService3.GetUserById(MyEntityPM.ApprovedByUserId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.VoidedByJournalId != null)
				   {
					   JournalQueryService JournalService4 = new JournalQueryService(Tenant);
					   					   temp.VoidedByJournal = JournalService4.GetJournalById(MyEntityPM.VoidedByJournalId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.CreatedByUserId != null)
				   {
					   UserQueryService UserService5 = new UserQueryService(Tenant);
					   					   temp.CreatedByUser = UserService5.GetUserById(MyEntityPM.CreatedByUserId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.TypeCode != null)
				   {
					   JournalTypeQueryService JournalTypeService6 = new JournalTypeQueryService(Tenant);
					   					   temp.JournalType = JournalTypeService6.GetJournalTypeByCode(MyEntityPM.TypeCode,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.StatusCode != null)
				   {
					   JournalStatusTypeQueryService JournalStatusTypeService7 = new JournalStatusTypeQueryService(Tenant);
					   					   temp.JournalStatusType = JournalStatusTypeService7.GetJournalStatusTypeByCode(MyEntityPM.StatusCode,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.AccountingEntityCode != null)
				   {
					   AccountingEntityQueryService AccountingEntityService8 = new AccountingEntityQueryService(Tenant);
					   					   temp.AccountingEntity = AccountingEntityService8.GetAccountingEntityByCode(MyEntityPM.AccountingEntityCode,Tenant); 
			       
					   				   }
				   
				   temp.AccountingEntityId = MyEntityPM.AccountingEntityId;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public JournalPM JournalDataMappingAndValidatin(Journal MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   					var temp = new JournalPM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
										   
					if(temp == null)
					{
					    throw new ApplicationException("Journal with Id " + MyEntity.Id + " doesn't exist");
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = MyEntity.Id;
					}
					temp.Tenant = MyEntity.Tenant;
					temp.JournalNumber = MyEntity.JournalNumber;
					temp.CreateDate = MyEntity.CreateDate;
					temp.AccountingDate = MyEntity.AccountingDate;
					temp.ExternalNo = MyEntity.ExternalNo;
					temp.UpdateDate = MyEntity.UpdateDate;
					temp.ApproveDate = MyEntity.ApproveDate;
					temp.AccountingEntityReference = MyEntity.AccountingEntityReference;
					temp.VoidDate = MyEntity.VoidDate;
					UserQueryService UpdatedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.UpdatedByUser != null)
					{
						var myUpdatedByUserPM = UpdatedByUserUserService.UserDataMappingAndValidatin(MyEntity.UpdatedByUser,Tenant,ComputingPartnerName);
												if(myUpdatedByUserPM != null)
						{
							temp.UpdatedByUserId = myUpdatedByUserPM.Id;
						}
						 
					}
			
					
					temp.ExternalSystem = MyEntity.ExternalSystem;
					temp.QueueId = MyEntity.QueueId;
					temp.IsVoided = MyEntity.IsVoided;
					UserQueryService VoidedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.VoidedByUser != null)
					{
						var myVoidedByUserPM = VoidedByUserUserService.UserDataMappingAndValidatin(MyEntity.VoidedByUser,Tenant,ComputingPartnerName);
												if(myVoidedByUserPM != null)
						{
							temp.VoidedByUserId = myVoidedByUserPM.Id;
						}
						 
					}
			
					
					JournalQueryService OriginalJournalJournalService = new JournalQueryService(Tenant);
					if(MyEntity.OriginalJournal != null)
					{
						var myOriginalJournalPM = OriginalJournalJournalService.JournalDataMappingAndValidatin(MyEntity.OriginalJournal,Tenant,ComputingPartnerName);
												if(myOriginalJournalPM != null)
						{
							temp.OriginalJournalId = myOriginalJournalPM.Id;
						}
						 
					}
			
					
					UserQueryService ApprovedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.ApprovedByUser != null)
					{
						var myApprovedByUserPM = ApprovedByUserUserService.UserDataMappingAndValidatin(MyEntity.ApprovedByUser,Tenant,ComputingPartnerName);
												if(myApprovedByUserPM != null)
						{
							temp.ApprovedByUserId = myApprovedByUserPM.Id;
						}
						 
					}
			
					
					JournalQueryService VoidedByJournalJournalService = new JournalQueryService(Tenant);
					if(MyEntity.VoidedByJournal != null)
					{
						var myVoidedByJournalPM = VoidedByJournalJournalService.JournalDataMappingAndValidatin(MyEntity.VoidedByJournal,Tenant,ComputingPartnerName);
												if(myVoidedByJournalPM != null)
						{
							temp.VoidedByJournalId = myVoidedByJournalPM.Id;
						}
						 
					}
			
					
					UserQueryService CreatedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.CreatedByUser != null)
					{
						var myCreatedByUserPM = CreatedByUserUserService.UserDataMappingAndValidatin(MyEntity.CreatedByUser,Tenant,ComputingPartnerName);
												if(myCreatedByUserPM != null)
						{
							temp.CreatedByUserId = myCreatedByUserPM.Id;
						}
						 
					}
			
					
					JournalTypeQueryService JournalTypeJournalTypeService = new JournalTypeQueryService(Tenant);
					if(MyEntity.JournalType != null)
					{
						var myJournalTypePM = JournalTypeJournalTypeService.JournalTypeDataMappingAndValidatin(MyEntity.JournalType,Tenant,ComputingPartnerName);
												if(myJournalTypePM != null)
						{
							temp.TypeCode = myJournalTypePM.JournalTypeID;
						}
						 
					}
			
					
					JournalStatusTypeQueryService JournalStatusTypeJournalStatusTypeService = new JournalStatusTypeQueryService(Tenant);
					if(MyEntity.JournalStatusType != null)
					{
						var myJournalStatusTypePM = JournalStatusTypeJournalStatusTypeService.JournalStatusTypeDataMappingAndValidatin(MyEntity.JournalStatusType,Tenant,ComputingPartnerName);
												if(myJournalStatusTypePM != null)
						{
							temp.StatusCode = myJournalStatusTypePM.JournalStatusID;
						}
						 
					}
			
					
					AccountingEntityQueryService AccountingEntityAccountingEntityService = new AccountingEntityQueryService(Tenant);
					if(MyEntity.AccountingEntity != null)
					{
						var myAccountingEntityPM = AccountingEntityAccountingEntityService.AccountingEntityDataMappingAndValidatin(MyEntity.AccountingEntity,Tenant,ComputingPartnerName);
												if(myAccountingEntityPM != null)
						{
							temp.AccountingEntityCode = myAccountingEntityPM.Code;
						}
						 
					}
			
					
					temp.AccountingEntityId = MyEntity.AccountingEntityId;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}