using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.CloseTables
{
    public class CustomsPartnerFtpDetails
    {
        public const string InterfaceName_SubManifest = "SUBMANIFEST";
        public const string PartnerCode_Mamam = "MAMAN";
        public const string TypeCode_Out = "OUT";
        public const string TypeCode_In = "IN";

        public List<KeyValuePair<string,string>> GetAllInterfaceName()
        {
            var all = new List<KeyValuePair<string, string>>();
            all.Add(new KeyValuePair<string, string>("", ""));
            all.Add(new KeyValuePair<string, string>(InterfaceName_SubManifest, "SubManifest"));
            //all.Add(new KeyValuePair<string, string>("TST", "Test"));
            return all;
        }

        public List<KeyValuePair<string, string>> GetAllPartnerCode()
        {
            var all = new List<KeyValuePair<string, string>>();
            all.Add(new KeyValuePair<string, string>("", ""));
            all.Add(new KeyValuePair<string, string>(PartnerCode_Mamam, "Mamam"));
            return all;
        }

        public List<KeyValuePair<string, string>> GetAllTypeCode()
        {
            var all = new List<KeyValuePair<string, string>>();
            all.Add(new KeyValuePair<string, string>("", ""));
            all.Add(new KeyValuePair<string, string>(TypeCode_Out, "Out"));
            all.Add(new KeyValuePair<string, string>(TypeCode_In, "In"));
            return all;
        }

    }

}
