using Simplog.Server.Infrastructure.DataContracts;

namespace WebFreight.Web.Controllers.WorkflowModel.Models
{
    public class ApiQueryTreeFilters
    {
        public QueryFilterItem QueryFilterItem { get; set; }
        public string ReturnedColumns { get; set; }
        public int Tenant { get; set; }
        public int PageSize { get; set; }
    }
}