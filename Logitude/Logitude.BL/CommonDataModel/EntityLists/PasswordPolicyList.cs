using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class PasswordPolicyList
    {
        [Key]
        public string Code { get; set; }

        public string PasswordStrength { get; set; }
        public string SearchFields { get; set; }
    }
}