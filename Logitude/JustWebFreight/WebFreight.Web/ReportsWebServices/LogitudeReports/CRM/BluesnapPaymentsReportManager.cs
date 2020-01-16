using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Bluesnap
{ 
    public class BluesnapPaymentsReportManager
    {
        private int tenant;
        private DateTime? fromDate = null;
        private DateTime? toDate = null;
        public BluesnapPaymentsReportManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations iQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);

        }

    }
}