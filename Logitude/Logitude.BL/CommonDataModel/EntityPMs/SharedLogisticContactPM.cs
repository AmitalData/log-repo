using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class SharedLogisticContactPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CardId { get; set; }
        public string ContactId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool InternetAccess { get; set; }
        public string EnglishName { get; set; }
        public string Position { get; set; }
        public string BusinessPhone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool IsCargoTrackingInvitation { get; set; }
    }
}