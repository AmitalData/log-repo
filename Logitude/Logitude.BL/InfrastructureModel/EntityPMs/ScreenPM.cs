using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class ScreenPM
    {
        [Key]
        public string Id { get; set; }

        
        public string Code { get; set; }

        public int NumberOfRows { get; set; }
        public int NumberOfColumns { get; set; }

        public string ObjectTableId { get; set; }
       
        public int Tenant { get; set; }

        public bool IsReadOnly { get; set; }

        public string ObjectTableName { get; set; }
        public string Name { get; set; }
        public int  UserTenant { get; set; }


        [Include]
        [Association("ScreenScreenField", "Id", "ScreenId")]
        public virtual List<ScreenFieldPM> ScreenFields { get; set; }
        
    }
}