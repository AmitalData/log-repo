namespace WebFreight.Web.DataContracts
{
    public class StockSeries
    {
        public int From { get; set; }
        public int To { get; set; }
        public int Total { get; set; }
        public string AirlineId { get; set; }
        public string CustomerId { get; set; }
        public string AirlineName { get; set; }
        public int Tenant { get; set; }
    }


  
}