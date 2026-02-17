using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class MenusTable
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string MenuTypeCode { get; set; }
        public string CategoryTypeCode { get; set; }
        public int IndexOfOrder { get; set; }
        public string Icon { get; set; }
        public string  TextCode { get; set; }
        public string UserControlName { get; set; }
        public string ObjectTableId { get; set; }
        public string FeatureId { get; set; }
        public string Code { get; set; }
        public string HtmlView { get; set; }

        //[Include]
        //[Association("MenuTypeMenusTable", "MenuTypeCode", "Code", IsForeignKey = true)]
        [ForeignKey("MenuTypeCode")]
        public virtual MenuType MenuType { get; set; }
        //[Include]
        //[Association("CategoryTypeMenusTable", "CategoryTypeCode", "Code", IsForeignKey = true)]
        [ForeignKey("CategoryTypeCode")]
        public virtual CategoryType CategoryType { get; set; }

        //[Include]
        //[Association("ObjectTableMenusTable","ObjectTableId","Id",IsForeignKey=true)]
         [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }

        [ForeignKey("FeatureId")]
        public Feature Feature { get; set; }
    }
}