using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.WorkerRole.MultiEntityUpdate
{
    public class MultiEntityUpdateLogExecutionService
    {
        private string multiEntityUpdateLogId = string.Empty;
        private int tenant;
        private MultiEntityUpdateLogPM multiEntityUpdateLogPM = null;
        private MultiEntityUpdateLogService multiEntityUpdateLogService = null;

        public MultiEntityUpdateLogExecutionService(string multiEntityUpdateLogId , int tenant)
        {
            this.multiEntityUpdateLogId = multiEntityUpdateLogId;
            this.tenant = tenant;
            IWebFreightContext objectContext = WebFreightContext.GetContext((int)tenant);
            multiEntityUpdateLogService = new MultiEntityUpdateLogService(objectContext, this.tenant);
        }


        public MultiEntityUpdateLogPM Get()
        {
            MultiEntityUpdateLogQuery multiEntityUpdateLogQuery = new MultiEntityUpdateLogQuery((int)tenant);
            multiEntityUpdateLogPM = multiEntityUpdateLogQuery.GetSinglePM(multiEntityUpdateLogId, tenant);
            return multiEntityUpdateLogPM;
        }

        public void Update(MultiEntityUpdateLogArgs multiEntityUpdateLogArgs)
        {
            if (multiEntityUpdateLogArgs == null) return ;
            multiEntityUpdateLogPM.StartDate = multiEntityUpdateLogArgs.StartDate != null ? multiEntityUpdateLogArgs.StartDate : multiEntityUpdateLogPM.StartDate;
            multiEntityUpdateLogPM.DoneDate = multiEntityUpdateLogArgs.DoneDate != null ? multiEntityUpdateLogArgs.DoneDate : multiEntityUpdateLogPM.DoneDate;
            multiEntityUpdateLogPM.StatusCode = !string.IsNullOrEmpty(multiEntityUpdateLogArgs.StatusCode) ? multiEntityUpdateLogArgs.StatusCode : multiEntityUpdateLogPM.StatusCode;
            multiEntityUpdateLogPM.UpdatedEntitiesNumber = multiEntityUpdateLogArgs.UpdatedEntitiesNumber;
            multiEntityUpdateLogPM.XMLData = !string.IsNullOrEmpty(multiEntityUpdateLogArgs.XMLData) ? multiEntityUpdateLogArgs.XMLData : multiEntityUpdateLogPM.XMLData;
            multiEntityUpdateLogPM.ExceptionMessage = !string.IsNullOrEmpty(multiEntityUpdateLogArgs.ExceptionMessage) ? multiEntityUpdateLogArgs.ExceptionMessage : multiEntityUpdateLogPM.ExceptionMessage;
            multiEntityUpdateLogService.Update(multiEntityUpdateLogPM);
        }

    }


    public class MultiEntityUpdateLogArgs
    {
        public string StatusCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public int UpdatedEntitiesNumber { get; set; }
        public string XMLData { get; set; }
        public string ExceptionMessage { get; set; }
    }
}