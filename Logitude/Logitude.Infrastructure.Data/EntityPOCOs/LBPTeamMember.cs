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
   
    public class LBPTeamMember
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("MemberUser")]
        [Column("MemberUserId")]
	    public string MemberUserId { get; set; }
	      
        public virtual User MemberUser { get; set; }
        [ForeignKey("Team")]
        [Column("TeamId")]
	    public string TeamId { get; set; }
	      
        public virtual Team Team { get; set; }
        [Column("AddDate")]
	    public DateTime AddDate { get; set; }
        [ForeignKey("AddedByUser")]
        [Column("AddedByUserId")]
	    public string AddedByUserId { get; set; }
	      
        public virtual User AddedByUser { get; set; }
        [ForeignKey("MemberTeam")]
        [Column("MemberTeamId")]
	    public string MemberTeamId { get; set; }
	      
        public virtual Team MemberTeam { get; set; }
    }
}
	 