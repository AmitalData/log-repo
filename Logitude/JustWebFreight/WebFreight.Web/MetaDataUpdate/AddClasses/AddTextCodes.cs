using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.Helpers;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Logitude.Server.Tools.Counters;
using System.Text.RegularExpressions;
using System.Text;
using System;
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
                textCode.LocalDefaultText = TryConvertFromBase64(textCode.LocalDefaultText);

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
                    newTextCode.LocalDefaultText = TryConvertFromBase64(newTextCode.LocalDefaultText);

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
              
                newTextCode.LocalDefaultText = TryConvertFromBase64(newTextCode.LocalDefaultText);

                addedTextCodes.Add(newTextCode);
                return newTextCode;
            }
        }

        private const string Base64Prefix =  "bs64:";


        public static string TryConvertFromBase64(string input)
        {
            try
            {
                if (input.StartsWith(Base64Prefix))
                {
                    input = input.Substring(Base64Prefix.Length);
                    string convertedInput = ConvertFromBase64(input);
                    convertedInput = convertedInput.Replace("\"", "");

                    if (IsHebrew(convertedInput))
                        return convertedInput;
                    return input;
                }
                return input;

            }
            catch (FormatException)
            {
                return input;
            }
        }
        private static bool IsHebrew(string text)
        {
            return text.Any(c => c >= '\u0590' && c <= '\u05FF');
        }

        private static string ConvertFromBase64(string input)
        {
            byte[] bytes = Convert.FromBase64String(input);
            string decodedString = Encoding.UTF8.GetString(bytes);
            return SanitizeXmlString(decodedString);
        }
        private static string SanitizeXmlString(string xml)
        {
            StringBuilder buffer = new StringBuilder(xml.Length);

            foreach (char c in xml)
            {
                if (IsLegalXmlChar(c))
                {
                    buffer.Append(c);
                }
                else
                {
                    // Optionally, you can replace invalid characters with a placeholder
                    // buffer.Append('?');
                }
            }

            return buffer.ToString();
        }
        private static bool IsLegalXmlChar(int character)
        {
            return
            (
                character == 0x9 /* == '\t' == 9   */          ||
                character == 0xA /* == '\n' == 10  */          ||
                character == 0xD /* == '\r' == 13  */          ||
                (character >= 0x20 && character <= 0xD7FF) ||
                (character >= 0xE000 && character <= 0xFFFD) ||
                (character >= 0x10000 && character <= 0x10FFFF)
            );
        }

    }
}