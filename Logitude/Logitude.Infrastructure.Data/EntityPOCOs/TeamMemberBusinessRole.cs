using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.Infrastructure.Data.EntityPOCOs
{
   
    public class TeamMemberBusinessRole
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("LBPTeamMember")]
        [Column("TeamMemberId")]
	    public string TeamMemberId { get; set; }
	      
        public virtual LBPTeamMember LBPTeamMember { get; set; }
        [ForeignKey("AddedByUser")]
        [Column("AddedByUserId")]
	    public string AddedByUserId { get; set; }
	      
        public virtual User AddedByUser { get; set; }
        [Column("AddDate")]
	    public DateTime AddDate { get; set; }
        [ForeignKey("BusinessRole")]
        [Column("BusinessRoleId")]
	    public string BusinessRoleId { get; set; }
	      
        public virtual BusinessRole BusinessRole { get; set; }
    }
}
	 