using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Customs.BL.Utils
{
    public class Serializer
    {
        public X CastXML<X,T>(T xml, string xmlUrl = "") where X : class
        {
            string DeclarationString;

            using (var stringwriter = new System.IO.StringWriter())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

                ns.Add("q", xmlUrl);
                var serializer = new XmlSerializer(xml.GetType());
                serializer.Serialize(stringwriter, xml, namespaces: ns);
                DeclarationString = stringwriter.ToString();
            }
            
            DeclarationString = DeclarationString.Replace("xmlns:q", "xmlns");
            DeclarationString = DeclarationString.Replace("<q:", "<");
            DeclarationString = DeclarationString.Replace("</q:", "</");

            using (var stringReader = new System.IO.StringReader(DeclarationString))
            {
                var serializer = new XmlSerializer(typeof(T));

                try
                {
                    return serializer.Deserialize(stringReader) as X;
                }
                catch (System.Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
