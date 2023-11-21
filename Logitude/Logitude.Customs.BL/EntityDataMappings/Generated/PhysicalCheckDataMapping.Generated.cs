
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
   
   public partial class PhysicalCheckDataMapping: IMapping<PhysicalCheckPM, PhysicalCheck>,IMappingEncodeBase64NVARCHARFields<PhysicalCheckPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         DeclarationId, 
	         StorageSiteCode, 
	         CheckSiteCode, 
	         QueueTypeCode, 
	         OperationCode, 
	         CheckId, 
	         ContainerNubmer, 
	         OpenDate, 
	         LimitDate, 
	         CargoIdentifierKey1, 
	         CargoIdentifierKey2, 
	         CargoIdentifierKey3, 
	         RowNumber, 
	         CheckEssence, 
	         IsClosed, 
	         SearchFields, 
	         StatusMessageCode, 
	         CargoTypeCode, 
	         InitiatorTypeCode, 
	         ImporterNumber, 
	         CargoIdentifierTypeCode, 
	         ConcurrencyGUID, 
	         IsComprehensiveCheck, 
	         CheckTypeCode, 
	         CustomerId, 
	         NoEscortRequired, 
	         VehicleChassisNumber, 
	         SearchResult, 
	         SealNumber, 
	         CheckAuthorityAttenderTypeID, 
	         CheckAuthorityAttenderTypeName, 
	         CheckAnwserStatus, 
	         RequestToAdvanceAQueue, 
	         RequestDetails, 
	         BringQueueForwardIndicatorS,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         DeclarationId, 
	         StorageSiteCode, 
	         StorageSiteName, 
	         CheckSiteCode, 
	         CheckSiteName, 
	         QueueTypeCode, 
	         QueueTypeName, 
	         OperationCode, 
	         CheckId, 
	         EntityTypeId, 
	         ContainerNubmer, 
	         OpenDate, 
	         LimitDate, 
	         CargoIdentifierKey1, 
	         CargoIdentifierKey2, 
	         CargoIdentifierKey3, 
	         RowNumber, 
	         CheckEssence, 
	         IsClosed, 
	         SearchFields, 
	         StatusMessageCode, 
	         CargoTypeCode, 
	         InitiatorTypeCode, 
	         ImporterNumber, 
	         CargoIdentifierTypeCode, 
	         OperationName, 
	         DeclarationNo, 
	         CargoIdentifierTypeName, 
	         CheckSiteId, 
	         CustomerCode, 
	         CustomerName, 
	         CustomFileNo, 
	         StatusMessageName, 
	         ConcurrencyGUID, 
	         NewConcurrencyGUID, 
	         IsComprehensiveCheck, 
	         CheckTypeCode, 
	         CheckTypeName, 
	         CustomerId, 
	         NoEscortRequired, 
	         VehicleChassisNumber, 
	         EndDate, 
	         SearchResult, 
	         SealNumber, 
	         CheckAuthorityAttenderTypeID, 
	         CheckAuthorityAttenderTypeName, 
	         CheckAnwserStatus, 
	         SearchResultName, 
	         RequestToAdvanceAQueue, 
	         RequestDetails, 
	         BringQueueForwardIndicatorS,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(PhysicalCheckPM entityPM, PhysicalCheck entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationId))
            {
				entityPOCO.DeclarationId = entityPM.DeclarationId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageSiteCode))
            {
				entityPOCO.StorageSiteCode = entityPM.StorageSiteCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CheckSiteCode))
            {
				entityPOCO.CheckSiteCode = entityPM.CheckSiteCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QueueTypeCode))
            {
				entityPOCO.QueueTypeCode = entityPM.QueueTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OperationCode))
            {
				entityPOCO.OperationCode = entityPM.OperationCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CheckId))
            {
				entityPOCO.CheckId = entityPM.CheckId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerNubmer))
            {
				entityPOCO.ContainerNubmer = entityPM.ContainerNubmer;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpenDate))
            {
				entityPOCO.OpenDate = entityPM.OpenDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LimitDate))
            {
				entityPOCO.LimitDate = entityPM.LimitDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoIdentifierKey1))
            {
				entityPOCO.CargoIdentifierKey1 = entityPM.CargoIdentifierKey1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoIdentifierKey2))
            {
				entityPOCO.CargoIdentifierKey2 = entityPM.CargoIdentifierKey2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoIdentifierKey3))
            {
				entityPOCO.CargoIdentifierKey3 = entityPM.CargoIdentifierKey3;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RowNumber))
            {
				entityPOCO.RowNumber = entityPM.RowNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CheckEssence))
            {
				entityPOCO.CheckEssence = entityPM.CheckEssence;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosed))
            {
				entityPOCO.IsClosed = entityPM.IsClosed;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusMessageCode))
            {
				entityPOCO.StatusMessageCode = entityPM.StatusMessageCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoTypeCode))
            {
				entityPOCO.CargoTypeCode = entityPM.CargoTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InitiatorTypeCode))
            {
				entityPOCO.InitiatorTypeCode = entityPM.InitiatorTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterNumber))
            {
				entityPOCO.ImporterNumber = entityPM.ImporterNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoIdentifierTypeCode))
            {
				entityPOCO.CargoIdentifierTypeCode = entityPM.CargoIdentifierTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConcurrencyGUID))
            {
				entityPOCO.ConcurrencyGUID = entityPM.ConcurrencyGUID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsComprehensiveCheck))
            {
				entityPOCO.IsComprehensiveCheck = entityPM.IsComprehensiveCheck;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CheckTypeCode))
            {
				entityPOCO.CheckTypeCode = entityPM.CheckTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
				entityPOCO.CustomerId = entityPM.CustomerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NoEscortRequired))
            {
				entityPOCO.NoEscortRequired = entityPM.NoEscortRequired;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VehicleChassisNumber))
            {
				entityPOCO.VehicleChassisNumber = entityPM.VehicleChassisNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchResult))
            {
				entityPOCO.SearchResult = entityPM.SearchResult;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SealNumber))
            {
				entityPOCO.SealNumber = entityPM.SealNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CheckAuthorityAttenderTypeID))
            {
				entityPOCO.CheckAuthorityAttenderTypeID = entityPM.CheckAuthorityAttenderTypeID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CheckAuthorityAttenderTypeName))
            {
				entityPOCO.CheckAuthorityAttenderTypeName = entityPM.CheckAuthorityAttenderTypeName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CheckAnwserStatus))
            {
				entityPOCO.CheckAnwserStatus = entityPM.CheckAnwserStatus;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestToAdvanceAQueue))
            {
				entityPOCO.RequestToAdvanceAQueue = entityPM.RequestToAdvanceAQueue;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestDetails))
            {
				entityPOCO.RequestDetails = entityPM.RequestDetails;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BringQueueForwardIndicatorS))
            {
				entityPOCO.BringQueueForwardIndicatorS = entityPM.BringQueueForwardIndicatorS;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(PhysicalCheckPM entityPM, PhysicalCheck entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StorageSiteCode))
            {
					entityPM.StorageSiteCode = entityPOCO.StorageSiteCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CheckSiteCode))
            {
					entityPM.CheckSiteCode = entityPOCO.CheckSiteCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QueueTypeCode))
            {
					entityPM.QueueTypeCode = entityPOCO.QueueTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OperationCode))
            {
					entityPM.OperationCode = entityPOCO.OperationCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CheckId))
            {
					entityPM.CheckId = entityPOCO.CheckId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContainerNubmer))
            {
					entityPM.ContainerNubmer = entityPOCO.ContainerNubmer;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OpenDate))
            {
					entityPM.OpenDate = entityPOCO.OpenDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LimitDate))
            {
					entityPM.LimitDate = entityPOCO.LimitDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CargoIdentifierKey1))
            {
					entityPM.CargoIdentifierKey1 = entityPOCO.CargoIdentifierKey1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CargoIdentifierKey2))
            {
					entityPM.CargoIdentifierKey2 = entityPOCO.CargoIdentifierKey2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CargoIdentifierKey3))
            {
					entityPM.CargoIdentifierKey3 = entityPOCO.CargoIdentifierKey3;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RowNumber))
            {
					entityPM.RowNumber = entityPOCO.RowNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CheckEssence))
            {
					entityPM.CheckEssence = entityPOCO.CheckEssence;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsClosed))
            {
					entityPM.IsClosed = entityPOCO.IsClosed;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatusMessageCode))
            {
					entityPM.StatusMessageCode = entityPOCO.StatusMessageCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CargoTypeCode))
            {
					entityPM.CargoTypeCode = entityPOCO.CargoTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InitiatorTypeCode))
            {
					entityPM.InitiatorTypeCode = entityPOCO.InitiatorTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ImporterNumber))
            {
					entityPM.ImporterNumber = entityPOCO.ImporterNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CargoIdentifierTypeCode))
            {
					entityPM.CargoIdentifierTypeCode = entityPOCO.CargoIdentifierTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConcurrencyGUID))
            {
					entityPM.ConcurrencyGUID = entityPOCO.ConcurrencyGUID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsComprehensiveCheck))
            {
					entityPM.IsComprehensiveCheck = entityPOCO.IsComprehensiveCheck;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CheckTypeCode))
            {
					entityPM.CheckTypeCode = entityPOCO.CheckTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerId))
            {
					entityPM.CustomerId = entityPOCO.CustomerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NoEscortRequired))
            {
					entityPM.NoEscortRequired = entityPOCO.NoEscortRequired;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VehicleChassisNumber))
            {
					entityPM.VehicleChassisNumber = entityPOCO.VehicleChassisNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchResult))
            {
					entityPM.SearchResult = entityPOCO.SearchResult;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SealNumber))
            {
					entityPM.SealNumber = entityPOCO.SealNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CheckAuthorityAttenderTypeID))
            {
					entityPM.CheckAuthorityAttenderTypeID = entityPOCO.CheckAuthorityAttenderTypeID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CheckAuthorityAttenderTypeName))
            {
					entityPM.CheckAuthorityAttenderTypeName = entityPOCO.CheckAuthorityAttenderTypeName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CheckAnwserStatus))
            {
					entityPM.CheckAnwserStatus = entityPOCO.CheckAnwserStatus;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequestToAdvanceAQueue))
            {
					entityPM.RequestToAdvanceAQueue = entityPOCO.RequestToAdvanceAQueue;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequestDetails))
            {
					entityPM.RequestDetails = entityPOCO.RequestDetails;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BringQueueForwardIndicatorS))
            {
					entityPM.BringQueueForwardIndicatorS = entityPOCO.BringQueueForwardIndicatorS;
            }

		}

		public void PMToOldPM(PhysicalCheckPM entityPM, PhysicalCheckPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationId))
            {
                oldEntityPM.DeclarationId = entityPM.DeclarationId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageSiteCode))
            {
                oldEntityPM.StorageSiteCode = entityPM.StorageSiteCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CheckSiteCode))
            {
                oldEntityPM.CheckSiteCode = entityPM.CheckSiteCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QueueTypeCode))
            {
                oldEntityPM.QueueTypeCode = entityPM.QueueTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OperationCode))
            {
                oldEntityPM.OperationCode = entityPM.OperationCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CheckId))
            {
                oldEntityPM.CheckId = entityPM.CheckId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerNubmer))
            {
                oldEntityPM.ContainerNubmer = entityPM.ContainerNubmer;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpenDate))
            {
                oldEntityPM.OpenDate = entityPM.OpenDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LimitDate))
            {
                oldEntityPM.LimitDate = entityPM.LimitDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoIdentifierKey1))
            {
                oldEntityPM.CargoIdentifierKey1 = entityPM.CargoIdentifierKey1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoIdentifierKey2))
            {
                oldEntityPM.CargoIdentifierKey2 = entityPM.CargoIdentifierKey2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoIdentifierKey3))
            {
                oldEntityPM.CargoIdentifierKey3 = entityPM.CargoIdentifierKey3;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RowNumber))
            {
                oldEntityPM.RowNumber = entityPM.RowNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CheckEssence))
            {
                oldEntityPM.CheckEssence = entityPM.CheckEssence;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosed))
            {
                oldEntityPM.IsClosed = entityPM.IsClosed;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusMessageCode))
            {
                oldEntityPM.StatusMessageCode = entityPM.StatusMessageCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoTypeCode))
            {
                oldEntityPM.CargoTypeCode = entityPM.CargoTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InitiatorTypeCode))
            {
                oldEntityPM.InitiatorTypeCode = entityPM.InitiatorTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterNumber))
            {
                oldEntityPM.ImporterNumber = entityPM.ImporterNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoIdentifierTypeCode))
            {
                oldEntityPM.CargoIdentifierTypeCode = entityPM.CargoIdentifierTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConcurrencyGUID))
            {
                oldEntityPM.ConcurrencyGUID = entityPM.ConcurrencyGUID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsComprehensiveCheck))
            {
                oldEntityPM.IsComprehensiveCheck = entityPM.IsComprehensiveCheck;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CheckTypeCode))
            {
                oldEntityPM.CheckTypeCode = entityPM.CheckTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
                oldEntityPM.CustomerId = entityPM.CustomerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NoEscortRequired))
            {
                oldEntityPM.NoEscortRequired = entityPM.NoEscortRequired;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VehicleChassisNumber))
            {
                oldEntityPM.VehicleChassisNumber = entityPM.VehicleChassisNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchResult))
            {
                oldEntityPM.SearchResult = entityPM.SearchResult;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SealNumber))
            {
                oldEntityPM.SealNumber = entityPM.SealNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CheckAuthorityAttenderTypeID))
            {
                oldEntityPM.CheckAuthorityAttenderTypeID = entityPM.CheckAuthorityAttenderTypeID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CheckAuthorityAttenderTypeName))
            {
                oldEntityPM.CheckAuthorityAttenderTypeName = entityPM.CheckAuthorityAttenderTypeName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CheckAnwserStatus))
            {
                oldEntityPM.CheckAnwserStatus = entityPM.CheckAnwserStatus;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestToAdvanceAQueue))
            {
                oldEntityPM.RequestToAdvanceAQueue = entityPM.RequestToAdvanceAQueue;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestDetails))
            {
                oldEntityPM.RequestDetails = entityPM.RequestDetails;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BringQueueForwardIndicatorS))
            {
                oldEntityPM.BringQueueForwardIndicatorS = entityPM.BringQueueForwardIndicatorS;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(PhysicalCheckPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.RequestToAdvanceAQueue)) //T4 find type == nText 
            {
                entityPM.RequestToAdvanceAQueue = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.RequestToAdvanceAQueue));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.RequestDetails)) //T4 find type == nText 
            {
                entityPM.RequestDetails = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.RequestDetails));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.BringQueueForwardIndicatorS)) //T4 find type == nText 
            {
                entityPM.BringQueueForwardIndicatorS = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.BringQueueForwardIndicatorS));
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
		
		private void BuildSearchFieldsGenerated(PhysicalCheckPM entityPM, PhysicalCheck entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 