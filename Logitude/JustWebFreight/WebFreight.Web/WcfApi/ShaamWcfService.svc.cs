using System;
using System.ServiceModel.Activation;
using WebFreight.Web.Security;
using Newtonsoft.Json;
using WebFreight.Web.Controllers.WebServices.Services;
using System.Net;

namespace WebFreight.Web.WcfApi
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ShaamWcfService : IShaamWcfService
    {
        AllocateInvoiceService allocateInvoiceService = new AllocateInvoiceService();

        public ApiToShaamRes CreateConfirmationNumber(string invoiceJson, int tenant)
        {
            ApiToShaamRes res;
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                HttpClienResponse apiToShaamRes = allocateInvoiceService.CreateConfirmationNumber(invoiceJson, tenant);

                if (apiToShaamRes.Res.StatusCode != HttpStatusCode.OK)
                {
                    res = new ApiToShaamRes()
                    {
                        approved = false,
                        errorCode = (int)ErrorCodeApiToShaamRes.GENERAL,
                        message = apiToShaamRes.Content,
                        status = 400
                    };
                }
                else
                    res = JsonConvert.DeserializeObject<ApiToShaamRes>(apiToShaamRes.Content);
            }
            catch (Exception ex)
            {
                res = new ApiToShaamRes()
                {
                    approved = false,
                    errorCode = (int)ErrorCodeApiToShaamRes.GENERAL,
                    message = ex.Message,
                    status = 400
                };
            }

            return res;
        }

    }
}
