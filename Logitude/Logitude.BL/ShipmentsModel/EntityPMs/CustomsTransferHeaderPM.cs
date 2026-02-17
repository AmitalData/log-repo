using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class CustomsTransferHeaderPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string SearchFields { get; set; }
        public string CustomsTransferTypeName { get; set; }
        public string CreatedByUserName { get; set; }
        
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TransferNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? TransferDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FileName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CreatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomsTransferTypeCode { get; set; }
        
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notes { get; set; }

        List<CustomsTransferLinePM> transferLines;
        [Include]
        [Composition]
        [Association("TransferHeaderTransferLines", "Id", "CustomsTransferHeaderId")]
        public virtual List<CustomsTransferLinePM> CustomsTransferLines
        {
            get
            {
                if (transferLines == null)
                {
                    transferLines = new List<CustomsTransferLinePM>();
                }

                return this.transferLines;
            }

            set
            {
                if (value != null)
                {
                    transferLines = value;
                }
            }
        }
    }
}
