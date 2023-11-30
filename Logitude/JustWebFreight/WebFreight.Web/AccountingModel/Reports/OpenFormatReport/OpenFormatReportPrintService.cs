using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WebFreight.Web.AccountingModel.Reports.OpenFormatReport
{
    public class OpenFormatReportPrintService
    {
        public OpenFormatReportPM openFormatReportPM;


        public OpenFormatReportDataProvider LoadDataProvider(string entityId, int tenant)
        {
            OpenFormatReportDataProvider OpenFormatReportDP = new OpenFormatReportDataProvider();


            OpenFormatReportQueryService openFormatReportQueryService = new OpenFormatReportQueryService(tenant);

            openFormatReportPM = openFormatReportQueryService.GetSingle(entityId, false, false);
            if (openFormatReportPM != null)
            {
                using (var stringReader = new System.IO.StringReader(openFormatReportPM.PDFRerportXML))
                {
                    var serializer = new XmlSerializer(typeof(OpenFormatReportDataProvider));
                    OpenFormatReportDP= serializer.Deserialize(stringReader) as OpenFormatReportDataProvider;
                };
            }
            OpenFormatReportDP.CreateDate = openFormatReportPM.CreateDate;
            return OpenFormatReportDP;
        }

    }
}