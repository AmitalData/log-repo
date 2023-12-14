using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class UserLastLoginPM
    {
        [Key]
        public string Id { get; set; }
        [Key]
        public string ComputerId { get; set; }
        public DateTime? LoginDateTime { get; set; }
        public int Tenant { get; set; }
        public string UserId { get; set; }
        [Key]
        public string WorkEnvironment { get; set; }

        public UserPM User { get; set; }
        public string IP { get; set; }
        public string ComputerUserName { get; set; }
    }
}
