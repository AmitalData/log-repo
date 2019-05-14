using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools.Mocks;
using Logitude.Server.Tools.Utils;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Resolvers
{
    public class TranslateTextsClassUtilResolver
    {
        public static void RegisterTranslateTextsClassUtil()
        {
            ContainerAccessor.Container.RegisterType<ITranslateTextsClassUtil, TranslateTextsClassUtil>("TranslateTextsClassUtil", new InjectionFactory(c => new TranslateTextsClassUtil()));
        }

        public static void RegisterMockTranslateTextsClassUtil()
        {
            ContainerAccessor.Container.RegisterType<ITranslateTextsClassUtil, MockTranslateTextsClassUtil>("TranslateTextsClassUtil", new InjectionFactory(c => new MockTranslateTextsClassUtil()));
        }

        public static string Translate(string textCodeCode, int tenant)
        {
            ITranslateTextsClassUtil translateTextsClassUtil = ResolveITranslateTextClassUtil(tenant);
            return translateTextsClassUtil.Translate(textCodeCode, tenant);
        }

        public static string Translate(string textCodeCode, int tenant,bool getLocalDefaultText)
        {
            ITranslateTextsClassUtil translateTextsClassUtil = ResolveITranslateTextClassUtil(tenant);
            return translateTextsClassUtil.Translate(textCodeCode, tenant, getLocalDefaultText);
        }

        public static string GetRequiredFieldForTableMessageTranslation(string requiredTextCodeCode, string fieldName, string tableName, string entityReference, int tenant)
        {
            ITranslateTextsClassUtil translateTextsClassUtil = ResolveITranslateTextClassUtil(tenant);
            return requiredTextCodeCode + " " + fieldName + " " + tableName + " " + entityReference + " " + tenant;
        }

        public static string GetTranslation(string textCodeCode, string var1, string var2, string var3, int tenant)
        {
            return textCodeCode + " " + var1 + " " + var2 + " " + var3 + " " + tenant;
        }

        public static ITranslateTextsClassUtil ResolveITranslateTextClassUtil(int tenant)
        {
            ITranslateTextsClassUtil translateTextsClassUtil = ContainerAccessor.Container.Resolve(typeof(ITranslateTextsClassUtil), "TranslateTextsClassUtil", new ParameterOverride("", tenant)) as ITranslateTextsClassUtil;
            return translateTextsClassUtil;

        }
    }
}
