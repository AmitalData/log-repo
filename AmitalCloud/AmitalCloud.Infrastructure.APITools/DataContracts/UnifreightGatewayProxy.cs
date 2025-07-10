using AmitalCloud.Infrastructure.APITools.Interfaces;
using System;
using System.Collections;
using System.Text;

namespace AmitalCloud.Infrastructure.APITools.DataContracts
{
    public abstract class UnifreightGatewayProxy : IUnifreightGatewayProxy, IDisposable //: UniBase
    {

        public readonly string UniDescription = "BaseWSProxy";
        public readonly string UniVersion = "1.000.000001";
        public readonly bool UniProduction = false;


        protected static string WebserviceURL = "";
        protected static string WebserviceUser = "";
        protected static string WebservicePass = "";
        protected static int TimeoutInSec = 900;



        private StringBuilder _sbLog = new StringBuilder();
        public UnifreightGatewayProxy(string uniDescription, string uniVersion, bool uniProduction)
        {
            UniDescription = uniDescription;
            UniVersion = uniVersion;
            UniProduction = uniProduction;
        }
        public object MyUnity { get; set; }
        public string GetLog()
        {
            return _sbLog.ToString();
        }
        public void AppendLogLine(string line)
        {
            _sbLog.AppendLine(line);
        }
        public void InsertLogLine(int index, string line)
        {
            _sbLog.Insert(index, line);
        }

        public abstract string GetAssemblyQualifiedName();
        public abstract string GetExampleDataIn1();
        public abstract string GetExampleDataIn2();
        public abstract string GetExampleDataout1();
        public abstract string GetExampleDataout2();
        public abstract void ProccessRequest(
            string DataIn1,
            string DataIn2,
            out string DataOut1,
            out string DataOut2,
            out string SUCCESS,
            ref string MoreParams,
            out string MessageOut);


        public abstract void ProccessBASE64Request(
                  string BASE64DataIn1,
                  string BASE64DataIn2,
            string BASE64DataIn3,
                  out string BASE64DataOut1,
                  out string BASE64DataOut2,
            out string BASE64DataOut3,
                  out string SUCCESS,
                  ref string MoreParams,
                  out string MessageOut
        );

        protected static string GetString(Hashtable hDataIn1, string pKey)
        {
            if (string.IsNullOrEmpty(pKey)) return "";
            if (hDataIn1 == null) return "";
            pKey = pKey.ToUpper();
            if (!hDataIn1.ContainsKey(pKey)) return "";
            return hDataIn1[pKey].ToString();
        }

        public void SetWebserviceURL(string cWebserviceURL)
        {
            //Gets or sets the base URL of the XML Web service the client is requesting. 
            WebserviceURL = cWebserviceURL;
        }
        public void SetCredentials(string cWebserviceUser, string cWebservicePas)
        {
            //Gets or sets the base URL of the XML Web service the client is requesting. 
            WebserviceUser = cWebserviceUser;
            WebservicePass = cWebservicePas;
        }
        public void SetTimeoutInSec(string cTimeoutInSec)
        {
            //Gets or sets the base URL of the XML Web service the client is requesting. 
            int to = 300;
            if (cTimeoutInSec != "")
            {

                int.TryParse(cTimeoutInSec, out to);
                if (to > 50)
                {
                    TimeoutInSec = to;
                }
                else
                {
                    TimeoutInSec = 300;
                }
            }
            else
            {
                TimeoutInSec = 300;

            }

        }



        //public abstract void Dispose();
        public virtual void Dispose()
        {
            MyUnity = null;
        }

    }

}
