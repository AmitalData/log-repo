using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.QuoteModel.EntityPMs
{
    public class QuoteComputedFieldPM
    {

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool ConnectedToShipment { get; set; }
        public bool ConnectedToTicket { get; set; }
        public string ToLocation {get; set;}
        public string FromLocation { get; set; }
        public string DeliveryFrom { get; set; }
        public string PickupFrom { get; set; }

    }
}
