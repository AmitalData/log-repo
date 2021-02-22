
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
	         WithPaper, 
	         IsClosedForFollowUp, 
	         IsClassificationRemarks, 
	         IsControllerRemarks, 
	         PreClassification, 
	         SearchFields, 
	         ExceptionReasonsList, 
	         ClassifiedUserId, 
	         ControllerUserId, 
	         CollectorUserId, 
	         NewFile, 
	         Favorite, 
	         LastStatusName, 
	         LastStatusDate, 
	         OrderMoney, 
	         Team, 
	         ImporterFile, 
	         FileOpenDate, 
	         FclLcl, 
	         PackageQuantity, 
	         ForwarderId,
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
	         WithPaper, 
	         IsClosedForFollowUp, 
	         IsClassificationRemarks, 
	         IsControllerRemarks, 
	         PreClassification, 
	         SearchFields, 
	         ExceptionReasonsList, 
	         ClassifiedUserId, 
	         ControllerUserId, 
	         CollectorUserId, 
	         NewFile, 
	         Favorite, 
	         IsCancelled, 
	         ClassifiedUserName, 
	         ControllerUserName, 
	         CollectorUserName, 
	         LastStatusName, 
	         LastStatusDate, 
	         OrderMoney, 
	         Team, 
	         ImporterFile, 
	         FileOpenDate, 
	         IsClose, 
	         FclLcl, 
	         PackageQuantity, 
	         ForwarderId, 
	         ForwarderName,
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExceptionReasonsList))
            {
				entityPOCO.ExceptionReasonsList = entityPM.ExceptionReasonsList;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClassifiedUserId))
            {
				entityPOCO.ClassifiedUserId = entityPM.ClassifiedUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ControllerUserId))
            {
				entityPOCO.ControllerUserId = entityPM.ControllerUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CollectorUserId))
            {
				entityPOCO.CollectorUserId = entityPM.CollectorUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NewFile))
            {
				entityPOCO.NewFile = entityPM.NewFile;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Favorite))
            {
				entityPOCO.Favorite = entityPM.Favorite;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastStatusName))
            {
				entityPOCO.LastStatusName = entityPM.LastStatusName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastStatusDate))
            {
				entityPOCO.LastStatusDate = entityPM.LastStatusDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OrderMoney))
            {
				entityPOCO.OrderMoney = entityPM.OrderMoney;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Team))
            {
				entityPOCO.Team = entityPM.Team;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterFile))
            {
				entityPOCO.ImporterFile = entityPM.ImporterFile;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FileOpenDate))
            {
				entityPOCO.FileOpenDate = entityPM.FileOpenDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FclLcl))
            {
				entityPOCO.FclLcl = entityPM.FclLcl;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageQuantity))
            {
				entityPOCO.PackageQuantity = entityPM.PackageQuantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForwarderId))
            {
				entityPOCO.ForwarderId = entityPM.ForwarderId;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExceptionReasonsList))
            {
					entityPM.ExceptionReasonsList = entityPOCO.ExceptionReasonsList;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClassifiedUserId))
            {
					entityPM.ClassifiedUserId = entityPOCO.ClassifiedUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ControllerUserId))
            {
					entityPM.ControllerUserId = entityPOCO.ControllerUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CollectorUserId))
            {
					entityPM.CollectorUserId = entityPOCO.CollectorUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NewFile))
            {
					entityPM.NewFile = entityPOCO.NewFile;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Favorite))
            {
					entityPM.Favorite = entityPOCO.Favorite;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastStatusName))
            {
					entityPM.LastStatusName = entityPOCO.LastStatusName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastStatusDate))
            {
					entityPM.LastStatusDate = entityPOCO.LastStatusDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OrderMoney))
            {
					entityPM.OrderMoney = entityPOCO.OrderMoney;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Team))
            {
					entityPM.Team = entityPOCO.Team;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ImporterFile))
            {
					entityPM.ImporterFile = entityPOCO.ImporterFile;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FileOpenDate))
            {
					entityPM.FileOpenDate = entityPOCO.FileOpenDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FclLcl))
            {
					entityPM.FclLcl = entityPOCO.FclLcl;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageQuantity))
            {
					entityPM.PackageQuantity = entityPOCO.PackageQuantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ForwarderId))
            {
					entityPM.ForwarderId = entityPOCO.ForwarderId;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExceptionReasonsList))
            {
                oldEntityPM.ExceptionReasonsList = entityPM.ExceptionReasonsList;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClassifiedUserId))
            {
                oldEntityPM.ClassifiedUserId = entityPM.ClassifiedUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ControllerUserId))
            {
                oldEntityPM.ControllerUserId = entityPM.ControllerUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CollectorUserId))
            {
                oldEntityPM.CollectorUserId = entityPM.CollectorUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NewFile))
            {
                oldEntityPM.NewFile = entityPM.NewFile;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Favorite))
            {
                oldEntityPM.Favorite = entityPM.Favorite;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastStatusName))
            {
                oldEntityPM.LastStatusName = entityPM.LastStatusName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastStatusDate))
            {
                oldEntityPM.LastStatusDate = entityPM.LastStatusDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OrderMoney))
            {
                oldEntityPM.OrderMoney = entityPM.OrderMoney;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Team))
            {
                oldEntityPM.Team = entityPM.Team;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterFile))
            {
                oldEntityPM.ImporterFile = entityPM.ImporterFile;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FileOpenDate))
            {
                oldEntityPM.FileOpenDate = entityPM.FileOpenDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FclLcl))
            {
                oldEntityPM.FclLcl = entityPM.FclLcl;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageQuantity))
            {
                oldEntityPM.PackageQuantity = entityPM.PackageQuantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForwarderId))
            {
                oldEntityPM.ForwarderId = entityPM.ForwarderId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DeclarationReferantDataPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.LastStatusName)) //T4 find type == nText 
            {
                entityPM.LastStatusName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LastStatusName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Team)) //T4 find type == nText 
            {
                entityPM.Team = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Team));
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
		
		private void BuildSearchFieldsGenerated(DeclarationReferantDataPM entityPM, DeclarationReferantData entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 