using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class ScreenList
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
        public string QuerySection { get; set; }


        

    }
}