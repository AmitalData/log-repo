
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Reflection.Emit;
using Simplog.Server.Infrastructure.Helpers;

using Logitude.Accounting.Data.DataContract;

namespace Logitude.Accounting.Data.Repositories
{
    public partial class LedgerTransactionRepository : IRepository<LedgerTransaction>
    {
     
    }
}
