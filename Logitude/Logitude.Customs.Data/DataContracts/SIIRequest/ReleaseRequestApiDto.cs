using Logitude.Customs.Data.DataContracts.SIIRequest;
using System;
using System.Collections.Generic;

namespace Logitude.Customs.Data.DataContracts.SIIRequest
{
    public class ReleaseRequestApiDto
    {
        public CredentialsDto Credentials { get; set; }
        public ReleaseRequestFormDto ReleaseRequestForm { get; set; }
        public List<FormAttachmentDto> FormAttachments { get; set; }
    }

    public class ReleaseRequestFormDto
    {
        public string FormApplicationId { get; set; }
        public string ImporterNumber { get; set; }
        public string ImporterEmail { get; set; }
        public string ImporterPhone { get; set; }
        public string ImporterCellPhone { get; set; }
        public string ImporterFax { get; set; }
        public string ApplicantIdNumber { get; set; }
        public string ApplicantFullName { get; set; }
        public string CustomsAgentRegisteredNumber { get; set; }
        public string CustomsAgentName { get; set; }
        public string AgentFileId { get; set; }
        public CountryAlphaDto ImportCountry { get; set; }
        public IdDto DestinationPort { get; set; }
        public DateTime DeliveryArrivalDate { get; set; }
        public string DeliveryComment { get; set; }
        public string ShipFlightNumber { get; set; }
        public string BillOfLadingId { get; set; }
        public int FormAttachmentIndex { get; set; }
        public IdDto WarehouseSettlement { get; set; }
        public string WarehouseLocationName { get; set; }

        // Contact person  
        public string ContactPersonFirstName { get; set; }
        public string ContactPersonLastName { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonPhone { get; set; }
        public string ContactPersonCellPhone { get; set; }
        public string ContactPersonFax { get; set; }

        public string IsNumericCountryCode { get; set; }
        public List<ReleaseRequestLineDto> ReleaseRequestLinesForm { get; set; }
    }

    public class ReleaseRequestLineDto
    {
        public int LineSerialNumber { get; set; }
        public string ProductFileNumber { get; set; }
        public string ProductCode { get; set; }
        public string ModelCode { get; set; }
        public string ModelDescription { get; set; }
        public CountryAlphaDto OriginCountry { get; set; }
        public string Manufacturer { get; set; }
        public bool IsDutchGroup1Requested { get; set; }
        public string CustomsItem { get; set; }
        public string Supplier { get; set; }
        public decimal? QuantityToRelease { get; set; }
        public string SiiUnitCode { get; set; }
        public decimal? QuantityByDecaredUnit { get; set; }
        public string Comment { get; set; }
        public List<int> FormAttachmentIndexes { get; set; }
        public string SupplierInvoiceNumber { get; set; }
        public DateTime? SupplierInvoiceDate { get; set; }
        public string DeclaredUnitCode { get; set; }
        public string VendorName { get; set; }
        public string ProductDutchGroup { get; set; }
    }

    public class FormAttachmentDto
    {
        public int FormAttachmentIndex { get; set; }
        public IdDto AttachmentType { get; set; }
        public string AttachmentDescription { get; set; } 
        public string FormAttachment { get; set; }        
        public string FileExtension { get; set; }
    }

    public class CountryAlphaDto
    {
        public string AlphaCode { get; set; }
    }

    public class IdDto
    {
        public string Id { get; set; }
    }
}
