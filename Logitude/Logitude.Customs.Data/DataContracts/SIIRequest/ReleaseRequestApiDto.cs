using Logitude.Customs.Data.DataContracts.SIIRequest;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Logitude.Customs.Data.DataContracts.SIIRequest
{
    public class ReleaseRequestApiDto
    {
        public CredentialsDto credentials { get; set; }
        public ReleaseRequestFormDto releaseRequestForm { get; set; }
        public List<FormAttachmentDto> formAttachments { get; set; }                                              
    }

    public class ReleaseRequestFormDto
    {
        public string formApplicationId { get; set; }

        public long importerNumber { get; set; }

        public string importerEmail { get; set; }
        public string importerPhone { get; set; }
        public string importerCellPhone { get; set; }
        public string importerFax { get; set; }
        public string applicantIdNumber { get; set; }
        public string applicantFullName { get; set; }
        public long customsAgentRegisteredNumber { get; set; }
        public string customsAgentName { get; set; }
        public string agentFileId { get; set; }
        public CountryAlphaDto importCountry { get; set; }
        public IdDto destinationPort { get; set; }
        public DateTime deliveryArrivalDate { get; set; }
        public string deliveryComment { get; set; }
        public string shipFlightNumber { get; set; }
        public string billOfLadingId { get; set; }
        public int formAttachmentIndex { get; set; }
        public IdDto warehouseSettlement { get; set; }
        public string warehouseLocationName { get; set; }

        public string contactPersonFirstName { get; set; }
        public string contactPersonLastName { get; set; }
        public string contactPersonEmail { get; set; }
        public string contactPersonPhone { get; set; }
        public string contactPersonCellPhone { get; set; }
        public string contactPersonFax { get; set; }

        public bool isNumericCountryCode { get; set; }
        public List<ReleaseRequestLineDto> releaseRequestLinesForm { get; set; }
    }

    public class ReleaseRequestLineDto
    {
        public int lineSerialNumber { get; set; }
        public string productFileNumber { get; set; }
        public string productCode { get; set; }
        public string modelCode { get; set; }
        public string modelDescription { get; set; }
        public CountryAlphaDto originCountry { get; set; }
        public string manufacturer { get; set; }
        public bool isDutchGroup1Requested { get; set; }
        public string customsItem { get; set; }
        public string supplier { get; set; }
        public decimal? quantityToRelease { get; set; }
        public int? siiUnitCode { get; set; }
        public decimal? quantityByDeclaredUnit { get; set; }
        public string comment { get; set; }
        public List<int> formAttachmentIndexes { get; set; }
        public string supplierInvoiceNumber { get; set; }
        public DateTime? supplierInvoiceDate { get; set; }
        public string declaredUnitCode { get; set; }
        public IdProductDutchGroup productDutchGroup { get; set; }
        [JsonIgnore]         
        [XmlIgnore]         
        public int? UiLineNumber { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        public int InvoiceCounterKey { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public int InvoiceItemLineNumber { get; set; }
    }

    public class FormAttachmentDto
    {
        public int formAttachmentIndex { get; set; }
        public IdDto attachmentType { get; set; }
        public string attachmentDescription { get; set; }
        public string formAttachment { get; set; }
        public string temporaryUrlToDownload { get; set; }

        public string fileExtension { get; set; }
    }

    public class CountryAlphaDto{public string alphaCode { get; set; }}

    public class IdDto { public int id { get; set; } }
    public class IdProductDutchGroup { public int id { get; set; } }
}
