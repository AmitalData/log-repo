using Logitude.Customs.Data.DataContracts.SIIRequest;
using System;
using System.Collections.Generic;

namespace Logitude.Customs.Data.DataContracts.SIIRequest
{
    // ---- Root of the request -------------------------------------------------
    public class ReleaseRequestApiDto
    {
        public CredentialsDto credentials { get; set; }
        public ReleaseRequestFormDto releaseRequestForm { get; set; }
        public List<FormAttachmentDto> formAttachments { get; set; }
    }

    // ---- releaseRequestForm --------------------------------------------------
    public class ReleaseRequestFormDto
    {
        public string formApplicationId { get; set; }
        public int importerNumber { get; set; }
        public string importerEmail { get; set; }
        public string importerPhone { get; set; }
        public string importerCellPhone { get; set; }
        public string importerFax { get; set; }
        public long applicantIdNumber { get; set; }
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

        // contact person
        public string contactPersonFirstName { get; set; }
        public string contactPersonLastName { get; set; }
        public string contactPersonEmail { get; set; }
        public string contactPersonPhone { get; set; }
        public string contactPersonCellPhone { get; set; }
        public string contactPersonFax { get; set; }

        public bool isNumericCountryCode { get; set; }
        public List<ReleaseRequestLineDto> releaseRequestLinesForm { get; set; }
    }

    // ---- releaseRequestLinesForm --------------------------------------------
    public class ReleaseRequestLineDto
    {
        public int lineSerialNumber { get; set; }
        public string productFileNumber { get; set; }
        public string modelCode { get; set; }
        public string modelDescription { get; set; }
        public CountryAlphaDto originCountry { get; set; }
        public bool isDutchGroup1Requested { get; set; }
        public string customsItem { get; set; }
        public string supplier { get; set; }
        public decimal quantityToRelease { get; set; }
        public int siiUnitCode { get; set; }
        public string comment { get; set; }
        public List<int> formAttachmentIndexes { get; set; }
        public string supplierInvoiceNumber { get; set; }
        public DateTime supplierInvoiceDate { get; set; }
    }

    // ---- formAttachments -----------------------------------------------------
    public class FormAttachmentDto
    {
        public int formAttachmentIndex { get; set; }
        public IdDto attachmentType { get; set; }
        public string formAttachment { get; set; }      // optional
        public string temporaryUrlToDownload { get; set; }      // optional
        public string fileExtension { get; set; }
    }

    // ---- small value objects -------------------------------------------------
    public class CountryAlphaDto
    {
        public string alphaCode { get; set; }
    }

    public class IdDto
    {
        public int id { get; set; }
    }
}
