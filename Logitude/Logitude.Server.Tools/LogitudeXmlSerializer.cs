using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Logitude.Server.Tools
{
    public class LogitudeXmlSerializer
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


        public static string SerializeObjectToXmlElementString<T>(T myObject)
        {

            MemoryStream memstream = new MemoryStream();
            XmlSerializer serilaizer = new XmlSerializer(typeof(T));
            serilaizer.Serialize(memstream, myObject);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            memstream.Seek(0, SeekOrigin.Begin);

            XElement element = XElement.Load(memstream);

            return element.ToString();

        }


        public static string SerializeObjectToElementString<T>(T myObject, Type[] knowntypes = null)
        {

            MemoryStream memoryStream = new MemoryStream();

            var serializer = new DataContractSerializer(typeof(T));
            if (knowntypes != null)
            {
                serializer = new DataContractSerializer(typeof(T), knowntypes);

            }
            serializer.WriteObject(memoryStream, myObject);

            memoryStream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(memoryStream, Encoding.UTF8);
            string content = reader.ReadToEnd();
            content = content.Replace(" />", "/>");

            return content;


        }


        
        public static string SerializeObjectToXmlString<T>(T myObject , bool useObjectGetType = false)
        {
            MemoryStream memstream = new MemoryStream();
            XmlSerializer serilaizer = new XmlSerializer(useObjectGetType ? myObject.GetType() : typeof(T));
            serilaizer.Serialize(memstream, myObject);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            memstream.Seek(0, SeekOrigin.Begin);

            XmlDocument doc = new XmlDocument();
            doc.Load(memstream);

            return doc.InnerXml;
        }

        public static string SerializeObjectToUTF8XmlString<T>(T myObject)
        {

            MemoryStream memstream = new MemoryStream();
            XmlSerializer serilaizer = new XmlSerializer(typeof(T));
            var streamWriter = new StreamWriter(memstream, System.Text.Encoding.UTF8);
            serilaizer.Serialize(streamWriter, myObject);
            byte[] utf8EncodedXml = memstream.ToArray();
            string xml = Encoding.UTF8.GetString(utf8EncodedXml);
            string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (xml.StartsWith(_byteOrderMarkUtf8))
            {
                xml = xml.Remove(0, _byteOrderMarkUtf8.Length);
            }
            // doc.LoadXml(xml);
            //memstream.Seek(0, SeekOrigin.Begin);
            //var reader = new StreamReader(memstream, Encoding.UTF8);
            //string content = reader.ReadToEnd();
            //memstream.Seek(0, SeekOrigin.Begin);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            return doc.InnerXml;
        }

        public static string SerializeObjectToJosnString<T>(T myObject)
        {

            string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(myObject);

            return json;
        }
        public static string SerializeObjectToJosnStringMax<T>(T myObject)
        {

            string json = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = int.MaxValue }.Serialize(myObject);

            return json;
        }
        public static object JsonConvertDeserializeObject(string json)
        {
            var objT = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            

            return objT;
        }
        //public static string SerializeObjectToJosnStringMax<T>(T myObject)
        //{

        //    string json = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = int.MaxValue }.Serialize(myObject);

        //    return json;
        //}
        public static T JsonConvertDeserializeTObject<T>(string json)
        {
            var objT = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);


            return objT;
        }

        public static T DeserializeElementObject<T>(string xml)
        {
            using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(xml)))
            {
                var serializer = new DataContractSerializer(typeof(T));
                T theObject = (T)serializer.ReadObject(stream);
                return theObject;
            }
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
  
    }
}
