using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class PartnerExternalAccountsServicePM
    {
        [Key]
        public int Tenant { get; set; }
        public string CardId { get; set; }
        public string BusinessArea { get; set; }
        public string ExternalId2 { get; set; }
        public string ObjectTableName { get; set; }
        public List<CardExternalAccountsByProductPM> Items { get; set; }
    }
}