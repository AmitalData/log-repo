using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class SharedLogisticsInvitationStatusPM
    {
        [Key]
        public int Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
    }
}