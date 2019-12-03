
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
   
   public partial class DBMigrationLineDataMapping: IMapping<DBMigrationLinePM, DBMigrationLine>,IMappingEncodeBase64NVARCHARFields<DBMigrationLinePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         CounterKey, 
	         SqlScript, 
	         ApprovedRemarks,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         CounterKey, 
	         SqlScript, 
	         ApprovedRemarks,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DBMigrationLinePM entityPM, DBMigrationLine entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SqlScript))
            {
				entityPOCO.SqlScript = entityPM.SqlScript;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ApprovedRemarks))
            {
				entityPOCO.ApprovedRemarks = entityPM.ApprovedRemarks;
			}
			}

		public void POCOToPM(DBMigrationLinePM entityPM, DBMigrationLine entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CounterKey))
            {
					entityPM.CounterKey = entityPOCO.CounterKey;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SqlScript))
            {
					entityPM.SqlScript = entityPOCO.SqlScript;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ApprovedRemarks))
            {
					entityPM.ApprovedRemarks = entityPOCO.ApprovedRemarks;
            }

		}

		public void PMToOldPM(DBMigrationLinePM entityPM, DBMigrationLinePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SqlScript))
            {
                oldEntityPM.SqlScript = entityPM.SqlScript;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ApprovedRemarks))
            {
                oldEntityPM.ApprovedRemarks = entityPM.ApprovedRemarks;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DBMigrationLinePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.ApprovedRemarks)) //T4 find type == nText 
            {
                entityPM.ApprovedRemarks = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ApprovedRemarks));
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
	 