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
				   if(MyEntityPM.UpdatedByUserId != null)
				   {
					   UserQueryService UserService0 = new UserQueryService(Tenant);
					   					   temp.UpdatedByUser = UserService0.GetUserById(MyEntityPM.UpdatedByUserId,Tenant); 
			       
					   				   }
				   
				   temp.ExternalSystem = MyEntityPM.ExternalSystem;
				   temp.OriginalJournal = MyEntityPM.OriginalJournalId;			  
				   if(MyEntityPM.ApprovedByUserId != null)
				   {
					   UserQueryService UserService1 = new UserQueryService(Tenant);
					   					   temp.ApprovedByUser = UserService1.GetUserById(MyEntityPM.ApprovedByUserId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.CreatedByUserId != null)
				   {
					   UserQueryService UserService2 = new UserQueryService(Tenant);
					   					   temp.CreatedByUser = UserService2.GetUserById(MyEntityPM.CreatedByUserId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.TypeCode != null)
				   {
					   JournalTypeQueryService JournalTypeService3 = new JournalTypeQueryService(Tenant);
					   					   temp.JournalType = JournalTypeService3.GetJournalTypeByCode(MyEntityPM.TypeCode,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.StatusCode != null)
				   {
					   JournalStatusTypeQueryService JournalStatusTypeService4 = new JournalStatusTypeQueryService(Tenant);
					   					   temp.JournalStatusType = JournalStatusTypeService4.GetJournalStatusTypeByCode(MyEntityPM.StatusCode,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.AccountingEntityCode != null)
				   {
					   AccountingEntityQueryService AccountingEntityService5 = new AccountingEntityQueryService(Tenant);
					   					   temp.AccountingEntity = AccountingEntityService5.GetAccountingEntityByCode(MyEntityPM.AccountingEntityCode,Tenant); 
			       
					   				   }
				   
				   temp.AccountingEntityId = MyEntityPM.AccountingEntityId;
				if(MyEntityPM.JournalLines != null && MyEntityPM.JournalLines.Count > 0)
				{
					 JournalLineQueryService JournalLineService6 = new JournalLineQueryService(Tenant);
					 temp.JournalLines = JournalLineService6.JournalLineCustomDataMapping(MyEntityPM,MyEntityPM.JournalLines,Tenant);
				}

							 					
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
					temp.OriginalJournalId = MyEntity.OriginalJournal;
					UserQueryService ApprovedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.ApprovedByUser != null)
					{
						var myApprovedByUserPM = ApprovedByUserUserService.UserDataMappingAndValidatin(MyEntity.ApprovedByUser,Tenant,ComputingPartnerName);
												if(myApprovedByUserPM != null)
						{
							temp.ApprovedByUserId = myApprovedByUserPM.Id;
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
					if(MyEntity.JournalLines != null && MyEntity.JournalLines.Count > 0)
					{
						JournalLineQueryService JournalLineService6 = new JournalLineQueryService(Tenant);
						temp.JournalLines = JournalLineService6.JournalLineCustomDataMappingAndValidatin(MyEntity,MyEntity.JournalLines,Tenant,ComputingPartnerName);
					}

								 					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}