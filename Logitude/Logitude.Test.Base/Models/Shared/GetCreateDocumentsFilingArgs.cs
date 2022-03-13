using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Base.Models.Shared
{
    public class GetCreateDocumentsFilingArgs
    {
        public string DocumentTypeId;
        public string EntityId;
        public string ObjectTableId;
        public string DirectionCode;
        public int Tenant;

    }
}
