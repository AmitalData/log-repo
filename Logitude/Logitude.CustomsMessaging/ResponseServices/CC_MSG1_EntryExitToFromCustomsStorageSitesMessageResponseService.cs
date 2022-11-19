using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using UnifreightIIG.Common.MessageLib.Claim;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.MessageLib.EntryExit;
using Logitude.Customs.BL.TraceEvents;
using System;
using Simplog.Data.CommonDataModel.Repositories;
using System.Data.Entity;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityMapping;
using System.Runtime.Remoting.Contexts;
using System.Data.Entity.Infrastructure;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class CC_MSG1_EntryExitToFromCustomsStorageSitesMessageResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, CC_MSG1_EntryExitToFromCustomsStorageSitesMessage, GenericRequestParams>
    {
        public override INF_MSG_GenericResponseData GetResponse(CC_MSG1_EntryExitToFromCustomsStorageSitesMessage customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(CC_MSG1_EntryExitToFromCustomsStorageSitesMessage customResponse, GenericRequestParams requestParams)
        {
             ICustomContext myDbContext = CustomContext.GetContext(requestParams.Tenant);
          
            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;

         

            if (customResponse.General.exitEntryEventType==1)
            {
                //איתור הצהרה
                var cargoTypeCode = customResponse.ReportingDetails.cargoIdentifier.cargoIdentifierType;
                var manifestNumber = customResponse.ReportingDetails.cargoIdentifier.cargoIdentifierKey1;
                var secondCargoID = customResponse.ReportingDetails.cargoIdentifier.cargoIdentifierKey2;
                var thirdCargoID = customResponse.ReportingDetails.cargoIdentifier.cargoIdentifierKey3;

                var declarationQueryService = new DeclarationQueryService(myDbContext);
                var declaration = declarationQueryService.GetDeclarationByConsignment(cargoTypeCode.ToString(), manifestNumber, secondCargoID, thirdCargoID);
                if (declaration.Id !=null)
                {
                    this.MyResponseData.UserMessage = " התקבל מסר יציאה ממסוף " + declaration.CustomFileNo;
                    this.MyRequestSheetParam = new RequestSheetParam();
                    this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                    this.MyRequestSheetParam.RequestDescription = " התקבל מסר יציאה ממסוף " + declaration.CustomFileNo;



                    this.MyResponseData.ApplicationID = declaration.Id;
                    this.MyRequestSheetParam.EntityId1 = declaration.Id;
                    this.MyRequestSheetParam.CustomFileNo = declaration.CustomFileNo ;
                    SendEXT(requestParams.Tenant, declaration, customResponse);
                }
                else
                {
                    this.MyResponseData.UserMessage = "לא נמצאה הצהרה";
                }


            }

            else
            {
                this.MyResponseData.UserMessage = "exitEntryEventType!=1";
            }

        }

        private void SendEXT(int Tanent, Declaration declaration, CC_MSG1_EntryExitToFromCustomsStorageSitesMessage customResponse)
        {
            try
            {

                //ICustomContext dbContext = CustomContext.GetContext(Tanent);

                UserRepository userRepository = new UserRepository();
                var user = userRepository.GetSingleUserByCode("MEHES", Tanent, true);
                string loggingUserId="";
                if (user != null)
                {
                    loggingUserId = user.Id;
                }

                DeliverySiteTypeQueryService deliverySiteTypeQueryService = new DeliverySiteTypeQueryService(Tanent);
                var deliverySiteType = deliverySiteTypeQueryService.GetSingle(customResponse.ReportingDetails.exitEntrySiteNumber, false, true);
                var storageSite = deliverySiteType?.LocalName;

                //var declarationQueryService = new DeclarationQueryService(dbContext);
                //DeclarationPM connectedDeclarationPM = declarationQueryService.GetSingle(declaration.Id, false, false);
                //if (connectedDeclarationPM.IsCourierDeclaration) return;
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = Tanent,
                    objectTableName = "Customs.Declaration",
                    EventCode = "EXT",
                    notes = "Exit from storage site",
                    CommunicationLoggingEntityReference = declaration.DeclarationNumber,
                    EntityId = declaration.Id,
                    UserId = loggingUserId,
                    //   UServerDelayTime = TimeSpan.FromMinutes(5),
                    CommunicationSubject = "FU Status EXT from logitude",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = declaration.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = "EXT",
                        status_DateTime = DateTime.Now,
                        //status_place = "FRA",
                        //status_save = "no_fail",
                        comments = "תאריך שעה הסטטוס:" + customResponse.General.entryExitDateTime + storageSite != null? ", שם אתר:" + storageSite : "" + customResponse.ReportingDetails.containerNumber != null ? ", מכולה:" + customResponse.ReportingDetails.containerNumber : "",
                        //מספ]ר מכולה  + אתר אחסון לשלוף מטבלת מכס , לקחת מהקאש
                    }
                };
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);
            }
            catch (System.Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }





    }
}

