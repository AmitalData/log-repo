using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class QueryColumn
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string QueryId { get; set; }
        public string ObjectFieldId { get; set; }
        public int IndexOrder { get; set; }
        public double ColumnWidth { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }


        //[Include]
        //[Association("QueryColumnQuery","QueryId","Id",IsForeignKey=true)]
        [ForeignKey("QueryId")]
        public virtual Query Query { get; set; }
        //[Include]
        //[Association("QueryColumnObjectField","ObjectFieldId","Id",IsForeignKey=true)]
        [ForeignKey("ObjectFieldId")]
        public virtual ObjectField ObjectField { get; set; }
    }
}
