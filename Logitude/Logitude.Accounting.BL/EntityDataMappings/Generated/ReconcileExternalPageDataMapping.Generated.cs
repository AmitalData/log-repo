
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
   
   public partial class ReconcileExternalPageDataMapping: IMapping<ReconcileExternalPagePM, ReconcileExternalPage>,IMappingEncodeBase64NVARCHARFields<ReconcileExternalPagePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         GLAccountId, 
	         PageNo, 
	         CreatedByUserId, 
	         SearchFields, 
	         FromDate, 
	         ToDate, 
	         StartBalance, 
	         CloseBalance, 
	         CreateDate, 
	         ApprovedByUserId, 
	         StatusCode, 
	         EntryTypeCode, 
	         ObjectTableId, 
	         EntityId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         GLAccountId, 
	         PageNo, 
	         CreatedByUserId, 
	         SearchFields, 
	         CreatedByUserName, 
	         FromDate, 
	         ToDate, 
	         StartBalance, 
	         CloseBalance, 
	         CreateDate, 
	         ApprovedByUserId, 
	         StatusCode, 
	         StatusName, 
	         EntryTypeCode, 
	         EntryTypeEnglishName, 
	         EntryTypeLocalName, 
	         ObjectTableId, 
	         EntityId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ReconcileExternalPagePM entityPM, ReconcileExternalPage entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GLAccountId))
            {
				entityPOCO.GLAccountId = entityPM.GLAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PageNo))
            {
				entityPOCO.PageNo = entityPM.PageNo;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromDate))
            {
				entityPOCO.FromDate = entityPM.FromDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToDate))
            {
				entityPOCO.ToDate = entityPM.ToDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartBalance))
            {
				entityPOCO.StartBalance = entityPM.StartBalance;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CloseBalance))
            {
				entityPOCO.CloseBalance = entityPM.CloseBalance;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ApprovedByUserId))
            {
				entityPOCO.ApprovedByUserId = entityPM.ApprovedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
				entityPOCO.StatusCode = entityPM.StatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntryTypeCode))
            {
				entityPOCO.EntryTypeCode = entityPM.EntryTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId))
            {
				entityPOCO.ObjectTableId = entityPM.ObjectTableId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId))
            {
				entityPOCO.EntityId = entityPM.EntityId;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(ReconcileExternalPagePM entityPM, ReconcileExternalPage entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GLAccountId))
            {
					entityPM.GLAccountId = entityPOCO.GLAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PageNo))
            {
					entityPM.PageNo = entityPOCO.PageNo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromDate))
            {
					entityPM.FromDate = entityPOCO.FromDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToDate))
            {
					entityPM.ToDate = entityPOCO.ToDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartBalance))
            {
					entityPM.StartBalance = entityPOCO.StartBalance;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CloseBalance))
            {
					entityPM.CloseBalance = entityPOCO.CloseBalance;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ApprovedByUserId))
            {
					entityPM.ApprovedByUserId = entityPOCO.ApprovedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatusCode))
            {
					entityPM.StatusCode = entityPOCO.StatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntryTypeCode))
            {
					entityPM.EntryTypeCode = entityPOCO.EntryTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ObjectTableId))
            {
					entityPM.ObjectTableId = entityPOCO.ObjectTableId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityId))
            {
					entityPM.EntityId = entityPOCO.EntityId;
            }

		}

		public void PMToOldPM(ReconcileExternalPagePM entityPM, ReconcileExternalPagePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GLAccountId))
            {
                oldEntityPM.GLAccountId = entityPM.GLAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PageNo))
            {
                oldEntityPM.PageNo = entityPM.PageNo;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromDate))
            {
                oldEntityPM.FromDate = entityPM.FromDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToDate))
            {
                oldEntityPM.ToDate = entityPM.ToDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartBalance))
            {
                oldEntityPM.StartBalance = entityPM.StartBalance;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CloseBalance))
            {
                oldEntityPM.CloseBalance = entityPM.CloseBalance;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ApprovedByUserId))
            {
                oldEntityPM.ApprovedByUserId = entityPM.ApprovedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
                oldEntityPM.StatusCode = entityPM.StatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntryTypeCode))
            {
                oldEntityPM.EntryTypeCode = entityPM.EntryTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId))
            {
                oldEntityPM.ObjectTableId = entityPM.ObjectTableId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId))
            {
                oldEntityPM.EntityId = entityPM.EntityId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ReconcileExternalPagePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

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
		
		private void BuildSearchFieldsGenerated(ReconcileExternalPagePM entityPM, ReconcileExternalPage entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 