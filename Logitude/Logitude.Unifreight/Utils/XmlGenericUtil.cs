using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Logitude.AmitalMessaging.Utils
{
    public class XmlGenericUtil<MyType> where MyType : class
    {

        public static string SerilazeObjectWithoutDefaultNameSpace(MyType myTObject)
        {
            XmlSerializerNamespaces myXmlSerializerNamespaces = new XmlSerializerNamespaces();
            myXmlSerializerNamespaces.Add(string.Empty,
                //string.Empty
                @"http://tempuri.org/gfusts");
            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;


            using (var ms = new MemoryStream()) using (var sw = XmlWriter.Create(ms, settings))
            {
                var myXmlSerializer = new XmlSerializer(typeof(MyType));

                //                To remove default namespaces: 





                myXmlSerializer.Serialize(ms, myTObject, myXmlSerializerNamespaces);

                // convert stream to string
                ms.Position = 0;
                StreamReader reader = new StreamReader(ms);
                string text = reader.ReadToEnd();





                return text;
            }

        }
        public static string SerilazeObjectWithoutDefaultNameSpace1(MyType myTObject)
        {
            using (var stringWriter = new StringWriter())
            {
                var myXmlSerializer = new XmlSerializer(typeof(MyType));

                //                To remove default namespaces: 



                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);

                myXmlSerializer.Serialize(stringWriter, myTObject, namespaces);

                var text = stringWriter.ToString();

                return text;
            }

        }
        public static string SerilazeObject(MyType myTObject, bool toRemoveDefaultNameSpace = false)
        {
            using (var stream = new MemoryStream())
            {
                var myXmlSerializer = new XmlSerializer(typeof(MyType));
                myXmlSerializer.Serialize(stream, myTObject);
                // convert stream to string
                stream.Position = 0;
                StreamReader reader = new StreamReader(stream);
                string text = reader.ReadToEnd();


                if (toRemoveDefaultNameSpace)
                {
                    text = stripNS(XElement.Parse(text)).ToString();
                }
                return text;
            }

        }
        static XElement stripNS(XElement root)
        {
            XElement res = new XElement(
                root.Name.LocalName,
                root.HasElements ?
                    root.Elements().Select(el => stripNS(el)) :
                    (object)root.Value
            );

            res.ReplaceAttributes(
                root.Attributes().Where(attr => (!attr.IsNamespaceDeclaration)));

            return res;
        }
        public static MyType DeserilazeObject(string Xml)
        {

            XmlSerializer ser;






            MyType myTObject = null;
            using (var stream = new MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(Xml)))
            {
                var myXmlSerializer = new XmlSerializer(typeof(MyType), GetDefaultNamespace(typeof(MyType)));
                var myObject = myXmlSerializer.Deserialize(stream);

                myTObject = myObject as MyType;
                // convert stream to string

            }
            return myTObject;

        }

        public static string GetDefaultNamespace(Type MyType)
        {
            object[] objArr = MyType.GetCustomAttributes(typeof(XmlTypeAttribute), false);
            if (objArr == null) return null;
            if (objArr.Count() < 1) return null;

            var xmltype = objArr[0] as XmlTypeAttribute;
            if (xmltype == null) return null;
            var att = new XmlRootAttribute { Namespace = xmltype.Namespace };
            return att.Namespace;
        }
    }
}
