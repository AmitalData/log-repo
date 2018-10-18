
namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class QuoteCustomerTypeDetails
    {
        public QuoteCustomerTypeDetails()
        {
            this.ShowInLOV = true;
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public bool ShowInLOV { get; set; }
    }
}