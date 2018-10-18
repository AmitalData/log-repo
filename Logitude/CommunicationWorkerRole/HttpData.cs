using Logitude.Server.Tools.Counters;
using Microsoft.VisualBasic;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Linq;

namespace CommunicationWorkerRole
{
   public class HttpData
    {

       public string txtURL = "https://start.exactonline.nl";
       public string cmbDivision = "475134";
       public string appKey = "{86ce9ca5-65aa-49ba-9b17-ca070ef8e39e}";
       int tenant = 1;
       private System.Net.CookieContainer CookieContainer = new System.Net.CookieContainer();

       private enum MessageType
       {
           Error = 0,
           Warning ,
           Message ,
           Fatal
       }

       private ProxyServerData _ProxyServer = new ProxyServerData();
       public ProxyServerData ProxyServer
       {
           get { return _ProxyServer; }
           set { _ProxyServer = value; }
       }

       private CredentialData _Credentials = new CredentialData();
       public CredentialData Credentials
       {
           get { return _Credentials; }
           set { _Credentials = value; }
       }

       private System.Collections.Generic.List<AdministrationData> Administrations = new System.Collections.Generic.List<AdministrationData>();
       public enum Directions
       {
           Export = 0,
           Import = 1
       }

       //private EOLTopics _topicXML = new EOLTopics();

       //private EOLTopics TopicXML
       //{
       //    get
       //    {
       //        if (_topicXML == null)
       //        {
       //            XmlSerializer xs = new XmlSerializer(typeof(EOLTopics));
       //            string sURL = Convert.ToString((txtURL.Substring(txtURL.Length-1) != "/" ? txtURL + "/" : txtURL));

       //            if (sURL.Length > 0 && cmbDivision != null)
       //            {
       //                try
       //                {

       //                    if (SetAdministration(sURL, cmbDivision))
       //                    {
       //                        //This aspx will return all available topics and their import/export parameters.
       //                        //It will also return possible values for combo boxes, checkbox lists and radio buttons.
       //                        string sURLTopics = sURL + "Docs/XMLTopicParameters.aspx";

       //                        HttpWebRequest request = _CreateWebRequest(Directions.Export, sURLTopics);
       //                        if ((request != null))
       //                        {
       //                            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
       //                            {
       //                                StreamReader readStream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
       //                                _topicXML = (EOLTopics)xs.Deserialize(readStream);
       //                            }
       //                        }
       //                    }
       //                }
       //                catch (Exception ex)
       //                {
       //                    throw new Exception(ex.Message);
       //                }
       //            }
       //        }
       //        return _topicXML;
       //    }
       //}

       //private ArrayList _Topics = new ArrayList();
       //public string[] Topics
       //{
       //    get
       //    {
       //        if (_Topics.Count == 0 && TopicXML != null)
       //        {
       //            foreach (Topic t in TopicXML.Topics)
       //            {
                       
       //                _Topics.Add(t.Code);
       //            }
       //        }
       //        return (string[])_Topics.ToArray(typeof(string));
       //    }
       //}

        private bool _CredentialsChanged = false;
        public bool CredentialsChanged
        {
            get { return _CredentialsChanged; }
            set
            {
                _CredentialsChanged = value;
                if (_CredentialsChanged)
                {
                    //_topicXML = null;
                    //_Topics.Clear();
                }
            }
        }

        public string GetTimeStamp(Stream stream)
        {
            XPathDocument xml = new XPathDocument(stream);
            XPathNavigator nav = xml.CreateNavigator();
            return Convert.ToString(nav.Evaluate("string(/eExact/Topics/Topic/@ts_d)"));
        }

        public int GetResultCount(Stream stream)
        {
            XPathDocument xml = new XPathDocument(stream);
            XPathNavigator nav = xml.CreateNavigator();
            string sCount = Convert.ToString(nav.Evaluate("string(/eExact/Topics/Topic/@count)"));
            if (sCount.Length > 0)
            {
                return Convert.ToInt32(sCount);
            }
            else
            {
                return 0;
            }
        }


        public int GetPageSize(Stream stream)
        {
            XPathDocument xml = new XPathDocument(stream);
            XPathNavigator nav = xml.CreateNavigator();
            string sPageSize = Convert.ToString(nav.Evaluate("string(/eExact/Topics/Topic/@pagesize)"));
            if (sPageSize.Length > 0)
            {
                return Convert.ToInt32(sPageSize);
            }
            else
            {
                return 0;
            }
        }

        public bool GetMessages(Stream stream,  string sMessages)
        {
            XPathDocument xml = new XPathDocument(stream);
            XPathNavigator nav = xml.CreateNavigator();
            XPathNodeIterator Messages = nav.Select("/eExact/Messages/Message");
            string sType = null;
            bool bErrorsFound = false;

            if (Messages.Count > 0)
            {
                while (Messages.MoveNext())
                {
                    nav = Messages.Current;
                    sType = nav.GetAttribute("type", string.Empty);

                    string sTopicNode = Convert.ToString(nav.Evaluate("string(Topic/@node)"));
                    string sKey = Convert.ToString(nav.Evaluate("string(Topic/Data/@key)"));
                    if (sKey.Length == 0)
                    {
                        sKey = Convert.ToString(nav.Evaluate("string(Topic/Data/@keyAlt)"));
                    }

                    //string msg = MessageType.Message.ToString();
                    //string warning = MessageType.Warning.ToString();
                    switch (sType)
                    {
                        case "2":
                            //2
                            break;
                        //Successful
                        case "1":
                            //1
                            sMessages = sMessages + "Warning: ";
                            break;
                        default:
                            //MessageType.Error (0) Or MessageType.Fatal (3)
                            sMessages = sMessages + "Error: ";
                            bErrorsFound = true;
                            break;
                    }

                    if ((sTopicNode + sKey).Length > 0)
                    {
                        sMessages = sMessages + sTopicNode + " " + sKey + ": " + Convert.ToString(nav.Evaluate("string(Description)")) + Environment.NewLine;
                    }
                    else
                    {
                        sMessages = sMessages + Convert.ToString(nav.Evaluate("string(Description)")) + Environment.NewLine;
                    }
                }
            }

            return bErrorsFound;
        }


        public HttpWebRequest CreateWebRequest(Directions Direction, string txtURL, string sDivision, string sTopic, string applicationKey, string sUploadFile = "", string sTimeStamp = "")
        {
            string sBaseURL = Convert.ToString((txtURL.Substring(txtURL.Length- 1) != "/" ? txtURL + "/" : txtURL));
            HttpWebRequest request = default(HttpWebRequest);
            string[] Params = null;

            //IMPORTANT:
            //First set the division to the selected division

            if (SetAdministration(txtURL, sDivision))
            {
                string sProcessURL = null;
                sBaseURL = Convert.ToString((txtURL.Substring(txtURL.Length- 1) != "/" ? txtURL + "/" : txtURL));
                switch (Direction)
                {
                    case Directions.Import:
                        sProcessURL = sBaseURL + "Docs/XMLUpload.aspx";
                     

                        break;
                    case Directions.Export:
                        sProcessURL = sBaseURL + "Docs/XMLDownload.aspx";
                    

                        break;
                }
                sProcessURL = sProcessURL + "?Topic=" + sTopic;
                sProcessURL = sProcessURL + "&ApplicationKey=" + applicationKey;

                if (Direction == Directions.Export)
                {
                    request = _CreateWebRequest(Directions.Export, sProcessURL);
                }
                //else
                //{
                //    request = _CreateWebRequest(Directions.Import, sProcessURL);
                //}
               

                if ((request != null))
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        XmlDocument doc = new XmlDocument();
                       // doc.LoadXml(response.GetResponseStream());

                        StreamReader readStream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                        doc.Load(readStream);
                        AnalyzeXMLDocument(doc);
                       // _topicXML = (EOLTopics)xs.Deserialize(readStream);
                    }
                }
               
                if (Direction == Directions.Export)
                {
                    sProcessURL = sProcessURL + "&output=1";
                    //Params = ExportParameters(sTopic);
                    //if ((Params != null))
                    //{
                    //    for (int i = 0; i <= Information.UBound(Params); i++)
                    //    {
                    //        if (Strings.Len(frmDemo.txtParameters(i).Text) > 0)
                    //        {
                    //            sProcessURL = sProcessURL + "&" + Params[i] + "=" + HttpUtility.UrlEncode(frmDemo.txtParameters(i).Text);
                    //        }
                    //        else if (Strings.Len(frmDemo.cmbParameters(i).SelectedValue) > 0)
                    //        {
                    //            sProcessURL = sProcessURL + "&" + Params[i] + "=" + HttpUtility.UrlEncode(Convert.ToString(frmDemo.cmbParameters(i).SelectedValue));
                    //        }
                    //        else if (frmDemo.clbParameters(i).CheckedItems.Count > 0)
                    //        {
                    //            string values = "";
                    //            CheckedListBox clb = frmDemo.clbParameters(i);
                    //            foreach (ParameterValue pv in clb.CheckedItems())
                    //            {
                    //                values += pv.value + ",";
                    //            }
                    //            if (Strings.Len(values) > 0)
                    //                values = Strings.Left(values, values.Length - 1);
                    //            sProcessURL = sProcessURL + "&" + Params[i] + "=" + HttpUtility.UrlEncode(values);
                    //        }
                    //    }
                    //}
                    if (sTimeStamp.Length > 0)
                    {
                        sProcessURL = sProcessURL + "&TSPaging=" + sTimeStamp;
                    }
                }
                else
                {
                  //  Params = ImportParameters(sTopic);
                    //if ((Params != null))
                    //{
                    //    for (int i = 0; i <= Information.UBound(Params); i++)
                    //    {
                    //        if (Strings.Len(frmDemo.txtParameters(i).Text) > 0)
                    //        {
                    //            sProcessURL = sProcessURL + "&" + Params[i] + "=" + HttpUtility.UrlEncode(frmDemo.txtParameters(i).Text);
                    //        }
                    //        else if (Strings.Len(frmDemo.cmbParameters(i).SelectedValue) > 0)
                    //        {
                    //            sProcessURL = sProcessURL + "&" + Params[i] + "=" + HttpUtility.UrlEncode(Convert.ToString(frmDemo.cmbParameters(i).SelectedValue));
                    //        }
                    //        else if (frmDemo.clbParameters(i).CheckedItems.Count > 0)
                    //        {
                    //            string values = "";
                    //            CheckedListBox clb = frmDemo.clbParameters(i);
                    //            foreach (ParameterValue pv in clb.CheckedItems())
                    //            {
                    //                values += pv.value + ",";
                    //            }
                    //            if (Strings.Len(values) > 0)
                    //                values = Strings.Left(values, Strings.Len(values) - 1);
                    //            sProcessURL = sProcessURL + "&" + Params[i] + "=" + HttpUtility.UrlEncode(values);
                    //        }
                    //    }
                    //}
                }

                request = _CreateWebRequest(Direction, sProcessURL, sUploadFile);
            }

            return request;
        }

        private void AnalyzeXMLDocument(XmlDocument doc)
        {
            ExternalSystemsTablesCodeRepository externalRep = new ExternalSystemsTablesCodeRepository(tenant);
            string type = doc.DocumentElement.FirstChild.FirstChild.Name;
            //  XmlNodeList nodes = doc.SelectNodes("Account");
            XmlNodeList nodes = doc.GetElementsByTagName(type);

            List<ExternalSystemsTablesCode> externalTables = externalRep.GetExternalSystemsTablesCodes(tenant).ToList();
            string logitudeTableName = null;
            switch (type)
            {
                case "Account": { logitudeTableName = "Card"; break; }
                case "Item": { logitudeTableName = "ChargesType"; break; }
                case "PaymentCondition": { logitudeTableName = "PaymentTerm"; break; }
                case "VAT": { logitudeTableName = "VatType"; break; }
                default: { break; }
            }

            foreach (XmlNode node in nodes)
            {
                string id = null;
                XmlNode name = null;
                if (type == "Account")
                {
                    id = node.Attributes["ID"].Value;
                    name = node.SelectSingleNode("Name");
                }
                else 
                {
                    id = node.Attributes["code"].Value;
                    name = node.SelectSingleNode("Description");

                }
               

                ExternalSystemsTablesCode table = (from a in externalTables
                                                   where a.Code == id
                                                   select a).FirstOrDefault();

                if (table != null)
                {

                    if (name != null)
                        table.Name = name.InnerText;

                }

                else
                {
                    ExternalSystemsTablesCode external = new ExternalSystemsTablesCode()
                    {
                        Id = IdCounter.GetNumber("ExternalSystemsTablesCode", tenant),
                        Code = id,
                        Name = name != null ? name.InnerText : null,
                        Tenant = tenant,
                        LogitudeTable = logitudeTableName,
                        CreatedDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                        SearchFields = id + ',' + name.InnerText + ',' + logitudeTableName,
                        UpdatedDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    };

                    externalRep.Add(external);
                    externalRep.SubmitChanges();

                }
            }

        }
        public void GetAdministrations(string txtURL, string cmbDivision)
        {
            string sURL = Convert.ToString((txtURL.Substring(txtURL.Length- 1) != "/" ? txtURL + "/" : txtURL));

            //cmbDivision.DataSource = null;
            //cmbDivision.Items.Clear();
            Administrations.Clear();



            if (txtURL.Length > 0)
            {
                HttpWebRequest request = default(HttpWebRequest);
                HttpWebResponse response = default(HttpWebResponse);
                Stream receiveStream = default(Stream);
                StreamReader readStream = default(StreamReader);
                XPathDocument xml = default(XPathDocument);

                try
                {
                    //Logging in can be done in two ways:
                    // - query string parameters in the url: "UserName" and "Password" -- OBSOLETE --
                    // - form data: "_UserName_" and "_Password_"

                    //NOTE:
                    //Logging in by specifying user name and password in the url will not be supported anymore in the near future.
                    //For backwards compatibility temporarily still supported.

                    bool bLoginWithFormData = true;

                    string sURLDivisions = sURL + "Docs/XMLDivisions.aspx";
                    if (bLoginWithFormData)
                    {
                        //Login with form data.
                        //An extra POST request is needed for this.
                        if (!_Login(sURLDivisions, "_UserName_=" + HttpUtility.UrlEncode(Credentials.UserName) + "&_Password_=" + HttpUtility.UrlEncode(Credentials.Password)))
                        {
                            throw new Exception("Access denied.");
                        }
                    }
                    else
                    {
                        //Obsolete method: login with query string parameters.
                        //This can be done at the same time when retrieving the administrations.
                        sURLDivisions = sURLDivisions + "?UserName=" + HttpUtility.UrlEncode(Credentials.UserName) + "&Password=" + HttpUtility.UrlEncode(Credentials.Password);
                    }


                    request = _CreateWebRequest(Directions.Export, sURLDivisions);
                    if ((request != null))
                    {
                        response = (HttpWebResponse)request.GetResponse();
                        receiveStream = response.GetResponseStream();
                        readStream = new StreamReader(receiveStream, Encoding.UTF8);

                        xml = new XPathDocument(readStream);
                        XPathNavigator nav = xml.CreateNavigator();
                        XPathNodeIterator Admins = nav.Select("/Administrations/Administration");
                        int iCurrentAdmin = -1;


                        if ((Admins != null) && Admins.Count > 0)
                        {
                            int iCount = 0;

                            while (Admins.MoveNext())
                            {
                                nav = Admins.Current;

                                Administrations.Add(new AdministrationData(Convert.ToInt64(nav.GetAttribute("Code", string.Empty)), Convert.ToInt64(nav.GetAttribute("HID", string.Empty)), Convert.ToString(nav.Evaluate("string(Description)"))));

                                //Automatically select the last used division by this user
                                if (string.Compare(Convert.ToString(nav.GetAttribute("Current", string.Empty)), "True") == 0)
                                {
                                    iCurrentAdmin = iCount;
                                }

                                iCount += 1;
                            }

                        }
                        //cmbDivision.DataSource = Administrations;
                        //cmbDivision.DisplayMember = "LongDescription";
                        //cmbDivision.ValueMember = "Code";
                        if (iCurrentAdmin != -1)
                        {
                            //cmbDivision.SelectedIndex = iCurrentAdmin;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                if ((request != null))
                    request.Abort();
                if ((readStream != null))
                    readStream.Close();
                if ((receiveStream != null))
                    receiveStream.Close();
                if ((response != null))
                    response.Close();
            }
        }

        private bool _Login(string sURL, string sFormData)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sURL);

            if (CredentialsChanged)
            {
                //The credentials are being cached. 
                //So if they have been changed a new cookie container should be created.
                CookieContainer = new System.Net.CookieContainer();
                CredentialsChanged = false;
            }

            request.CookieContainer = CookieContainer;

            if (ProxyServer.UseProxy)
            {
                if (ProxyServer.Port.Length > 0)
                {
                    request.Proxy = new WebProxy(ProxyServer.Server, Convert.ToInt32(ProxyServer.Port));
                }
                else
                {
                    request.Proxy = new WebProxy(ProxyServer.Server);
                }
                if (ProxyServer.Credentials.Domain.Length > 0)
                {
                    request.Proxy.Credentials = new System.Net.NetworkCredential(ProxyServer.Credentials.UserName, ProxyServer.Credentials.Password, ProxyServer.Credentials.Domain);
                }
                else
                {
                    request.Proxy.Credentials = new System.Net.NetworkCredential(ProxyServer.Credentials.UserName, ProxyServer.Credentials.Password);
                }
            }

            byte[] bFormData = Encoding.UTF8.GetBytes(sFormData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.AllowWriteStreamBuffering = true;

            Stream reqStream = request.GetRequestStream();
            reqStream.Write(bFormData, 0, bFormData.Length);
            reqStream.Close();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK && string.Compare(response.ResponseUri.AbsoluteUri, sURL) == 0)
                {
                    //Also check if not redirected to another page
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

       private bool SetAdministration(string txtURL, string sDivision)
       {
	string sBaseURL = Convert.ToString((txtURL.Substring(txtURL.Length-1) != "/" ? txtURL + "/" : txtURL));
	string sSwitchDivisionPart = "Docs/ClearSession.aspx?Division=" + sDivision + "&Remember=3";
	string sSwitchDivisionURL = null;
	HttpWebRequest request = default(HttpWebRequest);
	HttpWebResponse response = default(HttpWebResponse);
	bool statusOK = false;
	bool succeeded = false;

	sSwitchDivisionURL = sBaseURL + sSwitchDivisionPart;
	//The Remember parameter with a value '3' is needed to be able to switch divisions without affecting the last used division,
	//and to do nothing when the division has been switched already to the correct one.
	request = _CreateWebRequest(Directions.Export, sSwitchDivisionURL, "", true);
	response = (HttpWebResponse)request.GetResponse();

	if (response != null && response.StatusCode == HttpStatusCode.OK) {
		statusOK = true;
	}

	if (request != null)
		request.Abort();
	if (response != null)
		response.Close();

	if (statusOK) {
		//Check if the administration has been set successfully.
		request = _CreateWebRequest(Directions.Export, sBaseURL + "Docs/XMLDivisions.aspx");
		response = (HttpWebResponse)request.GetResponse();
		if (response != null) {
			XPathDocument xml = new XPathDocument(response.GetResponseStream());
			XPathNavigator nav = xml.CreateNavigator();
			XPathNavigator current = nav.SelectSingleNode("/Administrations/Administration[@Current=\"True\"]");
			if (current != null) {
				if (String.Compare(Convert.ToString(current.GetAttribute("Code", string.Empty)), sDivision) == 0) {
					succeeded = true;
				} else {
					throw new Exception("Invalid administration.");
				}
			}
			response.Close();
		}
		request.Abort();
	}

	return succeeded;
       }

       private HttpWebRequest _CreateWebRequest(Directions Direction, string sURL, string sUploadFile = "", bool bIgnoreResponseXML = false)
       {

           HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sURL);

           if (CredentialsChanged)
           {
               //The credentials are being cached. 
               //So if they have been changed a new cookie container should be created.
               CookieContainer = new System.Net.CookieContainer();
               CredentialsChanged = false;
           }

           request.CookieContainer = CookieContainer;

           if (ProxyServer.UseProxy)
           {
               if (ProxyServer.Port.Length > 0)
               {
                   request.Proxy = new WebProxy(ProxyServer.Server, Convert.ToInt32(ProxyServer.Port));
               }
               else
               {
                   request.Proxy = new WebProxy(ProxyServer.Server);
               }
               if (ProxyServer.Credentials.Domain.Length > 0)
               {
                   request.Proxy.Credentials = new System.Net.NetworkCredential(ProxyServer.Credentials.UserName, ProxyServer.Credentials.Password, ProxyServer.Credentials.Domain);
               }
               else
               {
                   request.Proxy.Credentials = new System.Net.NetworkCredential(ProxyServer.Credentials.UserName, ProxyServer.Credentials.Password);
               }
           }

           if (Direction == Directions.Import)
           {
               request.Method = "POST";
               request.AllowWriteStreamBuffering = true;



            

              // Retrieve request stream 
               Stream reqStream = request.GetRequestStream();
               byte[] byteData = Encoding.Unicode.GetBytes(sUploadFile);
               //Open the local file
              // FileStream rdr = new FileStream(sUploadFile, FileMode.Open);

               //Allocate byte buffer to hold file contents
              // byte[] inData = new byte[4097];

               //loop through the local file reading each data block
               //and writing to the request stream buffer
               //int bytesRead = rdr.Read(inData, 0, inData.Length);
               //while (bytesRead > 0)
               //{
                   reqStream.Write(byteData, 0, byteData.Length);
                   //bytesRead = rdr.Read(inData, 0, inData.Length);
               //}

              // rdr.Close();
               reqStream.Close();
           
           }
           else
           {
               request.Method = "GET";
           }

           HttpWebResponse response = (HttpWebResponse)request.GetResponse();
           if (response.StatusCode != HttpStatusCode.OK)
           {
               throw new Exception(response.StatusDescription);
               response.Close();
               return null;
           }
           else
           {
               if (bIgnoreResponseXML | response.ContentType == "text/xml")
               {
                   return request;
               }
               else
               {
                   StreamReader r = new StreamReader(response.GetResponseStream());
                   string sHtml = r.ReadToEnd();
                   response.Close();
                   throw new Exception(GetErrorFromHTML(sHtml));

                   return null;
               }
           }

       }

       private string GetErrorFromHTML(string shtml)
       {
           string sErrorCode = "";

           int iPos = shtml.IndexOf("id=\"mode\"");
           if (iPos > 0)
           {
               iPos = shtml.IndexOf( "value=\"",iPos);
               if (iPos > 0)
               {
                   //sErrorCode = Strings.Mid(shtml, iPos + 7, Strings.InStr(iPos + 7, shtml, "\"", CompareMethod.Text) - iPos - 7);
                  // sErrorCode=shtml.Substring(iPos+7,shtml.IndexOf(
               }
           }

           switch (sErrorCode)
           {
               case "0":
                   return "You have insufficient rights to perform this operation.";
               case "8":
                   return "Page requested too many times, please retry in a few moments.";
               default:
                   return "error";
                   return "Response is not xml. " + Environment.NewLine + "You might have insufficient rights to perform this operation." + Environment.NewLine + Environment.NewLine + shtml;
           }

       }

       //public string[] ExportParameters (string sTopic)
       //{
           
       //        ArrayList Parameters = new ArrayList();
       //        foreach (Topic t in TopicXML.Topics)
       //        {
       //            if (t.Code.ToLower() == sTopic.ToLower())
       //            {
       //                foreach (Parameter p in t.Parameters.Export)
       //                {
       //                    Parameters.Add(p.name);
       //                }
       //                break; // TODO: might not be correct. Was : Exit For
       //            }
       //        }
       //        return (string[])Parameters.ToArray(typeof(string));
           

         

       //}
 

    }





   public class AdministrationData
   {
       private long _Code;
       private long _HID;

       private string _Description;
       public AdministrationData(long Code, long HID, string Description)
       {
           _Code = Code;
           _HID = HID;
           _Description = Description;
       }

       public long Code
       {
           get { return _Code; }
       }

       public long HID
       {
           get { return _HID; }
       }

       //" s.Right(("000000" & _HID) - 6) + " - " + _Description; 
       public string LongDescription
       {
           get { return "000001 - Logitude World";}
       }

       public string Description
       {
           get { return _Description; }
       }

       public override string ToString()
       {
           return _Code + " - " + _HID + " - " + _Description;
       }

   }


   public class ProxyServerData
   {
       public bool UseProxy = false;
       public string Server;
       public string Port;
       public CredentialData Credentials = new CredentialData();
   }


    public class CredentialData
    {

        public string Domain;
        public string UserName;
        public string Password;

    }




       
}
