using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityPMs
{


    //[CustomValidation(typeof(JournalValidator), "IsJournalValid")]
    public partial class JournalPM : EntityPM
    {
        public JournalPM()
        {

        }
       
        public int LineCounter { get; set; }
        public JournalStatusTypePM.StatusCodeEnum StatusCodeEnum
        {
            get
            {
                var statusCodeEnum = JournalStatusTypePM.StatusCodeEnum.Draft;
                Enum.TryParse<JournalStatusTypePM.StatusCodeEnum>(this.StatusCode, out statusCodeEnum);
                return statusCodeEnum;
            }
            set
            {
                int iVal = (int)value;
                this.StatusCode = iVal.ToString();
            }
        }

        public string LineCreditAccountId { get; set; }
    }

}
