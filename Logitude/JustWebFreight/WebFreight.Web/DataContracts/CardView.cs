using System;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices;

using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace WebFreight.Web.DataContracts
{
    public class CardView
    {
       
        public string Id { get; set; }

         [Key]
        public string CardViewId { get; set; }

        public string EnglishName { get; set; }
        public int Tenant { get; set; }

       
        
        public string VatNumber { get; set; }

        public string CountryId { get; set; }

        public string Type { get; set; }

        public string CityName { get; set; }
        public string LocalName { get; set; }

        public string Code { get; set; }
        public bool InActive { get; set; }
        
        //public string PaymentTermId { get; set; }
         
        //public string PartnerTypeId { get; set; }
         
        //public string AccountingCard { get; set; }

        public DateTime? CreateDate { get; set; }

        [ExternalReference]
        [Association("Mohammad", "CountryId", "Id", IsForeignKey = true)]
        public Country Country { get; set; }

    }
}