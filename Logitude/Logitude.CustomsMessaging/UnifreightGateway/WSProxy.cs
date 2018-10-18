using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.UnifreightGateway
{
    public interface IWSProxy
    {
        string GetAssemblyQualifiedName();
        string GetExampleDataIn1();
        string GetExampleDataIn2();
        string GetExampleDataout1();
        string GetExampleDataout2();
        void ProccessRequest(
            string DataIn1,
            string DataIn2,
            out string DataOut1,
            out string DataOut2,
            out string SUCCESS,
            ref string MoreParams,
            out string MessageOut);


        void ProccessBASE64Request(
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


    }

    public abstract class WSProxy : IWSProxy, IDisposable //: UniBase
    {

        protected static string WebserviceURL = "";
        protected static string WebserviceUser = "";
        protected static string WebservicePass = "";
        protected static int TimeoutInSec = 900;


        public static string UniDescription = "BaseWSProxy";
        public static string UniVersion = "1.000.000001";
        public static bool UniProduction = false;
        protected StringBuilder _sbLog = new StringBuilder();

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



        public abstract void Dispose();
        
    }
}
