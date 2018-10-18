using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.RequestParams
{
    public class ConstraintApprovalListRequestParams : RequestParamsBase
    {
        public string DeclarationId { get; set; }
        public List<string> constraintNumber;

        public List<string> ConstraintNumber
        {
            get
            {
                if (constraintNumber == null)
                {
                    constraintNumber = new List<string>();
                }
                return constraintNumber;
            }
            set { constraintNumber = value; }
        }
    }
}
