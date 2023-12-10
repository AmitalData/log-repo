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
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Customs.Data.Repsitories;
using DocumentFormat.OpenXml.InkML;

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


            FeatureQuery featureQuery = new FeatureQuery();
            var features = featureQuery.GetAllowedFeaturesForLoggedUser(AuthenticationUtil.ResolveUserId(requestParams.Tenant), requestParams.Tenant);
            var feature = features.Features.FirstOrDefault(x => x.Code == "EntryExit");
            if (customResponse.General.exitEntryEventType==1 && feature != null)
            {
                //איתור הצהרה
                var cargoTypeCode = customResponse.ReportingDetails.cargoIdentifier.cargoIdentifierType;
                var manifestNumber = customResponse.ReportingDetails.cargoIdentifier.cargoIdentifierKey1;
                var secondCargoID = customResponse.ReportingDetails.cargoIdentifier.cargoIdentifierKey2;
                var thirdCargoID = customResponse.ReportingDetails.cargoIdentifier.cargoIdentifierKey3;

                var declarationQueryService = new DeclarationQueryService(myDbContext);
                var declaration = declarationQueryService.GetDeclarationByConsignment(cargoTypeCode.ToString(), manifestNumber, secondCargoID, thirdCargoID);
                if (declaration != null && declaration.Id != null)
                {
                    this.MyResponseData.UserMessage = " התקבל מסר יציאה ממסוף " + declaration.CustomFileNo;
                    this.MyRequestSheetParam = new RequestSheetParam();
                    this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                    this.MyRequestSheetParam.RequestDescription = " התקבל מסר יציאה ממסוף " + declaration.CustomFileNo;



                    this.MyResponseData.ApplicationID = declaration.Id;
                    this.MyRequestSheetParam.EntityId1 = declaration.Id;
                    this.MyRequestSheetParam.CustomFileNo = declaration.CustomFileNo ;


                    DeliverySiteTypeQueryService deliverySiteTypeQueryService = new DeliverySiteTypeQueryService(requestParams.Tenant);
                    var deliverySiteType = deliverySiteTypeQueryService.GetSingle(customResponse.ReportingDetails.exitEntrySiteNumber, false, true);
                    var storageSite = deliverySiteType?.LocalName;

                    var commentsStorageSite = !string.IsNullOrEmpty(storageSite) ? " שם אתר: " + storageSite + " " : "";
                    var commentsContainerNumber = !string.IsNullOrEmpty(customResponse.ReportingDetails.containerNumber) ? ", מכולה: " + customResponse.ReportingDetails.containerNumber : "";
                    var commentsExpectedArrivalSiteNumber = !string.IsNullOrEmpty(customResponse.ReportingDetails.expectedArrivalSiteNumber) ? ", אתר הגעה צפוי: " + customResponse.ReportingDetails.expectedArrivalSiteNumber : "";
                    var commentsDriverName = !string.IsNullOrEmpty(customResponse.TransferDetails.driverName) ? ", שם נהג: " + customResponse.TransferDetails.driverName : "";
                    var commentsDriverIdentityNumber = !string.IsNullOrEmpty(customResponse.TransferDetails.driverIdentityNumber) ? ", ת.ז נהג: " + customResponse.TransferDetails.driverIdentityNumber : "";
                    var commentsVehicleNumber = !string.IsNullOrEmpty(customResponse.TransferDetails.vehicleNumber) ? ", מספר משאית: " + customResponse.TransferDetails.vehicleNumber : "";
                    var commentsCargoWeight = customResponse.General.cargoWeight != null ? ", משקל: " + customResponse.General.cargoWeight.ToString() : "";
                    var comments = commentsStorageSite + commentsContainerNumber + commentsExpectedArrivalSiteNumber + commentsDriverName + commentsDriverIdentityNumber + commentsVehicleNumber;
                    DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(declaration.Tenant);
                    string defaultLex =defaultValueQueryService.GetDefault("ISRAEL", "CGG_EXITSTS_PCK", "NON", "NON", declaration.Tenant);
                    // TODO: CHANGE "somename" To real default name from unifreight (#195397- feature number)
                    string defaultLex2 = defaultValueQueryService.GetDefault("ISRAEL", "somename", "NON", "NON", declaration.Tenant);

                    Boolean raiseEvent = true;
                    if(defaultLex != null)
                    {
                        raiseEvent = false;
                        List<ConsignmentPackagePM> consignmentPackages = new ConsignmentPackageQueryService(myDbContext).GetConsignmentPackagesForDeclaration(declaration.Id);
                        foreach(ConsignmentPackagePM consignmentPackage in consignmentPackages)
                        {
                            if (!defaultLex.Contains(consignmentPackage.PackageTypeCode))
                            {
                                raiseEvent = true;
                            }
                        }

                    }
                    if(defaultLex2 != null)
                    {
                        raiseEvent = false;
                    }
                    RaiseEvent(requestParams.Tenant, "EXT", "Exit From Storage Site", declaration, customResponse, comments + commentsCargoWeight);
                    if (raiseEvent)
                    {
                        if (customResponse.ReportingDetails.isLastExiOrLasttEntry == true)

                            RaiseEvent(requestParams.Tenant, "LEX", "Last Exit From Storage Site", declaration, customResponse, comments);
                    }
                    
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




        private void RaiseEvent(int Tanent, string code, string notes, Declaration declaration, CC_MSG1_EntryExitToFromCustomsStorageSitesMessage customResponse, string comments)
        {
            try
            {

                UserRepository userRepository = new UserRepository();
                var user = userRepository.GetSingleUserByCode("MEHES", Tanent, true);
                string loggingUserId="";
                if (user != null)
                {
                    loggingUserId = user.Id;
                }

               
              
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = Tanent,
                    objectTableName = "Customs.Declaration",
                    EventCode = code,
                    notes = notes,
                    CommunicationLoggingEntityReference = declaration.DeclarationNumber,
                    EntityId = declaration.Id,
                    UserId = loggingUserId,
                    CommunicationSubject = "FU Status " + code + " from logitude",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = declaration.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = code,
                        status_DateTime = customResponse.General.entryExitDateTime != null ? customResponse.General.entryExitDateTime : DateTime.Now  ,
                        comments = comments,
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

