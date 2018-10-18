using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class TextCodePM
    {
        [Key]
        public string Id { get; set; }
        public string Code { get; set; }
        public string DefaultText { get; set; }
        public string ObjectTableId { get; set; }
        public string TextCodeTypeCode { get; set; }
        public int Tenant { get; set; }
        public string DefaultTextPlural { get; set; }
        public string ObjectTableName { get; set; }
        public bool IsSpellChecked { get; set; }
        public DateTime? SpellCheckDate { get; set; }
        public string SpellCheckedByUserId { get; set; }
        public string SpellCheckedByUserName { get; set; }
        public bool InActive { get; set; }
        public string LocalDefaultText { get; set; }
    }
}