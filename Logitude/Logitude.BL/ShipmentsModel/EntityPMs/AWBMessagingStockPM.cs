using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class AWBMessagingStockPM
    {
        [Key]
        public string Id { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int TenantNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? StartDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? EndDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? Amount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? Remaining { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? CreateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? UpdateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CreatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string UpdatedByUserId { get; set; }
                
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsCancelled { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Status { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notes { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SearchFields { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? TotalPrice { get; set; }

        // Ayman: Dummy fields
        public bool IsTotalPriceChanged { get; set; }
        public bool IsOtherFieldsChanged { get; set; }

        private List<AWBStockUsageHistoryPM> stockUsageHistories;
        [Include]
        [Composition]
        [Association("StockUsageHistoryPMStockPM", "Id", "StockId")]
        public virtual List<AWBStockUsageHistoryPM> StockUsageHistories
        {
            get
            {
                if (stockUsageHistories == null)
                {
                    stockUsageHistories = new List<AWBStockUsageHistoryPM>();
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
