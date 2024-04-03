using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.Workflow.BL.Models.MicrosoftOffice365
{
    public class CreateSubscription
    {
        [Required]
        public string WorkflowNumber { get; set; }

        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public string UserEmail { get; set; }

        [Required]
        public DateTime AccessTokenExpirationDateTime { get; set; }

        public int Tenant { get; set; }
    }
}