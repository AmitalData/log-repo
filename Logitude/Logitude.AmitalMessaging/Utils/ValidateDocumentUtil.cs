
using Logitude.AmitalMessaging.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;


namespace Logitude.AmitalMessaging.Utils
{
    public class ValidateDocumentUtil
    {
        public ValidateDocumentUtil()
        {
            ///http://msdn.microsoft.com/en-us/library/ff647820.aspx
            ///How to: Perform Message Validation with Schema Validation in WCF
        }
        private bool _Valid;

        public bool IsValid
        {
            get { return _Valid; }
            private set { _Valid = value; }
        }
        public string ErrorMessage;
        public void ValidateObjectAgainstWSDL<T>(T obj, string wsdlName) where T : class
        {
            _Valid = true;
            ErrorMessage = "";
            string errorMessage = "";
            XmlReaderSettings settings = null;
            //var leak2 = LoadTestUtil.GetLeak();
            ValidationEventHandler delg = (object sender, ValidationEventArgs e) =>
            {

                //var leak = LoadTestUtil.GetLeak();
                _Valid = false;
                errorMessage += e.Message + Environment.NewLine;
            };
            try
            {

                XmlSchemaSet myXmlSchemaSet;
                try
                {
                    myXmlSchemaSet = GetAllSchemaFromWSDL(obj, wsdlName);
                }
                catch (Exception e)
                {
                    errorMessage = "Unable to Found (Embedded Resource) wsdlName =" + wsdlName + Environment.NewLine + e.ToString();
                    throw new FaultException<UnifreightIIGFault>(UnifreightIIGFault.GetUnifreightValidation("ValidateDocument Exception:" + errorMessage));
                    return;
                }


                // Set the validation settings
                LogMe("Set the validation settings");
                ///XmlReaderSettings 
                settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas = myXmlSchemaSet;
                //settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
                settings.ValidationEventHandler += delg;
                //(object sender, ValidationEventArgs e) =>
                //{
                //    _Valid = false;
                //    errorMessage += e.Message + Environment.NewLine;
                //};

                using (var stream = new MemoryStream())
                {
                    object[] objArr = typeof(T).GetCustomAttributes(typeof(XmlTypeAttribute), false);
                    var xmltype = objArr[0] as XmlTypeAttribute;
                    var att = new XmlRootAttribute { Namespace = xmltype.Namespace };

                    LogMe("Serialize object" + typeof(T).FullName.ToString());
                    // serialize object to byte array
                    XmlSerializer ser = //new XmlSerializer(
                        CachingXmlSerializerFactory.Create(typeof(T), att);
                    ser.Serialize(stream, obj);

                    stream.Seek(0, SeekOrigin.Begin);
                    LogMe("Object XML:");
                    LogMe(Encoding.UTF8.GetString(stream.ToArray()));


                    //System.Xml.Linq.XDocument xDoc = System.Xml.Linq.XDocument.Parse(p_xml);
                    //xDoc.Validate(v_schemas, (o, e) =>
                    //{
                    //    throw new Exception("Validation against schema failed: Validation error was thrown." + Environment.NewLine + e.Message, e.Exception);
                    //});


                    using (XmlReader reader = XmlReader.Create(stream, settings))
                    {
                        while (reader.Read())
                        {

                        }
                    }
                    //reader.Close();
                }

                return;
            }
            catch (System.Exception ex)
            {
                _Valid = false;
                LogMe("Error in Validating WSDL " + wsdlName);
                LogMe("Error : " + ex);
                ErrorMessage = ex.ToString();
                _Valid = false;
                return;
            }
            finally
            {

                if (settings != null)
                {
                    settings.ValidationEventHandler -= delg;
                    //settings.ValidationEventHandler -= (sender, e) => { };
                }
                delg = null;
                if (!_Valid)
                {
                    ErrorMessage = errorMessage;
                    throw new FaultException<UnifreightIIGFault>(UnifreightIIGFault.GetUnifreightValidation("ValidateDocument Exception:" + errorMessage));
                }
            }

        }

        struct MyXmlSchemaSetStruct
        {
            public string MyWSDL;
            public XmlSchemaSet MyXmlSchemaSet;
        }
        private static List<MyXmlSchemaSetStruct> MyCache = new List<MyXmlSchemaSetStruct>();

        private static XmlSchemaSet GetAllSchemaFromWSDL<T>(T obj, string wsdlName)
        {
            wsdlName = wsdlName.ToUpper();
            var my = MyCache.FirstOrDefault(
                r => r.MyWSDL.ToUpper().Equals(wsdlName)
                );
            if (!String.IsNullOrWhiteSpace(my.MyWSDL))
            {
                return my.MyXmlSchemaSet;
            }
            // Create WsdlExporter
            WsdlExporter exporter = new WsdlExporter();
            System.Web.Services.Description.ServiceDescription description;
            try
            {
                using (var myManifestResourceStream = ManifestResourceUtil.GetManifestResourceStreamFromWSDL<T>(obj, wsdlName))
                {
                    description = System.Web.Services.Description.ServiceDescription.Read(myManifestResourceStream);
                }
            }
            catch (Exception e)
            {

                throw new Exception("Unable to Found (Embedded Resource!!!!) wsdlName =" + wsdlName + Environment.NewLine + e.ToString());

            }




            var schemas = new XmlSchemaSet();
            exporter.GeneratedWsdlDocuments.Add(description);

            //LogMe("Loading XSD From DataPower WSDL");

            // Loading XSD From DataPower WSDL Not All Schemas Are in the exporter.GeneratedWsdlDocuments[0] Collection 
            int i = 0;
            foreach (XmlSchema schema in exporter.GeneratedWsdlDocuments[0].Types.Schemas)
            {
                if (true)
                {
                    schemas.Add(schema);
                }
                else
                {
                    var filename = schema.Id + ".xsd";
                    filename = Path.Combine(Path.GetTempPath(), filename);
                    try
                    {
                        Debug.WriteLine("try get schema by writing to " + filename);
                        FileStream file = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite);
                        XmlTextWriter xwriter = new XmlTextWriter(file, new UTF8Encoding());
                        xwriter.Formatting = Formatting.Indented;
                        schema.Write(xwriter);
                        // Add Schema
                        schemas.Add(schema);

                        file.Close();
                        if (File.Exists(filename))
                        {
                            i++;
                            //File.Copy(filename, "itzik." + i.ToString() + filename);
                            File.Delete(filename);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("failed using " + filename);
                        throw;
                    }
                }
                
                
            }
            var myNew = new MyXmlSchemaSetStruct();
            myNew.MyXmlSchemaSet = schemas;
            myNew.MyWSDL = wsdlName;

            MyCache.Add(myNew);
            return schemas;
        }


        

        private void LogMe(string p)
        {
            //throw new NotImplementedException();
            p=p??"";
            if (p.Length > 1024)
            {
                p=p.Substring(0, 1024);
            }
            Debug.WriteLine(p);
        }

        // Display any validation errors.
        private void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            _Valid = true;
            LogMe("Validation Result: " + e.Message);
        }
    }
}