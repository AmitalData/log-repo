using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Web;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.DigitalPortal.Helpers
{
    public class DigitalPortalAuthenticationHelper
    {
        public Tuple<string, int> GetShipmentBySecurityKey(string securityKey)
        {
            var tenant = 0;
            var shipmentQuery = new ShipmentQuery(tenant);
            var shipmentIdAndTenant = shipmentQuery.GetShipmentIdBySecurityKey(securityKey);
            return shipmentIdAndTenant;
        }

        public Tuple<string, int, string> AuthenticateResponse(string cardId, string entityId, bool blockAccess = false, bool isEventOrDocument = false)
        {
            string securityKey = HttpContext.Current.Request.Headers["securitykey"];
            var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
            int tenant;

            if (securityKey != null && (!blockAccess || authToken != null || isEventOrDocument))
            {
                var shipmentIdAndTenant = GetShipmentBySecurityKey(securityKey);
                if (shipmentIdAndTenant == null)
                {
                    throw new AutenticationException("Sorry! this user is not authorized!");
                }

                entityId = shipmentIdAndTenant.Item1;
                tenant = shipmentIdAndTenant.Item2;

                if (isEventOrDocument && authToken == null)
                {
                    CheckAutenticationOfEventsOrDocuments(tenant);
                }

                return Tuple.Create(entityId, tenant, "");
            }
            else if (authToken != null)
            {
                tenant = authToken.Tenant;
                CheckSecurityByToken(tenant, cardId);
                return Tuple.Create(entityId, tenant, authToken.Email);
            }

            throw new AutenticationException("Sorry! this user is not authorized!");
        }

        private void CheckAutenticationOfEventsOrDocuments(int tenant)
        {
            var myTenantRepository = new TenantRepository(tenant);
            var myTenant = myTenantRepository.GetSingleTenant(tenant);
            if (!myTenant.DisplayDocumentsAndEvents)
            {
                throw new AutenticationException("Sorry! this user is not authorized!");
            }
        }

        public void CheckSecurityByToken(int tenant, string cardId)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckDigitalUserAuthentication(tenant, cardId);
        }
    }
}