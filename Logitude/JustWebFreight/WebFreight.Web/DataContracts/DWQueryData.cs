using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class DWQueryData
    {
        public DWSubQueryPM SubQueryData { get; set; }
        public List<DWObjectFieldsDetails> Columns { get; set; }
        public DWObjectFieldsDetails Filters { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string ColumnsSort { get; set; }
        public string FactTableName { get; set; }
        public string UserEmail { get; set; }

    }
}