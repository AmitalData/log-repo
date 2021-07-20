using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class DocumentFile
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string FileName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Extension { get; set; }
        public double? FileSize { get; set; } 
        public string Folder { get; set; }   
        public byte[] FileData { get; set; }
    }
}