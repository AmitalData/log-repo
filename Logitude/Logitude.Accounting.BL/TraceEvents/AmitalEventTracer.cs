
using Logitude.AmitalMessaging.Infrastructure.FuStatus;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.TraceEvents
{
    public class AmitalEventTracer
    {


        public static void CreateTraceEvent(AmitalEventTracerModel myAmitalEventTracer)
        {
            EventTracer.CreateTraceEvent(new TraceEvent(), myAmitalEventTracer.EventCode, myAmitalEventTracer.Tenant, myAmitalEventTracer.UserId, myAmitalEventTracer.EntityId, myAmitalEventTracer.notes, myAmitalEventTracer.objectTableName, myAmitalEventTracer.currentStatusId, myAmitalEventTracer.newStatusId, myAmitalEventTracer.manually);
            var mySetting = Logitude.Accounting.BL.EntityQueryServices.AccountingSettingQueryService.GetSettingByTenant(myAmitalEventTracer.Tenant);
            if (!mySetting.IsConnectedToUniFreight)
            {
                return;
            }
            if (myAmitalEventTracer.MyFUStatus == null)
            {
                throw new BusinessErrorException("NO DATA TO SEND FU/Status INTERFACE to Amital !! (myAmitalEventTracer.MyFUStatus == null)");
            }
            var myFUStatus = GetFUStatus(myAmitalEventTracer);
            var myUServerCommunicationService = new Logitude.Customs.BL.Messaging.Amital.UServerCommunicationService<AmitalEventTracerModel, GFUSTS>(myAmitalEventTracer, myFUStatus);
            myUServerCommunicationService.Send();

        }
        public static GFUSTS GetFUStatus(AmitalEventTracerModel myAmitalEventTracer)
        {

            var myFUStatus = new GFUSTS();
            var myFollow_up_status = new follow_up_status();
            myFollow_up_status.xml_status = myAmitalEventTracer.MyFUStatus.xml_status;
            myFollow_up_status.status = myAmitalEventTracer.MyFUStatus.status;
            myFollow_up_status.entname = myAmitalEventTracer.MyFUStatus.entname;


            if (!AuthenticationUtil.IsResolveUserIdentityNameEqualSystem(myAmitalEventTracer.tenant))
            {
                string unfreightUserId = AuthenticationUtil.ResolveUnifreightUserId(myAmitalEventTracer.tenant);
                if (!String.IsNullOrWhiteSpace(unfreightUserId))
                {
                    myFollow_up_status.foll_up_details = new foll_up_details[] {  
                    new  foll_up_details() 
                    { 
                        foll_up_detailsuser = new foll_up_detailsuser[]  
                        {
                            new foll_up_detailsuser() { foll_up_detailsid =  unfreightUserId }                           
                        }
                    }
                };
                }
            }
            myFollow_up_status.primary_number = new primary_number()
            {
                Value = myAmitalEventTracer.MyFUStatus.primary_number
            };


            myFollow_up_status.status_save = myAmitalEventTracer.MyFUStatus.status_save;// "no_fail";
            myFollow_up_status.status_date = myAmitalEventTracer.MyFUStatus.status_DateTime.ToShortDateString();
            myFollow_up_status.status_time = myAmitalEventTracer.MyFUStatus.status_DateTime.ToShortTimeString(); DateTime.Now.ToShortTimeString();
            myFollow_up_status.status_id = myAmitalEventTracer.MyFUStatus.status_id;// "PUI";

            myFollow_up_status.comments = new comments()
            {
                Value = myAmitalEventTracer.MyFUStatus.comments
            };


            myFollow_up_status.status_place = myAmitalEventTracer.MyFUStatus.status_place;// "FRA";
            myFUStatus.follow_up_status = new follow_up_status[] { myFollow_up_status };

            var myReference_list = new List<reference_list>();


            return myFUStatus;
        }
    }
}
