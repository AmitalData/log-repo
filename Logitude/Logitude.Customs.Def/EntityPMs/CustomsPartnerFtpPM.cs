using Logitude.Customs.Def.Validators;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityPMs
{
    //[CustomValidation(typeof(CustomsClassLevelValidator), "ValidateClass")]
    public partial class CustomsPartnerFtpPM
    {
        public FTPDetail MyFtpDetail { get; set; }
        
    }
}
