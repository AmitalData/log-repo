using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class ImporterDeclarationRequestParams : RequestParamsBase
    {
        public enum DeclarationConectTypesEnum
        {
            All = 0,
            ImportDeclaration = 1,
            Vendor = 2,
            Declaration = 3,
        }

        public string ImporterNumber { get; set; }
        public Boolean IsByExpireDate { get; set; }
        public Boolean IsByType { get; set; }
        public DateTime? DeclarationExpire { get; set; }
        public string DeclarationConect { get; set; }
        public string Code { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public Boolean? JoinCustomsVendors { get; set; }

    }
}
