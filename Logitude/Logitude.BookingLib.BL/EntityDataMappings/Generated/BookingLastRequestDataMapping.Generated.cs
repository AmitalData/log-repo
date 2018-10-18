
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
   
   public partial class BookingLastRequestDataMapping: IMapping<BookingLastRequestPM, BookingLastRequest>,IMappingEncodeBase64NVARCHARFields<BookingLastRequestPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         BookingId, 
	         CreateDate, 
	         ETD, 
	         FlightNumber, 
	         Origin, 
	         Destination, 
	         CarrierId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         BookingId, 
	         CreateDate, 
	         ETD, 
	         FlightNumber, 
	         Origin, 
	         Destination, 
	         CarrierId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(BookingLastRequestPM entityPM, BookingLastRequest entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingId))
            {
				entityPOCO.BookingId = entityPM.BookingId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ETD))
            {
				entityPOCO.ETD = entityPM.ETD;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FlightNumber))
            {
				entityPOCO.FlightNumber = entityPM.FlightNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Origin))
            {
				entityPOCO.Origin = entityPM.Origin;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Destination))
            {
				entityPOCO.Destination = entityPM.Destination;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CarrierId))
            {
				entityPOCO.CarrierId = entityPM.CarrierId;
			}
			}

		public void POCOToPM(BookingLastRequestPM entityPM, BookingLastRequest entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BookingId))
            {
					entityPM.BookingId = entityPOCO.BookingId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ETD))
            {
					entityPM.ETD = entityPOCO.ETD;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FlightNumber))
            {
					entityPM.FlightNumber = entityPOCO.FlightNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Origin))
            {
					entityPM.Origin = entityPOCO.Origin;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Destination))
            {
					entityPM.Destination = entityPOCO.Destination;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CarrierId))
            {
					entityPM.CarrierId = entityPOCO.CarrierId;
            }

		}

		public void PMToOldPM(BookingLastRequestPM entityPM, BookingLastRequestPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingId))
            {
                oldEntityPM.BookingId = entityPM.BookingId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ETD))
            {
                oldEntityPM.ETD = entityPM.ETD;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FlightNumber))
            {
                oldEntityPM.FlightNumber = entityPM.FlightNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Origin))
            {
                oldEntityPM.Origin = entityPM.Origin;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Destination))
            {
                oldEntityPM.Destination = entityPM.Destination;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CarrierId))
            {
                oldEntityPM.CarrierId = entityPM.CarrierId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(BookingLastRequestPM entityPM)
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
	 