using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class RequestSheetInQueueMesView
    {
        
        public string ObjectTableId1 { get; set; }
        [Key]
        public string EntityId1 { get; set; }
        public string InterfaceTypeCode { get; set; }
        public string InterfaceTypeName { get; set; }
        public int Tenant { get; set; }


    }
}
