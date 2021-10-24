using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.DataContracts
{
    public class CustomsRequestsSheetSummary
    {
        [Key]
        public Guid Id { get; set; }
        public int count { get; set; }
        public string InterfaceTypeName { get; set; }

    }
}