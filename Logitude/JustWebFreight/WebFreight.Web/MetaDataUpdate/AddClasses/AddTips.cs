using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.Helpers;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Logitude.Server.Tools.Counters;
namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddTips
    {
        public static Tip AddTip(TipDetails tipDetails, TipRepository tipsRepository, TextCodeRepository textCodeRepository, Dictionary<string, Tip> tenantZeroTips, Dictionary<string, TextCode> tenantZeroTextCodes)
        {
            if (!tenantZeroTips.Keys.Contains(tipDetails.Code))
            {
                Tip tip = new Tip()
                {
                    Code = tipDetails.Code,
                    ObjectTableId = tipDetails.ObjectTableId,
                    Tenant = tipDetails.Tenant,
                    VisibilityDefaultValue = tipDetails.VisibilityDefaultValue,

                };

                if (tipDetails.ShortTextCodeDefaultText != null)
                {
                    TextCode tipTextCode = new TextCode();
                    tipTextCode.Id = IdCounter.GetNumber("TextCode",tipDetails.Tenant).ToString();
                    tipTextCode.ObjectTableId = tipDetails.ObjectTableId;
                    tipTextCode.Code = tipDetails.ShortTextCodeCode;
                    tipTextCode.DefaultText = tipDetails.ShortTextCodeDefaultText;

                    tipTextCode.Tenant = 0;
                    tipTextCode.TextCodeTypeCode = "TIP";
                    textCodeRepository.Add(tipTextCode);
                    tip.ShortTextCode = tipTextCode.Id;
                    tip.ShortTextCodeCode = tipTextCode.Code;
                }


                tipsRepository.Add(tip);

                return tip;

            }
            else
            {
                Tip updatedTip = tenantZeroTips[tipDetails.Code];
                updatedTip.ObjectTableId = tipDetails.ObjectTableId;
                updatedTip.Tenant = tipDetails.Tenant;
                updatedTip.VisibilityDefaultValue = tipDetails.VisibilityDefaultValue;

                if (tipDetails.ShortTextCodeDefaultText != null)
                {
                    TextCode tipTextCode = null;
                    if (tenantZeroTextCodes.Keys.Contains(tipDetails.ShortTextCodeCode + tipDetails.Tenant + tipDetails.ObjectTableId))
                    {
                        tipTextCode = tenantZeroTextCodes[tipDetails.ShortTextCodeCode + tipDetails.Tenant + tipDetails.ObjectTableId];
                        if (!tipTextCode.IsSpellChecked)
                        {
                            tipTextCode.DefaultText = tipDetails.ShortTextCodeDefaultText;

                            textCodeRepository.Update(tipTextCode);
                        }
                    }

                    else
                    {
                        tipTextCode = new TextCode();
                        tipTextCode.Id = IdCounter.GetNumber("TextCode",0).ToString();
                        tipTextCode.ObjectTableId = tipDetails.ObjectTableId;
                        tipTextCode.Code = tipDetails.ShortTextCodeCode;
                        tipTextCode.DefaultText = tipDetails.ShortTextCodeDefaultText;

                        tipTextCode.Tenant = 0;
                        tipTextCode.TextCodeTypeCode = "TIP";
                        textCodeRepository.Add(tipTextCode);
                        updatedTip.ShortTextCode = tipTextCode.Id;
                        updatedTip.ShortTextCodeCode = tipTextCode.Code;

                    }
                }

                tipsRepository.Update(updatedTip);

                return updatedTip;

            }
        }
    }
}