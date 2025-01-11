using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel.DomainServices.Server;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
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

        private List<CardContactProductPM> cardContactProducts;
        [Include]
        [Association("CardContactProductCardContact", "Id", "CardContactId")]
        [Composition]
        [DataMember]
        public virtual List<CardContactProductPM> CardContactProducts
        {
            get
            {

                if (this.cardContactProducts == null)
                {
                    cardContactProducts = new List<CardContactProductPM>();
                }
                return this.cardContactProducts;
            }
            set
            {
                if (value != null)
                {
                    cardContactProducts = value;
                }
            }
        }

        private List<CardContactAdditionalServicePM> cardContactAdditionalServices;
        [Include]
        [Association("CardContactAdditionalServiceCardContact", "Id", "CardContactId")]
        [Composition]
        [DataMember]
        public virtual List<CardContactAdditionalServicePM> CardContactAdditionalServices
        {
            get
            {

                if (this.cardContactAdditionalServices == null)
                {
                    cardContactAdditionalServices = new List<CardContactAdditionalServicePM>();
                }
                return this.cardContactAdditionalServices;
            }
            set
            {
                if (value != null)
                {
                    cardContactAdditionalServices = value;
                }
            }
        }
    }
}
