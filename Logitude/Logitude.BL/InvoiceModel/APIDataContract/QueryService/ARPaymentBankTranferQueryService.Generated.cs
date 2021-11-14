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
   public partial class ARPaymentBankTranferQueryService
   {
   
		ARPaymentBankTranferQuery query; 

        public ARPaymentBankTranferQueryService(int tenant)
        {
		
			query = new ARPaymentBankTranferQuery(tenant);
        }

		
		public List<ARPaymentBankTranfer> ARPaymentBankTranferDataMapping(List<ARPaymentBankTranferPM> MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<ARPaymentBankTranfer>();
				foreach (var item in MyEntityPM)
				{
				   
				   var temp = new ARPaymentBankTranfer(); 
				   temp.Id = item.Id;
				   temp.Tenant = item.Tenant;
				   temp.LineNumber = item.LineNumber;
				   temp.PaymentRef = item.PaymentRef;
				   temp.ValueDate = item.ValueDate;
				   temp.LocalAmount = item.LocalAmount;
				   temp.ForeignAmount = item.ForeignAmount;
				   temp.BankAccountNumber = item.BankAccountNumber;
				   temp.BankAccountId = item.BankAccountId;					
					MyList.Add(temp);
				}
					
				   return MyList;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public List<ARPaymentBankTranferPM> ARPaymentBankTranferDataMappingAndValidatin(List<ARPaymentBankTranfer> MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   
				var MyList = new List<ARPaymentBankTranferPM>();
				foreach (var item in MyEntity)
				{
					   					var temp = new ARPaymentBankTranferPM();								  
					if (!string.IsNullOrEmpty(item.Id))
					{
						temp = query.GetSinglePM(item.Id, Tenant);
					} 
										   
					if(temp == null)
					{   
					    throw new ApplicationException("ARPaymentBankTranfer with Id " + item.Id + " doesn't exist");
					} 
					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(item.Id))
					    {
					        throw new ApplicationException("ARPaymentBankTranfer with provided key doesn't exist");
						
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

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.PaymentRef))
					{							//throw new ApplicationException("PaymentRef Can't be update"); 
							temp.PaymentRef = item.PaymentRef;

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

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.BankAccountNumber))
					{							//throw new ApplicationException("BankAccountNumber Can't be update"); 
							temp.BankAccountNumber = item.BankAccountNumber;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.BankAccountId))
					{							//throw new ApplicationException("BankAccountId Can't be update"); 
							temp.BankAccountId = item.BankAccountId;

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