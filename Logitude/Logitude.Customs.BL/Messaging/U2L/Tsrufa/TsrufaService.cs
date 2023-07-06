using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.StimulReport.Mapping;
using Logitude.Customs.Data;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.U2L.Tsrufa
{
    public class TsrufaService : UnifreightGenericService
    {
        private LOGITSRUFA _LOGITSRUFA;
        private LogitudeTsrufa _LogitudeTsrufa;
        private SupplierInvoicePM _MySupplierInvoicePM;
        private ICustomContext _context;

        public const string UpsertActionConst = "Logitude.Customs.BL.Messaging.U2L.Tsrufa.TsrufaService.Upsert()";
        private DeclarationPM _MyDeclarationPM;
        private Stopwatch _Stopwatch;

        public TsrufaService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }

        public override void ProccessGenericRequest(
              string xmlLOGITSRUFA,
              ref string MoreParams,
              out string MessageOut)
        {
            MessageOut = "";
            _Stopwatch = Stopwatch.StartNew();  
            MyCommunicationsParams.Subject = "TsrufaService ";

            DeserilazeObject(xmlLOGITSRUFA);
            AppendLogLine("DeserilazeObject:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart(); 
            
            CheckIntegrity();
            AppendLogLine("CheckIntegrity:Took:" + _Stopwatch.Elapsed.ToString());_Stopwatch.Restart(); 
            MyGenericResponseObj.Stage = "GetContext";
            _context = CustomContext.GetContext(ResolvedTenant());
            var myQueryService = new DeclarationQueryService(_context);

            MyGenericResponseObj.Stage = "GetSingle";
            this._MyDeclarationPM = myQueryService.GetAcceptDeclarationAmendmentWithComp(this._LogitudeTsrufa.Id, ResolvedTenant());
            if (this._MyDeclarationPM == null)
            {
                throw new BusinessErrorException("Id is " + this._LogitudeTsrufa.Id + " but not found");
            }
            AppendLogLine("GetSingle:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart(); 
            ICustomContext dbContext = CustomContext.GetContext(ResolvedTenant());
            MyGenericResponseObj.Stage = "GetXml for file " + this._MyDeclarationPM.CustomFileNo;
            DeclarationSRMapping declarationSRMapping = new DeclarationSRMapping();
            var xml = declarationSRMapping.GetXml(this._MyDeclarationPM);
            if (String.IsNullOrWhiteSpace(xml))
            {
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                MyGenericResponseObj.Message = "GetXml returned null";
                return;
            }
            MyGenericResponseObj.Stage = "Get Tsrufa Xml Done ";
            AppendLogLine("GetXml:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            MyGenericResponseObj.ApplicationId = _MyDeclarationPM.Id;
            MyGenericResponseObj.ResponseXml = xml;
            MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
            MyCommunicationsParams.LoggingEntityId = MyGenericResponseObj.ApplicationId;


        }

        private void DeserilazeObject(string xmlLOGITSRUFA)
        {

            MyGenericResponseObj.Stage = "Initalize ProccessRequest";
            AppendLogLine("TsrufaService.ProccessRequest");

            AppendLogLine("Deserialize(DataIn1) ..");


            if (string.IsNullOrWhiteSpace(xmlLOGITSRUFA))
            {
                throw new BusinessErrorException("DataIn1 is missing");
            }
            if (xmlLOGITSRUFA.Length > 1000)
            {
                AppendLogLine("XmlIn=" + xmlLOGITSRUFA.Substring(0, 1000));
                AppendLogLine(".Substring(0, 1000)");
            }
            else
            {
                AppendLogLine("XmlIn=" + xmlLOGITSRUFA);
            }


            AppendLogLine("Tring DeserilazeObject");
            MyGenericResponseObj.Stage = "Trying DeserilazeObject";
            this._LOGITSRUFA = XmlGenericUtil<LOGITSRUFA>.DeSerializeObject(xmlLOGITSRUFA);

            if (_LOGITSRUFA.LogitudeTsrufa == null || _LOGITSRUFA.LogitudeTsrufa.Length != 1)
            {
                throw new BusinessErrorException("_LOGITSRUFA.Tsrufa.Length != 1");
            }
            this._LogitudeTsrufa = _LOGITSRUFA.LogitudeTsrufa[0];
        }

        private void CheckIntegrity()
        {
            MyGenericResponseObj.Stage = "Check integrity ";

            if (String.IsNullOrWhiteSpace(this._LogitudeTsrufa.Id))
            {
                throw new BusinessErrorException("Id is missing");
            }
            AppendLogLine("Id = " + this._LogitudeTsrufa.Id);
        }

        

        public override string GetAssemblyQualifiedName()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataIn1()
        {
            var xml = "";
            var amitalObjExample = new LOGITSRUFA();
            var myAmitalTsrufa = new LogitudeTsrufa();
           

            myAmitalTsrufa.Id = "1-1";
            myAmitalTsrufa.Tenant = "1";
            

            amitalObjExample.LogitudeTsrufa = new LogitudeTsrufa[] { myAmitalTsrufa };

            xml = XmlGenericUtil<LOGITSRUFA>.SerializeObject(amitalObjExample);

            return xml;
        }

        public override string GetExampleDataIn2()
        {
            return "";
        }

        public override string GetExampleDataout1()
        {
            return "";
        }

        public override string GetExampleDataout2()
        {
            return "";
        }

        public override void ProccessRequest(string DataIn1, string DataIn2, out string DataOut1, out string DataOut2, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }

        public override void ProccessBASE64Request(string BASE64DataIn1, string BASE64DataIn2, string BASE64DataIn3, out string BASE64DataOut1, out string BASE64DataOut2, out string BASE64DataOut3, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }
    
    }
}

