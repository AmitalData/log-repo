using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Customs
{
    public class ClientProgressBarIndicatorService
    {
        /// <summary>
        /// ssssggg
        /// </summary>
        private RequestParamsBase _MyRequestParamsBase;
        private ClientProgressBarIndicatorBasicM _MyClientProgressBarIndicatorM;
            
        public ClientProgressBarIndicatorService(RequestParamsBase requestParamsBase)
        {
            // TODO: Complete member initialization
            this._MyRequestParamsBase = requestParamsBase;
            
        }
        

        public  void StartBroadcast(
            //List<Simplog.Data.CommonDataModel.EntityPOCOs.CommunicationLogStep> list
            string message
            )
        {
#if true
            if (_MyClientProgressBarIndicatorM != null)
            {
                _MyClientProgressBarIndicatorM.CurrentStage = message;
            }
            else
            {
                _MyClientProgressBarIndicatorM = new ClientProgressBarIndicatorBasicM(this._MyRequestParamsBase.PBId) { CurrentStage = message };
            }
                
#else
	
                
            MyClientProgressBarIndicatorM = new ClientProgressBarIndicatorM() { PB_Id = this.MyRequestParamsBase.PBId };
            MyClientProgressBarIndicatorM.Steps = list.Select(rec => rec.Name ).ToList();
            MyClientProgressBarIndicatorM.StepList = list.Select(rec => new StepM() { Name = rec.Name }).ToList();
            MyClientProgressBarIndicatorM.SelectedItemId = MyClientProgressBarIndicatorM.Steps.First();
            MyClientProgressBarIndicatorM.CurrStageLog = "";
#endif
            
            UpdateCache();
        }

        private void UpdateCache()
        {
            string entityKeyString = GetCacheKey(this._MyRequestParamsBase.PBId);
            var CachClientProgressBarIndicatorM = CacheManager.CacheWrapper.Get(entityKeyString) as ClientProgressBarIndicatorBasicM;
            if (CachClientProgressBarIndicatorM == null)
            {
                //CacheManager.CacheWrapper.Remove(entityKeyString);
                CacheManager.CacheWrapper.Insert(entityKeyString, _MyClientProgressBarIndicatorM);
            }
            
        }

        static string GetCacheKey(string PBId)
        {
            return "ClientProgressBarIndicatorM," + PBId;
        }
#if false
        internal void EndStep(Simplog.Data.CommonDataModel.EntityPOCOs.CommunicationLogStep communicationLogStep, 
            ClosedTable.CommStatusEnum stepStatusEnum, 
            string log)
        {
            if (MyClientProgressBarIndicatorM == null) return;
            MyClientProgressBarIndicatorM.SelectedItemId = communicationLogStep.Name;
            var curr=MyClientProgressBarIndicatorM.StepList.First(rec => rec.Name == communicationLogStep.Name);
            MyClientProgressBarIndicatorM.CurrStageLog= curr.Log = log;
            curr.StepTimeSpan = communicationLogStep.EndDate.Subtract(communicationLogStep.StartDate);
            UpdateCache();
        }
#endif
        public void StartStep(Func<string> GetStepMessage)
        {
            if (_MyClientProgressBarIndicatorM == null) return;
            var stepMessage =GetStepMessage();
            StartStep(stepMessage);
        }

        public void StartStep(string StepMessage)
        {
            if (_MyClientProgressBarIndicatorM == null) return;
            _MyClientProgressBarIndicatorM.CurrentStage = StepMessage;
            UpdateCache();
        }
        

        public static void UpsertClientProgressBarIndicatorCurrentStage(string PBId,string currStageLog)
        {
            if (String.IsNullOrWhiteSpace(PBId)) return;  
            var entityKeyString = ClientProgressBarIndicatorService.GetCacheKey(PBId);
            var CachClientProgressBarIndicatorM = CacheManager.CacheWrapper.Get(entityKeyString) as ClientProgressBarIndicatorBasicM;
            if (CachClientProgressBarIndicatorM == null)
            {
                CachClientProgressBarIndicatorM = new ClientProgressBarIndicatorBasicM(PBId) { CurrentStage = currStageLog };
                
                //CacheManager.CacheWrapper.Remove(entityKeyString);
                CacheManager.CacheWrapper.Insert(entityKeyString, CachClientProgressBarIndicatorM);
            }
            CachClientProgressBarIndicatorM.CurrentStage = currStageLog;
        }

        public static string GetClientProgressBarIndicatorCurrentStage(string PBId)
        {
            var entityKeyString = ClientProgressBarIndicatorService.GetCacheKey(PBId);
            var CachClientProgressBarIndicatorM = CacheManager.CacheWrapper.Get(entityKeyString) as ClientProgressBarIndicatorBasicM;
            if (CachClientProgressBarIndicatorM == null)
            {
                return "";

            }
            return CachClientProgressBarIndicatorM.CurrentStage;
        }

        public static void UpdateStage(string PBId, string StepMessage)
        {
            string entityKeyString = GetCacheKey(PBId);
            var CachClientProgressBarIndicatorM = CacheManager.CacheWrapper.Get(entityKeyString) as ClientProgressBarIndicatorBasicM;
            CachClientProgressBarIndicatorM.CurrentStage = StepMessage;
        }
    }
    public class ClientProgressBarIndicatorBasicM
    {
        
        //readonly 
            string _PB_Id;

        public ClientProgressBarIndicatorBasicM(string PB_Id)
        {
            this._PB_Id = PB_Id;
        }
        [Key]
        [DataMember]
        public string PB_Id
        {
            get { return _PB_Id; }
        }
        string _CurrentStage;
        [DataMember]
        public string CurrentStage
        {
            get { return _CurrentStage; }
            set { _CurrentStage = value; }
        }
        
        
    }
#if false
    class ClientProgressBarIndicatorM
    {
        [Key]
        [DataMember]
        public string PB_Id { get; set; }


        [DataMember]
        public string SelectedItemId { get; set; }
        [DataMember]
        public string CurrStageLog { get; set; }
        [DataMember]
        public List<StepM> StepList { get; set; }
        [DataMember]
        public List<string> Steps { get; set; }
    }
    public class StepM
    {
        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public TimeSpan? StepTimeSpan { get; set; }
        [DataMember]
        public string Log { get; set; }


        
   
    }
#endif
    
}
