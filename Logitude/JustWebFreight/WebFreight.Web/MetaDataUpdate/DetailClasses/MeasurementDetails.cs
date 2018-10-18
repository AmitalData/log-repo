namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class MeasurementDetails
    {
        public int Tenant { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool InActive { get; set; }
        public bool IsContainerMeasurement { get; set; }
        public bool IsContainer { get; set; }
        public string WeightUnitCode { get; set; }
        public string SearchFields { get; set; }
    }
}