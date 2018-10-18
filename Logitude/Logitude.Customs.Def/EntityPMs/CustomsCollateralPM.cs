using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityPMs
{
    public partial class CustomsCollateralPM
    {
        [DataMember]
        public int CustomsCollateralsAnswerLineNumber { get; set; }
        [DataMember]
        public int CustomsCollateralsConditionLineNumber { get; set; }
    }
}
