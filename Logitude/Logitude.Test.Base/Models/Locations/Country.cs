namespace Logitude.Base.Models.Locations
{
    public class Country
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string GlobalZoneId { get; set; }
    }
}