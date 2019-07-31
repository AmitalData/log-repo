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

namespace Logitude.Social.Data.EntityPOCOs
{
   
    public class ConversationHeaderParticipant
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("ConversationHeader")]
        [Column("ConversationHeaderId")]
	    public string ConversationHeaderId { get; set; }
	      
        public virtual ConversationHeader ConversationHeader { get; set; }
        [ForeignKey("ParticipantUser")]
        [Column("ParticipantUserId")]
	    public string ParticipantUserId { get; set; }
	      
        public virtual User ParticipantUser { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [Column("LeaveDate")]
	    public DateTime? LeaveDate { get; set; }
        [Column("IsLeft")]
	    public bool IsLeft { get; set; }
        [Column("Replied")]
	    public bool Replied { get; set; }
        [Column("IsRead")]
	    public bool IsRead { get; set; }
        [Column("LastReadDate")]
	    public DateTime? LastReadDate { get; set; }
        [Column("IsDelete")]
	    public bool IsDelete { get; set; }
        [Column("DeleteDate")]
	    public DateTime? DeleteDate { get; set; }
    }
}
	 