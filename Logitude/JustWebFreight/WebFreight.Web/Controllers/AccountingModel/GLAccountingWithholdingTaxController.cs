using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.AccountingModel
{
    public class GLAccountingWithholdingTaxController : ApiController
    {
        public HttpResponseMessage GetDeductionPercentage(string vendorId, string registerDate)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                decimal? percentage = null;

                CardRepository cardRep = new CardRepository(authToken.Tenant);
                Card card = cardRep.GetSingleCard(vendorId, authToken.Tenant);
                var glAccountId = card != null ? card.GLAccountId: null;
                GLAccountWithholdingTaxQueryService gLAccountWithholdingTaxQueryService = new GLAccountWithholdingTaxQueryService(authToken.Tenant);

                if (registerDate == "null" || registerDate == "undefined")
                    registerDate = null;

                GLAccountWithholdingTaxPM withholdingTaxPM= gLAccountWithholdingTaxQueryService.GetAccountWithholdingTaxPMByglAccountAndDate(glAccountId, DateHelper.GetDate(registerDate));

                var result = new GLAccountingWithholdingItem(); 

                if (withholdingTaxPM != null)
                {
                    percentage = withholdingTaxPM.Percentage;
                    result.Percentage = percentage;
                }
                else
                {
                    FullAccountingSettingQueryService settingService = new FullAccountingSettingQueryService(authToken.Tenant);
                    FullAccountingSettingPM setting = settingService.GetSingleFullAccountingSetting(authToken.Tenant);
                    percentage = setting.DefaultTaxWithholdPercentage;
                    if (percentage == null)
                    {
                        throw new ApplicationException(TranslateTextsClass.Translate("Accounting.O.MissingDefaultPercentage", authToken.Tenant));
                    }
                    else
                    {
                        result.Percentage = percentage;
                        result.IsDefault = true;
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }

    public class GLAccountingWithholdingItem
    {
        public decimal? Percentage { get; set; }
        public bool IsDefault { get; set; }
    }
}