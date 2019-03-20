using Logitude.Customs.BL.Messaging;
using Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue;
using Logitude.Customs.BL.Messaging.ILOVS;
using Logitude.Customs.BL.Messaging.Maman;
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
        public const string InterfaceName_ECMMNTHR_REQUEST = "ECTHR";//EC = E-Commerce
        public const string InterfaceName_ECMMNTHR_RESPONE = "ECTH+RS";//EC = E-Commerce
        public const string InterfaceName_ECOVSTHR = "ECOVSTHR";//EC = E-Commerce
        public const string InterfaceName_ECOVSTHR_Response = "ECOVSTHR+RS";//EC = E-Commerce
        public const string InterfaceName_ECMMNSPCL_REQUEST = "ECSPCL";//EC = E-Commerce
        public const string InterfaceName_ECMMNSPCL_Response = "ECSPCL+RS";//EC = E-Commerce

        public const string InterfaceName_ECOVSSPCL_REQUEST = "ECOVSSPCL+RQ";//EC = E-Commerce
        public const string InterfaceName_ECOVSSPCL_RESPONE = "ECOVSSPCL+RS";//EC = E-Commerce
        public const string InterfaceName_ECSTB = "ECSTB";//EC = E-Commerce
        public const string InterfaceName_ECOVSTB = "ECOVSSTB";//EC = E-Commerce
        public const string InterfaceName_ECSTB_Splited = "ECSTB+P";//EC = E-Commerce
        public const string InterfaceName_ECOVSTB_Splited = "ECOVSSTB+P";//EC = E-Commerce
        public const string InterfaceName_Ftp2Maman2470 = "ECM2470";//EC = E-Commerce 2 maman 2470
        public const string PartnerCode_Mamam = "MAMAN";
        public const string PartnerCode_ILOVS = "ILOVS";
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
                Code = InterfaceName_ECMMNTHR_REQUEST,
                Name = "ש.מ.ב לממן",
                TypeCode = TypeCode_Out,
                Partner = PartnerCode_Mamam,
                ViaMethod = GetViaMethods().First(r => r.Key == "WEBAPI").Key,
                 WEBAPICredentialType = CourierWEBAPICredentialType.Bearer,
                ResponseCode = InterfaceName_ECMMNTHR_RESPONE
            }
            ,
            new InterfaceDetails()
            {
                Code = InterfaceName_ECMMNTHR_RESPONE,
                Name = "ש.מ.ב מממן",
                TypeCode = TypeCode_In,
                Partner = PartnerCode_Mamam,
                ViaMethod = GetViaMethods().First(r => r.Key == "WEBAPI").Key,

                AnalyzeQueueService= AnalyzeQueueServiceEnum.MamanQHAWBService,
                Subject="ש.מ.ב מממן",
                ServerInternalDef= true

            }
            ,
            
            new InterfaceDetails()
            {
                Code = InterfaceName_ECOVSTHR,
                Name = "ש.מ.ב לאוברסיז",
                TypeCode = TypeCode_Out,
                Partner = PartnerCode_ILOVS,
                ViaMethod = GetViaMethods().First(r => r.Key == "WEBAPI").Key,
                ResponseCode=InterfaceName_ECOVSTHR_Response,
                WEBAPICredentialType = CourierWEBAPICredentialType.NetworkCredential

            }

            , new InterfaceDetails()
            {
                Code = InterfaceName_ECMMNSPCL_REQUEST,
                Name = "פעולות מיוחדות לממן",
                TypeCode = TypeCode_Out,
                Partner = PartnerCode_Mamam,
                ViaMethod = GetViaMethods().First(r => r.Key == "WEBAPI").Key,
                WEBAPICredentialType = CourierWEBAPICredentialType.Bearer,
                ResponseCode =InterfaceName_ECMMNSPCL_Response,
            }
            ,
            new InterfaceDetails()
            {
                Code = InterfaceName_ECMMNSPCL_Response,
                Name = "פעולות מיוחדות מממן",
                TypeCode = TypeCode_In,
                Partner = PartnerCode_Mamam,
                ViaMethod = GetViaMethods().First(r => r.Key == "WEBAPI").Key,

                AnalyzeQueueService= AnalyzeQueueServiceEnum.MamanQSPCLService,
                Subject="פעולות מיוחדות מממן",
                ServerInternalDef= true

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
                Subject="Status/Availability Maman Raw"
            },
            new InterfaceDetails()
            {
                Code = InterfaceName_ECSTB_Splited,
                Name = "סטטוס/זמינות ממן",
                TypeCode = TypeCode_In,
                Partner = PartnerCode_Mamam,
                ViaMethod = GetViaMethods().First(r => r.Key == "FTP").Key,
                
                AnalyzeQueueService= AnalyzeQueueServiceEnum.MamanStatusAvailabilityService,
                Subject="Status/Availability Maman",
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
                
            }
            ,
            new InterfaceDetails()
            {


                Code = InterfaceName_ECOVSTHR_Response,
                Name = "ש.מ.ב מאוברסיז",
                TypeCode = TypeCode_In,
                Partner = PartnerCode_ILOVS,
                ViaMethod = GetViaMethods().First(r => r.Key == "WEBAPI").Key,


                AnalyzeQueueService= AnalyzeQueueServiceEnum.OVSHAWBService,
                Subject="ש.מ.ב מאוברסיז",
                ServerInternalDef= true
            },

             new InterfaceDetails()
            {
                Code = InterfaceName_ECOVSSPCL_REQUEST,
                Name = "פעולות מיוחדות לאוברסיז",
                TypeCode = TypeCode_Out,
                Partner = PartnerCode_ILOVS,
                ViaMethod = GetViaMethods().First(r => r.Key == "WEBAPI").Key,
                 ResponseCode =InterfaceName_ECOVSSPCL_RESPONE,
                 WEBAPICredentialType = CourierWEBAPICredentialType.NetworkCredential
            }
             ,
             new InterfaceDetails()
            {
                Code = InterfaceName_ECOVSSPCL_RESPONE,
                Name = "פעולות מיוחדות מאוברסיז",
                TypeCode = TypeCode_In,
                Partner = PartnerCode_ILOVS,
                ViaMethod = GetViaMethods().First(r => r.Key == "WEBAPI").Key,

                 AnalyzeQueueService = AnalyzeQueueServiceEnum.OVSSpecialActionService,
                Subject= "פעולות מיוחדות מאוברסיז",
                ServerInternalDef= true

            }
             ,

            new InterfaceDetails()
            {
                Code = InterfaceName_ECOVSTB,
                Name = "סטטוס/זמינות מאוברסיז",
                TypeCode = TypeCode_In,
                Partner = PartnerCode_ILOVS,
                ViaMethod = GetViaMethods().First(r => r.Key == "FTP").Key,

                AnalyzeQueueService= AnalyzeQueueServiceEnum.OVSStatusAvailabilitySpliterService,
                Subject="Status/Availability ILOVS Raw"
            },
            new InterfaceDetails()
            {
                Code = InterfaceName_ECOVSTB_Splited,
                Name = "סטטוס/זמינות מאוברסיז",
                TypeCode = TypeCode_In,
                Partner = PartnerCode_ILOVS,
                ViaMethod = GetViaMethods().First(r => r.Key == "FTP").Key,

                AnalyzeQueueService= AnalyzeQueueServiceEnum.OVSStatusAvailabilityService,
                Subject="Status/Availability ILOVS",
                ServerInternalDef= true
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

        }

        public List<KeyValuePair<string, string>> GetAllPartnerCode()
        {
            var all = new List<KeyValuePair<string, string>>();
            all.Add(new KeyValuePair<string, string>("", ""));
            all.Add(new KeyValuePair<string, string>(PartnerCode_Mamam, "Mamam"));
            all.Add(new KeyValuePair<string, string>(PartnerCode_ILOVS, "Overseas"));
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
                    return new MamanStatusAvailabilitySplitterQService(@interface);
                    break;
                case AnalyzeQueueServiceEnum.MamanStatusAvailabilityService:
                    return new MamanStatusAvailabilityQService(@interface);
                    break;
                case AnalyzeQueueServiceEnum.OVSHAWBService:
                    return new CourierOVSHAWBQService(@interface);
                case AnalyzeQueueServiceEnum.OVSSpecialActionService:
                    return new CourierOVSSpecialActionQService(@interface);
                case AnalyzeQueueServiceEnum.OVSStatusAvailabilitySpliterService:
                    return new CourierOVSStatusAvailabilitySplitterQService(@interface);

                case AnalyzeQueueServiceEnum.OVSStatusAvailabilityService:
                    return new CourierOVSStatusAvailabilityQService(@interface);
                case AnalyzeQueueServiceEnum.MamanQHAWBService:
                    return new MamanQHAWBService(@interface);
                case AnalyzeQueueServiceEnum.MamanQSPCLService:
                    return new MamanQSPCLService(@interface);
                default:

                    throw new Exception("No analyze service define " + @interface.Code);
                    break;
            }
        }
#if false
        public IWebAPIMessage2MamanAnalyzer GetResponseService(/*Courier2MamanCommSettings courier2MamanCommSettings*/string MessageCode)
        {
            IWebAPIMessage2MamanAnalyzer analyzer = null;

            switch (/*courier2MamanCommSettings.*/MessageCode)
            {
                case CustomsPartnerFtpDetails.InterfaceName_ECMMNTHR_REQUEST:
                    {
                        analyzer = new CourierGWMessageECTHRDataMamanResponseService();
                    }
                    break;
                case CustomsPartnerFtpDetails.InterfaceName_ECMMNSPCL_REQUEST:
                    {
                        analyzer = new CourierGWMessageECSpclMamanResponseService();
                    }
                    break;
                case CustomsPartnerFtpDetails.InterfaceName_ECOVSTHR:
                    {
                        analyzer = new CourierOVSECTHMessageResponseService();
                    }
                    break;
                default:
                    throw new Exception("Please register  ");
                    break;
            }

            return analyzer;
        }
#endif

    }
    public enum AnalyzeQueueServiceEnum
    {
        none,
        MamanStatusAvailabilitySpliterService,
        MamanStatusAvailabilityService,
        OVSHAWBService,
        OVSSpecialActionService,
        OVSStatusAvailabilitySpliterService,
        OVSStatusAvailabilityService,
        MamanQHAWBService,
        MamanQSPCLService,
    }
    public enum CourierWEBAPICredentialType
    {
        none,
        Bearer,
        NetworkCredential,
    }
    public class InterfaceDetails
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public string TypeCode { get; set; }
        public string Partner { get; set; }
        public string ViaMethod { get; set; }
        public CourierWEBAPICredentialType WEBAPICredentialType { get; set; }
        //public string QueueName { get; set; }
        public AnalyzeQueueServiceEnum AnalyzeQueueService { get; internal set; }
        public string Subject { get; internal set; }
        public bool ServerInternalDef { get; set; }
        public string ResponseCode { get; set; }
    }


    public class WebApiDefinitionDTO
    {
        public string WEBAPIURL { get; set; }
        public string WEBAPIAuthenticationURL { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

    }

}
