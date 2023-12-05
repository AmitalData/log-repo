using CWXSD;
using iTextSharp.text;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.XSD.CW_API.ABM;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using static System.Net.WebRequestMethods;

namespace WebFreight.Web.Controllers.WebServices
{
    public class ShaamWebServiceController : ApiController
    {
        string baseUrl = "";

        [HttpGet]
        [Route("linkToCodeForToken")]
        public HttpResponseMessage LinkToCodeForToken()
        {
            try
            {
                int tenant = AuthorizedToken();

                // string link = "https://openapi.taxes.gov.il/shaam/tsandbox/longtimetoken/oauth2/authorize?response_type=code&client_id=6b9a363e74502366b3b15f7fcf941d75&scope=scope&redirect_uri=https://oauth.pstmn.io/v1/callback";
                // return Request.CreateResponse(HttpStatusCode.OK, link);

                HttpClient client = GetShaamApiHttpClient(tenant);
                string url = baseUrl + $"linkToCodeForToken";
                HttpResponseMessage res = client.GetAsync(url).Result;
                string content = res.Content.ReadAsStringAsync().Result;
                client.Dispose();

                if (res.StatusCode != HttpStatusCode.OK)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(new Exception(content)));

                return Request.CreateResponse(HttpStatusCode.OK, content);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("tokens")]
        public HttpResponseMessage Tokens(int pageSize, int page)
        {
            try
            {
                int tenant = AuthorizedToken();

                // string testRes = "{\"tokens\":[{\"Id\":\"df3040cf-c95d-4333-9276-b3569c0a9a5d\",\"Tenant\":0,\"UpdateDate\":null,\"UserCode\":\"dev\",\"RefreshExpierDate\":\"2024-02-26T17:20:34.8364751+02:00\",\"IsActive\":true,\"RefreshToken\":\"AALIv-GIqK7IzuAOAsgM0qwR-cONR_UGJcYnkMu1-he-XV6z4pLn0o7RILKx6A2fERCx_kOLPNi4Cj2e-NfMJoHM9ynLe_kJGa30_63jwDVxHg\",\"AccessExpireDate\":\"2023-11-29T22:06:55.8365884+02:00\",\"AccessToken\":\"AAIgNmI5YTM2M2U3NDUwMjM2NmIzYjE1ZjdmY2Y5NDFkNzWMmN6JiiT6H2TNNCVDc-0Pox43wMfz1SmCCtIPj9t9XO23hHEd4O6L1s7psLxrRMd7OKnv8gAvu3jUTHeyJ413j4kwqA0kMqWpPeM6NlKD-g\",\"CreateDate\":\"2023-11-29T16:05:37.29+00:00\"},{\"Id\":\"165952b3-d58b-4a89-ade5-5922ea3060df\",\"Tenant\":0,\"UpdateDate\":null,\"UserCode\":\"dev\",\"RefreshExpierDate\":\"2024-02-26T17:20:34.616956+02:00\",\"IsActive\":true,\"RefreshToken\":\"AAJFm4LzZLwEcFHPgHMg0KFMdydLoLff97MK-fzRF_xpbjqsl1oRsiUPuwaYjqpV4uL1f6J3z8H5RvDmnQ9zhNaCwQs0ks-JEyzEbCMDFqKjfg\",\"AccessExpireDate\":\"2023-11-29T22:02:50.6170607+02:00\",\"AccessToken\":\"AAIgNmI5YTM2M2U3NDUwMjM2NmIzYjE1ZjdmY2Y5NDFkNzWWMYO4WWx95qJATgDMptWX4r0b2659FScPhQWdZvecVRQQ30M-2lME5Z_1D-RJnl1fq7U-wwyX7eWZdayTtEIxVPmVBkX4gSYoE571jFsrBw\",\"CreateDate\":\"2023-11-29T16:01:32.0566667+00:00\"},{\"Id\":\"4650fa01-0a18-482d-8cea-7d1ef41d466d\",\"Tenant\":0,\"UpdateDate\":\"2023-11-29T16:01:31.5366667+00:00\",\"UserCode\":\"dev\",\"RefreshExpierDate\":\"2024-02-26T18:13:05.7912594+02:00\",\"IsActive\":false,\"RefreshToken\":\"AALlde57JCh2WaJZd42P6Rtfwr-n6T-UJT9r_RNbjCFfvQ9JqaZFk4aSUJMEGL5SuHZ68IzP4aRxMtId_25Qp4ZK9kUcVy0M2HqKb686dKLX3w\",\"AccessExpireDate\":\"2023-11-28T22:13:05.7913794+02:00\",\"AccessToken\":\"AAIgNmI5YTM2M2U3NDUwMjM2NmIzYjE1ZjdmY2Y5NDFkNzUYN_eyVWwWEEuV7UJdCb8ttngIJap4Qc91BY1JEyCWPP48MMvNfIIHW2Hl1anx57PShNVa17UKMzZEViaxwuw0AP-rfvQ7jfF91uVCOkI2uQ\",\"CreateDate\":\"2023-11-28T16:11:48.66+00:00\"},{\"Id\":\"8ee89469-7344-4c49-a485-4d688681ed3b\",\"Tenant\":0,\"UpdateDate\":null,\"UserCode\":\"dev\",\"RefreshExpierDate\":\"2024-02-26T17:20:33.258606+02:00\",\"IsActive\":true,\"RefreshToken\":\"AAIePjHK7kriE17ZaMHWksG4B2t9VWDO7EopE0VPQ4Azx1cIY_GCxUwkUrAfQfDFb5vjfJGrOlZvQa4oB-XLRIcXqYZbVeMB8rCpN7U5Gdln5A\",\"AccessExpireDate\":\"2023-11-28T21:22:11.2587551+02:00\",\"AccessToken\":\"AAIgNmI5YTM2M2U3NDUwMjM2NmIzYjE1ZjdmY2Y5NDFkNzWfFfmVOWgF96fZXjA6NHEixIm8YqhBCvietdJ2d26HnfvORdpmEyfESLkYrWDvHRoFAdd_DcpDG4YTtZt-jfZtgGnYIcJHzdp_yEw_1-xN_g\",\"CreateDate\":\"2023-11-28T15:20:54.2+00:00\"},{\"Id\":\"57e3807c-0665-4569-8ebe-6e7013c9a890\",\"Tenant\":0,\"UpdateDate\":null,\"UserCode\":\"dev\",\"RefreshExpierDate\":\"2024-02-26T17:20:33.3762492+02:00\",\"IsActive\":true,\"RefreshToken\":\"AAIB2APWRhIJYxBoAdY6UO-i300A0ALl0BrK0J3f5R_NZzmefbkyMu-HuN53OfVtDqvSo5VFn7QAo7j4_qSMT0QCVuWxyTsBosUW4lh9jr6pYQ\",\"AccessExpireDate\":\"2023-11-28T21:21:49.3764667+02:00\",\"AccessToken\":\"AAIgNmI5YTM2M2U3NDUwMjM2NmIzYjE1ZjdmY2Y5NDFkNzU8-t0B_ppSYgWmD6nqpc1c4kgpEnt8ekpVznJNiTdKI4n4llRxas9iS6cojPFhj64xObBiz30MeOScwobYryaZVXthwHVKlKSEQ6l_IhAaYg\",\"CreateDate\":\"2023-11-28T15:20:32.3133333+00:00\"},{\"Id\":\"86fc2235-7886-4ed1-952e-2d42917d4cca\",\"Tenant\":0,\"UpdateDate\":null,\"UserCode\":\"dev\",\"RefreshExpierDate\":\"2024-02-26T17:20:32.8598112+02:00\",\"IsActive\":true,\"RefreshToken\":\"AALsWo3whDseAKrwddjApj9dCy31Eajc9iFdmsWe9wLbSQhXeHouRBiWXL-I3Dx6oE5JPOKkWS8clAwNzesHKPAvLM0sPr1i3R4sYh4NAJyw7w\",\"AccessExpireDate\":\"2023-11-28T21:21:26.8599336+02:00\",\"AccessToken\":\"AAIgNmI5YTM2M2U3NDUwMjM2NmIzYjE1ZjdmY2Y5NDFkNzXm0snAxs8Zuba-XRhltxQZQ5XAz77zxWjREVN9G9VoiLOKbk8vMMnCDK0rrBAguOQz77RMwGmXqMdEcb7RVsoBVrt3L3p5jGtcHMziyAnVfw\",\"CreateDate\":\"2023-11-28T15:20:09.79+00:00\"},{\"Id\":\"efb489ec-a35f-4cf8-91c2-31deb7392a88\",\"Tenant\":0,\"UpdateDate\":null,\"UserCode\":\"dev\",\"RefreshExpierDate\":\"2024-02-26T17:20:33.1691107+02:00\",\"IsActive\":true,\"RefreshToken\":\"AAK8dPWQemijwdu4xgIV2FPCBfD3W5xGHMoRr104McUYCndhr9h9CwqyxrqU8oibChYW9hvvHx5n29StTJUicmXidxHZ5LE3RZAlOmzEpakJcQ\",\"AccessExpireDate\":\"2023-11-28T21:20:33.1692288+02:00\",\"AccessToken\":\"AAIgNmI5YTM2M2U3NDUwMjM2NmIzYjE1ZjdmY2Y5NDFkNzUaJ0mGSDYL7dnGdKwIeO5AsGUEuVV1fsuhVqiFrbQL0WJ7mrfZ-fWFbggA6nxydStuTaVkQqjPGjGRPvy8zelUK3_BPMxqlfql5gSWSGgmSg\",\"CreateDate\":\"2023-11-28T15:19:16.1+00:00\"},{\"Id\":\"61af71ac-422f-4eb1-bf8e-fa61d8df8d05\",\"Tenant\":0,\"UpdateDate\":null,\"UserCode\":\"7718388f-b892-476e-b810-50485937c815\",\"RefreshExpierDate\":\"2024-02-25T15:34:04.9220415+02:00\",\"IsActive\":true,\"RefreshToken\":\"AAIYVu0CoGzyYDF5XpfIa7YXqlOrH8l_XQfUxtvNFKTv8CQ1dfDw-IUCs_4bet65z4spnMYw3q8xmbe0UJ0Jpgevg6IuwnokAX_YeUQDf3HujQ\",\"AccessExpireDate\":\"2023-11-27T15:55:15.9221216+02:00\",\"AccessToken\":\"AAIgNmI5YTM2M2U3NDUwMjM2NmIzYjE1ZjdmY2Y5NDFkNzUyEQZ8TP6-L-PzNERbViSoz0zv9ft704GsW74krsViPtrAl5OOh0U3p6rOa2sZfCksF0R8ChBxtRTt-h5TSWxUHyjID7mOeAxapk5BDAfu_g\",\"CreateDate\":\"2023-11-27T13:45:15.1933333+00:00\"},{\"Id\":\"3a71e74c-c2b5-446b-84eb-17e8895bd275\",\"Tenant\":0,\"UpdateDate\":\"2023-11-29T16:01:31.5366667+00:00\",\"UserCode\":\"7718388f-b892-476e-b810-50485937c815\",\"RefreshExpierDate\":\"2024-02-25T15:34:05.0380705+02:00\",\"IsActive\":false,\"RefreshToken\":\"AALkeifDI21en64c_Tz4_fo3HUk4pRGBwX6JMurR3l6SMt1eSHgoPkjbQNrDk8nxD2_kkbLtcv2aDxlRCwMKZqB95IqqL5gc9GktmvYYd6XZag\",\"AccessExpireDate\":\"2023-11-27T15:44:06.0381849+02:00\",\"AccessToken\":\"AAIgNmI5YTM2M2U3NDUwMjM2NmIzYjE1ZjdmY2Y5NDFkNzXBptRMCG7Rp-KXjrzL02IW3dzEvIFbtikM7vJFIBwvsim--KQiswYVYgBhSowmj3uWTkXNtYBysEhd1Jih6KDnfYTBkCFrnYoWXGo1me0LGg\",\"CreateDate\":\"2023-11-27T13:34:05.3066667+00:00\"},{\"Id\":\"bc2b30d2-b67a-4396-a649-2ddc83a475ee\",\"Tenant\":0,\"UpdateDate\":\"2023-11-29T16:01:31.5366667+00:00\",\"UserCode\":\"7718388f-b892-476e-b810-50485937c815\",\"RefreshExpierDate\":\"2024-02-25T13:45:59.5088544+02:00\",\"IsActive\":false,\"RefreshToken\":\"AAJJd9Hnjx2XJTO3Rq3AUTsraH28dv-shpuhPnHQBQnO29wNGsmeU9iUmHEyTHPXV15NinL2rPbXqLgczTfaJZ6BUkb5mg4pw_2qh-8eHPCKcg\",\"AccessExpireDate\":\"2023-11-27T13:58:38.5089793+02:00\",\"AccessToken\":\"AAIgNmI5YTM2M2U3NDUwMjM2NmIzYjE1ZjdmY2Y5NDFkNzWYabckU2YIwrIeGUaFgsqML5Vrf_kJ6cauyb_qrcdqZVZOKd5D7NrIsC1wtK305kfXrjjucJsc8oE_SJ9rJKnAhELyMJnhm17o7xZq-QJyhw\",\"CreateDate\":\"2023-11-27T11:48:37.7866667+00:00\"},{\"Id\":\"7c3528a1-b6d7-473a-b066-fd61e9004fcb\",\"Tenant\":0,\"UpdateDate\":\"2023-11-29T16:01:31.5366667+00:00\",\"UserCode\":\"7718388f-b892-476e-b810-50485937c815\",\"RefreshExpierDate\":\"2024-02-25T13:44:46.4988308+02:00\",\"IsActive\":false,\"RefreshToken\":\"AAJGD66hEEmkcQios-eb8FLcIAJZnlhRSou_5aoxzWuTP3diuEXAA4B1dYwuZF-7HJWhDbVYNH6sUA0FNwqUkCYYGKqoTxaWGVNvqQduGGN5jA\",\"AccessExpireDate\":\"2023-11-27T13:54:47.4989986+02:00\",\"AccessToken\":\"AAIgNmI5YTM2M2U3NDUwMjM2NmIzYjE1ZjdmY2Y5NDFkNzV5aQ-Gmw40X0PlMabdcq_B5jNHftc275r-xlA-ekRj6UeTBylTtmVCYjmSNCjFv2Vl3jp7G1uQKvJ9vFKXKsnGnmibzPVmt6BaTsem_ImcvA\",\"CreateDate\":\"2023-11-27T11:44:46.75+00:00\"},{\"Id\":\"2b3ec8ef-324c-465f-ad1e-4f3268defb5a\",\"Tenant\":0,\"UpdateDate\":\"2023-11-29T16:01:31.5366667+00:00\",\"UserCode\":\"7718388f-b892-476e-b810-50485937c815\",\"RefreshExpierDate\":\"2024-02-25T13:15:01.5842369+02:00\",\"IsActive\":false,\"RefreshToken\":\"AAJEhSpF7qj7fv_eDXdbxsn4I5bix5mW4AUicj1Kdp2pKnsa8wg_l5hDWbs0tGFwotFwnBudLv_OXZbiFzltRyIYR6kwCpkIztsFeFdwlDu6rw\",\"AccessExpireDate\":\"2023-11-27T13:25:02.5843687+02:00\",\"AccessToken\":\"AAIgNmI5YTM2M2U3NDUwMjM2NmIzYjE1ZjdmY2Y5NDFkNzVRLGbhpK6Y1-KtGapwjgpnCHs_KGVa753rRE9mSoT08hZzR1opbvyMheUynR5-iGJJ_OZduntYW_ye4QNWYj85PZjALFAOh-wXH00u493nQw\",\"CreateDate\":\"2023-11-27T11:15:01.8433333+00:00\"},{\"Id\":\"6d3009f2-b70d-46fe-929f-bc69c57e6198\",\"Tenant\":0,\"UpdateDate\":\"2023-11-29T16:01:31.5366667+00:00\",\"UserCode\":\"7718388f-b892-476e-b810-50485937c815\",\"RefreshExpierDate\":\"2024-02-25T12:49:24.5635865+02:00\",\"IsActive\":false,\"RefreshToken\":\"AAJmohLHBmKfF7jHHrxUlB2hPSZBZsBQKPHIz67PvIGBaCaCjGVhGMzb2cJ91Ypkwi4GDsEElejPCz1pn26WMx5D7MbNF-Y3jRw1ENhXQ7pcQg\",\"AccessExpireDate\":\"2023-11-27T12:59:25.5637268+02:00\",\"AccessToken\":\"AAIgNmI5YTM2M2U3NDUwMjM2NmIzYjE1ZjdmY2Y5NDFkNzUXBKIZrOHq2roRJlypZg0s8IUaxgu58ZxAFnn6dWWooKHkL3WMuwuTAWNCS3kwlU8WHnvKpRZ-JLPwEhKpWA-MyD0HmTXbBcROX43DQXuu6Q\",\"CreateDate\":\"2023-11-27T10:49:24.88+00:00\"},{\"Id\":\"0232fddd-f97c-46d4-a4e9-2ccf4112b757\",\"Tenant\":0,\"UpdateDate\":\"2023-11-29T16:01:31.5366667+00:00\",\"UserCode\":\"7718388f-b892-476e-b810-50485937c815\",\"RefreshExpierDate\":\"0001-04-01T00:00:00+00:00\",\"IsActive\":false,\"RefreshToken\":\"AALygjanlfPoLKwLJ6h9K5TCHskuESfxpBXBgt07Qju9PoAWY2vaCjsXEEbmvUaliioRfvKxspg1K12wLmSWjQS41kXDlrp9qwcyIxxUfFZREw\",\"AccessExpireDate\":\"0001-01-01T00:10:01+00:00\",\"AccessToken\":\"AAIgNmI5YTM2M2U3NDUwMjM2NmIzYjE1ZjdmY2Y5NDFkNzW3zvAWbd52Gej-I5IauFuTOIibVTxWr_Xbo3-kY6jB4KYVL_PSGmF0GtsDcV-j24cVyrfUD1vaZCi5x0_KLDUFg_p9EH8V_kWKde3lSQOhzA\",\"CreateDate\":\"2023-11-27T10:29:25.6866667+00:00\"}],\"count\":14}";
                // var json = JsonConvert.DeserializeObject<object>(testRes);
                // return Request.CreateResponse(HttpStatusCode.OK, json);

                HttpClient client = GetShaamApiHttpClient(tenant);
                string url = baseUrl + $"tokens?tenant={tenant}&pageSize={pageSize}&page={page}";
                HttpResponseMessage res = client.GetAsync(url).Result;
                string content = res.Content.ReadAsStringAsync().Result;
                client.Dispose();

                if (res.StatusCode != HttpStatusCode.OK)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(new Exception(content)));

                return Request.CreateResponse(HttpStatusCode.OK, content);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private HttpClient GetShaamApiHttpClient(int tenant)
        {
            string jwt = new SettingQuery().GetJwtToken(tenant);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            return client;
        }

        private int AuthorizedToken()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            return authToken.Tenant;
        }

        class ShaamApiRes
        {
            public int Tenant { get; set; }
        }
    }
}