
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
   
   public partial class BookingAnswerDataMapping: IMapping<BookingAnswerPM, BookingAnswer>,IMappingEncodeBase64NVARCHARFields<BookingAnswerPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         BookingId, 
	         StatusCode, 
	         ETD, 
	         CreateDate, 
	         Origin, 
	         Destination, 
	         CommunicationLogId, 
	         FlightNumber, 
	         BookingSpaceAllocationCode, 
	         CarrierId, 
	         OtherServicesInformation, 
	         DescriptionOfGoods, 
	         NumberOfPieces, 
	         Weight, 
	         WeightUnitCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         BookingId, 
	         StatusCode, 
	         ETD, 
	         CreateDate, 
	         Master, 
	         Origin, 
	         Destination, 
	         CommunicationLogId, 
	         FlightNumber, 
	         BookingSpaceAllocationCode, 
	         CarrierId, 
	         OtherServicesInformation, 
	         DescriptionOfGoods, 
	         NumberOfPieces, 
	         Weight, 
	         WeightUnitCode, 
	         CarrierName, 
	         OriginCountryName, 
	         OriginCountryCode, 
	         DestinationCountryCode, 
	         DestinationCountryName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(BookingAnswerPM entityPM, BookingAnswer entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingId))
            {
				entityPOCO.BookingId = entityPM.BookingId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
				entityPOCO.StatusCode = entityPM.StatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ETD))
            {
				entityPOCO.ETD = entityPM.ETD;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Origin))
            {
				entityPOCO.Origin = entityPM.Origin;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Destination))
            {
				entityPOCO.Destination = entityPM.Destination;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommunicationLogId))
            {
				entityPOCO.CommunicationLogId = entityPM.CommunicationLogId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FlightNumber))
            {
				entityPOCO.FlightNumber = entityPM.FlightNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingSpaceAllocationCode))
            {
				entityPOCO.BookingSpaceAllocationCode = entityPM.BookingSpaceAllocationCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CarrierId))
            {
				entityPOCO.CarrierId = entityPM.CarrierId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OtherServicesInformation))
            {
				entityPOCO.OtherServicesInformation = entityPM.OtherServicesInformation;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DescriptionOfGoods))
            {
				entityPOCO.DescriptionOfGoods = entityPM.DescriptionOfGoods;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfPieces))
            {
				entityPOCO.NumberOfPieces = entityPM.NumberOfPieces;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Weight))
            {
				entityPOCO.Weight = entityPM.Weight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WeightUnitCode))
            {
				entityPOCO.WeightUnitCode = entityPM.WeightUnitCode;
			}
			}

		public void POCOToPM(BookingAnswerPM entityPM, BookingAnswer entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatusCode))
            {
					entityPM.StatusCode = entityPOCO.StatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ETD))
            {
					entityPM.ETD = entityPOCO.ETD;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Origin))
            {
					entityPM.Origin = entityPOCO.Origin;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Destination))
            {
					entityPM.Destination = entityPOCO.Destination;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CommunicationLogId))
            {
					entityPM.CommunicationLogId = entityPOCO.CommunicationLogId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FlightNumber))
            {
					entityPM.FlightNumber = entityPOCO.FlightNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BookingSpaceAllocationCode))
            {
					entityPM.BookingSpaceAllocationCode = entityPOCO.BookingSpaceAllocationCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CarrierId))
            {
					entityPM.CarrierId = entityPOCO.CarrierId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OtherServicesInformation))
            {
					entityPM.OtherServicesInformation = entityPOCO.OtherServicesInformation;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DescriptionOfGoods))
            {
					entityPM.DescriptionOfGoods = entityPOCO.DescriptionOfGoods;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NumberOfPieces))
            {
					entityPM.NumberOfPieces = entityPOCO.NumberOfPieces;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Weight))
            {
					entityPM.Weight = entityPOCO.Weight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WeightUnitCode))
            {
					entityPM.WeightUnitCode = entityPOCO.WeightUnitCode;
            }

		}

		public void PMToOldPM(BookingAnswerPM entityPM, BookingAnswerPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
                oldEntityPM.StatusCode = entityPM.StatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ETD))
            {
                oldEntityPM.ETD = entityPM.ETD;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Origin))
            {
                oldEntityPM.Origin = entityPM.Origin;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Destination))
            {
                oldEntityPM.Destination = entityPM.Destination;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommunicationLogId))
            {
                oldEntityPM.CommunicationLogId = entityPM.CommunicationLogId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FlightNumber))
            {
                oldEntityPM.FlightNumber = entityPM.FlightNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingSpaceAllocationCode))
            {
                oldEntityPM.BookingSpaceAllocationCode = entityPM.BookingSpaceAllocationCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CarrierId))
            {
                oldEntityPM.CarrierId = entityPM.CarrierId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OtherServicesInformation))
            {
                oldEntityPM.OtherServicesInformation = entityPM.OtherServicesInformation;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DescriptionOfGoods))
            {
                oldEntityPM.DescriptionOfGoods = entityPM.DescriptionOfGoods;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfPieces))
            {
                oldEntityPM.NumberOfPieces = entityPM.NumberOfPieces;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Weight))
            {
                oldEntityPM.Weight = entityPM.Weight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WeightUnitCode))
            {
                oldEntityPM.WeightUnitCode = entityPM.WeightUnitCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(BookingAnswerPM entityPM)
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
	 