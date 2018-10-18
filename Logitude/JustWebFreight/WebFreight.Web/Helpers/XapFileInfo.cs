using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class XapFileInfo
    {


        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public bool IsStaging { get; set; }

    }
}