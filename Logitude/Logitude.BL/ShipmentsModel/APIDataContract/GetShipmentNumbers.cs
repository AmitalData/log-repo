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
    public partial class GetShipmentNumbers
    {
        public string Id { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Direction { get; set; }
        public string TransportMode { get; set; }
        public string ShipmentLevel { get; set; }
        public string Master { get; set; }
        public string House { get; set; }
        public string Carrier { get; set; }
        public string ContainerNumber { get; set; }
    }

    sealed public class CustomValidateShipmentNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string validationResults = "";
            var getShipmentNumbersModel = validationContext.ObjectInstance as GetShipmentNumbers;

            if (string.IsNullOrEmpty(getShipmentNumbersModel.Master) && string.IsNullOrEmpty(getShipmentNumbersModel.House) 
                && string.IsNullOrEmpty(getShipmentNumbersModel.ContainerNumber))
                validationResults += " One of the master, house, container number is required. ";

            if (!string.IsNullOrEmpty(getShipmentNumbersModel.ContainerNumber) && getShipmentNumbersModel.TransportMode != "O")
                validationResults += " The Containers should be sent with the transport mode ocean. ";

            if (getShipmentNumbersModel.FromDate == null)
                validationResults += " From Date is required. ";

            if (getShipmentNumbersModel.ToDate == null)
                validationResults += " To Date is required. ";

            if (getShipmentNumbersModel.FromDate > getShipmentNumbersModel.ToDate)
                validationResults += " To Date can not be less than From date. ";

            else if (getShipmentNumbersModel.FromDate != null && getShipmentNumbersModel.ToDate != null 
                && (getShipmentNumbersModel.ToDate.Value.Year - getShipmentNumbersModel.FromDate.Value.Year) > 1)
                validationResults += " The Dates range must be not larger than 1 year. ";

            if (!string.IsNullOrEmpty(validationResults))
            {
                return new ValidationResult(validationResults);
            }

            return ValidationResult.Success;
        }
    }
}
