using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class ReportPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        public string Description { get; set; }
        public string SearchFields { get; set; }
        public string FilterControlName { get; set; }
        public bool IsSecured { get; set; }
        public byte[] ReportBody { get; set; }
        public bool hasTemplate { get; set; }
        public string ReportGroupId { get; set; }
        public string FeatureId { get; set; }
        public string FeatureCode { get; set; }
        public string ReportDocumentId { get; set; }
        public bool InActive { get; set; }
        public string FilterHtmlComponentUrl { get; set; }
        public string DefaultTemplateId { get; set; }
        public string DefaultMessageTemplateId { get; set; }
        public string FeatureUniqeCode { get; set; }
        public bool AvailableForScheduling { get; set; }
        public bool DisablePreview { get; set; }
        public string DefaultExcelTemplateId { get; set; }
        public bool IsExcelReportAllowed { get; set; }
    }
}