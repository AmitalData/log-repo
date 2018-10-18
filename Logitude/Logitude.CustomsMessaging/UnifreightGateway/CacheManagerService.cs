using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.UnifreightGateway
{
    public class CacheManagerService : UnifreightGenericService
    {

        
        private Stopwatch _Stopwatch;
        
        

        public CacheManagerService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {
            ///Simplog.Server.Infrastructure.Helpers.CacheManager.ClearCacheItems();
        }

        public override void ProccessGenericRequest(
              string xmlList,
              ref string MoreParams,
              out string MessageOut
            )
        {
            MessageOut = "";
            _Stopwatch = Stopwatch.StartNew();
            MyCommunicationsParams.Subject = "CacheManagerService";
            bool totest = false;
            if (totest)
            {
                CacheManager.GetOrInsertNewObject("GDFDATA,11", () => { return "11"; });
                CacheManager.GetOrInsertNewObject("will ressist ", () => { return "ressiest !!!"; });
                CacheManager.GetOrInsertNewObject("GDFDATA,1134", () => { return "1134"; });
                CacheManager.GetOrInsertNewObject("GDFDATA,1551", () => { return "1155"; });
            }
            
            DeserilazeObject(xmlList);
            CheckParamValid();
            AppendLogLine("CheckParamValid:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            int cacheItemsCleared = 0;
            switch (_ClearMethodEnum)
            {

                case ClearMethodEnum.All:
                    cacheItemsCleared =CacheManager.ClearCacheItems();
                    break;
                case ClearMethodEnum.StartWith:
                    cacheItemsCleared = CacheManager.ClearCacheItems(
                        (key) =>
                    {
                        bool res = key.StartsWith(this._ClearByPattern);
                        return res;

                    }

                    );
                    break;
                case ClearMethodEnum.EndWith:
                case ClearMethodEnum.Wildcard:
                case ClearMethodEnum.RegEx:
                case ClearMethodEnum.none:
                default:
                    throw new Exception("Not Implemented try (All / StartWith)");
                    break;
            }

            AppendLogLine("ClearCacheItems:" + cacheItemsCleared); 
            AppendLogLine("ClearCacheItems:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            MyGenericResponseObj.Stage = "GetContext";

            
            //MyGenericResponseObj.CorrelationId = responseData.CustomsRequestsSheetId;
            MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
        }

        ClearMethodEnum _ClearMethodEnum = ClearMethodEnum.none;
        private string _ClearMethodEnumString;
        private string _ClearByPattern;

        private void CheckParamValid()
        {
            ClearMethodEnum myClearMethodEnum = ClearMethodEnum.none;
            Enum.TryParse<ClearMethodEnum>(_ClearMethodEnumString, out myClearMethodEnum);
            _ClearMethodEnum = myClearMethodEnum;
            switch (_ClearMethodEnum)
            {
                
                case ClearMethodEnum.All:
                    break;
                case ClearMethodEnum.StartWith:
                    if (String.IsNullOrWhiteSpace(_ClearByPattern))
                    {
                        throw new Exception("While StartWith ,Please add ClearByPattern params !");
                    }
                    break;
                case ClearMethodEnum.EndWith:
                case ClearMethodEnum.Wildcard:
                case ClearMethodEnum.RegEx:
                case ClearMethodEnum.none:
                default:
                    throw new Exception("Not Implemented try (All / StartWith)");
                    break;
            }
            //throw new NotImplementedException();
        }

        private void DeserilazeObject(string xmlList)
        {

            MyGenericResponseObj.Stage = "Initalize ProccessRequest";
            AppendLogLine("CacheManagerService.ProccessRequest");

            AppendLogLine("Deserialize(DataIn1) ..");


            if (string.IsNullOrWhiteSpace(xmlList))
            {
                throw new BusinessErrorException("DataIn1 is missing");
            }
            if (xmlList.Length > 1000)
            {
                AppendLogLine("XmlIn=" + xmlList.Substring(0, 1000));
                AppendLogLine(".Substring(0, 1000)");
            }
            else
            {
                AppendLogLine("XmlIn=" + xmlList);
            }

            AppendLogLine("Tring DeserilazeObject");
            MyGenericResponseObj.Stage = "Trying DeserilazeObject";
            var dic = UnifreightListsUtil.Deserialize(xmlList);

            int tenant = this.ResolvedTenant();
            if (tenant == null)
            {
                throw new Exception("Tenant is missing !!!");
            }
            _ClearMethodEnumString = UnifreightListsUtil.GetValue(ref dic, "ClearMethodEnum");
            _ClearByPattern = UnifreightListsUtil.GetValue(ref dic, "ClearByPattern");


        }

        public override string GetAssemblyQualifiedName()
        {
            throw new NotImplementedException();
        }
        enum ClearMethodEnum
        {
            none,
            All,
            StartWith,
            EndWith,
            Wildcard,
            RegEx
        } 
        public override string GetExampleDataIn1()
        {

            return
@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<ArrayOfEntry xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
<Entry>
  <Key>ClearMethodEnumOptionLIst</Key>
    <Value>
            none,
            All,-Ok
            StartWith,-Ok
            EndWith,-ToBeContinue
            Wildcard,-ToBeContinue
            RegEx-ToBeContinue
    </Value>
 </Entry>
<Entry>
  <Key>ClearMethodEnum</Key>
  <Value>StartWith</Value>
 </Entry>
 <Entry>
  <Key>ClearByPattern</Key>
  <Value>GDFDATA</Value>
 </Entry>
</ArrayOfEntry>
";
        }

        public override string GetExampleDataIn2()
        {
            return "";
        }

        public override string GetExampleDataout1()
        {
            return "";
        }

        public override string GetExampleDataout2()
        {
            return "";
        }

        public override void ProccessRequest(string DataIn1, string DataIn2, out string DataOut1, out string DataOut2, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }

        public override void ProccessBASE64Request(string BASE64DataIn1, string BASE64DataIn2, string BASE64DataIn3, out string BASE64DataOut1, out string BASE64DataOut2, out string BASE64DataOut3, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }
    }
}

