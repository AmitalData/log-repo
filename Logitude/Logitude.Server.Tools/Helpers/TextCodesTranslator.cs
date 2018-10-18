using Logitude.Server.Tools.Helpers;
namespace Logitude.Server.Tools.Helpers
{
    public partial class TextCodesTranslator : ITextCodeTranslator
    {
        public string Translate(string textCodeCode, int tenant)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant);
        }
        public string Translate(string textCodeCode, int tenant, bool getLocalContact)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalContact);
        }
    }
}
