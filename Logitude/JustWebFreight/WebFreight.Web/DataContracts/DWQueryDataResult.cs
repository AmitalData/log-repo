using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class DWQueryDataResult
    {
        public DataTable SQLDataResult { get; set; }
        
        public string SQLString { get; set; }
        public bool IsParentTenant { get; set; }
    }
}