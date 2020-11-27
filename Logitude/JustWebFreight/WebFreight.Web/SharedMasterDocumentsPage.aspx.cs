using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Web;

namespace WebFreight.Web
{
    public partial class SharedMasterDocumentsPage : System.Web.UI.Page
    {
        public string CurrentEntityId;
        public int? Tenant = null;
        public List<Shipment> ConnectedHousesShipments = new List<Shipment>();
        protected void Page_Load(object sender, EventArgs e)
        {
            string userdata = Request.QueryString["securitykey"];

            if (userdata == null)
                CheckIfUserAuthenticated();
            else
                InitPage(userdata);
        }

        private static void CheckIfUserAuthenticated()
        {
            if (!HttpContext.Current.Request.IsAuthenticated)
            {
                HttpContext.Current.Response.Redirect("login.aspx");
            }
        }

        private void InitPage(string userdata)
        {
            string[] linkParameters = userdata?.Split(':');
            if (linkParameters.Length > 0)
            {
                HandlePage(linkParameters);
            }
        }

        private void HandlePage(string[] linkParameters)
        {
            SetCurrentEntityVariables(linkParameters);
            SetAllConnectedHousesShipments();
        }

        private void SetCurrentEntityVariables(string[] linkParameters)
        {
            CurrentEntityId = linkParameters[1];
            if (linkParameters[2] != null)
            {
                Tenant = Int32.Parse(linkParameters[2]);
            }
        }

        private void SetAllConnectedHousesShipments()
        {
            if (Tenant != null)
            {
                IShipmentsContext myContext = ShipmentsContext.GetContext((int)Tenant);
                ShipmentConsoleShipmentQuery shipmentConsoleShipmentQuery = new ShipmentConsoleShipmentQuery(myContext);
                List<Shipment> connectedHousesShipments = shipmentConsoleShipmentQuery.GetMasterConnectedHouseShipments(CurrentEntityId, (int)Tenant);
                if (connectedHousesShipments != null) ConnectedHousesShipments = connectedHousesShipments;
            }
        }
    }
}