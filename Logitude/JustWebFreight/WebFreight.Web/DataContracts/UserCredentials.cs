using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace WebFreight.Web.DataContracts
{
    public class UserCredentials
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
         
        public int Tenant { get; set; }
    }
}