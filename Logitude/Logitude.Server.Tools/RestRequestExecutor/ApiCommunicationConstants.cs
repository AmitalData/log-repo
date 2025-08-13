using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.RestRequestExecutor
{

    public  class ApiCommunicationConstants
    {
        public const char TypeQueue = 'Q';
        public const string DefaultFolder = "ExternalTasksQueue";
        public  string Subject { get; set; }
        public  string EntityId { get; set; }
        public  string ObjectTableId { get; set; }
        public  string InOut { get; set; }

}
}
