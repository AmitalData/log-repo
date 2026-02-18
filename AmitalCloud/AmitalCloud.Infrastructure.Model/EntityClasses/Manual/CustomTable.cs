using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class CustomTable
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableId { get; set; }

        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }
        //[Include]
        //[Association("ObjectTableCustomTable", "ObjectTableId", "Id", IsForeignKey = true)]
        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }
        ////[Include]
        ////[Association("ScreenCustomTable", "ScreenId", "Id", IsForeignKey = true)]
        //public virtual Screen Screen { get; set; }

    }
}