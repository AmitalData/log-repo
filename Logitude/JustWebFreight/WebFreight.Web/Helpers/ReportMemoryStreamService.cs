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
            //if(FeatureToggleHelper.HasFeatureToggle("DMS", tenant))
            //{
            return SerializeDataWithUsingMemoryStream(dataprovider, type);
            //}
            //return SerializeData(new MemoryStream(),dataprovider, type);
        }

        private byte[] SerializeDataWithUsingMemoryStream(object dataprovider, Type type)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                byte[] bytearray = SerializeDataWithUsingStreamReader(memoryStream, dataprovider, type);
                memoryStream.Dispose();
                memoryStream.Close();
                return bytearray;
            }
        }

        private byte[] SerializeDataWithUsingStreamReader(MemoryStream memoryStream, object dataprovider, Type type)
        {
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                XmlSerializer serializer = new XmlSerializer(type);
                serializer.Serialize(memoryStream, dataprovider);
                memoryStream.Seek(0, SeekOrigin.Begin);
                string content = reader.ReadToEnd();
                byte[] bytearray = memoryStream.ToArray();
                reader.Dispose();
                reader.Close();
                return bytearray;
            }
        }

        public byte[] SerializeData(MemoryStream memoryStream , object dataprovider, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            serializer.Serialize(memoryStream, dataprovider);
            memoryStream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memoryStream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }
    }
}