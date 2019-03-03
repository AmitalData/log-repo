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
				   temp.CreateDate = MyEntityPM.CreateDate;
				   temp.AccountingDate = MyEntityPM.AccountingDate;
				   temp.ExternalNo = MyEntityPM.ExternalNo;
				   temp.UpdateDate = MyEntityPM.UpdateDate;
				   temp.ApproveDate = MyEntityPM.ApproveDate;
				   temp.AccountingEntityReference = MyEntityPM.AccountingEntityReference;
				   temp.UpdatedByUser = MyEntityPM.UpdatedByUserId;
				   temp.ExternalSystem = MyEntityPM.ExternalSystem;
				   temp.OriginalJournalNumber = MyEntityPM.OriginalJournalName;
				   temp.ApprovedByUser = MyEntityPM.ApprovedByUserId;
				   temp.CreatedByUser = MyEntityPM.CreatedByUserId;
				   temp.JournalType = MyEntityPM.TypeCode;
				   temp.JournalStatusType = MyEntityPM.StatusCode;			  
				   if(MyEntityPM.AccountingEntityCode != null)
				   {
					   AccountingEntityQueryService AccountingEntityService0 = new AccountingEntityQueryService(Tenant);
					   					   temp.AccountingEntity = AccountingEntityService0.GetAccountingEntityByCode(MyEntityPM.AccountingEntityCode,Tenant); 
			       
					   				   }
				   
				   temp.AccountingEntityId = MyEntityPM.AccountingEntityId;
				if(MyEntityPM.JournalLines != null && MyEntityPM.JournalLines.Count > 0)
				{
					 JournalLineQueryService JournalLineService1 = new JournalLineQueryService(Tenant);
					 temp.JournalLines = JournalLineService1.JournalLineCustomDataMapping(MyEntityPM,MyEntityPM.JournalLines,Tenant);
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
					temp.CreateDate = MyEntity.CreateDate;
					temp.AccountingDate = MyEntity.AccountingDate;
					temp.ExternalNo = MyEntity.ExternalNo;
					temp.UpdateDate = MyEntity.UpdateDate;
					temp.ApproveDate = MyEntity.ApproveDate;
					temp.AccountingEntityReference = MyEntity.AccountingEntityReference;
					temp.UpdatedByUserId = MyEntity.UpdatedByUser;
					temp.ExternalSystem = MyEntity.ExternalSystem;
					temp.OriginalJournalName = MyEntity.OriginalJournalNumber;
					temp.ApprovedByUserId = MyEntity.ApprovedByUser;
					temp.CreatedByUserId = MyEntity.CreatedByUser;
					temp.TypeCode = MyEntity.JournalType;
					temp.StatusCode = MyEntity.JournalStatusType;
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
						JournalLineQueryService JournalLineService1 = new JournalLineQueryService(Tenant);
						temp.JournalLines = JournalLineService1.JournalLineCustomDataMappingAndValidatin(MyEntity,MyEntity.JournalLines,Tenant,ComputingPartnerName);
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