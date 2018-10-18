using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.DataContracts
{
    public class ImporterQueriesDataCounts
    {
        [Key]
        public int Id { get; set; }
        public int OpenShipmentsCount { get; set; }
        public int MissingDocsCount { get; set; }
        public int RecentCount { get; set; }
        public int ArchivedShipmentsCount { get; set; }
        public int AllShipmentsCount { get; set; }
        public int AgentShipmentsCount { get; set; }
        public int ImporterShipmentsCount { get; set; }
        public int RequestedDocsCount { get; set; }
        public int RequiredActionsCount { get; set; }
    }
}
