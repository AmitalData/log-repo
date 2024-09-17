using CustomsWorkerRole;
using Logitude.Customs.BL.Messaging;
using Logitude.Server.Tools.Helpers;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Web;

public class TokenManager
{
    private static TokenManager instance;
    private static readonly object lockObject = new object();
    private Lazy<AuthToken> lazyToken;
    private readonly CourierWEBAPICommSettings settings;

    // Private constructor to prevent external instantiation
    private TokenManager(CourierWEBAPICommSettings settings)
    {
        this.settings = settings;
        this.lazyToken = new Lazy<AuthToken>(FetchToken, true);
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
            if (!lazyToken.IsValueCreated || IsTokenExpired())
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
                client.Timeout = TimeSpan.FromMinutes(MyWebClient.TimeOutFromMinutes); // Assuming this is a defined constant or replace with actual value
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
                authoTask.Wait();  // Consider using async/await instead of .Result or .Wait() to avoid blocking threads
                myResultString = authoTask.Result.Content.ReadAsStringAsync().Result;
                LogMessagingUtil.Instance.AppendLine($"PostAsyncResult ({myResultString})");

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

                return new AuthToken
                {
                    AccessToken = access_token,
                    Token_Type = token_type,
                };
            }
            catch (Exception ex)
            {
                LogMessagingUtil.Instance.AppendLine($"An error occurred in the authenticate HTTP request: {ex.Message}");
                throw;  // Consider a retry mechanism or other error handling
            }
        }
    }


    private bool IsTokenExpired()
    {
        return DateTime.UtcNow >= lazyToken.Value.ExpiryTime;
    }

    public void RefreshToken()
    {
        lazyToken = new Lazy<AuthToken>(FetchToken, true);
    }
}

public class AuthToken
{
    public string AccessToken { get; set; }
    public string Token_Type { get; set; }
    public int ExpiresIn { get; set; }
    public DateTime ExpiryTime { get; set; }
}
