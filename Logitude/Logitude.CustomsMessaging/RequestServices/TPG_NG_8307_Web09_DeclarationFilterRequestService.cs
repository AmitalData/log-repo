//Yuval Chalup 07.09.2015 TASK-15037
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CreditQueryServiceReference;
using UnifreightIIG.Common.DeclarationFilterParamServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class TPG_NG_8307_Web09_DeclarationFilterRequestService
        : RequestServiceBase<TPG_NG_8307_Web09_DeclarationFilterParam, DeclarationFilterRequestParams>
    {
        public override TPG_NG_8307_Web09_DeclarationFilterParam GetRequest(DeclarationFilterRequestParams requestParams)
        {
            var myTPG_NG_8307_Web09_DeclarationFilterParam = new TPG_NG_8307_Web09_DeclarationFilterParam();
            myTPG_NG_8307_Web09_DeclarationFilterParam.Declaration = requestParams.DeclarationNumber;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = @"שאילתא לנתוני תפ""ג עבור הצהרה";

            return myTPG_NG_8307_Web09_DeclarationFilterParam;
        }
    }
}