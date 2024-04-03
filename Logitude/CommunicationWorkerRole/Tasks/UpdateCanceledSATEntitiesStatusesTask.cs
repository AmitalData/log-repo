using CommunicationWorkerRole.Services.SAT;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Tasks
{
    public class UpdateCanceledSATEntitiesStatusesTask : TaskManagerBase
    {
        public UpdateCanceledSATEntitiesStatusesTask(string Id, int tenant)
            : base(Id,tenant)
        {

        }
        public override void StartTask()
        {
            UpdateCanceledSATEntitiesStatusesService.Update();
        }
    }
}
