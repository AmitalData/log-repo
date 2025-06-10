using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts.SIIRequest;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Server.Tools;
using Logitude.Server.Tools.RestRequestExecutor;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mail;
using System.Windows.Forms;

namespace Logitude.Customs.BL.BL.SIIRequest
{
    public class SIIRequestApiDataMapper
    {
        int _tenant;
        readonly string SIIRequestComputingPartner = "SIIRequest";
        readonly string ComputingPartnerTableMeasurmentUnit = "Customs.MeasurmentUnit";
        readonly string ComputingPartnerTableUnloadingSiteType = "Customs.UnloadingSiteType";
        public SIIRequestApiDataMapper(int tenant)
        {
            _tenant = tenant;
        }
        public ReleaseRequestApiDto Build(CredentialsDto credentials,
        string siiRequestId, List<SupplierInvoiceItemsReqListKeys> requestItemsKeys)
        {
            try
            {
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

                // default agent name
                var agentName = defService
                    .GetDefault("ISRAEL", "GGG_COMP_NAM_L", "NON", "NON", _tenant);
                    
            
                var SIICompanyName = DefaultService.Instance.Get(_tenant, "SIIApplicationName", "SIIApplicationName")?.Value1;
                var maxNumber = siiService.GetSIIFormApplicationMaxNumber(_tenant) + 1;
                var nextId = $"{SIICompanyName}-{maxNumber}";


                var form = new ReleaseRequestFormDto
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
                    ApplicantIdNumber = contact is null
                                         ? null
                                         : userService.GetPersonalIdByUserId(contact.Id, _tenant),

                    DeliveryArrivalDate = sii.UnloadDate,
                    DeliveryComment = sii.Remarks,
                    ShipFlightNumber = sii.VesselName,
                    BillOfLadingId = sii.ManifestNumber,

                    ContactPersonFirstName = sii.ContactName,
                    ContactPersonLastName = sii.ContactName,
                    ContactPersonEmail = sii.ContactEmail,
                    ContactPersonPhone = sii.ContactCellPhone,
                    ContactPersonCellPhone = sii.ContactCellPhone,
                    ContactPersonFax = sii.ContactFax,

                    IsNumericCountryCode = CountryCode.alphaCode.ToString(),
                    ImportCountry = new CountryAlphaDto { AlphaCode = sii.OriginCountryCode },

                    WarehouseLocationName = sii.WareHouseAddress,
                    WarehouseSettlement = new IdDto { Id = sii.WareHouseCity },
                    DestinationPort = new IdDto
                    {
                        Id = GetComputingPartnerCodeTranslation(sii.UnloadPortCode,SIIRequestComputingPartner,ComputingPartnerTableUnloadingSiteType,_tenant)
                    },

                    //for now 0 until doucments is figured out
                    FormAttachmentIndex = 0
                };


                foreach (var requestItemKey in requestItemsKeys)
                {
                    var item = requestItemsService.GetSingle(requestItemKey.DeclarationId, requestItemKey.LineNumber, requestItemKey.SIIRequestID, requestItemKey.InvoiceCounterKey, requestItemKey.InvoiceItemLineNumber, true, false);
                    if (item == null)
                    {
                        throw new ArgumentException($"Request Item Linenumber {requestItemKey.LineNumber} for Declaration {requestItemKey.DeclarationId} not found in SII Request {siiRequestId}", nameof(requestItemKey));
                    }
                }


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
            ObjectTableRepository myObjectTabelRepository;
            ComputingPartnerQuery computingPartnerQuery;
            ComputingPartnerTranslationQuery computingPartnerTranslationQuery;
            context = CommonDataContext.GetContext(tenant);
            myObjectTabelRepository = new ObjectTableRepository(tenant);
            computingPartnerQuery = new ComputingPartnerQuery(new ComputingPartnerRepository(context));
            computingPartnerTranslationQuery = new ComputingPartnerTranslationQuery(new ComputingPartnerTranslationRepository(context));

            ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName(objectTableName, 0, true);
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



    }
    enum CountryCode
    {
        numeric = 1,
        alphaCode = 2,
    }
}
