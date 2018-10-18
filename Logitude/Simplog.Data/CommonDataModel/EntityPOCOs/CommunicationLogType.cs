using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class CommunicationLogType
    {
        [Key]
        public string Code { get; set; }

        public string Name { get; set; }
        public string SearchFields { get; set; }


        //public List<CommunicationLog> CommunicationLogs { get; set; }
    }
}