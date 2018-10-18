using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class EntityDate
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        ////[Include]
        ////[Association("FollowUpTypeEntityDate","Id","EntityDateId")]
        //public List<FollowUpType> FollowUpTypes { get; set; }


    }
}