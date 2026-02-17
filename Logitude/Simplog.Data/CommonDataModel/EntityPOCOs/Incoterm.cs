using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Incoterm
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        //[Required]
        //[StringLength(3, ErrorMessage = "The maximum length of the code is 3!")]
        public string Code { get; set; }
        //[Required]
        //[StringLength(40, ErrorMessage = "The maximum length of the name is 40!")]
       
        public string Name { get; set; }
        //[Required]
        //[StringLength(40, ErrorMessage = "The maximum length of the name is 40!")]
        //[Display(Name = "Local Name")]
        public string LocalName { get; set; }
        //[Required]
        //[StringLength(1, ErrorMessage = "The maximum length of the Freight must be  1!")]
        public string Freight { get; set; }
        //[Required]
        //[StringLength(1, ErrorMessage = "The maximum length of the other charges must be  1!")]
        //[Display(Name = "Other Charges")]
        public string OtherCharges { get; set; }
        //[Display(Name = "Added Manually")]
        public bool AddedManually { get; set; }
        //[Display(Name = "Inactive")]
        public bool InActive { get; set; }
        //[StringLength(250, ErrorMessage = "The maximum length of the remarks is 250!")]
        public string Notes { get; set; }
        public string SearchFields { get; set; }
        ////[Include]
        ////[Association("ShipmentIncoterm", "Id", "IncotermId")]
        //public List<Shipment> Shipments { get; set; }

        //[ExternalReference]
        //[Association("PrepaidCollectFreight", "Freight", "Id", IsForeignKey = true)]

        [ForeignKey("Freight")]
        public PrepaidCollect FreightPrepaidCollect { get; set; }

        //[ExternalReference]
        //[Association("PrepaidCollectOtherCharges", "OtherCharges", "Id", IsForeignKey = true)]
        [ForeignKey("OtherCharges")]
        public PrepaidCollect OtherChargesPrepaidCollect { get; set; }

        //[]
        ////[Association("QuoteIncoterm", "Id", "IncotermId")]
        //public List<Quote> Quotes { get; set; }
      

    }
}