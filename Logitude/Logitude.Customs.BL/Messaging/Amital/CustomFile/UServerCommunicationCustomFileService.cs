using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Utils;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Amital.CustomFile
{
    public class UServerCommunicationCustomFileService : UServerCommunicationServiceOld
    {
        protected AmitalCustomFileCommunicationModel _AmitalCustomFileCommunicationModel;

        public UServerCommunicationCustomFileService(AmitalCustomFileCommunicationModel amitalCustomFileCommunicationModel)
            :base(   amitalCustomFileCommunicationModel)
        {
            this._AmitalCustomFileCommunicationModel = amitalCustomFileCommunicationModel;
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

            //var myGFUSTS = GetFUStatus();
            var myDataList = new List<AmitalMessaging.Infrastructure.Transmission.data>();
            var mySerilazeObject = XmlGenericUtil<LOGICUSTFILE>.SerilazeObject(_AmitalCustomFileCommunicationModel.LogitudeFile, true);

            Debug.WriteLine(mySerilazeObject);
            myDataList.Add(new AmitalMessaging.Infrastructure.Transmission.data() { entity = mySerilazeObject });
            var myTransmission = GetTransmission(myDataList, _AmitalCustomFileCommunicationModel.CommunicationSubject);
            return myTransmission;
        }
    }
}
