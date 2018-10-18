using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class EmailProviderPM
    {
        [Key]
        public string ProviderNumber { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public string Status { get; set; }
        public DateTime? LastTestSendDate { get; set; }
        public DateTime? LastTestReceivedDate { get; set; }
        public bool SupportsEmailDelivery { get; set; }

    }
}