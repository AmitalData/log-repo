using Simplog.Data.CommonDataModel.Repositories;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace WebFreight.Web.Helpers
{
    public class StimulsoftReportFontSizeService
    {

        public StimulsoftReportFontSizeService()
        {

        }

        public void Run(StiReport report, string documentOutId, int tenant)
        {
            if (string.IsNullOrEmpty(documentOutId)) return;
            byte[] editableFields = new DocumentOutRepository(tenant).GetEditableFieldsById(documentOutId, tenant);
            if (editableFields == null) return;
            var editableFieldsString = System.Text.Encoding.UTF8.GetString(editableFields);
            XmlNodeList childFieldNodes = GetXmlNodeList(editableFieldsString);
            if (childFieldNodes == null) return;
            StiComponentsCollection stiComponentsCollection = report.GetComponents();
            foreach (StiComponent stiComponent in stiComponentsCollection)
            {
                ModifyFontSize(childFieldNodes, stiComponent);
            }
        }

        private void ModifyFontSize(XmlNodeList childFieldNodes, StiComponent stiComponent)
        {
            string orginalfontFamily = string.Empty;

            if (!(stiComponent is StiText) || !((stiComponent as StiText).Editable)) return;
            StiText textField = stiComponent as StiText;
            XmlNode field = (new System.Collections.Generic.List<XmlNode>(Shim<XmlNode>(childFieldNodes))).Where(d => (d["ComponentName"] != null ? d["ComponentName"].InnerText : "").ToLower() == textField.Name.ToLower()).FirstOrDefault();
            if (field == null) return;

            var fontInfo = textField.Font.ToString().Split(',');
            if (fontInfo.Length > 0)
            {
                orginalfontFamily = fontInfo[0].Split('=').Length > 1 ? fontInfo[0].Split('=')[1] : "";
            }


            string editableFontSize = field["FontSize"] != null ? field["FontSize"].InnerText : "";
            if (!string.IsNullOrEmpty(editableFontSize))
            {
                
                textField.Font = new Font(orginalfontFamily, ConvertFromPixelToPoint(float.Parse(editableFontSize)));
            }




        }
        private float ConvertFromPixelToPoint(float pixels)
        {
            return (pixels / 2) * 72 / 96;
        }

        private  XmlNodeList GetXmlNodeList(string editableFieldsString)
        {
            XmlNodeList childFieldNodes = null;
            if (string.IsNullOrEmpty(editableFieldsString))  return childFieldNodes;
            try
            {
                using (XmlReader xmlReader = XmlReader.Create(new StringReader(editableFieldsString)))
                {
                    XmlDataDocument messageFieldDoc = new XmlDataDocument();
                    messageFieldDoc.Load(xmlReader);
                    XmlNodeList ItemsFieldList = messageFieldDoc.GetElementsByTagName("Items");
                    if (ItemsFieldList != null && ItemsFieldList.Count > 0) childFieldNodes = ItemsFieldList[0].ChildNodes;
                }
            }
            catch (Exception exception)
            {

            }
            return childFieldNodes;
        }

        private static IEnumerable<T> Shim<T>(System.Collections.IEnumerable enumerable)
        {
            foreach (object current in enumerable)
            {
                yield return (T)current;
            }
        }
    }




}