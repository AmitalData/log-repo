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
   public partial class GLAccountQueryService
   {
   
		IAccountingContext  context;
		//GLAccountService service; 
		
		Logitude.Accounting.BL.EntityQueryServices.GLAccountQueryService query; 

        public GLAccountQueryService(int tenant)
        {
				    context = AccountingContext.GetContext(tenant); 
			//service = new GLAccountService(context, tenant); 
			query = new Logitude.Accounting.BL.EntityQueryServices.GLAccountQueryService(tenant);
        }

		
		public GLAccount GetGLAccountById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("GLAccount with Id " + Id + " doesn't exist");

				return GLAccountDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public GLAccount GLAccountDataMapping(GLAccountPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new GLAccount(); 
				   temp.Id = MyEntityPM.Id;
				   temp.Tenant = MyEntityPM.Tenant;
				   temp.InternalNumber = MyEntityPM.InternalNumber;			  
				   if(MyEntityPM.AccountTypeCode != null)
				   {
					   GLAccountTypeQueryService GLAccountTypeService0 = new GLAccountTypeQueryService(Tenant);
					   					   temp.GLAccountType = GLAccountTypeService0.GetGLAccountTypeByCode(MyEntityPM.AccountTypeCode,Tenant); 
			       
					   				   }
				   
				   temp.DisplayNumber = MyEntityPM.DisplayNumber;
				   temp.LocalName = MyEntityPM.LocalName;
				   temp.EnglishName = MyEntityPM.EnglishName;
				   temp.IsMultiCurrency = MyEntityPM.IsMultiCurrency;			  
				   if(MyEntityPM.CurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService1 = new CurrencyQueryService(Tenant);
					   					   temp.Currency = CurrencyService1.GetCurrencyById(MyEntityPM.CurrencyId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.ChartOfAccountsId != null)
				   {
					   ChartOfAccountQueryService ChartOfAccountService2 = new ChartOfAccountQueryService(Tenant);
					   					   temp.ChartOfAccount = ChartOfAccountService2.GetChartOfAccountById(MyEntityPM.ChartOfAccountsId,Tenant); 
			       
					   				   }
				   
				   temp.Inactive = MyEntityPM.Inactive;
				   temp.AccountTypeName = MyEntityPM.AccountTypeName;
				   temp.CurrencyName = MyEntityPM.CurrencyName;
				   temp.ChartOfAccountsName = MyEntityPM.ChartOfAccountsName;			  
				   if(MyEntityPM.ChartOfAccountsTypeCode != null)
				   {
					   ChartOfAccountsTypeQueryService ChartOfAccountsTypeService3 = new ChartOfAccountsTypeQueryService(Tenant);
					   					   temp.ChartOfAccountsType = ChartOfAccountsTypeService3.GetChartOfAccountsTypeByCode(MyEntityPM.ChartOfAccountsTypeCode,Tenant); 
			       
					   				   }
				   
				   temp.ChartOfAccountsTypeName = MyEntityPM.ChartOfAccountsTypeName;
				   temp.CurrencyCode = MyEntityPM.CurrencyCode;			  
				   if(MyEntityPM.ReconcileMethodCode != null)
				   {
					   ReconcileMethodQueryService ReconcileMethodService4 = new ReconcileMethodQueryService(Tenant);
					   					   temp.ReconcileMethod = ReconcileMethodService4.GetReconcileMethodByCode(MyEntityPM.ReconcileMethodCode,Tenant); 
			       
					   				   }
				   
				   temp.ReconcileMethodName = MyEntityPM.ReconcileMethodName;			  
				   if(MyEntityPM.ControlAccountId != null)
				   {
					   GLAccountQueryService GLAccountService5 = new GLAccountQueryService(Tenant);
					   					   temp.ControlAccount = GLAccountService5.GetGLAccountById(MyEntityPM.ControlAccountId,Tenant); 
			       
					   				   }
				   
				   temp.ControlAccountName = MyEntityPM.ControlAccountName;
				   temp.ControlAccountNumber = MyEntityPM.ControlAccountNumber;
				   temp.ActiveStatusName = MyEntityPM.ActiveStatusName;
				   temp.OldCurrencyId = MyEntityPM.OldCurrencyId;
				   temp.OldIsMultiCurrency = MyEntityPM.OldIsMultiCurrency;			  
				   if(MyEntityPM.AutomaticReconcileId != null)
				   {
					   AutomaticReconcileMethodQueryService AutomaticReconcileMethodService6 = new AutomaticReconcileMethodQueryService(Tenant);
					   					   temp.AutomaticReconcileMethod = AutomaticReconcileMethodService6.GetAutomaticReconcileMethodById(MyEntityPM.AutomaticReconcileId,Tenant); 
			       
					   				   }
				   
				   temp.AutomaticReconcileName = MyEntityPM.AutomaticReconcileName;
				   temp.PreviousEnglishName = MyEntityPM.PreviousEnglishName;
				   temp.PreviousLocalName = MyEntityPM.PreviousLocalName;
				   temp.PreviousNumber = MyEntityPM.PreviousNumber;			  
				   if(MyEntityPM.PreviousChartOfAccountsId != null)
				   {
					   ChartOfAccountQueryService ChartOfAccountService7 = new ChartOfAccountQueryService(Tenant);
					   					   temp.PreviousChartOfAccount = ChartOfAccountService7.GetChartOfAccountById(MyEntityPM.PreviousChartOfAccountsId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.CustomerGLAccountId != null)
				   {
					   GLAccountQueryService GLAccountService8 = new GLAccountQueryService(Tenant);
					   					   temp.CustomerGLAccount = GLAccountService8.GLAccountCustomDataMapping(MyEntityPM.CustomerGLAccountId,Tenant); 
			       
					   				   }
				   
				   temp.CustomerGLAccountName = MyEntityPM.CustomerGLAccountName;
				   temp.CustomerGLAccountNumber = MyEntityPM.CustomerGLAccountNumber;
				   temp.BalanceInLocalCurrency = MyEntityPM.BalanceInLocalCurrency;
				   temp.RevaluationEnabled = MyEntityPM.RevaluationEnabled;			  
				   if(MyEntityPM.ParentAccountId != null)
				   {
					   GLAccountQueryService GLAccountService9 = new GLAccountQueryService(Tenant);
					   					   temp.ParentAccount = GLAccountService9.GetGLAccountById(MyEntityPM.ParentAccountId,Tenant); 
			       
					   				   }
				   
				   temp.ParentAccountName = MyEntityPM.ParentAccountName;
				   temp.ParentAccountNumber = MyEntityPM.ParentAccountNumber;
				   temp.CustomerGLAccountInternalNumber = MyEntityPM.CustomerGLAccountInternalNumber;			  
				   if(MyEntityPM.Category1Id != null)
				   {
					   Category1QueryService Category1Service10 = new Category1QueryService(Tenant);
					   					   temp.Category1 = Category1Service10.GetCategory1ById(MyEntityPM.Category1Id,Tenant); 
			       
					   				   }
				   
				   temp.Category1Name = MyEntityPM.Category1Name;			  
				   if(MyEntityPM.Category2Id != null)
				   {
					   Category2QueryService Category2Service11 = new Category2QueryService(Tenant);
					   					   temp.Category2 = Category2Service11.GetCategory2ById(MyEntityPM.Category2Id,Tenant); 
			       
					   				   }
				   
				   temp.Category2Name = MyEntityPM.Category2Name;			  
				   if(MyEntityPM.Category3Id != null)
				   {
					   Category3QueryService Category3Service12 = new Category3QueryService(Tenant);
					   					   temp.Category3 = Category3Service12.GetCategory3ById(MyEntityPM.Category3Id,Tenant); 
			       
					   				   }
				   
				   temp.Category3Name = MyEntityPM.Category3Name;			  
				   if(MyEntityPM.Category4Id != null)
				   {
					   Category4QueryService Category4Service13 = new Category4QueryService(Tenant);
					   					   temp.Category4 = Category4Service13.GetCategory4ById(MyEntityPM.Category4Id,Tenant); 
			       
					   				   }
				   
				   temp.Category4Name = MyEntityPM.Category4Name;			  
				   if(MyEntityPM.Category5Id != null)
				   {
					   Category5QueryService Category5Service14 = new Category5QueryService(Tenant);
					   					   temp.Category5 = Category5Service14.GetCategory5ById(MyEntityPM.Category5Id,Tenant); 
			       
					   				   }
				   
				   temp.Category5Name = MyEntityPM.Category5Name;
				   temp.IsVATExempt = MyEntityPM.IsVATExempt;
				   temp.ChartOfAccountsCode = MyEntityPM.ChartOfAccountsCode;
				   temp.CustomerCode = MyEntityPM.CustomerCode;
				   temp.ParentAccountByCurrency = MyEntityPM.ParentAccountByCurrency;
				   temp.VatNumber = MyEntityPM.VatNumber;
				   temp.PaymentTermId = MyEntityPM.PaymentTermId;			  
				   if(MyEntityPM.CollectorId != null)
				   {
					   UserQueryService UserService15 = new UserQueryService(Tenant);
					   					   temp.Collector = UserService15.GetUserById(MyEntityPM.CollectorId,Tenant); 
			       
					   				   }
				   
				   temp.SalesmanUserId = MyEntityPM.SalesmanUserId;
				   temp.NewGLAccountCardId = MyEntityPM.NewGLAccountCardId;
				   temp.LocalBalanceInDue = MyEntityPM.LocalBalanceInDue;
				   temp.NextDueDate = MyEntityPM.NextDueDate;
				   temp.CurrencySign = MyEntityPM.CurrencySign;
				   temp.ConnectedItems = MyEntityPM.ConnectedItems;
				   temp.Type = MyEntityPM.Type;
				   temp.DeductionFileTypeId = MyEntityPM.DeductionFileTypeId;
				   temp.DeductionFileNumber = MyEntityPM.DeductionFileNumber;
				   temp.AssessingOfficeCode = MyEntityPM.AssessingOfficeCode;
				   temp.Occupation = MyEntityPM.Occupation;
				   temp.DeductionTypeId = MyEntityPM.DeductionTypeId;
				   temp.ConsolidationVat = MyEntityPM.ConsolidationVat;			  
				   if(MyEntityPM.RevenueExpenseType != null)
				   {
					   RevenueExpenseTypeQueryService RevenueExpenseTypeService16 = new RevenueExpenseTypeQueryService(Tenant);
					   					   temp.RevenueExpenseType = RevenueExpenseTypeService16.GetRevenueExpenseTypeByCode(MyEntityPM.RevenueExpenseType,Tenant); 
			       
					   				   }
				   
				   temp.Parent = MyEntityPM.Parent;
				   temp.CardCode = MyEntityPM.CardCode;
				   temp.PartnerTypeId = MyEntityPM.PartnerTypeId;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public GLAccountPM GLAccountDataMappingAndValidatin(GLAccount MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
					var temp = new GLAccountPM();
												  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
										   
					if(temp == null)
					{
					    throw new ApplicationException("GLAccount with Id " + MyEntity.Id + " doesn't exist");
						
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = MyEntity.Id;
					}
					temp.Tenant = MyEntity.Tenant;
					temp.InternalNumber = MyEntity.InternalNumber;
					GLAccountTypeQueryService GLAccountTypeGLAccountTypeService = new GLAccountTypeQueryService(Tenant);
					if(MyEntity.GLAccountType != null)
					{
						var myGLAccountTypePM = GLAccountTypeGLAccountTypeService.GLAccountTypeDataMappingAndValidatin(MyEntity.GLAccountType,Tenant,ComputingPartnerName);
												if(myGLAccountTypePM != null)
						{
							temp.AccountTypeCode = myGLAccountTypePM.Code;
						}
						 
					}
			
					
					temp.DisplayNumber = MyEntity.DisplayNumber;
					temp.LocalName = MyEntity.LocalName;
					temp.EnglishName = MyEntity.EnglishName;
					temp.IsMultiCurrency = MyEntity.IsMultiCurrency;
					CurrencyQueryService CurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.Currency != null)
					{
						var myCurrencyPM = CurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.Currency,Tenant,ComputingPartnerName);
												if(myCurrencyPM != null)
						{
							temp.CurrencyId = myCurrencyPM.Id;
						}
						 
					}
			
					
					ChartOfAccountQueryService ChartOfAccountChartOfAccountService = new ChartOfAccountQueryService(Tenant);
					if(MyEntity.ChartOfAccount != null)
					{
						var myChartOfAccountPM = ChartOfAccountChartOfAccountService.ChartOfAccountDataMappingAndValidatin(MyEntity.ChartOfAccount,Tenant,ComputingPartnerName);
												if(myChartOfAccountPM != null)
						{
							temp.ChartOfAccountsId = myChartOfAccountPM.Id;
						}
						 
					}
			
					
					temp.Inactive = MyEntity.Inactive;
					temp.AccountTypeName = MyEntity.AccountTypeName;
					temp.CurrencyName = MyEntity.CurrencyName;
					temp.ChartOfAccountsName = MyEntity.ChartOfAccountsName;
					ChartOfAccountsTypeQueryService ChartOfAccountsTypeChartOfAccountsTypeService = new ChartOfAccountsTypeQueryService(Tenant);
					if(MyEntity.ChartOfAccountsType != null)
					{
						var myChartOfAccountsTypePM = ChartOfAccountsTypeChartOfAccountsTypeService.ChartOfAccountsTypeDataMappingAndValidatin(MyEntity.ChartOfAccountsType,Tenant,ComputingPartnerName);
												if(myChartOfAccountsTypePM != null)
						{
							temp.ChartOfAccountsTypeCode = myChartOfAccountsTypePM.Code;
						}
						 
					}
			
					
					temp.ChartOfAccountsTypeName = MyEntity.ChartOfAccountsTypeName;
					temp.CurrencyCode = MyEntity.CurrencyCode;
					ReconcileMethodQueryService ReconcileMethodReconcileMethodService = new ReconcileMethodQueryService(Tenant);
					if(MyEntity.ReconcileMethod != null)
					{
						var myReconcileMethodPM = ReconcileMethodReconcileMethodService.ReconcileMethodDataMappingAndValidatin(MyEntity.ReconcileMethod,Tenant,ComputingPartnerName);
												if(myReconcileMethodPM != null)
						{
							temp.ReconcileMethodCode = myReconcileMethodPM.Code;
						}
						 
					}
			
					
					temp.ReconcileMethodName = MyEntity.ReconcileMethodName;
					GLAccountQueryService ControlAccountGLAccountService = new GLAccountQueryService(Tenant);
					if(MyEntity.ControlAccount != null)
					{
						var myControlAccountPM = ControlAccountGLAccountService.GLAccountDataMappingAndValidatin(MyEntity.ControlAccount,Tenant,ComputingPartnerName);
												if(myControlAccountPM != null)
						{
							temp.ControlAccountId = myControlAccountPM.Id;
						}
						 
					}
			
					
					temp.ControlAccountName = MyEntity.ControlAccountName;
					temp.ControlAccountNumber = MyEntity.ControlAccountNumber;
					temp.ActiveStatusName = MyEntity.ActiveStatusName;
					temp.OldCurrencyId = MyEntity.OldCurrencyId;
					temp.OldIsMultiCurrency = MyEntity.OldIsMultiCurrency;
					AutomaticReconcileMethodQueryService AutomaticReconcileMethodAutomaticReconcileMethodService = new AutomaticReconcileMethodQueryService(Tenant);
					if(MyEntity.AutomaticReconcileMethod != null)
					{
						var myAutomaticReconcileMethodPM = AutomaticReconcileMethodAutomaticReconcileMethodService.AutomaticReconcileMethodDataMappingAndValidatin(MyEntity.AutomaticReconcileMethod,Tenant,ComputingPartnerName);
												if(myAutomaticReconcileMethodPM != null)
						{
							temp.AutomaticReconcileId = myAutomaticReconcileMethodPM.Id;
						}
						 
					}
			
					
					temp.AutomaticReconcileName = MyEntity.AutomaticReconcileName;
					temp.PreviousEnglishName = MyEntity.PreviousEnglishName;
					temp.PreviousLocalName = MyEntity.PreviousLocalName;
					temp.PreviousNumber = MyEntity.PreviousNumber;
					ChartOfAccountQueryService PreviousChartOfAccountChartOfAccountService = new ChartOfAccountQueryService(Tenant);
					if(MyEntity.PreviousChartOfAccount != null)
					{
						var myPreviousChartOfAccountPM = PreviousChartOfAccountChartOfAccountService.ChartOfAccountDataMappingAndValidatin(MyEntity.PreviousChartOfAccount,Tenant,ComputingPartnerName);
												if(myPreviousChartOfAccountPM != null)
						{
							temp.PreviousChartOfAccountsId = myPreviousChartOfAccountPM.Id;
						}
						 
					}
			
					
					GLAccountQueryService CustomerGLAccountGLAccountService = new GLAccountQueryService(Tenant);
					if(MyEntity.CustomerGLAccount != null)
					{
						var myCustomerGLAccountPM = CustomerGLAccountGLAccountService.GLAccountCustomDataMappingAndValidatin(MyEntity.CustomerGLAccount,Tenant);
												if(myCustomerGLAccountPM != null)
						{
							temp.CustomerGLAccountId = myCustomerGLAccountPM.Id;
						}
						 
					}
			
					
					temp.CustomerGLAccountName = MyEntity.CustomerGLAccountName;
					temp.CustomerGLAccountNumber = MyEntity.CustomerGLAccountNumber;
					temp.BalanceInLocalCurrency = MyEntity.BalanceInLocalCurrency;
					temp.RevaluationEnabled = MyEntity.RevaluationEnabled;
					GLAccountQueryService ParentAccountGLAccountService = new GLAccountQueryService(Tenant);
					if(MyEntity.ParentAccount != null)
					{
						var myParentAccountPM = ParentAccountGLAccountService.GLAccountDataMappingAndValidatin(MyEntity.ParentAccount,Tenant,ComputingPartnerName);
												if(myParentAccountPM != null)
						{
							temp.ParentAccountId = myParentAccountPM.Id;
						}
						 
					}
			
					
					temp.ParentAccountName = MyEntity.ParentAccountName;
					temp.ParentAccountNumber = MyEntity.ParentAccountNumber;
					temp.CustomerGLAccountInternalNumber = MyEntity.CustomerGLAccountInternalNumber;
					Category1QueryService Category1Category1Service = new Category1QueryService(Tenant);
					if(MyEntity.Category1 != null)
					{
						var myCategory1PM = Category1Category1Service.Category1DataMappingAndValidatin(MyEntity.Category1,Tenant,ComputingPartnerName);
												if(myCategory1PM != null)
						{
							temp.Category1Id = myCategory1PM.Id;
						}
						 
					}
			
					
					temp.Category1Name = MyEntity.Category1Name;
					Category2QueryService Category2Category2Service = new Category2QueryService(Tenant);
					if(MyEntity.Category2 != null)
					{
						var myCategory2PM = Category2Category2Service.Category2DataMappingAndValidatin(MyEntity.Category2,Tenant,ComputingPartnerName);
												if(myCategory2PM != null)
						{
							temp.Category2Id = myCategory2PM.Id;
						}
						 
					}
			
					
					temp.Category2Name = MyEntity.Category2Name;
					Category3QueryService Category3Category3Service = new Category3QueryService(Tenant);
					if(MyEntity.Category3 != null)
					{
						var myCategory3PM = Category3Category3Service.Category3DataMappingAndValidatin(MyEntity.Category3,Tenant,ComputingPartnerName);
												if(myCategory3PM != null)
						{
							temp.Category3Id = myCategory3PM.Id;
						}
						 
					}
			
					
					temp.Category3Name = MyEntity.Category3Name;
					Category4QueryService Category4Category4Service = new Category4QueryService(Tenant);
					if(MyEntity.Category4 != null)
					{
						var myCategory4PM = Category4Category4Service.Category4DataMappingAndValidatin(MyEntity.Category4,Tenant,ComputingPartnerName);
												if(myCategory4PM != null)
						{
							temp.Category4Id = myCategory4PM.Id;
						}
						 
					}
			
					
					temp.Category4Name = MyEntity.Category4Name;
					Category5QueryService Category5Category5Service = new Category5QueryService(Tenant);
					if(MyEntity.Category5 != null)
					{
						var myCategory5PM = Category5Category5Service.Category5DataMappingAndValidatin(MyEntity.Category5,Tenant,ComputingPartnerName);
												if(myCategory5PM != null)
						{
							temp.Category5Id = myCategory5PM.Id;
						}
						 
					}
			
					
					temp.Category5Name = MyEntity.Category5Name;
					temp.IsVATExempt = MyEntity.IsVATExempt;
					temp.ChartOfAccountsCode = MyEntity.ChartOfAccountsCode;
					temp.CustomerCode = MyEntity.CustomerCode;
					temp.ParentAccountByCurrency = MyEntity.ParentAccountByCurrency;
					temp.VatNumber = MyEntity.VatNumber;
					temp.PaymentTermId = MyEntity.PaymentTermId;
					UserQueryService CollectorUserService = new UserQueryService(Tenant);
					if(MyEntity.Collector != null)
					{
						var myCollectorPM = CollectorUserService.UserDataMappingAndValidatin(MyEntity.Collector,Tenant,ComputingPartnerName);
												if(myCollectorPM != null)
						{
							temp.CollectorId = myCollectorPM.Id;
						}
						 
					}
			
					
					temp.SalesmanUserId = MyEntity.SalesmanUserId;
					temp.NewGLAccountCardId = MyEntity.NewGLAccountCardId;
					temp.LocalBalanceInDue = MyEntity.LocalBalanceInDue;
					temp.NextDueDate = MyEntity.NextDueDate;
					temp.CurrencySign = MyEntity.CurrencySign;
					temp.ConnectedItems = MyEntity.ConnectedItems;
					temp.Type = MyEntity.Type;
					temp.DeductionFileTypeId = MyEntity.DeductionFileTypeId;
					temp.DeductionFileNumber = MyEntity.DeductionFileNumber;
					temp.AssessingOfficeCode = MyEntity.AssessingOfficeCode;
					temp.Occupation = MyEntity.Occupation;
					temp.DeductionTypeId = MyEntity.DeductionTypeId;
					temp.ConsolidationVat = MyEntity.ConsolidationVat;
					RevenueExpenseTypeQueryService RevenueExpenseTypeRevenueExpenseTypeService = new RevenueExpenseTypeQueryService(Tenant);
					if(MyEntity.RevenueExpenseType != null)
					{
						var myRevenueExpenseTypePM = RevenueExpenseTypeRevenueExpenseTypeService.RevenueExpenseTypeDataMappingAndValidatin(MyEntity.RevenueExpenseType,Tenant,ComputingPartnerName);
												if(myRevenueExpenseTypePM != null)
						{
							temp.RevenueExpenseType = myRevenueExpenseTypePM.Code;
						}
						 
					}
			
					
					temp.Parent = MyEntity.Parent;
					temp.CardCode = MyEntity.CardCode;
					temp.PartnerTypeId = MyEntity.PartnerTypeId;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}