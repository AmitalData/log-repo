using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Address
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string StateId { get; set; }
        public string ZipCode { get; set; }
        public string FaxNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string ATTN { get; set; }
        public string AddressTypeId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Name { get; set; }
        public string CountryId { get; set; }
        public string CardId { get; set; }
        public bool  InActive { get; set; }
        public bool IsLocalLanguage { get; set; }
        public string SearchFields { get; set; }
        public DateTime? AutomaticLastUpdateDate { get; set; }
        public string ExternalId { get; set; }

        [ForeignKey("AddressTypeId")]
        public virtual AddressType AddressType { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        [ForeignKey("StateId")]
        public virtual State State { get; set; }

        [ForeignKey("CardId")]
        public Card Card { get; set; }

        [ForeignKey("ResponsibilityEntity")]
        [Column("Responsibility")]
        public string Responsibility { get; set; }

        public virtual Responsibility ResponsibilityEntity { get; set; }


        [ForeignKey("Trucker")]
        [Column("TruckerId")]
        public string TruckerId { get; set; }

        public virtual Trucker Trucker { get; set; }

        [ForeignKey("CountryCity")]
        [Column("CityId")]
        public string CityId { get; set; }

        public virtual CountryCity CountryCity { get; set; }


        public string TransportationInstructions { get; set; }



    }
}
