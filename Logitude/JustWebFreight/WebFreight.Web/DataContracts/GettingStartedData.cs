using System.ComponentModel.DataAnnotations;

namespace WebFreight.Web.DataContracts
{
    public class GettingStartedData
    {
        [Key]
        public int Id { get; set; }
        public int PortsCount { get; set; }
        public int AgentsCount { get; set; }
        public int CustomersCount { get; set; }
        public int UsersCount { get; set; }
        public int QuotesCount { get; set; }
        public int ShipmentsCount { get; set; }
        public int MastersCount { get; set; }
        public int AirlinesCount { get; set; }
        public int ShippinglinesCount { get; set; }
    }
}