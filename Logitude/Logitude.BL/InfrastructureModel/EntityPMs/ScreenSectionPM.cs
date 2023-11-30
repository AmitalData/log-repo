using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
  public  class ScreenSectionPM
    {

        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string CreatedByUserId { get; set; }
        public string ScreenCode { get; set; }
        public string ObjectTableName { get; set; }
        public int NumberOfRows { get; set; }
        public int Number { get; set; }
        public bool Inactive { get; set; }
        public string Type { get; set; }
        public string RelatedScreenCode { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }


    }
}
