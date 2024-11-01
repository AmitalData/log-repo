using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class ReportGroup
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public int OrderNumber { get; set; }
        //public List<Report> Reports { get; set; }
    }
}
