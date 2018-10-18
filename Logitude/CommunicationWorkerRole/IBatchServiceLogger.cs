using System;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.Server.Tools;

namespace CommunicationWorkerRole
{
    public interface IBatchServiceLogger
    {
        void InitWorkerFromDB(Func<BatchServicesDefinitionPM, WorkerEntryPointDoneLog> OverrideLoadWorkEntryPoint);
        void StartLogging();
    }
}