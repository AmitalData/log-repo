
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
   
   public partial class FlightsSchedulesResponseDataMapping: IMapping<FlightsSchedulesResponsePM, FlightsSchedulesResponse>,IMappingEncodeBase64NVARCHARFields<FlightsSchedulesResponsePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         FromPortId, 
	         ToPortId, 
	         AirlineId, 
	         RequestId, 
	         ETD, 
	         ETA, 
	         FlightNumber, 
	         AirplaneType, 
	         NumberOfStops, 
	         ResultNumber, 
	         LineNumber, 
	         FromPortCode, 
	         FromPortName, 
	         ToPortCode, 
	         ToPortName, 
	         MissingPort,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         FromPortId, 
	         ToPortId, 
	         AirlineId, 
	         RequestId, 
	         ETD, 
	         ETA, 
	         FlightNumber, 
	         AirplaneType, 
	         NumberOfStops, 
	         ResultNumber, 
	         LineNumber, 
	         FromPortCode, 
	         FromPortName, 
	         ToPortCode, 
	         ToPortName, 
	         MissingPort, 
	         AirlineCode, 
	         AirlineName, 
	         FromPortCountryCode, 
	         FromPortCountryName, 
	         ToPortCountryCode, 
	         ToPortCountryName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(FlightsSchedulesResponsePM entityPM, FlightsSchedulesResponse entityPOCO)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestId))
            {
				entityPOCO.RequestId = entityPM.RequestId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ETD))
            {
				entityPOCO.ETD = entityPM.ETD;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ETA))
            {
				entityPOCO.ETA = entityPM.ETA;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FlightNumber))
            {
				entityPOCO.FlightNumber = entityPM.FlightNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AirplaneType))
            {
				entityPOCO.AirplaneType = entityPM.AirplaneType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfStops))
            {
				entityPOCO.NumberOfStops = entityPM.NumberOfStops;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ResultNumber))
            {
				entityPOCO.ResultNumber = entityPM.ResultNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineNumber))
            {
				entityPOCO.LineNumber = entityPM.LineNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromPortCode))
            {
				entityPOCO.FromPortCode = entityPM.FromPortCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromPortName))
            {
				entityPOCO.FromPortName = entityPM.FromPortName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToPortCode))
            {
				entityPOCO.ToPortCode = entityPM.ToPortCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToPortName))
            {
				entityPOCO.ToPortName = entityPM.ToPortName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MissingPort))
            {
				entityPOCO.MissingPort = entityPM.MissingPort;
			}
			}

		public void POCOToPM(FlightsSchedulesResponsePM entityPM, FlightsSchedulesResponse entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequestId))
            {
					entityPM.RequestId = entityPOCO.RequestId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ETD))
            {
					entityPM.ETD = entityPOCO.ETD;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ETA))
            {
					entityPM.ETA = entityPOCO.ETA;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FlightNumber))
            {
					entityPM.FlightNumber = entityPOCO.FlightNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AirplaneType))
            {
					entityPM.AirplaneType = entityPOCO.AirplaneType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NumberOfStops))
            {
					entityPM.NumberOfStops = entityPOCO.NumberOfStops;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ResultNumber))
            {
					entityPM.ResultNumber = entityPOCO.ResultNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
					entityPM.LineNumber = entityPOCO.LineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromPortCode))
            {
					entityPM.FromPortCode = entityPOCO.FromPortCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromPortName))
            {
					entityPM.FromPortName = entityPOCO.FromPortName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToPortCode))
            {
					entityPM.ToPortCode = entityPOCO.ToPortCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToPortName))
            {
					entityPM.ToPortName = entityPOCO.ToPortName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MissingPort))
            {
					entityPM.MissingPort = entityPOCO.MissingPort;
            }

		}

		public void PMToOldPM(FlightsSchedulesResponsePM entityPM, FlightsSchedulesResponsePM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestId))
            {
                oldEntityPM.RequestId = entityPM.RequestId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ETD))
            {
                oldEntityPM.ETD = entityPM.ETD;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ETA))
            {
                oldEntityPM.ETA = entityPM.ETA;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FlightNumber))
            {
                oldEntityPM.FlightNumber = entityPM.FlightNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AirplaneType))
            {
                oldEntityPM.AirplaneType = entityPM.AirplaneType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfStops))
            {
                oldEntityPM.NumberOfStops = entityPM.NumberOfStops;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ResultNumber))
            {
                oldEntityPM.ResultNumber = entityPM.ResultNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineNumber))
            {
                oldEntityPM.LineNumber = entityPM.LineNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromPortCode))
            {
                oldEntityPM.FromPortCode = entityPM.FromPortCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromPortName))
            {
                oldEntityPM.FromPortName = entityPM.FromPortName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToPortCode))
            {
                oldEntityPM.ToPortCode = entityPM.ToPortCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToPortName))
            {
                oldEntityPM.ToPortName = entityPM.ToPortName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MissingPort))
            {
                oldEntityPM.MissingPort = entityPM.MissingPort;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(FlightsSchedulesResponsePM entityPM)
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
	 