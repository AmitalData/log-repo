using Logitude.Server.Tools.EntityChanges.Service;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.EntityChanges.AutomationResult
{
    public class AutomationOnUpdateDocumentService : GeneralAutomationResultService, IAutomationResultService
    {
        private AutomationResultArgs automationResultArgs { get; set; }
        private int tenant;
        private EntityChange entityChange;
        public bool DependencyOnLastEntityUpdate { get { return false; } }
        public string ResultCode { get { return "ONUPDATEDOCUMENT"; } }

        public void Run(AutomationResultArgs automationResultArgs)
        {
            this.automationResultArgs = automationResultArgs;
            this.entityChange = automationResultArgs.EntityChange;
            this.tenant = this.entityChange.Tenant;
        }

        private void ExecuteOnUpdateDocumentAutomations()
        {
        }
    }
}
