using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HypredTest
{ 
    public class ObjectXmlSerializer
    {

        public static byte[] SerializeObject<T>(T myObject)
        {

            MemoryStream memstream = new MemoryStream();
            XmlSerializer serilaizer = new XmlSerializer(typeof(T));
            serilaizer.Serialize(memstream, myObject);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();


            return bytearray;
        }

        public static string SerializeObjectToXmlString<T>(T myObject)
        {

            MemoryStream memstream = new MemoryStream();
            XmlSerializer serilaizer = new XmlSerializer(typeof(T));
            serilaizer.Serialize(memstream, myObject);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            memstream.Seek(0, SeekOrigin.Begin);

            XmlDocument doc = new XmlDocument();
            doc.Load(memstream);

            return doc.InnerXml;
        }



        public static T DeserializeObject<T>(string xmlstring)
        {
            XmlSerializer serilaizer = new XmlSerializer(typeof(T));
            using (var sr = new StringReader(xmlstring))
            {
                return (T)serilaizer.Deserialize(sr);
            }

        }

        public static T DeserializeObject<T>(byte[] byteData)
        {

            MemoryStream memstream = new MemoryStream(byteData);
            XmlSerializer serilaizer = new XmlSerializer(typeof(T));

            return (T)serilaizer.Deserialize(memstream);


        }
        public static T DeserializeObject<T>(Stream stream)
        {

             
            XmlSerializer serilaizer = new XmlSerializer(typeof(T));

            return (T)serilaizer.Deserialize(stream);


        }
    }
}
