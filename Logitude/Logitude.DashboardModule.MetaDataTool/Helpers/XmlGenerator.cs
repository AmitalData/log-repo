using Logitude.DashboardModule.MetaDataTool.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace Logitude.DashboardModule.MetaDataTool.Helpers
{
    public class XmlGenerator
    {
        public static void GenerateXmlFileFromTool(AnalyticsFactsMetaData table)
        {
            AnalyticsFactsMetaData analyticsFactsMetaData = CleanUnwantedProp(table);
            if (string.IsNullOrEmpty(App.DirectOpenPath))
            {
                MessageBox.Show("file path in not valid!");
                return;
            }
            XmlDocument doc = XMLParser.SerializeToXml(analyticsFactsMetaData);
            FileStream fileStream = new FileStream(App.DirectOpenPath, FileMode.Truncate, FileAccess.Write);
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true, NewLineOnAttributes = true, OmitXmlDeclaration = false, WriteEndDocumentOnClose = false };
            XmlWriter xmlWriter = XmlWriter.Create(fileStream, settings);

            doc.Save(xmlWriter);
            xmlWriter.Close();
            xmlWriter.Dispose();
        }

        private static AnalyticsFactsMetaData CleanUnwantedProp(AnalyticsFactsMetaData table)
        {
            return JsonConvert.DeserializeObject<AnalyticsFactsMetaData>(JsonConvert.SerializeObject(table));
        }
    }
}
