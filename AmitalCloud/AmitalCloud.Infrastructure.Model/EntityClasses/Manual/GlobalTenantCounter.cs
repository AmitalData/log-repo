namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class GlobalTenantCounter
    {
	[Key]
        public int Id { get; set; }
        public int LastNumber { get; set; }
    }
}
