using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Xml.Serialization;

namespace Logitude.BL.ShipmentsModel.APIDataContract.ApiV1
{
    [CustomValidateShipmentNumber]
    public partial class ShipmentNumbers
    {
        public string Id { get; set; }
        [Required]
        public DateTime? FromDate { get; set; }
        [Required]
        public DateTime? ToDate { get; set; }
        [Required]
        public Direction Direction { get; set; }
        public TransportMode TransportMode { get; set; }
        public ShipmentType ShipmentType { get; set; }
        public string Master { get; set; }
        public string House { get; set; }
        public string Carrier { get; set; }
        public List<Container> Containers { get; set; }
    }
    public partial class Container
    {
        [XmlAttribute]
        public string Number { get; set; }
    }

    sealed public class CustomValidateShipmentNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string validationResults = "";
            object shipmentInstance = validationContext.ObjectInstance;
            Type shipmentInstanceType = shipmentInstance.GetType();

            PropertyInfo property_Master = shipmentInstanceType.GetProperty("Master");
            PropertyInfo property_House = shipmentInstanceType.GetProperty("House");
            PropertyInfo property_Containers = shipmentInstanceType.GetProperty("Containers");
            PropertyInfo property_FromDate = shipmentInstanceType.GetProperty("FromDate");
            PropertyInfo property_ToDate = shipmentInstanceType.GetProperty("ToDate");
            string propertyValue_Master = (string)property_Master.GetValue(shipmentInstance);
            string propertyValue_House = (string)property_House.GetValue(shipmentInstance);
            DateTime? propertyValue_FromDate = (DateTime?)property_FromDate.GetValue(shipmentInstance);
            DateTime? propertyValue_ToDate = (DateTime?)property_ToDate.GetValue(shipmentInstance);
            List<Container> propertyValue_Containers = (List<Container>)property_Containers.GetValue(shipmentInstance);

            if (string.IsNullOrEmpty(propertyValue_Master) && string.IsNullOrEmpty(propertyValue_House) && (propertyValue_Containers == null || (propertyValue_Containers!= null && propertyValue_Containers.Count == 0)))
                validationResults += "One of the master, house, container number is required.";

            if (propertyValue_FromDate == null)
                validationResults += "From Date is required.";

            if (propertyValue_ToDate == null)
                validationResults += "To Date is required.";

            if (propertyValue_FromDate > propertyValue_ToDate)
                validationResults += "To Date can not be less than From date.";

            else if (propertyValue_FromDate != null && propertyValue_ToDate != null && (propertyValue_ToDate.Value.Year - propertyValue_FromDate.Value.Year) < 1)
                validationResults += "The Dates range must be larger than 1 year.";

            if (!string.IsNullOrEmpty(validationResults))
            {
                return new ValidationResult(validationResults);
            }
            return ValidationResult.Success;
        }
    }
}
