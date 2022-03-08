using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
// System.Runtime.Serialization.dll (.NET 3.0)



namespace Logitude.AmitalMessaging.Utils
{
    public class XmlGenericUtil<MyType> where MyType : class
    {

        public static MemoryStream MemoryStreamSerializeWithDefaultNamespace<MyType>(MyType MyObject)
            where MyType : class
        {
            MemoryStream stream = new MemoryStream();

            object[] objArr = typeof(MyType).GetCustomAttributes(typeof(XmlTypeAttribute), false);
            if (objArr == null || objArr.Length < 1)
            {
                throw new Exception("GetXmlSerializerWithDefaultNamespace<MyType>():No default Namespace in the class !!!");
            }
            var xmltype = objArr[0] as XmlTypeAttribute;
            var ser = CachingXmlSerializerFactory.Create(typeof(MyType), xmltype.Namespace);


            ser.Serialize(stream, MyObject);
            //stream.Seek(0, SeekOrigin.Begin);
            stream.Position = 0;

            return stream;
        }
        public static MemoryStream MemoryStreamSerialize<MyType>(MyType MyObject)
            where MyType : class
        {
            MemoryStream stream = new MemoryStream();
            var ser = CachingXmlSerializerFactory.Create(typeof(MyType));


            ser.Serialize(stream, MyObject);
            //stream.Seek(0, SeekOrigin.Begin);
            stream.Position = 0;

            return stream;
        }
        
        public static string SerializeObjectWithoutDefaultNameSpace(MyType myTObject)
        {
            XmlSerializerNamespaces myXmlSerializerNamespaces = new XmlSerializerNamespaces();
            myXmlSerializerNamespaces.Add(string.Empty,
                //string.Empty
                @"http://tempuri.org/gfusts");
            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;


            using (var ms = new MemoryStream()) using (var sw = XmlWriter.Create(ms, settings))
            {
                var myXmlSerializer = CachingXmlSerializerFactory.Create(typeof(MyType));

                //                To remove default namespaces: 





                myXmlSerializer.Serialize(ms, myTObject, myXmlSerializerNamespaces);

                // convert stream to string
                ms.Position = 0;
                StreamReader reader = new StreamReader(ms);
                string text = reader.ReadToEnd();





                return text;
            }

        }
        public static string SerializeObjectWithoutDefaultNameSpace1(MyType myTObject)
        {
            using (var stringWriter = new StringWriter())
            {
                var myXmlSerializer = CachingXmlSerializerFactory.Create(typeof(MyType));

                //                To remove default namespaces: 



                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);

                myXmlSerializer.Serialize(stringWriter, myTObject, namespaces);

                var text = stringWriter.ToString();

                return text;
            }

        }
        public static string SerializeObject(MyType myTObject, bool toRemoveDefaultNameSpace = false)
        {
            string defaultNamespace = null;
            XmlSerializer myXmlSerializer = null;
            using (var stream = new MemoryStream())
            {
                bool getDefaultNamespace = true;
                if (getDefaultNamespace)
                {
                    defaultNamespace = GetDefaultNamespace(typeof(MyType));
                }
                if (string.IsNullOrWhiteSpace(defaultNamespace))
                {
                    myXmlSerializer = CachingXmlSerializerFactory.Create(typeof(MyType));
                }
                else
                {
                    myXmlSerializer = CachingXmlSerializerFactory.Create(typeof(MyType), defaultNamespace);
                }
                
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

        public static string MySerializeObject(MyType myTObject)
        {
            XmlSerializer myXmlSerializer = null;
            using (var stream = new MemoryStream())
            {
                myXmlSerializer = CachingXmlSerializerFactory.Create(typeof(MyType));
                myXmlSerializer.Serialize(stream, myTObject);
                stream.Position = 0;
                StreamReader reader = new StreamReader(stream);
                string text = reader.ReadToEnd();
                text = mystripNS(XElement.Parse(text)).ToString();
                return text;
            }
        }
        
        static XElement mystripNS(XElement root)
        {
            XElement res = new XElement(
                root.Name.LocalName,
                root.HasElements ?
                    root.Elements().Select(el => mystripNS(el)) :
                    (object)root.Value
            );

            return res;
        }
        public static MyType DeserializeWithNoRootNamespace(string data)
        //where T : class
        {

            /* Each overridden field, property, or type requires
               an XmlAttributes object. We are overriding the serialization
               of the XmlRoot on the type, so we need one XmlAttributes collection. */
            var attrs = new XmlAttributes();

            /* Create an XmlRootAttribute to override the XmlRoot in-line attr.
               The override will use no namespace. */
            var root = new XmlRootAttribute("register-account");

            // set the XmlRoot on the XmlAttributes collection.
            attrs.XmlRoot = root;


            // Create the XmlAttributeOverrides object.
            var o = new XmlAttributeOverrides();

            /* Map the attributes collection to the thing being 
               overriden (the class). */
            o.Add(typeof(MyType), attrs);

            // create the serializer, specifying the overrides to use
            var ser = CachingXmlSerializerFactory.Create(typeof(MyType), o);

            using (var sr = new StringReader(data))
            {
                return (MyType)ser.Deserialize(sr);
            }
        }
        public static MyType DeSerializeObject_bad(string Xml)///, string defaultNamespace = "")
        {
            XmlSerializer ser;

            try
            {



                /* Each overridden field, property, or type requires
               an XmlAttributes object. We are overriding the serialization
               of the XmlRoot on the type, so we need one XmlAttributes collection. */
                var attrs = new XmlAttributes();

                /* Create an XmlRootAttribute to override the XmlRoot in-line attr.
                   The override will use no namespace. */
                XmlRootAttribute root = GetXmlRootAttribute(typeof(MyType));

                // set the XmlRoot on the XmlAttributes collection.
                attrs.XmlRoot = root;
                attrs.XmlType = GetXmlTypeAttribute(typeof(MyType));

                // Create the XmlAttributeOverrides object.
                var xmlAttributeOverrides = new XmlAttributeOverrides();

                /* Map the attributes collection to the thing being 
                   overriden (the class). */
                xmlAttributeOverrides.Add(typeof(MyType), attrs);
                var myXmlSerializer = CachingXmlSerializerFactory.Create(typeof(MyType), xmlAttributeOverrides);
                if (attrs.XmlRoot == null)
                {
                    xmlAttributeOverrides = null;
                    var defaultNamespace = GetDefaultNamespace(typeof(MyType));

                    myXmlSerializer = CachingXmlSerializerFactory.Create(typeof(MyType), defaultNamespace);
                }

                MyType myTObject = null;
                using (var stream = new MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(Xml)))
                {

                    var myObject = myXmlSerializer.Deserialize(stream);

                    myTObject = myObject as MyType;
                    // convert stream to string

                }

                return myTObject;
            }
            catch (Exception eee)
            {

                throw;
            }
        }

        private static XmlTypeAttribute GetXmlTypeAttribute(Type MyType)
        {
            object[] objArr = MyType.GetCustomAttributes(typeof(XmlTypeAttribute), false);
            if (objArr == null) return null;
            if (objArr.Count() < 1) return null;
            var xmltype = objArr[0] as XmlTypeAttribute;

            return xmltype;
        }

        private static XmlRootAttribute GetXmlRootAttribute(Type MyType)
        {
            object[] objArr = MyType.GetCustomAttributes(typeof(XmlRootAttribute), false);
            if (objArr == null) return null;
            if (objArr.Count() < 1) return null;
            var xmlRoot = objArr[0] as XmlRootAttribute;

            return xmlRoot;
        }
        
        public static MyType DeSerializeObject(string Xml)//, bool toStripNS = true )
        {

            XmlSerializer ser;

            if (string.IsNullOrWhiteSpace(Xml))
            {
                throw new Exception("why the hell the XML IsNullOrWhiteSpace ");
            }

            try
            {
                //if (toStripNS)
                //{
                //    Xml = stripNS(XElement.Parse(Xml)).ToString();
                //}


                var defaultNamespace = GetDefaultNamespace(typeof(MyType));
                MyType myTObject = null;
                Object myObject = null;

                try
                {
                    using (var stream = new MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(Xml)))
                    {
                        var myXmlSerializer = //CachingXmlSerializerFactory.Create(typeof(MyType), null, null, GetXmlRootAttribute(typeof(MyType)), defaultNamespace);
                            CachingXmlSerializerFactory.Create(typeof(MyType), GetXmlRootAttribute(typeof(MyType)), defaultNamespace);
#if false
                        myXmlSerializer.UnreferencedObject += myXmlSerializer_UnreferencedObject;
                        myXmlSerializer.UnknownNode += myXmlSerializer_UnknownNode;
                        myXmlSerializer.UnknownElement += myXmlSerializer_UnknownElement;
                        myXmlSerializer.UnknownAttribute += myXmlSerializer_UnknownAttribute;
#endif
                        myObject = myXmlSerializer.Deserialize(stream);
                    }
                }
                catch (System.InvalidOperationException myInvalidOperationException)
                {
                    try
                    {
                        myObject = DeSerWODefaultNameSpace1(Xml, myObject);
                    }
                    catch (System.InvalidOperationException myInvalidOperationException2)
                    {
                       // myObject = DeserializeTry1(Xml, defaultNamespace, myObject);
                        myObject = DeSerWODefaultNameSpace2(Xml, defaultNamespace, myObject);
                    }
                    
                }


                myTObject = myObject as MyType;
                // convert stream to string



                return myTObject;
            }

            catch (Exception eee)
            {

                throw;
            }
        }

        static void myXmlSerializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            //throw new NotImplementedException();
        }

        static void myXmlSerializer_UnknownElement(object sender, XmlElementEventArgs e)
        {
            //throw new NotImplementedException();
        }

        static void myXmlSerializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            //throw new NotImplementedException();
        }

        static void myXmlSerializer_UnreferencedObject(object sender, UnreferencedObjectEventArgs e)
        {
//            throw new NotImplementedException();
        }

        private static object DeSerWODefaultNameSpace2(string Xml, string defaultNamespace, Object myObject)
        {
            var myXDocument = XDocument.Parse(Xml);
            myXDocument.Root.SetDefaultXmlNamespace(defaultNamespace);
            var xmlWithNS = myXDocument.ToString();
            using (var stream = new MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(xmlWithNS)))
            {
                var myXmlSerializer = CachingXmlSerializerFactory.Create(typeof(MyType));

                myObject = myXmlSerializer.Deserialize(stream);
            }
            return myObject;
        }

        private static object DeSerWODefaultNameSpace1(string Xml, Object myObject)
        {
            using (var stream = new MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(Xml)))
            {
                var myXmlSerializer = CachingXmlSerializerFactory.Create(typeof(MyType));

                myObject = myXmlSerializer.Deserialize(stream);
            }
            return myObject;
        }
        


        private static object DeserializeTry1(string Xml, string defaultNamespace, Object myObject)
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = defaultNamespace;
            xRoot.IsNullable = true;



            using (var stream = new MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(Xml)))
            {
                var myXmlSerializer = CachingXmlSerializerFactory.Create(typeof(MyType), xRoot);

                myObject = myXmlSerializer.Deserialize(stream);
            }
            return myObject;
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

    ///////////////////////////////////////////

    public static class MyExtensionClass
    {
        public static void SetDefaultXmlNamespace(this XElement xelem, XNamespace xmlns)
        {
            if (xelem.Name.NamespaceName == string.Empty)
                xelem.Name = xmlns + xelem.Name.LocalName;
            foreach (var e in xelem.Elements())
                e.SetDefaultXmlNamespace(xmlns);
        }
    }

}
