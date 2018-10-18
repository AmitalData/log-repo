using System.ComponentModel.DataAnnotations;

namespace WebFreight.Web.InfrastructureModel.DashBoard
{
    public class DashBoardClass
    {
        private static int counter = 0;
        public DashBoardClass()
        {
            linePrimary = ++counter;
        }

        [Key]
        public int linePrimary { get; set; }

        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public string directionID { get; set; }
        public string transportModeID { get; set; }

        public string countryCode { get; set; }
        public string countryName { get; set; }
        public string country { get; set; }

        public string shipmentTypeId { get; set; }

        public double total { get; set; }
        public decimal sumChargeableWeight { get; set; }
        public decimal sumGrossWeight { get; set; }

        public double totalLastMonth { get; set; }
        public decimal sumChargeableWeightLastMonth { get; set; }
        public decimal sumGrossWeightLastMonth { get; set; }

        public string CustomerID { get; set; }
        public string CustomerName { get; set; }

        public string XField { get; set; }
        public int YField { get; set; }
    }
}