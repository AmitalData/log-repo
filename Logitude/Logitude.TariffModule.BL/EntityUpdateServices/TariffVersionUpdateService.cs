using Logitude.TariffModule.BL.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityUpdateServices
{
    public partial class TariffVersionUpdateService
    {
        protected override void OnCreating(TariffVersionPM entityPM, TariffPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                if (entityParentPM != null)
                {
                    entityParentPM.LastVersion = entityPM.Version;
                }
            }
        }

        protected override void UpdateComposition(TariffVersionPM entityPM)
        {
            this.ComputeLinesUniqueKey(entityPM);

            TariffLineUpdateService tariffLineUpdateService = new TariffLineUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            tariffLineUpdateService.UpdateMulti(entityPM.TariffLines, entityPM.DeletedTariffLines, entityPM, false);
        }

        private void ComputeLinesUniqueKey(TariffVersionPM entityPM)
        {
            foreach (TariffLinePM line in entityPM.TariffLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete))
            {
                string iUniqueKey = null;

                if (!string.IsNullOrEmpty(line.OriginPortCode))
                {
                    line.OriginPortCode = line.OriginPortCode.Trim().ToUpper();
                    iUniqueKey = line.OriginPortCode;
                }

                if (!string.IsNullOrEmpty(line.DestinationPortCode))
                {
                    line.DestinationPortCode = line.DestinationPortCode.Trim().ToUpper();

                    if (iUniqueKey == null)
                    {
                        iUniqueKey = line.DestinationPortCode;
                    }

                    else
                    {
                        iUniqueKey += "," + line.DestinationPortCode;
                    }
                }

                if (line.ErrorText == "Line is a duplicate")
                {
                    line.HasErrors = false;
                    line.ErrorText = null;
                }

                line.LineUniqueKey = iUniqueKey;
                line.LineUniqueKeyText = iUniqueKey;

                if (line.ChangeSetOp == ChangeSetOperation.None)
                {
                    line.ChangeSetOp = ChangeSetOperation.Update;
                }
            }

            var groupd = (from d in entityPM.TariffLines
                          where d.LineUniqueKey != null && d.ChangeSetOp != ChangeSetOperation.Delete
                          group d by d.LineUniqueKey into g
                          select new
                          {
                              LineUniqueKey = g.Key,
                              CountOfDuplication = g.Count(),
                          }).ToList();


            if (groupd.Where(d => d.CountOfDuplication > 1).Any())
            {
                foreach (var item in groupd.Where(d => d.CountOfDuplication > 1))
                {
                    int index = 0;

                    foreach (TariffLinePM line in entityPM.TariffLines.Where(d => d.LineUniqueKey == item.LineUniqueKey))
                    {
                        index++;
                        line.HasErrors = true;
                        line.ErrorText = "Line is a duplicate";
                        line.LineUniqueKeyText = line.LineUniqueKey + index;

                        if (line.ChangeSetOp == ChangeSetOperation.None)
                        {
                            line.ChangeSetOp = ChangeSetOperation.Update;
                        }
                    }
                }
            }


        }

    }
}
