using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.LogitudeGridExportToExcel
{
    public class LogitudeGridExportToExcelArguments
    {
        public List<QueryColumnPM> QueryColumns { get; set; }
        public int Tenant { get; set; }
        public string UserId { get; set; }
        public string ObjectTableName { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string QuerySection { get; set; }
        public string QueryName { get; set; }

        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public List<QueryFilterItem> AdditionalFilters { get; set; }

        public bool IsXslxFormat { get; set; }  


    }
}
