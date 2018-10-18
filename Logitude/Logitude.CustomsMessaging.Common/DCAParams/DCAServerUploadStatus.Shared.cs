using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.DCAParams
{
    public class DCAServerUploadStatus
    {
        public enum StatusEum
        {
            LogitudeDefault,
            NOT_FOUND,FAILD,IN_PROGRESS,SENT
        }
        public DCAServerUploadResponse TheDCAServerUploadResponse { get; set; }
        
        public string UnifreightQueueOutStatus { get; set; }
        public string DcaMessage { get; set; }
        public StatusEum GetStatusEum()
        {
            StatusEum sts = StatusEum.LogitudeDefault;
            Enum.TryParse<StatusEum>(UnifreightQueueOutStatus, out sts);
            return sts;
        }
    }
}
