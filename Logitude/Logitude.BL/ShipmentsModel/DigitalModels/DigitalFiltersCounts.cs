namespace Logitude.BL.ShipmentsModel.DigitalModels
{
    public class DigitalFiltersCounts
    {
        public int All { get; set; }
        public int Active { get; set; }
        public int Closed { get; set; }
        public int AtOrigin { get; set; }
        public int AtOriginAir { get; set; }
        public int AtOriginOcean { get; set; }
        public int AtOriginInland { get; set; }
        public int InTransit { get; set; }
        public int InTransitAir { get; set; }
        public int InTransitOcean { get; set; }
        public int InTransitInland { get; set; }
        public int AtDestination { get; set; }
        public int AtDestinationAir { get; set; }
        public int AtDestinationOcean { get; set; }
        public int AtDestinationInland { get; set; }
    }
}
