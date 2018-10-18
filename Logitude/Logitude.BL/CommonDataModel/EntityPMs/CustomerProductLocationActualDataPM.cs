using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class CustomerProductLocationActualDataPM
    {
        [Key]
        public string CustomerId { get; set; }
        [Key]

        public string ProductTypeCode { get; set; }
        [Key]
        public int Month { get; set; }
        [Key]

        public int Year { get; set; }
        [Key]

        public string CountryId { get; set; }


        public int Tenant { get; set; }

        public decimal? TEU { get; set; }

        public int? NumberOfShipments { get; set; }

        public decimal? ChargeableWeight { get; set; }


        public decimal? Revenue { get; set; }

        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
}