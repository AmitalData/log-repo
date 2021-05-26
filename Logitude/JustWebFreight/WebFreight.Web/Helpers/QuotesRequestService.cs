using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class QuotesRequestService
    {


    }

    public class QuotesRequest
    {
        public string ReferenceNumber { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? QuotationUpdateDate { get; set; }
        public bool IsQuotationPrepard { get; set; }

    }

}