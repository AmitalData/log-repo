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


    }
}