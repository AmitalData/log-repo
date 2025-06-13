using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class UserPermittedProductPM : BaseClasses.BaseEntityPM
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ProductTypeCode { get; set; }

    }
}
