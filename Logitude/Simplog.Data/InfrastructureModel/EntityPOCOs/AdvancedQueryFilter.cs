using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class AdvancedQueryFilter
    {
        [Key]
        public string Id { get; set; }
        public string QueryCode { get; set; }
        public int Tenant { get; set; }
        public string QueryId { get; set; }
        public string ObjectFieldId { get; set; }
        public bool IsPredefined { get; set; }
        public string PredefinedValue { get; set; }
        public string PredefinedValue2 { get; set; }
        public string Operator { get; set; }
        public int IndexOrder { get; set; }
        public bool CustomPredefined { get; set; }
        public string UserId { get; set; }
        public string ObjectFieldCode { get; set; }
        

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
       

        //[Include]
        //[Association("QueryAdvancedQueryFilter","QueryId","Id",IsForeignKey=true)]

        [ForeignKey("QueryId")]
        public virtual Query Query { get; set; }
        //[Include]
        //[Association("ObjectFieldAdvancedQueryFilter","ObjectFieldId","Id",IsForeignKey=true)]

        [ForeignKey("ObjectFieldId")]
        public virtual ObjectField ObjectField { get; set; }
    }
}
