using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public class CCUFILEM_4L2U
    {
        //CCUFILEM
        public int FILENO { get; set; }
        public string CUSTOMSBRANCH { get; set; }
        public DateTime? GRANTDATE { get; set; }
        public string RESHIMONTYPEN { get; set; }
        public DateTime? RESHMDATE { get; set; }
        public double? CIFVALUE { get; set; }
        public double? TOTALTAX { get; set; }
        public int? MEHESDRAFTSTATUS { get; set; }
        public string RESHIMONNON { get; set; }
        public DateTime? PAYDATE { get; set; }
        public DateTime? PAYTIME { get; set; }

        List<CCUMSHGR_4L2U> _CCUMSHGRs;
        public List<CCUMSHGR_4L2U> CCUMSHGRs
        {
            get { return _CCUMSHGRs = _CCUMSHGRs ?? new List<CCUMSHGR_4L2U>(); }
            set
            {
                _CCUMSHGRs = value;
            }
        }
    }

    public class CCUMSHGR_4L2U
    {
        //CCUMSHGR
        public string FIRSTCARGOID { get; set; }
        public string SECONDCARGOID { get; set; }
        public DateTime? HAWBDATE { get; set; }
        public string HAWB { get; set; }
        public string HAWBN { get; set; }
        public string IDENTIFIERNO { get; set; }
        public string LOADPORTID { get; set; }
        public string EXPORTLANDN { get; set; }
        public string DESCOFGOODS1 { get; set; }
        public string DESCOFGOODS2 { get; set; }
        public string DESCOFGOODS3 { get; set; }
        public DateTime? UNLOADDATE { get; set; }
        public string PACKTYPEIDN { get; set; }
        public string WAREHOUSEIDN { get; set; }
        public string UNLOADPORTID { get; set; }
        public int? WEIGHT { get; set; }
        public int? QUANTITY { get; set; }
    }
}
