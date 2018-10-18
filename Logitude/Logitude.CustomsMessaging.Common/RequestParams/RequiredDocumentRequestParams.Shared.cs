using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class RequiredDocumentRequestParams : RequestParamsBase
    {
        public string AppicationId { get; set; }
        public string ParentEntityCode { get; set; }
        public string ParentEntityId { get; set; }
        public string Child1EntityCode { get; set; }
        public string Child1EntityId { get; set; }
        public string Child2EntityCode { get; set; }
        public string Child2EntityId { get; set; }
    }
}
