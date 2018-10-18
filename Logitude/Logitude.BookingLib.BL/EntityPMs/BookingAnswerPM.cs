using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BookingLib.BL.EntityPMs
{
    public partial class BookingAnswerPM
    {
        //[DataMember]
        //public string CarrierName { get; set; }

        //[DataMember]
        //public string OriginCountryName { get; set; }

        //[DataMember]
        //public string OriginCountryCode { get; set; }

        //[DataMember]
        //public string DestinationCountryCode { get; set; }

        //[DataMember]
        //public string DestinationCountryName { get; set; }

        [DataMember]
        public string AllotmentIdentification { get; set; } 

        [DataMember]
        public string SpaceAllocationName { get; set; }
    }
}
