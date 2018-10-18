using System.ComponentModel.DataAnnotations;

namespace Logitude.Customs.Def.EntityPMs
{ 
    public class DeclarationErrorViewPM
    {
        [Key]
        public string Id { get; set; }
        public string DeclarationId { get; set; }
        public int? ParentLine { get; set; }
        public string ParentEntityName { get; set; }
        public string Description { get; set; }
        [Key]
        public string ErrorType { get; set; }
        public string Field { get; set; }
        public int? Line { get; set; }
        public string EntityName { get; set; }
        public string ConstraintId { get; set; }
        public string FieldNameTextCode { get; set; }
        public string TableNameTextCode { get; set; }
        public string ListVersionId { get; set; }
        public bool ConstraintIndication { get; set; }
        public string LineNumber { get; set; }
    }
}
