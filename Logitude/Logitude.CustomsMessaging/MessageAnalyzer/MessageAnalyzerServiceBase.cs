using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using UnifreightIIG.Common.Utils;


namespace Logitude.CustomsMessaging.MessageAnalyzer
{
    //TResponseData,TCustomResponse,TRequestParams
    public abstract class MessageAnalyzerServiceBase<TRequestParams, TResponseData, TCustomResponse, TResponseService> : IMessageAnalyzerService
        where TCustomResponse : class
        where TRequestParams : Logitude.CustomsMessaging.Common.RequestParams.RequestParamsBase,new()
        where TResponseService : Logitude.CustomsMessaging.ResponseServices.ResponseServiceBase<TResponseData, TCustomResponse, TRequestParams>, new()
        where TResponseData : Logitude.CustomsMessaging.Common.ResponseData.INF_MSG_GenericResponseData, new()
    {
   

        public readonly string MessageXSD;//= "UnifreightIIG.Common.MessageLib.PhysicalCheck.CH_NG_190_MSG1_NoticeToClient.xsd";
        bool _IsValid;
        string _Xml;
        
        protected TCustomResponse _CustomResponse;


        public MessageAnalyzerServiceBase(string myXSD)
        {
            MessageXSD = myXSD;
        }
        public MessageAnalyzerServiceBase()
        {

        }
        
        protected virtual void LoadXML(string fileContents)
        {
            _CustomResponse = XmlGenericUtil<TCustomResponse>.DeSerializeObject(fileContents);
            Xml = fileContents;
        }

        protected virtual void Validate()
        {
            var toValidate = false;

            if (!toValidate) return;
            if (String.IsNullOrWhiteSpace(MessageXSD)) return;
            if (_CustomResponse == null)
            {
                throw new Exception("Loadxml 1st ");
            }


            var xsdStream = ManifestResourceUtil.GetManifestResourceStreamXSD<TCustomResponse>(_CustomResponse, MessageXSD);
            string targetNamespace = XmlGenericUtil<TCustomResponse>.GetDefaultNamespace(typeof(TCustomResponse));

            XmlSchemaSet schemas = new XmlSchemaSet();


            schemas.Add(targetNamespace, XmlReader.Create(xsdStream));
            // new StringReader(xsdMarkup)));

            XDocument doc1 = XDocument.Parse(Xml);




            Debug.WriteLine("Validating doc1");
            bool errors = false;
            doc1.Validate(schemas, (o, e) =>
            {
                Debug.WriteLine("{0}", e.Message);
                errors = true;
            });
            Debug.WriteLine("doc1 {0}", errors ? "did not validate" : "validated");
        }

        public abstract string GetObjectTableName();
        public abstract int ResolveTenant();
        public abstract string GetLoggingEntityReference();

        public virtual string GetLoggingObjectTableId()
        {
            ObjectTabelRepository objectTableRepository = new ObjectTabelRepository(0);//ResolveTenant());
            ObjectTable objectTable = objectTableRepository.GetObjectTableByName(GetObjectTableName(), 0, true);
            return objectTable.Id;
        }

        public virtual string GetLoggingEntityId()
        {
            if (ResponseData == null) return "";
            return ResponseData.ApplicationID;

        }

        public virtual string GetSubject()
        {
            var n = this.GetType().Name.ToString();
            return "DCA " + n; ;
        }


        //public abstract Logitude.CustomsMessaging.Common.ResponseData.INF_MSG_GenericResponseData AnalyzeToOverride();

        public virtual TResponseData AnalyzeToOverride()
        {
            //XmlSerializer serializer = new XmlSerializer(typeof(CH_NG_190_MSG1_NoticeToClient));
            TCustomResponse requestMessage = null;///(CH_NG_190_MSG1_NoticeToClient)serializer.Deserialize(memstream);
            requestMessage = _CustomResponse;

            var reData = new TResponseData();
            
            var requestParams = new TRequestParams();
            requestParams.Tenant = ResolveTenant();
            ///InsertPhysicalCheck(physicalCheckMessage);
            var customResponseService = new TResponseService();
            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(customResponseService.GetType().Name + ":Update ...");
            customResponseService.Update(requestMessage, requestParams);
            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(customResponseService.GetType().Name + ":Update End!");
            if (customResponseService.MyResponseData == null)
            {
                //??
                return null;
            }
            reData.ExceptionMessage = customResponseService.MyResponseData.ExceptionMessage;
            reData.ApplicationID = customResponseService.MyResponseData.ApplicationID;
            reData.HasException = customResponseService.MyResponseData.HasException;
            reData.Succeeded = customResponseService.MyResponseData.Succeeded;
            
            return reData;
        }

        public void Analyze(string dcaResponseMessage)
        {
            try
            {
                LoadXML(dcaResponseMessage);
            }
            catch (Exception e)
            {
                Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("LoadXML:failed ! Please insure that u have the accurate schema");
                throw;
            }
            
            Validate();
            try
            {
                MyResponseData = AnalyzeToOverride();
            }
            catch (DbEntityValidationException ex)
            {
              
                throw ExceptionFormatUtil.GetFormated(ex);
            }
            catch (BusinessErrorException businessErrorException)
            {
                throw;
            }
            
            catch (Exception)
            {

                throw;
            }
            
        }




        public TResponseData MyResponseData { get; private set; }


        public string Xml
        {
            get { return _Xml; }
            set { _Xml = value; }
        }


        public INF_MSG_GenericResponseData ResponseData
        {
            get { return MyResponseData as INF_MSG_GenericResponseData; }
        }
    }
}
