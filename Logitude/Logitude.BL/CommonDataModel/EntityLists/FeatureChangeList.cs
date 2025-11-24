using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class FeatureChangeList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public DateTime? EventDateTime { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string PackageCode { get; set; }
        public string SearchFields { get; set; }
        public string UserName { get; set; }
    }
}
