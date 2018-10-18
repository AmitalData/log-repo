
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
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.BL.EntityPMs; 
using Logitude.BookingLib.Data;

namespace Logitude.BookingLib.BL.EntityDataMappings
{
   
   public partial class FlightsSchedulesRequestDataMapping: IMapping<FlightsSchedulesRequestPM, FlightsSchedulesRequest>,IMappingEncodeBase64NVARCHARFields<FlightsSchedulesRequestPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         FromPortId, 
	         ToPortId, 
	         AirlineId, 
	         ShipmentId, 
	         BookingId, 
	         ETD, 
	         ETA, 
	         Volume, 
	         GrossWeight, 
	         VolumeUnitCode, 
	         GrossWeightUnitCode, 
	         CreateDate, 
	         ResponseDate, 
	         CreatedByUserId, 
	         StatusCode, 
	         AnswerOSI, 
	         AnswerReasonForNoReply, 
	         RequestDetails,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         FromPortId, 
	         ToPortId, 
	         AirlineId, 
	         ShipmentId, 
	         BookingId, 
	         ETD, 
	         ETA, 
	         Volume, 
	         GrossWeight, 
	         VolumeUnitCode, 
	         GrossWeightUnitCode, 
	         CreateDate, 
	         ResponseDate, 
	         CreatedByUserId, 
	         StatusCode, 
	         AnswerOSI, 
	         AnswerReasonForNoReply, 
	         RequestDetails, 
	         AirlineName, 
	         AirlineCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(FlightsSchedulesRequestPM entityPM, FlightsSchedulesRequest entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromPortId))
            {
				entityPOCO.FromPortId = entityPM.FromPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToPortId))
            {
				entityPOCO.ToPortId = entityPM.ToPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AirlineId))
            {
				entityPOCO.AirlineId = entityPM.AirlineId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentId))
            {
				entityPOCO.ShipmentId = entityPM.ShipmentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingId))
            {
				entityPOCO.BookingId = entityPM.BookingId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ETD))
            {
				entityPOCO.ETD = entityPM.ETD;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ETA))
            {
				entityPOCO.ETA = entityPM.ETA;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Volume))
            {
				entityPOCO.Volume = entityPM.Volume;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeight))
            {
				entityPOCO.GrossWeight = entityPM.GrossWeight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumeUnitCode))
            {
				entityPOCO.VolumeUnitCode = entityPM.VolumeUnitCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeightUnitCode))
            {
				entityPOCO.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ResponseDate))
            {
				entityPOCO.ResponseDate = entityPM.ResponseDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
				entityPOCO.StatusCode = entityPM.StatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AnswerOSI))
            {
				entityPOCO.AnswerOSI = entityPM.AnswerOSI;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AnswerReasonForNoReply))
            {
				entityPOCO.AnswerReasonForNoReply = entityPM.AnswerReasonForNoReply;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestDetails))
            {
				entityPOCO.RequestDetails = entityPM.RequestDetails;
			}
			}

		public void POCOToPM(FlightsSchedulesRequestPM entityPM, FlightsSchedulesRequest entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromPortId))
            {
					entityPM.FromPortId = entityPOCO.FromPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToPortId))
            {
					entityPM.ToPortId = entityPOCO.ToPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AirlineId))
            {
					entityPM.AirlineId = entityPOCO.AirlineId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentId))
            {
					entityPM.ShipmentId = entityPOCO.ShipmentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BookingId))
            {
					entityPM.BookingId = entityPOCO.BookingId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ETD))
            {
					entityPM.ETD = entityPOCO.ETD;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ETA))
            {
					entityPM.ETA = entityPOCO.ETA;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Volume))
            {
					entityPM.Volume = entityPOCO.Volume;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GrossWeight))
            {
					entityPM.GrossWeight = entityPOCO.GrossWeight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VolumeUnitCode))
            {
					entityPM.VolumeUnitCode = entityPOCO.VolumeUnitCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GrossWeightUnitCode))
            {
					entityPM.GrossWeightUnitCode = entityPOCO.GrossWeightUnitCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ResponseDate))
            {
					entityPM.ResponseDate = entityPOCO.ResponseDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatusCode))
            {
					entityPM.StatusCode = entityPOCO.StatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AnswerOSI))
            {
					entityPM.AnswerOSI = entityPOCO.AnswerOSI;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AnswerReasonForNoReply))
            {
					entityPM.AnswerReasonForNoReply = entityPOCO.AnswerReasonForNoReply;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequestDetails))
            {
					entityPM.RequestDetails = entityPOCO.RequestDetails;
            }

		}

		public void PMToOldPM(FlightsSchedulesRequestPM entityPM, FlightsSchedulesRequestPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromPortId))
            {
                oldEntityPM.FromPortId = entityPM.FromPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToPortId))
            {
                oldEntityPM.ToPortId = entityPM.ToPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AirlineId))
            {
                oldEntityPM.AirlineId = entityPM.AirlineId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentId))
            {
                oldEntityPM.ShipmentId = entityPM.ShipmentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingId))
            {
                oldEntityPM.BookingId = entityPM.BookingId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ETD))
            {
                oldEntityPM.ETD = entityPM.ETD;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ETA))
            {
                oldEntityPM.ETA = entityPM.ETA;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Volume))
            {
                oldEntityPM.Volume = entityPM.Volume;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeight))
            {
                oldEntityPM.GrossWeight = entityPM.GrossWeight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumeUnitCode))
            {
                oldEntityPM.VolumeUnitCode = entityPM.VolumeUnitCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeightUnitCode))
            {
                oldEntityPM.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ResponseDate))
            {
                oldEntityPM.ResponseDate = entityPM.ResponseDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
                oldEntityPM.StatusCode = entityPM.StatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AnswerOSI))
            {
                oldEntityPM.AnswerOSI = entityPM.AnswerOSI;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AnswerReasonForNoReply))
            {
                oldEntityPM.AnswerReasonForNoReply = entityPM.AnswerReasonForNoReply;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestDetails))
            {
                oldEntityPM.RequestDetails = entityPM.RequestDetails;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(FlightsSchedulesRequestPM entityPM)
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
	 