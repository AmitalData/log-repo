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
    public class AirlineAreaPM
    {
        [Key]
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AirlineId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Description { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Name { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? CreateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? UpdateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CreatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string UpdatedByUserId { get; set; }

        public string CreatedByUserName { get; set; }
        public string UpdatedByUserName { get; set; }

        private List<AirlineAreasPortPM> airlineAreasPorts;
        [Include]
        [Association("AirlineAreasPortAirlineArea", "Id", "AirlineAreaId")]
        [Composition]
        public virtual List<AirlineAreasPortPM> AirlineAreasPorts
        {
            get
            {

                if (this.airlineAreasPorts == null)
                {
                    airlineAreasPorts = new List<AirlineAreasPortPM>();
                }
                return this.airlineAreasPorts;
            }
            set
            {
                if (value != null)
                {
                    airlineAreasPorts = value;
                }
            }
        }

    }
}
