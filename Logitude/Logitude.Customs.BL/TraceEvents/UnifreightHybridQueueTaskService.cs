
using Logitude.AmitalMessaging.Infrastructure.FuStatus;
using Logitude.Customs.BL.Messaging.Amital;
using Logitude.Server.Tools;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.AmitalMessaging.Infrastructure.Transmission;
using Logitude.AmitalMessaging.Utils;
using System.Diagnostics;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using System.Web;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Customs.Data.EntityPOCOs;

namespace Logitude.Customs.BL.TraceEvents
{
    public class UnifreightHybridQueueTaskService<CommunicationModelType, TransmissionBodyType>
        where TransmissionBodyType : class
        where CommunicationModelType : AmitalCommunicationModelBase

    {
        private CommunicationModelType _CommunicationModel;
        private TransmissionBodyType _TransmissionBodyModel;
        private CommunicationsParams _CommunicationsParams;

        public UnifreightHybridQueueTaskService(CommunicationModelType myAmitalEventTracer, TransmissionBodyType myFUStatus)
        {
            this._CommunicationModel = myAmitalEventTracer;
            this._TransmissionBodyModel = myFUStatus;
        }
        protected virtual string GetLoggingObjectTableId()
        {
            if (String.IsNullOrWhiteSpace(_CommunicationModel.objectTableName)) return "";//not must 
            var objectTableRepository = new ObjectTableRepository(0); // ObjectTabelRepository tenant must be zero !!
            var objectTable = objectTableRepository.GetObjectTableByName(_CommunicationModel.objectTableName,// "Customs.PhysicalCheck", 
                0, true);

            return objectTable.Id;
        }
        protected virtual string GetSubject()
        {
            return _CommunicationModel.CommunicationSubject;
        }

        protected virtual string GetLoggingUserId()
        {
            return _CommunicationModel.UserId;
        }

        protected virtual string GetLoggingEntityReference()
        {
            return _CommunicationModel.CommunicationLoggingEntityReference;
        }

        protected virtual string GetLoggingEntityId()
        {
            return _CommunicationModel.EntityId;
        }
        transmission GetTransmission()
        {

            var CommunicationsParamsSubject = _CommunicationsParams.Subject;
            var mytransmission = new transmission();
            var mytransmission_details = new List<transmission_details>();
            var mytransmission_detail1 = new transmission_details()
            {
                //sender = new sender() { Value = $"Mehes Cloud ({_CommunicationModel.Tenant})" },
                sender = new sender() { Value = $"HYBRID" },
                subject = new subject() { Value = CommunicationsParamsSubject },
            };
            if (_CommunicationModel.special_instruction != null)
            {
                mytransmission_detail1
                    .special_instructions = new special_instructions[] { _CommunicationModel.special_instruction };
            }

            var mySerilazeObject = XmlGenericUtil<TransmissionBodyType>.SerializeObject(this._TransmissionBodyModel, true);

           NetCommonHelper.Logger.DevLog.Instance.WriteDebug(mySerilazeObject);



            var myListdata = new List<data>() { new data() { entity = mySerilazeObject } };

            mytransmission.data = myListdata.ToArray();// GetDataList().ToArray();
            if (mytransmission.data.Count() < 1)
            {
                throw new Exception("(mytransmission.data.Count < 1)");
            }

            mytransmission_details.Add(mytransmission_detail1);
            
            mytransmission.transmission_details = mytransmission_details.ToArray();
            return mytransmission;

        }
        public void Send(UnifreightHybridQueueTaskParam unifreightHybridQueueTasParam ,bool  withTransmission=true)
        {
            CustomsSettingPM setting = CustomsSettingQueryService.GetSettingByTenant(_CommunicationModel.Tenant);
            if (setting != null && setting.StandAlone)
                return;
            //if (string.IsNullOrWhiteSpace(unifreightHybridQueueTasParam.QueueName))
            //{
            //    throw new ArgumentNullException(nameof(unifreightHybridQueueTasParam.QueueName));
            //}

            const string queueName = "ExternalTasksQueue";
            _CommunicationsParams = new CommunicationsParams()
            {
                Tenant = _CommunicationModel.Tenant,
                CommunicationLogTypeCode = "Q",
                //using   QueueName = "externaltasksqueue" + _CommunicationModel.Tenant + 1,
                Priority = 1,
                InOut = "O",
                Status = "W",
                LoggingUserId = _CommunicationModel.UserId ?? ResolveUserIdOrMehesID(tenant: _CommunicationModel.Tenant),

                Subject = GetSubject(),// "FU Status",
                LoggingEntityReference = GetLoggingEntityReference(),//_PhysicalCheckPM.CheckId,
                LoggingObjectTableId = GetLoggingObjectTableId(),// objectTable.Id,
                LoggingEntityId = GetLoggingEntityId(),//_PhysicalCheckPM.Id,



                FolderName =
                //"ExternalTasksQueue",
                queueName//"ExportStorageStatus"
            };

            string myMainObject;
            if (withTransmission)
            {
                var mytransmission = GetTransmission();
                myMainObject = XmlGenericUtil<transmission>.SerializeObject(mytransmission, true);
            }else
            {

                myMainObject = XmlGenericUtil<TransmissionBodyType>.SerializeObject(this._TransmissionBodyModel, true);
                


            }
            
            bool withoutEnvelop = true;
            if (withoutEnvelop)
            {
                List<QueueTask> queue1Tasks = GetArrayOfQueueTask(unifreightHybridQueueTasParam, myMainObject);
                _CommunicationsParams.ByteData = LogitudeXmlSerializer.SerializeObject(queue1Tasks);

            }
            else
            {
                myMainObject = HttpUtility.HtmlEncode(myMainObject);
                var myEnvelope = new Envelope()
                {

                    CommunicationLogId = Guid.NewGuid().ToString(),
                    Tasks = new List<QueueTask>() {

                    new QueueTask() {
                        Action = "StatusUpdate",
                        Parameters = new List<Parameter>()
                        {
                            new Parameter()
                            {

                                 Name="transmission",
                                 Value = myMainObject
                            }
                        }
                    }
                    }

                };
                _CommunicationsParams.ByteData = LogitudeXmlSerializer.SerializeObject(myEnvelope);
            }
           
            

            string communicationLogId = Communications.AddCommunicationLog(_CommunicationsParams);

            Communications.
                          SendCommunicationLogMessageToQueue(
                          //queueName: "externaltasksqueue" + _CommunicationModel.Tenant + 1,
                          queueName: queueName.ToLower() + _CommunicationModel.Tenant + 1,
                          communicationLogId: communicationLogId,
                          tenant: _CommunicationsParams.Tenant,
                          queueParameters: null,
                          delayTime: unifreightHybridQueueTasParam.UServerDelayTime,
                          InterfaceTypeCode: unifreightHybridQueueTasParam.InterfaceTypeCode

                          );

            LogMessagingUtil.Instance.Append("UnifreightHybridQueueTaskService()")
                        .Append("CommunicationLogId:").Append(communicationLogId)
                        .Append("Tenant").Append(_CommunicationsParams.Tenant);
        }

        private static List<QueueTask> GetArrayOfQueueTask(UnifreightHybridQueueTaskParam unifreightHybridQueueTasParam, string myMainObject)
        {
            List<QueueTask> queue1Tasks = new List<QueueTask>();


            queue1Tasks.Add(new QueueTask()
            {
                Action = unifreightHybridQueueTasParam.Action ?? throw new Exception("UnifreightHybridQueueTasParam Action  is missing "),//  "StatusUpdate",
                Parameters = new List<Parameter>()
                                             {
                                                new
                                                Parameter{
                                                    Name = unifreightHybridQueueTasParam.ParameterName ??   throw new Exception("UnifreightHybridQueueTasParam ParameterName is missing "),//  
                                                    Order = 1,
                                                    Value = myMainObject }
                                             }
            });
            return queue1Tasks;
        }

        private string ResolveUserIdOrMehesID(int tenant)
        {
            var res = AuthenticationUtil.ResolveUserIdOfUnifreightUser(tenant);
            if (res!=null)
            {
                 return res.ToString();
            }

            UserRepository userRepository = new UserRepository(tenant);
            var user = userRepository.GetSingleUserByCode("MEHES", tenant, true);

            return user.Id;
            
        }


    }

    public class UnifreightHybridQueueTaskParam
    {
        //public bool? SendImmediately { get; set; }
        public string Action { get; set; }
        public string ParameterName { get; set; }
        public TimeSpan UServerDelayTime { get; set; }
        public string InterfaceTypeCode { get; internal set; }
    }

}
