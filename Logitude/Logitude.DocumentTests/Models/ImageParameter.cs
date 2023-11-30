using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.DocumentTests.Models
{
    public class ImageParameter
    {
        public bool IsReadDocumentFromBarCode { get; set; }
        public bool IsCheckedSecuritykey { get; set; }
        public string ShipmentNumber { get; set; }
        public string Key { get; set; }
        public byte[] buffer { get; set; }
        public int FileSize { get; set; }
        public int SentSize { get; set; }
        public byte[] FileData { get; set; }
        public int Buffersize { get; set; }

        public int BufferNumber { get; set; }
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public List<string> BlockIdsList { get; set; }

        public string ContactId { get; set; }
        
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string ObjectTableId { get; set; }
        public string DocumentTypeId { get; set; }
        public string ShipmentId { get; set; }
        public string UserId { get; set; }
        public string DocumentsFilingId { get; set; }
        public string DocumentId { get; set; }
        public string FileLocation { get; set; }
 
        public string EncodedFileName { get; set; }

        public double BlocksNumber { get; set; }
        public int Position { get; set; }
        public string Result { get; set; }
        public string Base64String { get; set; }
        public string Extension { get; set; }

        public bool IsFirstTry { get; set; }
        public string UploadMode { get; set; }
        public string UploadType { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        
        public string PhoneNumber { get; set; }
        public string DeviceName { get; set; }
        public string SecurityKey { get; set; }
        public bool KeepOriginalSize { get; set; }
    }
}