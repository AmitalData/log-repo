using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class DocumentTypeMetaDataPM
    {
        [Key]
        public string Id { get; set; }

        public string DocumentTypeId { get; set; }

        public string DocumentsMetaDataTypeId { get; set; }

        public int Tenant { get; set; }

        public string DocumentsMetaDataTypeCode { get; set; }

        public string DocumentsMetaDataTypeEnglishName{ get; set; }

        public string DocumentsMetaDataTypeLocalName { get; set; }

        public string DocumentsMetaDataTypeFormat { get; set; }

        public bool Mandatory { get; set; }

       

    }
}