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
        public int MajorYear { get; }
        public int MajorYearRelease { get; }
        public decimal MajorVersion { get; }
        public int PatchCounter_Minor { get; }

        public string PatchName { get; }

        public string PatchDetails { get; }
        public DateTime CreatedAt { get; }
        public string What2DOWhileCrash { get; set; }

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
            MajorVersion = Decimal.Parse($"{MajorYear}.{MajorYearRelease}");
            PatchCounter_Minor = int.Parse(partsOfClassName[1]);
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
