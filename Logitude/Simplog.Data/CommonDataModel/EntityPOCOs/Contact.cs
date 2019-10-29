using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.DomainServices.Server;
using System.ServiceModel.DomainServices.Server.ApplicationServices;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Contact : IUser
    {
        [Key]
        public string Id { get; set; }

        public string UserType { get; set; }
        public int Tenant { get; set; }
        public bool InActive { get; set; }
        public string FacebookId { get; set; }
        public string EnglishName { get; set; }
        public string ExternalId { get; set; }
        public string LocalName { get; set; }
        public string SearchFields { get; set; }
        public string Email { get; set; }
        public string BusinessPhone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? Anniversary { get; set; }
        public string Notes { get; set; }
        public byte[] Signature { get; set; }
        public byte[] SignatureHtml { get; set; }
        public bool DisplayGettingStarted { get; set; }
        public string ImageDetailId { get; set; }
        public bool DontShowLocalLabels { get; set; }
        public string ComputedKey { get; set; }
        public string CompanyName { get; set; }

        public virtual ContactLastLogin ContactLastLogin { get; set; }

        public virtual SharedLogisticsContactLastLogin SharedLogisticsContactLastLogin { get; set; }

        [ForeignKey("ImageDetailId")]
        public ImageDetail ImageDetail { get; set; }

        public virtual User User { get; set; }

        [DataMember]
        public IEnumerable<string> Roles
        {
            get { return this.Email.Split(','); }
            set { this.Email = string.Join(",", value.ToArray()); }
        }

        public string Name
        {
            get
            {
                return Email;
            }
            set
            {
                Email = value;
            }
        }

        public string Position { get; set; }
        public bool BirthdayReminder { get; set; }
        public bool AnniversaryReminder { get; set; }

        public DateTime? DoneDate { get; set; }
        public int? BirthDayOfYear { get; set; }
        public string ContactDoneMethodCode { get; set; }

        public int? IndexColor { get; set; }

        [ForeignKey("ContactDoneMethodCode")]
        public ContactDoneMethod ContactDoneMethod { get; set; }

        [ForeignKey("IndexColor")]
        public ColorIndex ColorIndex { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
