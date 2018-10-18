using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class MoveTypeList
    {
        [Key]
        public string Id { get; set; }

        public int Tenant { get; set; }

        public string MoveTypeEnglishName { get; set; }

        public string MoveTypeLocalName { get; set; }

        public string TransportModeId { get; set; }

        public bool AddedManually { get; set; }

        public bool InActive { get; set; }

        public string Code { get; set; }

        public string SearchFields { get; set; }

        public bool IsAir { get; set; }

        public bool IsInland { get; set; }

        public bool IsOcean { get; set; }
    }
}