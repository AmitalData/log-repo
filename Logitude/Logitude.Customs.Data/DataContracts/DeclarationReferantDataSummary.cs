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
    public class DeclarationReferantDataSummary
    {
        [Key]
        public Guid Id { get; set; }
        public int FilesInProcess { get; set; }
        public int TrackingCases { get; set; }
        public int FilesInOCR { get; set; }
        public int FilesInSivug { get; set; }
        public int FilesInReview { get; set; }
        public int FilesInCreditControl { get; set; }
        public int FilesAvailableFreeOfCharge { get; set; }
        public int AllCases { get; set; }
        public int FilesWithoutRelease { get; set; }
        public int FilesToPay { get; set; }

        public int FilesInProcess_A { get; set; }
        public int TrackingCases_A { get; set; }
        public int FilesInOCR_A { get; set; }
        public int FilesInSivug_A { get; set; }
        public int FilesInReview_A { get; set; }
        public int FilesInCreditControl_A { get; set; }
        public int FilesAvailableFreeOfCharge_A { get; set; }
        public int AllCases_A { get; set; }
        public int FilesInAllInclusive { get; internal set; }
        public int FilesInAllInclusive_A { get; internal set; }
        public int FilesRejectedByController { get; internal set; }
        public int FilesRejectedByController_A { get; internal set; }
        public int FilesRejectedByClassification { get; internal set; }
        public int FilesRejectedByClassification_A { get; internal set; }
        public int FilesWithoutRelease_A { get; internal set; }
        public int FilesToPay_A { get; set; }

    }
}