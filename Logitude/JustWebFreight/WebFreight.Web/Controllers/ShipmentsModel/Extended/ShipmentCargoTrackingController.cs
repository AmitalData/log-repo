using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Zip;
using Logitude.BL.CommonDataModel.DataContracts;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Xml;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.ShipmentsModel.Extended
{
    public class ShipmentCargoTrackingController : ApiController
    {

        public HttpResponseMessage GetSingle(string id)
        {
            try
            {
                int tenant = GetAuthinticatedTenant();

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                ShipmentPM shipmentPM = shipmentQuery.GetShipmentPMForCargoTrackingByEntityId(id, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, shipmentPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetShipmentCustomsData(string shipmentId)
        {
            try
            {
                int tenant = GetAuthinticatedTenant();

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                CargoTrackingShipmentCustomsData customsData = shipmentQuery.GetCargoTrackingShipmentCustomsData(shipmentId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, customsData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetPartnersAddresses([FromUri] List<string> partnersIds)
        {
            try
            {
                int tenant = GetAuthinticatedTenant();


                AddressQuery addressQuery = new AddressQuery(tenant);
                List<AddressList> addresses = addressQuery.GetAddressesByCardIds(partnersIds, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, addresses);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage GetDocumentsFilingsConnectedToShipment(string id)
        {
            try
            {
                int tenant = GetAuthinticatedTenant();
                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
                List<DocumentsFilingPM> documentsFilingPM = documentsFilingQuery.GetInputDocumentsFilingPMsByEntityId(id, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, documentsFilingPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage GetShipmentPackages(string id)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                int tenant = GetAuthinticatedTenant();

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                List<ShipmentPackagePM> shipmentPM = shipmentQuery.GetPackagesOfShipment(tenant, id);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, shipmentPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private int GetAuthinticatedTenant()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            var tenant = authToken.Tenant;
            SecurityUtility.AuthenticationOnTenant(tenant);
            return tenant;
        }

    }
}