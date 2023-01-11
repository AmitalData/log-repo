
namespace Simplog.Server.Infrastructure.DataContracts.Models
{
    public class CheckObjectFieldExistenceRequest
    {
        public string ObjectTableId { get; set; }
        public string ProfileId { get; set; }
        public string FieldCode { get; set; }
    }
}
