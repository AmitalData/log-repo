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
   public partial class JournalLineQueryService
   {
   
		Logitude.Accounting.BL.EntityQueryServices.JournalLineQueryService query; 

        public JournalLineQueryService(int tenant)
        {
		
			query = new Logitude.Accounting.BL.EntityQueryServices.JournalLineQueryService(tenant);
        }

		
		public List<JournalLine> JournalLineDataMapping(List<JournalLinePM> MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<JournalLine>();
				foreach (var item in MyEntityPM)
				{
				   
				   var temp = new JournalLine(); 
				   temp.JournalNumber = item.JournalId;
				   temp.Line = item.Line;
				   temp.Tenant = item.Tenant;
				   temp.DebitControlAccount = item.DebitControlAccountId;
				   temp.DebitAccount = item.DebitAccountId;
				   temp.CreditControlAccount = item.CreditControlAccountId;
				   temp.CreditAccount = item.CreditAccountId;
				   temp.DocumentDate = item.DocumentDate;
				   temp.AccountingDate = item.AccountingDate;
				   temp.DueDate = item.DueDate;
				   temp.LocalAmount = item.LocalAmount;			  
				   if(item.CurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService0 = new CurrencyQueryService(Tenant);
					   					   temp.Currency = CurrencyService0.GetCurrencyById(item.CurrencyId,Tenant); 
			       
					   				   }
				   
				   temp.ForeignAmount = item.ForeignAmount;
				   temp.ExchangeRate = item.ExchangeRate;
				   temp.Reference1 = item.Reference1;
				   temp.Reference2 = item.Reference2;
				   temp.Reference3 = item.Reference3;
				   temp.Notes = item.Notes;
				   temp.ExternalOpenAmount = item.ExternalOpenAmount;
				   temp.ActionCode = item.ActionCode;
				   temp.ExternalReconcileNumber = item.ExternalReconcileNumber;
				   temp.ConfirmationNumber = item.ConfirmationNumber;					
					MyList.Add(temp);
				}
					
				   return MyList;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public List<JournalLinePM> JournalLineDataMappingAndValidatin(List<JournalLine> MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<JournalLinePM>();
				foreach (var item in MyEntity)
				{
					   
					var temp = new JournalLinePM();
										if (!string.IsNullOrEmpty(item.JournalNumber))
					{
						temp = query.GetSinglePMByJournalId(item.JournalNumber, Tenant);
					} 					if (!string.IsNullOrEmpty(item.Line))
					{
						temp = query.GetSinglePMByLine(item.Line, Tenant);
					} 					   
					if(temp == null)
					{
					    throw new ApplicationException("JournalLine with Line " + item.Line + " doesn't exist");
						
					} 
					temp.JournalId = item.JournalNumber;
					temp.Line = item.Line;
					temp.Tenant = item.Tenant;
					temp.DebitControlAccountId = item.DebitControlAccount;
					temp.DebitAccountId = item.DebitAccount;
					temp.CreditControlAccountId = item.CreditControlAccount;
					temp.CreditAccountId = item.CreditAccount;
					temp.DocumentDate = item.DocumentDate;
					temp.AccountingDate = item.AccountingDate;
					temp.DueDate = item.DueDate;
					temp.LocalAmount = item.LocalAmount;
					CurrencyQueryService CurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(item.Currency != null)
					{
						var myCurrencyPM = CurrencyCurrencyService.CurrencyDataMappingAndValidatin(item.Currency,Tenant,ComputingPartnerName);
												if(myCurrencyPM != null)
						{
							temp.CurrencyId = myCurrencyPM.Id;
						}
						 
					}
			
					
					temp.ForeignAmount = item.ForeignAmount;
					temp.ExchangeRate = item.ExchangeRate;
					temp.Reference1 = item.Reference1;
					temp.Reference2 = item.Reference2;
					temp.Reference3 = item.Reference3;
					temp.Notes = item.Notes;
					temp.ExternalOpenAmount = item.ExternalOpenAmount;
					temp.ActionCode = item.ActionCode;
					temp.ExternalReconcileNumber = item.ExternalReconcileNumber;
					temp.ConfirmationNumber = item.ConfirmationNumber;					   
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