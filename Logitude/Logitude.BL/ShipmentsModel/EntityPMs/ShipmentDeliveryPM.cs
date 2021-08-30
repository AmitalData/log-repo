using Logitude.BL.Validators;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    [CustomValidation(typeof(ShipmentDeliveryValidator), "IsShipmentDeliveryValid")]
    public class ShipmentDeliveryPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PickUpDeliveryNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PickUpDeliveryTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool FullResponsibility { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PickUpDeliveryFromTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PickUpDeliveryToTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FromPortId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ToPortId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FromPartnerCardId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FromAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FromAddress { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FromAddressCity { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FromAddressZipCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FromAddressCountryId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ToPartnerCardId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ToAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ToAddress { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ToAddressCity { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ToAddressZipCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ToAddressCountryId { get; set; }

        #region Port Dummy fields
        public string FromPortCode { get; set; }
        public string FromPortName { get; set; }
        public string FromPortCountryCode { get; set; }
        public string FromPortCountryName { get; set; }

        public string ToPortCode { get; set; }
        public string ToPortName { get; set; }
        public string ToPortCountryCode { get; set; }
        public string ToPortCountryName { get; set; }
        #endregion

        #region Address Dummy fields
        public string FromAddressCity_Dummy { get; set; }
        public string FromAddressCountryCode { get; set; }
        public string FromAddressCountryName { get; set; }
        public string FromLocation { get; set; }

        public string ToAddressCity_Dummy { get; set; }
        public string ToAddressCountryCode { get; set; }
        public string ToAddressCountryName { get; set; }
        public string ToLocation { get; set; }
        #endregion

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ATD { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ATA { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ETD { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ETA { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CarrierId { get; set; }
        public string CarrierCode { get; set; }
        public string CarrierName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CarrierNumber { get; set; }
        public string CarrierWebSite { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Driver { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TruckNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TrailerNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notes { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EmptyPickupContainerPartnerId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EmptyPickupDepotReference { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EmptyDeliveryContainerPartnerId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EmptyDeliveryDepotReference { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TransportModeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TransportModeName { get; set; }

        public string CustomerId { get; set; }
        public string DirectionId { get; set; }
        public string MasterNumber { get; set; }
        public string AgentName { get; set; }
        public string ShipmentNumber { get; set; }
        public string ShippingLine { get; set; }
        public double? PackageTEU { get; set; }
        public string AgentId { get; set; }
        public string ConnectedPackageId { get; set; }
        public List<string> AllConnectedPackagesId { get; set; }

        public bool IsCancelled { get; set; }

        public string ParentPickUpDeliveryId { get; set; }
        public int? ChildDeliveryIndex { get; set; }
        public string BookingConfirmationNumber { get; set; }

        private List<ShipmentPickUpDeliveryPackagePM> shipmentPickUpDeliveryPackages;
        [Include]
        [Composition]
        [Association("ShipmentDeliveryPackagePMShipmentDeliveryPM", "Id", "ShipmentPickUpDeliveryId")]
        public virtual List<ShipmentPickUpDeliveryPackagePM> ShipmentPickUpDeliveryPackages
        {
            get
            {
                if (shipmentPickUpDeliveryPackages == null)
                {
                    shipmentPickUpDeliveryPackages = new List<ShipmentPickUpDeliveryPackagePM>();
                }

                return this.shipmentPickUpDeliveryPackages;
            }
            set
            {
                if (value != null)
                {
                    shipmentPickUpDeliveryPackages = value;
                }
            }
        }

        public ChangeSetOperation ChangeSetOp { get; set; }

        public List<ShipmentPickUpDeliveryPackagePM> ShipmentDeliveryPackagesChangeSet { get; set; }

        public int SplitIndex { get; set; }
        public bool IsFromSplit { get; set; }
        public string StandaloneShipmentId { get; set; }
        public string StandaloneShipmentNumber { get; set; }
        public bool IsConnectedToStandalone { get; set; }
        public string CarrierLocalName { get; set; }

    }
}
