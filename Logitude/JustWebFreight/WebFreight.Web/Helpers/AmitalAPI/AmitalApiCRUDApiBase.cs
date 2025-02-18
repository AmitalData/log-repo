using Newtonsoft.Json;
using System.Net.Http;

namespace WebFreight.Web.Helpers.AmitalAPI
{
    public class AmitalApiCRUDApiBase<T>
    {
        private readonly string baseUrl;

        public AmitalApiCRUDApiBase(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }
        public virtual T Get(string token, string id)
        {
            HttpClienResponse res = AmitalAPIHelper.SendRequest(token, $"{baseUrl}/{id}", HttpMethod.Get);
            if (!res.Res.IsSuccessStatusCode)
                return default(T);

            T schema = JsonConvert.DeserializeObject<T>(res.Content);
            return schema;
        }

        public virtual T[] GetAll(string token)
        {
            HttpClienResponse res = AmitalAPIHelper.SendRequest(token, baseUrl, HttpMethod.Get);
            T[] schemas = JsonConvert.DeserializeObject<T[]>(res.Content);
            if (!res.Res.IsSuccessStatusCode)
                return null;

            return schemas;
        }

        public virtual T Create(string token, T body, string user)
        {
            HttpClienResponse res = AmitalAPIHelper.SendRequest(token, $"{baseUrl}", HttpMethod.Post, body, user);
            if (!res.Res.IsSuccessStatusCode)
                return default(T);

            T schema = JsonConvert.DeserializeObject<T>(res.Content);
            return schema;
        }

        public virtual HttpClienResponse Delete(string token, string id, string user) =>
            AmitalAPIHelper.SendRequest(token, $"{baseUrl}/{id}", HttpMethod.Delete, null, user);

        public virtual HttpClienResponse Update(string token, string id, T body, string user) =>
            AmitalAPIHelper.SendRequest(token, $"{baseUrl}/{id}", HttpMethod.Put, body, user);
    }
}