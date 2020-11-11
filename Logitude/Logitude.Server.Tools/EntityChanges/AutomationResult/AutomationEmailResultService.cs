using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.EntityChanges.AutomationResult
{
    public class AutomationEmailResultService : GeneralAutomationResultService, IAutomationResultService
    {
        private AutomationResultArgs automationResultArgs { get; set; }
        private int tenant;
        private EntityChange entityChange;

        private List<Automation> automationsEmail = new List<Automation>();

        public AutomationEmailResultService()
        {
        }

        public void Run(AutomationResultArgs automationResultArgs)
        {
            this.automationResultArgs = automationResultArgs;
            this.entityChange = automationResultArgs.EntityChange;
            this.tenant = this.entityChange.Tenant;

            this.automationsEmail = automationResultArgs.AutomationLists.Where(d => d.ResultCode == "EMAIL").ToList();
            if (automationsEmail.Count > 0)
            {
                ExecuteEmailAutomations();
            }


        }

        private void ExecuteEmailAutomations()
        {
            foreach (Automation automation in automationsEmail)
            {
                string lastAuomationUpdateDate = GetLastAuomationUpdateDate(automationResultArgs.AutomationObjectTable, automationResultArgs.OtherAutomationObjectTable, automation);
                AutomatedBackup automatedBackup = GetAutomatedBackupClass(automation, entityChange, lastAuomationUpdateDate);
                TimeSpan? automationDelayTime = null;
                if (automatedBackup.Type == "Delayed")
                {
                    DelaytimeDetails delaytimeDetails = new DelaytimeDetails() { Type = automatedBackup.Type, Delaytime = automatedBackup.Delaytime, DelaytimeIndicator = automatedBackup.DelaytimeIndicator, DelaytimeOp = automatedBackup.DelaytimeOp, SelectedDelaytimeFieldCode = automatedBackup.SelectedDelaytimeFieldCode };
                    automationDelayTime = GetAutomationDelayTime(delaytimeDetails, automationResultArgs.AutomationFieldLists);
                }

                AddAutomationQueue(new AutomationQueueArgs() { EntityChangeId = entityChange.Id, AutomationId = automation.Id, AutomationType = automationResultArgs.EntityChangeArgs.ProcessType, EntityId = entityChange.EntityId, Tenant = automation.Tenant, AutomationDelayTime = automationDelayTime, ExecutedImmediately = (automatedBackup.Type == "Delayed" ? false : true) });
            }
        }

    }

}
