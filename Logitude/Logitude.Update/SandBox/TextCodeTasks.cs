using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Logitude.Update.SandBox
{
    public class TextCodeTasks
    {
        public const string SourceConnection = //"TextCodeCopy,sa,Saas256,AmitalDB";
            "Amital1_Main,sa,Saas256,.";
        public const string TargetConnection = //"TextCodeCopy,sa,Saas256,AmitalDB";
            //TargetConnection = 
             "User Id=logitude_main;  Password=oracle;Direct=True;Data Source=localhost;port=1521;sid=xe";
           // "User Id=logitude_main;  Password=oracle;Direct=True;Data Source=10.10.10.58;port=1521;sid=amital";
            //"Amital1_Main,sa,Saas256,.";
        public static  void SaveTextCodeToDisk(string fileName = @"c:\TextCodeDTO.xml")
        {
            int tenant = 0;
            Dictionary<string, TextCode> LocalDefaultTextSource = new Dictionary<string, TextCode>();
            List<TextCode> LocalDefaultTextTarget = new List<TextCode>();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(2, 30, 0)))
            {
                //DbConnection connection = DatabaseInitializer.GetConnection(SourceConnection);

                IWebFreightContext context = //new WebFreightContext(connection);
                    WebFreightContext.GetContext(tenant);
                TextCodeRepository textCodeRepository = new TextCodeRepository(context);

                LocalDefaultTextSource = (from a in textCodeRepository.GetTextCodes()
                                          where !string.IsNullOrEmpty(a.LocalDefaultText) & a.IsSpellChecked
                                          select a).ToDictionary(d => d.Code, t => t);
                scope.Complete();
            }
            List<TextCodeDTO> listTextCodeDTO = LocalDefaultTextSource.Values.Select(rec => new TextCodeDTO()
            {
                Code = rec.Code,
                DefaultText = rec.DefaultText,
                DefaultTextPlural = rec.DefaultTextPlural,
                Id = rec.Id,
                InActive = rec.InActive,
                IsSpellChecked = rec.IsSpellChecked,
                LocalDefaultText = rec.LocalDefaultText,

                ObjectTableId = rec.ObjectTableId,
                SpellCheckDate = rec.SpellCheckDate,

                SpellCheckedByUserId = rec.SpellCheckedByUserId,
                Tenant = rec.Tenant,

                TextCodeTypeCode = rec.TextCodeTypeCode
            }).ToList();
            var xml = XmlGenericUtil<List<TextCodeDTO>>.SerializeObject(listTextCodeDTO);
            File.WriteAllText(fileName, xml);
        }
        public static void LoadTextCodeFromDisk(string fileName = @"c:\TextCodeDTO.xml")
        {
            int tenant = 0;
            Dictionary<string, TextCodeDTO> LocalDefaultTextSource = new Dictionary<string, TextCodeDTO>();


            var xxxml = File.ReadAllText(fileName);
            List<TextCodeDTO> myTextCodeDTOList = XmlGenericUtil<List<TextCodeDTO>>.DeSerializeObject(xxxml);
            LocalDefaultTextSource = myTextCodeDTOList.ToDictionary(d => d.Code, t => t);
            //LocalDefaultTextSource  =  listtt.ToDictionary( 


            


            List<TextCode> LocalDefaultTextTarget = new List<TextCode>();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(2, 30, 0)))
            {
                //LogitudeSettings.DatabaseManagementSystem="oracle";
                //DbConnection connection = DatabaseInitializer.GetConnection(TargetConnection);
                IWebFreightContext context = 
                    //new WebFreightContext(connection);
                     WebFreightContext.GetContext(tenant);
                TextCodeRepository textCodeRepository = new TextCodeRepository(context);
                LocalDefaultTextTarget = (from a in textCodeRepository.GetTextCodes()
                                          where LocalDefaultTextSource.Keys.Contains(a.Code)
                                          select a).ToList();

                int counter = 0;
                foreach (TextCode textCode in LocalDefaultTextTarget)
                {
                    if (!LocalDefaultTextSource.ContainsKey(textCode.Code))
                    {
                        continue;
                    }
                    textCode.LocalDefaultText = LocalDefaultTextSource[textCode.Code].LocalDefaultText;
                    textCode.IsSpellChecked = LocalDefaultTextSource[textCode.Code].IsSpellChecked;
                    textCode.DefaultText = LocalDefaultTextSource[textCode.Code].DefaultText;
                    textCodeRepository.Update(textCode);
                    counter++;
                    if (counter == 500)
                    {
                        textCodeRepository.SubmitChanges();
                        counter = 0;
                    }
                }

                textCodeRepository.SubmitChanges();
                scope.Complete();
            }
        }
    }

      #region MyRegion
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
            var ser = new XmlSerializer(typeof(MyType), xmltype.Namespace);


            ser.Serialize(stream, MyObject);
            //stream.Seek(0, SeekOrigin.Begin);
            stream.Position = 0;

            return stream;
        }
        public static MemoryStream MemoryStreamSerialize<MyType>(MyType MyObject)
            where MyType : class
        {
            MemoryStream stream = new MemoryStream();
            var ser = new XmlSerializer(typeof(MyType));


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
        public static string SerializeObjectWithoutDefaultNameSpace1(MyType myTObject)
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
                    myXmlSerializer = new XmlSerializer(typeof(MyType));
                }
                else
                {
                    myXmlSerializer = new XmlSerializer(typeof(MyType), defaultNamespace);
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
            var ser = new XmlSerializer(typeof(MyType), o);

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
                var myXmlSerializer = new XmlSerializer(typeof(MyType), xmlAttributeOverrides);
                if (attrs.XmlRoot == null)
                {
                    xmlAttributeOverrides = null;
                    var defaultNamespace = GetDefaultNamespace(typeof(MyType));

                    myXmlSerializer = new XmlSerializer(typeof(MyType), defaultNamespace);
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
                        var myXmlSerializer = new XmlSerializer(typeof(MyType), null, null, GetXmlRootAttribute(typeof(MyType)), defaultNamespace);

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

        private static object DeSerWODefaultNameSpace2(string Xml, string defaultNamespace, Object myObject)
        {
            var myXDocument = XDocument.Parse(Xml);
            myXDocument.Root.SetDefaultXmlNamespace(defaultNamespace);
            var xmlWithNS = myXDocument.ToString();
            using (var stream = new MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(xmlWithNS)))
            {
                var myXmlSerializer = new XmlSerializer(typeof(MyType));

                myObject = myXmlSerializer.Deserialize(stream);
            }
            return myObject;
        }

        private static object DeSerWODefaultNameSpace1(string Xml, Object myObject)
        {
            using (var stream = new MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(Xml)))
            {
                var myXmlSerializer = new XmlSerializer(typeof(MyType));

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
                var myXmlSerializer = new XmlSerializer(typeof(MyType), xRoot);

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

   
        #endregion
}
