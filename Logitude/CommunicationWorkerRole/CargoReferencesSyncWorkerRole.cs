using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.CargoTracking.Data;
using Logitude.Server.Tools.KafkaConfigurations;
using Logitude.Server.Tools.Messages;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading;

namespace CommunicationWorkerRole
{
    public class CargoReferencesSyncWorkerRole : WorkerEntryPoint
    {
        private ICargoTrackingContext currentContext;
        public override void Run()
        {
            while (IsRunning)
            {
                Do();
            }
        }

        private void Do()
        {
            if (General.IsUpdating())
            {
                Thread.Sleep(60000);
                return;
            }
            try
            {
                SyncConnectedSepments();
            }
            catch (Exception ex)
            {
                Thread.Sleep(10000);
            }

        }

        private void SyncConnectedSepments()
        {

        }

        public override bool OnStart()
        {


            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "CargoReferencesSync";
            currentContext = CargoTrackingContext.GetContext(0);
            return base.OnStart();
        }

    }
}
