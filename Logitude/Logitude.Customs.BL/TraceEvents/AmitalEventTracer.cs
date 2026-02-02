
using Logitude.AmitalMessaging.Infrastructure.FuStatus;
using Logitude.Customs.BL.Messaging.Amital;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Def.Messaging.Customs;
///using Logitude.Customs.BL.Messaging.Amital.FuStatus;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.AmitalMessaging.Utils;
using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityUpdateServices;
using Unifreight.Data.AmitalModel;
using Logitude.Customs.Def.EntityPMs;

namespace Logitude.Customs.BL.TraceEvents
{
    public class AmitalEventTracer
    {
        public const bool UseHybrid_When_NotIsConnectedToUniFreight = true;

        public static void CreateTraceEvent(AmitalEventTracerModel myAmitalEventTracer, bool suppressSendToUniFreight = false, bool suppress_RAISE_EVENT = false, bool iscustomUser = false,bool isExport = false)
        {
            try
            {
                if (myAmitalEventTracer?.MyFUStatus != null)
                {
                    LogMessagingUtil.Instance.AppendLine($"CreateFUStatus:{myAmitalEventTracer.MyFUStatus.status_id}:{myAmitalEventTracer?.MyFUStatus.entname}={myAmitalEventTracer?.MyFUStatus.primary_number}");
                }

                if (myAmitalEventTracer.notes == "DO_NOT_RAISE_EVENT")
                {
                    suppress_RAISE_EVENT = true;
                }
                //if (myAmitalEventTracer.notes != "DO_NOT_RAISE_EVENT") // moran 27.8.15 - Task 4154
                if (!suppress_RAISE_EVENT)
                {
                    //EventTracer.CreateTraceEvent(new TraceEvent(), myAmitalEventTracer.EventCode, myAmitalEventTracer.Tenant, myAmitalEventTracer.UserId, myAmitalEventTracer.EntityId, myAmitalEventTracer.notes, myAmitalEventTracer.objectTableName, myAmitalEventTracer.currentStatusId, myAmitalEventTracer.newStatusId, myAmitalEventTracer.manually);
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = myAmitalEventTracer.Tenant,
                        EventTypeCode = myAmitalEventTracer.EventCode,
                        UserId = myAmitalEventTracer.UserId,
                        EntityId = myAmitalEventTracer.EntityId,
                        ObjectTableName = myAmitalEventTracer.objectTableName,
                        Notes = myAmitalEventTracer.notes,
                        IsAddedManually = myAmitalEventTracer.manually,
                        NewStatusId = myAmitalEventTracer.newStatusId,
                        CurrentStatusId = myAmitalEventTracer.currentStatusId,
                    });
                }

                var mySetting = Logitude.Customs.BL.EntityQueryServices.CustomsSettingQueryService.GetSettingByTenant(myAmitalEventTracer.Tenant);

                if (suppressSendToUniFreight) // Dont Raise UniFreight Status- only event
                {
                    return;
                }

                if (mySetting.StandAlone)
                    return;

  
                if (myAmitalEventTracer.NotConnectedToUniface)//לא מחובר ברמת ההצהרה
                {

                    return;

                }
                //EventTracer.CreateTraceEvent(new TraceEvent(), "CRTR", entityPM.Tenant, loggedContact.Id, entityPM.Id, null, "Trucker", null, null, false);
                if (myAmitalEventTracer.MyFUStatus == null)//itzik
                {
                    throw new BusinessErrorException("NO DATA TO SEND FU/Status INTERFACE to Amital !! (myAmitalEventTracer.MyFUStatus == null)");
                }

                var myFUStatus = GetFUStatus(myAmitalEventTracer, iscustomUser: iscustomUser);
                if (isExport && !mySetting.IsConnectedToUniFreight)
                {

 
                        string queueName = GetQueueNameByUnifreightEntity(myAmitalEventTracer.MyFUStatus.entname);
                        if (!string.IsNullOrWhiteSpace(queueName))
                        {

                            var unifreightHybridQueueTaskService = new UnifreightHybridQueueTaskService<AmitalEventTracerModel, GFUSTS>(myAmitalEventTracer, myFUStatus);
                            unifreightHybridQueueTaskService.Send(new UnifreightHybridQueueTaskParam()
                            {
                                Action = "StatusUpdate",
                                ParameterName = "transmission",
                                UServerDelayTime = myAmitalEventTracer.UServerDelayTime,
                                InterfaceTypeCode = queueName

                            });

                        }

                        else
                        {
                            LogMessagingUtil.Instance.AppendLine($"suppress UnifreightHybridQueueTaskService({myAmitalEventTracer.MyFUStatus.status_id}):expected only MSCSTORAGE/BFIFILE");
                       }
                }
                else
                {
                    AmitalContext context = AmitalContext.GetContext(myAmitalEventTracer.Tenant);
                    Simplog.Server.Infrastructure.ChangeSetOperation insertOperation = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                    CreateYCULTask(context, myAmitalEventTracer, myFUStatus, mySetting, insertOperation);
                    CreateGGGQTask(context, myAmitalEventTracer, mySetting, insertOperation);
                }

            }
            finally
            {

                if (
                    myAmitalEventTracer.MyUnifreightEventParam != null && myAmitalEventTracer.MyUnifreightEventParam.IsValid())
                {

                    var myOpenUnifreighTask = new UnifreightEventTaskService();
                    myOpenUnifreighTask.UpsertEventLE2U(
                        myAmitalEventTracer.Tenant,
                        myAmitalEventTracer.UserId,
                        myAmitalEventTracer.MyUnifreightEventParam);
                }

            }

        }


        // Creates YCULTASK record for sending FU status to Unifreight
        private static void CreateYCULTask(AmitalContext context, AmitalEventTracerModel myAmitalEventTracer, AmitalMessaging.Infrastructure.FuStatus.GFUSTS myFUStatus, CustomsSettingPM mySetting, Simplog.Server.Infrastructure.ChangeSetOperation insertOperation)
        {
            string xml = XmlGenericUtil<AmitalMessaging.Infrastructure.FuStatus.GFUSTS>.SerializeObject(myFUStatus, true);

            Unifreight.BL.EntityPMs.YCULTASKPM myYCULTASKPM = new Unifreight.BL.EntityPMs.YCULTASKPM
            {
                ChangeSetOp = insertOperation,
                STATUS = "W",
                REQUESTDATA = xml,
                ENTNAME = "CFIFILEM",
                PRIMARYNUM = myAmitalEventTracer.MyFUStatus.primary_number,
                PRIORITY = Unifreight.BL.EntityPMs.YCULTASKPM.calcPriority("L2U"),
                TYPE = "L2U",
                USRCODE = myAmitalEventTracer.MyFUStatus.OwnerUnifreightUserCode,
                ARCHIVE = "F"
            };

            if (!mySetting.IsConnectedToUniFreight)
                myYCULTASKPM.Tenant = myAmitalEventTracer.Tenant;

            Unifreight.BL.EntityUpdateServices.YCULTASKUpdateService myYCULTASKUpdateService = new Unifreight.BL.EntityUpdateServices.YCULTASKUpdateService(context);
            myYCULTASKUpdateService.DontAddTransaction = true;
            myYCULTASKUpdateService.Update(myYCULTASKPM, true);

        }

        // Creates GGGQ record for async execution in Unifreight
        private static void CreateGGGQTask(AmitalContext context, AmitalEventTracerModel myAmitalEventTracer, CustomsSettingPM mySetting, Simplog.Server.Infrastructure.ChangeSetOperation insertOperation)
        {
            GGGQPM myGGGQPM = new GGGQPM
            {
                ChangeSetOp = insertOperation,
                ORIGINQUE = "LGT",
                STATUS = "1",
                EXPTASKTIME = 5,
                EXECDATE = DateTime.Now,
                TRY = 9,
                PRIORITY = 8,
                ENTNAME = "CFIFILEM",
                PRIMARYNUM = myAmitalEventTracer.MyFUStatus.primary_number,
                FORMID = "LGT_UPDATE_FCI",
                DEBUG = "F",
                DONEOPERATION = "D"
            };

            if (!mySetting.IsConnectedToUniFreight)
                myGGGQPM.Tenant = myAmitalEventTracer.Tenant;
            GGGQUpdateService myGGGQUpdateService = new GGGQUpdateService(context);
            myGGGQUpdateService.DontAddTransaction = true;
            myGGGQUpdateService.Update(myGGGQPM, true);
        }

        private static string GetQueueNameByUnifreightEntity(string entname)
        {
            string queueName = "";
            switch (entname)
            {
                case "MSCSTORAGE":
                    queueName = "ExportStorageStatus";
                    break;
                case "BFIFILE"://"BFIFILE" : "CFIFILEM",
                    queueName = "ExportDeclarationStatus";
                    break;

                case "CFIFILEM":
                    queueName = "ImportDeclarationStatus";
                    break;

                default:
                    //throw new Exception("GetQueueNameByUnifreightEntity():expected only MSCSTORAGE/BFIFILE");
                    
                    break;
            }

            return queueName;
        }


        public static GFUSTS GetFUStatus(AmitalEventTracerModel myAmitalEventTracer, bool iscustomUser = false)
        {

            var myFUStatus = new GFUSTS();
            var myFollow_up_status = new follow_up_status();
            myFollow_up_status.xml_status = myAmitalEventTracer.MyFUStatus.xml_status;
            myFollow_up_status.status = myAmitalEventTracer.MyFUStatus.status;
            myFollow_up_status.entname = myAmitalEventTracer.MyFUStatus.entname; // = "CFIFILEM";

            //--> Mirit 07/06/15 task 13520
            //string unfreightUserId = AuthenticationUtil.ResolveUnifreightUserId(myAmitalEventTracer.Tenant);
            string unfreightUserId = null;
            if (RequestSheetContext.Current != null)
            {
                var loggingUserIdFromRS = RequestSheetContext.Current.GetContextOrDefault().GetUserFromRequestParam();
                if (!string.IsNullOrWhiteSpace(loggingUserIdFromRS))
                {
                    UserRepository userRep = new UserRepository(myAmitalEventTracer.Tenant);
                    User user = userRep.GetSingleUser(loggingUserIdFromRS, myAmitalEventTracer.Tenant, true);
                    if (user != null)
                    {
                        if (!String.IsNullOrWhiteSpace(user.Code))
                        {
                            unfreightUserId = user.Code;
                        }
                    }
                }
            }
            if (String.IsNullOrWhiteSpace(unfreightUserId) || iscustomUser)
            {
                if (!String.IsNullOrWhiteSpace(myAmitalEventTracer.UserId))
                {
                    //unfreightUserId = AuthenticationUtil.ResolveUnifreightUserById(myAmitalEventTracer.UserId, myAmitalEventTracer.Tenant);
                    var userRepository = new UserRepository(myAmitalEventTracer.Tenant);
                    User myUser = userRepository.GetSingleUser(myAmitalEventTracer.UserId, myAmitalEventTracer.Tenant, true);
                    if (myUser != null)
                    {
                        unfreightUserId = myUser.Code;
                    }
                }
            }
            if (String.IsNullOrWhiteSpace(unfreightUserId))
            {
                unfreightUserId = AuthenticationUtil.ResolveUnifreightUserId(myAmitalEventTracer.Tenant);
                if (String.IsNullOrWhiteSpace(unfreightUserId))
                {
                    unfreightUserId = myAmitalEventTracer.MyFUStatus.OwnerUnifreightUserCode ?? "MEHES"; // change from "AMITAL"
                }
            }

            //<-- Mirit 07/06/15 task 13520
            myFollow_up_status.foll_up_details = new foll_up_details[] {
                    new  foll_up_details()
                    {
                        foll_up_detailsuser = new foll_up_detailsuser[]
                        {
                            new foll_up_detailsuser() { foll_up_detailsid =  unfreightUserId }
                        }
                    }
                };

            myFollow_up_status.primary_number = new primary_number()
            {
                Value = myAmitalEventTracer.MyFUStatus.primary_number  //declarationPM.CustomFileNo
            };


            myFollow_up_status.status_save = myAmitalEventTracer.MyFUStatus.status_save;// "no_fail";
                                                                                        //myFollow_up_status.status_date = myAmitalEventTracer.MyFUStatus.status_DateTime.ToShortDateString();// = DateTime.Now.ToShortDateString();
                                                                                        //myFollow_up_status.status_time = myAmitalEventTracer.MyFUStatus.status_DateTime.ToShortTimeString(); DateTime.Now.ToShortTimeString();
            myFollow_up_status.status_date = myAmitalEventTracer.MyFUStatus.status_DateTime.Date.ToString("dd.MM.yy");
            myFollow_up_status.status_time = myAmitalEventTracer.MyFUStatus.status_DateTime.TimeOfDay.ToString("hh\\:mm");
            myFollow_up_status.status_id = myAmitalEventTracer.MyFUStatus.status_id;// "PUI";

            myFollow_up_status.comments = new comments()
            {
                Value = myAmitalEventTracer.MyFUStatus.comments  //physicalCheckPM.LimitDate.ToString()
            };


            myFollow_up_status.status_place = myAmitalEventTracer.MyFUStatus.status_place;// "FRA";
            if (myFollow_up_status.xml_status == "del")
            {
                myFollow_up_status.reference = new reference[] { new reference() { referencexml = "*ANY*" } };
            }
            myFUStatus.follow_up_status = new follow_up_status[] { myFollow_up_status };

            var myReference_list = new List<reference_list>();


            return myFUStatus;
        }
        //protected  void Trace(DeclarationPM entityPM)
        //{
        //    //this commented code is just for sample you can create a trace event now you only need to add the needed event types.

        //    ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
        //    Contact contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);
        //    EventTracer.CreateTraceEvent(new TraceEvent(), "UPDT", entityPM.Tenant, contact.Id, entityPM.Id, "Update Declaration", "Customs.Declaration", null, null, false);
        //    base.Trace(entityPM);
        //}
    }



}
