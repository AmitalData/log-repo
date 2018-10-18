using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [DataContract]
    public class CardExternalCodeByCurrencyPM
    {
        [Key]
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public int Tenant { get; set; }
        [DataMember]
        public string CardId { get; set; }
        [DataMember]
        public string CurrencyId { get; set; }
        [DataMember]
        public string ExternalRecievableTableId { get; set; }
        [DataMember]
        public string ExternalPayableTableId { get; set; }
        [DataMember]
        public string CurrencyCode { get; set; }
        [DataMember]
        public string ExternalTableName {get; set;}
        [DataMember]
        public string ExternalTableCode { get; set; }
         [DataMember]
        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}