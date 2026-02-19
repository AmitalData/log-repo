using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    [CustomValidation(typeof(Validators.ShipmentClassLevelValidator), "ValidateClass")]
    public partial class ShipmentCommodityPM : EntityPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? ChargeRate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? ChargeAmount { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? Volume { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? VolumetricWeight { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? GrossWeight { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? ChargeableWeight { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public int? NumberOfPackages { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string RateClassCode { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string CommodityNumber { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string DescriptionOfGoods { get; set; }

        public bool IsFirstLine { get; set; }

        //private List<CommodityPackagePM> commodityPackages;
        [Include]
        [Composition]
        [Association("CommodityPackagePMShipmentCommodity", "Id", "CommodityId")]
        public virtual List<CommodityPackagePM> CommodityPackages
        {
            get
            {
                if (commodityPackages == null)
                {
                    commodityPackages = new List<CommodityPackagePM>();
                }

                return this.commodityPackages;
            }

            set
            {
                if (value != null)
                {
                    commodityPackages = value;
                }
            }
        }

        //public ChangeSetOperation ChangeSetOp { get; set; }
    }
}