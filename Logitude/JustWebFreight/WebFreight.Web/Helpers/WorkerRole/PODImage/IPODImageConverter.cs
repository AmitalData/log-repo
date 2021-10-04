using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.WorkerRole.PODImage
{
    public interface IPODImageConverter
    {
        string Extention { get; }
        byte[] Convert(byte[] fileData);
    }
}
