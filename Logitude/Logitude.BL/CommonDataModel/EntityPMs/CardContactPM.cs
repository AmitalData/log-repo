using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [DataContract]
    public class CardContactPM
    {
        [Key]
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string ContactId { get; set; }
        [DataMember]
        public string CardId { get; set; }
        [DataMember]
        public int Tenant { get; set; }
        [DataMember]
        public bool InternetAccess { get; set; }
        [DataMember]
        public DateTime? LastLoginDate { get; set; }
        [DataMember]
        public bool IsHybrid { get; set; }

        [DataMember]
        public bool IsAirExport { get; set; }
        [DataMember]
        public bool IsAirImport { get; set; }
        [DataMember]
        public bool IsOceanExport { get; set; }
        [DataMember]
        public bool IsOceanImport { get; set; }
        [DataMember]
        public bool IsInlandExport { get; set; }
        [DataMember]
        public bool IsInlandImport { get; set; }
        [DataMember]
        public bool IsAll { get; set; }
        [DataMember]
        public bool IsCustomsImport { get; set; }
        [DataMember]
        public bool IsInlandDomestic { get; set; }
    }
}
