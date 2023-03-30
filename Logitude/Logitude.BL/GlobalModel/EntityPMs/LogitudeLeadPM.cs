using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.GlobalModel.EntityPMs
{
    public class LogitudeLeadPM
    {
        [Key]
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreateDate { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public int NumberOfBranches { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public int NumberOfUsers { get; set; }
        public string Comments { get; set; }



        public string LeadSource { get; set; }

        public string IATACode { get; set; }
        public string CASSCode { get; set; }

        public string PackageCode { get; set; }

        public string RequestType { get; set; }
        public bool IsEmailVerified { get; set; }
        public int TenantNumber { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public bool IsSentToCustomer { get; set; }
        public string StatusCode { get; set; }
        public bool UnassignedCountry { get; set; }
        public string CustomerId { get; set; }
        public bool IsUserOpened { get; set; }
        public string OpportunityId { get; set; }
        public string SearchFields { get; set; }


        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string VatNumber { get; set; }

        public bool IsUserEmailSent { get; set; }
        public string ClientId { get; set; }
        public string LeadOrigin { get; set; }
        public string Campaign { get; set; }

    }
}
