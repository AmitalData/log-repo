using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Infrastructure.BL.Validators
{
    public class InfrastructureTranslateTextsClass
    {
        public static string GetTranslation(string textCodeCode, string var1, string var2, string var3, int tenant)
        {
            string result = string.Empty;

            TranslationRepository translationRepository = new TranslationRepository(tenant);
            TextCodeRepository textCodeRepository = new TextCodeRepository(tenant);
            Tenant myTenant = TenantRepository.GetSingleTenant(tenant, true);
            Translation translation = null;
            TextCode textCode = null;
            string textCodeCodeTranslated = "";
            string variable1 = "";
            string variable2 = "";
            string variable3 = "";

            if (myTenant != null)
            {
                translation = translationRepository.GetSingleTranslation(tenant, textCodeCode, myTenant.Language);
                if (translation != null)
                {
                    textCodeCodeTranslated = translation.TranslatedText;
                }

                else if (translation == null)
                {
                    textCode = textCodeRepository.GetTextCodeByTenantAndCode(textCodeCode, tenant);
                    if (textCode != null)
                    {
                        textCodeCodeTranslated = textCode.DefaultText;
                    }
                }

                /* Translated Var1 */
                string var1Translated = "";
                string[] var1Array = var1.Split(',');
                List<string> var1TranslatedList = new List<string>();

                foreach (string variable in var1Array)
                {
                    translation = translationRepository.GetSingleTranslation(tenant, variable, myTenant.Language);
                    if (translation != null)
                    {
                        var1Translated = translation.TranslatedText;
                    }

                    else
                    {
                        textCode = textCodeRepository.GetTextCodeByTenantAndCode(variable, tenant);
                        if (textCode != null)
                        {
                            var1Translated = textCode.DefaultText;
                        }
                    }

                    if (!string.IsNullOrEmpty(var1Translated))
                    {
                        var1TranslatedList.Add(var1Translated);
                    }
                }

                if (var1TranslatedList.Count >= 0)
                {
                    foreach (string s in var1TranslatedList)
                    {
                        variable1 = variable1 + "," + s;

                    }
                    variable1 = variable1.TrimStart(',');
                }

                else
                {
                    variable1 = var1TranslatedList[0];
                }


                /* Translated Var2 */
                string var2Translated = var2;
                translation = translationRepository.GetSingleTranslation(tenant, var2, myTenant.Language);
                if (translation != null)
                {
                    var2Translated = translation.TranslatedText;
                }

                else
                {
                    textCode = textCodeRepository.GetTextCodeByTenantAndCode(var2, tenant);
                    if (textCode != null)
                    {
                        var2Translated = textCode.DefaultText;
                    }
                }

                variable2 = var2Translated;

                /* Translated Var3 */
                string var3Translated = var3;
                translation = translationRepository.GetSingleTranslation(tenant, var3, myTenant.Language);
                if (translation != null)
                {
                    var3Translated = translation.TranslatedText;
                }

                else
                {
                    textCode = textCodeRepository.GetTextCodeByTenantAndCode(var3, tenant);
                    if (textCode != null)
                    {
                        var3Translated = textCode.DefaultText;
                    }
                }

                variable3 = var3Translated;
            }

            if (textCodeCodeTranslated.Contains("%FieldName"))
            {
                result = textCodeCodeTranslated.Replace("%FieldName", variable1);
            }
            if (result.Contains("%Minlength"))
            {
                result = result.Replace("%Minlength", variable2);
            }
            if (result.Contains("%Maxlength"))
            {
                result = result.Replace("%Maxlength", variable3);
            }

            return result;

        }

        public static string Translate(string textCodeCode, int tenant)
        {
            string result = string.Empty;

            TranslationRepository translationRepository = new TranslationRepository(tenant);
            TextCodeRepository textCodeRepository = new TextCodeRepository(tenant);
            Tenant myTenant = TenantRepository.GetSingleTenant(tenant, true);

            if (myTenant != null)
            {
                Translation translation = translationRepository.GetSingleTranslation(tenant, textCodeCode, myTenant.Language);
                if (translation != null)
                {
                    result = translation.TranslatedText;
                }

                else if (translation == null)
                {
                    TextCode textCode = textCodeRepository.GetTextCodeByTenantAndCode(textCodeCode, tenant);
                    if (textCode != null)
                    {
                        result = textCode.DefaultText;
                    }
                }
            }

            return result;
            //return "! " + Result;
        }
    }
}
