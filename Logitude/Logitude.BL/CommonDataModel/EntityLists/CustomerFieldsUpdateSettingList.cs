using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
  public  class CustomerFieldsUpdateSettingList
    {

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectFieldId { get; set; }
        public string UpdateDirection { get; set; }
        public string ObjectFieldName { get; set; }
        public string ObjectFieldCode { get; set; }

    }
}
