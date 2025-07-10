using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.Validators;
using System.Transactions;
using WebFreight.Web.Security;
using Logitude.Server.Tools;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Accounting.BL.Utils;
using System.Web.Mvc;
using System.Web.Http;
using WebFreight.Web.WcfApi;
using Newtonsoft.Json;

namespace WebFreight.Web.Controllers.HybridModel
{

    public class AccountingPartnerController : Controller
    {

        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t) //AccountingPartnerPM entityPM, bool batch)
        {

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            AccountingPartnerPM accountingPartnerPM = JsonConvert.DeserializeObject<AccountingPartnerPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);

            AccountingPartnerWcfService AccountingPartnerWcfService = new AccountingPartnerWcfService();
            Response response = AccountingPartnerWcfService.Upsert(accountingPartnerPM, batch);
            return response;


        }
    }
}