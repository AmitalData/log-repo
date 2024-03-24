using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.WcfApi;

namespace WebFreight.Web.App_Code
{
    public class FollowedShipmentsController : ApiController
    {



        public bool PostInsertDeleteSharedFollowedShipment(int tenant, EntityTrackingFilter filters)
        {
            bool issuccess;
            SharedFollowedShipmentRepository sharedFollowedShipmentRepository = new SharedFollowedShipmentRepository(tenant);

            SharedFollowedShipment entity = null;

            if (filters.IsDelete)
            {
                if (!string.IsNullOrEmpty(filters.CustomFileId))
                {
                    entity = sharedFollowedShipmentRepository.GetSingleSharedFollowedShipmentByShipmentIdAndContactId(filters.CustomFileId, filters.ContactId, tenant);
                    if (entity != null) sharedFollowedShipmentRepository.Remove(entity);
                }
                entity = sharedFollowedShipmentRepository.GetSingleSharedFollowedShipmentByShipmentIdAndContactId(filters.ShipmentId, filters.ContactId, tenant);
                if (entity != null) sharedFollowedShipmentRepository.Remove(entity);

                issuccess = true;
            }

            else
            {
                if (filters.TrackDate != null && !string.IsNullOrEmpty(filters.ShipmentId) && !string.IsNullOrEmpty(filters.ContactId))
                {
                    if (!string.IsNullOrEmpty(filters.CustomFileId))
                    {
                        entity = new SharedFollowedShipment() { Id = Guid.NewGuid().ToString(), Tenant = tenant, ShipmentId = filters.CustomFileId, ContactId = filters.ContactId, TrackDate = filters.TrackDate };
                        sharedFollowedShipmentRepository.Add(entity);
                    }

                    entity = new SharedFollowedShipment() { Id = Guid.NewGuid().ToString(), Tenant = tenant, ShipmentId = filters.ShipmentId, ContactId = filters.ContactId, TrackDate = filters.TrackDate };
                    sharedFollowedShipmentRepository.Add(entity);
                    issuccess = true;
                }

                else issuccess = false;

            }

            sharedFollowedShipmentRepository.SubmitChanges();

            return issuccess;

        }



        public HttpResponseMessage GetShipmentFollowers(string shipmentNumber, int tenant)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                Authentication(tenant);
                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                string entityId = shipmentQuery.GetEntitiyIdByShipmentNumber(shipmentNumber, tenant);
                SharedFollowedShipmentRepository sharedFollowedShipmentRepository = new SharedFollowedShipmentRepository(tenant);
                List<string> contactIds = sharedFollowedShipmentRepository.GetContactsFollowedShipmentIdByShipmentId(entityId, tenant).ToList();
                ContactQuery contactQuery = new ContactQuery(tenant);
                List<ContactList> contactLists = contactQuery.GetContactListsByIds(contactIds, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, contactLists);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));

            }

        }


        private static void Authentication(int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);

        }


    }
}