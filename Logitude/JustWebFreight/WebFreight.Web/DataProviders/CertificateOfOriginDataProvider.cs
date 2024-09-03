using CWXSD;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class CertificateOfOriginDataProvider : BaseDataProvider
    {
        public List<CertificateOfOrigin> CertificateOfOrigin{ get; set; }
    }
    public class CertificateOfOrigin
    {
        public string CustomFileNo { get; set; }
        public string CooStatusCodeName { get; set; }
        public string COONumber { get; set; }
        public string CooTypeCodeName { get; set; }
    }
}