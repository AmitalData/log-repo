using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Logitude.BL.Helpers
{
    public class EntityPMChangeTrackingHelper
    {

        public static string GetChangesDetectedXml(List<NotifyPropertyChangeValues> ChangedProperties)
        {
            string xml = null;

            if (ChangedProperties != null && ChangedProperties.Count > 0)
            {
                r root = new r();
                root.cs = new List<c>();
                foreach (NotifyPropertyChangeValues value in ChangedProperties)
                {
                    string oldValue = value.OldValue != null ? value.OldValue.ToString() : "";
                    string newValue = value.NewValue != null ? value.NewValue.ToString() : "";

                    c change = new c()
                    {
                        f = value.PropertyName,
                        n = newValue,
                        o = oldValue,
                    };
                    root.cs.Add(change);
                }

                xml = SerializeObjectToXml<r>(root);
            }

            return xml;
        }


        public static string GetChangesDetectedXml(EntityPMBase changesTrackingEntityPM)
        {
            string xml = null;

            if (changesTrackingEntityPM != null)
            {
                r root = new r();
                root.cs = new List<c>();
                foreach (NotifyPropertyChangeValues value in changesTrackingEntityPM.ChangedProperties)
                {
                    string oldValue = value.OldValue != null ? value.OldValue.ToString() : "";
                    string newValue = value.NewValue != null ? value.NewValue.ToString() : "";

                    c change = new c()
                    {
                        f = value.PropertyName,
                        n = newValue,
                        o = oldValue,
                    };
                    root.cs.Add(change);
                }

                xml = SerializeObjectToXml<r>(root);
            }

            return xml;
        }


       




        public static string SerializeObjectToXml<T>(T dataObject)
        {
            MemoryStream memstream = new MemoryStream();

            XmlSerializer ser = new XmlSerializer(typeof(T));

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "http://www.champ.aero/GCCS/CargoXML");
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "",
                OmitXmlDeclaration = true,
                NewLineChars = "",
                NewLineHandling = NewLineHandling.Replace,
            };

            XmlWriter writer = XmlTextWriter.Create(memstream, settings);
            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");
            ser.Serialize(writer, dataObject, ns);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            return content;
        }


    }
}
