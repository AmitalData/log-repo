using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BookingLib.BL.EntityPMs
{
    public partial class BookingPM
    {
        [DataMember]
        public bool IsFFRButtonClicked { get; set; }

        [DataMember]
        public bool IsCancellationButtonClicked { get; set; }

        //[DataMember]
        //public string MainCarriageFinalDestinationPortName { get; set; }

        [DataMember]
        public string MainCarriageSpaceAllocationName { get; set; }
        [DataMember]
        public string Transshipment1SpaceAllocationName { get; set; }
        [DataMember]
        public string Transshipment2SpaceAllocationName { get; set; }

        [DataMember]
        public bool IsFSU { get; set; }

        [DataMember]
        public bool IsUpdatedByChampAnalyzer { get; set; }

        //[DataMember]
        //public bool TenantZeroAirlineGLSHKFSRFSA { get; set; }

        //[DataMember]
        //public bool TenantZeroAirlineChampFSRFSA { get; set; }
    }
}
