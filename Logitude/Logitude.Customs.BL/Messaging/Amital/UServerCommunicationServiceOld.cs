using Logitude.AmitalMessaging.Infrastructure.Transmission;
using Logitude.AmitalMessaging.Utils;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Models;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Amital
{
   public  abstract class UServerCommunicationServiceOld
    {
        CommunicationsParams _CommunicationsParams = null;
        const string _From = "AMITAL"; //please note THE STARDART ANALYZE WANT AMITAL PARTNAER  "Logitude";
        private AmitalCommunicationModelBase _AmitalCommunicationModelBase;
        public UServerCommunicationServiceOld(AmitalCommunicationModelBase amitalCommunicationModelBase)
        {
            _AmitalCommunicationModelBase = amitalCommunicationModelBase;
        }

        public struct Info
        {
            public string CommunicationLogId;
        }
        public Info Send()
        {
            var myInfo = new Info();
            string validationErrors = GetValidationErrors();
            if (!String.IsNullOrWhiteSpace(validationErrors))
            {
                throw new Exception("validationErrors :" + validationErrors);
            }
            Build();




            byte[] myByteData = null;
            _CommunicationsParams = new CommunicationsParams()
            {

                Tenant = GetTenant(),//_PhysicalCheckPM.Tenant,

                LoggingObjectTableId = GetLoggingObjectTableId(),// objectTable.Id,
                LoggingEntityId = GetLoggingEntityId(),//_PhysicalCheckPM.Id,

                Subject = GetSubject(),// "FU Status",
                LoggingEntityReference = GetLoggingEntityReference(),//_PhysicalCheckPM.CheckId,
                LoggingUserId = GetLoggingUserId(),//loggingUserId,


                Status = "W",
                To = "Unifreight",
                CommunicationLogTypeCode = "T",
                FolderName = "Amital",
                From = _From , //"Logitude",
                InOut = "O",

                //XMLData=some xml data string 
            };

            var mytransmission = GetTransmission();
            var mySerilazetransmission = XmlGenericUtil<transmission>.SerilazeObject(mytransmission, true);

            //Debug.WriteLine(mySerilazeObject);
            myByteData = Encoding.ASCII.GetBytes(mySerilazetransmission);
            _CommunicationsParams.ByteData = myByteData;
            string communicationLogId = Communications.AddCommunicationLog(_CommunicationsParams);
            Communications.SendCommunicationLogMessageToQueue("Amitalqueue", communicationLogId, _CommunicationsParams.Tenant
                ///, (AmitalStandardCommunicationModel)this._AmitalCommunicationModelBase
                );
            myInfo.CommunicationLogId = communicationLogId;
            return myInfo;
        }

        

        protected abstract string GetValidationErrors();
        protected abstract void Build();
        protected abstract List<data> GetDataList();
        

        //protected abstract string GetSubject();
        //protected abstract string GetLoggingUserId();
        //protected abstract string GetLoggingEntityReference();
        //protected abstract string GetLoggingEntityId();
        //protected abstract int GetTenant();
        //protected abstract string GetLoggingObjectTableId();


        protected virtual string GetSubject()
        {
            return _AmitalCommunicationModelBase.CommunicationSubject;
        }

        protected virtual string GetLoggingUserId()
        {
            return _AmitalCommunicationModelBase.UserId;
        }

        protected virtual string GetLoggingEntityReference()
        {
            return _AmitalCommunicationModelBase.CommunicationLoggingEntityReference;
        }

        protected virtual string GetLoggingEntityId()
        {
            return _AmitalCommunicationModelBase.EntityId;
        }

        protected virtual int GetTenant()
        {
            return _AmitalCommunicationModelBase.Tenant;
        }

        protected virtual string GetLoggingObjectTableId()
        {

            var objectTableRepository = new ObjectTabelRepository(GetTenant());
            var objectTable = objectTableRepository.GetObjectTableByName(_AmitalCommunicationModelBase.objectTableName,// "Customs.PhysicalCheck", 
                0, true);

            return objectTable.Id;
        }

        protected virtual transmission GetTransmission()
        {
            return GetTransmission(GetDataList(), //_CommunicationsParams.From,
                _CommunicationsParams.Subject);

        }
         
        protected transmission GetTransmission(
            List<data> myListdata ,// string  CommunicationsParamsFrom, 
            string CommunicationsParamsSubject)
        {
            var mytransmission = new transmission();
            var mytransmission_details = new List<transmission_details>();
            var mytransmission_detail1 = new transmission_details()
            {
                sender = new sender() { Value = _From },
                subject = new subject() { Value = CommunicationsParamsSubject }
            };

            var myArrayOfdata = new List<data>();



            mytransmission.data = myListdata.ToArray();// GetDataList().ToArray();
            if (mytransmission.data.Count() < 1)
            {
                throw new Exception("(mytransmission.data.Count < 1)");
            }

            mytransmission_details.Add(mytransmission_detail1);
            mytransmission.transmission_details = mytransmission_details.ToArray();
            return mytransmission;

        }
        
        

        
    }
}
