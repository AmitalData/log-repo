using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.InfrastructureModel.EntityPMs;
using System.ComponentModel.DataAnnotations;

namespace WebFreight.Web.DataContracts
{
    public class PickListGeneralEntitiesArgs
    {
        public List<CustomPickListPM> CustomPickListPMs { get; set; }
        public List<CustomPickListPM> RemovedCustomPickListPMs { get; set; } 
    }
}