
namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class ShipmentCustomerTypeDetails
    {
        public ShipmentCustomerTypeDetails()
        {
            this.ShowInLOV = true;
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        public bool ShowInLOV { get; set; }
    }
}