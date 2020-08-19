
using Logitude.AmitalMessaging.Infrastructure.FuStatus;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Def.Messaging.Customs;
///using Logitude.Customs.BL.Messaging.Amital.FuStatus;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.TraceEvents
{
    public class AmitalEventTracer
    {


        public static void CreateTraceEvent(AmitalEventTracerModel myAmitalEventTracer, bool suppressSendToUniFreight = false, bool suppress_RAISE_EVENT = false, bool iscustomUser=false)
        {
            try
            {

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

                if (!mySetting.IsConnectedToUniFreight && mySetting.UnfConnectionString == null)
                {
                    return;
                }
                if (myAmitalEventTracer.NotConnectedToUniface)
                {
                    return;
                }
                //EventTracer.CreateTraceEvent(new TraceEvent(), "CRTR", entityPM.Tenant, loggedContact.Id, entityPM.Id, null, "Trucker", null, null, false);
                if (myAmitalEventTracer.MyFUStatus == null)//itzik
                {
                    throw new BusinessErrorException("NO DATA TO SEND FU/Status INTERFACE to Amital !! (myAmitalEventTracer.MyFUStatus == null)");
                }

                var myFUStatus = GetFUStatus(myAmitalEventTracer, iscustomUser:  iscustomUser);
                var myUServerCommunicationService = new Logitude.Customs.BL.Messaging.Amital.UServerCommunicationService<AmitalEventTracerModel, GFUSTS>(myAmitalEventTracer, myFUStatus);
                myUServerCommunicationService.Send();

            }
            finally
            {
                
                if (myAmitalEventTracer.MyUnifreightEventParam!=null && myAmitalEventTracer.MyUnifreightEventParam.IsValid())
                {

                    var myOpenUnifreighTask = new UnifreightEventTaskService();
                    myOpenUnifreighTask.UpsertEventLE2U(
                        myAmitalEventTracer.Tenant,
                        myAmitalEventTracer.UserId,
                        myAmitalEventTracer.MyUnifreightEventParam);
                }

            }

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
                    unfreightUserId = "MEHES"; // change from "AMITAL"
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
