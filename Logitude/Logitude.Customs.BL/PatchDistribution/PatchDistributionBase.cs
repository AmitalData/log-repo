using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution
{
    public abstract class PatchDistributionBase : IUpScript
    {
        public string Branch { get; }
        public int PatchCounter { get; }

        public string PatchName { get; }

        public string PatchDetails { get; }
        public string What2DOWhileCrash { get; set; }

        public PatchDistributionBase(string patchDetails)
        {
            PatchDetails = patchDetails;
            Type type = this.GetType().UnderlyingSystemType;
            String className = type.Name;
            var partsOfClassName = className.Split("_"[0]).ToList();
            Branch = partsOfClassName[0];
            PatchCounter = int.Parse(partsOfClassName[1]);
            PatchName = partsOfClassName[2];
        }
        public abstract List<ScriptDTO> GetUpScripts();

        public abstract List<ScriptDTO> GetDownScripts();

    }

    public interface IUpScript
    {
        List<ScriptDTO> GetUpScripts();
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
