using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
   public class ScreenSection
    {

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string CreateByUserId { get; set; }
        public string ScreenCode { get; set; }

        [ForeignKey("ScreenCode")]
        public virtual Screen Screen { get; set; }



        [ForeignKey("CreateByUserId")]
        public virtual User CreateByUser { get; set; }

    }
}
