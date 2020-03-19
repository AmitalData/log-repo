using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class ObjectTableTab
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableId { get; set; }
        public string ControlPath { get; set; }
        public string TabNameTextCodeId { get; set; }
        public int IndexOrder { get; set; }
        public string Code { get; set; }
        public string FeatureId { get; set; }
        public string TabNameTextCodeCode { get; set; }
        public string FeatureUniqeCode { get; set; }


        //[ForeignKey("FeatureId")]
        public Feature Feature { get; set; }


        //[Include]
        //[Association("ObjectTableTabObjectTable","ObjectTableId","Id",IsForeignKey=true)]
         [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }

        //[Include]
        //[Association("ObjectTableTabTextCode", "TabNameTextCodeId", "Id", IsForeignKey = true)]
        [ForeignKey("TabNameTextCodeId")]
        public virtual TextCode TabNameTextCode { get; set; }

        public string HtmlComponentName { get; set; }
        public string HtmlComponentUrl { get; set; }
    }
}