using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.DataContract
{
    public class ARinvoiceSequencesReportData: BaseDataProvider
    {
        public List<string> Sequances {get; set;}
    }
}

