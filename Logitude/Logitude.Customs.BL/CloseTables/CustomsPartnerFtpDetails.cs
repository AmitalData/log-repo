using Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue;
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
        public const string InterfaceName_ECTHR = "ECTHR";//EC = E-Commerce
        public const string InterfaceName_ECSPCL = "ECSPCL";//EC = E-Commerce
        public const string InterfaceName_ECSTB = "ECSTB";//EC = E-Commerce
        public const string InterfaceName_ECSTB_Splited = "ECSTB+P";//EC = E-Commerce
        public const string InterfaceName_Ftp2Maman2470 = "ECM2470";//EC = E-Commerce 2 maman 2470
        public const string PartnerCode_Mamam = "MAMAN";
        public const string TypeCode_Out = "OUT";
        public const string TypeCode_In = "IN";

        public List<InterfaceDetails> GetAllInterfaceDetails()
        {
            var all = new List<InterfaceDetails>() { 
            //all.Add(new KeyValuePair<string, string>("", ""));

            new InterfaceDetails()
            {
                Code = InterfaceName_SubManifest,
                Name = "תת מצהר לממן",
                TypeCode = TypeCode_Out,
                Partner = PartnerCode_Mamam,
                ViaMethod = GetViaMethods().First(r => r.Key == "FTP").Key
            }
            ,
            new InterfaceDetails()
            {
                Code = InterfaceName_ECTHR,
                Name = "ש.מ.ב לממן",
                TypeCode = TypeCode_Out,
                Partner = PartnerCode_Mamam,
                ViaMethod = GetViaMethods().First(r => r.Key == "WEBAPI").Key
            }

            , new InterfaceDetails()
            {
                Code = InterfaceName_ECSPCL,
                Name = "פעולות מיוחדות לממן",
                TypeCode = TypeCode_Out,
                Partner = PartnerCode_Mamam,
                ViaMethod = GetViaMethods().First(r => r.Key == "WEBAPI").Key
            }
            ,

            new InterfaceDetails()
            {
                Code = InterfaceName_ECSTB,
                Name = "סטטוס/זמינות ממן",
                TypeCode = TypeCode_In,
                Partner = PartnerCode_Mamam,
                ViaMethod = GetViaMethods().First(r => r.Key == "FTP").Key,
                
                AnalyzeQueueService= AnalyzeQueueServiceEnum.MamanStatusAvailabilitySpliterService,
                Subject="Status / Availability Maman Raw"
            },
            new InterfaceDetails()
            {
                Code = InterfaceName_ECSTB_Splited,
                Name = "סטטוס/זמינות ממן",
                TypeCode = TypeCode_In,
                Partner = PartnerCode_Mamam,
                ViaMethod = GetViaMethods().First(r => r.Key == "FTP").Key,
                
                AnalyzeQueueService= AnalyzeQueueServiceEnum.MamanStatusAvailabilityService,
                Subject="Status / Availability Maman",
                ServerInternalDef= true
            },
            new InterfaceDetails()
            {
                Code = InterfaceName_Ftp2Maman2470,
                Name = "מסר 2470",
                TypeCode = TypeCode_Out,
                Partner = PartnerCode_Mamam,
                ViaMethod = GetViaMethods().First(r => r.Key == "FTP").Key,

                //AnalyzeQueueService= AnalyzeQueueServiceEnum.MamanStatusAvailabilityService,
                Subject="2470 to Maman",
                
            },
            

            };
            ///

            //all.Add(new KeyValuePair<string, string>("TST", "Test"));
            return all;
        }

        public List<KeyValuePair<string,string>> GetAllInterfaceName()
        {
            var all = new List<KeyValuePair<string, string>>();
            //all.Add(new KeyValuePair<string, string>("", ""));

            var allInterfaceDetails = GetAllInterfaceDetails().Where(r => !r.ServerInternalDef).ToList();
            allInterfaceDetails.ForEach(r =>
            {
                all.Add(new KeyValuePair<string, string>(r.Code,
                    ProxyUtil.JsonConvertSerialize(r)));
            });
            return all;
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

            all.Add(new KeyValuePair<string, string>(InterfaceName_ECSTB,
                ProxyUtil.JsonConvertSerialize(new InterfaceDetails()
                {
                    Code = InterfaceName_ECSTB,
                    Name = "סטטוס/זמינות ממן",
                    TypeCode = TypeCode_In,
                    Partner = PartnerCode_Mamam,
                    ViaMethod = GetViaMethods().First(r => r.Key == "FTP").Key
                }
            )));
            ///

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

        public CustomAnalyzerQueueBase GetCustomAnalyzerQueueService(InterfaceDetails @interface)
        {
            switch (@interface.AnalyzeQueueService)
            {
                case AnalyzeQueueServiceEnum.MamanStatusAvailabilitySpliterService:
                    return new MamanStatusAvailabilitySplitterService(@interface);
                    break;
                case AnalyzeQueueServiceEnum.MamanStatusAvailabilityService:
                    return new MamanStatusAvailabilityService(@interface);
                    break;
                default:
                    throw new Exception("No analyze service define " + @interface.Code);
                    break;
            }
        }
    }
    public enum AnalyzeQueueServiceEnum
    {
        none,
        MamanStatusAvailabilitySpliterService,
        MamanStatusAvailabilityService,
    }
    public class InterfaceDetails
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public string TypeCode { get; set; }
        public string Partner { get; set; }
        public string ViaMethod { get; set; }
        //public string QueueName { get; set; }
        public AnalyzeQueueServiceEnum AnalyzeQueueService { get; internal set; }
        public string Subject { get; internal set; }
        public bool ServerInternalDef { get; set; }
    }


    public class WebApiDefinitionDTO
    {
        public string WEBAPIURL { get; set; }
        public string WEBAPIAuthenticationURL { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

    }

}
