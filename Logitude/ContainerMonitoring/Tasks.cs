using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Collections;
//using Unifreight.Utlity;
using System.Data.SqlClient;
using System.Data;
using HttpUtils;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Net;

//using System.Windows.Forms;


namespace Unifreight.ContainerTasks
{
    public class ContainerTasks : IActivateOperation, IDisposable
    {
        string _token = "e2ad6f1dafbdd3dbdf9f6cb8f684d7cae1ff4306";
        string baseEndPoint = @"https://capi.ocean-insights.com/containertracking/v1/";
        public ContainerTasks(bool useOIV2)
        {
            if (useOIV2)
            {
                baseEndPoint = @"https://capi.ocean-insights.com/containertracking/v2/";
            }
        }
        public void ActivateOperation(string operation, ref Hashtable hash_data_in, ref object obj_prj_inner_data, out string data_out, out string status, out string err_message)
        {
            data_out = "";
            status = "";
            err_message = "";
            ////Hashtable hash_data_out = null;
            //string containerNo = UnifreightLists.GetValue(ref hash_data_in, "CONTAINER_NO");
            //string carrierSCAC = UnifreightLists.GetValue(ref hash_data_in, "CARRIER_SCAC");  //Standard Carrier Alpha Code (SCAC) 
            //string reqId = UnifreightLists.GetValue(ref hash_data_in, "REQ_ID");
            //string token = UnifreightLists.GetValue(ref hash_data_in, "TOKEN");
            //string username = UnifreightLists.GetValue(ref hash_data_in, "USERNAME");
            //string password = UnifreightLists.GetValue(ref hash_data_in, "PASSWORD");
            //string v_result = "";
            //string more = UnifreightLists.GetValue(ref hash_data_in, "MORE");
            //string properties = UnifreightLists.GetValue(ref hash_data_in, "PROPERTIES");
            //if (operation != "") operation = operation.ToUpper();
            //Logger.LogMe("Project: ContainerMonitor" + Environment.NewLine + "Operation: " + operation + Environment.NewLine +
            //"Container: " + containerNo + ", Carrier: " + carrierSCAC + " ,ReqID: " + reqId + " ,Token: " + token, false);
            //switch (operation)
            //{
            //    case "GETCARRIERLIST":
            //        GetCarrierList(token,out v_result, out status, out err_message);
            //        //GetSqlQueryResult(v_database, v_catalog, v_username, v_password, v_server, v_sql_query, more, out v_sql_result, out status, out err_message);
            //        data_out = v_result;
            //        break;
            //    case "STARTMONITOR":
            //        StartMonitor(carrierSCAC,containerNo,token, out v_result, out status, out err_message);
            //        data_out = v_result;
            //        break;
            //    case "ENDTMONITOR":
            //        EndMonitor(reqId, token, out v_result, out status, out err_message);
            //        data_out = v_result;
            //        break;
            //    case "GETSTATUS":
            //        GetStatus(reqId, token, out v_result, out status, out err_message);
            //        data_out = v_result;
            //        break;
            //    default:
            //        err_message = "Project PrinterTasks: There  is no definition form Operation '" + operation + "'";
            //        status = "-1";
            //        break;
            //}
        }

        public void GetStatus(string reqId, string token, out string v_result, out string status, out string err_message,string Type)
        {
            status = "0";
            err_message = "";
            v_result = "";
            string stage = "";
            //string endPoint = @"http://capi.ocean-insights.com/containertracking/v1/subscriptions/";
            string endPoint = baseEndPoint + (Type == "c_id" ? @"subscriptions/" : @"shipments/") + reqId + "/";
            try
            {
                if (string.IsNullOrEmpty(reqId) || string.IsNullOrWhiteSpace(reqId))
                {
                    err_message = "Parameter 'reqId' is missing !!!";
                    status = "-1";
                    return;
                }
                if (string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token))
                {
                    err_message = "Parameter 'Token' is missing !!!";
                    status = "-1";
                    return;
                }
                stage = "HTTP Request";

                var client = new RestClient(endPoint);
                var json = client.MakeRequest("", token);
                // To convert JSON text contained in string json into an XML node
                stage = "Deserialize Json Response";
                XmlDocument doc = JsonConvert.DeserializeXmlNode("{\"container\":" + json, "Root");
                stage = "Deserialize Xml Response";
                v_result = System.Xml.Linq.XElement.Parse(doc.OuterXml).ToString();
            }
            //catch (WebException ex)
            //{
            //    if (ex.Message.Contains("404"))
            //        err_message = "The is not subsctiption for Request Id '" + reqId + "' on the Container Tracking Server" + Environment.NewLine +
            //            "Stage: " + stage + Environment.NewLine + ex.ToString();
            //    else
            //        err_message = "Failed to Get Container Status" + Environment.NewLine + "Stage: " + stage + Environment.NewLine + ex.ToString();
            //    status = "-1";
            //}
            catch (Exception ex)
            {
                if (ex.Message.Contains("404"))
                    err_message = "There is not subsctiption for Request Id '" + reqId + "' on the Container Tracking Server" + Environment.NewLine +
                        "Stage: " + stage + Environment.NewLine + ex.ToString();
                else
                    err_message = "Failed to Get Container Status" + Environment.NewLine + "Stage: " + stage + Environment.NewLine + ex.ToString();
                status = "-1";
            }
        }

        private void EndMonitor(string reqId, string token, out string v_result, out string status, out string err_message)
        {
            v_result = "";
            status = "";
            err_message = "";
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                err_message = "To Be Implemented !!!" + Environment.NewLine + ex.ToString();
            }

        }

        public void StartMonitor(string carrierSCAC, string containerNo, string token, out string v_result, out string status, out string err_message, bool isCarrier = true)
        {
            status = "0";
            err_message = "";
            v_result = "";
            string stage = "";
            //string endPoint = @"http://capi.ocean-insights.com/containertracking/v1/subscriptions/";
            string endPoint = baseEndPoint + @"subscriptions/";
            try
            {
                if (string.IsNullOrEmpty(carrierSCAC) || string.IsNullOrWhiteSpace(carrierSCAC))
                {
                    err_message = "Parameter 'CarrierSCAC' (Standard Carrier Alpha Code) is missing !!!";
                    status = "-1";
                    return;
                }
                if (string.IsNullOrEmpty(containerNo) || string.IsNullOrWhiteSpace(containerNo))
                {
                    err_message = "Parameter 'ContainerNo' is missing !!!";
                    status = "-1";
                    return;
                }
                if (string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token))
                {
                    err_message = "Parameter 'Token' is missing !!!";
                    status = "-1";
                    return;
                }
                stage = "HTTP Request";
                string postData = "";
                if (isCarrier)
                {
                    postData = jSonTest(containerNo, carrierSCAC,true);
                }
                else
                {
                    postData = jSonTest(containerNo, carrierSCAC,false);
                }
                
                var client = new RestClient(endPoint, HttpVerb.POST, postData);
                var json = client.MakeRequest("", token);//YMLU8718132
                stage = "Deserialize Json Response";
                XmlDocument doc = JsonConvert.DeserializeXmlNode("{\"container\":" + json, "Root");
                //var id = doc.SelectNodes("//id");
                //if (id != null)
                //{
                //    if (id.Count > 0)
                //    {
                //        var request_id = id[0].InnerText;
                //        txtRequestId.Text = request_id;
                //    }
                //}
                stage = "Deserialize Xml Response";
                v_result = System.Xml.Linq.XElement.Parse(doc.OuterXml).ToString();
            }
            catch (WebException ex)
            {
                string message = "";
                var res = ((HttpWebResponse)ex.Response);
                if (res != null)
                {
                    message = res.StatusCode + ": " + res.StatusDescription;
                    message += Environment.NewLine +
                        new StreamReader(res.GetResponseStream()).ReadToEnd();
                }
                err_message = message;
                status = "-1";

            }
            catch (Exception ex)
            {
                err_message = "Failed to Start Monitor" + Environment.NewLine + "Stage: " + stage + Environment.NewLine + ex.ToString();
                status = "-1";
            }
        }

        public void StartMonitorForExistedRequest(string containerNo, string token, out string v_result, out string status, out string err_message)
        { 
            status = "0";
            err_message = "";
            v_result = "";
            string stage = "";
            //string endPoint = @"http://capi.ocean-insights.com/containertracking/v1/subscriptions/?search=<CONTAINER_NUMBER>";
            string endPoint = baseEndPoint + @"subscriptions/";
            try
            {
                //if (string.IsNullOrEmpty(carrierSCAC) || string.IsNullOrWhiteSpace(carrierSCAC))
                //{
                //    err_message = "Parameter 'CarrierSCAC' (Standard Carrier Alpha Code) is missing !!!";
                //    status = "-1";
                //    return;
                //}
                if (string.IsNullOrEmpty(containerNo) || string.IsNullOrWhiteSpace(containerNo))
                {
                    err_message = "Parameter 'ContainerNo' is missing !!!";
                    status = "-1";
                    return;
                }
                if (string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token))
                {
                    err_message = "Parameter 'Token' is missing !!!";
                    status = "-1";
                    return;
                }
                stage = "HTTP Request";
                //string postData = jSonTest(containerNo, carrierSCAC);
                var client = new RestClient(endPoint);//, HttpVerb.POST, postData);
                var json = client.MakeRequest("?search=" + containerNo, token);//YMLU8718132
                stage = "Deserialize Json Response";
                XmlDocument doc = JsonConvert.DeserializeXmlNode("{\"container\":" + json, "Root");
                //var id = doc.SelectNodes("//id");
                //if (id != null)
                //{
                //    if (id.Count > 0)
                //    {
                //        var request_id = id[0].InnerText;
                //        txtRequestId.Text = request_id;
                //    }
                //}
                stage = "Deserialize Xml Response";
                v_result = System.Xml.Linq.XElement.Parse(doc.OuterXml).ToString();
            }
            catch (WebException ex)
            {
                string message = "";
                var res = ((HttpWebResponse)ex.Response);
                if (res != null)
                {
                    message = res.StatusCode + ": " + res.StatusDescription;
                    message += Environment.NewLine +
                        new StreamReader(res.GetResponseStream()).ReadToEnd();
                }
                err_message = message;
                status = "-1";
            }
            catch (Exception ex)
            {
                err_message = "Failed to Start Monitor" + Environment.NewLine + "Stage: " + stage + Environment.NewLine + ex.ToString();
                status = "-1";
            }
        }

        private void GetCarrierList(string token,out string v_result, out string status, out string err_message)
        {
            status = "0";
            err_message = "";
            v_result = "";
            string stage = "";
            try
            {
                stage = "HTTP Request";
                //string endPoint = @"http://capi.ocean-insights.com/containertracking/v1/carriers/";
                string endPoint = baseEndPoint + @"carriers/";
                var client = new RestClient(endPoint);
                var json = client.MakeRequest(token);
                stage = "Deserialize Json Response";
                var serializer = new JavaScriptSerializer();
                var deserializedResult = serializer.Deserialize<List<Carrier>>(json);
                //foreach (var item in deserializedResult)
                    //cBoxCarrier.Items.Add(item);
                //cBoxCarrier.Items = deserializedResult;
                stage = "Deserialize Xml Response";
                XmlDocument doc = JsonConvert.DeserializeXmlNode("{\"carrier\":" + json, "Root");
                v_result = System.Xml.Linq.XElement.Parse(doc.OuterXml).ToString();
            }
            catch (Exception ex)
            {
                err_message = "Failed to get Carriers List" + Environment.NewLine + "Stage: " + stage + Environment.NewLine + ex.ToString();
                status = "-1";
            }

        }

        private string jSonTest(string continer, string carrier, bool isCarrier)
        {
            if (isCarrier)
                return ("{\"request_key\":\"" + continer + "\",\"request_carrier_code\":\"" + carrier + "\",\"request_type\":\"c_id\"}");
            else
                return ("{\"request_key\":\"" + continer + "\",\"request_carrier_code\":\"" + carrier + "\",\"request_type\":\"m_bl\"}");
            //return ("{\"request_key\":\"" + continer + "\",\"request_carrier_code\":\"" + carrier + "\",\"request_type\":\"c_id\"}");
        }


        private string ErrorMessage(Exception ex)
        {
            string errmessage = "";
            //errmessage = "[printer '" + m_printername + "']: " + ex.Message;
            if (ex.InnerException != null)
                errmessage += "\n" + ex.InnerException.Message;
            return (errmessage);
        }
        private string DecodeBase64(string base64Text)
        {
            if (string.IsNullOrEmpty(base64Text))
                return (String.Empty);
            string txt = base64Text.ToUpper();
            if (txt.Contains("UPDATE") || txt.Contains("INSERT") || txt.Contains("SELECT") || txt.Contains("FROM") || txt.Contains("TABLE") || txt.Contains("WHERE"))
                return (base64Text);
            try
            {
                byte[] data = Convert.FromBase64String(base64Text);
                //string decodedString = Encoding.UTF8.GetString(data);
                string decodedString =  Encoding.GetEncoding("windows-1255").GetString(data);
                return (decodedString);
            }
            catch (Exception ex)
            {
                return (base64Text);
            }
        }

        public Hashtable Deserialize(string xml)
        {
            return new Hashtable();
            //return UnifreightLists.Deserialize(xml);
        }

        public string GetValue(ref Hashtable hash_data_in, string key)
        {
            return "";
            //return UnifreightLists.GetValue(ref hash_data_in, key);
        }
        

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                /*if (pi != null)
                {
                    pi.Dispose();
                }*/
            }
            catch (Exception ex)
            {
            }
        }

        #endregion
    }
    public interface IActivateOperation
    {
        void ActivateOperation(string operation, ref Hashtable data_in, ref object obj_prj_inner_data, out string data_out, out string status, out string err_message);
    }
}
