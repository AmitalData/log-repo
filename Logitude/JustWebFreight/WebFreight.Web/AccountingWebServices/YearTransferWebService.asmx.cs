using Logitude.Accounting.BL.Utils;


using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;

namespace WebFreight.Web.AccountingWebServices
{
    /// <summary>
    /// Summary description for YearTransferWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class YearTransferWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public byte[] PerformYearTransfer(byte[] requestParams)
        {
            MemoryStream memorystream = new MemoryStream(requestParams);
            XmlSerializer serializer = new XmlSerializer(typeof(YearTransferRequestParams));
            YearTransferRequestParams newRequestParams = (YearTransferRequestParams)serializer.Deserialize(memorystream);
            int year = newRequestParams.Year;
            int tenant = newRequestParams.Tenant;
            YearTransferResponseData responseData = new YearTransferResponseData();
            responseData.HasException = false;
            responseData.Succeeded = true;
            responseData.UserMessage = null;
            try
            {
                GLAccountEndOfTheYearBatch.GLAccountRevenueExpenseTransfer(year, tenant);
            }
            catch (Exception e)
            {
                responseData.HasException = true;
                responseData.Succeeded = false;
                responseData.UserMessage = e.Message;
            }
            if (!responseData.HasException)
            {
                responseData.UserMessage = year.ToString() + " " + TranslateTextsClass.Translate("GLAccounts.Q.YearTransferSucceed", tenant);
            }
            
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(YearTransferResponseData)); 
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }
    }
    class YearTransferResponseData
    {
        public bool HasException { get; set; }
        public bool Succeeded { get; set; }
        public string UserMessage { get; set; }
    }
    class YearTransferRequestParams
    {
        public int Year { get; set; }
        public int Tenant { get; set; }
    }
}
