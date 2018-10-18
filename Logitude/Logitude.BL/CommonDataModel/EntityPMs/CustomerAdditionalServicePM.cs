using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class CustomerAdditionalServicePM
    {
        [Key]
        public string CustomerId { get; set; }

        [Key]
        public string AdditionalServiceId { get; set; }

        public int Tenant { get; set; }
        public bool Potential { get; set; }

        public string AdditionalServiceName { get; set; }
        public string AdditionalServiceCode { get; set; }
        public string CustomerName { get; set; }

        public string Notes { get; set; }

        // dummy for customer additional service report
        public string Salesman { get; set; }
        public string PrimaryContact { get; set; }
        public string SalesmanUserId { get; set; }
        public string BusinessUnitId { get; set; }
        //

        public Simplog.Server.Infrastructure.ChangeSetOperation ChangeSetOp { get; set; }
        public bool NotesRightToLeft { get; set; }

    }
}