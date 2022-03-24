using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.DataContracts
{
    public class DeclarationCourierStatusSummary
    {
        [Key]
        public Guid Id { get; set; }
        public int OpenCourierMasterCount { get; set; }
        public long TookOpenCourierMasterCount { get; set; }
        public int UnReleasedFastProcessCount { get; set; }
        public long TookUnReleasedFastProcessCount { get; set; }
        public int WithoutIdCount { get; set; }
        public long TookWithoutIdCount { get; set; }
        public int WithoutClassificationCount { get; set; }
        public long TookWithoutClassificationCount { get; set; }
        public int PendingPaymentCount { get; set; }
        public long TookPendingPaymentCount { get; set; }
        public int PendingCustomsCount { get; set; }
        public long TookPendingCustomsCount { get; set; }
        public int PendingCount { get; set; }
        public long TookPendingCount { get; set; }
        public int CourierMasterOpenIndividualCount { get; set; }
        public long TookCourierMasterOpenIndividualCount { get; set; }
        public int UnReleasedIndividualCount { get; set; }
        public long TookUnReleasedIndividualCount { get; set; }
        public int AllCourierDeclarationsCount { get; set; }
        public long TookAllCourierDeclarationsCount { get; set; }

    }
}
