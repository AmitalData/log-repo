using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    public class P19R03_0000_PatchDist: PatchDistributionBase
    {

        public P19R03_0000_PatchDist() : base("טבלאות תשתית הפצה ",new DateTime(2019,12,2))
        {
            
            //this.PatchName = "טבלאות תשתית הפצה ";
            //this.Branch = "19R03";
            //this.Counter  = 0;
            //this.ScriptCount = 5;
        }
        
        

        public override List<ScriptDTO> GetUpScripts()
        {
            int ScriptCount = 0;
            return new List<ScriptDTO>()
            {
                new ScriptDTO() { ScriptCounter = ScriptCount++, SqlScript = "create table " }
            };
        }

        public override List<ScriptDTO> GetDownScripts()
        {
            throw new NotImplementedException();
        }
    }
   
}
