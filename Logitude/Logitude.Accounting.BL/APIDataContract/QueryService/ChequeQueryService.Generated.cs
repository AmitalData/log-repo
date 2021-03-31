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
   public partial class ChequeQueryService
   {
   
		IAccountingContext  context;
		//ARPaymentChequeService service; 
		
		Logitude.Accounting.BL.EntityQueryServices.ARPaymentChequeQueryService query; 

        public ChequeQueryService(int tenant)
        {
				    context = AccountingContext.GetContext(tenant); 
			//service = new ARPaymentChequeService(context, tenant); 
			query = new Logitude.Accounting.BL.EntityQueryServices.ARPaymentChequeQueryService(tenant);
        }

		
		public Cheque GetChequeById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("ARPaymentCheque with Id " + Id + " doesn't exist");

				return ChequeDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Cheque ChequeDataMapping(ARPaymentChequePM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new Cheque(); 
				   temp.Id = MyEntityPM.Id;
				   temp.Tenant = MyEntityPM.Tenant;			  
				   if(MyEntityPM.CurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService0 = new CurrencyQueryService(Tenant);
					   					   temp.Currency = CurrencyService0.GetCurrencyById(MyEntityPM.CurrencyId,Tenant); 
			       
					   				   }
				   
				   temp.ChequeNumber = MyEntityPM.ChequeNumber;
				   temp.ValueDate = MyEntityPM.ValueDate;
				   temp.LocalAmount = MyEntityPM.LocalAmount;
				   temp.ForeignAmount = MyEntityPM.ForeignAmount;
				   temp.BankId = MyEntityPM.BankId;
				   temp.BankBranch = MyEntityPM.BankBranch;
				   temp.BankAccount = MyEntityPM.BankAccount;
				   temp.StatusCode = MyEntityPM.StatusCode;
				   temp.StatusName = MyEntityPM.StatusName;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public ARPaymentChequePM ChequeDataMappingAndValidatin(Cheque MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
					var temp = new ARPaymentChequePM();
												  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
										   
					if(temp == null)
					{
					    throw new ApplicationException("ARPaymentCheque with Id " + MyEntity.Id + " doesn't exist");
						
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = MyEntity.Id;
					}
					temp.Tenant = MyEntity.Tenant;
					CurrencyQueryService CurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.Currency != null)
					{
						var myCurrencyPM = CurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.Currency,Tenant,ComputingPartnerName);
												if(myCurrencyPM != null)
						{
							temp.CurrencyId = myCurrencyPM.Id;
						}
						 
					}
			
					
					temp.ChequeNumber = MyEntity.ChequeNumber;
					temp.ValueDate = MyEntity.ValueDate;
					temp.LocalAmount = MyEntity.LocalAmount;
					temp.ForeignAmount = MyEntity.ForeignAmount;
					temp.BankId = MyEntity.BankId;
					temp.BankBranch = MyEntity.BankBranch;
					temp.BankAccount = MyEntity.BankAccount;
					temp.StatusCode = MyEntity.StatusCode;
					temp.StatusName = MyEntity.StatusName;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}
