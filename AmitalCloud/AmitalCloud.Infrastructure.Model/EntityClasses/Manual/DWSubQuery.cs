using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{

    public class DWSubQuery
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DWFactTableCode { get; set; }
        public string DWQueryId { get; set; }
        public string SQLString { get; set; }
        public string FiltersXML { get; set; }
        public string ColumnsXML { get; set; }



        //[ForeignKey("DWFactTableCode")]
        //public virtual DWObjectTable DWObjectTable { get; set; }

        [ForeignKey("DWQueryId")]
        public virtual DWQuery DWQuery { get; set; }


    }
}
