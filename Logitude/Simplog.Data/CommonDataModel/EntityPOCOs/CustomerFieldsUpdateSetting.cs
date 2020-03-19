using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class CustomerFieldsUpdateSetting
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectFieldId { get; set; }
        public string UpdateDirection { get; set; }
        public string ObjectFieldCode { get; set; }


        [ForeignKey("ObjectFieldId")]
        public virtual ObjectField ObjectField { get; set; }
    }
}
