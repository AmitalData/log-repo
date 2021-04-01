using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;


namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class CardExtendedController : ApiController
    {
        AuthenticationToken authToken;
        public HttpResponseMessage GetDisconnectGLAccountFromCard(string Id, string partnerTypeId,string eventTypeCode)
        {
            try
            {
                authToken = GetAuthenticationToken();
                CheckContactFeature(partnerTypeId);

                CardPM card = GetCardById(Id);
                if (card.GLAccountId != null)
                {
                    GLAccountPM glaccount = UpdateGLAccountFields(card);
                    SendHybridTask(glaccount);
                    UpdateCard(card);
                    CreateEvents(Id, eventTypeCode);
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Ok");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        string partnerObjectTableName;
        private void CheckContactFeature(string partnerTypeId)
        {

            partnerObjectTableName = GetPartnerObjectTableName(partnerTypeId);
            partnerObjectTableName = partnerObjectTableName.Replace(" ", String.Empty);
            CheckObjectTableContactFeature(partnerObjectTableName);
        }

        private void CheckObjectTableContactFeature(string tableName)
        {
            SecurityUtility.CheckContactFeature(tableName, "UPDATE", authToken.Tenant);
        }

        private string GetPartnerObjectTableName(string id)
        {
            PartnerTypeQuery partnerTypeQuery = new PartnerTypeQuery(authToken.Tenant);
            PartnerTypePM partnerType = partnerTypeQuery.GetSinglePM(id);
            if (partnerType != null)
            {
                return partnerType.Name;
            }

            else return null;
        }

        private CardPM GetCardById(string id)
        {
            CardQuery cardQuery = new CardQuery(authToken.Tenant);
            return cardQuery.GetSinglePM(id, authToken.Tenant);
        }
        private AuthenticationToken GetAuthenticationToken()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            return authToken;
        }

        private void UpdateCard(CardPM card)
        {
            card.GLAccountId = null;
            card.GLAccountDisplayNumber = null;
            ICommonDataContext commonContext = CommonDataContext.GetContext(authToken.Tenant);
            CardService cardService = new CardService(commonContext, authToken.Tenant);
            cardService.Update(card);

        }
        GLAccountPM glaccount;
        private GLAccountPM UpdateGLAccountFields(CardPM card)
        {
            glaccount = GetGLAccountById(card.GLAccountId);
            if (glaccount != null)
            {
              
                glaccount.PartnerTypeId = card.PartnerTypeId;
                glaccount.CardCode = card.Code;
            }
            return glaccount;
        }
        private void SendHybridTask(GLAccountPM gLAccount)
        {
            if (gLAccount != null)
            {
                IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
                GLAccountUpdateService service = new GLAccountUpdateService(MyContext, new Dictionary<string, IContext>(), authToken.Tenant);
                service.SendHybridTask(gLAccount);
            }

        }
        private GLAccountPM GetGLAccountById(string id)
        {
            GLAccountQueryService gLAccountQuery = new GLAccountQueryService(authToken.Tenant);
            return gLAccountQuery.GetSingle(id, false, false);
        }
        private void CreateEvents(string cardId, string eventTypeCode)
        {
            CreateTraceEvent(glaccount.Id, "GLAccount", "DIST");
            CreateTraceEvent(cardId, partnerObjectTableName, eventTypeCode);
        }
        private void CreateTraceEvent(string id,string objecttableName,  string eventTypeCode)
        {
            ContactRepository contactRep = new ContactRepository(authToken.Tenant);
            string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(authToken.Tenant);
            Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, authToken.Tenant);
        
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                EntityId = id,
                Tenant = authToken.Tenant,
                UserId = contact.Id,
                ObjectTableName = objecttableName,
                IsAddedManually = false,
                EventTypeCode = eventTypeCode,
                Notes = "Internal number: " + glaccount.InternalNumber+ "\n Local name: " + glaccount.LocalName,

            });
        }

        public HttpResponseMessage GetAllConnectedPartnersByGLAccountId(string glAccountId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("GLAccount", "READ", tenant);

                IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
                GLAccountConnectedPartnerService gLAccountConnectedPartnerService = new GLAccountConnectedPartnerService(tenant);
                List<ShortPartnersDetails> connectedPartners= gLAccountConnectedPartnerService.GetAllConnectedPartnersByGLAccountId(glAccountId);


                return Request.CreateResponse(HttpStatusCode.OK, connectedPartners);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}