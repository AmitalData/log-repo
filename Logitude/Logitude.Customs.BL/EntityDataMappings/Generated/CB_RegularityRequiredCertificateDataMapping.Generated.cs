
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
   
   public partial class CB_RegularityRequiredCertificateDataMapping: IMapping<CB_RegularityRequiredCertificatePM, CB_RegularityRequiredCertificate>,IMappingEncodeBase64NVARCHARFields<CB_RegularityRequiredCertificatePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ID, 
	         RegularityInceptionID, 
	         ConfirmationTypeID, 
	         Number, 
	         TextualCondition, 
	         TrNumber, 
	         AuthorityID, 
	         CB_ID, 
	         IsVoluntaryOrImporterOfTrust,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ID, 
	         RegularityInceptionID, 
	         ConfirmationTypeID, 
	         Number, 
	         TextualCondition, 
	         TrNumber, 
	         AuthorityID, 
	         CB_ID, 
	         IsVoluntaryOrImporterOfTrust,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CB_RegularityRequiredCertificatePM entityPM, CB_RegularityRequiredCertificate entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ID))
            {
				entityPOCO.ID = entityPM.ID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RegularityInceptionID))
            {
				entityPOCO.RegularityInceptionID = entityPM.RegularityInceptionID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConfirmationTypeID))
            {
				entityPOCO.ConfirmationTypeID = entityPM.ConfirmationTypeID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Number))
            {
				entityPOCO.Number = entityPM.Number;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TextualCondition))
            {
				entityPOCO.TextualCondition = entityPM.TextualCondition;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TrNumber))
            {
				entityPOCO.TrNumber = entityPM.TrNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AuthorityID))
            {
				entityPOCO.AuthorityID = entityPM.AuthorityID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsVoluntaryOrImporterOfTrust))
            {
				entityPOCO.IsVoluntaryOrImporterOfTrust = entityPM.IsVoluntaryOrImporterOfTrust;
			}
			}

		public void POCOToPM(CB_RegularityRequiredCertificatePM entityPM, CB_RegularityRequiredCertificate entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ID))
            {
					entityPM.ID = entityPOCO.ID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RegularityInceptionID))
            {
					entityPM.RegularityInceptionID = entityPOCO.RegularityInceptionID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConfirmationTypeID))
            {
					entityPM.ConfirmationTypeID = entityPOCO.ConfirmationTypeID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Number))
            {
					entityPM.Number = entityPOCO.Number;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TextualCondition))
            {
					entityPM.TextualCondition = entityPOCO.TextualCondition;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TrNumber))
            {
					entityPM.TrNumber = entityPOCO.TrNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AuthorityID))
            {
					entityPM.AuthorityID = entityPOCO.AuthorityID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CB_ID))
            {
					entityPM.CB_ID = entityPOCO.CB_ID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsVoluntaryOrImporterOfTrust))
            {
					entityPM.IsVoluntaryOrImporterOfTrust = entityPOCO.IsVoluntaryOrImporterOfTrust;
            }

		}

		public void PMToOldPM(CB_RegularityRequiredCertificatePM entityPM, CB_RegularityRequiredCertificatePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ID))
            {
                oldEntityPM.ID = entityPM.ID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RegularityInceptionID))
            {
                oldEntityPM.RegularityInceptionID = entityPM.RegularityInceptionID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConfirmationTypeID))
            {
                oldEntityPM.ConfirmationTypeID = entityPM.ConfirmationTypeID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Number))
            {
                oldEntityPM.Number = entityPM.Number;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TextualCondition))
            {
                oldEntityPM.TextualCondition = entityPM.TextualCondition;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TrNumber))
            {
                oldEntityPM.TrNumber = entityPM.TrNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AuthorityID))
            {
                oldEntityPM.AuthorityID = entityPM.AuthorityID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsVoluntaryOrImporterOfTrust))
            {
                oldEntityPM.IsVoluntaryOrImporterOfTrust = entityPM.IsVoluntaryOrImporterOfTrust;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CB_RegularityRequiredCertificatePM entityPM)
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
	 