using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Validators
{
    public class ClaimValidator
    {
        private ClaimPM _ClaimPM;
        public List<String> ErrorCode { get; private set; }

        public ClaimValidator(ClaimPM claimPM)
        {
            _ClaimPM = claimPM;
            ErrorCode = new List<string>();
        }

        //Check if declaration was already paid
        public void PaymentDateCheck()
        {
            string errorMessage = "";
            //if (_DeclarationPM.PaymentDate.HasValue)
            if (_ClaimPM != null)
            {
                /*if (_ClaimPM.CreateDate)
                {
                    errorMessage = "Customs.General.O.NoPaymentDate";
                    if (!string.IsNullOrWhiteSpace(errorMessage))
                    {
                        ErrorCode.Add(errorMessage);
                    }
                }*/
            }
        }

        //Validation Checks before sending Declaration to customs
        public void PreClaimSendChecks()
        {
            //PaymentDateCheck();
        }

        public void SubmitDateTimeCheck()
        {
            if (_ClaimPM != null)
            {
                var errorMessage = "";
                if (!_ClaimPM.SubmitDate.HasValue)
                {
                    errorMessage = "Customs.General.O.TaxationDateTimeNotToday";
                }
                else
                {
                    if ((_ClaimPM.SubmitDate.Value.Date.Year != DateTime.Now.Date.Year) ||
                        (_ClaimPM.SubmitDate.Value.Date.Month != DateTime.Now.Date.Month) ||
                        (_ClaimPM.SubmitDate.Value.Date.Day != DateTime.Now.Date.Day))
                    {
                        errorMessage = "Customs.General.O.TaxationDateTimeNotToday";
                    }
                }
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    ErrorCode.Add(errorMessage);
                }
            }
        }
    }
}
