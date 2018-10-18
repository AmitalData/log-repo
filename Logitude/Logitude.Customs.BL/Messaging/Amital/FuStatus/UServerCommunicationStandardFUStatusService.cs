using Logitude.AmitalMessaging.Infrastructure.FuStatus;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.TraceEvents;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Amital.FuStatus
{
  
   
    
    public class UServerCommunicationStandardFUStatusService : UServerCommunicationServiceOld
    {
        private AmitalEventTracerModel _AmitalEventTracerModel;

        public UServerCommunicationStandardFUStatusService(AmitalEventTracerModel amitalEventTracerModel)
            : base(amitalEventTracerModel)
        {
            this._AmitalEventTracerModel = amitalEventTracerModel;
        }
        

        protected override string GetValidationErrors()
        {
            return "";
        }

        protected override void Build()
        {
            
        }

        protected override List<AmitalMessaging.Infrastructure.Transmission.data> GetDataList()
        {
            return null;
        }

        
        protected override AmitalMessaging.Infrastructure.Transmission.transmission GetTransmission()
        {
            var myGFUSTS = GetFUStatus();
            var myDataList = new List<AmitalMessaging.Infrastructure.Transmission.data>();
            var mySerilazeObject = XmlGenericUtil<GFUSTS>.SerilazeObject(myGFUSTS, true);

            Debug.WriteLine(mySerilazeObject);
            myDataList.Add(new AmitalMessaging.Infrastructure.Transmission.data() { entity = mySerilazeObject });
            return GetTransmission(myDataList, _AmitalEventTracerModel.CommunicationSubject);
        }
        public GFUSTS GetFUStatus()
        {
            
            var myFUStatus = new GFUSTS();
            var myFollow_up_status = new follow_up_status();
            myFollow_up_status.xml_status = this._AmitalEventTracerModel.MyFUStatus.xml_status;
            myFollow_up_status.status = this._AmitalEventTracerModel.MyFUStatus.status;
            myFollow_up_status.entname = this._AmitalEventTracerModel.MyFUStatus.entname; // = "CFIFILEM";
            myFollow_up_status.primary_number = new primary_number()
            {
                Value = this._AmitalEventTracerModel.MyFUStatus.primary_number  //declarationPM.CustomFileNo
            };
            

            myFollow_up_status.status_save  = this._AmitalEventTracerModel.MyFUStatus.status_save;// "no_fail";
            myFollow_up_status.status_date = this._AmitalEventTracerModel.MyFUStatus.status_DateTime.ToShortDateString();// = DateTime.Now.ToShortDateString();
            myFollow_up_status.status_time = this._AmitalEventTracerModel.MyFUStatus.status_DateTime.ToShortTimeString(); DateTime.Now.ToShortTimeString();
            myFollow_up_status.status_id = this._AmitalEventTracerModel.MyFUStatus.status_id;// "PUI";

            myFollow_up_status.comments = new comments()
            {
                Value = this._AmitalEventTracerModel.MyFUStatus.comments  //physicalCheckPM.LimitDate.ToString()
            };


            myFollow_up_status.status_place = this._AmitalEventTracerModel.MyFUStatus.status_place;// "FRA";
            myFUStatus.follow_up_status = new follow_up_status[] { myFollow_up_status };

            var myReference_list = new List<reference_list>();


            return myFUStatus;
        }
    }
}
