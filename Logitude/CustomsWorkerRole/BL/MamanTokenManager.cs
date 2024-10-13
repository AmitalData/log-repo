using CustomsWorkerRole;
using Logitude.Customs.BL.Messaging;
using Logitude.Server.Tools.Helpers;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;

public class TokenManager
{
    private static volatile TokenManager instance;
    private static readonly object lockObject = new object();
    private Lazy<AuthToken> lazyToken;
    private readonly CourierWEBAPICommSettings settings;

    // Private constructor to prevent external instantiation
    private TokenManager(CourierWEBAPICommSettings settings)
    {
        this.settings = settings;
        this.lazyToken = new Lazy<AuthToken>(FetchToken, LazyThreadSafetyMode.ExecutionAndPublication);
    }

    // Public static method to get the Singleton instance
    public static TokenManager GetInstance(CourierWEBAPICommSettings settings)
    {
        if (instance == null)
        {
            lock (lockObject)
            {
                if (instance == null)
                {
                    instance = new TokenManager(settings);
                }
            }
        }
        return instance;
    }

    public AuthToken Token
    {
        get
        {
            if (!lazyToken.IsValueCreated)
            {
                RefreshToken();
            }
            return lazyToken.Value;
        }
    }

    private AuthToken FetchToken()
    {
        using (HttpClient client = new HttpClient())
        {
            string myResultString = "";
            try
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteTrace("FetchToken started.");

                client.Timeout = TimeSpan.FromMinutes(MyWebClient.TimeOutFromMinutes); // Ensure this is defined or replace with an actual value
                var agent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
                client.DefaultRequestHeaders.Add("User-Agent", agent);
                StringContent content;

                if (settings.IsNewAPI)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
                    var requestBody = new
                    {
                        Username = settings.username,
                        Password = settings.password
                    };
                    var jsonBody = JsonConvert.SerializeObject(requestBody);
                    content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                }
                else
                {
                    string tokenReq = $"grant_type=password&username={HttpUtility.UrlEncode(settings.username)}&Password={HttpUtility.UrlEncode(settings.password)}";
                    content = new StringContent(tokenReq, Encoding.UTF8, "application/x-www-form-urlencoded");
                }

                LogMessagingUtil.Instance.AppendLine($"PostAsync({settings.URIToken}, {content})");
                var authoTask = client.PostAsync(settings.URIToken, content);
                authoTask.Wait(); // Consider using async/await instead of .Result or .Wait() to avoid blocking threads

                var response = authoTask.Result;
                myResultString = response.Content.ReadAsStringAsync().Result;

                // Log the response status and headers before parsing
                NetCommonHelper.Logger.DevLog.Instance.WriteTrace($"Response Status Code: {response.StatusCode}");
                NetCommonHelper.Logger.DevLog.Instance.WriteTrace($"Response Headers: {response.Headers}");
                NetCommonHelper.Logger.DevLog.Instance.WriteTrace($"Raw Response Content: {myResultString}");

                // Check if the response is XML or HTML instead of JSON
                if (myResultString.Trim().StartsWith("<"))
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteTrace("[WARNING] The response appears to be XML or HTML instead of JSON.");
                    throw new Exception("The token response is not in the expected JSON format.");
                }

                dynamic d = JsonConvert.DeserializeObject(myResultString);
                string access_token, token_type;
                if (settings.IsNewAPI)
                {
                    access_token = d.Token.Value;
                    token_type = "bearer";
                }
                else
                {
                    access_token = d.access_token;
                    token_type = d.token_type;
                }
                NetCommonHelper.Logger.DevLog.Instance.WriteTrace($"New Token Generated: {access_token}");

                return new AuthToken
                {
                    AccessToken = access_token,
                    Token_Type = token_type,
                };
            }
            catch (Exception ex)
            {
                // Enhanced logging with NetCommonHelper
                NetCommonHelper.Logger.DevLog.Instance.WriteTrace($"An error occurred in the authenticate HTTP request: {ex.Message}");
                NetCommonHelper.Logger.DevLog.Instance.WriteTrace($"Exception Details: {ex.ToString()}");
                throw;
            }
        }
    }

    public void RefreshToken()
    {
        NetCommonHelper.Logger.DevLog.Instance.WriteTrace("Entering RefreshToken method.");
        lock (lockObject)
        {
           lazyToken = new Lazy<AuthToken>(FetchToken, LazyThreadSafetyMode.ExecutionAndPublication);
           NetCommonHelper.Logger.DevLog.Instance.WriteTrace("Token has been reset and will be refreshed on the next access.");
        }
    }
}

public class AuthToken
{
    public string AccessToken { get; set; }
    public string Token_Type { get; set; }
    public int ExpiresIn { get; set; }
    public DateTime ExpiryTime { get; set; }
}
