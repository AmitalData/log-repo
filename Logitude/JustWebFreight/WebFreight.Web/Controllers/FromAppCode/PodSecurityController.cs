using Logitude.BL.ShipmentsModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.App_Code
{
    public class PodSecurityController : ApiController
    {
        public SuccessMobile GetCheckPODSecurityKeyValidation(string shipmentNumber, string securityKey, int tenant)
        {

            SuccessMobile successMobile = new SuccessMobile();
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            successMobile.IsScceed = shipmentQuery.CheckPODSecurityKeyValidation(shipmentNumber, securityKey, tenant);
            return successMobile;
        }
    }
}
