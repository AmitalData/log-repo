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
   public partial class LedgerTransactionQueryService
   {
   
		IAccountingContext  context;
		//LedgerTransactionService service; 
		
		Logitude.Accounting.BL.EntityQueryServices.LedgerTransactionQueryService query; 

        public LedgerTransactionQueryService(int tenant)
        {
				    context = AccountingContext.GetContext(tenant); 
			//service = new LedgerTransactionService(context, tenant); 
			query = new Logitude.Accounting.BL.EntityQueryServices.LedgerTransactionQueryService(tenant);
        }

		
		public LedgerTransaction GetLedgerTransactionById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("LedgerTransaction with Id " + Id + " doesn't exist");

				return LedgerTransactionDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public LedgerTransaction LedgerTransactionDataMapping(LedgerTransactionPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new LedgerTransaction(); 
				   temp.Id = MyEntityPM.Id;			  
				   if(MyEntityPM.CurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService0 = new CurrencyQueryService(Tenant);
					   					   temp.Currency = CurrencyService0.GetCurrencyById(MyEntityPM.CurrencyId,Tenant); 
			       
					   				   }
				   
				   temp.DueDate = MyEntityPM.DueDate;
				   temp.LocalAmountCredit = MyEntityPM.LocalAmountCredit;
				   temp.ForeignAmountCredit = MyEntityPM.ForeignAmountCredit;
				   temp.Reference1 = MyEntityPM.Reference1;
				   temp.Reference2 = MyEntityPM.Reference2;
				   temp.Notes = MyEntityPM.Notes;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public LedgerTransactionPM LedgerTransactionDataMappingAndValidatin(LedgerTransaction MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
					var temp = new LedgerTransactionPM();
												  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
										   
					if(temp == null)
					{
					    throw new ApplicationException("LedgerTransaction with Id " + MyEntity.Id + " doesn't exist");
						
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = MyEntity.Id;
					}
					CurrencyQueryService CurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.Currency != null)
					{
						var myCurrencyPM = CurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.Currency,Tenant,ComputingPartnerName);
												if(myCurrencyPM != null)
						{
							temp.CurrencyId = myCurrencyPM.Id;
						}
						 
					}
			
					
					temp.DueDate = MyEntity.DueDate;
					temp.LocalAmountCredit = MyEntity.LocalAmountCredit;
					temp.ForeignAmountCredit = MyEntity.ForeignAmountCredit;
					temp.Reference1 = MyEntity.Reference1;
					temp.Reference2 = MyEntity.Reference2;
					temp.Notes = MyEntity.Notes;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}