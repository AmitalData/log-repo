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
        public int UnReleasedFastProcessCount { get; set; }
        public int WithoutIdCount { get; set; }
        public int WithoutClassificationCount { get; set; }
        public int PendingPaymentCount { get; set; }
        public int PendingCustomsCount { get; set; }
        public int PendingCount { get; set; }
        public int CourierMasterOpenIndividualCount { get; set; }
        public int UnReleasedIndividualCount { get; set; }
        public int AllCourierDeclarationsCount { get; set; }

    }
}
