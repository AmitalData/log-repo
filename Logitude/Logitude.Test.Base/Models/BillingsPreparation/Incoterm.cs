namespace Logitude.Base.Models.BillingsPreparation
{
    public class Incoterm
    {
        public string Code { get; set; }
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string Freight { get; set; }
        public string OtherCharges { get; set; }
    }
}