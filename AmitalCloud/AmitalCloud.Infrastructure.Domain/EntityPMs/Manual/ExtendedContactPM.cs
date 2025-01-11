using System.Runtime.Serialization;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class ExtendedContactPM : ContactPM
    {
        [DataMember]
        public string UserRoles { get; set; }
    }
}
