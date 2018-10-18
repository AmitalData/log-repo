using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class FeaturesUpdateHelper
    {
        [Key]
        public int Tenant { get; set; }
        public string RoleId { get; set; }
        public string PackageCode { get; set; }
        public List<FeaturePM> Items { get; set; }
    }
}