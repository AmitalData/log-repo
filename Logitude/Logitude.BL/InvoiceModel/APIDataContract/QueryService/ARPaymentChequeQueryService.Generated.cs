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
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InvoiceModel;

 namespace Logitude.BL.InvoiceModel.APIDataContract.ApiV1
{ 
   public partial class ARPaymentChequeQueryService
   {
   
		ARPaymentChequeReplicaQuery query; 

        public ARPaymentChequeQueryService(int tenant)
        {
		
			query = new ARPaymentChequeReplicaQuery(tenant);
        }

		
		public List<ARPaymentCheque> ARPaymentChequeDataMapping(List<ARPaymentChequeReplicaPM> MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<ARPaymentCheque>();
				foreach (var item in MyEntityPM)
				{
				   
				   var temp = new ARPaymentCheque(); 
				   temp.Id = item.Id;
				   temp.Tenant = item.Tenant;
				   temp.LineNumber = item.LineNumber;
				   temp.ChequeNumber = item.ChequeNumber;
				   temp.ValueDate = item.ValueDate;
				   temp.LocalAmount = item.LocalAmount;
				   temp.ForeignAmount = item.ForeignAmount;
				   temp.BankBranch = item.BankBranch;
				   temp.BankAccount = item.BankAccount;
				   temp.Bank = item.BankId; 

			  
				   if(item.StatusCode != null)
				   {
					   ARPaymentChequeStatusReplicaQueryService ARPaymentChequeStatusReplicaService0 = new ARPaymentChequeStatusReplicaQueryService(Tenant);
					   					   temp.ChequeStatus = ARPaymentChequeStatusReplicaService0.GetARPaymentChequeStatusReplicaByCode(item.StatusCode,Tenant); 
			       
					   				   }
				   					
					MyList.Add(temp);
				}
					
				   return MyList;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public List<ARPaymentChequeReplicaPM> ARPaymentChequeDataMappingAndValidatin(List<ARPaymentCheque> MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   
				var MyList = new List<ARPaymentChequeReplicaPM>();
				foreach (var item in MyEntity)
				{
					   					var temp = new ARPaymentChequeReplicaPM();								  
					if (!string.IsNullOrEmpty(item.Id))
					{
						temp = query.GetSinglePM(item.Id, Tenant);
					} 
										   
					if(temp == null)
					{   
					    throw new ApplicationException("ARPaymentChequeReplica with Id " + item.Id + " doesn't exist");
					} 
					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(item.Id))
					    {
					        throw new ApplicationException("ARPaymentChequeReplica with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = item.Id;

						//} 

						
					}
                    
					if(!IsUpdate)// && item.Tenant != null)
					{							//throw new ApplicationException("Tenant Can't be update"); 
							temp.Tenant = item.Tenant;

										}  

					
                    
					if(!IsUpdate)// && item.LineNumber != null)
					{							//throw new ApplicationException("LineNumber Can't be update"); 
							temp.LineNumber = item.LineNumber;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.ChequeNumber))
					{							//throw new ApplicationException("ChequeNumber Can't be update"); 
							temp.ChequeNumber = item.ChequeNumber;

										}  

					
                    
					if(!IsUpdate)// && item.ValueDate != null)
					{							//throw new ApplicationException("ValueDate Can't be update"); 
							temp.ValueDate = item.ValueDate;

										}  

					
                    
					if(!IsUpdate)// && item.LocalAmount != null)
					{							//throw new ApplicationException("LocalAmount Can't be update"); 
							temp.LocalAmount = item.LocalAmount;

										}  

					
                    
					if(!IsUpdate)// && item.ForeignAmount != null)
					{							//throw new ApplicationException("ForeignAmount Can't be update"); 
							temp.ForeignAmount = item.ForeignAmount;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.BankBranch))
					{							//throw new ApplicationException("BankBranch Can't be update"); 
							temp.BankBranch = item.BankBranch;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.BankAccount))
					{							//throw new ApplicationException("BankAccount Can't be update"); 
							temp.BankAccount = item.BankAccount;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.Bank))
					{							//throw new ApplicationException("Bank Can't be update"); 
							temp.BankId = item.Bank;

										}  

					
					ARPaymentChequeStatusReplicaQueryService ChequeStatusARPaymentChequeStatusReplicaService = new ARPaymentChequeStatusReplicaQueryService(Tenant);
					if(item.ChequeStatus != null)
					{
						var myChequeStatusPM = ChequeStatusARPaymentChequeStatusReplicaService.ARPaymentChequeStatusReplicaDataMappingAndValidatin(item.ChequeStatus,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myChequeStatusPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("ChequeStatus Can't be update"); 
								temp.StatusCode = myChequeStatusPM.Code;
						  
							}  

							
						} 

					}
			
										   
						MyList.Add(temp);
					}
						
					return MyList;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}