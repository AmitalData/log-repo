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
                   // newTextCode.LocalDefaultText = TryConvertFromBase64(newTextCode.LocalDefaultText);

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
              
                //newTextCode.LocalDefaultText = TryConvertFromBase64(newTextCode.LocalDefaultText);

                addedTextCodes.Add(newTextCode);
                return newTextCode;
            }
        }



        public static string TryConvertFromBase64(string input)
        {
            try
            {
                if (input == null)
                {
                    return null;
                }
                if (input.StartsWith("BS64:") || input.StartsWith("\"BS64:"))
                {

                    return ConvertFromBase64(input);

                   
                }
                return input;

            }
            catch (FormatException)
            {
                return input;
            }
        }
      
        private static string ConvertFromBase64(string input)
        {
            string substringToRemove = "\"";
            string backUp = input;
            try
            {
                input = input.Trim('\"');
                input = input.Substring(5);//REMOVE BS64:
                byte[] data = Convert.FromBase64String(input);
                string decodedString = Encoding.UTF8.GetString(data);
               
                return decodedString;

            }
            catch (FormatException)
            {
                return backUp;
            }

        }
   
       
     

    }
}