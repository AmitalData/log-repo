using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Tasks
{
    public class CustomsCloseCourierMasterTask : TaskManagerBase
    {
        private int _SeedDefaultTenant=0;
        private string _TaskId;

        public CustomsCloseCourierMasterTask(string Id, int tenant)
            : base(Id, tenant)
        {
            _SeedDefaultTenant = tenant;
            _TaskId = Id;
        }
        public override void StartTask()
        {

            LogMessagingUtil.Instance.Clear();
            try
            {
                ICustomsCloseCourierMasterService myICustomsCloseCourierMasterService = ContainerAccessor.Container.Resolve(typeof(ICustomsCloseCourierMasterService), "CloseCourierMasterService", new ParameterOverride("", this._SeedDefaultTenant)) as ICustomsCloseCourierMasterService;
                myICustomsCloseCourierMasterService.StartRun(_TaskId, _SeedDefaultTenant);
            }
            catch (Exception e)
            {

                LogException(e.ToString());
            }
            finally
            {
                LogInfo(LogMessagingUtil.Instance.ToString());
                this.LogInfoToDB(LogMessagingUtil.Instance.ToString());
            }
        }
    }
}