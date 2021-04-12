using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class SystemDataPM
    {
        [Key]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public byte[] Signature { get; set; }
        public byte[] SignatureHtml { get; set; }
        
        public string Date { get; set; }
        public string LocalCurrencyId { get; set; }

        public string ContactId { get; set; }

        public string Company { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string IATA { get; set; }

        public string AddressId { get; set; }
        public string VatNumber { get; set; }
        public string Supportemail { get; set; }
        public string UserSignatureImage { get; set; }


        



    }
}