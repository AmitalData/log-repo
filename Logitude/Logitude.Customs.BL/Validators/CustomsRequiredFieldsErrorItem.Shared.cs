using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Validators
{
    public  class CustomsRequiredFieldsErrorItem
    {
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string EntityReference { get; set; }
        public string EntityReference2 { get; set; }
        public string CustomMessageError { get; set; }
    }
}
