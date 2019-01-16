using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.APIDataContract.;
using Logitude.BL.QuoteModel.APIDataContract.;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.APIDataContract.;
using Logitude.BL.ShipmentsModel.APIDataContract.;
using Logitude.BL.Helpers;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;

 namespace Logitude.Accounting.BL.APIDataContract.
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
				   temp.JournalId = item.JournalId;
				   temp.Line = item.Line;
				   temp.Tenant = item.Tenant;			  
				   if(item.DebitControlAccountId != null)
				   {
					   GLAccountQueryService GLAccountService0 = new GLAccountQueryService(Tenant);
					   					   temp.DebitControlAccount = GLAccountService0.GetGLAccountById(item.DebitControlAccountId,Tenant); 
			       
					   				   }
				   			  
				   if(item.DebitAccountId != null)
				   {
					   GLAccountQueryService GLAccountService1 = new GLAccountQueryService(Tenant);
					   					   temp.DebitAccount = GLAccountService1.GetGLAccountById(item.DebitAccountId,Tenant); 
			       
					   				   }
				   			  
				   if(item.CreditControlAccountId != null)
				   {
					   GLAccountQueryService GLAccountService2 = new GLAccountQueryService(Tenant);
					   					   temp.CreditControlAccount = GLAccountService2.GetGLAccountById(item.CreditControlAccountId,Tenant); 
			       
					   				   }
				   			  
				   if(item.CreditAccountId != null)
				   {
					   GLAccountQueryService GLAccountService3 = new GLAccountQueryService(Tenant);
					   					   temp.CreditAccount = GLAccountService3.GetGLAccountById(item.CreditAccountId,Tenant); 
			       
					   				   }
				   
				   temp.DocumentDate = item.DocumentDate;
				   temp.AccountingDate = item.AccountingDate;
				   temp.DueDate = item.DueDate;
				   temp.LocalAmount = item.LocalAmount;			  
				   if(item.CurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService4 = new CurrencyQueryService(Tenant);
					   					   temp.Currency = CurrencyService4.GetCurrencyById(item.CurrencyId,Tenant); 
			       
					   				   }
				   
				   temp.ForeignAmount = item.ForeignAmount;
				   temp.ExchangeRate = item.ExchangeRate;
				   temp.Reference1 = item.Reference1;
				   temp.Reference2 = item.Reference2;
				   temp.Reference3 = item.Reference3;
				   temp.CreditAccountNumber = item.CreditAccountNumber;
				   temp.DebitAccountNumber = item.DebitAccountNumber;
				   temp.Notes = item.Notes;
				   temp.ExternalOpenAmount = item.ExternalOpenAmount;
				   temp.IsCreditAccountMulti = item.IsCreditAccountMulti;
				   temp.IsDebitAccountMulti = item.IsDebitAccountMulti;					
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
										if (!string.IsNullOrEmpty(item.JournalId))
					{
						temp = query.GetSinglePMByJournalId(item.JournalId, Tenant);
					} 					if (!string.IsNullOrEmpty(item.Line))
					{
						temp = query.GetSinglePMByLine(item.Line, Tenant);
					} 					   
					if(temp == null)
					{
					    throw new ApplicationException("JournalLine with Line " + item.Line + " doesn't exist");
						
					} 
					temp.JournalId = item.JournalId;
					temp.Line = item.Line;
					temp.Tenant = item.Tenant;
					GLAccountQueryService DebitControlAccountGLAccountService = new GLAccountQueryService(Tenant);
					if(item.DebitControlAccount != null)
					{
						var myDebitControlAccountPM = DebitControlAccountGLAccountService.GLAccountDataMappingAndValidatin(item.DebitControlAccount,Tenant,ComputingPartnerName);
												if(myDebitControlAccountPM != null)
						{
							temp.DebitControlAccountId = myDebitControlAccountPM.Id;
						}
						 
					}
			
					
					GLAccountQueryService DebitAccountGLAccountService = new GLAccountQueryService(Tenant);
					if(item.DebitAccount != null)
					{
						var myDebitAccountPM = DebitAccountGLAccountService.GLAccountDataMappingAndValidatin(item.DebitAccount,Tenant,ComputingPartnerName);
												if(myDebitAccountPM != null)
						{
							temp.DebitAccountId = myDebitAccountPM.Id;
						}
						 
					}
			
					
					GLAccountQueryService CreditControlAccountGLAccountService = new GLAccountQueryService(Tenant);
					if(item.CreditControlAccount != null)
					{
						var myCreditControlAccountPM = CreditControlAccountGLAccountService.GLAccountDataMappingAndValidatin(item.CreditControlAccount,Tenant,ComputingPartnerName);
												if(myCreditControlAccountPM != null)
						{
							temp.CreditControlAccountId = myCreditControlAccountPM.Id;
						}
						 
					}
			
					
					GLAccountQueryService CreditAccountGLAccountService = new GLAccountQueryService(Tenant);
					if(item.CreditAccount != null)
					{
						var myCreditAccountPM = CreditAccountGLAccountService.GLAccountDataMappingAndValidatin(item.CreditAccount,Tenant,ComputingPartnerName);
												if(myCreditAccountPM != null)
						{
							temp.CreditAccountId = myCreditAccountPM.Id;
						}
						 
					}
			
					
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
					temp.CreditAccountNumber = item.CreditAccountNumber;
					temp.DebitAccountNumber = item.DebitAccountNumber;
					temp.Notes = item.Notes;
					temp.ExternalOpenAmount = item.ExternalOpenAmount;
					temp.IsCreditAccountMulti = item.IsCreditAccountMulti;
					temp.IsDebitAccountMulti = item.IsDebitAccountMulti;					   
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