using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Update.SandBox
{
    public class TextCodeDTO
    {


        public string Code { get; set; }
        public string DefaultText { get; set; }
        public string DefaultTextPlural { get; set; }

        public string Id { get; set; }
        public bool InActive { get; set; }
        public bool IsSpellChecked { get; set; }
        public string LocalDefaultText { get; set; }

        public string ObjectTableId { get; set; }
        public DateTime? SpellCheckDate { get; set; }

        public string SpellCheckedByUserId { get; set; }
        public int Tenant { get; set; }

        public string TextCodeTypeCode { get; set; }
    }
}
