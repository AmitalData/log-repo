using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Interfaces
{
    public interface ITranslateTextsClassUtil
    {
        string GetTranslation(string textCodeCode, string var1, string var2, string var3, int tenant);
        string Translate(string textCodeCode, int tenant);
        string Translate(string textCodeCode, int tenant, bool getLocalDefaultText);
        string GetRequiredFieldForTableMessageTranslation(string requiredTextCodeCode, string fieldName, string tableName, string entityReference, int tenant);

    }
}
