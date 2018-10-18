using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class CustomerForwarderByProductPM
    {
        [Key]
        public string ProductTypeCode { get; set; }

        public string ForwarderId { get; set; }
        [Key]
        public string CustomerId { get; set; }

        public int Tenant { get; set; }
        public string ForwarderName { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
