namespace WebFreight.Web.Helpers
{
    public class APICredentialsParameters
    {
        public string PrimaryKey { get; set; }
        public string SecondaryKey { get; set; }
        public int Tenant { get; set; }
        public bool WithDocumentDownloadToken { get; set; }
    }
}