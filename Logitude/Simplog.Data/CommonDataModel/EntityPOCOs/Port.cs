using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    
    public class Port
    {
        [Key]
        public string Id { get; set; }

        private string code;
        //[Display(Name="Code")]
        //[Required]
        //[StringLength(3, MinimumLength = 3, ErrorMessage = "The length of the code must be 3")]
       
        public string Code
        {
            get { return code; }
            set
            {
                code = value != null ? value.ToUpper() : value;
            }
        }
        
        //[Display(Name = "Tenant")]
        //[Required]
        public int Tenant { get; set; }

        //[StringLength(120, ErrorMessage = "The Maximum length Of The Name Is 120! ")]
        //[Required]
        //[Display(Name = "Name")]
        public string EnglishName { get; set; }

        //[StringLength(120, ErrorMessage = "The Maximum length Of The Name Is 120 ")]
        //[Required]
        //[Display(Name = "Local Name")]
        public string  LocalName { get; set; }
       
        //[Display(Name = "Remark")]
        //[StringLength(250, ErrorMessage = "The maximum length of the remarks is 250!")]
        public string Notes { get; set; }
        
        //[Required]

        //[Display(Name = "Country")]
        public string CountryId { get; set; }



        //[Display(Name = "Inactive")]
        //[Required]
        public bool InActive { get; set; }

        //[Required]
        //[Display(Name = "Air")]
        public bool IsAir { get; set; }

        //[Required]
        //[Display(Name = "Ocean")]
        public bool IsOcean { get; set; }

        //[Required]
        //[Display(Name = "Ground")]
        public bool IsInland { get; set; }

        //[Required]
        //[Display(Name = "Added Manually")]
        public bool AddedManually { get; set; }

        public double Latitude { get; set; }
        public double Longtitude { get; set; }

        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }
        public string SearchFields { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string CombinedCode { get; set; }

        [ForeignKey("StateId")]
        public virtual State State { get; set; }
        //[ExternalReference]
        //[Association("PortCountry", "CountryId", "Id", IsForeignKey = true)]
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }


        //public List<Shipment> FromPortShipments { get; set; }
        //public List<Shipment> ToPortShipments { get; set; }
        //public List<ShipmentMasterData> MainCarriageFromShipmentMasterDatas { get; set; }
        

        //public List<ShipmentMasterData> MainCarriageToShipmentMasterDatas { get; set; }
        //public List<ShipmentMasterData> MainCarriageFinalDestinationShipmentMasterDatas { get; set; }

       
        //public List<Shipment> PreCarriageFromShipments { get; set; }
        //public List<Shipment> OnCarriageFromShipments { get; set; }
        //public List<Shipment> DelivaryFromShipments { get; set; }
        

        //public List<Shipment> PreCarriageToShipments { get; set; }
        //public List<Shipment> OnCarriageToShipments { get; set; }
        
        //public List<Shipment> PickUpToShipments { get; set; }


        //public List<Quote> FromPortQuotes { get; set; }

        //public List<Quote> ToPortQuotes { get; set; }

        //public List<ShipmentMasterData> Transshipment1FromShipmentMasterDatas { get; set; }
        //public List<ShipmentMasterData> Transshipment1ToShipmentMasterDatas { get; set; }

        //public List<ShipmentMasterData> Transshipment2FromShipmentMasterDatas { get; set; }
        //public List<ShipmentMasterData> Transshipment2ToShipmentMasterDatas { get; set; }

        //public List<ShipmentMasterData> Transshipment3FromShipmentMasterDatas { get; set; }
        //public List<ShipmentMasterData> Transshipment3ToShipmentMasterDatas { get; set; }
        //public List<Shipment> Shipments { get; set; }
        //public List<ShipmentPickUp> FromShipmentPickUps { get; set; }
        //public List<ShipmentPickUpDelivery> FromShipmentPickUpDeliveries { get; set; }

        //public List<ShipmentPickUpDelivery> ToShipmentPickUpDeliveries { get; set; }
        //public List<ShipmentPickUp> ToShipmentPickUps { get; set; }
       
        //public List<Quote> PickUpQuotes { get; set; }
        //public List<Quote> DeliveryQuotes { get; set; }

        //public List<TarrifFromTo> TarrifFromToes { get; set; }

        //public List<ShipmentCarrierStatus> FromShipmentCarrierStatus { get; set; }
        //public List<ShipmentCarrierStatus> ToShipmentCarrierStatus { get; set; }
        //public List<ShipmentCarrierStatus> LocationShipmentCarrierStatus { get; set; }
      

        
    }
}
