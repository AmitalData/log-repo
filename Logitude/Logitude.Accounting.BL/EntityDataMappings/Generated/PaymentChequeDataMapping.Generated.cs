
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
   
   public partial class PaymentChequeDataMapping: IMapping<PaymentChequePM, PaymentCheque>,IMappingEncodeBase64NVARCHARFields<PaymentChequePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         CreatedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         SearchFields, 
	         InternalNumber, 
	         ChequeNumber, 
	         PayToGLAccountId, 
	         PayToName, 
	         BankAccountId, 
	         BankAccountGLAccountId, 
	         LocalAmount, 
	         CurrencyId, 
	         ForeignAmount, 
	         ExchangeRate, 
	         ValueDate, 
	         PrintDate, 
	         ApproveDate, 
	         ApprovedByUserId, 
	         IsCancelled, 
	         CancelledByUserId, 
	         CancelledDate, 
	         CancellationRemarks, 
	         PaymentChequeStatusCode, 
	         EntityId, 
	         ObjectTableId, 
	         Notes, 
	         UniqueField, 
	         APPaymentId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         CreatedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         SearchFields, 
	         InternalNumber, 
	         ChequeNumber, 
	         PayToGLAccountId, 
	         PayToName, 
	         BankAccountId, 
	         BankAccountGLAccountId, 
	         LocalAmount, 
	         CurrencyId, 
	         ForeignAmount, 
	         ExchangeRate, 
	         ValueDate, 
	         PrintDate, 
	         ApproveDate, 
	         ApprovedByUserId, 
	         IsCancelled, 
	         CancelledByUserId, 
	         CancelledDate, 
	         CancellationRemarks, 
	         PaymentChequeStatusCode, 
	         EntityId, 
	         ObjectTableId, 
	         GLAccountNumber, 
	         BankAccountName, 
	         PaymentChequeStatusName, 
	         BankAccountCode, 
	         Notes, 
	         PaymentChequeLineLastLine, 
	         GLAccountCurrencyId, 
	         BankGLAccountCurrencyId, 
	         IsGLAccountMultiCurrency, 
	         IsBankGlAccountMultiCur, 
	         UniqueField, 
	         GLAccountName, 
	         CurrencyCode, 
	         JournalNumber, 
	         JournalId, 
	         StatusEnglishName, 
	         BankLocalName, 
	         BankEnglishName, 
	         APPaymentId, 
	         CancelledByAPPayment, 
	         APPaymentNo,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(PaymentChequePM entityPM, PaymentCheque entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InternalNumber))
            {
				entityPOCO.InternalNumber = entityPM.InternalNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChequeNumber))
            {
				entityPOCO.ChequeNumber = entityPM.ChequeNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PayToGLAccountId))
            {
				entityPOCO.PayToGLAccountId = entityPM.PayToGLAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PayToName))
            {
				entityPOCO.PayToName = entityPM.PayToName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BankAccountId))
            {
				entityPOCO.BankAccountId = entityPM.BankAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BankAccountGLAccountId))
            {
				entityPOCO.BankAccountGLAccountId = entityPM.BankAccountGLAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalAmount))
            {
				entityPOCO.LocalAmount = entityPM.LocalAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrencyId))
            {
				entityPOCO.CurrencyId = entityPM.CurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForeignAmount))
            {
				entityPOCO.ForeignAmount = entityPM.ForeignAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExchangeRate))
            {
				entityPOCO.ExchangeRate = entityPM.ExchangeRate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ValueDate))
            {
				entityPOCO.ValueDate = entityPM.ValueDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PrintDate))
            {
				entityPOCO.PrintDate = entityPM.PrintDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ApproveDate))
            {
				entityPOCO.ApproveDate = entityPM.ApproveDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ApprovedByUserId))
            {
				entityPOCO.ApprovedByUserId = entityPM.ApprovedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
				entityPOCO.IsCancelled = entityPM.IsCancelled;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CancelledByUserId))
            {
				entityPOCO.CancelledByUserId = entityPM.CancelledByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CancelledDate))
            {
				entityPOCO.CancelledDate = entityPM.CancelledDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CancellationRemarks))
            {
				entityPOCO.CancellationRemarks = entityPM.CancellationRemarks;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentChequeStatusCode))
            {
				entityPOCO.PaymentChequeStatusCode = entityPM.PaymentChequeStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId))
            {
				entityPOCO.EntityId = entityPM.EntityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId))
            {
				entityPOCO.ObjectTableId = entityPM.ObjectTableId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
				entityPOCO.Notes = entityPM.Notes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UniqueField))
            {
				entityPOCO.UniqueField = entityPM.UniqueField;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.APPaymentId))
            {
				entityPOCO.APPaymentId = entityPM.APPaymentId;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(PaymentChequePM entityPM, PaymentCheque entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InternalNumber))
            {
					entityPM.InternalNumber = entityPOCO.InternalNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChequeNumber))
            {
					entityPM.ChequeNumber = entityPOCO.ChequeNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PayToGLAccountId))
            {
					entityPM.PayToGLAccountId = entityPOCO.PayToGLAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PayToName))
            {
					entityPM.PayToName = entityPOCO.PayToName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BankAccountId))
            {
					entityPM.BankAccountId = entityPOCO.BankAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BankAccountGLAccountId))
            {
					entityPM.BankAccountGLAccountId = entityPOCO.BankAccountGLAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalAmount))
            {
					entityPM.LocalAmount = entityPOCO.LocalAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CurrencyId))
            {
					entityPM.CurrencyId = entityPOCO.CurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ForeignAmount))
            {
					entityPM.ForeignAmount = entityPOCO.ForeignAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExchangeRate))
            {
					entityPM.ExchangeRate = entityPOCO.ExchangeRate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ValueDate))
            {
					entityPM.ValueDate = entityPOCO.ValueDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PrintDate))
            {
					entityPM.PrintDate = entityPOCO.PrintDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ApproveDate))
            {
					entityPM.ApproveDate = entityPOCO.ApproveDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ApprovedByUserId))
            {
					entityPM.ApprovedByUserId = entityPOCO.ApprovedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCancelled))
            {
					entityPM.IsCancelled = entityPOCO.IsCancelled;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CancelledByUserId))
            {
					entityPM.CancelledByUserId = entityPOCO.CancelledByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CancelledDate))
            {
					entityPM.CancelledDate = entityPOCO.CancelledDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CancellationRemarks))
            {
					entityPM.CancellationRemarks = entityPOCO.CancellationRemarks;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentChequeStatusCode))
            {
					entityPM.PaymentChequeStatusCode = entityPOCO.PaymentChequeStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityId))
            {
					entityPM.EntityId = entityPOCO.EntityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ObjectTableId))
            {
					entityPM.ObjectTableId = entityPOCO.ObjectTableId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Notes))
            {
					entityPM.Notes = entityPOCO.Notes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UniqueField))
            {
					entityPM.UniqueField = entityPOCO.UniqueField;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.APPaymentId))
            {
					entityPM.APPaymentId = entityPOCO.APPaymentId;
            }

		}

		public void PMToOldPM(PaymentChequePM entityPM, PaymentChequePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InternalNumber))
            {
                oldEntityPM.InternalNumber = entityPM.InternalNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChequeNumber))
            {
                oldEntityPM.ChequeNumber = entityPM.ChequeNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PayToGLAccountId))
            {
                oldEntityPM.PayToGLAccountId = entityPM.PayToGLAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PayToName))
            {
                oldEntityPM.PayToName = entityPM.PayToName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BankAccountId))
            {
                oldEntityPM.BankAccountId = entityPM.BankAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BankAccountGLAccountId))
            {
                oldEntityPM.BankAccountGLAccountId = entityPM.BankAccountGLAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalAmount))
            {
                oldEntityPM.LocalAmount = entityPM.LocalAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrencyId))
            {
                oldEntityPM.CurrencyId = entityPM.CurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForeignAmount))
            {
                oldEntityPM.ForeignAmount = entityPM.ForeignAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExchangeRate))
            {
                oldEntityPM.ExchangeRate = entityPM.ExchangeRate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ValueDate))
            {
                oldEntityPM.ValueDate = entityPM.ValueDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PrintDate))
            {
                oldEntityPM.PrintDate = entityPM.PrintDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ApproveDate))
            {
                oldEntityPM.ApproveDate = entityPM.ApproveDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ApprovedByUserId))
            {
                oldEntityPM.ApprovedByUserId = entityPM.ApprovedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
                oldEntityPM.IsCancelled = entityPM.IsCancelled;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CancelledByUserId))
            {
                oldEntityPM.CancelledByUserId = entityPM.CancelledByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CancelledDate))
            {
                oldEntityPM.CancelledDate = entityPM.CancelledDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CancellationRemarks))
            {
                oldEntityPM.CancellationRemarks = entityPM.CancellationRemarks;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentChequeStatusCode))
            {
                oldEntityPM.PaymentChequeStatusCode = entityPM.PaymentChequeStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId))
            {
                oldEntityPM.EntityId = entityPM.EntityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId))
            {
                oldEntityPM.ObjectTableId = entityPM.ObjectTableId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
                oldEntityPM.Notes = entityPM.Notes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UniqueField))
            {
                oldEntityPM.UniqueField = entityPM.UniqueField;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.APPaymentId))
            {
                oldEntityPM.APPaymentId = entityPM.APPaymentId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(PaymentChequePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.PayToName)) //T4 find type == nText 
            {
                entityPM.PayToName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.PayToName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CancellationRemarks)) //T4 find type == nText 
            {
                entityPM.CancellationRemarks = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CancellationRemarks));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Notes)) //T4 find type == nText 
            {
                entityPM.Notes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Notes));
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
		
		private void BuildSearchFieldsGenerated(PaymentChequePM entityPM, PaymentCheque entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 