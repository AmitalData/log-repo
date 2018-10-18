
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
   
   public partial class SupplierInvoiceItemDataMapping: IMapping<SupplierInvoiceItemPM, SupplierInvoiceItem>,IMappingEncodeBase64NVARCHARFields<SupplierInvoiceItemPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         CounterKey, 
	         LineNumber, 
	         ItemCode, 
	         OriginCountryCode, 
	         ClassificationCode, 
	         DangerousClassificationCode, 
	         DangerousPackingGroupTypeCode, 
	         ItemPrice, 
	         NonCustomsItemPrice, 
	         WholeSaleItemPrice, 
	         ManufactureIdentifier, 
	         CustomsBookTypeCode, 
	         TaxExemptCode, 
	         OptionalTamaPercentage, 
	         SalesTaxExemptionTypeCode, 
	         Tenant, 
	         SequenceNumeric, 
	         ItemPriceCurrencyCode, 
	         NonCustomsItemPriceCurCode, 
	         WholeSaleItemPriceCurrencyCode, 
	         ActualInvoiceLines, 
	         TradeAgreementCode, 
	         StatisticQuantity, 
	         InvoiceQuantity, 
	         AdditionalQuantity, 
	         InvoiceQuantityType, 
	         StatisticQuantityType, 
	         AdditionalQuantityType, 
	         PreferenceDocumentNumber, 
	         ItemDescription, 
	         CertificatesStatusCode, 
	         IsUsed, 
	         DeferredCustomsTax, 
	         DeferredPurchaseTax, 
	         VehicleStatus, 
	         ItemAdditionalStatus, 
	         ItemHash, 
	         ParentLineNumber, 
	         NotForAccumaltion, 
	         IsParent, 
	         UnfInvoiceLine, 
	         OrderByLineNo, 
	         LastCopyFromOrderNo, 
	         ClasifiedRemarks, 
	         SearchFields,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         CounterKey, 
	         LineNumber, 
	         ItemCode, 
	         OriginCountryCode, 
	         ClassificationCode, 
	         DangerousClassificationCode, 
	         DangerousPackingGroupTypeCode, 
	         ItemPrice, 
	         NonCustomsItemPrice, 
	         WholeSaleItemPrice, 
	         ManufactureIdentifier, 
	         CustomsBookTypeCode, 
	         TaxExemptCode, 
	         OptionalTamaPercentage, 
	         SalesTaxExemptionTypeCode, 
	         Tenant, 
	         SequenceNumeric, 
	         ItemPriceCurrencyCode, 
	         NonCustomsItemPriceCurCode, 
	         WholeSaleItemPriceCurrencyCode, 
	         ActualInvoiceLines, 
	         OriginCountryName, 
	         TradeAgreementCode, 
	         TradeAgreementName, 
	         StatisticQuantity, 
	         InvoiceQuantity, 
	         AdditionalQuantity, 
	         InvoiceQuantityType, 
	         StatisticQuantityType, 
	         AdditionalQuantityType, 
	         InvoiceQuantityTypeName, 
	         StatisticQuantityTypeName, 
	         AdditionalQuantityTypeName, 
	         TaxExemptName, 
	         PreferenceDocumentNumber, 
	         ItemDescription, 
	         IsItemChanged, 
	         CatalogNumber, 
	         CertificatesStatusCode, 
	         IsUsed, 
	         InvoiceNumber, 
	         DeferredCustomsTax, 
	         DeferredPurchaseTax, 
	         VehicleStatus, 
	         ItemAdditionalStatus, 
	         ItemHash, 
	         ParentLineNumber, 
	         NotForAccumaltion, 
	         IsParent, 
	         UnfInvoiceLine, 
	         OrderByLineNo, 
	         IsCopy, 
	         LastCopyFromOrderNo, 
	         ClasifiedRemarks, 
	         SearchFields,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(SupplierInvoiceItemPM entityPM, SupplierInvoiceItem entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ItemCode))
            {
				entityPOCO.ItemCode = entityPM.ItemCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginCountryCode))
            {
				entityPOCO.OriginCountryCode = entityPM.OriginCountryCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClassificationCode))
            {
				entityPOCO.ClassificationCode = entityPM.ClassificationCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousClassificationCode))
            {
				entityPOCO.DangerousClassificationCode = entityPM.DangerousClassificationCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousPackingGroupTypeCode))
            {
				entityPOCO.DangerousPackingGroupTypeCode = entityPM.DangerousPackingGroupTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ItemPrice))
            {
				entityPOCO.ItemPrice = entityPM.ItemPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonCustomsItemPrice))
            {
				entityPOCO.NonCustomsItemPrice = entityPM.NonCustomsItemPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WholeSaleItemPrice))
            {
				entityPOCO.WholeSaleItemPrice = entityPM.WholeSaleItemPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ManufactureIdentifier))
            {
				entityPOCO.ManufactureIdentifier = entityPM.ManufactureIdentifier;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsBookTypeCode))
            {
				entityPOCO.CustomsBookTypeCode = entityPM.CustomsBookTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxExemptCode))
            {
				entityPOCO.TaxExemptCode = entityPM.TaxExemptCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OptionalTamaPercentage))
            {
				entityPOCO.OptionalTamaPercentage = entityPM.OptionalTamaPercentage;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SalesTaxExemptionTypeCode))
            {
				entityPOCO.SalesTaxExemptionTypeCode = entityPM.SalesTaxExemptionTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SequenceNumeric))
            {
				entityPOCO.SequenceNumeric = entityPM.SequenceNumeric;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ItemPriceCurrencyCode))
            {
				entityPOCO.ItemPriceCurrencyCode = entityPM.ItemPriceCurrencyCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonCustomsItemPriceCurCode))
            {
				entityPOCO.NonCustomsItemPriceCurCode = entityPM.NonCustomsItemPriceCurCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WholeSaleItemPriceCurrencyCode))
            {
				entityPOCO.WholeSaleItemPriceCurrencyCode = entityPM.WholeSaleItemPriceCurrencyCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActualInvoiceLines))
            {
				entityPOCO.ActualInvoiceLines = entityPM.ActualInvoiceLines;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TradeAgreementCode))
            {
				entityPOCO.TradeAgreementCode = entityPM.TradeAgreementCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatisticQuantity))
            {
				entityPOCO.StatisticQuantity = entityPM.StatisticQuantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceQuantity))
            {
				entityPOCO.InvoiceQuantity = entityPM.InvoiceQuantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AdditionalQuantity))
            {
				entityPOCO.AdditionalQuantity = entityPM.AdditionalQuantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceQuantityType))
            {
				entityPOCO.InvoiceQuantityType = entityPM.InvoiceQuantityType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatisticQuantityType))
            {
				entityPOCO.StatisticQuantityType = entityPM.StatisticQuantityType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AdditionalQuantityType))
            {
				entityPOCO.AdditionalQuantityType = entityPM.AdditionalQuantityType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreferenceDocumentNumber))
            {
				entityPOCO.PreferenceDocumentNumber = entityPM.PreferenceDocumentNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ItemDescription))
            {
				entityPOCO.ItemDescription = entityPM.ItemDescription;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CertificatesStatusCode))
            {
				entityPOCO.CertificatesStatusCode = entityPM.CertificatesStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsUsed))
            {
				entityPOCO.IsUsed = entityPM.IsUsed;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeferredCustomsTax))
            {
				entityPOCO.DeferredCustomsTax = entityPM.DeferredCustomsTax;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeferredPurchaseTax))
            {
				entityPOCO.DeferredPurchaseTax = entityPM.DeferredPurchaseTax;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VehicleStatus))
            {
				entityPOCO.VehicleStatus = entityPM.VehicleStatus;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ItemAdditionalStatus))
            {
				entityPOCO.ItemAdditionalStatus = entityPM.ItemAdditionalStatus;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ItemHash))
            {
				entityPOCO.ItemHash = entityPM.ItemHash;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParentLineNumber))
            {
				entityPOCO.ParentLineNumber = entityPM.ParentLineNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NotForAccumaltion))
            {
				entityPOCO.NotForAccumaltion = entityPM.NotForAccumaltion;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsParent))
            {
				entityPOCO.IsParent = entityPM.IsParent;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UnfInvoiceLine))
            {
				entityPOCO.UnfInvoiceLine = entityPM.UnfInvoiceLine;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OrderByLineNo))
            {
				entityPOCO.OrderByLineNo = entityPM.OrderByLineNo;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastCopyFromOrderNo))
            {
				entityPOCO.LastCopyFromOrderNo = entityPM.LastCopyFromOrderNo;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClasifiedRemarks))
            {
				entityPOCO.ClasifiedRemarks = entityPM.ClasifiedRemarks;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(SupplierInvoiceItemPM entityPM, SupplierInvoiceItem entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CounterKey))
            {
					entityPM.CounterKey = entityPOCO.CounterKey;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
					entityPM.LineNumber = entityPOCO.LineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ItemCode))
            {
					entityPM.ItemCode = entityPOCO.ItemCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OriginCountryCode))
            {
					entityPM.OriginCountryCode = entityPOCO.OriginCountryCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClassificationCode))
            {
					entityPM.ClassificationCode = entityPOCO.ClassificationCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DangerousClassificationCode))
            {
					entityPM.DangerousClassificationCode = entityPOCO.DangerousClassificationCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DangerousPackingGroupTypeCode))
            {
					entityPM.DangerousPackingGroupTypeCode = entityPOCO.DangerousPackingGroupTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ItemPrice))
            {
					entityPM.ItemPrice = entityPOCO.ItemPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NonCustomsItemPrice))
            {
					entityPM.NonCustomsItemPrice = entityPOCO.NonCustomsItemPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WholeSaleItemPrice))
            {
					entityPM.WholeSaleItemPrice = entityPOCO.WholeSaleItemPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ManufactureIdentifier))
            {
					entityPM.ManufactureIdentifier = entityPOCO.ManufactureIdentifier;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsBookTypeCode))
            {
					entityPM.CustomsBookTypeCode = entityPOCO.CustomsBookTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TaxExemptCode))
            {
					entityPM.TaxExemptCode = entityPOCO.TaxExemptCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OptionalTamaPercentage))
            {
					entityPM.OptionalTamaPercentage = entityPOCO.OptionalTamaPercentage;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SalesTaxExemptionTypeCode))
            {
					entityPM.SalesTaxExemptionTypeCode = entityPOCO.SalesTaxExemptionTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SequenceNumeric))
            {
					entityPM.SequenceNumeric = entityPOCO.SequenceNumeric;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ItemPriceCurrencyCode))
            {
					entityPM.ItemPriceCurrencyCode = entityPOCO.ItemPriceCurrencyCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NonCustomsItemPriceCurCode))
            {
					entityPM.NonCustomsItemPriceCurCode = entityPOCO.NonCustomsItemPriceCurCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WholeSaleItemPriceCurrencyCode))
            {
					entityPM.WholeSaleItemPriceCurrencyCode = entityPOCO.WholeSaleItemPriceCurrencyCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActualInvoiceLines))
            {
					entityPM.ActualInvoiceLines = entityPOCO.ActualInvoiceLines;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TradeAgreementCode))
            {
					entityPM.TradeAgreementCode = entityPOCO.TradeAgreementCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatisticQuantity))
            {
					entityPM.StatisticQuantity = entityPOCO.StatisticQuantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InvoiceQuantity))
            {
					entityPM.InvoiceQuantity = entityPOCO.InvoiceQuantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AdditionalQuantity))
            {
					entityPM.AdditionalQuantity = entityPOCO.AdditionalQuantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InvoiceQuantityType))
            {
					entityPM.InvoiceQuantityType = entityPOCO.InvoiceQuantityType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatisticQuantityType))
            {
					entityPM.StatisticQuantityType = entityPOCO.StatisticQuantityType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AdditionalQuantityType))
            {
					entityPM.AdditionalQuantityType = entityPOCO.AdditionalQuantityType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PreferenceDocumentNumber))
            {
					entityPM.PreferenceDocumentNumber = entityPOCO.PreferenceDocumentNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ItemDescription))
            {
					entityPM.ItemDescription = entityPOCO.ItemDescription;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CertificatesStatusCode))
            {
					entityPM.CertificatesStatusCode = entityPOCO.CertificatesStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsUsed))
            {
					entityPM.IsUsed = entityPOCO.IsUsed;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeferredCustomsTax))
            {
					entityPM.DeferredCustomsTax = entityPOCO.DeferredCustomsTax;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeferredPurchaseTax))
            {
					entityPM.DeferredPurchaseTax = entityPOCO.DeferredPurchaseTax;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VehicleStatus))
            {
					entityPM.VehicleStatus = entityPOCO.VehicleStatus;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ItemAdditionalStatus))
            {
					entityPM.ItemAdditionalStatus = entityPOCO.ItemAdditionalStatus;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ItemHash))
            {
					entityPM.ItemHash = entityPOCO.ItemHash;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ParentLineNumber))
            {
					entityPM.ParentLineNumber = entityPOCO.ParentLineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NotForAccumaltion))
            {
					entityPM.NotForAccumaltion = entityPOCO.NotForAccumaltion;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsParent))
            {
					entityPM.IsParent = entityPOCO.IsParent;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UnfInvoiceLine))
            {
					entityPM.UnfInvoiceLine = entityPOCO.UnfInvoiceLine;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OrderByLineNo))
            {
					entityPM.OrderByLineNo = entityPOCO.OrderByLineNo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastCopyFromOrderNo))
            {
					entityPM.LastCopyFromOrderNo = entityPOCO.LastCopyFromOrderNo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClasifiedRemarks))
            {
					entityPM.ClasifiedRemarks = entityPOCO.ClasifiedRemarks;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

		}

		public void PMToOldPM(SupplierInvoiceItemPM entityPM, SupplierInvoiceItemPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ItemCode))
            {
                oldEntityPM.ItemCode = entityPM.ItemCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginCountryCode))
            {
                oldEntityPM.OriginCountryCode = entityPM.OriginCountryCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClassificationCode))
            {
                oldEntityPM.ClassificationCode = entityPM.ClassificationCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousClassificationCode))
            {
                oldEntityPM.DangerousClassificationCode = entityPM.DangerousClassificationCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousPackingGroupTypeCode))
            {
                oldEntityPM.DangerousPackingGroupTypeCode = entityPM.DangerousPackingGroupTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ItemPrice))
            {
                oldEntityPM.ItemPrice = entityPM.ItemPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonCustomsItemPrice))
            {
                oldEntityPM.NonCustomsItemPrice = entityPM.NonCustomsItemPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WholeSaleItemPrice))
            {
                oldEntityPM.WholeSaleItemPrice = entityPM.WholeSaleItemPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ManufactureIdentifier))
            {
                oldEntityPM.ManufactureIdentifier = entityPM.ManufactureIdentifier;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsBookTypeCode))
            {
                oldEntityPM.CustomsBookTypeCode = entityPM.CustomsBookTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxExemptCode))
            {
                oldEntityPM.TaxExemptCode = entityPM.TaxExemptCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OptionalTamaPercentage))
            {
                oldEntityPM.OptionalTamaPercentage = entityPM.OptionalTamaPercentage;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SalesTaxExemptionTypeCode))
            {
                oldEntityPM.SalesTaxExemptionTypeCode = entityPM.SalesTaxExemptionTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SequenceNumeric))
            {
                oldEntityPM.SequenceNumeric = entityPM.SequenceNumeric;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ItemPriceCurrencyCode))
            {
                oldEntityPM.ItemPriceCurrencyCode = entityPM.ItemPriceCurrencyCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonCustomsItemPriceCurCode))
            {
                oldEntityPM.NonCustomsItemPriceCurCode = entityPM.NonCustomsItemPriceCurCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WholeSaleItemPriceCurrencyCode))
            {
                oldEntityPM.WholeSaleItemPriceCurrencyCode = entityPM.WholeSaleItemPriceCurrencyCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActualInvoiceLines))
            {
                oldEntityPM.ActualInvoiceLines = entityPM.ActualInvoiceLines;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TradeAgreementCode))
            {
                oldEntityPM.TradeAgreementCode = entityPM.TradeAgreementCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatisticQuantity))
            {
                oldEntityPM.StatisticQuantity = entityPM.StatisticQuantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceQuantity))
            {
                oldEntityPM.InvoiceQuantity = entityPM.InvoiceQuantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AdditionalQuantity))
            {
                oldEntityPM.AdditionalQuantity = entityPM.AdditionalQuantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceQuantityType))
            {
                oldEntityPM.InvoiceQuantityType = entityPM.InvoiceQuantityType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatisticQuantityType))
            {
                oldEntityPM.StatisticQuantityType = entityPM.StatisticQuantityType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AdditionalQuantityType))
            {
                oldEntityPM.AdditionalQuantityType = entityPM.AdditionalQuantityType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreferenceDocumentNumber))
            {
                oldEntityPM.PreferenceDocumentNumber = entityPM.PreferenceDocumentNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ItemDescription))
            {
                oldEntityPM.ItemDescription = entityPM.ItemDescription;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CertificatesStatusCode))
            {
                oldEntityPM.CertificatesStatusCode = entityPM.CertificatesStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsUsed))
            {
                oldEntityPM.IsUsed = entityPM.IsUsed;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeferredCustomsTax))
            {
                oldEntityPM.DeferredCustomsTax = entityPM.DeferredCustomsTax;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeferredPurchaseTax))
            {
                oldEntityPM.DeferredPurchaseTax = entityPM.DeferredPurchaseTax;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VehicleStatus))
            {
                oldEntityPM.VehicleStatus = entityPM.VehicleStatus;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ItemAdditionalStatus))
            {
                oldEntityPM.ItemAdditionalStatus = entityPM.ItemAdditionalStatus;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ItemHash))
            {
                oldEntityPM.ItemHash = entityPM.ItemHash;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParentLineNumber))
            {
                oldEntityPM.ParentLineNumber = entityPM.ParentLineNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NotForAccumaltion))
            {
                oldEntityPM.NotForAccumaltion = entityPM.NotForAccumaltion;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsParent))
            {
                oldEntityPM.IsParent = entityPM.IsParent;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UnfInvoiceLine))
            {
                oldEntityPM.UnfInvoiceLine = entityPM.UnfInvoiceLine;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OrderByLineNo))
            {
                oldEntityPM.OrderByLineNo = entityPM.OrderByLineNo;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastCopyFromOrderNo))
            {
                oldEntityPM.LastCopyFromOrderNo = entityPM.LastCopyFromOrderNo;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClasifiedRemarks))
            {
                oldEntityPM.ClasifiedRemarks = entityPM.ClasifiedRemarks;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(SupplierInvoiceItemPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.TaxExemptCode)) //T4 find type == nText 
            {
                entityPM.TaxExemptCode = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.TaxExemptCode));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ItemDescription)) //T4 find type == nText 
            {
                entityPM.ItemDescription = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ItemDescription));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ClasifiedRemarks)) //T4 find type == nText 
            {
                entityPM.ClasifiedRemarks = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ClasifiedRemarks));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
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
		
		private void BuildSearchFieldsGenerated(SupplierInvoiceItemPM entityPM, SupplierInvoiceItem entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 