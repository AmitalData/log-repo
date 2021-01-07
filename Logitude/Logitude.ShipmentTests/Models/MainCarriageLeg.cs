namespace Logitude.ShipmentTests.Models
{
    public class MainCarriageLeg
    {
        public int LegIndex { get; set; }
        public Card Carrier { get; set; }
        public Port FromPort { get; set; }
        public Port ToPort { get; set; }
    }
}