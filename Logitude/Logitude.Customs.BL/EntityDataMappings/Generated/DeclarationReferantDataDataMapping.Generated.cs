
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
   
   public partial class DeclarationReferantDataDataMapping: IMapping<DeclarationReferantDataPM, DeclarationReferantData>,IMappingEncodeBase64NVARCHARFields<DeclarationReferantDataPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         Tenant, 
	         OrderNumber, 
	         VendorId, 
	         ArrivalDate, 
	         EstimatedArrivalDate, 
	         Weight, 
	         ClassificationStatus, 
	         ControllerStatus, 
	         CollectionOfMoneyStatus, 
	         FollowUpDate, 
	         IsExceptional, 
	         WithPaper, 
	         IsClosedForFollowUp, 
	         IsClassificationRemarks, 
	         IsControllerRemarks, 
	         PreClassification,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         Tenant, 
	         OrderNumber, 
	         VendorId, 
	         ArrivalDate, 
	         EstimatedArrivalDate, 
	         Weight, 
	         ClassificationStatus, 
	         ControllerStatus, 
	         CollectionOfMoneyStatus, 
	         FollowUpDate, 
	         IsExceptional, 
	         WithPaper, 
	         IsClosedForFollowUp, 
	         IsClassificationRemarks, 
	         IsControllerRemarks, 
	         PreClassification,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DeclarationReferantDataPM entityPM, DeclarationReferantData entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OrderNumber))
            {
				entityPOCO.OrderNumber = entityPM.OrderNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VendorId))
            {
				entityPOCO.VendorId = entityPM.VendorId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ArrivalDate))
            {
				entityPOCO.ArrivalDate = entityPM.ArrivalDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EstimatedArrivalDate))
            {
				entityPOCO.EstimatedArrivalDate = entityPM.EstimatedArrivalDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Weight))
            {
				entityPOCO.Weight = entityPM.Weight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClassificationStatus))
            {
				entityPOCO.ClassificationStatus = entityPM.ClassificationStatus;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ControllerStatus))
            {
				entityPOCO.ControllerStatus = entityPM.ControllerStatus;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CollectionOfMoneyStatus))
            {
				entityPOCO.CollectionOfMoneyStatus = entityPM.CollectionOfMoneyStatus;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FollowUpDate))
            {
				entityPOCO.FollowUpDate = entityPM.FollowUpDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsExceptional))
            {
				entityPOCO.IsExceptional = entityPM.IsExceptional;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WithPaper))
            {
				entityPOCO.WithPaper = entityPM.WithPaper;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosedForFollowUp))
            {
				entityPOCO.IsClosedForFollowUp = entityPM.IsClosedForFollowUp;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClassificationRemarks))
            {
				entityPOCO.IsClassificationRemarks = entityPM.IsClassificationRemarks;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsControllerRemarks))
            {
				entityPOCO.IsControllerRemarks = entityPM.IsControllerRemarks;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreClassification))
            {
				entityPOCO.PreClassification = entityPM.PreClassification;
			}
			}

		public void POCOToPM(DeclarationReferantDataPM entityPM, DeclarationReferantData entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OrderNumber))
            {
					entityPM.OrderNumber = entityPOCO.OrderNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VendorId))
            {
					entityPM.VendorId = entityPOCO.VendorId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ArrivalDate))
            {
					entityPM.ArrivalDate = entityPOCO.ArrivalDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EstimatedArrivalDate))
            {
					entityPM.EstimatedArrivalDate = entityPOCO.EstimatedArrivalDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Weight))
            {
					entityPM.Weight = entityPOCO.Weight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClassificationStatus))
            {
					entityPM.ClassificationStatus = entityPOCO.ClassificationStatus;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ControllerStatus))
            {
					entityPM.ControllerStatus = entityPOCO.ControllerStatus;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CollectionOfMoneyStatus))
            {
					entityPM.CollectionOfMoneyStatus = entityPOCO.CollectionOfMoneyStatus;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FollowUpDate))
            {
					entityPM.FollowUpDate = entityPOCO.FollowUpDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsExceptional))
            {
					entityPM.IsExceptional = entityPOCO.IsExceptional;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WithPaper))
            {
					entityPM.WithPaper = entityPOCO.WithPaper;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsClosedForFollowUp))
            {
					entityPM.IsClosedForFollowUp = entityPOCO.IsClosedForFollowUp;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsClassificationRemarks))
            {
					entityPM.IsClassificationRemarks = entityPOCO.IsClassificationRemarks;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsControllerRemarks))
            {
					entityPM.IsControllerRemarks = entityPOCO.IsControllerRemarks;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PreClassification))
            {
					entityPM.PreClassification = entityPOCO.PreClassification;
            }

		}

		public void PMToOldPM(DeclarationReferantDataPM entityPM, DeclarationReferantDataPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OrderNumber))
            {
                oldEntityPM.OrderNumber = entityPM.OrderNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VendorId))
            {
                oldEntityPM.VendorId = entityPM.VendorId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ArrivalDate))
            {
                oldEntityPM.ArrivalDate = entityPM.ArrivalDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EstimatedArrivalDate))
            {
                oldEntityPM.EstimatedArrivalDate = entityPM.EstimatedArrivalDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Weight))
            {
                oldEntityPM.Weight = entityPM.Weight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClassificationStatus))
            {
                oldEntityPM.ClassificationStatus = entityPM.ClassificationStatus;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ControllerStatus))
            {
                oldEntityPM.ControllerStatus = entityPM.ControllerStatus;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CollectionOfMoneyStatus))
            {
                oldEntityPM.CollectionOfMoneyStatus = entityPM.CollectionOfMoneyStatus;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FollowUpDate))
            {
                oldEntityPM.FollowUpDate = entityPM.FollowUpDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsExceptional))
            {
                oldEntityPM.IsExceptional = entityPM.IsExceptional;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WithPaper))
            {
                oldEntityPM.WithPaper = entityPM.WithPaper;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosedForFollowUp))
            {
                oldEntityPM.IsClosedForFollowUp = entityPM.IsClosedForFollowUp;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClassificationRemarks))
            {
                oldEntityPM.IsClassificationRemarks = entityPM.IsClassificationRemarks;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsControllerRemarks))
            {
                oldEntityPM.IsControllerRemarks = entityPM.IsControllerRemarks;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreClassification))
            {
                oldEntityPM.PreClassification = entityPM.PreClassification;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DeclarationReferantDataPM entityPM)
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
	 