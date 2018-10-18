
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
   
   public partial class CustomsClosedTableDataMapping: IMapping<CustomsClosedTablePM, CustomsClosedTable>,IMappingEncodeBase64NVARCHARFields<CustomsClosedTablePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         CustomsName, 
	         CustomsLocalName, 
	         DbName, 
	         LastUpdateDate, 
	         StatusCode, 
	         SearchFields, 
	         ObjectTableId, 
	         Existed, 
	         RetreiveDateTime,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         CustomsName, 
	         CustomsLocalName, 
	         DbName, 
	         LastUpdateDate, 
	         StatusCode, 
	         StatusName, 
	         SearchFields, 
	         ObjectTableName, 
	         ObjectTableId, 
	         Existed, 
	         RetreiveDateTime,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CustomsClosedTablePM entityPM, CustomsClosedTable entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsName))
            {
				entityPOCO.CustomsName = entityPM.CustomsName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsLocalName))
            {
				entityPOCO.CustomsLocalName = entityPM.CustomsLocalName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DbName))
            {
				entityPOCO.DbName = entityPM.DbName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastUpdateDate))
            {
				entityPOCO.LastUpdateDate = entityPM.LastUpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
				entityPOCO.StatusCode = entityPM.StatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId))
            {
				entityPOCO.ObjectTableId = entityPM.ObjectTableId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Existed))
            {
				entityPOCO.Existed = entityPM.Existed;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RetreiveDateTime))
            {
				entityPOCO.RetreiveDateTime = entityPM.RetreiveDateTime;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(CustomsClosedTablePM entityPM, CustomsClosedTable entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsName))
            {
					entityPM.CustomsName = entityPOCO.CustomsName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsLocalName))
            {
					entityPM.CustomsLocalName = entityPOCO.CustomsLocalName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DbName))
            {
					entityPM.DbName = entityPOCO.DbName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastUpdateDate))
            {
					entityPM.LastUpdateDate = entityPOCO.LastUpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatusCode))
            {
					entityPM.StatusCode = entityPOCO.StatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ObjectTableId))
            {
					entityPM.ObjectTableId = entityPOCO.ObjectTableId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Existed))
            {
					entityPM.Existed = entityPOCO.Existed;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RetreiveDateTime))
            {
					entityPM.RetreiveDateTime = entityPOCO.RetreiveDateTime;
            }

		}

		public void PMToOldPM(CustomsClosedTablePM entityPM, CustomsClosedTablePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsName))
            {
                oldEntityPM.CustomsName = entityPM.CustomsName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsLocalName))
            {
                oldEntityPM.CustomsLocalName = entityPM.CustomsLocalName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DbName))
            {
                oldEntityPM.DbName = entityPM.DbName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastUpdateDate))
            {
                oldEntityPM.LastUpdateDate = entityPM.LastUpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
                oldEntityPM.StatusCode = entityPM.StatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId))
            {
                oldEntityPM.ObjectTableId = entityPM.ObjectTableId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Existed))
            {
                oldEntityPM.Existed = entityPM.Existed;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RetreiveDateTime))
            {
                oldEntityPM.RetreiveDateTime = entityPM.RetreiveDateTime;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CustomsClosedTablePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.CustomsLocalName)) //T4 find type == nText 
            {
                entityPM.CustomsLocalName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CustomsLocalName));
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
		
		private void BuildSearchFieldsGenerated(CustomsClosedTablePM entityPM, CustomsClosedTable entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 