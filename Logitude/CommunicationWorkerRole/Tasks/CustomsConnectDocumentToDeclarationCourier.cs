using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
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
    public class CustomsConnectDocumentToDeclarationCourier : TaskManagerBase
    {
        private int _SeedDefaultTenant = 0;
        private string _TaskId;

        public CustomsConnectDocumentToDeclarationCourier(string Id, int tenant)
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
				ICustomsConnectDocumentToDeclarationCourier myICustomsConnectDocumentToDeclarationCourier = ContainerAccessor.Container.Resolve(typeof(ICustomsConnectDocumentToDeclarationCourier), "ConnectDocumentToDeclarationCourier", new ParameterOverride("", this._SeedDefaultTenant)) as ICustomsConnectDocumentToDeclarationCourier;
				myICustomsConnectDocumentToDeclarationCourier.StartRun(_TaskId, _SeedDefaultTenant);
            }
            catch (Exception e)
            {

                LogException(e.ToString());
            }
            finally
            {
                this.LogInfoToDB(LogMessagingUtil.Instance.ToString());
                LogInfo(LogMessagingUtil.Instance.ToString());

            }
        }

 

   
        
    }
}