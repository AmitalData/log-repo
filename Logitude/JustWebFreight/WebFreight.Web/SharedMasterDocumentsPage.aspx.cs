using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web;

namespace WebFreight.Web
{
    public partial class SharedMasterDocumentsPage : System.Web.UI.Page
    {
        public string CurrentEntityId;
        public int? Tenant = null;
        public List<Shipment> ConnectedHousesShipments = new List<Shipment>();
        public string Domain = "";
        public string MainColor = "#000000";
        public string SecondaryColor = "#000000";
        protected void Page_Load(object sender, EventArgs e)
        {
            string userdata = Request.QueryString["securitykey"];
            
            if (userdata == null)
                CheckIfUserAuthenticated();
            else {
                string absoluteUri = Request?.Url?.AbsoluteUri;
                SetSystemDomain(absoluteUri);
                InitPage(userdata); 
            }
        }

        private void SetSystemDomain(string absoluteUri)
        {
            string link = absoluteUri.ToLower();
            link = SplitString(link, "/sharedmasterdocumentspage", 0);
            link = SplitString(link, "https://", 1);
            link = SplitString(link, "http://", 1);
            link = RemovePortNumber(link);

            this.Domain = link;
        }

        private string RemovePortNumber(string domainLink)
        {
            string link = domainLink;
            if (link.IndexOf(":") != -1 && link.IndexOf("/") != -1)
            {
                string port = link.Substring(link.IndexOf(":"), link.IndexOf("/") - link.IndexOf(":"));
                link = link.Replace(port, "");
            }
            else if (link.IndexOf(":") != -1)
                link = SplitString(link, ":", 0);
            return link;
        }

        private string SplitString(string allString, string splitString, int index)
        {
            string myString = allString;
            if (myString.IndexOf(splitString) != -1)
                myString = myString.Split(new string[] { splitString }, StringSplitOptions.None)[index];
            return myString;
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
            SetCargoTrackingBrandingData();
            SetCurrentEntityVariables(linkParameters);
            SetAllConnectedHousesShipments();
        }

        private void SetCurrentEntityVariables(string[] linkParameters)
        {
            if (Tenant != null)
            {
                ShipmentQuery shipmentQuery = new ShipmentQuery((int)Tenant);

                ShipmentPM pm = shipmentQuery.GetSinglePMBySecurityKeyAndTenant(linkParameters[0], (int)Tenant);
                if (pm != null)
                {
                    CurrentEntityId = pm.Id;
                }
            }
        }

        private void SetCargoTrackingBrandingData()
        {
            TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(0);
            TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePMByDomain(this.Domain);
            if(tenantManagementPM == null)
            {
                tenantManagementPM = tenantManagementQuery.GetSinglePMByDomain(this.Domain + "/cargotracking");
            }
            if (tenantManagementPM != null)
            {
                this.Tenant = tenantManagementPM.Id;
                this.MainColor = tenantManagementPM.MainColor == null ? this.MainColor : tenantManagementPM.MainColor;// ConvertHexaToRGBA(tenantManagementPM.MainColor);
                this.SecondaryColor = tenantManagementPM.SecondaryColor == null ? this.SecondaryColor : tenantManagementPM.SecondaryColor;// ConvertHexaToRGBA(tenantManagementPM.SecondaryColor);
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

        private string ConvertHexaToRGBA(string hexString)
        {
            //replace # occurences
            if (hexString.IndexOf('#') != -1)
                hexString = hexString.Replace("#", "");

            int r, g, b = 0;
            double a = 0.0;
            if (hexString.Length > 7)
                a = int.Parse(hexString.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier) / 255.0;
            r = hexString.Length > 5 ? int.Parse(hexString.Substring(hexString.Length-6, 2), System.Globalization.NumberStyles.AllowHexSpecifier) : 0;
            g = hexString.Length > 3 ? int.Parse(hexString.Substring(hexString.Length-4, 2), System.Globalization.NumberStyles.AllowHexSpecifier) : 0;
            b = hexString.Length > 1 ? int.Parse(hexString.Substring(hexString.Length-2, 2), System.Globalization.NumberStyles.AllowHexSpecifier) : 0;

            return "rgb" + (a > 0 ? "a" : "") + "(" + r + "," + g + "," + b + (a > 0 ? "," + a : "")  + ")";
        }
    }
}