using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.InfrastructureModel.EntityPMs;
using System.ComponentModel.DataAnnotations;

namespace WebFreight.Web.DataContracts
{
    public class ScreenLayoutArgs
    {
        public List<ScreenFieldPM> ScreenFields { get; set; }
        public List<ScreenFieldPM> RemovedScreenFields { get; set; }
        public string ScreenId { get; set; }
        public string ScreenCode { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
    }
}