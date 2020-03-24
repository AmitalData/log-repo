using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public class CSVStringLinesGetter
    {
        public string[] GetstringLinesFromCSV(string path)
        {
            string combinedPath = Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), path);
            string[] pathstringSeparators = new string[] { "bin\\Debug\\" };
            string[] pathParts = combinedPath.Split(pathstringSeparators, StringSplitOptions.None);
            string filePath = pathParts[0] + pathParts[1];
            string csvFileTxt = File.ReadAllText(filePath);
            string[] stringSeparators = new string[] { "\r\n" };
            string[] lines = csvFileTxt.Split(stringSeparators, StringSplitOptions.None);
            lines = lines.Where(d => d != lines[0]).ToArray();
            return lines;
        }
    }
}
