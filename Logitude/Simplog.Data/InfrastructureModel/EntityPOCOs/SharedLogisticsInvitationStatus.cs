using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
   public  class SharedLogisticsInvitationStatus
    {
        [Key]
        public int Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }

        //public List<Card> Cards { get; set; }
    }
}
