using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class UserLicenseUpdateHelper
    {
        [Key]
        public int Tenant { get; set; }
        public List<UserLicensePM> Items { get; set; }
    }
}