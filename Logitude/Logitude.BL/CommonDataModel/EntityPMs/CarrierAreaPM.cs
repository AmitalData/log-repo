using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    [DataContract]
    public class CarrierAreaPM
    {
        [Key]
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public int Tenant { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Description { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CarrierId { get; set; }

        

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Name { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? CreateDate { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? UpdateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CreatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string UpdatedByUserId { get; set; }



        [DataMember]
        public string CreatedByUserName { get; set; }
        [DataMember]
        public string UpdatedByUserName { get; set; }

        [DataMember]
        public ChangeSetOperation ChangeSetOp { get; set; }

        private List<CarrierAreasPortPM> carrierAreasPorts;
        [Include]
        [Association("CarrierAreasPortPM", "Id", "CarrierAreaId")]
        [Composition]
        [DataMember]
        public virtual List<CarrierAreasPortPM> CarrierAreasPorts
        {
            get
            {

                if (this.carrierAreasPorts == null)
                {
                    carrierAreasPorts = new List<CarrierAreasPortPM>();
                }
                return this.carrierAreasPorts;
            }
            set
            {
                if (value != null)
                {
                    carrierAreasPorts = value;
                }
            }
        }

    }
}
