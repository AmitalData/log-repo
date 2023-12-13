using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.CommonDataModel
{
    public class AllActiveGLAccountsView
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
    }
}