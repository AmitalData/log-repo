using System.Runtime.Serialization;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class ExtendedContactPM : ContactPM
    {
        [DataMember]
        public string UserRoles { get; set; }
    }
}
