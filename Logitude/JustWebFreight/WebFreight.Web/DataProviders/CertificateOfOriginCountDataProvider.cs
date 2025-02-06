using CWXSD;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class CertificateOfOriginCountDataProvider : BaseDataProvider
    {
        public List<CertificateOfOriginCount> CertificateOfOriginCount{ get; set; }
    }
    public class CertificateOfOriginCount
    {
        public int Tenant { get; set; }
        public string TenantName { get; set; }
        public int Count { get; set; }
    }
}