using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class RuleNotificationType
    {

        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }

        //public virtual List<ObjectTableRule> ObjectTableRules { get; set; }
        //public virtual List<ObjectTableRuleField> ObjectTableRuleFields { get; set; }
    }
}