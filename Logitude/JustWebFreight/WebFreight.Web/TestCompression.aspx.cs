using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace WebFreight.Web
{
    public partial class TestCompression : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string json = "{\"events\":{\"security_token\":\"598f975e3db27c19a3cc8f4b85f53e61e04c8389\",\"generated\":\"2016-03-13T10:10 0000\",\"event\":{\"id\":\"827078\",\"shipment_id\":\"159759\",\"details\":{\"message\":\"Status changed to: waiting for departure from POL\",\"new_status\":\"13\",\"new_status_verbose\":\"waiting for departure from POL\"},\"shipment\":\"http://capi.ocean-insights.com/containertracking/v1/shipments/159759/\",\"code\":\"0\",\"severity\":\"1\",\"walltime\":\"2016-03-13T10:10 0000\",\"created\":\"2016-03-13T10:10 0000\"},\"shipment\":{\"id\":\"159759\",\"shipmentsubscription_status_verbose\":\"new / initializing\",\"shipmentsubscription_status\":\"0\",\"shipmentsubscription_id\":\"142707\",\"status_verbose\":\"waiting for departure from POL\",\"current_vessel_nextport\":null,\"current_vessel_position\":null,\"current_vessel\":null,\"tsp3_loc\":null,\"tsp2_loc\":null,\"tsp1_loc\":null,\"leg4_vessel\":null,\"leg3_vessel\":null,\"leg2_vessel\":null,\"leg1_vessel\":{\"name\":\"MSC TOKYO\",\"imo\":\"9295361\"},\"dlv_loc\":{\"name\":\"Ashdod\",\"locode\":\"ILASH\"},\"pod_loc\":null,\"pol_loc\":{\"name\":\"Antwerp\",\"locode\":\"BEANR\"},\"origin_loc\":null,\"carrier_name\":\"MSC\",\"carrier_scac\":\"MSCU\",\"url\":\"http://capi.ocean-insights.com/containertracking/v1/shipments/159759/\",\"container_number\":\"TEXU7164896\",\"booking_number\":null,\"bl_number\":null,\"weight\":null,\"container_type_str\":\"40' DRY VAN\",\"container_type_iso\":null,\"status\":\"13\",\"lifecycle_status\":\"0\",\"id_date\":\"2016-02-26\",\"empty_pickup_planned_initial\":null,\"empty_pickup_planned_last\":null,\"empty_pickup_actual\":\"2016-02-26\",\"origin_pickup_planned_initial\":null,\"origin_pickup_planned_last\":null,\"origin_pickup_actual\":null,\"pol_arrival_planned_initial\":null,\"pol_arrival_planned_last\":null,\"pol_arrival_actual\":\"2016-02-29\",\"pol_loaded_planned_initial\":null,\"pol_loaded_planned_last\":null,\"pol_loaded_actual\":\"2016-03-09\",\"pol_vsldeparture_planned_initial\":null,\"pol_vsldeparture_planned_last\":null,\"pol_vsldeparture_actual\":null,\"pol_vsldeparture_detected\":null,\"leg1_voyage\":\"NI609A\",\"tsp1_vslarrival_detected\":null,\"tsp1_vslarrival_planned_initial\":null,\"tsp1_vslarrival_planned_last\":null,\"tsp1_vslarrival_actual\":null,\"tsp1_discharge_planned_initial\":null,\"tsp1_discharge_planned_last\":null,\"tsp1_discharge_actual\":null,\"tsp1_loaded_planned_initial\":null,\"tsp1_loaded_planned_last\":null,\"tsp1_loaded_actual\":null,\"tsp1_vsldeparture_planned_initial\":null,\"tsp1_vsldeparture_planned_last\":null,\"tsp1_vsldeparture_actual\":null,\"tsp1_vsldeparture_detected\":null,\"leg2_voyage\":null,\"tsp2_vslarrival_detected\":null,\"tsp2_vslarrival_planned_initial\":null,\"tsp2_vslarrival_planned_last\":null,\"tsp2_vslarrival_actual\":null,\"tsp2_discharge_planned_initial\":null,\"tsp2_discharge_planned_last\":null,\"tsp2_discharge_actual\":null,\"tsp2_loaded_planned_initial\":null,\"tsp2_loaded_planned_last\":null,\"tsp2_loaded_actual\":null,\"tsp2_vsldeparture_planned_initial\":null,\"tsp2_vsldeparture_planned_last\":null,\"tsp2_vsldeparture_actual\":null,\"tsp2_vsldeparture_detected\":null,\"leg3_voyage\":null,\"tsp3_vslarrival_detected\":null,\"tsp3_vslarrival_planned_initial\":null,\"tsp3_vslarrival_planned_last\":null,\"tsp3_vslarrival_actual\":null,\"tsp3_discharge_planned_initial\":null,\"tsp3_discharge_planned_last\":null,\"tsp3_discharge_actual\":null,\"tsp3_loaded_planned_initial\":null,\"tsp3_loaded_planned_last\":null,\"tsp3_loaded_actual\":null,\"tsp3_vsldeparture_planned_initial\":null,\"tsp3_vsldeparture_planned_last\":null,\"tsp3_vsldeparture_actual\":null,\"tsp3_vsldeparture_detected\":null,\"leg4_voyage\":null,\"pod_vslarrival_planned_initial\":\"2016-03-21\",\"pod_vslarrival_planned_last\":\"2016-03-21\",\"pod_vslarrival_actual\":null,\"pod_vslarrival_detected\":null,\"pod_discharge_planned_initial\":null,\"pod_discharge_planned_last\":null,\"pod_discharge_actual\":null,\"pod_departure_planned_initial\":null,\"pod_departure_planned_last\":null,\"pod_departure_actual\":null,\"dlv_delivery_planned_initial\":null,\"dlv_delivery_planned_last\":null,\"dlv_delivery_actual\":null,\"empty_return_planned_initial\":null,\"empty_return_planned_last\":null,\"empty_return_actual\":null,\"ts_count\":\"0\",\"last_carrier_update\":\"2016-03-13T10:10 0000\",\"last_actuals_update\":null,\"visible\":\"true\",\"created\":\"2016-03-13T10:10 0000\",\"modified\":\"2016-03-13T10:10 0000\",\"shipmentsubscription\":\"http://capi.ocean-insights.com/containertracking/v1/subscriptions/142707/\"}}}";
            
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(json);
            Response.End();
        }

        private static string GetTextFromXMLFile(string file)
        {
            StreamReader reader = new StreamReader(file);
            string ret = reader.ReadToEnd();
            reader.Close();
            return ret;
        }
    }
}