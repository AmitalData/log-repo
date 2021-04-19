
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;  
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class GLAccountDataMapping: IMapping<GLAccountPM, GLAccount>,IMappingEncodeBase64NVARCHARFields<GLAccountPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         InternalNumber, 
	         AccountTypeCode, 
	         DisplayNumber, 
	         LocalName, 
	         EnglishName, 
	         SearchFields, 
	         IsMultiCurrency, 
	         CurrencyId, 
	         RevenueExpenseType, 
	         IsControlAccount, 
	         ChartOfAccountsId, 
	         Inactive, 
	         ChartOfAccountsTypeCode, 
	         ReconcileMethodCode, 
	         ControlAccountId, 
	         AutomaticReconcileId, 
	         PreviousEnglishName, 
	         PreviousEnglishNameChangeDate, 
	         PreviousLocalName, 
	         PreviousLocalNameChangeDate, 
	         PreviousNumber, 
	         PreviousNumberChangeDate, 
	         PreviousChartOfAccountsId, 
	         PreviousChartOfAccountsChangeDate, 
	         CustomerGLAccountId, 
	         RevaluationEnabled, 
	         ParentAccountId, 
	         Category1Id, 
	         Category2Id, 
	         Category3Id, 
	         Category4Id, 
	         Category5Id, 
	         IsVATExempt, 
	         DeductionFileTypeId, 
	         DeductionFileNumber, 
	         AssessingOfficeCode, 
	         Occupation, 
	         DeductionTypeId, 
	         ConsolidationVat, 
	         IsEquipmentVendor, 
	         ExcludeFromDeductionReport, 
	         CreatedByUserId, 
	         UpdatedByUserId, 
	         CreateDate, 
	         UpdateDate, 
	         AllowEditChequePayToName, 
	         ActiveForInterest, 
	         InterestCalculationStartDate, 
	         ActiveForInterestCreditInvoice, 
	         InterestCreditLimit, 
	         NameForPrintingCheques, 
	         Smallcashbook, 
	         MinimumInterestInvoiceBilling, 
	         ReportingAsAnotherDocument, 
	         CreditAllotmentPercentage,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         InternalNumber, 
	         AccountTypeCode, 
	         DisplayNumber, 
	         LocalName, 
	         EnglishName, 
	         SearchFields, 
	         IsMultiCurrency, 
	         CurrencyId, 
	         RevenueExpenseType, 
	         IsControlAccount, 
	         ChartOfAccountsId, 
	         Inactive, 
	         AccountTypeName, 
	         CurrencyName, 
	         RevenueExpenseName, 
	         ChartOfAccountsName, 
	         ChartOfAccountsTypeCode, 
	         ChartOfAccountsTypeName, 
	         CurrencyCode, 
	         ReconcileMethodCode, 
	         ReconcileMethodName, 
	         ControlAccountId, 
	         ControlAccountName, 
	         ControlAccountNumber, 
	         ActiveStatusName, 
	         OldCurrencyId, 
	         OldIsMultiCurrency, 
	         AutomaticReconcileId, 
	         AutomaticReconcileName, 
	         PreviousEnglishName, 
	         PreviousEnglishNameChangeDate, 
	         PreviousLocalName, 
	         PreviousLocalNameChangeDate, 
	         PreviousNumber, 
	         PreviousNumberChangeDate, 
	         PreviousChartOfAccountsId, 
	         PreviousChartOfAccountsChangeDate, 
	         CustomerGLAccountId, 
	         CustomerGLAccountName, 
	         CustomerGLAccountNumber, 
	         BalanceInLocalCurrency, 
	         RevaluationEnabled, 
	         ParentAccountId, 
	         ParentAccountName, 
	         ParentAccountNumber, 
	         CustomerGLAccountInternalNumber, 
	         Category1Id, 
	         Category1Name, 
	         Category2Id, 
	         Category2Name, 
	         Category3Id, 
	         Category3Name, 
	         Category4Id, 
	         Category4Name, 
	         Category5Id, 
	         Category5Name, 
	         IsVATExempt, 
	         ChartOfAccountsCode, 
	         CustomerCode, 
	         ParentAccountByCurrency, 
	         VatNumber, 
	         PaymentTermId, 
	         CollectorId, 
	         SalesmanUserId, 
	         NewGLAccountCardId, 
	         LocalBalanceInDue, 
	         NextDueDate, 
	         CurrencySign, 
	         ConnectedItems, 
	         Type, 
	         DeductionFileTypeId, 
	         DeductionFileNumber, 
	         AssessingOfficeCode, 
	         Occupation, 
	         DeductionTypeId, 
	         ConsolidationVat, 
	         TaxWithholdingLastLine, 
	         ReconcilationCount, 
	         IsEquipmentVendor, 
	         ExcludeFromDeductionReport, 
	         Parent, 
	         DeductionTypeName, 
	         DeductionFileTypeCode, 
	         DeductionFileTypeName, 
	         AssessingOfficeName, 
	         DeductionTypeEnglishName, 
	         TotalOpenChequesInLocalCur, 
	         TotFutureOpenChequesInLocalCur, 
	         CardId, 
	         CreatedByUserId, 
	         UpdatedByUserId, 
	         CreateDate, 
	         UpdateDate, 
	         CreatedByUserName, 
	         UpdatedByUserName, 
	         UpdatedByLocalName, 
	         CardCode, 
	         PartnerTypeId, 
	         AllowEditChequePayToName, 
	         ActiveForInterest, 
	         InterestCalculationStartDate, 
	         ActiveForInterestCreditInvoice, 
	         InterestCreditLimit, 
	         NameForPrintingCheques, 
	         Smallcashbook, 
	         MinimumInterestInvoiceBilling, 
	         IsSplitted, 
	         SalesmanName, 
	         CollectorName, 
	         SplitCurrencyAccount, 
	         ParentName, 
	         ParentCurrencyId, 
	         ReportingAsAnotherDocument, 
	         CreditAllotmentPercentage, 
	         RelatedGLAccount,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(GLAccountPM entityPM, GLAccount entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InternalNumber))
            {
				entityPOCO.InternalNumber = entityPM.InternalNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountTypeCode))
            {
				entityPOCO.AccountTypeCode = entityPM.AccountTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DisplayNumber))
            {
				entityPOCO.DisplayNumber = entityPM.DisplayNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalName))
            {
				entityPOCO.LocalName = entityPM.LocalName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishName))
            {
				entityPOCO.EnglishName = entityPM.EnglishName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMultiCurrency))
            {
				entityPOCO.IsMultiCurrency = entityPM.IsMultiCurrency;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrencyId))
            {
				entityPOCO.CurrencyId = entityPM.CurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RevenueExpenseType))
            {
				entityPOCO.RevenueExpenseType = entityPM.RevenueExpenseType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsControlAccount))
            {
				entityPOCO.IsControlAccount = entityPM.IsControlAccount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChartOfAccountsId))
            {
				entityPOCO.ChartOfAccountsId = entityPM.ChartOfAccountsId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Inactive))
            {
				entityPOCO.Inactive = entityPM.Inactive;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChartOfAccountsTypeCode))
            {
				entityPOCO.ChartOfAccountsTypeCode = entityPM.ChartOfAccountsTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReconcileMethodCode))
            {
				entityPOCO.ReconcileMethodCode = entityPM.ReconcileMethodCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ControlAccountId))
            {
				entityPOCO.ControlAccountId = entityPM.ControlAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AutomaticReconcileId))
            {
				entityPOCO.AutomaticReconcileId = entityPM.AutomaticReconcileId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreviousEnglishName))
            {
				entityPOCO.PreviousEnglishName = entityPM.PreviousEnglishName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreviousEnglishNameChangeDate))
            {
				entityPOCO.PreviousEnglishNameChangeDate = entityPM.PreviousEnglishNameChangeDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreviousLocalName))
            {
				entityPOCO.PreviousLocalName = entityPM.PreviousLocalName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreviousLocalNameChangeDate))
            {
				entityPOCO.PreviousLocalNameChangeDate = entityPM.PreviousLocalNameChangeDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreviousNumber))
            {
				entityPOCO.PreviousNumber = entityPM.PreviousNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreviousNumberChangeDate))
            {
				entityPOCO.PreviousNumberChangeDate = entityPM.PreviousNumberChangeDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreviousChartOfAccountsId))
            {
				entityPOCO.PreviousChartOfAccountsId = entityPM.PreviousChartOfAccountsId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreviousChartOfAccountsChangeDate))
            {
				entityPOCO.PreviousChartOfAccountsChangeDate = entityPM.PreviousChartOfAccountsChangeDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerGLAccountId))
            {
				entityPOCO.CustomerGLAccountId = entityPM.CustomerGLAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RevaluationEnabled))
            {
				entityPOCO.RevaluationEnabled = entityPM.RevaluationEnabled;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParentAccountId))
            {
				entityPOCO.ParentAccountId = entityPM.ParentAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Category1Id))
            {
				entityPOCO.Category1Id = entityPM.Category1Id;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Category2Id))
            {
				entityPOCO.Category2Id = entityPM.Category2Id;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Category3Id))
            {
				entityPOCO.Category3Id = entityPM.Category3Id;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Category4Id))
            {
				entityPOCO.Category4Id = entityPM.Category4Id;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Category5Id))
            {
				entityPOCO.Category5Id = entityPM.Category5Id;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsVATExempt))
            {
				entityPOCO.IsVATExempt = entityPM.IsVATExempt;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeductionFileTypeId))
            {
				entityPOCO.DeductionFileTypeId = entityPM.DeductionFileTypeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeductionFileNumber))
            {
				entityPOCO.DeductionFileNumber = entityPM.DeductionFileNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AssessingOfficeCode))
            {
				entityPOCO.AssessingOfficeCode = entityPM.AssessingOfficeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Occupation))
            {
				entityPOCO.Occupation = entityPM.Occupation;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeductionTypeId))
            {
				entityPOCO.DeductionTypeId = entityPM.DeductionTypeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsolidationVat))
            {
				entityPOCO.ConsolidationVat = entityPM.ConsolidationVat;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsEquipmentVendor))
            {
				entityPOCO.IsEquipmentVendor = entityPM.IsEquipmentVendor;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExcludeFromDeductionReport))
            {
				entityPOCO.ExcludeFromDeductionReport = entityPM.ExcludeFromDeductionReport;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AllowEditChequePayToName))
            {
				entityPOCO.AllowEditChequePayToName = entityPM.AllowEditChequePayToName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActiveForInterest))
            {
				entityPOCO.ActiveForInterest = entityPM.ActiveForInterest;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestCalculationStartDate))
            {
				entityPOCO.InterestCalculationStartDate = entityPM.InterestCalculationStartDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActiveForInterestCreditInvoice))
            {
				entityPOCO.ActiveForInterestCreditInvoice = entityPM.ActiveForInterestCreditInvoice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestCreditLimit))
            {
				entityPOCO.InterestCreditLimit = entityPM.InterestCreditLimit;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NameForPrintingCheques))
            {
				entityPOCO.NameForPrintingCheques = entityPM.NameForPrintingCheques;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Smallcashbook))
            {
				entityPOCO.Smallcashbook = entityPM.Smallcashbook;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MinimumInterestInvoiceBilling))
            {
				entityPOCO.MinimumInterestInvoiceBilling = entityPM.MinimumInterestInvoiceBilling;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReportingAsAnotherDocument))
            {
				entityPOCO.ReportingAsAnotherDocument = entityPM.ReportingAsAnotherDocument;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreditAllotmentPercentage))
            {
				entityPOCO.CreditAllotmentPercentage = entityPM.CreditAllotmentPercentage;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(GLAccountPM entityPM, GLAccount entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InternalNumber))
            {
					entityPM.InternalNumber = entityPOCO.InternalNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AccountTypeCode))
            {
					entityPM.AccountTypeCode = entityPOCO.AccountTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DisplayNumber))
            {
					entityPM.DisplayNumber = entityPOCO.DisplayNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalName))
            {
					entityPM.LocalName = entityPOCO.LocalName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishName))
            {
					entityPM.EnglishName = entityPOCO.EnglishName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsMultiCurrency))
            {
					entityPM.IsMultiCurrency = entityPOCO.IsMultiCurrency;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CurrencyId))
            {
					entityPM.CurrencyId = entityPOCO.CurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RevenueExpenseType))
            {
					entityPM.RevenueExpenseType = entityPOCO.RevenueExpenseType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsControlAccount))
            {
					entityPM.IsControlAccount = entityPOCO.IsControlAccount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChartOfAccountsId))
            {
					entityPM.ChartOfAccountsId = entityPOCO.ChartOfAccountsId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Inactive))
            {
					entityPM.Inactive = entityPOCO.Inactive;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChartOfAccountsTypeCode))
            {
					entityPM.ChartOfAccountsTypeCode = entityPOCO.ChartOfAccountsTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReconcileMethodCode))
            {
					entityPM.ReconcileMethodCode = entityPOCO.ReconcileMethodCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ControlAccountId))
            {
					entityPM.ControlAccountId = entityPOCO.ControlAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AutomaticReconcileId))
            {
					entityPM.AutomaticReconcileId = entityPOCO.AutomaticReconcileId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PreviousEnglishName))
            {
					entityPM.PreviousEnglishName = entityPOCO.PreviousEnglishName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PreviousEnglishNameChangeDate))
            {
					entityPM.PreviousEnglishNameChangeDate = entityPOCO.PreviousEnglishNameChangeDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PreviousLocalName))
            {
					entityPM.PreviousLocalName = entityPOCO.PreviousLocalName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PreviousLocalNameChangeDate))
            {
					entityPM.PreviousLocalNameChangeDate = entityPOCO.PreviousLocalNameChangeDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PreviousNumber))
            {
					entityPM.PreviousNumber = entityPOCO.PreviousNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PreviousNumberChangeDate))
            {
					entityPM.PreviousNumberChangeDate = entityPOCO.PreviousNumberChangeDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PreviousChartOfAccountsId))
            {
					entityPM.PreviousChartOfAccountsId = entityPOCO.PreviousChartOfAccountsId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PreviousChartOfAccountsChangeDate))
            {
					entityPM.PreviousChartOfAccountsChangeDate = entityPOCO.PreviousChartOfAccountsChangeDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerGLAccountId))
            {
					entityPM.CustomerGLAccountId = entityPOCO.CustomerGLAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RevaluationEnabled))
            {
					entityPM.RevaluationEnabled = entityPOCO.RevaluationEnabled;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ParentAccountId))
            {
					entityPM.ParentAccountId = entityPOCO.ParentAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Category1Id))
            {
					entityPM.Category1Id = entityPOCO.Category1Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Category2Id))
            {
					entityPM.Category2Id = entityPOCO.Category2Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Category3Id))
            {
					entityPM.Category3Id = entityPOCO.Category3Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Category4Id))
            {
					entityPM.Category4Id = entityPOCO.Category4Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Category5Id))
            {
					entityPM.Category5Id = entityPOCO.Category5Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsVATExempt))
            {
					entityPM.IsVATExempt = entityPOCO.IsVATExempt;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeductionFileTypeId))
            {
					entityPM.DeductionFileTypeId = entityPOCO.DeductionFileTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeductionFileNumber))
            {
					entityPM.DeductionFileNumber = entityPOCO.DeductionFileNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AssessingOfficeCode))
            {
					entityPM.AssessingOfficeCode = entityPOCO.AssessingOfficeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Occupation))
            {
					entityPM.Occupation = entityPOCO.Occupation;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeductionTypeId))
            {
					entityPM.DeductionTypeId = entityPOCO.DeductionTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsolidationVat))
            {
					entityPM.ConsolidationVat = entityPOCO.ConsolidationVat;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsEquipmentVendor))
            {
					entityPM.IsEquipmentVendor = entityPOCO.IsEquipmentVendor;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExcludeFromDeductionReport))
            {
					entityPM.ExcludeFromDeductionReport = entityPOCO.ExcludeFromDeductionReport;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AllowEditChequePayToName))
            {
					entityPM.AllowEditChequePayToName = entityPOCO.AllowEditChequePayToName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActiveForInterest))
            {
					entityPM.ActiveForInterest = entityPOCO.ActiveForInterest;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InterestCalculationStartDate))
            {
					entityPM.InterestCalculationStartDate = entityPOCO.InterestCalculationStartDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActiveForInterestCreditInvoice))
            {
					entityPM.ActiveForInterestCreditInvoice = entityPOCO.ActiveForInterestCreditInvoice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InterestCreditLimit))
            {
					entityPM.InterestCreditLimit = entityPOCO.InterestCreditLimit;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NameForPrintingCheques))
            {
					entityPM.NameForPrintingCheques = entityPOCO.NameForPrintingCheques;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Smallcashbook))
            {
					entityPM.Smallcashbook = entityPOCO.Smallcashbook;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MinimumInterestInvoiceBilling))
            {
					entityPM.MinimumInterestInvoiceBilling = entityPOCO.MinimumInterestInvoiceBilling;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReportingAsAnotherDocument))
            {
					entityPM.ReportingAsAnotherDocument = entityPOCO.ReportingAsAnotherDocument;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreditAllotmentPercentage))
            {
					entityPM.CreditAllotmentPercentage = entityPOCO.CreditAllotmentPercentage;
            }

		}

		public void PMToOldPM(GLAccountPM entityPM, GLAccountPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InternalNumber))
            {
                oldEntityPM.InternalNumber = entityPM.InternalNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountTypeCode))
            {
                oldEntityPM.AccountTypeCode = entityPM.AccountTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DisplayNumber))
            {
                oldEntityPM.DisplayNumber = entityPM.DisplayNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalName))
            {
                oldEntityPM.LocalName = entityPM.LocalName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishName))
            {
                oldEntityPM.EnglishName = entityPM.EnglishName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMultiCurrency))
            {
                oldEntityPM.IsMultiCurrency = entityPM.IsMultiCurrency;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrencyId))
            {
                oldEntityPM.CurrencyId = entityPM.CurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RevenueExpenseType))
            {
                oldEntityPM.RevenueExpenseType = entityPM.RevenueExpenseType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsControlAccount))
            {
                oldEntityPM.IsControlAccount = entityPM.IsControlAccount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChartOfAccountsId))
            {
                oldEntityPM.ChartOfAccountsId = entityPM.ChartOfAccountsId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Inactive))
            {
                oldEntityPM.Inactive = entityPM.Inactive;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChartOfAccountsTypeCode))
            {
                oldEntityPM.ChartOfAccountsTypeCode = entityPM.ChartOfAccountsTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReconcileMethodCode))
            {
                oldEntityPM.ReconcileMethodCode = entityPM.ReconcileMethodCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ControlAccountId))
            {
                oldEntityPM.ControlAccountId = entityPM.ControlAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AutomaticReconcileId))
            {
                oldEntityPM.AutomaticReconcileId = entityPM.AutomaticReconcileId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreviousEnglishName))
            {
                oldEntityPM.PreviousEnglishName = entityPM.PreviousEnglishName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreviousEnglishNameChangeDate))
            {
                oldEntityPM.PreviousEnglishNameChangeDate = entityPM.PreviousEnglishNameChangeDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreviousLocalName))
            {
                oldEntityPM.PreviousLocalName = entityPM.PreviousLocalName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreviousLocalNameChangeDate))
            {
                oldEntityPM.PreviousLocalNameChangeDate = entityPM.PreviousLocalNameChangeDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreviousNumber))
            {
                oldEntityPM.PreviousNumber = entityPM.PreviousNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreviousNumberChangeDate))
            {
                oldEntityPM.PreviousNumberChangeDate = entityPM.PreviousNumberChangeDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreviousChartOfAccountsId))
            {
                oldEntityPM.PreviousChartOfAccountsId = entityPM.PreviousChartOfAccountsId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreviousChartOfAccountsChangeDate))
            {
                oldEntityPM.PreviousChartOfAccountsChangeDate = entityPM.PreviousChartOfAccountsChangeDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerGLAccountId))
            {
                oldEntityPM.CustomerGLAccountId = entityPM.CustomerGLAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RevaluationEnabled))
            {
                oldEntityPM.RevaluationEnabled = entityPM.RevaluationEnabled;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParentAccountId))
            {
                oldEntityPM.ParentAccountId = entityPM.ParentAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Category1Id))
            {
                oldEntityPM.Category1Id = entityPM.Category1Id;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Category2Id))
            {
                oldEntityPM.Category2Id = entityPM.Category2Id;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Category3Id))
            {
                oldEntityPM.Category3Id = entityPM.Category3Id;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Category4Id))
            {
                oldEntityPM.Category4Id = entityPM.Category4Id;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Category5Id))
            {
                oldEntityPM.Category5Id = entityPM.Category5Id;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsVATExempt))
            {
                oldEntityPM.IsVATExempt = entityPM.IsVATExempt;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeductionFileTypeId))
            {
                oldEntityPM.DeductionFileTypeId = entityPM.DeductionFileTypeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeductionFileNumber))
            {
                oldEntityPM.DeductionFileNumber = entityPM.DeductionFileNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AssessingOfficeCode))
            {
                oldEntityPM.AssessingOfficeCode = entityPM.AssessingOfficeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Occupation))
            {
                oldEntityPM.Occupation = entityPM.Occupation;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeductionTypeId))
            {
                oldEntityPM.DeductionTypeId = entityPM.DeductionTypeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsolidationVat))
            {
                oldEntityPM.ConsolidationVat = entityPM.ConsolidationVat;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsEquipmentVendor))
            {
                oldEntityPM.IsEquipmentVendor = entityPM.IsEquipmentVendor;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExcludeFromDeductionReport))
            {
                oldEntityPM.ExcludeFromDeductionReport = entityPM.ExcludeFromDeductionReport;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AllowEditChequePayToName))
            {
                oldEntityPM.AllowEditChequePayToName = entityPM.AllowEditChequePayToName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActiveForInterest))
            {
                oldEntityPM.ActiveForInterest = entityPM.ActiveForInterest;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestCalculationStartDate))
            {
                oldEntityPM.InterestCalculationStartDate = entityPM.InterestCalculationStartDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActiveForInterestCreditInvoice))
            {
                oldEntityPM.ActiveForInterestCreditInvoice = entityPM.ActiveForInterestCreditInvoice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestCreditLimit))
            {
                oldEntityPM.InterestCreditLimit = entityPM.InterestCreditLimit;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NameForPrintingCheques))
            {
                oldEntityPM.NameForPrintingCheques = entityPM.NameForPrintingCheques;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Smallcashbook))
            {
                oldEntityPM.Smallcashbook = entityPM.Smallcashbook;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MinimumInterestInvoiceBilling))
            {
                oldEntityPM.MinimumInterestInvoiceBilling = entityPM.MinimumInterestInvoiceBilling;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReportingAsAnotherDocument))
            {
                oldEntityPM.ReportingAsAnotherDocument = entityPM.ReportingAsAnotherDocument;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreditAllotmentPercentage))
            {
                oldEntityPM.CreditAllotmentPercentage = entityPM.CreditAllotmentPercentage;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(GLAccountPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.LocalName)) //T4 find type == nText 
            {
                entityPM.LocalName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LocalName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.PreviousLocalName)) //T4 find type == nText 
            {
                entityPM.PreviousLocalName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.PreviousLocalName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Occupation)) //T4 find type == nText 
            {
                entityPM.Occupation = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Occupation));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.NameForPrintingCheques)) //T4 find type == nText 
            {
                entityPM.NameForPrintingCheques = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.NameForPrintingCheques));
            }
            entityPM.EncodeBase64NVARCHARFieldsBy=null;
		}


	    public void AddPOCOPropertyName(POCOPropertyNames pocoPropertyName)
        {
            CustomMappedPOCOProperties.Add(pocoPropertyName);
        }

        public void AddPMPropertyName(PMPropertyNames pocoPropertyName)
        {
            CustomMappedPMProperties.Add(pocoPropertyName);
        }
		
		private void BuildSearchFieldsGenerated(GLAccountPM entityPM, GLAccount entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 