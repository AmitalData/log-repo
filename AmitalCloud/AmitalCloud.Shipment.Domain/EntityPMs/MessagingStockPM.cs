using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    [CustomValidation(typeof(Validators.ShipmentClassLevelValidator), "ValidateClass")]
    public partial class MessagingStockPM : EntityPM
    {
        [Key]
        public string Id { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public int TenantNumber { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? StartDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? EndDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public int? Amount { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public int? Remaining { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? CreateDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? UpdateDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string CreatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string UpdatedByUserId { get; set; }
                
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public bool IsCancelled { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string Status { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string Notes { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string SearchFields { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? TotalPrice { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string StockType { get; set; }

        // Ayman: Dummy fields
        public bool IsTotalPriceChanged { get; set; }
        public bool IsOtherFieldsChanged { get; set; }

        //private List<MessagingStockUsageHistoryPM> stockUsageHistories;
        [Include]
        [Composition]
        [Association("StockUsageHistoryPMStockPM", "Id", "StockId")]
        public virtual List<MessagingStockUsageHistoryPM> StockUsageHistories
        {
            get
            {
                if (stockUsageHistories == null)
                {
                    stockUsageHistories = new List<MessagingStockUsageHistoryPM>();
                }

                return this.stockUsageHistories;
            }

            set
            {
                if (value != null)
                {
                    stockUsageHistories = value;
                }
            }
        }

        public int DummyTenant { get; set; }
    }
}
