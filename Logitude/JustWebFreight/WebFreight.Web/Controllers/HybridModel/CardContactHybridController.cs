using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebFreight.Web.WcfApi;



namespace WebFreight.Web.Controllers.HybridModel
{
    //api/HybridModel
    public class CardContactHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(CardContactPM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            CardContactPM cardContactPM = JsonConvert.DeserializeObject<CardContactPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            CardContactWcfService CardContactWcfService = new CardContactWcfService();
            Response response = CardContactWcfService.Upsert(cardContactPM, batch);
            return response;

        }


        [System.Web.Http.HttpPost]
        public Response Delete([FromBody] object[] t)//(string contactExternalId, string cardCode, int tenant, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string contactExternalId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string cardCode = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);


            CardContactWcfService CardContactWcfService = new CardContactWcfService();
            Response response = CardContactWcfService.Delete(contactExternalId, cardCode, tenant, batch);
            return response;

        }


        [System.Web.Http.HttpPost]
        public CardContactPM GetCardContactPM([FromBody] object[] t)//(string contactExternalId, string cardCode, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string contactExternalId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string cardCode = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);


            CardContactWcfService CardContactWcfService = new CardContactWcfService();
            CardContactPM Response = CardContactWcfService.GetCardContactPM(contactExternalId, cardCode, tenant, ref response);
            return Response;

        }

    }
}

