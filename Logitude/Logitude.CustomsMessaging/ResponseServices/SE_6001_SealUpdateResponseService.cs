using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SealUpdateServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class SE_6001_SealUpdateResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, INF_MSG_Generic, CargoSealsRequestParams>
    {


        public override void OnRequestFail(INF_MSG_Generic customResponse, CargoSealsRequestParams requestParams)
        {
            base.OnRequestFail(customResponse, requestParams);
        }

        public override INF_MSG_GenericResponseData GetResponse(INF_MSG_Generic customResponse, CargoSealsRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(INF_MSG_Generic customResponse, CargoSealsRequestParams requestParams)
        {
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var cargoSealIdentifierQueryService = new CargoSealIdentifierQueryService(dbContext);
            var cargoSealIdentifierUpdateService = new CargoSealIdentifierUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);

            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;

            CargoSealIdentifierPM cargoSealIdentifierPM = cargoSealIdentifierQueryService.GetSingle(requestParams.CargoSealIdentifierId, true, false);
            if (cargoSealIdentifierPM == null)
            {
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception[0].ExeptionDescription;
                return;
            }

            if (customResponse.ResponseContentHeader.Exception != null)
            {
                cargoSealIdentifierPM.Status = "2";
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception[0].ExeptionDescription;


                foreach (var exception in customResponse.ResponseContentHeader.Exception)
                {
                    if(exception.ExeptionType== 13931 && requestParams.DeclarationID!=null)
                    {
                        DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParams.Tenant);

                      var declaration=  declarationQueryService.GetSingleDeclarationById(requestParams.DeclarationID, requestParams.Tenant);

                        if(declaration!= null)
                        {
                            var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                            {
                                Tenant = declaration.Tenant,
                                objectTableName = "Customs.Declaration",
                                EventCode = "SCH",
                                notes = null,
                                CommunicationLoggingEntityReference = declaration.DeclarationNumber,
                                EntityId = declaration.Id,
                                UserId = requestParams.LoggingUserId,

                                CommunicationSubject = "FU Status SCH from logitude ",
                                MyFUStatus = new AmitalEventTracerModel.FUStatus()
                                {
                                    entname = "CFIFILEM",
                                    primary_number = declaration.CustomFileNo,
                                    status = "new",
                                    xml_status = "new",
                                    status_id = "SCH",
                                    status_DateTime = DateTime.Now,
                                    comments = null,
                                }
                            };

                            AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);

                        }

                    }
                }

            }
            else
            {
                cargoSealIdentifierPM.Status = "1";

                cargoSealIdentifierPM.CargoSeals.ForEach(x => { x.UpdateTypeCode = "2"; x.ChangeSetOp = ChangeSetOperation.Update; });
                this.MyResponseData.UserMessage = "התקבלה תשובה תקינה והסגר עודכן";
            }
            cargoSealIdentifierPM.ChangeSetOp = ChangeSetOperation.Update;
            cargoSealIdentifierUpdateService.Update(cargoSealIdentifierPM, true);
        }
    }
}
