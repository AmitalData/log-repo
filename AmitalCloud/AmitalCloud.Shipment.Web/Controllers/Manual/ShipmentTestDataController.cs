using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
                IGlobalContext globalContext = GlobalContext.GetContext();

                //GlobalTenant otherGlobalTenant = (from a in globalContext.GlobalTenants where a.Id != tenant && a.Id != 0 && a.IsActive == true select a).FirstOrDefault();
                //Tenant otherTenant = (from a in commonContext.Tenants where a.Id == otherGlobalTenant.Id select a).FirstOrDefault();
                IShipmentsContext otherTenantShipmentsContext = ShipmentsContext.GetContext(tenant);

                ShipmentTestData shipmentTenantFields = new ShipmentTestData();
                shipmentTenantFields.CountryId = (from a in commonContext.Countries where a.Tenant == tenant && a.InActive == false select a.Id).FirstOrDefault();
                shipmentTenantFields.Port1Id = (from a in commonContext.Ports where a.Tenant == tenant && a.InActive == false select a.Id).FirstOrDefault();
                shipmentTenantFields.Port1CountryId = (from a in commonContext.Ports where a.Tenant == tenant && a.Id == shipmentTenantFields.Port1Id select a.CountryId).FirstOrDefault();
                shipmentTenantFields.Port2Id = (from a in commonContext.Ports where a.Tenant == tenant && a.Id != shipmentTenantFields.Port1Id && a.InActive == false select a.Id).FirstOrDefault();
                shipmentTenantFields.Port2CountryId = (from a in commonContext.Ports where a.Tenant == tenant && a.Id == shipmentTenantFields.Port2Id select a.CountryId).FirstOrDefault();
                shipmentTenantFields.CurrencyId = (from a in commonContext.Currencies where a.Tenant == tenant && a.InActive == false select a.Id).FirstOrDefault();
                shipmentTenantFields.UserId = (from a in commonContext.Users where a.Tenant == tenant select a.Id).FirstOrDefault();
                shipmentTenantFields.BranchId = (from a in commonContext.Branches where a.Tenant == tenant && a.InActive == false select a.Id).FirstOrDefault();
                shipmentTenantFields.DepartmentId = (from a in commonContext.Departments where a.Tenant == tenant && a.InActive == false select a.Id).FirstOrDefault();
                shipmentTenantFields.IncotermId = (from a in commonContext.Incoterms where a.Tenant == tenant && a.InActive == false select a.Id).FirstOrDefault();

                Card ShipperCard = (from a in commonContext.Cards
                                    where a.Tenant == tenant && a.PartnerTypeId == "CS" && a.InActive == false
      && (from b in commonContext.Addresses where b.Tenant == tenant && b.CardId == a.Id && b.AddressTypeId.ToUpper() == "M" select b).FirstOrDefault() != null
                                    select a).FirstOrDefault();
                Address ShipperCardAddress = (from a in commonContext.Addresses where a.Tenant == tenant && a.CardId == ShipperCard.Id && a.AddressTypeId.ToUpper() == "M" select a).FirstOrDefault();

                shipmentTenantFields.ShipperId = ShipperCard.Id;
                shipmentTenantFields.ShipperAddressId = ShipperCardAddress.Id;
                shipmentTenantFields.CustomerId = ShipperCard.Id;
                shipmentTenantFields.CustomerAddressId = ShipperCardAddress.Id;


                Card ConsigneeCard = (from a in commonContext.Cards
                                      where a.Tenant == tenant && a.PartnerTypeId == "CS" && a.Id != ShipperCard.Id && a.InActive == false
        && (from b in commonContext.Addresses where b.Tenant == tenant && b.CardId == a.Id && b.AddressTypeId.ToUpper() == "M" select b).FirstOrDefault() != null
                                      select a).FirstOrDefault();
                Address ConsigneerCardAddress = (from a in commonContext.Addresses where a.Tenant == tenant && a.CardId == ConsigneeCard.Id && a.AddressTypeId.ToUpper() == "M" select a).FirstOrDefault();

                shipmentTenantFields.ConsigneeId = ConsigneeCard.Id;
                shipmentTenantFields.ConsigneeAddressId = ConsigneerCardAddress.Id;

                Card AgentCard = (from a in commonContext.Cards
                                  where a.Tenant == tenant && a.PartnerTypeId == "AG" && a.InActive == false
    && (from b in commonContext.Addresses where b.Tenant == tenant && b.CardId == a.Id && b.AddressTypeId.ToUpper() == "M" select b).FirstOrDefault() != null
                                  select a).FirstOrDefault();
                Address AgentCardAddress = (from a in commonContext.Addresses where a.Tenant == tenant && a.CardId == AgentCard.Id && a.AddressTypeId.ToUpper() == "M" select a).FirstOrDefault();

                shipmentTenantFields.AgentId = AgentCard.Id;
                shipmentTenantFields.AgentAddressId = AgentCardAddress.Id;



                Card Notify1Card = (from a in commonContext.Cards
                                    where a.Tenant == tenant && a.PartnerTypeId == "CS" && a.InActive == false
      && (from b in commonContext.Addresses where b.Tenant == tenant && b.CardId == a.Id && b.AddressTypeId.ToUpper() == "M" select b).FirstOrDefault() != null
                                    select a).OrderBy(r => Guid.NewGuid()).FirstOrDefault();
                Address Notify1CardAddress = (from a in commonContext.Addresses where a.Tenant == tenant && a.CardId == Notify1Card.Id && a.AddressTypeId.ToUpper() == "M" select a).FirstOrDefault();

                shipmentTenantFields.Notify1Id = Notify1Card.Id;
                shipmentTenantFields.Notify1AddressId = Notify1CardAddress.Id;


                Card AirlineCard = (from a in commonContext.Cards where a.Tenant == tenant && a.PartnerTypeId == "AL" && a.InActive == false select a).FirstOrDefault();
                shipmentTenantFields.AirlineId = AirlineCard.Id;

                Card TruckerCard = (from a in commonContext.Cards where a.Tenant == tenant && a.PartnerTypeId == "TR" && a.InActive == false select a).FirstOrDefault();
                shipmentTenantFields.TruckerId = TruckerCard.Id;

                Card ShippingLineIdCard = (from a in commonContext.Cards where a.Tenant == tenant && a.PartnerTypeId == "SL" && a.InActive == false select a).FirstOrDefault();
                shipmentTenantFields.ShippingLineId = ShippingLineIdCard.Id;

                shipmentTenantFields.AirChargesTypeId = (from a in commonContext.ChargesTypes where a.Tenant == tenant && a.IsAir == true && a.InActive == false select a.Id).FirstOrDefault();
                shipmentTenantFields.OceanChargesTypeId = (from a in commonContext.ChargesTypes where a.Tenant == tenant && a.IsOcean == true && a.InActive == false select a.Id).FirstOrDefault();
                shipmentTenantFields.InlandChargesTypeId = (from a in commonContext.ChargesTypes where a.Tenant == tenant && a.IsInland == true && a.InActive == false select a.Id).FirstOrDefault();

                shipmentTenantFields.OtherTenantShipmentId = (from a in otherTenantShipmentsContext.Shipments where a.Tenant != tenant && a.Tenant != 0 select a.Id).FirstOrDefault();


                return Request.CreateResponse(HttpStatusCode.OK, shipmentTenantFields);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


    }


}