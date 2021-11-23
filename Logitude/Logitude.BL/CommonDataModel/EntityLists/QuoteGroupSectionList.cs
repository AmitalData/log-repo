using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class QuoteGroupSectionList
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string Searchfields { get; set; }
    }
}