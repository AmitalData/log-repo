
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
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.BL.EntityPMs; 
using Logitude.DashboardModule.Data;

namespace Logitude.DashboardModule.BL.EntityDataMappings
{
   
   public partial class AnalyticsFactsFieldsMetaDataDataMapping: IMapping<AnalyticsFactsFieldsMetaDataPM, AnalyticsFactsFieldsMetaData>,IMappingEncodeBase64NVARCHARFields<AnalyticsFactsFieldsMetaDataPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         AnalyticsFactsMetaDataId, 
	         DataTypeCode, 
	         CanMeasure, 
	         CanGroup, 
	         FieldCode, 
	         DisplayName, 
	         DisplayNamePlural, 
	         JoinedTableName, 
	         JoinedTableKey, 
	         JoinedTableDisplayField, 
	         SearchFields, 
	         JoinedTableDBName, 
	         HasUnit, 
	         Unit, 
	         CommonFilterCode, 
	         CanSecondaryGroup, 
	         AllowTenantZeroFilter,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         AnalyticsFactsMetaDataId, 
	         DataTypeCode, 
	         CanMeasure, 
	         CanGroup, 
	         FieldCode, 
	         DisplayName, 
	         DisplayNamePlural, 
	         JoinedTableName, 
	         JoinedTableKey, 
	         JoinedTableDisplayField, 
	         SearchFields, 
	         JoinedTableDBName, 
	         ObjectTableName, 
	         HasUnit, 
	         Unit, 
	         CommonFilterCode, 
	         CanSecondaryGroup, 
	         AllowTenantZeroFilter,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(AnalyticsFactsFieldsMetaDataPM entityPM, AnalyticsFactsFieldsMetaData entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AnalyticsFactsMetaDataId))
            {
				entityPOCO.AnalyticsFactsMetaDataId = entityPM.AnalyticsFactsMetaDataId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DataTypeCode))
            {
				entityPOCO.DataTypeCode = entityPM.DataTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CanMeasure))
            {
				entityPOCO.CanMeasure = entityPM.CanMeasure;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CanGroup))
            {
				entityPOCO.CanGroup = entityPM.CanGroup;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldCode))
            {
				entityPOCO.FieldCode = entityPM.FieldCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DisplayName))
            {
				entityPOCO.DisplayName = entityPM.DisplayName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DisplayNamePlural))
            {
				entityPOCO.DisplayNamePlural = entityPM.DisplayNamePlural;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.JoinedTableName))
            {
				entityPOCO.JoinedTableName = entityPM.JoinedTableName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.JoinedTableKey))
            {
				entityPOCO.JoinedTableKey = entityPM.JoinedTableKey;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.JoinedTableDisplayField))
            {
				entityPOCO.JoinedTableDisplayField = entityPM.JoinedTableDisplayField;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.JoinedTableDBName))
            {
				entityPOCO.JoinedTableDBName = entityPM.JoinedTableDBName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HasUnit))
            {
				entityPOCO.HasUnit = entityPM.HasUnit;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Unit))
            {
				entityPOCO.Unit = entityPM.Unit;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommonFilterCode))
            {
				entityPOCO.CommonFilterCode = entityPM.CommonFilterCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CanSecondaryGroup))
            {
				entityPOCO.CanSecondaryGroup = entityPM.CanSecondaryGroup;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AllowTenantZeroFilter))
            {
				entityPOCO.AllowTenantZeroFilter = entityPM.AllowTenantZeroFilter;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(AnalyticsFactsFieldsMetaDataPM entityPM, AnalyticsFactsFieldsMetaData entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AnalyticsFactsMetaDataId))
            {
					entityPM.AnalyticsFactsMetaDataId = entityPOCO.AnalyticsFactsMetaDataId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DataTypeCode))
            {
					entityPM.DataTypeCode = entityPOCO.DataTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CanMeasure))
            {
					entityPM.CanMeasure = entityPOCO.CanMeasure;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CanGroup))
            {
					entityPM.CanGroup = entityPOCO.CanGroup;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldCode))
            {
					entityPM.FieldCode = entityPOCO.FieldCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DisplayName))
            {
					entityPM.DisplayName = entityPOCO.DisplayName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DisplayNamePlural))
            {
					entityPM.DisplayNamePlural = entityPOCO.DisplayNamePlural;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.JoinedTableName))
            {
					entityPM.JoinedTableName = entityPOCO.JoinedTableName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.JoinedTableKey))
            {
					entityPM.JoinedTableKey = entityPOCO.JoinedTableKey;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.JoinedTableDisplayField))
            {
					entityPM.JoinedTableDisplayField = entityPOCO.JoinedTableDisplayField;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.JoinedTableDBName))
            {
					entityPM.JoinedTableDBName = entityPOCO.JoinedTableDBName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.HasUnit))
            {
					entityPM.HasUnit = entityPOCO.HasUnit;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Unit))
            {
					entityPM.Unit = entityPOCO.Unit;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CommonFilterCode))
            {
					entityPM.CommonFilterCode = entityPOCO.CommonFilterCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CanSecondaryGroup))
            {
					entityPM.CanSecondaryGroup = entityPOCO.CanSecondaryGroup;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AllowTenantZeroFilter))
            {
					entityPM.AllowTenantZeroFilter = entityPOCO.AllowTenantZeroFilter;
            }

		}

		public void PMToOldPM(AnalyticsFactsFieldsMetaDataPM entityPM, AnalyticsFactsFieldsMetaDataPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AnalyticsFactsMetaDataId))
            {
                oldEntityPM.AnalyticsFactsMetaDataId = entityPM.AnalyticsFactsMetaDataId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DataTypeCode))
            {
                oldEntityPM.DataTypeCode = entityPM.DataTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CanMeasure))
            {
                oldEntityPM.CanMeasure = entityPM.CanMeasure;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CanGroup))
            {
                oldEntityPM.CanGroup = entityPM.CanGroup;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldCode))
            {
                oldEntityPM.FieldCode = entityPM.FieldCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DisplayName))
            {
                oldEntityPM.DisplayName = entityPM.DisplayName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DisplayNamePlural))
            {
                oldEntityPM.DisplayNamePlural = entityPM.DisplayNamePlural;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.JoinedTableName))
            {
                oldEntityPM.JoinedTableName = entityPM.JoinedTableName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.JoinedTableKey))
            {
                oldEntityPM.JoinedTableKey = entityPM.JoinedTableKey;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.JoinedTableDisplayField))
            {
                oldEntityPM.JoinedTableDisplayField = entityPM.JoinedTableDisplayField;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.JoinedTableDBName))
            {
                oldEntityPM.JoinedTableDBName = entityPM.JoinedTableDBName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HasUnit))
            {
                oldEntityPM.HasUnit = entityPM.HasUnit;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Unit))
            {
                oldEntityPM.Unit = entityPM.Unit;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommonFilterCode))
            {
                oldEntityPM.CommonFilterCode = entityPM.CommonFilterCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CanSecondaryGroup))
            {
                oldEntityPM.CanSecondaryGroup = entityPM.CanSecondaryGroup;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AllowTenantZeroFilter))
            {
                oldEntityPM.AllowTenantZeroFilter = entityPM.AllowTenantZeroFilter;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(AnalyticsFactsFieldsMetaDataPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.DisplayName)) //T4 find type == nText 
            {
                entityPM.DisplayName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.DisplayName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.DisplayNamePlural)) //T4 find type == nText 
            {
                entityPM.DisplayNamePlural = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.DisplayNamePlural));
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
		
		private void BuildSearchFieldsGenerated(AnalyticsFactsFieldsMetaDataPM entityPM, AnalyticsFactsFieldsMetaData entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 