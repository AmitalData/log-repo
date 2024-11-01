using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class CardContact
    {
        [Key] 
        public string Id { get; set; }
        public string ContactId { get; set; }
        public string CardId { get; set; }
        public int Tenant { get; set; }
        public bool InternetAccess { get; set; }


        public bool IsAirExport {get; set;}
        public bool IsAirImport { get; set; }
        public bool IsOceanExport { get; set; }
        public bool IsOceanImport { get; set; }
        public bool IsInlandExport { get; set; }
        public bool IsInlandImport {get; set;}
        public bool IsCustomsImport { get; set; }
        public bool IsInlandDomestic { get; set; }
        public bool IsAll { get; set; }

        //[Include]
        //[Association("CardContactCard", "CardId", "Id", IsForeignKey = true)]
        [ForeignKey("CardId")]
        public virtual Card Card { get; set; }

        //[Include]
        //[Association("ContactCardContact", "ContactId", "Id", IsForeignKey = true)]
        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }

        public DateTime? LastLoginDate { get; set; }
    }
}