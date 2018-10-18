using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class TextCodeList
    {
        public string Code { get; set; }
        public string DefaultText { get; set; }
        public string ObjectTableId { get; set; }
        public string TextCodeTypeCode { get; set; }
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DefaultTextPlural { get; set; }
        public string ObjectTableName { get; set; }
        public string LocalDefaultText { get; set; }
    }
}