using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.BL.DataContracts
{
    public class FileInformation
    {
        public string ShipmentNumber { get; set; }
        public string Key { get; set; } 
        public int FileSize { get; set; }
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public string[] BlockIdsList { get; set; }
        public byte[] buffer { get; set; }
        public int BufferNumber { get; set; }
        public long SentSize { get; set; } 
        public string FileName { get; set; }
        public string ObjectTableId { get; set; }
        public string DocumentTypeId { get; set; }
        public string ShipmentId { get; set; }
        public string UserId { get; set; }
        public string DocumentsFilingId { get; set; }
        public string DocumentId { get; set; }
        
    }
}