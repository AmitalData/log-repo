using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class CommunicationLogExtendedArgs
    {
        public int Tenant { get; set; }
        public string ObjectTableName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string CommunicationLogTypeCode { get; set; }
        public int Priority { get; set; }
        public string InOut { get; set; }
        public string CommunicationStatusTypeCode { get; set; }
        public string EntityId { get; set; }
        public string Subject { get; set; }
        public string FolderName { get; set; }
        public string MessageBody { get; set; }
        public string FileExtension { get; set; }
        public string MessageException { get; set; }
    }
}
