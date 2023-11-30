using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class ContactList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool InActive { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string Email { get; set; }
        public string BusinessPhone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? Anniversary { get; set; }
        public string Notes { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        public bool DisplayGettingStarted { get; set; }
        public bool BirthdayReminder { get; set; }
        public bool AnniversaryReminder { get; set; }
        public bool DontShowLocal { get; set; }
        public string ImageDetailId { get; set; }
        public DateTime? DoneDate { get; set; }
        public int? BirthDayOfYear { get; set; }
        public string ContactDoneMethodCode { get; set; }
        public string Position { get; set; }
        public bool HasCardContact { get; set; }
        public string Company { get; set; }  
        public int? IndexColor { get; set; }
        public string CompanyName { get; set; }
        public string DigitalPortalLanguage { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        
    }
}