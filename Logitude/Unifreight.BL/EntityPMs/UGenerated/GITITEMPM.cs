using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class GITITEMPM : EntityPM
    {
        public decimal COUNTER { get; set; }

        public string PARTNERID { get; set; }

        public string ITEMNO { get; set; }

        public string SAPAKID { get; set; }

        public DateTime? OPENDATE { get; set; }

        public string BRANCHID { get; set; }

        public short? ACCOUNTINGCLOSE { get; set; }

        public short? ITEMCLOSE { get; set; }

        public string ITEMCANCELLED { get; set; }

        public string ITEMOPENUSER { get; set; }

        public DateTime? ITEMUPDATEDATE { get; set; }

        public string PRATID { get; set; }

        public string ITEMUPDATEUSER { get; set; }

        public string NOSTANDART { get; set; }

        public string APPROVTYPEID { get; set; }

        public string NAMEENG { get; set; }

        public string SEARCHENG { get; set; }

        public string LICENCESIV { get; set; }

        public string ORIGINCOUNTRY { get; set; }

        public string UNITID { get; set; }
    }


    public class GITITEMDto
    {
        public decimal COUNTER { get; set; }

        public string PARTNERID { get; set; }

        public string ITEMNO { get; set; }

        public string SAPAKID { get; set; }

        public DateTime OPENDATE { get; set; }

        public string BRANCHID { get; set; }

        public bool ACCOUNTINGCLOSE { get; set; }

        public bool ITEMCLOSE { get; set; }

        public string ITEMCANCELLED { get; set; }

        public string ITEMOPENUSER { get; set; }

        public DateTime ITEMUPDATEDATE { get; set; }

        public string PRATID { get; set; }

        public string ITEMUPDATEUSER { get; set; }

        public string NOSTANDART { get; set; }

        public string APPROVTYPEID { get; set; }

        public string NAMEENG { get; set; }

        public string SEARCHENG { get; set; }

        public string LICENCESIV { get; set; }

        public string ORIGINCOUNTRY { get; set; }

        public string UNITID { get; set; }
    }
}
