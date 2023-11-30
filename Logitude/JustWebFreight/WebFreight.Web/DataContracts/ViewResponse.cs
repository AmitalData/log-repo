namespace WebFreight.Web.DataContracts
{
    public class ServiceResponse
    {
        public object Result { get; set; }

        public int Count { get; set; }

        public long TookMS { get; set; }
    }
}
