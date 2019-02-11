using Logitude.Server.Tools.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Mocks
{
    public class MockTranslateTextsClassUtil : ITranslateTextsClassUtil
    {
        public string GetRequiredFieldForTableMessageTranslation(string requiredTextCodeCode, string fieldName, string tableName, string entityReference, int tenant)
        {
            return requiredTextCodeCode; //+ " " + fieldName + " " + tableName + " " + entityReference + " " + tenant;
        }

        public string GetTranslation(string textCodeCode, string var1, string var2, string var3, int tenant)
        {
            return textCodeCode;// + " " + var1 + " " + var2 + " " + var3 + " " + tenant;
        }

        public string Translate(string textCodeCode, int tenant)
        {
            return textCodeCode;// + " " + tenant;
        }

        public string Translate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return textCodeCode;// + " " + tenant + " " + getLocalDefaultText;
        }
    }
}
