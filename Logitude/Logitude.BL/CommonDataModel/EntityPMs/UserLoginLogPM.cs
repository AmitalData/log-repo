using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class UserLoginLogPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string IP { get; set; }
        public string Browser { get; set; }
        public string UserId { get; set; }
        public DateTime? GMTDateTime { get; set; }
        public DateTime? LocalDateTime { get; set; }
        public string ComputerId { get; set; }
        public string UserAgent { get; set; }
        public UserPM User { get; set; }
    }
}