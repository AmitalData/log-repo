
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
   
   public partial class BankAccountDataMapping: IMapping<BankAccountPM, BankAccount>,IMappingEncodeBase64NVARCHARFields<BankAccountPM>
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
	         LocalName, 
	         EnglishName, 
	         BankId, 
	         BranchNumber, 
	         AccountNumber, 
	         GLAccountId, 
	         DeferredGLAccountId, 
	         IBAN, 
	         SwiftCode, 
	         BranchAddress, 
	         Inactive, 
	         ChequeCounter, 
	         LastPageNumber, 
	         LastPageEndDate, 
	         LastPageCloseBalance, 
	         TransferGLAcccountId, 
	         CurrencyId, 
	         PrintingBranchNumber, 
	         PrintingAccountNumber,
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
	         LocalName, 
	         EnglishName, 
	         BankId, 
	         BranchNumber, 
	         AccountNumber, 
	         GLAccountId, 
	         DeferredGLAccountId, 
	         IBAN, 
	         SwiftCode, 
	         BranchAddress, 
	         Inactive, 
	         ChequeCounter, 
	         GLAccountNumber, 
	         DeferedGLAccountNumber, 
	         BankCode, 
	         GLAccountCurrencyId, 
	         LastPageNumber, 
	         LastPageEndDate, 
	         LastPageCloseBalance, 
	         TransferGLAcccountId, 
	         DeferedGLAccountLocalName, 
	         TransferGLAcccountNumber, 
	         TransferGLAcccountLocalName, 
	         DeferedGLAccountEnglishName, 
	         TransferGLAcccountEnglishName, 
	         IsBankPageEvent, 
	         CurrencyId, 
	         CurrencyName, 
	         CurrencyCode, 
	         CurrencySign, 
	         PrintingBranchNumber, 
	         PrintingAccountNumber,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(BankAccountPM entityPM, BankAccount entityPOCO)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalName))
            {
				entityPOCO.LocalName = entityPM.LocalName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishName))
            {
				entityPOCO.EnglishName = entityPM.EnglishName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BankId))
            {
				entityPOCO.BankId = entityPM.BankId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BranchNumber))
            {
				entityPOCO.BranchNumber = entityPM.BranchNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountNumber))
            {
				entityPOCO.AccountNumber = entityPM.AccountNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GLAccountId))
            {
				entityPOCO.GLAccountId = entityPM.GLAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeferredGLAccountId))
            {
				entityPOCO.DeferredGLAccountId = entityPM.DeferredGLAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IBAN))
            {
				entityPOCO.IBAN = entityPM.IBAN;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SwiftCode))
            {
				entityPOCO.SwiftCode = entityPM.SwiftCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BranchAddress))
            {
				entityPOCO.BranchAddress = entityPM.BranchAddress;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Inactive))
            {
				entityPOCO.Inactive = entityPM.Inactive;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChequeCounter))
            {
				entityPOCO.ChequeCounter = entityPM.ChequeCounter;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastPageNumber))
            {
				entityPOCO.LastPageNumber = entityPM.LastPageNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastPageEndDate))
            {
				entityPOCO.LastPageEndDate = entityPM.LastPageEndDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastPageCloseBalance))
            {
				entityPOCO.LastPageCloseBalance = entityPM.LastPageCloseBalance;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransferGLAcccountId))
            {
				entityPOCO.TransferGLAcccountId = entityPM.TransferGLAcccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrencyId))
            {
				entityPOCO.CurrencyId = entityPM.CurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PrintingBranchNumber))
            {
				entityPOCO.PrintingBranchNumber = entityPM.PrintingBranchNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PrintingAccountNumber))
            {
				entityPOCO.PrintingAccountNumber = entityPM.PrintingAccountNumber;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(BankAccountPM entityPM, BankAccount entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalName))
            {
					entityPM.LocalName = entityPOCO.LocalName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishName))
            {
					entityPM.EnglishName = entityPOCO.EnglishName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BankId))
            {
					entityPM.BankId = entityPOCO.BankId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BranchNumber))
            {
					entityPM.BranchNumber = entityPOCO.BranchNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AccountNumber))
            {
					entityPM.AccountNumber = entityPOCO.AccountNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GLAccountId))
            {
					entityPM.GLAccountId = entityPOCO.GLAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeferredGLAccountId))
            {
					entityPM.DeferredGLAccountId = entityPOCO.DeferredGLAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IBAN))
            {
					entityPM.IBAN = entityPOCO.IBAN;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SwiftCode))
            {
					entityPM.SwiftCode = entityPOCO.SwiftCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BranchAddress))
            {
					entityPM.BranchAddress = entityPOCO.BranchAddress;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Inactive))
            {
					entityPM.Inactive = entityPOCO.Inactive;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChequeCounter))
            {
					entityPM.ChequeCounter = entityPOCO.ChequeCounter;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastPageNumber))
            {
					entityPM.LastPageNumber = entityPOCO.LastPageNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastPageEndDate))
            {
					entityPM.LastPageEndDate = entityPOCO.LastPageEndDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastPageCloseBalance))
            {
					entityPM.LastPageCloseBalance = entityPOCO.LastPageCloseBalance;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransferGLAcccountId))
            {
					entityPM.TransferGLAcccountId = entityPOCO.TransferGLAcccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CurrencyId))
            {
					entityPM.CurrencyId = entityPOCO.CurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PrintingBranchNumber))
            {
					entityPM.PrintingBranchNumber = entityPOCO.PrintingBranchNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PrintingAccountNumber))
            {
					entityPM.PrintingAccountNumber = entityPOCO.PrintingAccountNumber;
            }

		}

		public void PMToOldPM(BankAccountPM entityPM, BankAccountPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalName))
            {
                oldEntityPM.LocalName = entityPM.LocalName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishName))
            {
                oldEntityPM.EnglishName = entityPM.EnglishName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BankId))
            {
                oldEntityPM.BankId = entityPM.BankId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BranchNumber))
            {
                oldEntityPM.BranchNumber = entityPM.BranchNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountNumber))
            {
                oldEntityPM.AccountNumber = entityPM.AccountNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GLAccountId))
            {
                oldEntityPM.GLAccountId = entityPM.GLAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeferredGLAccountId))
            {
                oldEntityPM.DeferredGLAccountId = entityPM.DeferredGLAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IBAN))
            {
                oldEntityPM.IBAN = entityPM.IBAN;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SwiftCode))
            {
                oldEntityPM.SwiftCode = entityPM.SwiftCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BranchAddress))
            {
                oldEntityPM.BranchAddress = entityPM.BranchAddress;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Inactive))
            {
                oldEntityPM.Inactive = entityPM.Inactive;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChequeCounter))
            {
                oldEntityPM.ChequeCounter = entityPM.ChequeCounter;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastPageNumber))
            {
                oldEntityPM.LastPageNumber = entityPM.LastPageNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastPageEndDate))
            {
                oldEntityPM.LastPageEndDate = entityPM.LastPageEndDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastPageCloseBalance))
            {
                oldEntityPM.LastPageCloseBalance = entityPM.LastPageCloseBalance;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransferGLAcccountId))
            {
                oldEntityPM.TransferGLAcccountId = entityPM.TransferGLAcccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrencyId))
            {
                oldEntityPM.CurrencyId = entityPM.CurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PrintingBranchNumber))
            {
                oldEntityPM.PrintingBranchNumber = entityPM.PrintingBranchNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PrintingAccountNumber))
            {
                oldEntityPM.PrintingAccountNumber = entityPM.PrintingAccountNumber;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(BankAccountPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.LocalName)) //T4 find type == nText 
            {
                entityPM.LocalName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LocalName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.BranchAddress)) //T4 find type == nText 
            {
                entityPM.BranchAddress = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.BranchAddress));
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
		
		private void BuildSearchFieldsGenerated(BankAccountPM entityPM, BankAccount entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 