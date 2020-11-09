using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Controllers.Integration.Factories;

namespace WebFreight.Web.Controllers.Integration.Shipment
{
    public class ShipmentIntegrationVariables
    {
        public TenantPM TenantPM { get; set; }
        public UserPM LoggedUserPM { get; set; }
        public List<IntegrationPartner> Partners { get; set; }
        public ShipmentIntegrationVariables()
        {
            this.Partners = new List<IntegrationPartner>();
        }
    }
}