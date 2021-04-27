using Logitude.AmitalMessaging.Utils;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnifreightIIG.Common.CommonIIGInterface;


namespace Logitude.CustomsMessaging.Dca
{
    public class DcaReceivedService<TCustomsResponse>
        where TCustomsResponse : class ,IINF_MSG_Generic,new()
    {
        private string _SelectedFile;
        private string _FileContents;
        XDocument _XDocument;
        private Customs.BL.Utils.DCAFileModel _DCAFileModel;
        private string _ESBResponseParserBody;
        public DcaReceivedService(string selectedFile, string fileContents)
        {
            // TODO: Complete member initialization
            this._SelectedFile = selectedFile;
            this._FileContents = fileContents;
            _DCAFileModel = Logitude.Customs.BL.Utils.DCAFilePraser.GetDCAFileModel(_SelectedFile);
        }

        internal void ProccessIt()
        {
            try
            {
                

                try
                {
                    _XDocument = System.Xml.Linq.XDocument.Parse(_FileContents);
                    IsValidXml = true;
                }
                catch //(Exception)
                {
                    this.ErrorMessage = "fileContents Is not Valid Xml";
                    return;
                    //throw;
                }

                var myESBResponseParser = new UnifreightIIG.Common.MessageLib.General.ESBResponseParser(_FileContents);
                var errorMessage = "";
                var parsed = myESBResponseParser.Procces(out errorMessage);
                if (!parsed)
                {
                    this.ErrorMessage = "Exception:Bad XML FILE Can not Parse myESBResponseParser.Procces()" + errorMessage;
                    return;
                    throw new System.Exception("Bad XML FILE Can not Parse myESBResponseParser.Procces()" + errorMessage);
                }
                CorrelationId = myESBResponseParser.ResponseHeader.CorrelationId;
                ExternalId = myESBResponseParser.ResponseHeader.ExternalId;
                IsValidESBResponseParser = true;


                try
                {
                    //if (TCustomsResponse is DF_NG_2754_MSG10004_ImportDeclarationResponse)
                    //{
                    //    if (myESBResponseParser.Body.Contains("DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg"))
                    //    {
                    //        var xdoc=XDocument.Parse(myESBResponseParser.Body).Document;
                    //        var xmlResponseContentHeader = xdoc.Elements().Elements().First().ToString();
                    //        var list = xdoc.Elements().Elements().Select(node => node.Name.ToString().ToUpper());
                    //        {URN:WCO:DATAMODEL:WCO:RES:1}RESPONSE
                    //        var xmlResponse = xdoc.Elements().Elements().FirstOrDefault (  node => node.Name.ToString().ToUpper().Equals( "{URN:WCO:DATAMODEL:WCO:RES:1}RESPONSE")).ToString();
                            
                    //        var myTCustomsResponse = new UnifreightIIG.Common.ImportDeclarationServiceReference.DF_NG_2754_MSG10004_ImportDeclarationResponse();
                    //        myTCustomsResponse.ResponseContentHeader = new UnifreightIIG.Common.ImportDeclarationServiceReference.ResponseContentHeader(); //XmlGenericUtil<UnifreightIIG.Common.ImportDeclarationServiceReference.ResponseContentHeader>.DeSerializeObject(xmlResponseContentHeader);
                    //        myTCustomsResponse.Response = XmlGenericUtil<UnifreightIIG.Common.ImportDeclarationServiceReference.Response>.DeSerializeObject(xmlResponse);
                    //        CustomsResponse = myTCustomsResponse as TCustomsResponse;
                    //    }
                    //}
                    if (CustomsResponse == null)
                    {
                        CustomsResponse = XmlGenericUtil<TCustomsResponse>.DeSerializeObject(myESBResponseParser.Body);
                        _ESBResponseParserBody = myESBResponseParser.Body;
                    }

                }
                catch
                {
                    this.ErrorMessage = "<TCustomsResponse>.DeserilazeObject fail -the XSD not valid !!!(ask itzik to refresh XSD )";
                    return;
                }
                
                string externalId = //GetExternalId(_SelectedFile);
                    _DCAFileModel.OurRefExtrenalId;
                if (externalId != myESBResponseParser.ResponseHeader.ExternalId && externalId != myESBResponseParser.ResponseHeader.ExternalId +"_E")
                {

                    this.ErrorMessage = ("Exception:it must be GetExternalId(selectedFile) (" + externalId + ")  == myESBResponseParser.ResponseHeader.ExternalId (" + myESBResponseParser.ResponseHeader.ExternalId + ")") + _DCAFileModel.ErrorMessage;
                    return;
                    throw new Exception("it must be GetExternalId(selectedFile) (" + externalId + ")  == myESBResponseParser.ResponseHeader.ExternalId (" + myESBResponseParser.ResponseHeader.ExternalId + ")");
                }
                IsExternalIdMatchFileNameAndContents = true;
            }
            finally
            {
                if (string.IsNullOrWhiteSpace(ExternalId))
                {
                    ExternalId = _DCAFileModel.OurRefExtrenalId; // GetExternalId(_SelectedFile);
                }

            }
        }



        public MemoryStream GetMemoryStreamCustomsResponse()
        {
            var contents = _FileContents;
            if (!string.IsNullOrWhiteSpace(_ESBResponseParserBody))
            {
                contents = _ESBResponseParserBody;
            }
            var ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(contents));

            if (CustomsResponse != null)
            {

                LogMessagingUtil.Instance.AppendLine(@"semi validateXML see if i can do XmlGenericUtil<TCustomsResponse>.MemoryStreamSerializeWithDefaultNamespace(CustomsResponse);");
                var memCustomsResponse = XmlGenericUtil<TCustomsResponse>.MemoryStreamSerializeWithDefaultNamespace(CustomsResponse);
                
                    
                    var toShowSourceXml = true;
                    if (toShowSourceXml)
                    {
                        return ms;
                    }
                    else
                    {
                        return memCustomsResponse;
                    }
                
            }

            return ms;
        }

        private static bool CompareMemoryStreams(MemoryStream ms1, MemoryStream ms2)
        {
            if (ms1.Length != ms2.Length)
                return false;
            ms1.Position = 0;
            ms2.Position = 0;

            var msArray1 = ms1.ToArray();
            var msArray2 = ms2.ToArray();

            return msArray1.SequenceEqual(msArray2);
        }
        public bool IsValidESBResponseParser { get;private  set; }

        public bool IsValidXml { get;private  set; }

        public bool IsExternalIdMatchFileNameAndContents { get;private  set; }
        public string CorrelationId { get;private  set; }

        public TCustomsResponse CustomsResponse { get; private set; }
  

        public string ErrorMessage { get; private set; }

        public string ExternalId { get; private set; }

        
    }
    

}
