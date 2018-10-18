using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class ContainerFollowUpPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContactName { get; set; }
        public string CarrierName { get; set; }
        public string VesselName { get; set; }
        public string ShipmentNotes { get; set; }
        public string ShipperName { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeReference { get; set; }
        public string LongMaster { get; set; }
        public string House { get; set; }
        public string DirectionId { get; set; }
        public string DirectionName { get; set; }
        public string TransportModeId { get; set; }
        public string TransportModeName { get; set; }
        public string ShipmentType { get; set; }
        public string ShipmentLevelCode { get; set; }
        public string ShipmentLevelName { get; set; }
        public string StatusId { get; set; }
        public string ContainerTypeName { get; set; }
        public string ContainerNumber { get; set; }
        public string ShipperSeal { get; set; }
        public double? Volume { get; set; }
        public bool IsDangerous { get; set; }
        public string Description { get; set; }
        public string MarksAndNumbers { get; set; }

        public bool IsDeliveryFU { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DeliveryId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? DeliveryETD { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? DeliveryATD { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? DeliveryETA { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? DeliveryATA { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? DeliveryDeparture { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? DeliveryArrival { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DeliveryFrom { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DeliveryTo { get; set; }

        public bool IsEmptyContainerReturnFU { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EmptyContainerReturnId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? EmptyContainerReturnETD { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? EmptyContainerReturnATD { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? EmptyContainerReturnETA { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? EmptyContainerReturnATA { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ReturnDeparture { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ReturnArrival { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EmptyContainerReturnFrom { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EmptyContainerReturnTo { get; set; }
    }
}
