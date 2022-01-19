using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.Data.EntityPOCOs;
using Simplog.Data.Helpers;
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
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.CreatedByUserId = entityParentPM.CreatedByUserId;
                entityPM.TariffId = entityParentPM.Id;
                entityPM.SearchFields = entityPM.Version.ToString();

                if (entityParentPM.TypeCode == "AFC" || entityParentPM.TypeCode == "OLC")
                {
                    entityPM.InitialEnddate = entityPM.ExpirationDate;
                }
                
                if (entityParentPM != null)
                {
                    entityParentPM.LastVersion = entityPM.Version;                
                }
            }
        }

        protected override void OnUpdating(TariffVersionPM entityPM, TariffVersion entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {  
                
            }            
        }

        protected override void UpdateComposition(TariffVersionPM entityPM)
        {
            if (EntityParentPM.TypeCode == "ECC" || EntityParentPM.TypeCode == "ICC")
            {
                this.ComputeLinesUniqueKey_CustomsCharges(entityPM);
            }

            else
            {
                this.ComputeLinesUniqueKey(entityPM);
            }                

            TariffLineUpdateService tariffLineUpdateService = new TariffLineUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            TariffVersionAllInChargeUpdateService tariffVersionAllInChargeUpdateService = new TariffVersionAllInChargeUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);

            tariffLineUpdateService.UpdateMulti(entityPM.TariffLines, entityPM.DeletedTariffLines, entityPM, false);
            tariffVersionAllInChargeUpdateService.UpdateMulti(entityPM.TariffAllInCharges, entityPM.DeletedTariffAllInCharges, entityPM, false);
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
                else if(line.IsFromAllOtherPorts)
                {
                    iUniqueKey = "From All Other Ports";
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
                else if (line.IsToAllOtherPorts)
                {
                    if (iUniqueKey == null)
                    {
                        iUniqueKey = "To All Other Ports";
                    }

                    else
                    {
                        iUniqueKey += "," + "To All Other Ports";
                    }
                }
                if (!string.IsNullOrEmpty(line.ViaPortCode))
                {
                    line.ViaPortCode = line.ViaPortCode.Trim().ToUpper();

                    if (iUniqueKey == null)
                    {
                        iUniqueKey = line.ViaPortCode;
                    }

                    else
                    {
                        iUniqueKey += "," + line.ViaPortCode;
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
                          where d.ChangeSetOp != ChangeSetOperation.Delete
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

                        if (line.LineUniqueKey == null)
                        {
                            line.LineUniqueKeyText = "" + line.Index + index;
                        }

                        else
                        {
                            line.HasErrors = true;
                            line.ErrorText = "Line is a duplicate";
                            line.LineUniqueKeyText = line.LineUniqueKey + line.Index + index;
                        }

                        if (line.ChangeSetOp == ChangeSetOperation.None)
                        {
                            line.ChangeSetOp = ChangeSetOperation.Update;
                        }
                    }
                }
            }
        }
        private void ComputeLinesUniqueKey_CustomsCharges(TariffVersionPM entityPM)
        {
            foreach (TariffLinePM line in entityPM.TariffLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete))
            {
                string iUniqueKey = null;

                if (!string.IsNullOrEmpty(line.FromCountryCode))
                {
                    line.FromCountryCode = line.FromCountryCode.Trim().ToUpper();
                    iUniqueKey = line.FromCountryCode;
                }
                else if (line.IsFromAllOtherCountries)
                {
                    iUniqueKey = "From All Other Countries";
                }

                if (!string.IsNullOrEmpty(line.ToCountryCode))
                {
                    line.ToCountryCode = line.ToCountryCode.Trim().ToUpper();

                    if (iUniqueKey == null)
                    {
                        iUniqueKey = line.ToCountryCode;
                    }

                    else
                    {
                        iUniqueKey += "," + line.ToCountryCode;
                    }
                }
                else if (line.IsToAllOtherCountries)
                {
                    if (iUniqueKey == null)
                    {
                        iUniqueKey = "To All Other Countries";
                    }

                    else
                    {
                        iUniqueKey += "," + "To All Other Countries";
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
                          where d.ChangeSetOp != ChangeSetOperation.Delete
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

                        if (line.LineUniqueKey == null)
                        {
                            line.LineUniqueKeyText = "" + line.Index + index;
                        }

                        else
                        {
                            line.HasErrors = true;
                            line.ErrorText = "Line is a duplicate";
                            line.LineUniqueKeyText = line.LineUniqueKey + line.Index + index;
                        }

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
