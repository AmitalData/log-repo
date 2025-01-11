using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Data.Services
{ 
    public class LogQueueMessage
    {
        public string Message { get; set; }
        public string FileName { get; set; }
        public string FolderName { get; set; }

        public string FileExtension { get; set; }

        public int Tenant { get; set; }
    }
}
