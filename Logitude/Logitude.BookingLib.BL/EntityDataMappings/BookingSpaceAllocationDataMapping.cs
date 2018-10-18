
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

namespace Logitude.BookingLib.BL.EntityDataMappings
{
   
   public partial class BookingSpaceAllocationDataMapping: IMapping<BookingSpaceAllocationPM, BookingSpaceAllocation>
   {

        public void CustomPMToPOCO(BookingSpaceAllocationPM entityPM, BookingSpaceAllocation entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(BookingSpaceAllocationPM entityPM, BookingSpaceAllocation entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   