
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
   
   public partial class DeficitDataMapping: IMapping<DeficitPM, Deficit>,IMappingEncodeBase64NVARCHARFields<DeficitPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         NotificationTypeCode, 
	         DebtNotificationNumber, 
	         ProductionDate, 
	         DebtNotificationReason, 
	         RealesGoodsDescription, 
	         ValidityDateTo, 
	         PaymentOrderNumber, 
	         TapagId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         NotificationTypeCode, 
	         NotificationTypeName, 
	         DebtNotificationNumber, 
	         ProductionDate, 
	         DebtNotificationReason, 
	         RealesGoodsDescription, 
	         ValidityDateTo, 
	         PaymentOrderNumber, 
	         TapagNumber, 
	         LeadingFileNumber, 
	         TapagTypeCode, 
	         TapagTypeName, 
	         CustomerId, 
	         CustomerName, 
	         ImporterId, 
	         ImporterName, 
	         CustomsBranchCode, 
	         CustomsBranchName, 
	         ProfessionUnitTypeCode, 
	         ProfessionUnitTypeName, 
	         SpecializationTypeCode, 
	         SpecializationTypeName, 
	         CreateDate, 
	         FollowDate, 
	         ValidityDate, 
	         IsClosed, 
	         TapagId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DeficitPM entityPM, Deficit entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NotificationTypeCode))
            {
				entityPOCO.NotificationTypeCode = entityPM.NotificationTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DebtNotificationNumber))
            {
				entityPOCO.DebtNotificationNumber = entityPM.DebtNotificationNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProductionDate))
            {
				entityPOCO.ProductionDate = entityPM.ProductionDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DebtNotificationReason))
            {
				entityPOCO.DebtNotificationReason = entityPM.DebtNotificationReason;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RealesGoodsDescription))
            {
				entityPOCO.RealesGoodsDescription = entityPM.RealesGoodsDescription;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ValidityDateTo))
            {
				entityPOCO.ValidityDateTo = entityPM.ValidityDateTo;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentOrderNumber))
            {
				entityPOCO.PaymentOrderNumber = entityPM.PaymentOrderNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TapagId))
            {
				entityPOCO.TapagId = entityPM.TapagId;
			}
			}

		public void POCOToPM(DeficitPM entityPM, Deficit entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NotificationTypeCode))
            {
					entityPM.NotificationTypeCode = entityPOCO.NotificationTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DebtNotificationNumber))
            {
					entityPM.DebtNotificationNumber = entityPOCO.DebtNotificationNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProductionDate))
            {
					entityPM.ProductionDate = entityPOCO.ProductionDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DebtNotificationReason))
            {
					entityPM.DebtNotificationReason = entityPOCO.DebtNotificationReason;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RealesGoodsDescription))
            {
					entityPM.RealesGoodsDescription = entityPOCO.RealesGoodsDescription;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ValidityDateTo))
            {
					entityPM.ValidityDateTo = entityPOCO.ValidityDateTo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentOrderNumber))
            {
					entityPM.PaymentOrderNumber = entityPOCO.PaymentOrderNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TapagId))
            {
					entityPM.TapagId = entityPOCO.TapagId;
            }

		}

		public void PMToOldPM(DeficitPM entityPM, DeficitPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NotificationTypeCode))
            {
                oldEntityPM.NotificationTypeCode = entityPM.NotificationTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DebtNotificationNumber))
            {
                oldEntityPM.DebtNotificationNumber = entityPM.DebtNotificationNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProductionDate))
            {
                oldEntityPM.ProductionDate = entityPM.ProductionDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DebtNotificationReason))
            {
                oldEntityPM.DebtNotificationReason = entityPM.DebtNotificationReason;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RealesGoodsDescription))
            {
                oldEntityPM.RealesGoodsDescription = entityPM.RealesGoodsDescription;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ValidityDateTo))
            {
                oldEntityPM.ValidityDateTo = entityPM.ValidityDateTo;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentOrderNumber))
            {
                oldEntityPM.PaymentOrderNumber = entityPM.PaymentOrderNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TapagId))
            {
                oldEntityPM.TapagId = entityPM.TapagId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DeficitPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.DebtNotificationReason)) //T4 find type == nText 
            {
                entityPM.DebtNotificationReason = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.DebtNotificationReason));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.RealesGoodsDescription)) //T4 find type == nText 
            {
                entityPM.RealesGoodsDescription = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.RealesGoodsDescription));
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
	 