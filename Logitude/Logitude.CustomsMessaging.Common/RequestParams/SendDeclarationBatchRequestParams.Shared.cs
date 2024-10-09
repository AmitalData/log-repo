using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class SendDeclarationBatchRequestParams : RequestParamsBase
    {
        public string Action { get; set; }
        public bool IsAllSelected { get; set; }
        public List<string> SelectedIds { get; set; }
        public QueryOperations QueryOperations { get; set; }
    }
}
