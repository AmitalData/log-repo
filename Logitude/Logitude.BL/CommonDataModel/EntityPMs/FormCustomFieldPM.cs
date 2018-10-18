using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class FormCustomFieldPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DocumentTypeId { get; set; }
        public string ObjectTableId { get; set; }
        public string EntityId { get; set; }
        public string FieldCode { get; set; }
        public string Value { get; set; }
    }
}