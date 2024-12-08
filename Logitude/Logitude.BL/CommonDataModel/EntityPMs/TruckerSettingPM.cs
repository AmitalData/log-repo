using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    [DataContract]
    public class TruckerSettingPM
    {
        [Key]
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public int Tenant { get; set; }
        [DataMember]
        public string SearchFields { get; set; }
        [DataMember]
        public string AddressId { get; set; }
        [DataMember]
        public string FromAddressCityId { get; set; }
        [DataMember]
        public string ToAddressCityId { get; set; }
        [DataMember]
        public string ShipmentType { get; set; }
        [DataMember]
        public string TruckerId { get; set; }
        [DataMember]
        public string Responsibility { get; set; }


    }
}
