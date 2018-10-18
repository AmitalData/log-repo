using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;


namespace Logitude.BL.CommonDataModel.EntityPMs
{
  public  class TwoFactorAuthenticationDevicePM
    {
        [Key]
        public string Id { get; set; }
        public string TwoFactorkey { get; set; }

        public int Tenant { get; set; }
        public string UserId { get; set; }
        public string DeviceDescription { get; set; }

        public DateTime? LastLoginDate { get; set; }
        public string LastLoginIP { get; set; }
        public string AuthenticationCode { get; set; }
        public bool InActive { get; set; }

        public DateTime CodeExpirationDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }



    }
}
