using Logitude.BL.CommonDataModel.EntityPMs;
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
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.ShardLogistics
{
    public class SharedLogisticContactController : ApiController
    {
        public HttpResponseMessage GetSharedLogisticContactsbyCardId(string cardId, int tenant)
        {

            try
            {
                Authentication();

                CardContactRepository cardContactRepository = new CardContactRepository(tenant);

                IQueryable<SharedLogisticContactPM> contacts = cardContactRepository.GetCardContacts(tenant).Where(c => c.CardId == cardId && c.ContactId == c.ContactId && c.Tenant == tenant).Select(a => new SharedLogisticContactPM
                {
                    Id = a.Id,
                    CardId = cardId,
                    ContactId = a.Contact.Id,
                    Name = a.Contact.EnglishName,
                    Email = a.Contact.Email,
                    InternetAccess = a.InternetAccess,
                    Tenant = a.Tenant,
                    EnglishName = a.Contact.EnglishName,
                    BusinessPhone = a.Contact.BusinessPhone,
                    Mobile = a.Contact.Mobile,
                    Fax = a.Contact.Fax,
                    LastLoginDate = a.LastLoginDate,
                });


                return Request.CreateResponse(HttpStatusCode.OK, contacts);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage PutContactInternetAccessInvitation(SharedLogisticContactPM sharedLogisticsContact)
        {

            try
            {
                Authentication();

                SharedLogisticContactHelper sharedLogisticContactHelper = new SharedLogisticContactHelper();
                sharedLogisticContactHelper.InternetAccessInvitation(sharedLogisticsContact, null);
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        
        private static void Authentication()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

        }

    }

}