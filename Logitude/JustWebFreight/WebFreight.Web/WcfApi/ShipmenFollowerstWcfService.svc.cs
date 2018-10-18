using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ShipmenFollowerstWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ShipmenFollowerstWcfService.svc or ShipmenFollowerstWcfService.svc.cs at the Solution Explorer and start debugging.

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ShipmenFollowerstWcfService : IShipmenFollowerstWcfService
    {
        public List<ContactList> GetShipmentFollowersByShipmentNumber(string shipmentNumber, int tenant, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                string entityId = shipmentQuery.GetEntitiyIdByShipmentNumber(shipmentNumber, tenant);
                SharedFollowedShipmentRepository sharedFollowedShipmentRepository = new SharedFollowedShipmentRepository(tenant);
                List<string> contactIds = sharedFollowedShipmentRepository.GetContactsFollowedShipmentIdByShipmentId(entityId, tenant).ToList();
                ContactQuery contactQuery = new ContactQuery(tenant);
                List<ContactList> contactLists = contactQuery.GetContactListsByIds(contactIds, tenant);
                return contactLists;
            }
            catch (Exception ex)
            {
                response = new Response();
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }

        }









    }
}
