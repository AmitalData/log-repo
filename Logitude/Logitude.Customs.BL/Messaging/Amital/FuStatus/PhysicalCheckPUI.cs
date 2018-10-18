using Logitude.AmitalMessaging.Infrastructure.FuStatus;
using Logitude.AmitalMessaging.Utils;
using Logitude.Server.Tools;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Amital.FuStatus
{
    class PhysicalCheckPUIModel :AmitalCommunicationModelBase
    {
        public PhysicalCheckPUIModel()
            :base( OperationMethod.AnalyzeStandard ,"","")
        {

        }
        public EntityPMs.PhysicalCheckPM physicalCheckPM;
        public EntityPMs.DeclarationPM connectedDeclarationPM;
    }
    class PhysicalCheckPUI : UServerCommunicationServiceOld
    {
        EntityPMs.PhysicalCheckPM _PhysicalCheckPM;
        EntityPMs.DeclarationPM _ConnectedDeclarationPM;
        private string _LoggingObjectTableId;
        private List<AmitalMessaging.Infrastructure.Transmission.data> _DataList;
        public PhysicalCheckPUI(PhysicalCheckPUIModel curr)
            :base(curr)
        {}


    

        private string ResolveLoggingUserID()
        {
            // TODO: ResolveLoggingUserID from PM
            return "1-2"; //jalal ??
        }

        protected override string GetValidationErrors()
        {
            return "";
        }

        protected override void Build()
        {
            var objectTableRepository = new ObjectTabelRepository(_PhysicalCheckPM.Tenant);
            var objectTable = objectTableRepository.GetObjectTableByName("Customs.PhysicalCheck", 0, true);
            this._LoggingObjectTableId = objectTable.Id;



            this._GFUSTS = GetFUStatus();
            _DataList = new List<AmitalMessaging.Infrastructure.Transmission.data>(); 
            var mySerilazeObject = XmlGenericUtil<GFUSTS>.SerilazeObject(_GFUSTS, true);

            Debug.WriteLine(mySerilazeObject);
            _DataList.Add(new AmitalMessaging.Infrastructure.Transmission.data() { entity = mySerilazeObject });



        }

        public GFUSTS GetFUStatus()
        {
            var physicalCheckPM = this._PhysicalCheckPM ;
            var declarationPM = this._ConnectedDeclarationPM;


            var myFUStatus = new GFUSTS();
            var myFollow_up_status = new follow_up_status();
            myFollow_up_status.xml_status = myFollow_up_status.status = "new";
            myFollow_up_status.entname = "CFIFILEM";
            myFollow_up_status.primary_number = new primary_number() { Value = declarationPM.CustomFileNo };
            myFollow_up_status.status = "new";

            myFollow_up_status.status_save = "no_fail";
            myFollow_up_status.status_date = DateTime.Now.ToShortDateString();
            myFollow_up_status.status_time = DateTime.Now.ToShortTimeString();
            myFollow_up_status.status_id = "PUI";

            myFollow_up_status.comments = new comments()
            {
                Value = physicalCheckPM.LimitDate.ToString()
            };


            myFollow_up_status.status_place = "FRA";
            myFUStatus.follow_up_status = new follow_up_status[] { myFollow_up_status };

            var myReference_list = new List<reference_list>();

            
            return myFUStatus;
        }

        protected override string GetSubject()
        {
            return "FU Status";
        }

        protected override string GetLoggingUserId()
        {
            return ResolveLoggingUserID();
        }

        protected override string GetLoggingEntityReference()
        {
            return _PhysicalCheckPM.CheckId;   
        }

        protected override string GetLoggingEntityId()
        {
            return _PhysicalCheckPM.Id;
        }

        

        protected override int GetTenant()
        {
            return _PhysicalCheckPM.Tenant;
        }

        protected override string GetLoggingObjectTableId()
        {
            return _LoggingObjectTableId;
        }

        public GFUSTS _GFUSTS { get; set; }

        protected override List<AmitalMessaging.Infrastructure.Transmission.data> GetDataList()
        {
            return _DataList;
        }
    }
}
