using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BookingLib.BL.EntityPMs
{
    public partial class BookingPackagePM
    {
        [DataMember]
        public bool IsAWBWizardDefault { get; set; }

        //[DataMember]
        //public string PackageTypeName { get; set; }
    }
}
