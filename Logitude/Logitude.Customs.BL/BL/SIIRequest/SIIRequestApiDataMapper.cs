using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts.SIIRequest;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityKeys.Extended;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Logitude.Customs.BL.BL.SIIRequest
{
    public class SIIRequestApiDataMapper
    {
        int _tenant;
        readonly string SIIRequestComputingPartner = "SIIRequest";
        readonly string ComputingPartnerTableMeasurmentUnit = "Customs.MeasurmentUnit";
        readonly string ComputingPartnerTableUnloadingSiteType = "Customs.UnloadingSiteType";

        internal const string ItemNotCompleted = "Customs.SIIRequest.O.DataNotCompleted";
        internal const string LineCode = "Customs.SIIRequest.O.Line";
        internal const string FileTypeNotAllowed = "Customs.SIIRequest.O.FileTypeNotAllowed";


        public const string NoProduct = "0";
        public const int DutchGroup1 = 1;
        private static readonly HashSet<string> AllowedExts = new HashSet<string>(new[] { "pdf", "gif", "jpg" }, StringComparer.OrdinalIgnoreCase);

        private static readonly System.Text.RegularExpressions.Regex HebrewRegex = new System.Text.RegularExpressions.Regex(@"\p{IsHebrew}", System.Text.RegularExpressions.RegexOptions.Compiled);

        private static string SanitizePhone(string raw) =>
            string.IsNullOrWhiteSpace(raw)
            ? raw
            : new string(raw.Where(char.IsDigit).ToArray());
        public SIIRequestApiDataMapper(int tenant)
        {
            _tenant = tenant;
        }
        private static int ToInt(string value, string fieldName)
        {
            if (int.TryParse(value, out var n)) return n;
            throw new InvalidOperationException(
                $"Value '{value}' supplied for '{fieldName}' is not a valid integer.");
        }
        private static long ToLong(string value, string field) =>
            long.TryParse(value, out var n)
            ? n
            : throw new InvalidOperationException($"'{field}' value '{value}' is not a valid long.");


        public ReleaseRequestApiDto Build(CredentialsDto credentials,
            string siiRequestId,
            List<SiiSelectedRowDto> requestItemsKeys)
        {
            try
            {
                if (requestItemsKeys == null || requestItemsKeys.Count == 0)
                    throw new ArgumentException("No items selected", nameof(requestItemsKeys));

                var context = CustomContext.GetContext(_tenant);
                var siiService = new SIIRequestQueryService(context);
                var decService = new DeclarationQueryService(context);
                var requestItemsService = new SupplierInvoiceItemsReqListQueryService(context);
                var pointerRepo = new CustomsDocumentsTicketRepository(context);
                var filingRepo = new DocumentsFilingRepository(_tenant);
                var defService = new DefaultValueQueryService(context);
                var userQuery = new UserQuery(_tenant);
                var contactRepo = new ContactRepository(_tenant);

                var sii = siiService.GetSingle(siiRequestId, true, false)
                    ?? throw new ArgumentException($"SII Request {siiRequestId} not found");

                var dec = decService.GetDataForSIIRequest(sii.DeclarationId, _tenant);
                var importer = dec?.ImporterId != null
                    ? userQuery.GetSinglePM(dec.ImporterId, _tenant)
                    : null;

                var email = HttpContext.Current.User.Identity.Name;
                var contact = contactRepo.GetSingleContactByEmail(email, _tenant);

                var pointers = pointerRepo.GetPointersWithFilingId(requestItemsKeys, _tenant);
                var filingIds = pointers.Select(p => p.DocumentsFilingId)
                                        .Where(id => id != null)
                                        .Distinct()
                                        .ToList();
                var filingInfo = filingRepo.GetSecurityIdsByFilingIds(filingIds, _tenant)
                                    .ToDictionary(x => x.Id);

                var mainFormAttachmentIndexes = new List<int>();
                var invoiceDict = new Dictionary<string, List<int>>();
                var itemDict = new Dictionary<string, List<int>>();
                var attachments = new List<FormAttachmentDto>();

                int nextIndex = 1;
                var urlTemplate = GetMandatoryDefault(_tenant, "DownloadDocumentURL");
                var cloudTenant = GetMandatoryDefault(_tenant, "CloudTenant");

                foreach (var ptr in pointers)
                {
                    if (!filingInfo.TryGetValue(ptr.DocumentsFilingId, out var info)
                        || string.IsNullOrWhiteSpace(info.SecurityId))
                        continue;

                    var url = urlTemplate
                        .Replace("<SecurityID>", info.SecurityId)
                        .Replace("<Tenant>", cloudTenant);

                    var idx = nextIndex++;
                    attachments.Add(new FormAttachmentDto
                    {
                        formAttachmentIndex = idx,
                        attachmentType = new IdDto { id = ToInt(ptr.DocumentTypeCode, "DocumentTypeCode") },
                        attachmentDescription = ptr.DocumentTypeCode == "101"
                                ? ptr.DocumentTypeCodeName
                                : null,
                        temporaryUrlToDownload = url,
                        fileExtension = EnsureAllowedExtOrThrow(info.Extension, _tenant)
                    });

                    bool hasChild2 = !string.IsNullOrWhiteSpace(ptr.Child2EntityId);
                    bool hasChild3 = !string.IsNullOrWhiteSpace(ptr.Child3EntityId);

                    if (!hasChild2)
                    {
                        if (ptr.DocumentTypeCode == "1")
                            mainFormAttachmentIndexes.Add(idx);
                    }
                    else if (!hasChild3)
                    {
                        string key = $"{ptr.ParentEntityId}|{ptr.Child2EntityId}";
                        if (!invoiceDict.TryGetValue(key, out var list))
                            invoiceDict[key] = list = new List<int>();
                        list.Add(idx);
                    }
                    else
                    {
                        string key = $"{ptr.ParentEntityId}|{ptr.Child2EntityId}|{ptr.Child3EntityId}";
                        if (!itemDict.TryGetValue(key, out var list))
                            itemDict[key] = list = new List<int>();
                        list.Add(idx);
                    }
                }

                var form = BuildForm(sii, dec, importer, contact, defService, siiService);
                form.formAttachmentIndex = mainFormAttachmentIndexes.Count > 0
                    ? mainFormAttachmentIndexes[0]
                    : -1;

                _lineCounter = 0;
                form.releaseRequestLinesForm = requestItemsKeys.Select(k =>
                {
                    var line = BuildLine(k, requestItemsService);
                    string invoiceKey = $"{k.DeclarationId}|{k.InvoiceCounterKey}";
                    string itemKey = $"{k.DeclarationId}|{k.InvoiceCounterKey}|{k.InvoiceItemLineNumber}";
                    var idxs = (invoiceDict.TryGetValue(invoiceKey, out var inv) ? inv : Enumerable.Empty<int>())
                        .Concat(itemDict.TryGetValue(itemKey, out var itm) ? itm : Enumerable.Empty<int>())
                        .Distinct()
                        .ToList();
                    line.formAttachmentIndexes = idxs;
                    return line;
                }).ToList();

                var dto = new ReleaseRequestApiDto
                {
                    credentials = credentials,
                    releaseRequestForm = form,
                    formAttachments = attachments
                };

                SIIRequestValidator.Validate(dto, _tenant);
                return dto;
            }
            catch (InvalidOperationException) { throw; }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                throw new ApplicationException("Error building SII Request API data mapper", ex);
            }
        }

        private static string GetMandatoryDefault(int tenant, string key)
        {
            var value = DefaultService.Instance.Get(tenant, key, key)?.Value1;
            if (string.IsNullOrWhiteSpace(value))
                throw new ConfigurationErrorsException($"Default key '{key}' is missing for tenant {tenant}.");
            return value;
        }


        public string GetComputingPartnerCodeTranslation(string logitudeCode, string computingPartner, string objectTableName, int tenant)
        {
            ICommonDataContext context;
            ObjectTableRepository myObjectTableRepository;
            ComputingPartnerQuery computingPartnerQuery;
            ComputingPartnerTranslationQuery computingPartnerTranslationQuery;
            context = CommonDataContext.GetContext(tenant);
            myObjectTableRepository = new ObjectTableRepository(tenant);
            computingPartnerQuery = new ComputingPartnerQuery(new ComputingPartnerRepository(context));
            computingPartnerTranslationQuery = new ComputingPartnerTranslationQuery(new ComputingPartnerTranslationRepository(context));

            ObjectTable objectTable = myObjectTableRepository.GetObjectTableByName(objectTableName, 0, true);
            ComputingPartnerPM partner = computingPartnerQuery.GetSinglePMByCode(computingPartner, tenant);
            if (partner == null)
            {
                partner = computingPartnerQuery.GetSinglePMByCode(computingPartner, 0);
            }

            string partnerCode = null;
            if (partner != null && objectTable != null)
            {
                partnerCode = computingPartnerTranslationQuery.GetPartnerCodeTranslation(logitudeCode, partner.Id, objectTable.Id, tenant);
            }

            return partnerCode;
        }
        private ReleaseRequestFormDto BuildForm(
            SIIRequestPM sii,
            Declaration dec,
            UserPM importer,
            Contact contact,
            DefaultValueQueryService defService,
            SIIRequestQueryService siiService)
        {
            var agentName = defService.GetDefault("ISRAEL", "GGG_COMP_NAM_L", "NON", "NON", _tenant);
            string GetSiiCompanyName() => DefaultService.Instance.Get(_tenant, "SIIApplicationName", "SIIApplicationName")?.Value1
                ?? throw new InvalidOperationException("Default 'SIIApplicationName' is missing.");

            var formApplicationId = !string.IsNullOrWhiteSpace(sii.FromApplicationId)
                ? sii.FromApplicationId
                : $"{GetSiiCompanyName()}-{siiService.GetSIIFormApplicationMaxNumber(_tenant) + 1}";

            var contactName = contact?.LocalName;

            return new ReleaseRequestFormDto
            {
                formApplicationId = formApplicationId,
                customsAgentRegisteredNumber = dec?.AgentId != null
                    ? ToLong(dec.AgentId, "AgentId")
                    : 0,
                agentFileId = dec?.CustomFileNo,
                customsAgentName = agentName,

                importerNumber = importer?.Code != null
                ? ToLong(importer.Code, "ImporterCode")
                : dec?.ImporterCode != null
                ? ToLong(dec.ImporterCode, "ImporterCode")
                : 0,
                importerEmail = sii.ContactEmail,
                importerPhone = SanitizePhone(sii.ContactTel),
                importerCellPhone = SanitizePhone(sii.ContactCellPhone),
                importerFax = SanitizePhone(sii.ContactFax),

                applicantFullName = contactName,
                applicantIdNumber = contact == null
                    ? null
                    : new UserQuery(_tenant).GetPersonalIdByUserId(contact.Id, _tenant),

                deliveryArrivalDate = sii.UnloadDate,
                deliveryComment = sii.Remarks,
                shipFlightNumber = sii.VesselName,
                billOfLadingId = sii.ManifestNumber,

                contactPersonFirstName = contactName,
                contactPersonLastName = contactName,
                contactPersonEmail = contact?.Email,
                contactPersonPhone = SanitizePhone(contact?.BusinessPhone ?? contact?.Mobile),
                contactPersonCellPhone = SanitizePhone(contact?.Mobile ?? contact?.BusinessPhone),
                contactPersonFax = SanitizePhone(contact?.Fax),

                isNumericCountryCode = false, // we always send AlphaCode
                importCountry = new CountryAlphaDto { alphaCode = sii.OriginCountryCode },

                warehouseLocationName = sii.WareHouseAddress,
                warehouseSettlement = string.IsNullOrEmpty(sii.WareHouseCity)? null: new IdDto { id = ToInt(sii.WareHouseCity, "WareHouseCity") },
                destinationPort = new IdDto
                {
                    id = ToInt(
                        GetComputingPartnerCodeTranslation(
                            sii.UnloadPortCode,
                            SIIRequestComputingPartner,
                            ComputingPartnerTableUnloadingSiteType,
                            _tenant),
                        "destinationPort")
                }

            };
        }

        private ReleaseRequestLineDto BuildLine(
            SiiSelectedRowDto key,
            SupplierInvoiceItemsReqListQueryService service)
        {
            var item = service.GetRequest(
                key.SIIRequestID,
                key.DeclarationId,
                key.InvoiceCounterKey,
                key.InvoiceItemLineNumber,
                this._tenant );
            if (item == null)
            {
                var lineLbl = SIIRequestValidator.Translate(LineCode, _tenant);
                var dataMsg = SIIRequestValidator.Translate(ItemNotCompleted, _tenant);

                throw new InvalidOperationException(
                    $"{lineLbl} {key.UiIndex} - {dataMsg}");
            }

            var line = new ReleaseRequestLineDto
            {
                lineSerialNumber = ++_lineCounter,
                customsItem = item.ClassificationCode,
                productFileNumber = item.ProductFileNumber,
                quantityToRelease = item.InvoiceQuantity,
                siiUnitCode = ToInt(
                    GetComputingPartnerCodeTranslation(
                        item.InvoiceQuantityTypeCode,
                        SIIRequestComputingPartner,
                        ComputingPartnerTableMeasurmentUnit,
                        _tenant),
                    "siiUnitCode"),
                quantityByDeclaredUnit = item.StatisticQuantity,
                declaredUnitCode = GetComputingPartnerCodeTranslation(item.StatisticQuantityTypeCode, SIIRequestComputingPartner, ComputingPartnerTableMeasurmentUnit, _tenant),
                originCountry = new CountryAlphaDto { alphaCode = item.OriginCountryCode },
                manufacturer = item.ManufacturerName,
                modelCode = item.ItemNo,
                modelDescription = item.ItemName,
                comment = item.Remarks,
                isDutchGroup1Requested = string.IsNullOrWhiteSpace(item.ProductFileNumber),
                supplierInvoiceNumber = item.InvoiceNumber,
                supplierInvoiceDate = item.IssueDate,
                supplier = string.IsNullOrWhiteSpace(item.VendorName)
                ? item.VendorName
                : HebrewRegex                     
                .Replace(item.VendorName, "")
               .Trim()                       
               .Substring(0, Math.Min(20,     
                                      HebrewRegex
                                          .Replace(item.VendorName, "")
                                          .Trim().Length)),
                formAttachmentIndexes = new List<int>(),
               UiLineNumber = key.UiIndex
            };

            if (string.IsNullOrWhiteSpace(item.ProductFileNumber))
            {
                line.productCode = NoProduct;
                line.quantityToRelease = null;
                line.siiUnitCode = null;
                line.productDutchGroup = new IdProductDutchGroup { id = DutchGroup1 };
            }

            return line;
        }

        private int _lineCounter;


        private string EnsureAllowedExtOrThrow(string ext, int tenant)
        {
            if (string.IsNullOrWhiteSpace(ext) || !AllowedExts.Contains(ext))
            {
                string template = SIIRequestValidator.Translate(FileTypeNotAllowed, tenant);
                if (string.IsNullOrWhiteSpace(template))
                {
                    template = $"$Text({FileTypeNotAllowed})";
                }

                string bad = string.IsNullOrWhiteSpace(ext) ? "?" : ext;
                string allowedList = string.Join(", ", AllowedExts);

                string msg;
                try
                {
                    msg = string.Format(template, bad, allowedList);
                }
                catch (FormatException)
                {
                    msg = template + $" ({bad} / {allowedList})";
                }

                throw new InvalidOperationException(msg);
            }

            return ext.ToLowerInvariant();
        }

    }
}