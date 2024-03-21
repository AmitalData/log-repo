using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial  class CCUMSHGRPM : EntityPM
    {
        public int FILENO { get; set; }

        public int LINENO { get; set; }
        
        public int? ADDQUANTITY { get; set; }
        
        public string CARNETNUMBER { get; set; }
        
        public string CARRIERID { get; set; }
        
        public string DESCOFGOODS1 { get; set; }
        
        public string DESCOFGOODS2 { get; set; }
        
        public string DESCOFGOODS3 { get; set; }
        
        public string EXPORTLAND { get; set; }
        
        public string HAWB { get; set; }
        
        public DateTime? HAWBDATE { get; set; }
        
        public string IDENTIFIERNO { get; set; }
        
        public string IDENTIFIERTYPE { get; set; }
        
        public string LOADPORTID { get; set; }
        
        public string MANIFESTNO { get; set; }
        
        public string MISHGORNO { get; set; }
        
        public string MISHGORTYPE { get; set; }
        
        public string PACKDET { get; set; }
        
        public string PACKTYPEID { get; set; }
        
        public string PARTIALITYID { get; set; }
        
        public int? QUANTITY { get; set; }
        
        public int? SEALQTY { get; set; }
        
        public string STORAGESITE { get; set; }
        
        public string TRANSPTYPE { get; set; }
        
        public DateTime? UNLOADDATE { get; set; }
        
        public string UNLOADPORTID { get; set; }
        
        public string WAREHOUSEID { get; set; }
        
        public string WAREHOUSEREC { get; set; }
        
        public int? WEIGHT { get; set; }

        public string WAREHOUSEIDN { get; set; }
 
        public string WAREHOUSERECN { get; set; }

        public string EXPORTLANDN { get; set; }

        public string PACKTYPEIDN { get; set; }

        public string HAWBN { get; set; }

        public string IDENTIFIERTYPEN { get; set; }

        public string FIRSTCARGOID { get; set; }

        public string SECONDCARGOID { get; set; }

        public int Tenant { get; set; }

        public bool IS_SYNCH { get; set; }

        public DateTime? LAST_UPDATE_DT { get; set; }

    }
}
