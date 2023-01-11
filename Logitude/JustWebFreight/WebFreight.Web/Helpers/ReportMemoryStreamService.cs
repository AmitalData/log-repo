using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WebFreight.Web.Services
{
    public class ReportMemoryStreamService
    {
        public byte[] Convert(Object dataprovider, Type type, int tenant)
        {
            if(FeatureToggleHelper.HasFeatureToggle("DMS", tenant))
            {
                using (MemoryStream memstream = new MemoryStream())
                {
                    XmlSerializer serializer = new XmlSerializer(type);
                    serializer.Serialize(memstream, dataprovider);
                    memstream.Seek(0, SeekOrigin.Begin);
                    var reader = new StreamReader(memstream);
                    string content = reader.ReadToEnd();
                    byte[] bytearray = memstream.ToArray();
                    memstream.Dispose();
                    memstream.Close();
                    return bytearray;
                }
            }
            else
            {
                XmlSerializer serializer = new XmlSerializer(type);
                MemoryStream memstream = new MemoryStream();
                serializer.Serialize(memstream, dataprovider);
                memstream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(memstream);
                string content = reader.ReadToEnd();
                byte[] bytearray = memstream.ToArray();
                return bytearray;
            }
        }
    }
}