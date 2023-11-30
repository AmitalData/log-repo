using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class Screen
    {

        [Key]
        public string Id { get; set; }

        //[Required]
        public string Code { get; set; }

        public int NumberOfRows { get; set; }
        public int NumberOfColumns { get; set; }

        public string ObjectTableId { get; set; }
       // public virtual List<User> Users { get; set; }
        public int Tenant { get; set;}

        public bool IsReadOnly { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Inactive { get; set; }
        public string SortedByFieldCode { get; set; }
        public string SortedType { get; set; }
        public string SearchFields { get; set; }
        public string RelatedScreenCode { get; set; }
        public bool IsHeaderScreen { get; set; }
        //[Include]
        //[Association("ObjectTableScreen", "ObjectTableId", "Id",IsForeignKey=true)]

        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }

        //public List<ObjectTable> ObjectTables { get; set; }

        //[Include]
        //[Association("ScreenScreenField", "Id", "ScreenId")]
        //public virtual List<ScreenField> ScreenFields { get; set; }

        //public List<ScreenModification> ScreenModifications { get; set; }


        //public List<FollowUpType> FollowUpTypes { get; set; }

        //public List<VerifyQueue> VerifyQueues { get; set; }

      //  public List<CustomTable> CustomTables { get; set; }
       

    }
}