using System.Xml.Serialization;

namespace AmitalCloud.Infrastructure.APITools.ApiV1
{
    public partial class DocumentsFiling
    {


        public string Id { get; set; }

        [XmlAttribute]
        public string Code { get; set; }

        public User CreatedByUser { get; set; }

        public string EntityNumber { get; set; }

        public ObjectTable EntityType { get; set; }

        public DocumentType DocumentType { get; set; }

        public string BlobId { get; set; }

        public bool IsDigitallySigned { get; set; }

        public string SignersList { get; set; }

        public string BlobName { get; set; }

        public string Description { get; set; }

        public bool IsSharedWithCustomer { get; set; }

        public string ComputingPartnerCode { get; set; }

    }

}
