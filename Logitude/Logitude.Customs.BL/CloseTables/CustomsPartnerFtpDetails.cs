using Logitude.Server.Tools.Utils;
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
        public const string InterfaceName_ECTHR = "ECTHR";
        public const string InterfaceName_ECSPCL = "ECSPCL";
        public const string PartnerCode_Mamam = "MAMAN";
        public const string TypeCode_Out = "OUT";
        public const string TypeCode_In = "IN";

        public List<KeyValuePair<string,string>> GetAllInterfaceName()
        {
            var all = new List<KeyValuePair<string, string>>();
            //all.Add(new KeyValuePair<string, string>("", ""));

            all.Add(new KeyValuePair<string, string>(InterfaceName_SubManifest,
                ProxyUtil.JsonConvertSerialize(new InterfaceDetails()
                {
                    Code = InterfaceName_SubManifest,
                    Name = "תת מצהר לממן",
                     TypeCode= TypeCode_Out,
                    Partner = PartnerCode_Mamam,
                    ViaMethod = GetViaMethods().First(r => r.Key == "FTP").Key
                }
            )));
            all.Add(new KeyValuePair<string, string>(InterfaceName_ECTHR,
                ProxyUtil.JsonConvertSerialize(new InterfaceDetails()
                {
                    Code = InterfaceName_ECTHR,
                    Name = "ש.מ.ב לממן",
                    TypeCode = TypeCode_Out,
                    Partner = PartnerCode_Mamam,
                    ViaMethod = GetViaMethods().First(r => r.Key == "WEBAPI").Key
                }
            )));
            all.Add(new KeyValuePair<string, string>(InterfaceName_ECSPCL,
                ProxyUtil.JsonConvertSerialize(new InterfaceDetails()
                {
                    Code = InterfaceName_ECSPCL,
                    Name = "פעולות מיוחדות לממן",
                    TypeCode = TypeCode_Out,
                    Partner = PartnerCode_Mamam,
                    ViaMethod = GetViaMethods().First(r => r.Key == "WEBAPI").Key
                }
            )));

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
        public List<KeyValuePair<string, string>> GetViaMethods()
        {
            var all = new List<KeyValuePair<string, string>>();
            all.Add(new KeyValuePair<string, string>("", ""));
            all.Add(new KeyValuePair<string, string>("FTP", "FTP"));
            all.Add(new KeyValuePair<string, string>("WEBAPI", "WEBAPI"));
            return all;
        }
        

    }
    class InterfaceDetails
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public string TypeCode { get; set; }
        public string Partner { get; set; }
        public string ViaMethod { get; set; }
    }


    public class WebApiDefinitionDTO
    {
        public string WEBAPIURL { get; set; }
        public string WEBAPIAuthenticationURL { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

    }
}
