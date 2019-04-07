
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
   
   public partial class DeclarationCourierStatusDataMapping: IMapping<DeclarationCourierStatusPM, DeclarationCourierStatus>,IMappingEncodeBase64NVARCHARFields<DeclarationCourierStatusPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         Tenant, 
	         CourierManifestStatusCode, 
	         CourierDeclarationStatusCode, 
	         CourierPaymentStatusCode, 
	         IsCourierMissingClassification, 
	         IsClosedForFollowUp, 
	         HighLowValue, 
	         DocumentStatusCode, 
	         TotalInvoiceAmountInUSD, 
	         CourierPendingReasonCode, 
	         PendingRemarks, 
	         SpecialActionStatus, 
	         FastIndividualProcessCode, 
	         ManualProcessCode, 
	         TerminalSuspentionNumber, 
	         LastMileStatusCode, 
	         LastMileStatusDate, 
	         LastMileStatusRemarks,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         Tenant, 
	         CourierManifestStatusCode, 
	         CourierManifestStatusName, 
	         CourierDeclarationStatusCode, 
	         CourierDeclarationStatusName, 
	         CourierPaymentStatusCode, 
	         CourierPaymentStatusName, 
	         IsCourierMissingClassification, 
	         IsClosedForFollowUp, 
	         HighLowValue, 
	         DocumentStatusCode, 
	         CourierHawb, 
	         ProcedureCurrentCode, 
	         ProcedureCurrentName, 
	         ImporterCode, 
	         CourierMasterId, 
	         CourierCustomStatusName, 
	         DeclarationStatusTypeName, 
	         CustomerName, 
	         IsMNFTab, 
	         IsPAYTab, 
	         IsDECTab, 
	         IsDOCTab, 
	         IsSVGTab, 
	         IsMNFRTab, 
	         IsDECRTab, 
	         IsHOLDTab, 
	         IsACCTab, 
	         CourierSearchFields, 
	         CourierCustomStatusCode, 
	         ImporterName, 
	         TotalInvoiceAmountInUSD, 
	         DeclarationNumber, 
	         CourierPendingReasonCode, 
	         CourierPendingReasonName, 
	         PendingRemarks, 
	         CourierSuspentionReasonName, 
	         AcceptanceStatusCode, 
	         MamanStatusCode, 
	         MamanErrorXml, 
	         CourierSuspentionCode, 
	         CourierSuspentionName, 
	         SpecialActionStatus, 
	         SpecialActionsErrorXml, 
	         CourierPendingReasonErrorPlace, 
	         FastIndividualProcessCode, 
	         ManualProcessCode, 
	         TerminalSuspentionNumber, 
	         LastMileStatusCode, 
	         LastMileStatusDate, 
	         LastMileStatusRemarks,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DeclarationCourierStatusPM entityPM, DeclarationCourierStatus entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierManifestStatusCode))
            {
				entityPOCO.CourierManifestStatusCode = entityPM.CourierManifestStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierDeclarationStatusCode))
            {
				entityPOCO.CourierDeclarationStatusCode = entityPM.CourierDeclarationStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierPaymentStatusCode))
            {
				entityPOCO.CourierPaymentStatusCode = entityPM.CourierPaymentStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCourierMissingClassification))
            {
				entityPOCO.IsCourierMissingClassification = entityPM.IsCourierMissingClassification;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosedForFollowUp))
            {
				entityPOCO.IsClosedForFollowUp = entityPM.IsClosedForFollowUp;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HighLowValue))
            {
				entityPOCO.HighLowValue = entityPM.HighLowValue;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentStatusCode))
            {
				entityPOCO.DocumentStatusCode = entityPM.DocumentStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalInvoiceAmountInUSD))
            {
				entityPOCO.TotalInvoiceAmountInUSD = entityPM.TotalInvoiceAmountInUSD;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierPendingReasonCode))
            {
				entityPOCO.CourierPendingReasonCode = entityPM.CourierPendingReasonCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PendingRemarks))
            {
				entityPOCO.PendingRemarks = entityPM.PendingRemarks;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SpecialActionStatus))
            {
				entityPOCO.SpecialActionStatus = entityPM.SpecialActionStatus;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FastIndividualProcessCode))
            {
				entityPOCO.FastIndividualProcessCode = entityPM.FastIndividualProcessCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ManualProcessCode))
            {
				entityPOCO.ManualProcessCode = entityPM.ManualProcessCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TerminalSuspentionNumber))
            {
				entityPOCO.TerminalSuspentionNumber = entityPM.TerminalSuspentionNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastMileStatusCode))
            {
				entityPOCO.LastMileStatusCode = entityPM.LastMileStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastMileStatusDate))
            {
				entityPOCO.LastMileStatusDate = entityPM.LastMileStatusDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastMileStatusRemarks))
            {
				entityPOCO.LastMileStatusRemarks = entityPM.LastMileStatusRemarks;
			}
			}

		public void POCOToPM(DeclarationCourierStatusPM entityPM, DeclarationCourierStatus entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CourierManifestStatusCode))
            {
					entityPM.CourierManifestStatusCode = entityPOCO.CourierManifestStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CourierDeclarationStatusCode))
            {
					entityPM.CourierDeclarationStatusCode = entityPOCO.CourierDeclarationStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CourierPaymentStatusCode))
            {
					entityPM.CourierPaymentStatusCode = entityPOCO.CourierPaymentStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCourierMissingClassification))
            {
					entityPM.IsCourierMissingClassification = entityPOCO.IsCourierMissingClassification;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsClosedForFollowUp))
            {
					entityPM.IsClosedForFollowUp = entityPOCO.IsClosedForFollowUp;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.HighLowValue))
            {
					entityPM.HighLowValue = entityPOCO.HighLowValue;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DocumentStatusCode))
            {
					entityPM.DocumentStatusCode = entityPOCO.DocumentStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TotalInvoiceAmountInUSD))
            {
					entityPM.TotalInvoiceAmountInUSD = entityPOCO.TotalInvoiceAmountInUSD;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CourierPendingReasonCode))
            {
					entityPM.CourierPendingReasonCode = entityPOCO.CourierPendingReasonCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PendingRemarks))
            {
					entityPM.PendingRemarks = entityPOCO.PendingRemarks;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SpecialActionStatus))
            {
					entityPM.SpecialActionStatus = entityPOCO.SpecialActionStatus;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FastIndividualProcessCode))
            {
					entityPM.FastIndividualProcessCode = entityPOCO.FastIndividualProcessCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ManualProcessCode))
            {
					entityPM.ManualProcessCode = entityPOCO.ManualProcessCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TerminalSuspentionNumber))
            {
					entityPM.TerminalSuspentionNumber = entityPOCO.TerminalSuspentionNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastMileStatusCode))
            {
					entityPM.LastMileStatusCode = entityPOCO.LastMileStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastMileStatusDate))
            {
					entityPM.LastMileStatusDate = entityPOCO.LastMileStatusDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastMileStatusRemarks))
            {
					entityPM.LastMileStatusRemarks = entityPOCO.LastMileStatusRemarks;
            }

		}

		public void PMToOldPM(DeclarationCourierStatusPM entityPM, DeclarationCourierStatusPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierManifestStatusCode))
            {
                oldEntityPM.CourierManifestStatusCode = entityPM.CourierManifestStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierDeclarationStatusCode))
            {
                oldEntityPM.CourierDeclarationStatusCode = entityPM.CourierDeclarationStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierPaymentStatusCode))
            {
                oldEntityPM.CourierPaymentStatusCode = entityPM.CourierPaymentStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCourierMissingClassification))
            {
                oldEntityPM.IsCourierMissingClassification = entityPM.IsCourierMissingClassification;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosedForFollowUp))
            {
                oldEntityPM.IsClosedForFollowUp = entityPM.IsClosedForFollowUp;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HighLowValue))
            {
                oldEntityPM.HighLowValue = entityPM.HighLowValue;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentStatusCode))
            {
                oldEntityPM.DocumentStatusCode = entityPM.DocumentStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalInvoiceAmountInUSD))
            {
                oldEntityPM.TotalInvoiceAmountInUSD = entityPM.TotalInvoiceAmountInUSD;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierPendingReasonCode))
            {
                oldEntityPM.CourierPendingReasonCode = entityPM.CourierPendingReasonCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PendingRemarks))
            {
                oldEntityPM.PendingRemarks = entityPM.PendingRemarks;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SpecialActionStatus))
            {
                oldEntityPM.SpecialActionStatus = entityPM.SpecialActionStatus;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FastIndividualProcessCode))
            {
                oldEntityPM.FastIndividualProcessCode = entityPM.FastIndividualProcessCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ManualProcessCode))
            {
                oldEntityPM.ManualProcessCode = entityPM.ManualProcessCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TerminalSuspentionNumber))
            {
                oldEntityPM.TerminalSuspentionNumber = entityPM.TerminalSuspentionNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastMileStatusCode))
            {
                oldEntityPM.LastMileStatusCode = entityPM.LastMileStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastMileStatusDate))
            {
                oldEntityPM.LastMileStatusDate = entityPM.LastMileStatusDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastMileStatusRemarks))
            {
                oldEntityPM.LastMileStatusRemarks = entityPM.LastMileStatusRemarks;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DeclarationCourierStatusPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.PendingRemarks)) //T4 find type == nText 
            {
                entityPM.PendingRemarks = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.PendingRemarks));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.LastMileStatusRemarks)) //T4 find type == nText 
            {
                entityPM.LastMileStatusRemarks = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LastMileStatusRemarks));
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
	 