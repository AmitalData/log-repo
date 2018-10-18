

using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.UnifreightGateway
{
    public class SivugUpsertBatchService : UnifreightGenericService
    {
        
        public const string UpsertActionConst = "Logitude.Customs.BL.Messaging.U2L.Sivug.SivugUpsertBatchService.Upsert()";
        private Stopwatch _Stopwatch;
        private Unifreight_L2US01RequestParam _Unifreight_L2US01RequestParam;

        public SivugUpsertBatchService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }
     
        public override void ProccessGenericRequest(
              string xmlList,
              ref string MoreParams,
              out string MessageOut
            )
        {
            MessageOut = "";
            _Stopwatch = Stopwatch.StartNew();
            MyCommunicationsParams.Subject = "SivugUpsertBatchService ";
            if (DateTime.Now < new DateTime(2017, 01, 15))
            {
                System.Threading.Thread.Sleep(5000);
            }
            DeserilazeObject(xmlList);
            

            AppendLogLine("DeserilazeObject:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

            ///CheckIntegrity();
            AppendLogLine("CheckIntegrity:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            MyGenericResponseObj.Stage = "GetContext";
            var s = new Unifreight_L2US01_US2L01_SivugMessagingService();

            var userId=AuthenticationUtil.ResolveUserId(ResolvedTenant());
            var res = s.Send(new Logitude.CustomsMessaging.Common.RequestParams.Unifreight_L2US01RequestParam()
            {
                CCUFILEmFileNo =  this._Unifreight_L2US01RequestParam.CCUFILEmFileNo,

                CFIFILEMFileNo = this._Unifreight_L2US01RequestParam.CFIFILEMFileNo,


                Tenant = ResolvedTenant(),
                


                ///<-- itzik (yaron ask )

                LoggingEntityId = this._Unifreight_L2US01RequestParam.LoggingEntityId, //declarationId
                DeclarationId = this._Unifreight_L2US01RequestParam.DeclarationId,
                ///LoggingEntityReference = entityPM.DeclarationNumber,
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                LoggingUserId = //this._Unifreight_L2US01RequestParam.LoggingUserId,
                userId ,
                RequestName = "L2US01 Request",
                ResponseName = "US2L01 Request",
                RequestVIA = SendRequestVIA.WebServiceBatch,
            });

            
            MyGenericResponseObj.CorrelationId = res.CustomsRequestsSheetId;
            MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;

        }

        private void DeserilazeObject(string xmlList)
        {

            MyGenericResponseObj.Stage = "Initalize ProccessRequest";
            AppendLogLine("SivugUpsertBatchService.ProccessRequest");

            AppendLogLine("Deserialize(DataIn1) ..");


            if (string.IsNullOrWhiteSpace(xmlList))
            {
                throw new BusinessErrorException("DataIn1 is missing");
            }
            if (xmlList.Length > 1000)
            {
                AppendLogLine("XmlIn=" + xmlList.Substring(0, 1000));
                AppendLogLine(".Substring(0, 1000)");
            }
            else
            {
                AppendLogLine("XmlIn=" + xmlList);
            }


            AppendLogLine("Tring DeserilazeObject");
            MyGenericResponseObj.Stage = "Trying DeserilazeObject";
            var dic = UnifreightListsUtil.Deserialize(xmlList);



            /*string stenant = UnifreightListsUtil.GetValue(ref dic, "Tenant");
                if (String.IsNullOrWhiteSpace(stenant))
                {
                    throw new Exception("Tenant is missing !!!");
                }
                int tenant ;
                if (!int.TryParse(stenant,out tenant))
                {
                    throw new Exception("Tenant is not int  !!!");
                }*/

                int tenant = this.ResolvedTenant();
                if (tenant == null)
                {
                    throw new Exception("Tenant is missing !!!");
                }
                this._Unifreight_L2US01RequestParam = new Unifreight_L2US01RequestParam()
                {
                    Tenant = tenant,
                    LoggingEntityId = UnifreightListsUtil.GetValue(ref dic, "DeclarationId"),
                    DeclarationId = UnifreightListsUtil.GetValue(ref dic, "DeclarationId"),
                    CCUFILEmFileNo = UnifreightListsUtil.GetValue(ref dic, "CCUFILEMFileNo"),
                    CFIFILEMFileNo = UnifreightListsUtil.GetValue(ref dic, "CFIFILEMFileNo"),
                    LoggingUserId = AuthenticationUtil.ResolveUserId(tenant)
                    //CCUFILEmFileNo = UnifreightListsUtil.GetValue(ref dic, "CCUFILEmFileNo"),
                };

            
        }

        



        public override string GetAssemblyQualifiedName()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataIn1()
        {
            
            return 
@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<ArrayOfEntry xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
 <Entry>
  <Key>DeclarationId</Key>
  <Value>1-2482</Value>
 </Entry>
 <Entry>
  <Key>LoggingUserId</Key>
  <Value>MORAN</Value>
 </Entry>
 <Entry>
  <Key>CFIFILEMFileNo</Key>
  <Value>616200307</Value>
 </Entry>
 <Entry>
  <Key>CCUFILEMFileNo</Key>
  <Value>50012482</Value>
 </Entry>
</ArrayOfEntry>
";
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

