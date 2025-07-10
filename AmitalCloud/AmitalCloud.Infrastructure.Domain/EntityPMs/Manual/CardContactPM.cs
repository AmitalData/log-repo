using AmitalCloud.Infrastructure.Domain.BaseClasses;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel.DomainServices.Server;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    [DataContract]
    public class CardContactPM : BaseEntityPM
    {
        public CardContactPM()
        {
            this.CardContactProducts = new List<CardContactProductPM>();
            this.CardContactAdditionalServices = new List<CardContactAdditionalServicePM>();
        }   

        public CardContactPM(CardContact entity)
        {
            this.Id = entity.Id;
            this.ContactId = entity.ContactId;
            this.CardId = entity.CardId;
            this.Tenant = entity.Tenant;
            this.InternetAccess = entity.InternetAccess;
            this.LastLoginDate = entity.LastLoginDate;
            this.IsHybrid = default;
            this.IsAirExport = entity.IsAirExport;
            this.IsAirImport = entity.IsAirImport;
            this.IsOceanExport = entity.IsOceanExport;
            this.IsOceanImport = entity.IsOceanImport;
            this.IsInlandExport = entity.IsInlandExport;
            this.IsInlandImport = entity.IsInlandImport;
            this.IsAll = entity.IsAll;
            this.IsCustomsImport = entity.IsCustomsImport;
            this.IsInlandDomestic = entity.IsInlandDomestic;
            this.CardContactProducts =  new List<CardContactProductPM>();
            this.CardContactAdditionalServices = new List<CardContactAdditionalServicePM>();
        }

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
