using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class TermsofUseList
    {
        [Key]
        public int Version { get; set; }
        public DateTime Date { get; set; }
    }
}
