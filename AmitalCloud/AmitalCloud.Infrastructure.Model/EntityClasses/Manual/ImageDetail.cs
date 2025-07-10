using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class ImageDetail
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Extension { get; set; }
        public double Size { get; set; }

        // public List<Contact> Contacts { get; set; }
        //  public List<Card> Cards { get; set; }
    }
}