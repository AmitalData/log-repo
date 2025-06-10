using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts.SIIRequest;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Server.Tools;
using Logitude.Server.Tools.RestRequestExecutor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mail;
using System.Windows.Forms;

namespace Logitude.Customs.BL.BL.SIIRequest
{
    public class SIIRequestApiDataMapper
    {
        int _tenant;
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

                var siiQueryService = new SIIRequestQueryService(context);
                var requestItemsQueryService = new SupplierInvoiceItemsReqListQueryService(context);
                var declarationQueryService = new DeclarationQueryService(context);
                var userQueryService = new UserQuery(_tenant);



                var form = new ReleaseRequestFormDto();

                var siiRequest = siiQueryService.GetSingle(siiRequestId, true, false);
                if (siiRequest == null)
                {
                    throw new ArgumentException($"SII Request with ID {siiRequestId} not found", nameof(siiRequestId));
                }
                var SIICompanyName = DefaultService.Instance.Get(_tenant, "SIICustomerUniqueCode", "SIICustomerUniqueCode")?.Value1;
                var maxIdNumber = siiQueryService.GetSIIFormApplicationMaxNumber(_tenant) + 1 ;
                form.FormApplicationId = SIICompanyName + "-" + maxIdNumber.ToString();

                var dec = declarationQueryService.GetDataForSIIRequest(siiRequest.DeclarationId, _tenant);
                if(dec != null)
                {
                    form.CustomsAgentRegisteredNumber = dec.AgentId;
                    form.AgentFileId = dec.CustomFileNo;
                    if (dec.ImporterId != null)
                    {
                        var importerPM = userQueryService.GetSinglePM(dec.ImporterId, _tenant);
                        if (importerPM != null)
                        {
                            form.ImporterEmail = importerPM.Email;
                            form.ImporterPhone = importerPM.BusinessPhone;
                            form.ImporterCellPhone = importerPM.BusinessPhone;
                            form.ImporterFax = importerPM.Fax;
                            form.ImporterNumber = importerPM.Id;
                        }
                    }
                    else
                    {
                        form.ImporterNumber = dec.ImporterCode;
                    }
                }
                
                form.DeliveryArrivalDate = siiRequest.UnloadDate;
                form.DeliveryComment = siiRequest.Remarks;
                form.ShipFlightNumber = siiRequest.VesselName;
                form.BillOfLadingId = siiRequest.ManifestNumber;
                form.ContactPersonFirstName = siiRequest.ContactName;
                form.ContactPersonLastName = siiRequest.ContactName;
                form.ContactPersonEmail = siiRequest.ContactEmail;
                form.ContactPersonPhone = siiRequest.ContactCellPhone;
                form.ContactPersonCellPhone = siiRequest.ContactCellPhone;
                form.ContactPersonFax = siiRequest.ContactFax;
                form.IsNumericCountryCode = CountryCode.alphaCode.ToString();





                foreach (var requestItemKey in requestItemsKeys)
                {
                    var item = requestItemsQueryService.GetSingle(requestItemKey.DeclarationId, requestItemKey.LineNumber, requestItemKey.SIIRequestID, requestItemKey.InvoiceCounterKey, requestItemKey.InvoiceItemLineNumber, true, false);
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
        
    }
    enum CountryCode
    {
        numeric = 1,
        alphaCode = 2,
    }
}
