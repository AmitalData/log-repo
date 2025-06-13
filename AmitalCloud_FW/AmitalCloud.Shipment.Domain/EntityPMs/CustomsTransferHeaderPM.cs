using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Text;
using System.Threading.Tasks;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    [CustomValidation(typeof(Validators.ShipmentClassLevelValidator), "ValidateClass")]
    public partial class CustomsTransferHeaderPM : EntityPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string SearchFields { get; set; }
        public string CustomsTransferTypeName { get; set; }
        public string CreatedByUserName { get; set; }
        
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string TransferNumber { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? TransferDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string FileName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string CreatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string CustomsTransferTypeCode { get; set; }
        
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string Notes { get; set; }
        public string ShipmentNumber { get; set; }

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
