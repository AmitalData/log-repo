using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class GTBITEMPM : EntityPM
    {
        public string PARTNERID { get; set; }
        public string ITEMID { get; set; }
        public string NAMEHEB { get; set; }
        public string NAMEENG { get; set; }
        public string BLOCKRECORD { get; set; }
        public string SEARCHENG { get; set; }
        public string UNITID { get; set; }
        public int? PRICEQTY { get; set; }
        public string COINID { get; set; }
        public double? PRICE { get; set; }
        public string HARMONIZEID { get; set; }
        public string PRATID { get; set; }
        public double? WEIGHT { get; set; }
        public string WEIGHTUM { get; set; }
        public string SERIAL { get; set; }
        public string ITEMTYPE { get; set; }
        public string CHARGE { get; set; }
        public string NOOVERHEAD { get; set; }
        public string ORIGINCOUNTRY { get; set; }
        public string STANDARTSIV { get; set; }
        public string LICENSENO { get; set; }
        public string DESCRIPTION { get; set; }
        public string LICENCESIV { get; set; }
        public string DESCRIPTIONHEB { get; set; }
        public string NOSTANDART { get; set; }
        public string APPROVTYPEID { get; set; }
    }
}
