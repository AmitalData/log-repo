using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class CreateDocumentOutArgs
    {
        public string DocumentTypeId { get; set; }
        public string EntityId { get; set; }
        public string ChildEntityId { get; set; }
        public string ChildReference { get; set; }
        public string ObjectTableId { get; set; }
        public int Tenant { get; set; }

    }
}