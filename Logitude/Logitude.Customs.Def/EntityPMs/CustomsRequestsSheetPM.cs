using Logitude.Customs.Def.ClosedTable;

using Logitude.Customs.Def.Validators;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityPMs
{
    
    [CustomValidation(typeof(CustomsClassLevelValidator), "ValidateClass")]
    public partial class CustomsRequestsSheetPM : EntityPM
    {
        //[CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
        public SheetStatusEnum RequestStatusEnum
        {
            get
            {
                var  requestStatusEnum =SheetStatusEnum.Created ;
                Enum.TryParse<SheetStatusEnum>(this.RequestStatusCode, out requestStatusEnum);
                return requestStatusEnum;
            }
            set
            {
                int iVal = (int)value;
                this.RequestStatusCode = iVal.ToString();  
            }
        }
        public string SignStepName { get; set; }
        [DataMember]
        public string MainEntityConcurrencyGUID { get; set; }


    }
}
