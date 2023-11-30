using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.CargoTracking.BL.EntityQueryServices;
using Logitude.SystemLogs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Linq;
using System.Web;


namespace WebFreight.Web.Monitoring
{
    public partial class CargoShipmentsWaiting : System.Web.UI.Page
    {
        int MaxShipmentsWaiting = 20000;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/xml";
            Response.Write("<pingdom_http_custom_check>");
            if (AnyFailedStatus())
            {
                Response.Write("<status>Fail</status>");
            }
            else
            {
                Response.Write("<status>OK</status>");
            }
            int ResponseTime_Millisecond = HttpContext.Current.Timestamp.Millisecond;
            String ResponseTime = "<response_time>" + ResponseTime_Millisecond + "</response_time>";
            Response.Write(ResponseTime);
            Response.Write("</pingdom_http_custom_check>");
            Response.End();
        }
        private bool AnyFailedStatus()
        {
            bool isReaches = false;
            var numberOfShipmentsWaiting = GetNumberOfShipmentsWaiting();
            if (numberOfShipmentsWaiting >= MaxShipmentsWaiting)
                isReaches = true;
            return isReaches;
        }

        private int GetNumberOfShipmentsWaiting()
        {
            var numberOfShipmentsWaiting = 0;
            CargoTrackingWatermarkQueryService cargoTrackingWatermarkQueryService = new CargoTrackingWatermarkQueryService(0);
            CargoTrackingIncrementalStatQueryService cargoTrackingIncrementalStatQueryService = new CargoTrackingIncrementalStatQueryService(0);
            var cargoTrackingWatermarks = cargoTrackingWatermarkQueryService.GetShipmentsWaterMarks();
            ShipmentQuery shipmentQuery = new ShipmentQuery(0);
            IQueryable<Shipment> Shipments = shipmentQuery.GetAllShipments();
            if (cargoTrackingWatermarks != null)
            {
                numberOfShipmentsWaiting = Shipments.Where(s => s.AutomaticLastUpdateDate > cargoTrackingWatermarks.LastUpdateDate).Count() ;
            }
            return numberOfShipmentsWaiting;
        }
    }
}