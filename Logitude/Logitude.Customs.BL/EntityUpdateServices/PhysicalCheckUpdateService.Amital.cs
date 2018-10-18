using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class PhysicalCheckUpdateService
    {
        private void UpdateUnifreight(PhysicalCheckPM dirtyEntityPM)
        {
            var toSendStatusPCE = false;
            var toSendStatusPCF = false;
            var toSendStatusPUI = false;
            var toSendStatusPUC = false;

            DeclarationPM connectedDeclarationPM = null;
            bool toLoadDeclarationPM = false;

            string loggingUserId = AuthenticationUtil.ResolveUserId(dirtyEntityPM.Tenant);

            var dbOccPhysicalCheckPM = GetDBEntity(dirtyEntityPM);
            var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
            //<--- Yuval Chalup 08.03.2016 TASK-20003 - Check if this is a Multi Status EventContextTagModel
            var eventContextTagModelList = dirtyEntityPM.CurrentContextTag as List<EventContextTagModel>;
            if (eventContextTagModelList != null)
            {
                eventContextTagModel = eventContextTagModelList.FirstOrDefault();
            }
            //Yuval Chalup 08.03.2016 TASK-20003 --->
            if (eventContextTagModel != null)
            {
                switch (eventContextTagModel.CallProccessID)
                {
                    case EventContextTagModel.ProccessEnum.None:
                        break;
                    case EventContextTagModel.ProccessEnum.CH_NG_196_MSG7_CargoExitFromCheckSiteResponseServiceUpdate:
                        toSendStatusPCE = true;
                        break;
                    case EventContextTagModel.ProccessEnum.CH_NG_190_MSG1_NoticeToClientResponseServiceInsert:
                        toSendStatusPCF = true;
                        break;
                    case EventContextTagModel.ProccessEnum.CH_NG_190_MSG1_NoticeToClientResponseServiceUpdate:
                        toSendStatusPUI = true;
                        break;
                    case EventContextTagModel.ProccessEnum.CH_NG_190_MSG1_NoticeToClientResponseServiceDelete:
                        toSendStatusPUC = true;
                        break;
                    default:
                        break;
                }
            }

            toLoadDeclarationPM = (toSendStatusPUI || toSendStatusPCE || toSendStatusPCF || toSendStatusPUC);
            if (toLoadDeclarationPM)
            {
                connectedDeclarationPM = GetConnectedDeclarationPM(dirtyEntityPM);
                if (connectedDeclarationPM == null || string.IsNullOrWhiteSpace(connectedDeclarationPM.CustomFileNo))
                {
                    return;
                }
            }
            
            if (toSendStatusPUI)
            {
                SendPUI(dirtyEntityPM, connectedDeclarationPM, loggingUserId);
            }
            if (toSendStatusPCE)
            {
                SendPCE(dirtyEntityPM, connectedDeclarationPM, loggingUserId);
            }
            if (toSendStatusPCF)
            {
                SendPCF(dirtyEntityPM, connectedDeclarationPM, loggingUserId);
            }
            if (toSendStatusPUC)
            {
                SendPUC(dirtyEntityPM, connectedDeclarationPM, loggingUserId);
            }

        }

        private void SendPUC(PhysicalCheckPM dirtyEntityPM, DeclarationPM connectedDeclarationPM, string loggingUserId)
        {
            try
            {
                var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyEntityPM.Tenant,
                    objectTableName = "Customs.PhysicalCheck",
                    EventCode = "PUC",
                    notes = eventContextTagModel.EventRemarks,
                    CommunicationLoggingEntityReference = dirtyEntityPM.CheckId.ToString(),
                    EntityId = dirtyEntityPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status PUC from logitude ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = connectedDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = "PUC",
                        status_DateTime = DateTime.Now,
                        //status_place = "FRA",
                        //status_save = "no_fail",
                        comments = eventContextTagModel.FUStatusRemarks,
                    }
                };
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);
            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }

        private void SendPCF(PhysicalCheckPM dirtyEntityPM, DeclarationPM connectedDeclarationPM, string loggingUserId)
        {
            try
            {
                var eventContextTagModelList = dirtyEntityPM.CurrentContextTag as List<EventContextTagModel>;
                if (eventContextTagModelList != null)
                {
                    SendMultiStatus(dirtyEntityPM, connectedDeclarationPM, loggingUserId);
                    return;
                }
                var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyEntityPM.Tenant,
                    objectTableName = "Customs.PhysicalCheck",
                    EventCode = "PCF",
                    notes = eventContextTagModel.EventRemarks,
                    CommunicationLoggingEntityReference = dirtyEntityPM.CheckId.ToString(),
                    EntityId = dirtyEntityPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status PCF from logitude ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = connectedDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = "PCF",
                        status_DateTime = DateTime.Now,
                        //status_place = "FRA",
                        //status_save = "no_fail",
                        comments = eventContextTagModel.FUStatusRemarks,
                    }
                };
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);
            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }

        private static void SendPCE(PhysicalCheckPM dirtyEntityPM, DeclarationPM connectedDeclarationPM, string loggingUserId)
        {
            try
            {
                //<--- Yuval Chalup 08.03.2016 TASK-19919 - Check if this is a Multi Status EventContextTagModel
                var eventContextTagModelList = dirtyEntityPM.CurrentContextTag as List<EventContextTagModel>;
                if (eventContextTagModelList != null)
                {
                    SendMultiStatus(dirtyEntityPM, connectedDeclarationPM, loggingUserId);
                    return;
                }
                //Yuval Chalup 08.03.2016 TASK-19919 --->

                var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyEntityPM.Tenant,
                    objectTableName = "Customs.PhysicalCheck",
                    EventCode = "PCE",
                    notes = eventContextTagModel.EventRemarks,
                    CommunicationLoggingEntityReference = dirtyEntityPM.CheckId.ToString(),
                    EntityId = dirtyEntityPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status PCE from logitude ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = connectedDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = "PCE",
                        status_DateTime = DateTime.Now,
                        //status_place = "FRA",
                        //status_save = "no_fail",
                        comments = eventContextTagModel.FUStatusRemarks,
                    }
                };
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);
            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }

        private static void SendPUI(PhysicalCheckPM dirtyEntityPM, DeclarationPM connectedDeclarationPM, string loggingUserId)
        {
            try
            {
#if (false)
                {
                    var myPhysicalCheckPUI = new PhysicalCheckPUI(new PhysicalCheckPUIModel() { physicalCheckPM = dirtyEntityPM, connectedDeclarationPM = connectedDeclarationPM });
                    var communicationInfo = myPhysicalCheckPUI.Send();
                }
#endif

                //var objectTableRepository = new ObjectTabelRepository(dirtyEntityPM.Tenant);
                //var objectTable = objectTableRepository.GetObjectTableByName("Customs.PhysicalCheck", 0, true);


                //var UserID=Logitude.Customs.BL.Utils.ContextUtil.GetIdentityName();
                //EventTracer.CreateTraceEvent(new Simplog.Data.InfrastructureModel.EntityPOCOs.TraceEvent(), "UPDT", entityPM.Tenant, contact.Id, entityPM.Id, "Update Declaration", "Customs.Declaration", null, null, false);


                //<--- Yuval Chalup 08.03.2016 TASK-20003 - Check if this is a Multi Status EventContextTagModel
                var eventContextTagModelList = dirtyEntityPM.CurrentContextTag as List<EventContextTagModel>;
                if (eventContextTagModelList != null)
                {
                    SendMultiStatus(dirtyEntityPM, connectedDeclarationPM, loggingUserId);
                    return;
                }
                //Yuval Chalup 08.03.2016 TASK-20003 --->

                var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {

                    Tenant = dirtyEntityPM.Tenant,
                    objectTableName = "Customs.PhysicalCheck",
                    EventCode = "PUI",
                    notes = "Physical Check",
                    CommunicationLoggingEntityReference = dirtyEntityPM.CheckId,
                    //gccentity // gdmentity                             
                    EntityId = dirtyEntityPM.Id,
                    //jalal
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status PUI from logitude ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = connectedDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = "PUI",
                        status_DateTime = DateTime.Now,
                        //status_place = "FRA",
                        //status_save = "no_fail",
                        comments = eventContextTagModel.FUStatusRemarks,
                    }
                };

                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);

            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }

        //<--- Yuval Chalup 08.03.2016 TASK-20003
        private static void SendMultiStatus(PhysicalCheckPM dirtyEntityPM, DeclarationPM connectedDeclarationPM, string loggingUserId)
        {
            try
            {
                var eventContextTagModelList = dirtyEntityPM.CurrentContextTag as List<EventContextTagModel>;
                if (eventContextTagModelList == null)
                {
                    return;
                }

                foreach (var eventContextTagModel in eventContextTagModelList)
                {
                    var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                    {

                        Tenant = dirtyEntityPM.Tenant,
                        objectTableName = "Customs.PhysicalCheck",
                        EventCode = eventContextTagModel.EventCode,
                        notes = "Physical Check",
                        CommunicationLoggingEntityReference = dirtyEntityPM.CheckId,
                        EntityId = dirtyEntityPM.Id,
                        UserId = loggingUserId,

                        CommunicationSubject = "FU Status " + eventContextTagModel.EventCode + " from logitude ",
                        MyFUStatus = new AmitalEventTracerModel.FUStatus()
                        {
                            entname = "CFIFILEM",
                            primary_number = connectedDeclarationPM.CustomFileNo,
                            status = "new",
                            xml_status = "new",
                            status_id = eventContextTagModel.FUStatusCode,
                            status_DateTime = DateTime.Now,
                            //status_save = "no_fail",
                            comments = eventContextTagModel.FUStatusRemarks,
                        }
                    };
                    if (string.IsNullOrWhiteSpace(myAmitalEventTracerModel.MyFUStatus.status_id))
                    {
                        myAmitalEventTracerModel.MyFUStatus.status_id = eventContextTagModel.EventCode;
                    }

                    AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);
                }
            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }   
        //Yuval Chalup 08.03.2016 TASK-20003 -->

        private DeclarationPM GetConnectedDeclarationPM(PhysicalCheckPM dirtyEntityPM)
        {
            var declarationQueryService = new Logitude.Customs.BL.EntityQueryServices.DeclarationQueryService(dirtyEntityPM.Tenant);
            return declarationQueryService.GetSingle(dirtyEntityPM.DeclarationId, false, false);
        }
        private PhysicalCheckPM GetDBEntity(PhysicalCheckPM dirtyEntityPM)
        {
            var qService = new Logitude.Customs.BL.EntityQueryServices.PhysicalCheckQueryService(dirtyEntityPM.Tenant);
            return qService.GetSingle(dirtyEntityPM.Id, true, false) ??new PhysicalCheckPM();

        }

    }
}
