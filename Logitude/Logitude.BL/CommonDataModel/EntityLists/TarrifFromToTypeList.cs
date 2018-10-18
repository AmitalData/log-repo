using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class TarrifFromToTypeList
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}