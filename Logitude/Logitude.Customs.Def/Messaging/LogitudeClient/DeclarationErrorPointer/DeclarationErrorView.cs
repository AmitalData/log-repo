using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.Messaging.LogitudeClient.DeclarationErrorPointer
{
    public class DeclarationErrorView
    {
        [Key]
        public string Id { get; set; }//GUID

        [Key]
        public string DeclarationId { get; set; }
        public int? ParentLine { get; set; }
        public string ParentEntityName { get; set; }
        public string Description { get; set; }
        [Key]
        public string ErrorType { get; set; }
        public string Field { get; set; }
        public int? Line { get; set; }
        public int? Sort { get; set; }
        public string EntityName { get; set; }
        public string ConstraintId { get; set; }
        public string FieldNameTextCode { get; set; }
        public string TableNameTextCode { get; set; }

        public string ListVersionId { get; set; }
        public bool ConstraintIndication { get; set; }
        public string LineNumber { get; set; }

    }
}
