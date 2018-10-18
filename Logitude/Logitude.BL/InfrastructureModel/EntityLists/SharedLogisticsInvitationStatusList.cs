using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class SharedLogisticsInvitationStatusList
    {
        [Key]
        public int Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
    }
}