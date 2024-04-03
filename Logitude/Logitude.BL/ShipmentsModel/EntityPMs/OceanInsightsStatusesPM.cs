using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class OceanInsightsStatusesPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string OceanInsightsRequestId { get; set; }
        public string ContentDocumentId { get; set; }
        public string CommunicationLogId { get; set; }
        public DateTime CreateDate { get; set; }
		public string XML { get; set; }


	}
}
