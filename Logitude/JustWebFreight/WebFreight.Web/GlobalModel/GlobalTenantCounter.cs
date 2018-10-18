using System.ComponentModel.DataAnnotations;

namespace WebFreight.Web.GlobalModel
{
    public class GlobalTenantCounter
    {
        [Key]
        public int Id { get; set; }
        public int LastNumber { get; set; }
    }
}