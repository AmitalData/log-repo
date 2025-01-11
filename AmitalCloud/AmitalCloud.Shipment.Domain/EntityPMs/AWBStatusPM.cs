using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    public partial class AWBStatusPM : EntityPM
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
    }
}