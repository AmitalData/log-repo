using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.ShipmentsModel.Extended
{
    public class ShipmentTestDataController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);


                ShipmentTestData shipmentTenantFields = new ShipmentTestData();
                shipmentTenantFields.CountryId = (from a in commonContext.Countries where a.Tenant == tenant && a.InActive == false select a.Id).FirstOrDefault();
                shipmentTenantFields.PortId = (from a in commonContext.Ports where a.Tenant == tenant && a.InActive == false select a.Id).FirstOrDefault();
                shipmentTenantFields.PortId2 = (from a in commonContext.Ports where a.Tenant == tenant && a.Id != shipmentTenantFields.PortId && a.InActive == false select a.Id).FirstOrDefault();
                shipmentTenantFields.CurrencyId = (from a in commonContext.Currencies where a.Tenant == tenant && a.InActive == false select a.Id).FirstOrDefault();
                shipmentTenantFields.UserId = (from a in commonContext.Users where a.Tenant == tenant select a.Id).FirstOrDefault();
                shipmentTenantFields.BranchId = (from a in commonContext.Branches where a.Tenant == tenant && a.InActive == false select a.Id).FirstOrDefault();
                shipmentTenantFields.DepartmentId = (from a in commonContext.Departments where a.Tenant == tenant && a.InActive == false select a.Id).FirstOrDefault();

                Card ShipperCard = (from a in commonContext.Cards
                                    where a.Tenant == tenant && a.PartnerTypeId == "CS" && a.InActive == false
      && (from b in commonContext.Addresses where b.Tenant == tenant && b.CardId == a.Id && b.AddressTypeId.ToUpper() == "M" select b).FirstOrDefault() != null
                                    select a).FirstOrDefault();
                Address ShipperCardAddress = (from a in commonContext.Addresses where a.Tenant == tenant && a.CardId == ShipperCard.Id && a.AddressTypeId.ToUpper() == "M" select a).FirstOrDefault();

                shipmentTenantFields.ShipperId = ShipperCard.Id;
                shipmentTenantFields.ShipperAddressId = ShipperCardAddress.Id;
                shipmentTenantFields.CustomerId = ShipperCard.Id;
                shipmentTenantFields.CustomerAddressId = ShipperCardAddress.Id;


                return Request.CreateResponse(HttpStatusCode.OK, shipmentTenantFields);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


    }



}