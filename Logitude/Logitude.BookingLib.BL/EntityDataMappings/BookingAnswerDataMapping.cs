
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.BL.EntityPMs; 
using Logitude.BookingLib.Data;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BookingLib.Data.Repositories;

namespace Logitude.BookingLib.BL.EntityDataMappings
{
   
   public partial class BookingAnswerDataMapping: IMapping<BookingAnswerPM, BookingAnswer>
   {
       public void CustomPMToPOCO(BookingAnswerPM entityPM, BookingAnswer entityPOCO)
       {
           this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);

           if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert && entityPOCO != null)
           {
               entityPOCO.Id = entityPM.Id;
           }
       }

       public void CustomPOCOToPM(BookingAnswerPM entityPM, BookingAnswer entityPOCO)
       {
           PortRepository portRep = new PortRepository(entityPOCO.Tenant);

           Card cardObject = CardRepository.GetSingleCard(entityPOCO.CarrierId, entityPOCO.Tenant, true);
          
           if (cardObject != null)
           {
               entityPM.CarrierName = cardObject.EnglishName;
           }

           Port originPort = portRep.GetSinglePortByCode(entityPOCO.Tenant, entityPOCO.Origin, false);
           if (originPort != null)
           {
               if (originPort.Country != null)
               {
                   entityPM.OriginCountryCode = originPort.Country.Code;
                   entityPM.OriginCountryName = originPort.Country.EnglishName;
               }
           }

           Port destinationPort = portRep.GetSinglePortByCode(entityPOCO.Tenant, entityPOCO.Destination, false);
           if (destinationPort != null)
           {
               if (destinationPort.Country != null)
               {
                   entityPM.DestinationCountryCode = destinationPort.Country.Code;
                   entityPM.DestinationCountryName = destinationPort.Country.EnglishName;
               }
           }

           BookingSpaceAllocationRepository allocationRepository = new BookingSpaceAllocationRepository(entityPOCO.Tenant);
           BookingSpaceAllocation spaceallocation = allocationRepository.GetSingle(entityPOCO.BookingSpaceAllocationCode);

           if (spaceallocation != null)
           {
               entityPM.SpaceAllocationName = spaceallocation.Name;
           }
       }
   }
}
   