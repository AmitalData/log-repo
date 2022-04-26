using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.Infrastructure.Data.Models
{
    public class BIReportsFilterArguments
    {
        public QueryOperations QueryOperations { get; set; }
        public int Tenant { get; set; }
        public bool GetAll { get; set; } = true;
        public string[] FactTableCodes { get; set; }
    }
}
