
using System;
using System.ComponentModel.DataAnnotations;


namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class TwoFactorAuthenticationDeviceList
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

