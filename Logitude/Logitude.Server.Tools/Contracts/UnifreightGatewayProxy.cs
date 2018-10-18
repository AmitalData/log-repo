using Logitude.AmitalMessaging.Infrastructure;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Contracts
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



        private  StringBuilder _sbLog = new StringBuilder();
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
        public void InsertLogLine(int index,string line)
        {
            _sbLog.Insert(index,line);
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
  
    public interface IUnifreightGatewayProxy
    {
        string GetLog();
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
        object MyUnity { get; set; }


    }


    public abstract class UnifreightGenericService : UnifreightGatewayProxy, IUnifreightGenericService
    {
        private GenericResponseObj _GenericResponseObj = new GenericResponseObj();
        private CommunicationsParams _CommunicationsParams = new CommunicationsParams();
        private int _iTenanat;
        private string _contactEmail;

        public CommunicationsParams MyCommunicationsParams
        {
            get { return _CommunicationsParams; }

        }
        public GenericResponseObj MyGenericResponseObj
        {
            get { return _GenericResponseObj; }

        }
        protected virtual string GetLoggingObjectTableId(string objectTableName)
        {
            if (String.IsNullOrWhiteSpace(objectTableName)) return "";//not must 
            var objectTableRepository = new ObjectTableRepository(0); // ObjectTabelRepository tenant must be zero !!
            var objectTable = objectTableRepository.GetObjectTableByName(objectTableName,// "Customs.PhysicalCheck", 
                0, true);

            return objectTable.Id;
        }
        protected virtual int ResolvedTenant()
        {
            return this._iTenanat;
        }


        public UnifreightGenericService(string uniDescription, string uniVersion, bool uniProduction)
            : base(uniDescription, uniVersion, uniProduction)
        {
            _GenericResponseObj = new GenericResponseObj();
        }

        /// <summary>
        /// call from UnifreightGatewayService.ProccessGenericRequest
        /// can throw BusinessErrorException  == BusinessError Other TecinicalFailure
        /// 
        /// </summary>
        /// <param name="DataIn"></param>
        /// <param name="MoreParams"></param>
        /// <param name="MessageOut"></param>
        public abstract void ProccessGenericRequest(
                    string DataIn,

                    ref string MoreParams,
                    out string MessageOut);



        public void SetTenant(int iTenanat)
        {
            _iTenanat = iTenanat;
        }

        public void SetIdentityName(string contactEmail)
        {
            _contactEmail = contactEmail;
        }
    }
    public interface IUnifreightGenericService
    {
        void ProccessGenericRequest(
                    string DataIn,

                    ref string MoreParams,
                    out string MessageOut);
        GenericResponseObj MyGenericResponseObj { get; }
        CommunicationsParams MyCommunicationsParams { get; }
    }
}
