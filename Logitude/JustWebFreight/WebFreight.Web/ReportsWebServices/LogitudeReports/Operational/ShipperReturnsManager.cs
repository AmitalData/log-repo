using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Operational
{
    public class ShipperReturnsManager
    {

        public ShipperReturnsManager(byte[] xmlFilters, int tenant)
        {

        }


        public byte[] GetData()
        {
            ShipperReturnsDataProvider myDataProvider = new ShipperReturnsDataProvider();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ShipperReturnsDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, myDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);

            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }

    }

}