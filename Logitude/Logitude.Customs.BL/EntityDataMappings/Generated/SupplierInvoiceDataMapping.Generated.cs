
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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class SupplierInvoiceDataMapping: IMapping<SupplierInvoicePM, SupplierInvoice>,IMappingEncodeBase64NVARCHARFields<SupplierInvoicePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         InvoiceCounterKey, 
	         Tenant, 
	         SequenceNumeric, 
	         InvoiceNumber, 
	         IssueDate, 
	         AccountTypeCode, 
	         IsPreference, 
	         PreferenceDocumentTypeCode, 
	         PaymentTypeCode, 
	         InvoiceCurrencyTypeCode, 
	         InvoiceAmount, 
	         ActualPayedCurrencyTypeCode, 
	         ActualPayedAmount, 
	         VendorId, 
	         IncotermCode, 
	         IssueCountryCode, 
	         PaymentTermsCode, 
	         TotalFreightInFreightCurrency, 
	         TotalFreightInNIS, 
	         ExchangeRate, 
	         InsruanceCurrencyTypeCode, 
	         InsuranceAmount, 
	         InsruancePercentage, 
	         FreightCurrencyTypeCode, 
	         IsAccumalated, 
	         AccumalationStateCode, 
	         UnfInvoiceCounterKey, 
	         VendorComissionPercentage, 
	         InvoiceAmountInUSD, 
	         ChangeInSupplierInvoice,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         InvoiceCounterKey, 
	         Tenant, 
	         SequenceNumeric, 
	         InvoiceNumber, 
	         IssueDate, 
	         AccountTypeCode, 
	         IsPreference, 
	         PreferenceDocumentTypeCode, 
	         PaymentTypeCode, 
	         InvoiceCurrencyTypeCode, 
	         InvoiceAmount, 
	         ActualPayedCurrencyTypeCode, 
	         ActualPayedAmount, 
	         VendorId, 
	         IncotermCode, 
	         IssueCountryCode, 
	         PaymentTermsCode, 
	         TotalFreightInFreightCurrency, 
	         TotalFreightInNIS, 
	         ExchangeRate, 
	         IssueCountryName, 
	         PreferenceDocumentTypeName, 
	         InsruanceCurrencyTypeCode, 
	         InsuranceAmount, 
	         VendorName, 
	         InsruancePercentage, 
	         FreightCurrencyTypeCode, 
	         IsPrimarySupplierInvoice, 
	         ActualPayedCurrencyTypeName, 
	         InvoiceItemLastLineNumber, 
	         FullItemsCount, 
	         IncotermName, 
	         MaxSequence, 
	         IsAccumalated, 
	         AccumalationStateCode, 
	         FullParentsCount, 
	         FullChildrenCount, 
	         UnfInvoiceCounterKey, 
	         VendorComissionPercentage, 
	         IsValueForCustomsOnly, 
	         InvoiceAmountInUSD, 
	         ChangeInSupplierInvoice,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(SupplierInvoicePM entityPM, SupplierInvoice entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SequenceNumeric))
            {
				entityPOCO.SequenceNumeric = entityPM.SequenceNumeric;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceNumber))
            {
				entityPOCO.InvoiceNumber = entityPM.InvoiceNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IssueDate))
            {
				entityPOCO.IssueDate = entityPM.IssueDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountTypeCode))
            {
				entityPOCO.AccountTypeCode = entityPM.AccountTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsPreference))
            {
				entityPOCO.IsPreference = entityPM.IsPreference;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreferenceDocumentTypeCode))
            {
				entityPOCO.PreferenceDocumentTypeCode = entityPM.PreferenceDocumentTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentTypeCode))
            {
				entityPOCO.PaymentTypeCode = entityPM.PaymentTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceCurrencyTypeCode))
            {
				entityPOCO.InvoiceCurrencyTypeCode = entityPM.InvoiceCurrencyTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceAmount))
            {
				entityPOCO.InvoiceAmount = entityPM.InvoiceAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActualPayedCurrencyTypeCode))
            {
				entityPOCO.ActualPayedCurrencyTypeCode = entityPM.ActualPayedCurrencyTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActualPayedAmount))
            {
				entityPOCO.ActualPayedAmount = entityPM.ActualPayedAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VendorId))
            {
				entityPOCO.VendorId = entityPM.VendorId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IncotermCode))
            {
				entityPOCO.IncotermCode = entityPM.IncotermCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IssueCountryCode))
            {
				entityPOCO.IssueCountryCode = entityPM.IssueCountryCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentTermsCode))
            {
				entityPOCO.PaymentTermsCode = entityPM.PaymentTermsCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalFreightInFreightCurrency))
            {
				entityPOCO.TotalFreightInFreightCurrency = entityPM.TotalFreightInFreightCurrency;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalFreightInNIS))
            {
				entityPOCO.TotalFreightInNIS = entityPM.TotalFreightInNIS;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExchangeRate))
            {
				entityPOCO.ExchangeRate = entityPM.ExchangeRate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InsruanceCurrencyTypeCode))
            {
				entityPOCO.InsruanceCurrencyTypeCode = entityPM.InsruanceCurrencyTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InsuranceAmount))
            {
				entityPOCO.InsuranceAmount = entityPM.InsuranceAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InsruancePercentage))
            {
				entityPOCO.InsruancePercentage = entityPM.InsruancePercentage;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FreightCurrencyTypeCode))
            {
				entityPOCO.FreightCurrencyTypeCode = entityPM.FreightCurrencyTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAccumalated))
            {
				entityPOCO.IsAccumalated = entityPM.IsAccumalated;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccumalationStateCode))
            {
				entityPOCO.AccumalationStateCode = entityPM.AccumalationStateCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UnfInvoiceCounterKey))
            {
				entityPOCO.UnfInvoiceCounterKey = entityPM.UnfInvoiceCounterKey;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VendorComissionPercentage))
            {
				entityPOCO.VendorComissionPercentage = entityPM.VendorComissionPercentage;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceAmountInUSD))
            {
				entityPOCO.InvoiceAmountInUSD = entityPM.InvoiceAmountInUSD;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChangeInSupplierInvoice))
            {
				entityPOCO.ChangeInSupplierInvoice = entityPM.ChangeInSupplierInvoice;
			}
			}

		public void POCOToPM(SupplierInvoicePM entityPM, SupplierInvoice entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InvoiceCounterKey))
            {
					entityPM.InvoiceCounterKey = entityPOCO.InvoiceCounterKey;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SequenceNumeric))
            {
					entityPM.SequenceNumeric = entityPOCO.SequenceNumeric;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InvoiceNumber))
            {
					entityPM.InvoiceNumber = entityPOCO.InvoiceNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IssueDate))
            {
					entityPM.IssueDate = entityPOCO.IssueDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AccountTypeCode))
            {
					entityPM.AccountTypeCode = entityPOCO.AccountTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsPreference))
            {
					entityPM.IsPreference = entityPOCO.IsPreference;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PreferenceDocumentTypeCode))
            {
					entityPM.PreferenceDocumentTypeCode = entityPOCO.PreferenceDocumentTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentTypeCode))
            {
					entityPM.PaymentTypeCode = entityPOCO.PaymentTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InvoiceCurrencyTypeCode))
            {
					entityPM.InvoiceCurrencyTypeCode = entityPOCO.InvoiceCurrencyTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InvoiceAmount))
            {
					entityPM.InvoiceAmount = entityPOCO.InvoiceAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActualPayedCurrencyTypeCode))
            {
					entityPM.ActualPayedCurrencyTypeCode = entityPOCO.ActualPayedCurrencyTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActualPayedAmount))
            {
					entityPM.ActualPayedAmount = entityPOCO.ActualPayedAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VendorId))
            {
					entityPM.VendorId = entityPOCO.VendorId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IncotermCode))
            {
					entityPM.IncotermCode = entityPOCO.IncotermCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IssueCountryCode))
            {
					entityPM.IssueCountryCode = entityPOCO.IssueCountryCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentTermsCode))
            {
					entityPM.PaymentTermsCode = entityPOCO.PaymentTermsCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TotalFreightInFreightCurrency))
            {
					entityPM.TotalFreightInFreightCurrency = entityPOCO.TotalFreightInFreightCurrency;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TotalFreightInNIS))
            {
					entityPM.TotalFreightInNIS = entityPOCO.TotalFreightInNIS;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExchangeRate))
            {
					entityPM.ExchangeRate = entityPOCO.ExchangeRate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InsruanceCurrencyTypeCode))
            {
					entityPM.InsruanceCurrencyTypeCode = entityPOCO.InsruanceCurrencyTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InsuranceAmount))
            {
					entityPM.InsuranceAmount = entityPOCO.InsuranceAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InsruancePercentage))
            {
					entityPM.InsruancePercentage = entityPOCO.InsruancePercentage;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FreightCurrencyTypeCode))
            {
					entityPM.FreightCurrencyTypeCode = entityPOCO.FreightCurrencyTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsAccumalated))
            {
					entityPM.IsAccumalated = entityPOCO.IsAccumalated;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AccumalationStateCode))
            {
					entityPM.AccumalationStateCode = entityPOCO.AccumalationStateCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UnfInvoiceCounterKey))
            {
					entityPM.UnfInvoiceCounterKey = entityPOCO.UnfInvoiceCounterKey;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VendorComissionPercentage))
            {
					entityPM.VendorComissionPercentage = entityPOCO.VendorComissionPercentage;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InvoiceAmountInUSD))
            {
					entityPM.InvoiceAmountInUSD = entityPOCO.InvoiceAmountInUSD;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChangeInSupplierInvoice))
            {
					entityPM.ChangeInSupplierInvoice = entityPOCO.ChangeInSupplierInvoice;
            }

		}

		public void PMToOldPM(SupplierInvoicePM entityPM, SupplierInvoicePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SequenceNumeric))
            {
                oldEntityPM.SequenceNumeric = entityPM.SequenceNumeric;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceNumber))
            {
                oldEntityPM.InvoiceNumber = entityPM.InvoiceNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IssueDate))
            {
                oldEntityPM.IssueDate = entityPM.IssueDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountTypeCode))
            {
                oldEntityPM.AccountTypeCode = entityPM.AccountTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsPreference))
            {
                oldEntityPM.IsPreference = entityPM.IsPreference;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreferenceDocumentTypeCode))
            {
                oldEntityPM.PreferenceDocumentTypeCode = entityPM.PreferenceDocumentTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentTypeCode))
            {
                oldEntityPM.PaymentTypeCode = entityPM.PaymentTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceCurrencyTypeCode))
            {
                oldEntityPM.InvoiceCurrencyTypeCode = entityPM.InvoiceCurrencyTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceAmount))
            {
                oldEntityPM.InvoiceAmount = entityPM.InvoiceAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActualPayedCurrencyTypeCode))
            {
                oldEntityPM.ActualPayedCurrencyTypeCode = entityPM.ActualPayedCurrencyTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActualPayedAmount))
            {
                oldEntityPM.ActualPayedAmount = entityPM.ActualPayedAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VendorId))
            {
                oldEntityPM.VendorId = entityPM.VendorId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IncotermCode))
            {
                oldEntityPM.IncotermCode = entityPM.IncotermCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IssueCountryCode))
            {
                oldEntityPM.IssueCountryCode = entityPM.IssueCountryCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentTermsCode))
            {
                oldEntityPM.PaymentTermsCode = entityPM.PaymentTermsCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalFreightInFreightCurrency))
            {
                oldEntityPM.TotalFreightInFreightCurrency = entityPM.TotalFreightInFreightCurrency;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalFreightInNIS))
            {
                oldEntityPM.TotalFreightInNIS = entityPM.TotalFreightInNIS;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExchangeRate))
            {
                oldEntityPM.ExchangeRate = entityPM.ExchangeRate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InsruanceCurrencyTypeCode))
            {
                oldEntityPM.InsruanceCurrencyTypeCode = entityPM.InsruanceCurrencyTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InsuranceAmount))
            {
                oldEntityPM.InsuranceAmount = entityPM.InsuranceAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InsruancePercentage))
            {
                oldEntityPM.InsruancePercentage = entityPM.InsruancePercentage;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FreightCurrencyTypeCode))
            {
                oldEntityPM.FreightCurrencyTypeCode = entityPM.FreightCurrencyTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAccumalated))
            {
                oldEntityPM.IsAccumalated = entityPM.IsAccumalated;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccumalationStateCode))
            {
                oldEntityPM.AccumalationStateCode = entityPM.AccumalationStateCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UnfInvoiceCounterKey))
            {
                oldEntityPM.UnfInvoiceCounterKey = entityPM.UnfInvoiceCounterKey;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VendorComissionPercentage))
            {
                oldEntityPM.VendorComissionPercentage = entityPM.VendorComissionPercentage;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceAmountInUSD))
            {
                oldEntityPM.InvoiceAmountInUSD = entityPM.InvoiceAmountInUSD;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChangeInSupplierInvoice))
            {
                oldEntityPM.ChangeInSupplierInvoice = entityPM.ChangeInSupplierInvoice;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(SupplierInvoicePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

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
			  
   }
}
	 