using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
//using System.Reflection.Emit;
using System.Text;
using System.Threading;
using Logitude.Server.Tools.QueueService;
using Logitude.Accounting.BL.CloseTables;
using static Simplog.Server.Infrastructure.DbContextBase;
using System.Transactions;
using Simplog.Data.Helpers;
using System.Data;
using System.Data.SqlClient;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Server.Tools.Resolvers;

namespace Logitude.Accounting.BL.CoreBL
{
    public class GLAccountCounterService
    {
        int tenant;
        public GLAccountCounterService(int _tenant)
        {
            tenant = _tenant;
        }
        private GLAccountCounterPM GetOrInsertCounter(string prefix)
        {
            GLAccountCounterPM gLAccountCounterPM = GetByPrefix(prefix);

            if(gLAccountCounterPM == null)
            {
                gLAccountCounterPM = GetNewCounter(prefix);
                SubmitCounter(gLAccountCounterPM);
            }

            return gLAccountCounterPM;
        }

        private GLAccountCounterPM GetNewCounter(string prefix)
        {
            GLAccountCounterPM gLAccountCounterPM = new GLAccountCounterPM()
            {
                Id = IdCounterUtilResolver.GetNewIdCounter("GLAccountCounter", tenant),
                Tenant = tenant,
                ChangeSetOp = ChangeSetOperation.Insert,

                Prefix = prefix,
                StartNumber = 1,
                CurrentNumber = 1,
            };
            return gLAccountCounterPM;
        }
        private GLAccountCounterPM GetByPrefix(string prefix)
        {
            // get counter
            GLAccountCounterQueryService gLAccountCounterQuery = new GLAccountCounterQueryService(tenant);
            GLAccountCounterPM gLAccountCounterPM = gLAccountCounterQuery.GetByPrefix(prefix, tenant);
            return gLAccountCounterPM;
        }
        private void SubmitCounter(GLAccountCounterPM gLAccountCounterPM)
        {
            GLAccountCounterUpdateService gLAccountCounterUpdateService = new GLAccountCounterUpdateService(tenant);
            gLAccountCounterUpdateService.Update(gLAccountCounterPM, true);
        }
    }
}
