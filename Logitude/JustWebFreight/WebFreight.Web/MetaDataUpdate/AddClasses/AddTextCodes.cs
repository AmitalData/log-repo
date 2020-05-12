using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.Helpers;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Logitude.Server.Tools.Counters;
namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddTextCodes
    {
        
        private static Dictionary<string, TextCode> AddedTextCodes = new Dictionary<string, TextCode>();
        public static TextCode AddTextCode(TextCodeDetails textCodeDetails, TextCodeRepository textCodeRepository, Dictionary<string, TextCode> textCodes)
        {
            if (textCodes.Keys.Contains(textCodeDetails.Code + textCodeDetails.Tenant + textCodeDetails.ObjectTableId))
            {
                TextCode textCode = textCodes[textCodeDetails.Code + textCodeDetails.Tenant + textCodeDetails.ObjectTableId];
                if (!textCode.IsSpellChecked)
                {
                    textCode.DefaultText = textCodeDetails.DefaultText;
                    textCode.LocalDefaultText = textCodeDetails.LocalDefaultText;
                }
                textCode.DefaultTextPlural = textCodeDetails.DefaultTextPlural;
                textCode.TextCodeTypeCode = textCodeDetails.TextCodeTypeCode;
                textCode.InActive = textCodeDetails.InActive;
                textCode.IsSpellChecked = textCodeDetails.IsSpellChecked;
                textCodeRepository.Update(textCode);
                return textCode;
            }
            else
            {
                if (!AddedTextCodes.ContainsKey(textCodeDetails.Code))
                {
                    TextCode newTextCode = new TextCode()
                    {
                        TextCodeTypeCode = textCodeDetails.TextCodeTypeCode,
                        DefaultTextPlural = textCodeDetails.DefaultTextPlural,
                        DefaultText = textCodeDetails.DefaultText,
                        Code = textCodeDetails.Code,
                        Id = IdCounter.GetNumber("TextCode", textCodeDetails.Tenant).ToString(),
                        ObjectTableId = textCodeDetails.ObjectTableId,
                        Tenant = textCodeDetails.Tenant,
                        LocalDefaultText = textCodeDetails.LocalDefaultText,
                        IsSpellChecked = textCodeDetails.IsSpellChecked,
                    };
                    textCodeRepository.Add(newTextCode);
                    AddedTextCodes.Add(textCodeDetails.Code, newTextCode);
                    return newTextCode;
                }
                else
                    return AddedTextCodes[textCodeDetails.Code];
            }
        }


        public static TextCode AddTextCode(TextCodeDetails textCodeDetails, Dictionary<string, TextCode> textCodes, List<TextCode> addedTextCodes)
        {
            if (textCodes.Keys.Contains(textCodeDetails.Code + textCodeDetails.Tenant + textCodeDetails.ObjectTableId))
            {
                TextCode textCode = textCodes[textCodeDetails.Code + textCodeDetails.Tenant + textCodeDetails.ObjectTableId];
                return textCode;
            }
            else
            {
                TextCode newTextCode = new TextCode()
                {
                    TextCodeTypeCode = textCodeDetails.TextCodeTypeCode,
                    DefaultTextPlural = textCodeDetails.DefaultTextPlural,
                    DefaultText = textCodeDetails.DefaultText,
                    Code = textCodeDetails.Code,
                    Id = IdCounter.GetIdWithIdsRange("TextCode", 100, textCodeDetails.Tenant).ToString(),//IdCounter.GetNumber("TextCode",textCodeDetails.Tenant).ToString(),
                    ObjectTableId = textCodeDetails.ObjectTableId,
                    Tenant = textCodeDetails.Tenant,
                    LocalDefaultText = textCodeDetails.LocalDefaultText,
                    IsSpellChecked = textCodeDetails.IsSpellChecked,
                };
                addedTextCodes.Add(newTextCode);
                return newTextCode;
            }
        }

    }
}