namespace Logitude.Server.Tools.Helpers
{
    public partial class TextCodesTranslator : ITextCodeTranslator
    {
        public static string TranslateText(string textCodeCode, int tenant)
        {
            Helpers.TextCodesTranslator translator = new Helpers.TextCodesTranslator();
            var  TranslateText= translator.Translate(textCodeCode, tenant);
            if (string.IsNullOrWhiteSpace(TranslateText))
            {
                TranslateText = "$Text(" + textCodeCode + ")";//Our Version Of Uniface Convention   
            }
            return TranslateText;
        }
        public static string TranslateText(string textCodeCode, int tenant, bool useLoggedContact) // get logged contact and translate to local/englsih
        {
            Helpers.TextCodesTranslator translator = new Helpers.TextCodesTranslator();
            var TranslateText = translator.Translate(textCodeCode, tenant,true);
            if (string.IsNullOrWhiteSpace(TranslateText))
            {
                TranslateText = "$Text(" + textCodeCode + ")";//Our Version Of Uniface Convention   
            }
            return TranslateText;
        }
    }

    public interface ITextCodeTranslator
    {
        string Translate(string textCodeCode, int tenant);
    }
}