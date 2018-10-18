using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
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
    public abstract class MessageAnalyzerServiceBaseOld<TCustomResponse> : IMessageAnalyzerService
            where TCustomResponse : class
    {

        public readonly string MessageXSD;//= "UnifreightIIG.Common.MessageLib.PhysicalCheck.CH_NG_190_MSG1_NoticeToClient.xsd";

        bool _IsValid;
        string _Xml;
        //string _EntityReference;
        
        //string _ErrorMessage;
        

        
        protected TCustomResponse _CustomResponse;


        public MessageAnalyzerServiceBaseOld(string myXSD)
        {
            MessageXSD = myXSD;
        }
        public MessageAnalyzerServiceBaseOld()
        {

        }
        //public MessageAnalyzerServiceBase(string myXsd)
        //{
        //    MessageXSD = myXsd;
        //}
        protected  virtual void LoadXML(string fileContents)
        {
            _CustomResponse = XmlGenericUtil<TCustomResponse>.DeSerializeObject(fileContents);
            Xml = fileContents;
        }

        protected virtual void Validate()
        {
            var toDo = false;

            if (!toDo) return;
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

        // public abstract  string GetMessageXSD();




        public abstract string GetObjectTableName();
        public abstract int ResolveTenant();
        public abstract string GetLoggingEntityReference();

        public virtual  string GetLoggingObjectTableId()
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
            return "DCA " +n;;
        }
        

        public abstract Logitude.CustomsMessaging.Common.ResponseData.INF_MSG_GenericResponseData AnalyzeToOverride();
        public void Analyze(string dcaResponseMessage)
        {
            LoadXML(dcaResponseMessage);
            Validate();
            ResponseData =AnalyzeToOverride();
        }


        

        public Logitude.CustomsMessaging.Common.ResponseData.INF_MSG_GenericResponseData ResponseData { get; private set; }
        
       


        public string Xml
        {
            get { return _Xml; }
            set { _Xml = value; }
        }












        
    }
}
