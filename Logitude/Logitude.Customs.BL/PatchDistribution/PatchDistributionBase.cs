using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution
{
    public abstract class PatchDistributionBase 
    {
        public string Branch { get; }
        public int MajorYear { get; }
        public int MajorYearRelease { get; }
        public decimal MajorVersionYYPRR { get; }
        public int PatchCounter_Minor { get; }

        public string PatchName { get; }

        public string PatchDetails { get; }
        public DateTime CreatedAt { get; }
        public string What2DOWhileCrash { get; set; }

        
        protected ScriptList UpScripts { get; }
        protected ScriptList DownScripts { get; }

        public PatchDistributionBase(string patchDetails, DateTime dateTime)
        {
            PatchDetails = patchDetails;
            CreatedAt = dateTime;
            Type type = this.GetType().UnderlyingSystemType;
            String className = type.Name;
            var partsOfClassName = className.Split("_"[0]).ToList();
            Branch = partsOfClassName[0]??"";
            if (!Branch.StartsWith("P"))
            {
                throw new Exception("Class name must start with P");
            }
            var release = Branch.Substring(1);
            var parts=release.Split('R').ToList();
            if (parts.Count()!=2)
            {
                throw new Exception("Class name must start with PYYRXX  YY=MajorYear XX=MajorYearRelease");
            }
            MajorYear = int.Parse(parts[0]);
            MajorYearRelease = int.Parse(parts[1]);
            MajorVersionYYPRR = Decimal.Parse($"{parts[0]}.{parts[1]}");
            PatchCounter_Minor = int.Parse(partsOfClassName[1]);
            PatchName = partsOfClassName[2];
            UpScripts = new ScriptList();
            CreateUpScripts();
            DownScripts = new ScriptList();

        }
        public abstract void CreateUpScripts();

        public abstract void CreateDownScripts();

        public List<ScriptDTO> GetSortedScripts()
        {
            return 
            UpScripts.GetSortedScripts();
        }
        protected void AddUpSqlScript(string sqlScript)
        {
            this.UpScripts.Add(sqlScript);
        }

    }

    


    public class ScriptList 
    {
        List<ScriptDTO> _ScriptDTOs;
        int _ListScriptCounter;
        public ScriptList()
        {
            _ListScriptCounter = 1;
            _ScriptDTOs = new List<ScriptDTO>();
        }

        internal void Add(string sqlScript)
        {
            _ScriptDTOs.Add(new ScriptDTO()
            {
                ScriptCounter = this._ListScriptCounter++,
                SqlScript = sqlScript
            });
        }
      
        public List<ScriptDTO> GetSortedScripts() {
            return _ScriptDTOs.OrderBy(r => r.ScriptCounter).ToList();
        }
    }
    public class ScriptDTO
    {

        public int ScriptCounter { get; set; }
        public string SqlScript { get; set; }
        public string OSScript { get; set; }
        public string MessageIfCrash { get; set; }

        public override string ToString()
        {
            return SqlScript;
        }
    }

}
