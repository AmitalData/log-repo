using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts.SIIRequest;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
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
        public const string NoProduct = "0";
        public const string DutchGroup1 = "1";

        public SIIRequestApiDataMapper(int tenant)
        {
            _tenant = tenant;
        }
        public ReleaseRequestApiDto Build(CredentialsDto credentials,
        string siiRequestId, List<SupplierInvoiceItemsReqListKeys> requestItemsKeys)
        {
            try
            {
                if (requestItemsKeys == null || requestItemsKeys.Count == 0)
                    throw new ArgumentException("No items selected", nameof(requestItemsKeys));

                var context = CustomContext.GetContext(_tenant);
                var siiService = new SIIRequestQueryService(context);
                var decService = new DeclarationQueryService(context);
                var requestItemsService = new SupplierInvoiceItemsReqListQueryService(context);
                var defService = new DefaultValueQueryService(context);
                var userService = new UserQuery(_tenant);
                var contactRepo = new ContactRepository(_tenant);

                var sii = siiService.GetSingle(siiRequestId, true, false) 
                    ?? throw new ArgumentException($"SII Request {siiRequestId} not found");

                var dec = decService.GetDataForSIIRequest(sii.DeclarationId, _tenant);
                var importer = dec?.ImporterId != null
                    ? userService.GetSinglePM(dec.ImporterId, _tenant) 
                    : null;

                // get the logged‐in contact
                var email = HttpContext.Current.User.Identity.Name;
                var contact = contactRepo.GetSingleContactByEmail(email, _tenant);

                var form = BuildForm(sii, dec, importer, contact, defService, siiService);

                form.ReleaseRequestLinesForm = requestItemsKeys
                    .Select(k => BuildLine(k, requestItemsService))
                    .ToList();

                var attachments = new List<FormAttachmentDto>();

                return new ReleaseRequestApiDto
                {
                    Credentials = credentials,
                    ReleaseRequestForm = form,
                    FormAttachments = attachments
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error building SII Request API data mapper", ex);
            }
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
            // Default agent name
            var agentName = defService.GetDefault("ISRAEL", "GGG_COMP_NAM_L", "NON", "NON", _tenant);

            // Company name (mandatory)
            var siiCompanyName = DefaultService.Instance.Get(_tenant, "SIIApplicationName", "SIIApplicationName")?.Value1;
            if (string.IsNullOrWhiteSpace(siiCompanyName))
                throw new InvalidOperationException("Please set a default value for 'SIIApplicationName'.");

            // New form-application ID
            var nextSequence = siiService.GetSIIFormApplicationMaxNumber(_tenant) + 1;
            var nextId = $"{siiCompanyName}-{nextSequence}";

            return new ReleaseRequestFormDto
            {
                FormApplicationId = nextId,
                CustomsAgentRegisteredNumber = dec?.AgentId,
                AgentFileId = dec?.CustomFileNo,
                CustomsAgentName = agentName,

                ImporterNumber = importer?.Id ?? dec?.ImporterCode,
                ImporterEmail = importer?.Email,
                ImporterPhone = importer?.BusinessPhone,
                ImporterCellPhone = importer?.BusinessPhone,
                ImporterFax = importer?.Fax,

                ApplicantFullName = contact?.LocalName,
                ApplicantIdNumber = contact == null
                                                ? null
                                                : new UserQuery(_tenant).GetPersonalIdByUserId(contact.Id, _tenant),

                DeliveryArrivalDate = sii.UnloadDate,
                DeliveryComment = sii.Remarks,
                ShipFlightNumber = sii.VesselName,
                BillOfLadingId = sii.ManifestNumber,
                ContactPersonFirstName = contact.LocalName,
                ContactPersonLastName = contact.LocalName,
                ContactPersonEmail = contact.Email,
                ContactPersonPhone = contact.BusinessPhone,
                ContactPersonCellPhone = contact.Mobile,
                ContactPersonFax = contact.Fax,
                IsNumericCountryCode = CountryCode.alphaCode.ToString(),
                ImportCountry = new CountryAlphaDto { AlphaCode = sii.OriginCountryCode },

                WarehouseLocationName = sii.WareHouseAddress,
                WarehouseSettlement = new IdDto { Id = sii.WareHouseCity },
                DestinationPort = new IdDto
                {
                    Id = GetComputingPartnerCodeTranslation(
                            sii.UnloadPortCode,
                            SIIRequestComputingPartner,
                            ComputingPartnerTableUnloadingSiteType,
                            _tenant)
                },

                FormAttachmentIndex = 0 // “until documents are figured out”
            };
        }
        private ReleaseRequestLineDto BuildLine(
           SupplierInvoiceItemsReqListKeys key,
           SupplierInvoiceItemsReqListQueryService requestItemsService)
        {
            var item = requestItemsService.GetSingle(
                key.DeclarationId,
                key.LineNumber,
                key.SIIRequestID,
                key.InvoiceCounterKey,
                key.InvoiceItemLineNumber,
                true,
                false);

            if (item == null)
            {
                throw new ArgumentException(
                    $"Line {key.LineNumber} / Declaration {key.DeclarationId} not found in SII Request {key.SIIRequestID}",
                    nameof(key));
            }

            var line = new ReleaseRequestLineDto
            {
                LineSerialNumber = ++_lineCounter, 
                CustomsItem = item.ClassificationCode,
                ProductFileNumber = item.ProductFileNumber,
                QuantityToRelease = item.InvoiceQuantity,
                SiiUnitCode = GetComputingPartnerCodeTranslation(
                                                item.InvoiceQuantityTypeCode,
                                                SIIRequestComputingPartner,
                                                ComputingPartnerTableMeasurmentUnit,
                                                _tenant),
                QuantityByDecaredUnit = item.StatisticQuantity,
                DeclaredUnitCode = GetComputingPartnerCodeTranslation(
                                                item.StatisticQuantityTypeCode,
                                                SIIRequestComputingPartner,
                                                ComputingPartnerTableMeasurmentUnit,
                                                _tenant),
                OriginCountry = new CountryAlphaDto { AlphaCode = item.OriginCountryCode },
                Manufacturer = item.ManufacturerName,
                ModelCode = item.ItemNo,
                ModelDescription = item.ItemName,
                Comment = item.Remarks,
                IsDutchGroup1Requested = item.DutchRequested,
                SupplierInvoiceNumber = item.InvoiceNumber,
                SupplierInvoiceDate = item.IssueDate,
                VendorName = item.VendorName
            };

            if (string.IsNullOrEmpty(item.ProductFileNumber))
            {
                line.ProductCode = NoProduct;
                line.QuantityToRelease = null;
                line.SiiUnitCode = null;
                line.QuantityByDecaredUnit = null;
                line.DeclaredUnitCode = null;
                line.ProductDutchGroup = DutchGroup1;
            }
            line.FormAttachmentIndexes = new List<int>();
            return line;
        }
        private int _lineCounter = 0;


    }
    enum CountryCode
    {
        numeric = 1,
        alphaCode = 2,
    }
}
